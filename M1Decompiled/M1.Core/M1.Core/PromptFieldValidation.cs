using System.Data;

namespace M1.Core;

public class PromptFieldValidation
{
	public string FieldName = string.Empty;

	public object FieldValue;

	public string Message = string.Empty;

	public string SearchFilter = string.Empty;

	public PromptFieldValidation(string fieldName, object fieldValue, string message)
	{
		FieldName = fieldName;
		FieldValue = fieldValue;
		Message = message;
	}

	public virtual bool IsValid(DataRow row)
	{
		return true;
	}
}
