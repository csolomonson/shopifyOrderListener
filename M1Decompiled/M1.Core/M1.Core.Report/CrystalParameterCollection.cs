using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace M1.Core.Report;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
[ComDefaultInterface(typeof(IM1CrystalParameterCollection))]
public class CrystalParameterCollection : KeyedCollection<string, CrystalParameter>, IM1CrystalParameterCollection
{
	private List<CrystalParameter> _Items = new List<CrystalParameter>();

	public CrystalParameterCollection()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override void InsertItem(int index, CrystalParameter item)
	{
		base.InsertItem(index, item);
		_Items.Add(item);
	}

	public CrystalParameter GetItem(int index)
	{
		return _Items[index];
	}

	protected override string GetKeyForItem(CrystalParameter item)
	{
		return item.Name;
	}

	CrystalParameter IM1CrystalParameterCollection.get__Default(string name)
	{
		return base[name];
	}
}
