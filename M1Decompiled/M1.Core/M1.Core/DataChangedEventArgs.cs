using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class DataChangedEventArgs : EventArgs
{
	public DataChangedFlag DataChangedFlag;

	public List<string> ChangedTables = new List<string>();

	public object[] NewKeys;

	public DataChangedEventArgs(DataChangedFlag flag)
	{
		DataChangedFlag = flag;
	}

	public DataChangedEventArgs(string table)
	{
		ChangedTables.Add(table);
	}

	public DataChangedEventArgs(string table, object[] newKeys)
	{
		ChangedTables.Add(table);
		NewKeys = newKeys;
	}
}
