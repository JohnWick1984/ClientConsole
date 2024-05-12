using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Введите IP-адрес сервера: ");
        string ipAddress = Console.ReadLine();

        Console.Write("Введите порт сервера: ");
        int port = int.Parse(Console.ReadLine());

        Console.Write("Введите ваш логин: ");
        string login = Console.ReadLine();

        TcpClient client = new TcpClient(ipAddress, port);
        NetworkStream stream = client.GetStream();

        Console.WriteLine("Вы успешно подключились к серверу. Можете начать отправку сообщений.");

        Thread receiveThread = new Thread(() =>
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine(message);
            }
        });
        receiveThread.Start();

        while (true)
        {
            string message = Console.ReadLine();
            message = $"{login}: {message}"; 
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }
    }
}
