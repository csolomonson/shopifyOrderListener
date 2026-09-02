using System;

namespace M1.Core;

public static class SecurityAccessLevelExtensions
{
	public static string ToSql(this SecurityAccessLevel s)
	{
		return Convert.ToByte(s).ToString();
	}
}
