using System;

namespace M1.Core;

public class ShowErrorEventArgs : EventArgs
{
	public string Message;

	public ShowErrorEventArgs(string msg)
	{
		Message = msg;
	}
}
