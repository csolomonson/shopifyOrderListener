using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDispatch)]
[ComDefaultInterface(typeof(IM1TestComCollection))]
public class M1AdoRecordsetFieldsProxy : IM1TestComCollection
{
	public M1AdoRecordsetProxy Parent;

	private List<M1AdoFieldProxy> usedFields = new List<M1AdoFieldProxy>();

	public int Count => Parent.dataView.Table.Columns.Count;

	[IndexerName("_Default")]
	[DispId(0)]
	public M1AdoFieldProxy this[string name]
	{
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get
		{
			foreach (M1AdoFieldProxy usedField in usedFields)
			{
				if (usedField.Name != null && usedField.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
				{
					return usedField;
				}
			}
			M1AdoFieldProxy m1AdoFieldProxy = new M1AdoFieldProxy(Parent, name);
			usedFields.Add(m1AdoFieldProxy);
			return m1AdoFieldProxy;
		}
	}

	public M1AdoRecordsetFieldsProxy(M1AdoRecordsetProxy parent)
	{
		Parent = parent;
	}

	public int Add(object value)
	{
		return 0;
	}

	public bool Contains(object value)
	{
		return Parent.dataView.Table.Columns.Contains(value.ToString());
	}

	public int IndexOf(object value)
	{
		return Parent.dataView.Table.Columns.IndexOf(value.ToString());
	}

	public void Clear()
	{
		usedFields.Clear();
	}

	public void Remove(object value)
	{
	}

	public void RemoveAt(int index)
	{
	}

	public void Insert(int index, object value)
	{
	}

	[DispId(-4)]
	public IEnumerator GetEnumerator()
	{
		loadAllColumns();
		return usedFields.GetEnumerator();
	}

	private void loadAllColumns()
	{
		if (usedFields.Count == Parent.dataView.Table.Columns.Count)
		{
			return;
		}
		usedFields.Clear();
		foreach (DataColumn column in Parent.dataView.Table.Columns)
		{
			usedFields.Add(new M1AdoFieldProxy(Parent, column.ColumnName));
		}
	}
}
