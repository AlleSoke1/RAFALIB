#include <Windows.h>
#include <string>
#include <thread>
#include "networking.h"


#define WAIT(x) Sleep(x);

void InitThread()
{
	WAIT(1000)
	printf("RAFALIB.dll Loading...\n");

	WAIT(60000)
	networking net;
	if (net.Initialize() == false)
	{
		printf("Failed to initialize networking();\n");
	}
	else 
	{
		printf("RAFALIB.dll - Network Listening!\n");
		printf("RAFALIB.dll - CLR Loaded!\n");
		net.Process();
	}

	
}

#ifdef _DLL
BOOL WINAPI DllMain(
	HINSTANCE hinstDLL,  // handle to DLL module
	DWORD fdwReason,     // reason for calling function
	LPVOID lpReserved)  // reserved
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
		{
			if (GetModuleHandleW(L"XVillageServer.exe"))
			{
				std::thread LoadingThread(InitThread);
				LoadingThread.detach();
			}
		}
		break;

		case DLL_THREAD_ATTACH:
			// Do thread-specific initialization.
			break;

		case DLL_THREAD_DETACH:
			// Do thread-specific cleanup.            
			break;

		case DLL_PROCESS_DETACH:
			// Perform any necessary cleanup.
			break;
		}
		return TRUE;
}
//extern "C" {
//	__declspec(dllexport) int Downtown() { return 1; };
//}
#else
int main()
{
	std::thread LoadingThread(InitThread);
	LoadingThread.detach();

	while (1)
	{
		Sleep(1000);
	}
}
#endif