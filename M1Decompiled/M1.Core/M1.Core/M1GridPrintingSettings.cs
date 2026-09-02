using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace M1.Core;

public class M1GridPrintingSettings
{
	public bool PrintColumnHeadings = true;

	public bool PrintSelectedRowsOnly;

	public bool UseFieldNameAsHeading;

	public string Printer = string.Empty;

	public string PrintPaperSize = string.Empty;

	public string PrintPaperSource = string.Empty;

	public bool PrintOrientationLandscape;

	public string PrintColumns = string.Empty;

	public string ColumnHeadings = string.Empty;

	public string GetGridProperties(M1DataDictionary dataDictionary, string gridID, string userID)
	{
		SqlCommand sqlCommand = dataDictionary.NewSqlCommand("SELECT dgPrintingProperties FROM DDGridDetails WHERE dgGridID=@Grid AND dgUserID = @User");
		sqlCommand.Parameters.Add(new SqlParameter("@Grid", SqlDbType.NVarChar)).Value = gridID;
		sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = userID;
		return Convert.ToString(dataDictionary.ExecuteScalar(sqlCommand));
	}

	public void LoadDefaults()
	{
		PrintColumnHeadings = true;
		PrintSelectedRowsOnly = false;
		UseFieldNameAsHeading = false;
		Printer = string.Empty;
		PrintPaperSize = string.Empty;
		PrintPaperSource = string.Empty;
		PrintOrientationLandscape = false;
		PrintColumns = string.Empty;
		ColumnHeadings = string.Empty;
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
				case "PRINTCOLUMNHEADINGS":
					PrintColumnHeadings = convertPropToBool(value);
					break;
				case "PRINTSELECTEDROWSONLY":
					PrintSelectedRowsOnly = convertPropToBool(value);
					break;
				case "USEFIELDNAMEASHEADING":
					UseFieldNameAsHeading = convertPropToBool(value);
					break;
				case "PRINTER":
					Printer = convertPropToString(value);
					break;
				case "PRINTPAPERSIZE":
					PrintPaperSize = convertPropToString(value);
					break;
				case "PRINTPAPERSOURCE":
					PrintPaperSource = convertPropToString(value);
					break;
				case "PRINTORIENTATIONLANDSCAPE":
					PrintOrientationLandscape = convertPropToBool(value);
					break;
				case "PRINTCOLUMNS":
					PrintColumns = convertPropToString(value);
					break;
				case "COLUMNHEADINGS":
					ColumnHeadings = convertPropToString(value);
					break;
				}
			}
		}
	}

	public bool SaveSettings(DataRow userRow)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("PrintColumnHeadings = " + convertBoolToProp(PrintColumnHeadings) + "\r");
		stringBuilder.Append("PrintSelectedRowsOnly = " + convertBoolToProp(PrintSelectedRowsOnly) + "\r");
		stringBuilder.Append("UseFieldNameAsHeading = " + convertBoolToProp(UseFieldNameAsHeading) + "\r");
		stringBuilder.Append("Printer = " + convertStringToProp(Printer) + "\r");
		stringBuilder.Append("PrintPaperSize = " + convertStringToProp(PrintPaperSize) + "\r");
		stringBuilder.Append("PrintPaperSource = " + convertStringToProp(PrintPaperSource) + "\r");
		stringBuilder.Append("PrintOrientationLandscape = " + convertBoolToProp(PrintOrientationLandscape) + "\r");
		stringBuilder.Append("PrintColumns = " + convertStringToProp(PrintColumns) + "\r");
		stringBuilder.Append("ColumnHeadings = " + convertStringToProp(ColumnHeadings) + "\r");
		userRow.SetField("dgPrintingProperties", stringBuilder.ToString());
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
