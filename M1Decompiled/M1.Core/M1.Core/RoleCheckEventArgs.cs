using System;

namespace M1.Core;

public class RoleCheckEventArgs : EventArgs
{
	public string RoleID;

	public bool Cancel;

	public RoleCheckEventArgs(string roleID)
	{
		RoleID = roleID;
	}
}
