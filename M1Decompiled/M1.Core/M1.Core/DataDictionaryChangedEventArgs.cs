using System;

namespace M1.Core;

public class DataDictionaryChangedEventArgs : EventArgs
{
	public string ChangeType;

	public string DefinitionID;

	public DataDictionaryChangedEventArgs(string changeType, string definitionID)
	{
		ChangeType = changeType;
		DefinitionID = definitionID;
	}
}
