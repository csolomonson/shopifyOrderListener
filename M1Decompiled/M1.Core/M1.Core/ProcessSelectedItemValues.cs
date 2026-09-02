using System.Collections.Generic;

namespace M1.Core;

public class ProcessSelectedItemValues
{
	public object[] KeyValues;

	public Dictionary<string, object> EditableValues;

	public Dictionary<string, object> ExtraFieldValues;

	private bool _discardSave;

	public bool DiscardSave
	{
		get
		{
			return _discardSave;
		}
		set
		{
			_discardSave = value;
		}
	}
}
