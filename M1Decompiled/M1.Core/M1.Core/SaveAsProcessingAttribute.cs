using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class SaveAsProcessingAttribute : ProcessingAttribute
{
	public SaveAsProcessingAttribute(string table)
		: base(table)
	{
	}
}
