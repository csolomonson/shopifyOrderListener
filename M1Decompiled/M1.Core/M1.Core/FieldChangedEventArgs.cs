using System;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class FieldChangedEventArgs : EventArgs
{
	public string FieldName = string.Empty;

	public FieldChangedEventArgs(string fieldName)
	{
		FieldName = fieldName;
	}
}
