using System.Collections.Generic;
using System.Data;
using System.Text;

namespace M1.Core;

public class MatchingFieldsInfo
{
	public Dictionary<string, string> Fields = new Dictionary<string, string>();

	public string SourceTable = string.Empty;

	public string DestinationTable = string.Empty;

	public void CopyData(DataRow sourceRow, DataRow destinationRow)
	{
		if (Fields == null || Fields.Count == 0)
		{
			return;
		}
		foreach (KeyValuePair<string, string> field in Fields)
		{
			if (field.Key.StartsWith("-"))
			{
				destinationRow[field.Value] = -sourceRow.Field<decimal>(field.Key.Substring(1));
				continue;
			}
			int num = field.Key.IndexOf('=');
			if (num != -1)
			{
				destinationRow[field.Value] = sourceRow[field.Key.Substring(0, num)];
			}
			else
			{
				destinationRow[field.Value] = sourceRow[field.Key];
			}
		}
	}

	public string CheckFieldFormatting(string field)
	{
		if (field.StartsWith("-"))
		{
			return field.Substring(1);
		}
		int num = field.IndexOf('=');
		if (num != -1)
		{
			return field.Substring(0, num);
		}
		return field;
	}

	public MatchingFieldsInfo(string sourceTable, string destinationTable)
	{
		SourceTable = sourceTable;
		DestinationTable = destinationTable;
	}

	public string GetDestinationFieldList(string prefix, string suffix)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Fields != null && Fields.Count != 0)
		{
			foreach (KeyValuePair<string, string> field in Fields)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(field.Value);
			}
			stringBuilder.Insert(0, prefix);
			stringBuilder.Append(suffix);
		}
		return stringBuilder.ToString();
	}

	public string GetSourceFieldList(string prefix, string suffix)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Fields != null && Fields.Count != 0)
		{
			foreach (KeyValuePair<string, string> field in Fields)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				if (field.Key.StartsWith("-"))
				{
					stringBuilder.Append(field.Key.Substring(1));
				}
				else
				{
					stringBuilder.Append(field.Key);
				}
			}
			stringBuilder.Insert(0, prefix);
			stringBuilder.Append(suffix);
		}
		return stringBuilder.ToString();
	}
}
