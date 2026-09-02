using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace M1.Core;

[ComVisible(true)]
public class ValidEventArgs : DbAndRowEventArgs
{
	public ValidationInfo ValidationInfo;

	public ValidEventArgs(ValidationInfo validationInfo, M1Database database, DataRow row, SqlTransaction transaction)
		: base(database, row, transaction)
	{
		ValidationInfo = validationInfo;
	}

	public void AddError(string errorText, bool setAsModified = false)
	{
		ValidationInfo.AddError(errorText, setAsModified);
	}

	public void AddWarning(string errorText)
	{
		ValidationInfo.AddWarning(errorText);
	}

	public void AddWarnings(string errorText, string separator)
	{
		string[] array = errorText.Split(new string[1] { separator }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string errorText2 in array)
		{
			AddWarning(errorText2);
		}
	}

	public void AddMessage(string errorText)
	{
		ValidationInfo.AddMessage(errorText);
	}
}
