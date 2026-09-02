using System;

namespace M1.Core.Script;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class FormsAxAttribute : Attribute
{
	private readonly string value;

	public string Value => value;

	public FormsAxAttribute(string value)
	{
		this.value = value;
	}
}
