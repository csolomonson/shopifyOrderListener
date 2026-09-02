using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class AutoLogoutSettings
{
	public decimal ALOSundayStartTime;

	public decimal ALOSundayHours;

	public decimal ALOMondayStartTime;

	public decimal ALOMondayHours;

	public decimal ALOTuesdayStartTime;

	public decimal ALOTuesdayHours;

	public decimal ALOWednesdayStartTime;

	public decimal ALOWednesdayHours;

	public decimal ALOThursdayStartTime;

	public decimal ALOThursdayHours;

	public decimal ALOFridayStartTime;

	public decimal ALOFridayHours;

	public decimal ALOSaturdayStartTime;

	public decimal ALOSaturdayHours;

	public AutoLogoutSettings()
	{
	}

	public AutoLogoutSettings(string settings)
	{
		LoadSettings(settings);
	}

	public void LoadSettings(M1DataDictionary dataDictionary, string userID)
	{
		_ = string.Empty;
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select duAutoLogout from DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		DataTable dataTable = dataDictionary.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			LoadSettings(dataTable.Rows[0].Field<string>("duAutoLogout"));
		}
	}

	public void LoadDefaults()
	{
		ALOSundayStartTime = default(decimal);
		ALOSundayHours = default(decimal);
		ALOMondayStartTime = default(decimal);
		ALOMondayHours = default(decimal);
		ALOTuesdayStartTime = default(decimal);
		ALOTuesdayHours = default(decimal);
		ALOWednesdayStartTime = default(decimal);
		ALOWednesdayHours = default(decimal);
		ALOThursdayStartTime = default(decimal);
		ALOThursdayHours = default(decimal);
		ALOFridayStartTime = default(decimal);
		ALOFridayHours = default(decimal);
		ALOSaturdayStartTime = default(decimal);
		ALOSaturdayHours = default(decimal);
	}

	public void LoadSettings(string properties)
	{
		LoadDefaults();
		if (properties == null)
		{
			return;
		}
		string[] array = properties.Split('\r');
		foreach (string text in array)
		{
			int num = text.IndexOf("=");
			if (num > 0)
			{
				string text2 = text.Substring(0, num - 1).Trim().ToUpper();
				string value = text.Substring(num + 1).Trim();
				switch (text2)
				{
				case "ALOSUNDAYSTARTTIME":
					ALOSundayStartTime = convertPropToDecimal(value);
					break;
				case "ALOSUNDAYHOURS":
					ALOSundayHours = convertPropToDecimal(value);
					break;
				case "ALOMONDAYSTARTTIME":
					ALOMondayStartTime = convertPropToDecimal(value);
					break;
				case "ALOMONDAYHOURS":
					ALOMondayHours = convertPropToDecimal(value);
					break;
				case "ALOTUESDAYSTARTTIME":
					ALOTuesdayStartTime = convertPropToDecimal(value);
					break;
				case "ALOTUESDAYHOURS":
					ALOTuesdayHours = convertPropToDecimal(value);
					break;
				case "ALOWEDNESDAYSTARTTIME":
					ALOWednesdayStartTime = convertPropToDecimal(value);
					break;
				case "ALOWEDNESDAYHOURS":
					ALOWednesdayHours = convertPropToDecimal(value);
					break;
				case "ALOTHURSDAYSTARTTIME":
					ALOThursdayStartTime = convertPropToDecimal(value);
					break;
				case "ALOTHURSDAYHOURS":
					ALOThursdayHours = convertPropToDecimal(value);
					break;
				case "ALOFRIDAYSTARTTIME":
					ALOFridayStartTime = convertPropToDecimal(value);
					break;
				case "ALOFRIDAYHOURS":
					ALOFridayHours = convertPropToDecimal(value);
					break;
				case "ALOSATURDAYSTARTTIME":
					ALOSaturdayStartTime = convertPropToDecimal(value);
					break;
				case "ALOSATURDAYHOURS":
					ALOSaturdayHours = convertPropToDecimal(value);
					break;
				}
			}
		}
	}

	public bool IsNowAValidTime()
	{
		bool result = true;
		DateTime now = DateTime.Now;
		decimal timeToCheck = (decimal)(int)now.DayOfWeek * 24m + (decimal)now.Hour + (decimal)((double)now.Minute / 60.0);
		if (IsTimeWithin(timeToCheck, 1, ALOSundayStartTime, ALOSundayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 2, ALOMondayStartTime, ALOMondayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 3, ALOTuesdayStartTime, ALOTuesdayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 4, ALOWednesdayStartTime, ALOWednesdayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 5, ALOThursdayStartTime, ALOThursdayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 6, ALOFridayStartTime, ALOFridayHours))
		{
			result = false;
		}
		else if (IsTimeWithin(timeToCheck, 7, ALOSaturdayStartTime, ALOSaturdayHours))
		{
			result = false;
		}
		return result;
	}

	private bool IsTimeWithin(decimal timeToCheck, int dayOfWeek, decimal startTime, decimal hours)
	{
		bool result = false;
		if (startTime == 0m && hours == 0m)
		{
			result = false;
		}
		else
		{
			int num = Convert.ToInt32(startTime);
			int num2 = Convert.ToInt32((startTime - (decimal)num) * 100m);
			decimal num3 = (dayOfWeek - 1) * 24 + num;
			decimal num4 = num3;
			num3 += (decimal)num2 / 60m;
			num = Convert.ToInt32(hours);
			int num5 = (int)(hours - (decimal)num) * 60;
			num4 += (decimal)num;
			num5 += num2;
			num4 += (decimal)num5 / 60m;
			if (timeToCheck >= num3 && timeToCheck <= num4)
			{
				result = true;
			}
			else if (dayOfWeek == 7 && timeToCheck < 24m)
			{
				timeToCheck += 168m;
				if (timeToCheck >= num3 && timeToCheck <= num4)
				{
					result = true;
				}
			}
		}
		return result;
	}

	private decimal convertPropToDecimal(string value)
	{
		decimal result = default(decimal);
		if (decimal.TryParse(value, out result))
		{
			return result;
		}
		return 0m;
	}

	private string convertDecimalToProp(decimal value)
	{
		return value.ToString("G");
	}

	public bool SaveSettings(DataRow userRow)
	{
		if (ALOSundayStartTime == 0m && ALOSundayHours == 0m && ALOMondayStartTime == 0m && ALOMondayHours == 0m && ALOTuesdayStartTime == 0m && ALOTuesdayHours == 0m && ALOWednesdayStartTime == 0m && ALOWednesdayHours == 0m && ALOThursdayStartTime == 0m && ALOThursdayHours == 0m && ALOFridayStartTime == 0m && ALOFridayHours == 0m && ALOSaturdayStartTime == 0m && ALOSaturdayHours == 0m)
		{
			userRow.SetField<string>("duAutoLogout", null);
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ALOSundayStartTime = " + convertDecimalToProp(ALOSundayStartTime) + "\r");
			stringBuilder.Append("ALOSundayHours = " + convertDecimalToProp(ALOSundayHours) + "\r");
			stringBuilder.Append("ALOMondayStartTime = " + convertDecimalToProp(ALOMondayStartTime) + "\r");
			stringBuilder.Append("ALOMondayHours = " + convertDecimalToProp(ALOMondayHours) + "\r");
			stringBuilder.Append("ALOTuesdayStartTime = " + convertDecimalToProp(ALOTuesdayStartTime) + "\r");
			stringBuilder.Append("ALOTuesdayHours = " + convertDecimalToProp(ALOTuesdayHours) + "\r");
			stringBuilder.Append("ALOWednesdayStartTime = " + convertDecimalToProp(ALOWednesdayStartTime) + "\r");
			stringBuilder.Append("ALOWednesdayHours = " + convertDecimalToProp(ALOWednesdayHours) + "\r");
			stringBuilder.Append("ALOThursdayStartTime = " + convertDecimalToProp(ALOThursdayStartTime) + "\r");
			stringBuilder.Append("ALOThursdayHours = " + convertDecimalToProp(ALOThursdayHours) + "\r");
			stringBuilder.Append("ALOFridayStartTime = " + convertDecimalToProp(ALOFridayStartTime) + "\r");
			stringBuilder.Append("ALOFridayHours = " + convertDecimalToProp(ALOFridayHours) + "\r");
			stringBuilder.Append("ALOSaturdayStartTime = " + convertDecimalToProp(ALOSaturdayStartTime) + "\r");
			stringBuilder.Append("ALOSaturdayHours = " + convertDecimalToProp(ALOSaturdayHours) + "\r");
			userRow.SetField("duAutoLogout", stringBuilder.ToString());
		}
		return true;
	}

	public bool SaveSettings(M1DataDictionary dataDictionary, string userID)
	{
		bool result = false;
		DataSet dataSet = new DataSet();
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("Select * From DDUsers Where duUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
		sqlDataAdapter.Fill(dataSet, "Users");
		if (dataSet.Tables["Users"].Rows.Count != 0)
		{
			SaveSettings(dataSet.Tables["Users"].Rows[0]);
			new SqlCommandBuilder(sqlDataAdapter);
			sqlDataAdapter.Update(dataSet.Tables["Users"].GetChanges());
			result = true;
		}
		return result;
	}
}
