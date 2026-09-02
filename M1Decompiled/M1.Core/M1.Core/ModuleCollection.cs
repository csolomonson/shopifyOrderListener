using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace M1.Core;

public class ModuleCollection : KeyedCollection<string, DDModule>
{
	public ModuleCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(DDModule item)
	{
		return item.ModuleID;
	}

	public virtual void Refresh(M1DataDictionary dataDictionary)
	{
		Clear();
		DataTable dataTable = dataDictionary.GetDataTable("Select * From DDModules Order By ddmCaption,ddmModuleID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			Add(new DDModule(row));
		}
	}

	public string GetModuleText(string module)
	{
		if (string.IsNullOrWhiteSpace(module))
		{
			return string.Empty;
		}
		return base[module].Caption;
	}
}
