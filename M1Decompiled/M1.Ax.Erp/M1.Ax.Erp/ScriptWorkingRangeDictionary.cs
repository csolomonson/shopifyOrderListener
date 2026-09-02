using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace M1.Ax.Erp;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IScriptWorkingRange))]
public class ScriptWorkingRangeDictionary : Dictionary<string, ScriptWorkingRangeInfo>, IScriptWorkingRange
{
	public ScriptWorkingRangeDictionary()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	public int GetDaysForPlant(string plantID)
	{
		return base[plantID].Days;
	}

	public decimal GetHoursForPlant(string plantID)
	{
		return base[plantID].Hours;
	}
}
