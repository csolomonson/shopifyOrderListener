using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace M1.Core;

public class ProcessValidationCollection : KeyedCollection<string, ProcessValidation>
{
	public ProcessValidationCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(ProcessValidation item)
	{
		return item.MessageID;
	}
}
