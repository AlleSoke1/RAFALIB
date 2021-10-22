#include "sendchat.h"
#include <windows.h>

typedef __int64(__fastcall* SendChatFunc)(__int64, char, UINT, const WCHAR*, short, int);
SendChatFunc pSendChat = (SendChatFunc)0x14050E0A0;

const INT64 SendChatPointer = 0x1407560C0;

// __int64 __fastcall sub_14050E0A0(__int64 a1, char a2, int a3, const wchar_t* a4, __int16 a5, int a6)

void SendChat(char cType, UINT nFromAccountDBID, const WCHAR* pwszChatMsg, short wChatLen, int nMapIdx)
{
	pSendChat(*((__int64*)SendChatPointer), cType, nFromAccountDBID, pwszChatMsg, wChatLen, nMapIdx);
}
