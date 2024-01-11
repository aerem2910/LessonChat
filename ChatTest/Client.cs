using System.Net.Sockets;
using System.Net;
using System.Text;

internal class Client
{
    public static void SendMsg(string name)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5050);
        UdpClient udpClient = new UdpClient(0);

        while (true)
        {
            Console.Write("Введите сообщение (или 'Exit' для завершения): ");
            string text = Console.ReadLine();

            if (text.ToLower() == "exit")
                break;

            Message msg = new Message(name, text);
            string responseMsgJs = msg.ToJson();
            byte[] responseData = Encoding.UTF8.GetBytes(responseMsgJs);
            udpClient.Send(responseData, responseData.Length, ep);

            byte[] answerData = udpClient.Receive(ref ep);
            string answerMsgJs = Encoding.UTF8.GetString(answerData);
            Message answerMsg = Message.FromJson(answerMsgJs);
            Console.WriteLine(answerMsg.ToString());
        }
    }
}
