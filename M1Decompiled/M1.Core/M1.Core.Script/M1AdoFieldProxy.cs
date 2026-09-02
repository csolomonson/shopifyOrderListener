using System;
using System.Data;
using System.Runtime.InteropServices;

namespace M1.Core.Script;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class M1AdoFieldProxy
{
	public M1AdoRecordsetProxy Recordset;

	private Type dataType;

	private string _Name = string.Empty;

	[DispId(0)]
	public object Value
	{
		get
		{
			DataRow currentDataRow = Recordset.CurrentDataRow;
			if (currentDataRow == null)
			{
				return null;
			}
			if (currentDataRow[Name] == DBNull.Value)
			{
				return null;
			}
			if (dataType == null)
			{
				dataType = currentDataRow.Table.Columns[Name].DataType;
			}
			if (dataType == typeof(int))
			{
				return currentDataRow.Field<int>(Name);
			}
			if (dataType == typeof(byte))
			{
				return currentDataRow.Field<byte>(Name);
			}
			if (dataType == typeof(short))
			{
				return currentDataRow.Field<short>(Name);
			}
			if (dataType == typeof(string))
			{
				return currentDataRow.Field<string>(Name);
			}
			if (dataType == typeof(decimal))
			{
				return currentDataRow.Field<decimal>(Name);
			}
			if (dataType == typeof(double))
			{
				return currentDataRow.Field<double>(Name);
			}
			if (dataType == typeof(Guid))
			{
				return currentDataRow.Field<Guid>(Name).ToString("B");
			}
			if (currentDataRow[Name] == DBNull.Value)
			{
				return null;
			}
			return currentDataRow[Name];
		}
		set
		{
			if (value == null)
			{
				Recordset.CurrentDataRow[Name] = DBNull.Value;
			}
			else
			{
				Recordset.CurrentDataRow[Name] = value;
			}
		}
	}

	public object OriginalValue
	{
		get
		{
			DataRow currentDataRow = Recordset.CurrentDataRow;
			if (currentDataRow.HasVersion(DataRowVersion.Original))
			{
				if (dataType == null)
				{
					dataType = currentDataRow.Table.Columns[Name].DataType;
				}
				if (dataType == typeof(int))
				{
					return currentDataRow.Field<int>(Name, DataRowVersion.Original);
				}
				if (dataType == typeof(byte))
				{
					return currentDataRow.Field<byte>(Name, DataRowVersion.Original);
				}
				if (dataType == typeof(short))
				{
					return currentDataRow.Field<short>(Name, DataRowVersion.Original);
				}
				if (dataType == typeof(string))
				{
					return currentDataRow.Field<string>(Name, DataRowVersion.Original);
				}
				if (dataType == typeof(decimal))
				{
					return currentDataRow.Field<decimal>(Name, DataRowVersion.Original);
				}
				if (dataType == typeof(double))
				{
					return currentDataRow.Field<double>(Name, DataRowVersion.Original);
				}
				if (currentDataRow[Name, DataRowVersion.Original] == DBNull.Value)
				{
					return null;
				}
				return currentDataRow[Name, DataRowVersion.Original];
			}
			return Value;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			return _Name;
		}
		private set
		{
		}
	}

	public M1AdoFieldProxy(M1AdoRecordsetProxy recordset, string fieldName)
	{
		_Name = fieldName;
		Recordset = recordset;
	}
}
