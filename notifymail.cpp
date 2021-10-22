#include <windows.h>
#include "notifymail.h"

typedef __int64(__fastcall* NotifyMailFunc)(__int64, UINT, INT64, short, short, short, bool);
NotifyMailFunc pNotifyMail = (NotifyMailFunc)0x1405081F0;

const INT64 MasterConnectionPointer = 0x1407560C0;

void NotifyMail(UINT nToAccountDBID, INT64 biToCharacterDBID, short wTotalMailCount, short wNotReadMailCount, short w7DaysLeftCount, bool bNewMail)
{
	pNotifyMail(*((__int64*)MasterConnectionPointer), nToAccountDBID, biToCharacterDBID, wTotalMailCount, wNotReadMailCount, w7DaysLeftCount, bNewMail);
}
