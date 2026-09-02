using System;
using System.Runtime.InteropServices;
using M1.Extensions;
using M1.Script.Interfaces;

namespace M1.Core.Script;

[ComVisible(true)]
public class AppConvert : IConvert
{
	public string StringToSql(string textToModify)
	{
		return textToModify.ToSql();
	}

	public string StringToLinq(string textToModify)
	{
		return textToModify.ToLinq();
	}

	public string NumberToSql(object value)
	{
		if (Type.GetTypeCode(value.GetType()) == TypeCode.String)
		{
			return value.ToString();
		}
		if (int.TryParse(value.ToString(), out var result))
		{
			return result.ToString().Trim();
		}
		return value.ToString();
	}

	public string DateTimeToSql(object value)
	{
		if (M1Util.IsNullOrEmpty(value))
		{
			return "NULL";
		}
		return Convert.ToDateTime(value).ToSql();
	}

	public string DateToSql(object value)
	{
		if (M1Util.IsNullOrEmpty(value))
		{
			return "NULL";
		}
		return Convert.ToDateTime(value).ToSql(dateOnly: true);
	}

	public string ToSql(object value, bool bDateAsDateTime = false)
	{
		return M1Util.ConvertToSql(value, !bDateAsDateTime);
	}

	public string ToScript(object value)
	{
		return M1Util.ConvertToScript(value);
	}
}
