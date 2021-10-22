#include <Windows.h>
#include "cmdparser.h"
#include "cmdenum.h"
#include <stdio.h>

//Commands
#include "sendnotice.h"
#include "sendchat.h" 
#include "notifymail.h"

cmdparser gCmdParser;
void cmdparser::ParseCommand(int nCommandType, const BYTE* pData, int nDataSize)
{

	switch (nCommandType)
	{
		case PING:
		{	 
			//placeholder.
			printf("CMD(%d) PING\n", nCommandType);
		}
		break;

		case SENDNOTICE:
		{
			wchar_t* pText = (wchar_t*)pData;
			printf("CMD(%d) SendNotice: %ws\n", nCommandType, pText);
			SendNotice(pText);
		}
		break;

		case SENDCHAT:
		{

#pragma pack(push, 1)
			struct VIMAChat			
			{
				UINT nAccountDBID;
				char cType;
				short wChatLen;
				int nMapIdx;
				WCHAR wszChatMsg[512];
			};
#pragma pack(pop)

			VIMAChat* pStruct = (VIMAChat*)pData;
			if (pStruct == NULL)
				return;
			//
			printf("CMD(%d) SendChat Type(%d) ToAccountID(%d) Len(%d) MapID(%d) Message: %ws\n", nCommandType, pStruct->cType, pStruct->nAccountDBID, pStruct->wChatLen, pStruct->nMapIdx, pStruct->wszChatMsg);
			SendChat(pStruct->cType, pStruct->nAccountDBID, pStruct->wszChatMsg, pStruct->wChatLen, pStruct->nMapIdx);
		}
		break;

		case NOTIFYMAIL:
		{
#pragma pack(push, 1)
			struct VIMANotifyMail		// VIMA_NOTIFYMAIL
			{
				UINT nToAccountDBID;	// 받는이 AccountDBID
				INT64 biToCharacterDBID;
				short wTotalMailCount;			// 총 우편수
				short wNotReadMailCount;			// 읽지 않은 메일
				short w7DaysLeftMailCount;			// 만료경고 메일
				bool bNewMail;
			};
#pragma pack(pop)
			VIMANotifyMail* pStruct = (VIMANotifyMail*)pData;
			if (pStruct == NULL)
				return;
			printf("CMD(%d) RefreshNotifyMail AccID(%d) CharID(%lld) TotalMail(%d) wNotReadMailCount(%d) 7DayLeft(%d) IsNewMail?(%d)\n", nCommandType,
				pStruct->nToAccountDBID, pStruct->biToCharacterDBID, pStruct->wTotalMailCount, pStruct->wNotReadMailCount, pStruct->w7DaysLeftMailCount, pStruct->bNewMail);
			NotifyMail(pStruct->nToAccountDBID, pStruct->biToCharacterDBID, pStruct->wTotalMailCount, pStruct->wNotReadMailCount, pStruct->w7DaysLeftMailCount, pStruct->bNewMail);
		}
		break;

		default:
		{
			//placeholder.
			printf("[ERR] CMD(%d) invalid cmd\n", nCommandType);
		}
		break;
	}
}
