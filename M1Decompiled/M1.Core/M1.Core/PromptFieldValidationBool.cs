using System;
using System.Data;

namespace M1.Core;

public class PromptFieldValidationBool : PromptFieldValidation
{
	public PromptFieldValidationBool(string fieldName, bool fieldValue, string message)
		: base(fieldName, fieldValue, message)
	{
		if (fieldValue)
		{
			SearchFilter = fieldName + " <> 0";
		}
		else
		{
			SearchFilter = fieldName + " = 0";
		}
	}

	public override bool IsValid(DataRow row)
	{
		return Convert.ToBoolean(row[FieldName]) != Convert.ToBoolean(FieldValue);
	}
}
