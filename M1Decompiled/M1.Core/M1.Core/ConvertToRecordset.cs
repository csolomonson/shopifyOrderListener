using System;
using System.Data;
using System.Reflection;
using ADODB;

namespace M1.Core;

public class ConvertToRecordset
{
	public static Recordset ConvertDataTableToRecordset(DataTable dataTable)
	{
		Recordset recordset = new RecordsetClass();
		recordset.CursorLocation = CursorLocationEnum.adUseClient;
		Fields fields = recordset.Fields;
		DataColumnCollection columns = dataTable.Columns;
		foreach (DataColumn item in columns)
		{
			fields.Append(item.ColumnName, TranslateType(item.DataType), item.MaxLength, item.AllowDBNull ? FieldAttributeEnum.adFldIsNullable : FieldAttributeEnum.adFldUnspecified);
		}
		recordset.Open(Missing.Value, Missing.Value, CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockBatchOptimistic, 0);
		foreach (DataRow row in dataTable.Rows)
		{
			recordset.AddNew(Missing.Value, Missing.Value);
			for (int i = 0; i < columns.Count; i++)
			{
				fields[i].Value = row[i];
			}
		}
		return recordset;
	}

	private static DataTypeEnum TranslateType(Type columnType)
	{
		switch (columnType.UnderlyingSystemType.ToString())
		{
		case "System.Boolean":
			return DataTypeEnum.adBoolean;
		case "System.Byte":
		case "M1.Core.SecurityAccessLevel":
			return DataTypeEnum.adUnsignedTinyInt;
		case "System.Char":
			return DataTypeEnum.adChar;
		case "System.DateTime":
			return DataTypeEnum.adDate;
		case "System.Decimal":
			return DataTypeEnum.adCurrency;
		case "System.Double":
			return DataTypeEnum.adDouble;
		case "System.Int16":
			return DataTypeEnum.adSmallInt;
		case "System.Int32":
			return DataTypeEnum.adInteger;
		case "System.Int64":
			return DataTypeEnum.adBigInt;
		case "System.SByte":
			return DataTypeEnum.adTinyInt;
		case "System.Single":
			return DataTypeEnum.adSingle;
		case "System.UInt16":
			return DataTypeEnum.adUnsignedSmallInt;
		case "System.UInt32":
			return DataTypeEnum.adUnsignedInt;
		case "System.UInt64":
			return DataTypeEnum.adUnsignedBigInt;
		default:
			return DataTypeEnum.adVarChar;
		}
	}
}
