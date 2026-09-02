using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M1.Core;

public class ReferencedFieldsList : List<string>
{
	public Dictionary<string, Dictionary<string, string>> SubFieldReferences;

	public ReferencedFieldsList()
	{
	}

	public ReferencedFieldsList(string codeToCheck)
	{
		ParseCodeForFields(codeToCheck);
	}

	public new void Clear()
	{
		base.Clear();
		if (SubFieldReferences != null)
		{
			SubFieldReferences.Clear();
			SubFieldReferences = null;
		}
	}

	public void ParseCodeForFields(string codeToCheck)
	{
		ParseCodeForFields(codeToCheck, string.Empty);
	}

	public void ParseCodeForFields(string codeToCheck, string fieldsPrefix)
	{
		string text = fieldsPrefix + "Fields(\"";
		if (string.IsNullOrEmpty(codeToCheck))
		{
			return;
		}
		int length = text.Length;
		int num = codeToCheck.IndexOf(text, StringComparison.OrdinalIgnoreCase);
		while (num != -1)
		{
			num += length;
			int num2 = codeToCheck.IndexOf('"', num);
			if (num2 == -1)
			{
				continue;
			}
			string text2 = codeToCheck.Substring(num, num2 - num);
			if (num == length || codeToCheck[num - (length + 1)] != '.')
			{
				if (!this.Contains(text2, StringComparer.CurrentCultureIgnoreCase))
				{
					Add(text2);
				}
				num = num2;
				num = ParseCodeForSubFields(codeToCheck, num, text2, string.Empty);
			}
			num = codeToCheck.IndexOf(text, num, StringComparison.OrdinalIgnoreCase);
		}
	}

	protected int ParseCodeForSubFields(string codeToCheck, int startPos, string curTableLinkField, string parentField)
	{
		if (codeToCheck.Length >= startPos + 11 && codeToCheck.Substring(startPos, 11).Equals("\").Fields(\"", StringComparison.CurrentCultureIgnoreCase))
		{
			startPos += 11;
			int num = codeToCheck.IndexOf('"', startPos);
			if (num != -1)
			{
				string text = codeToCheck.Substring(startPos, num - startPos);
				if (SubFieldReferences == null)
				{
					SubFieldReferences = new Dictionary<string, Dictionary<string, string>>(StringComparer.CurrentCultureIgnoreCase);
				}
				if (!SubFieldReferences.ContainsKey(curTableLinkField))
				{
					SubFieldReferences.Add(curTableLinkField, new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase));
				}
				Dictionary<string, string> dictionary = SubFieldReferences[curTableLinkField];
				if (!dictionary.ContainsKey(text))
				{
					dictionary.Add(text, parentField);
				}
				startPos = num;
				startPos = ParseCodeForSubFields(codeToCheck, startPos, curTableLinkField, text);
			}
		}
		return startPos;
	}

	public void ParseCodeForRelatedDataFields(string codeToCheck)
	{
		if (string.IsNullOrEmpty(codeToCheck))
		{
			return;
		}
		int num = codeToCheck.IndexOf("RelatedTableGetAdoRecord(\"", StringComparison.CurrentCultureIgnoreCase);
		int num2 = 0;
		string empty = string.Empty;
		while (num != -1)
		{
			num2 = codeToCheck.IndexOf("\"", num + 26);
			if (num2 != -1)
			{
				empty = codeToCheck.Substring(num + 26, num2 - (num + 26));
				AddFieldList(empty);
			}
			num = codeToCheck.IndexOf("RelatedTableGetAdoRecord(\"", num + 26, StringComparison.CurrentCultureIgnoreCase);
		}
	}

	public void AddRange(ReferencedFieldsList collection)
	{
		foreach (string item in collection)
		{
			Add(item);
		}
	}

	public new void AddRange(IEnumerable<string> collection)
	{
		foreach (string item in collection)
		{
			Add(item);
		}
	}

	public new void Add(string field)
	{
		if (field.Length != 0 && !field.Equals("''") && !field.Equals("0") && !this.Contains(field, StringComparer.CurrentCultureIgnoreCase))
		{
			base.Add(field);
		}
	}

	public void AddFieldList(string fieldList)
	{
		if (fieldList != null && fieldList.Length != 0)
		{
			string[] array = fieldList.Split(',');
			foreach (string field in array)
			{
				Add(field);
			}
		}
	}

	public string FieldList()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(current);
			}
		}
		return stringBuilder.ToString();
	}
}
