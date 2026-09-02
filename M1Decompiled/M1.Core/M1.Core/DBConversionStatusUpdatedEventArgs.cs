using System;

namespace M1.Core;

public class DBConversionStatusUpdatedEventArgs : EventArgs
{
	public string Message;

	public DBConversionStatusUpdatedEventArgs(string message)
	{
		Message = message;
	}
}
