using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
[ComDefaultInterface(typeof(IM1ComCollection))]
public class M1AdoFields : KeyedCollection<string, FieldDefinition>, IM1ComCollection
{
	public FieldCollection Fields;

	[IndexerName("_Default")]
	[DispId(0)]
	public new object this[string name] => Fields[name];

	public M1AdoFields()
		: base((IEqualityComparer<string>)StringComparer.CurrentCultureIgnoreCase)
	{
	}

	protected override string GetKeyForItem(FieldDefinition item)
	{
		return item.FieldName;
	}

	public int Add(object value)
	{
		Fields.Add((FieldDefinition)value);
		return 0;
	}

	public bool Contains(object value)
	{
		return Fields.Contains((FieldDefinition)value);
	}

	public int IndexOf(object value)
	{
		return Fields.IndexOf((FieldDefinition)value);
	}

	public void Remove(object value)
	{
		Fields.Remove((FieldDefinition)value);
	}

	public void Insert(int index, object value)
	{
		Fields.Insert(index, (FieldDefinition)value);
	}

	[DispId(-4)]
	public new IEnumerator GetEnumerator()
	{
		return Fields.GetEnumerator();
	}

	public void LoadCollection(object controlCollection)
	{
		throw new NotImplementedException();
	}
}
