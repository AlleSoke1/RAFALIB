#pragma once

class networking
{
public:
	networking();
	~networking();

	bool Initialize();
	bool Clean();
	void Process();

	struct packet
	{
		int nLen;
		int nCommand;
		int nDataLen;
		const unsigned char Data[4096];
	};

protected:
	SOCKET ListenSocket;
	SOCKET ClientSocket;
};