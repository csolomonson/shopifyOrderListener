using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class M1GridExportSettings
{
	public string ExportFilePath = string.Empty;

	public bool ExportFieldHeadings;

	public bool ExportFieldCaptions;

	public bool ExportSelectedRowsOnly;

	public int ExportFormat = 2;

	public string ExportColumns = string.Empty;

	public string ExportColumnCaptions = string.Empty;

	public string GetGridProperties(M1DataDictionary dataDictionary, string gridID, string userID)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("SELECT dgExportProperties FROM DDGridDetails WHERE dgGridID=@Grid AND dgUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@Grid", SqlDbType.NVarChar)).Value = gridID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		return Convert.ToString(dataDictionary.ExecuteScalar(sqlCommand));
	}

	public void LoadDefaults()
	{
		ExportFilePath = string.Empty;
		ExportFieldHeadings = false;
		ExportFieldCaptions = false;
		ExportSelectedRowsOnly = false;
		ExportFormat = 2;
		ExportColumns = string.Empty;
		ExportColumnCaptions = string.Empty;
	}

	public void LoadSettings(string properties)
	{
		LoadDefaults();
		if (properties.Length == 0)
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
				case "FILEPATH":
					ExportFilePath = convertPropToString(value);
					break;
				case "EXPORTFIELDHEADINGS":
					ExportFieldHeadings = convertPropToBool(value);
					break;
				case "EXPORTFIELDCAPTIONS":
					ExportFieldCaptions = convertPropToBool(value);
					break;
				case "SELECTEDROWSONLY":
					ExportSelectedRowsOnly = convertPropToBool(value);
					break;
				case "EXPORTFORMAT":
					ExportFormat = convertPropToInt(value);
					break;
				case "EXPORTCOLUMNS":
					ExportColumns = convertPropToString(value);
					break;
				case "EXPORTCOLUMNCAPTIONS":
					ExportColumnCaptions = convertPropToString(value);
					break;
				}
			}
		}
	}

	public bool SaveSettings(DataRow userRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("FilePath = " + convertStringToProp(ExportFilePath) + "\r");
		stringBuilder.Append("ExportFieldHeadings = " + convertBoolToProp(ExportFieldHeadings) + "\r");
		stringBuilder.Append("ExportFieldCaptions = " + convertBoolToProp(ExportFieldCaptions) + "\r");
		stringBuilder.Append("SelectedRowsOnly = " + convertBoolToProp(ExportSelectedRowsOnly) + "\r");
		stringBuilder.Append("ExportFormat = " + convertDecimalToProp(ExportFormat) + "\r");
		stringBuilder.Append("ExportColumns = " + convertStringToProp(ExportColumns) + "\r");
		stringBuilder.Append("ExportColumnCaptions = " + convertStringToProp(ExportColumnCaptions) + "\r");
		userRow.SetField("dgExportProperties", stringBuilder.ToString());
		return true;
	}

	public bool SaveSettings(M1DataDictionary dataDictionary, string gridID, string userID)
	{
		bool result = false;
		DataSet dataSet = new DataSet();
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("SELECT * FROM DDGridDetails WHERE dgGridID=@Grid AND dgUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@Grid", SqlDbType.NVarChar)).Value = gridID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
		sqlDataAdapter.Fill(dataSet, "GridDetails");
		if (dataSet.Tables["GridDetails"].Rows.Count != 0)
		{
			SaveSettings(dataSet.Tables["GridDetails"].Rows[0]);
			new SqlCommandBuilder(sqlDataAdapter);
			sqlDataAdapter.Update(dataSet.Tables["GridDetails"].GetChanges());
			result = true;
		}
		return result;
	}

	private bool convertPropToBool(string value)
	{
		return value.Trim().ToUpper() != "FALSE";
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

	private int convertPropToInt(string value)
	{
		int result = 0;
		if (int.TryParse(value, out result))
		{
			return result;
		}
		return 0;
	}

	private string convertPropToString(string value)
	{
		value = value.Trim().Substring(1);
		value = value.Substring(0, value.Length - 1);
		return value;
	}

	private string convertBoolToProp(bool value)
	{
		if (value)
		{
			return "True";
		}
		return "False";
	}

	private string convertDecimalToProp(decimal value)
	{
		return value.ToString("G");
	}

	private string convertStringToProp(string value)
	{
		return "'" + value + "'";
	}
}
