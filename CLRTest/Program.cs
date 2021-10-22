using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CLRTest
{
    class Program
    {
        readonly static string host = "localhost";
        readonly static int port = 17777;
        enum eCommandEnum
        {
            PING,
            SENDMESSAGE,
        };
        static void Main(string[] args)
        {
            string message = "rafa smol dik";
            try
            {
                /*
                    struct packet
                    {
                        int nLen;
                        int nCommand;
                        int nDataLen;
                        const unsigned char Data[4096];
                    };
                */

                message += "\0";

                Byte[] data = System.Text.Encoding.Unicode.GetBytes(message);

                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(host, port);

                MemoryStream dataStream = new MemoryStream();
                BinaryWriter packetStream = new BinaryWriter(dataStream);

                //4+4+4+4096.
                int nPacketLength = 4 + 4 + 4 + message.Length + 11;

                packetStream.Write(nPacketLength);
                packetStream.Write(2);
                packetStream.Write((message.Length + 11)); //UINT+char+short+int

                packetStream.Write(555);
                byte chatType = 1;
                packetStream.Write(chatType);
                short msgsize = (short)message.Length;
                packetStream.Write(msgsize);
                packetStream.Write(-1);

                packetStream.Write(data);

                packetStream.Seek(0, SeekOrigin.Begin);
                byte[] packetData = dataStream.ToArray();

                sock.Send(packetData);

                sock.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
    }
}
