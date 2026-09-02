using System;
using System.Collections.Generic;
using System.Linq;
using M1.Extensions;

namespace M1.Core;

public class ThirdPartyInfo
{
	public static Dictionary<ThirdParty, ThirdPartyDefinition> Entities = new Dictionary<ThirdParty, ThirdPartyDefinition>
	{
		{
			ThirdParty.EasyOrder,
			new ThirdPartyDefinition
			{
				Field = "ddEasyOrder",
				Properties = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
			}
		},
		{
			ThirdParty.EDI,
			new ThirdPartyDefinition
			{
				Field = "ddEDI",
				Properties = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
			}
		},
		{
			ThirdParty.Mobile,
			new ThirdPartyDefinition
			{
				Field = "ddMobile",
				Properties = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
			}
		}
	};

	public static void Set(ThirdParty module, string property, object value)
	{
		if (Entities[module].Properties.ContainsKey(property))
		{
			Entities[module].Properties[property] = value.ToString();
		}
		else
		{
			Entities[module].Properties.Add(property, value.ToString());
		}
	}

	public static string GetValue(M1DataDictionary DataDictionary, ThirdParty module, string property, string defaultValue = "")
	{
		string queryString = "SELECT " + Entities[module].Field + " FROM DDInfo";
		Dictionary<string, string> dictionary = ConvertStringToProperties((DataDictionary.ExecuteScalar(queryString) ?? string.Empty).ToString());
		if (!dictionary.ContainsKey(property))
		{
			return defaultValue;
		}
		return dictionary[property];
	}

	public static string GetQueryForUpdate()
	{
		string empty = string.Empty;
		List<string> list = new List<string>();
		foreach (KeyValuePair<ThirdParty, ThirdPartyDefinition> entity in Entities)
		{
			string text = string.Join("\n", Entities[entity.Key].Properties.Select((KeyValuePair<string, string> property) => property.Key + " = " + property.Value));
			list.Add(entity.Value.Field + " = '" + text + "'");
		}
		empty = string.Join(", ", list);
		return "UPDATE DDInfo SET " + empty;
	}

	public static void SaveTo(M1DataDictionary DataDictionary)
	{
		string queryForUpdate = GetQueryForUpdate();
		DataDictionary.ExecuteCommand(queryForUpdate);
	}

	public static Dictionary<string, string> ConvertStringToProperties(string properties)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(properties))
		{
			int num = -1;
			string empty = string.Empty;
			string empty2 = string.Empty;
			foreach (string item in M1Util.ParseFieldList(properties, '\n'))
			{
				num = item.IndexOf('=');
				if (num > -1)
				{
					empty = item.Substring(0, num).Trim();
					empty2 = item.Substring(num + 1).Trim();
					if (!dictionary.ContainsKey(empty) && !string.IsNullOrEmpty(empty2))
					{
						dictionary.Add(empty, empty2);
					}
				}
			}
		}
		return dictionary;
	}
}
