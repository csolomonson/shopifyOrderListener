using System;
using System.Data;

namespace M1.Core;

public class ForeignKeyInvalidEventArgs : EventArgs
{
	public FieldDefinition Field;

	public M1Database Database;

	public DataRow Row;

	public ValidationInfo ValidationInfo;

	public bool RetryValidation;

	public bool Cancel;

	public ForeignKeyInvalidEventArgs(DataRow row, FieldDefinition field, M1Database database, ValidationInfo validInfo)
	{
		Row = row;
		Field = field;
		Database = database;
		ValidationInfo = validInfo;
	}
}
