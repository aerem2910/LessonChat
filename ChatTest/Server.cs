using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Server
{
    private static bool exitRequested = false;

    public static void AcceptMsg()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
        UdpClient udpClient = new UdpClient(5050);
        Console.WriteLine("Сервер ожидает сообщения. Для завершения нажмите клавишу...");

        // Запустим поток для ожидания нажатия клавиши
        Thread exitThread = new Thread(() =>
        {
            Console.ReadKey();
            exitRequested = true;
        });
        exitThread.Start();

        while (!exitRequested)
        {
            byte[] buffer = udpClient.Receive(ref ep);
            string data = Encoding.UTF8.GetString(buffer);

            Thread tr = new Thread(() =>
            {
                Message msg = Message.FromJson(data);
                Console.WriteLine(msg.ToString());
                Message responseMsg = new Message("Server", "Message accept on serv!");
                string responseMsgJs = responseMsg.ToJson();
                byte[] responseDate = Encoding.UTF8.GetBytes(responseMsgJs);
                udpClient.Send(responseDate, responseDate.Length, ep);
            });
            tr.Start();
        }
    }
}

