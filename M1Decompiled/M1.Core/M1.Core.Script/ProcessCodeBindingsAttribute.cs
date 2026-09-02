using System;

namespace M1.Core.Script;

[AttributeUsage(AttributeTargets.Event, Inherited = false, AllowMultiple = false)]
public class ProcessCodeBindingsAttribute : Attribute
{
	private readonly bool value;

	public bool Value => value;

	public ProcessCodeBindingsAttribute(bool value)
	{
		this.value = value;
	}
}
