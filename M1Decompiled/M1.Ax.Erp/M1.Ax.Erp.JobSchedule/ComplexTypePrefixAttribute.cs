using System;

namespace M1.Ax.Erp.JobSchedule;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
internal sealed class ComplexTypePrefixAttribute : Attribute
{
	private readonly string prefix;

	public string Prefix => prefix;

	public ComplexTypePrefixAttribute(string prefix)
	{
		this.prefix = prefix;
	}
}
