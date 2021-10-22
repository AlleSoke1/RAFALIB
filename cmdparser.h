#pragma once
class cmdparser
{
public:
	void ParseCommand(int nCommandType, const BYTE* pData, int nDataSize);
};

extern cmdparser gCmdParser;