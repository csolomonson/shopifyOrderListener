using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class M1DataTableFieldComProxy
{
	public DataRow Row;

	public string Name = string.Empty;

	[DispId(0)]
	[Browsable(false)]
	public object Value
	{
		get
		{
			if (Row.RowState == DataRowState.Deleted)
			{
				return getOriginalValueForRow(Row);
			}
			return getValueForRow(Row);
		}
		set
		{
			Row[Name] = value;
		}
	}

	private object getValueForRow(DataRow curDataRow)
	{
		Type dataType = curDataRow.Table.Columns[Name].DataType;
		if (dataType == typeof(decimal))
		{
			return (double)curDataRow.Field<decimal>(Name);
		}
		if (dataType == typeof(string))
		{
			return curDataRow.Field<string>(Name);
		}
		if (dataType == typeof(int))
		{
			return curDataRow.Field<int>(Name);
		}
		if (curDataRow.HasVersion(DataRowVersion.Proposed))
		{
			return curDataRow[Name, DataRowVersion.Proposed];
		}
		return curDataRow[Name, DataRowVersion.Current];
	}

	private object getOriginalValueForRow(DataRow curDataRow)
	{
		Type dataType = curDataRow.Table.Columns[Name].DataType;
		if (dataType == typeof(decimal))
		{
			return (double)curDataRow.Field<decimal>(Name, DataRowVersion.Original);
		}
		if (dataType == typeof(string))
		{
			return curDataRow.Field<string>(Name, DataRowVersion.Original);
		}
		if (dataType == typeof(int))
		{
			return curDataRow.Field<int>(Name, DataRowVersion.Original);
		}
		return curDataRow[Name, DataRowVersion.Original];
	}
}
