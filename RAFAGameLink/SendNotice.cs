using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net.Sockets;
using System.IO;

public partial class StoredProcedures
{
    readonly static string host = "localhost";
    readonly static int port = 17777;
    enum eCommandEnum
      {
        PING,
        SENDMESSAGE,
        SENDCHAT,
        NOTIFYMAIL,
    };

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SendNotice(SqlString message)
    {
        try
        {
            string messageToSend = message.Value;
            messageToSend += "\0";

            Byte[] data = System.Text.Encoding.Unicode.GetBytes(messageToSend);

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(host, port);
            if (sock.Connected)
            {
                MemoryStream dataStream = new MemoryStream();
                BinaryWriter packetStream = new BinaryWriter(dataStream);

                //4+4+4+4096.
                int nPacketLength = 4 + 4 + 4 + messageToSend.Length;
                packetStream.Write(nPacketLength);
                packetStream.Write((int)eCommandEnum.SENDMESSAGE);
                packetStream.Write(messageToSend.Length);
                packetStream.Write(data);

                packetStream.Seek(0, SeekOrigin.Begin);
                byte[] packetData = dataStream.ToArray();

                sock.Send(packetData);

                sock.Close();
            }
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
