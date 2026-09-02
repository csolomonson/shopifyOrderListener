using System;

namespace M1.Core;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ImportProcessingAttribute : ProcessingAttribute
{
	public ImportProcessingAttribute(string table)
		: base(table)
	{
	}
}
