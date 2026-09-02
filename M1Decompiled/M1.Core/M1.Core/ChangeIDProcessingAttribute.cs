using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ChangeIDProcessingAttribute : ProcessingAttribute
{
	public ChangeIDProcessingAttribute(string table)
		: base(table)
	{
	}
}
