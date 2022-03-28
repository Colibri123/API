using System.Net.Sockets;


namespace Client 
{
    internal class Client
    {
        static TcpClient TcpClient;
        static TcpClient Connect(String server)
        {
            Int32 port = 7000;
            TcpClient client = new TcpClient(server, port);
            TcpClient = client;
            return client;
        }
        static void Messege(TcpClient tcpClient, string message) 
        {

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            NetworkStream stream = tcpClient.GetStream();
            stream.Write(data, 0, data.Length);
            data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine(responseData);
        }

        static void Main(string[] args)
        {
            
            if (TcpClient == null)
            {
                Connect("127.0.0.1");
            }
            if (TcpClient != null)
            {
                while (true)
                {
                    Messege(TcpClient, Console.ReadLine());
                }
            }
            
            
        }

    }
}
