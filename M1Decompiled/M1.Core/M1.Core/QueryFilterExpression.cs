using System;
using System.Text;

namespace M1.Core;

public class QueryFilterExpression
{
	public bool IsAndClause = true;

	public string FieldName = string.Empty;

	public string Operator = string.Empty;

	public string Operator2 = string.Empty;

	public string FromValue = string.Empty;

	public string ToValue = string.Empty;

	public string GroupTypeIndicator = string.Empty;

	public int NumberOfGroups = 1;

	public QueryFilterExpression(string filterSetting)
	{
		int num = -1;
		filterSetting = filterSetting.TrimStart();
		if (filterSetting.StartsWith("AND ", StringComparison.CurrentCultureIgnoreCase))
		{
			IsAndClause = true;
			filterSetting = filterSetting.Substring(4).TrimStart();
		}
		else if (filterSetting.StartsWith("OR ", StringComparison.CurrentCultureIgnoreCase))
		{
			IsAndClause = false;
			filterSetting = filterSetting.Substring(3).TrimStart();
		}
		else
		{
			IsAndClause = true;
		}
		num = filterSetting.IndexOf(' ');
		if (num > 0)
		{
			parseFieldName(filterSetting.Substring(0, num));
			filterSetting = filterSetting.Substring(num + 1).TrimStart();
		}
		else
		{
			parseFieldName(filterSetting);
			filterSetting = string.Empty;
		}
		num = filterSetting.IndexOf(' ');
		if (num > 0)
		{
			parseOperators(filterSetting.Substring(0, num));
			filterSetting = filterSetting.Substring(num + 1).TrimStart();
		}
		else
		{
			parseOperators(filterSetting);
			filterSetting = string.Empty;
		}
		num = filterSetting.IndexOf('|');
		if (num > 0)
		{
			FromValue = filterSetting.Substring(0, num);
			ToValue = filterSetting.Substring(num + 1);
		}
		else if (num == 0)
		{
			FromValue = string.Empty;
			ToValue = filterSetting.Substring(num + 1);
		}
		else
		{
			FromValue = filterSetting;
			ToValue = string.Empty;
		}
	}

	private void parseFieldName(string fieldString)
	{
		int num = fieldString.IndexOf('|');
		if (num == -1)
		{
			FieldName = fieldString;
			fieldString = string.Empty;
		}
		else
		{
			FieldName = fieldString.Substring(0, num);
			fieldString = fieldString.Substring(num + 1);
		}
		num = fieldString.IndexOf('|');
		if (num == -1)
		{
			GroupTypeIndicator = fieldString;
			fieldString = string.Empty;
		}
		else
		{
			GroupTypeIndicator = fieldString.Substring(0, num);
			fieldString = fieldString.Substring(num + 1);
		}
		string text = fieldString;
		NumberOfGroups = 1;
		if (text.Length != 0)
		{
			int result = 0;
			if (int.TryParse(text, out result))
			{
				NumberOfGroups = result;
			}
		}
	}

	private void parseOperators(string ops)
	{
		int num = ops.IndexOf('|');
		if (num == -1)
		{
			Operator = ops;
			Operator2 = string.Empty;
		}
		else
		{
			Operator = ops.Substring(0, num);
			Operator2 = ops.Substring(num + 1);
		}
	}

	public string getSettingText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (IsAndClause)
		{
			stringBuilder.Append("AND ");
		}
		else
		{
			stringBuilder.Append("OR ");
		}
		stringBuilder.Append(FieldName.Trim() + " ");
		stringBuilder.Append(Operator.Trim() + " ");
		stringBuilder.Append(FromValue.Trim() + " ");
		return stringBuilder.ToString();
	}
}
