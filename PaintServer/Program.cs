using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PaintServer
{
    class Program
    {
        // IP là localhost, Port 8080
        private const string IP_ADDRESS = "127.0.0.1";
        private const int PORT = 8080;

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // 1. Khoi tao Server
                IPAddress localAddr = IPAddress.Parse(IP_ADDRESS);
                server = new TcpListener(localAddr, PORT);
                server.Start();

                Console.WriteLine($"[Server] Dang chay tai {IP_ADDRESS}:{PORT}");
                Console.WriteLine("[Server] Dang cho Client ket noi...");

                while (true)
                {
                    // 2. Chap nhan ket noi (Cho o day den khi co nguoi vao)
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine(">> Co nguoi ket noi!");

                    // 3. Lay luong gui/nhan
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Vong lap doc tin nhan lien tuc
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        // Doi byte sang chu
                        string text = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Nhan duoc: {text}");

                        // Echo: Gui lai dung noi dung do
                        byte[] response = Encoding.UTF8.GetBytes($"Server tra loi: {text}");
                        stream.Write(response, 0, response.Length);
                    }

                    client.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Loi: " + e.Message);
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}