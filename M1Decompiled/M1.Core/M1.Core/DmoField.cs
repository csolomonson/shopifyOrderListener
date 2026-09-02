namespace M1.Core;

public class DmoField
{
	public string FieldName;

	public string FieldType;

	public byte FieldLength;

	public byte FieldDecimals;

	public string SqlType;

	public bool Nullable;

	public string DefaultValue;

	public DmoField(string name, string type)
		: this(name, type, 0, 0)
	{
	}

	public DmoField(string name, string type, byte length)
		: this(name, type, length, 0)
	{
	}

	public DmoField(string name, string type, byte length, byte decimals)
		: this(name, type, length, decimals, nullable: false)
	{
		Nullable = getNullable();
	}

	public DmoField(string name, string type, byte length, byte decimals, bool nullable)
	{
		FieldName = name;
		FieldLength = length;
		FieldDecimals = decimals;
		FieldType = type.ToLower();
		SqlType = getSqlType();
		Nullable = nullable;
		DefaultValue = getDefaultValue();
	}

	private bool getNullable()
	{
		switch (FieldType)
		{
		case "date":
		case "text":
		case "image":
		case "ntext":
		case "nvarchar(max)":
		case "smalldatetime":
		case "timestamp":
		case "varbinary":
		case "binary":
		case "datetime":
		case "varchar(max)":
			return true;
		default:
			return false;
		}
	}

	private string getSqlType()
	{
		switch (FieldType)
		{
		case "char":
		case "nvarchar":
		case "varchar":
		case "nchar":
			return FieldType + "(" + FieldLength + ")";
		case "date":
			return "datetime";
		case "identity":
			return "int";
		case "numeric":
			return FieldType + "(" + FieldLength + "," + FieldDecimals + ")";
		default:
			return FieldType;
		}
	}

	private string getDefaultValue()
	{
		switch (FieldType)
		{
		case "bit":
		case "int":
		case "money":
		case "float":
		case "numeric":
		case "tinyint":
		case "real":
		case "smallint":
		case "bigint":
		case "smallmoney":
			return "Default 0";
		case "ntext":
		case "nchar":
		case "varchar":
		case "char":
		case "text":
		case "nvarchar":
		case "nvarchar(max)":
		case "varchar(max)":
			return "Default ''";
		case "identity":
			return "Identity(1,1)";
		case "uniqueidentifier":
			return "Default NewId()";
		default:
			return null;
		}
	}
}
