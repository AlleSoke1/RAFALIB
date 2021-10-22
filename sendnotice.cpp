#include "sendnotice.h"
#include <windows.h>

__int64 __fastcall sub_14050DBD0(__int64 a1, const wchar_t* a2, int a3);

/*
0000000140443670 | 44:8B85 B0050000 | mov r8d, dword ptr ss : [rbp + 5B0] |
0000000140443677 | 48 : 8D95 B0010000 | lea rdx, qword ptr ss : [rbp + 1B0] |
000000014044367E | 48 : 8B0D 3B2A3100 | mov rcx, qword ptr ds : [1407560C0] |
0000000140443685 | E8 46A50C00 | call xvillageserver.14050DBD0 |
*/

//__int64(__fastcall* pSendNotice)(__int64 a1, const wchar_t* a2, int a3) = (__int64(__fastcall*)(__int64, const wchar_t*, int))0x14050DBD0;

typedef __int64 (__fastcall *SendNoticeFunc)(__int64, const wchar_t*, int);
SendNoticeFunc pSendNotice = (SendNoticeFunc)0x14050DBD0;

const INT64 SendNoticePointer = 0x1407560C0;

void SendNotice(const wchar_t* pNotice)
{
	pSendNotice(*((__int64*)SendNoticePointer), pNotice, wcslen(pNotice));
}