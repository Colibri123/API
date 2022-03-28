using APISERVER;
using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener tcpListener = new TcpListener(IPAddress.Any, 7000);
TcpClient client = null;
PAFOAR pAFOAR = new PAFOAR();
try
{
    
    
    while (true)
    {
        if (client == null)
        {
            Console.WriteLine("server started");
            tcpListener.Start();
            Console.WriteLine("Ожидание подключений... ");
            client = tcpListener.AcceptTcpClient();
            Console.WriteLine("Подключен клиент. Выполнение запроса...");
            Console.WriteLine("Клиент, подключенный к " + IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) + " по номеру порта " + ((IPEndPoint)client.Client.RemoteEndPoint).Port.ToString());

        }
        if (client != null)
        {
            NetworkStream stream = client.GetStream();
            byte[] data = new byte[256];
            string req = Encoding.ASCII.GetString(data, 0, stream.Read(data, 0, data.Length));
            
            Byte[] data_ = System.Text.Encoding.ASCII.GetBytes(pAFOAR.DataProcessing(req));
            stream.Write(data_, 0, data_.Length);
            data_ = new Byte[256];
            String responseData = String.Empty;
            
        }       
        
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally 
{
    if (tcpListener != null)
    {
        Console.WriteLine("server down");
        tcpListener.Stop();
    }
   
}

