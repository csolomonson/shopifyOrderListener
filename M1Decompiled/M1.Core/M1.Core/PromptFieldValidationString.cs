using System;
using System.Data;
using M1.Extensions;

namespace M1.Core;

public class PromptFieldValidationString : PromptFieldValidation
{
	public PromptFieldValidationString(string fieldName, string fieldValue, string message)
		: base(fieldName, fieldValue, message)
	{
		SearchFilter = fieldName + " = " + fieldValue.ToSql();
	}

	public override bool IsValid(DataRow row)
	{
		return Convert.ToString(row[FieldName]) != Convert.ToString(FieldValue);
	}
}
