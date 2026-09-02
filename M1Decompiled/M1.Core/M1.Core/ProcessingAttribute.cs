using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ProcessingAttribute : Attribute
{
	private readonly string _Table;

	public string Table => _Table;

	public ProcessingAttribute(string table)
	{
		_Table = table;
	}
}
