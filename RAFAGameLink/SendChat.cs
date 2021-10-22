using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net.Sockets;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{

   
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SendChat (SqlInt32 nAccountDBID, SqlInt32 ChatType, SqlInt32 nMapID, SqlString message)
    {
        try
        {
            /*		struct VIMAChat			
                    {
                        UINT nAccountDBID;
                        char cType;
                        short wChatLen;
                        int nMapIdx;
                        WCHAR wszChatMsg[512];
                    };
                    */
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
                int nPacketLength = 4 + 4 + 4 + messageToSend.Length + 11;

                packetStream.Write(nPacketLength);
                packetStream.Write((int)eCommandEnum.SENDCHAT);
                packetStream.Write((messageToSend.Length + 11) ); //UINT+char+short+int

                packetStream.Write(nAccountDBID.Value);
                packetStream.Write((byte)ChatType.Value);
                packetStream.Write((short)messageToSend.Length);
                packetStream.Write(nMapID.Value);

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
