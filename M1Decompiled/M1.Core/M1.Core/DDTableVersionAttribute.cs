using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
public sealed class DDTableVersionAttribute : Attribute
{
	private readonly string _Version;

	private readonly string _Table;

	public string Version => _Version;

	public string Table => _Table;

	public DDTableVersionAttribute(string table, string version)
	{
		_Version = version;
		_Table = table;
	}
}
