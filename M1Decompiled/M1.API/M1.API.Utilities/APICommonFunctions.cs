using System;
using System.Globalization;
using System.Text;

namespace M1.API.Utilities;

public static class APICommonFunctions
{
	public static DateTime? GetDateConvertedValue(string dateTimeVal)
	{
		DateTime result = DateTime.Today;
		dateTimeVal = dateTimeVal.Replace("/", "").Trim().Replace("-", "")
			.Trim();
		string[] formats = new string[8] { "ddMMyyyy", "ddMyyyy", "dMyyyy", "dMMyyyy", "ddMMyy", "ddMyy", "dMyy", "dMMyy" };
		string[] formats2 = new string[8] { "yyyyMMdd", "yyyyMdd", "yyyyMd", "yyyyMMd", "yyMMdd", "yyMdd", "yyMd", "yyMMd" };
		if (!DateTime.TryParseExact(dateTimeVal, formats2, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result) && !DateTime.TryParseExact(dateTimeVal, formats, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result))
		{
			return null;
		}
		return result;
	}

	public static string GetISO8601FormatString(DateTime dateValue)
	{
		return new DateTimeOffset(dateValue, TimeZoneInfo.Local.GetUtcOffset(dateValue)).ToString("yyyy-MM-ddTHH:mm:sszzz", DateTimeFormatInfo.CurrentInfo);
	}

	public static decimal ParseStringToInvariantDecimal(string value)
	{
		value = (string.IsNullOrEmpty(value) ? "0" : value);
		return decimal.Parse(value, CultureInfo.InvariantCulture);
	}

	public static string ConvertStringToRTF(string input)
	{
		StringBuilder stringBuilder = new StringBuilder(input);
		stringBuilder.Replace("\\", "\\\\");
		stringBuilder.Replace("{", "\\{");
		stringBuilder.Replace("}", "\\}");
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = stringBuilder.ToString();
		foreach (char c in text)
		{
			if (c <= '\u007f')
			{
				stringBuilder2.Append(c);
			}
			else
			{
				stringBuilder2.Append("\\u" + Convert.ToUInt32(c) + "?");
			}
		}
		return stringBuilder2.ToString();
	}
}
