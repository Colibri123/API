using System.Net;
using System.Net.Sockets;
using System.Text;

namespace APISERVER
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        static void Main()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("192.168.0.15"), port);
            DataProcessing pAFOAR = new DataProcessing();

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(0);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    string message = pAFOAR.request(builder.ToString());
                    // отправляем ответ
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();                    
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally 
            {
                listenSocket.Close();
                Main();

            }
        }
    }
}
