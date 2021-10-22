using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net.Sockets;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    /*      struct VIMANotifyMail		// VIMA_NOTIFYMAIL
			{
				UINT nToAccountDBID;	// 받는이 AccountDBID
				INT64 biToCharacterDBID;
				short wTotalMailCount;			// 총 우편수
				short wNotReadMailCount;			// 읽지 않은 메일
				short w7DaysLeftMailCount;			// 만료경고 메일
				bool bNewMail;
			};
    */
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void NotifyMail(SqlInt32 nAccountID, SqlInt64 nCharacterID, SqlInt16 wTotalMailCount, SqlInt16 wNotReadMailCount, SqlInt16 w7DaysLeftMailCount, SqlBoolean bNewMail)
    {
        try
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(host, port);
            if (sock.Connected)
            {
                MemoryStream dataStream = new MemoryStream();
                BinaryWriter packetStream = new BinaryWriter(dataStream);

                //4+4+4+4096.
                int nPacketLength = 4 + 4 + 4 + 19;
                packetStream.Write(nPacketLength);
                packetStream.Write((int)eCommandEnum.NOTIFYMAIL);
                packetStream.Write(19);
                //
                packetStream.Write(nAccountID.Value);
                packetStream.Write(nCharacterID.Value);
                packetStream.Write(wTotalMailCount.Value);
                packetStream.Write(wNotReadMailCount.Value);
                packetStream.Write(w7DaysLeftMailCount.Value);
                packetStream.Write(bNewMail.Value);

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