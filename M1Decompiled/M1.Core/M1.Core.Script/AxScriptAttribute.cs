using System;

namespace M1.Core.Script;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class AxScriptAttribute : Attribute
{
	private readonly string value;

	public string Value => value;

	public AxScriptAttribute(string value)
	{
		this.value = value;
	}
}
