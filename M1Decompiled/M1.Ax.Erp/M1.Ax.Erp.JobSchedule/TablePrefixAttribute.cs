using System;

namespace M1.Ax.Erp.JobSchedule;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
internal sealed class TablePrefixAttribute : Attribute
{
	private readonly string prefix;

	public string Prefix => prefix;

	public TablePrefixAttribute(string prefix)
	{
		this.prefix = prefix;
	}
}
