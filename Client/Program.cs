using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type your port :");
            string port = Console.ReadLine();
            int formatted_port;
            try
            {
                formatted_port = Convert.ToInt32(port);
                Console.WriteLine("provide ip address like: 127.0.0.1 - and press enter");
                string ipAddress = Console.ReadLine();
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string bufSize = clientSocket.SendBufferSize.ToString();
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddress), formatted_port);
                clientSocket.Connect(ep);
                Console.WriteLine("Hello , Client !");
                while (true)
                {
                    string msgFromClient = null;
                    do
                    {
                        Console.WriteLine("Type your number or type >>> exit");
                        msgFromClient = Console.ReadLine();
                        if (msgFromClient == "exit") { clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(msgFromClient), 0, msgFromClient.Length, SocketFlags.None); endSession(clientSocket); }
                        else
                        {
                            clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(msgFromClient), 0, msgFromClient.Length, SocketFlags.None);
                            byte[] msgFromServer = new byte[1024];
                            int size = clientSocket.Receive(msgFromServer);
                            Console.WriteLine("Server: " + System.Text.Encoding.UTF8.GetString(msgFromServer, 0, size));
                        }
                    } while (true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void endSession(Socket skt)
        {
            try
            {
                skt.SafeHandle.Close();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
