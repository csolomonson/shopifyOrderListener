using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IM1ComCollection))]
public class M1DataTableFieldsComProxy : KeyedCollection<string, M1DataTableFieldComProxy>, IM1ComCollection
{
	public DataColumnCollection Columns;

	public DataRow Row;

	private M1DataTableFieldComProxy fieldProxy = new M1DataTableFieldComProxy();

	public new object this[string name]
	{
		get
		{
			fieldProxy.Name = name;
			fieldProxy.Row = Row;
			return fieldProxy;
		}
	}

	public int EditMode => 1;

	public string EntryMode => "FORM";

	public M1DataTableFieldsComProxy()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(M1DataTableFieldComProxy item)
	{
		return item.Name;
	}

	public int Add(object value)
	{
		return 0;
	}

	public bool Contains(object value)
	{
		return Columns.Contains(value.ToString());
	}

	public int IndexOf(object value)
	{
		return Columns.IndexOf((DataColumn)value);
	}

	public void Remove(object value)
	{
	}

	public void Insert(int index, object value)
	{
	}

	[DispId(-4)]
	public new IEnumerator GetEnumerator()
	{
		return Columns.GetEnumerator();
	}

	public void LoadCollection(object controlCollection)
	{
		throw new NotImplementedException();
	}

	object IM1ComCollection.get__Default(string name)
	{
		return this[name];
	}
}
