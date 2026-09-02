using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Payroll
{
	public bool RefreshSessionAmount(M1Database database, int sessionId, SqlTransaction transaction)
	{
		if (sessionId == 0)
		{
			return false;
		}
		try
		{
			SqlCommand sqlCommand = new SqlCommand("Update PayrollSessions Set pasPaymentAmount = IsNull((Select Sum(patNetPayAmount) From PayrollHeaders Where patPayrollSessionID = @sessionID),0) Where pasPayrollSessionID = @sessionID");
			sqlCommand.Parameters.Add(new SqlParameter("@sessionID", SqlDbType.Int)).Value = sessionId;
			database.ExecuteCommand(sqlCommand, transaction);
		}
		catch (Exception ex)
		{
			throw new M1Exception(ex.Message);
		}
		return true;
	}

	public bool ProcessStateUITaxYearQuarter(M1Database database, SqlTransaction transaction, int yearId, string plantId, int quarterId)
	{
		if (yearId == 0 || quarterId == 0)
		{
			return false;
		}
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		try
		{
			using SqlCommand sqlCommand = database.NewSqlCommand("Select * From StateUITaxYearQuarters Where puqStateUITaxYearID = @yearID And puqPlantID = @plantID And puqStateUITaxYearQuarterID = @quarterID");
			sqlCommand.Parameters.Add(new SqlParameter("@yearID", SqlDbType.Int)).Value = yearId;
			sqlCommand.Parameters.Add(new SqlParameter("@plantID", SqlDbType.Char, 5)).Value = plantId;
			sqlCommand.Parameters.Add(new SqlParameter("@quarterID", SqlDbType.Int)).Value = quarterId;
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand, fillSchema: false, out adapter);
			if (dataTable.Rows.Count > 0)
			{
				SqlDataAdapter adapter2 = new SqlDataAdapter();
				DataTable dataTable2 = database.GetDataTable("Select * From StateUITaxYearQuarterTotals Where 0=1", fillSchema: true, out adapter2);
				DataRow dataRow = null;
				string text = string.Empty;
				database.ExecuteCommand("Delete From StateUITaxYearQuarterTotals Where putStateUITaxYearID = " + yearId.ToSql() + " And putPlantID = " + plantId.ToSql() + " And putStateUITaxYearQuarterID = " + quarterId.ToSql());
				if ((database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).ProductCode.IsModulePurchased("MP", database))
				{
					text = " And pasPlantID = '" + plantId + "'";
				}
				DateTime dateTime = default(DateTime);
				DateTime dateTime2 = default(DateTime);
				DateTime dateTime3 = default(DateTime);
				DateTime dateTime4 = default(DateTime);
				DateTime dateTime5 = default(DateTime);
				switch (quarterId)
				{
				case 1:
					dateTime = new DateTime(yearId, 1, 1);
					dateTime2 = new DateTime(yearId, 3, 31);
					dateTime3 = new DateTime(yearId, 1, 12);
					dateTime4 = new DateTime(yearId, 2, 12);
					dateTime5 = new DateTime(yearId, 3, 12);
					break;
				case 2:
					dateTime = new DateTime(yearId, 4, 1);
					dateTime2 = new DateTime(yearId, 6, 30);
					dateTime3 = new DateTime(yearId, 4, 12);
					dateTime4 = new DateTime(yearId, 5, 12);
					dateTime5 = new DateTime(yearId, 6, 12);
					break;
				case 3:
					dateTime = new DateTime(yearId, 7, 1);
					dateTime2 = new DateTime(yearId, 9, 30);
					dateTime3 = new DateTime(yearId, 7, 12);
					dateTime4 = new DateTime(yearId, 8, 12);
					dateTime5 = new DateTime(yearId, 9, 12);
					break;
				case 4:
					dateTime = new DateTime(yearId, 10, 1);
					dateTime2 = new DateTime(yearId, 12, 31);
					dateTime3 = new DateTime(yearId, 10, 12);
					dateTime4 = new DateTime(yearId, 11, 12);
					dateTime5 = new DateTime(yearId, 12, 12);
					break;
				}
				for (int i = 1; i < 4; i++)
				{
					int month = (quarterId - 1) * 3 + i;
					DateTime d = new DateTime(yearId, month, 12);
					string queryString = "Select Count(patPayrollEmployeeID) As EmployeeCount From (Select Distinct patPayrollEmployeeID From PayrollSessions Inner Join PayrollHeaders On pasPayrollSessionID = patPayrollSessionID Where pasPayrollStartDate <= " + d.ToSql() + " AND pasPayrollEndDate >= " + d.ToSql() + " And pasPostedToGL <> 0 " + text + " ) As Temp";
					object value = database.ExecuteScalar(queryString);
					switch (i)
					{
					case 1:
						dataTable.Rows[0]["puqMonth1Employment"] = Convert.ToInt32(value);
						break;
					case 2:
						dataTable.Rows[0]["puqMonth2Employment"] = Convert.ToInt32(value);
						break;
					case 3:
						dataTable.Rows[0]["puqMonth3Employment"] = Convert.ToInt32(value);
						break;
					}
				}
				SqlCommand sqlCommand2 = new SqlCommand("Select xccCountyCode, Count(*) As EmployeeCount From (Select Distinct patPayrollEmployeeID, xccCountyCode From PayrollSessions Inner Join PayrollHeaders On pasPayrollSessionID = patPayrollSessionID Inner Join Employees on patPayrollEmployeeID = lmeEmployeeID Left Outer Join CountyCodes on lmeCountyCodeID = xccCountyCodeID Where pasPayrollStartDate >= @startDate AND pasPayrollEndDate <= @endDate And pasPostedToGL <> 0 " + text + ") As Temp Group by xccCountyCode Order By EmployeeCount DESC");
				sqlCommand2.Parameters.AddWithValue("@startDate", dateTime);
				sqlCommand2.Parameters.AddWithValue("@endDate", dateTime2);
				DataTable dataTable3 = database.GetDataTable(sqlCommand2);
				if (dataTable3.Rows.Count > 0)
				{
					if (dataTable3.Rows[0].Field<string>("xccCountyCode") == null)
					{
						dataTable.Rows[0]["puqCountyCode"] = string.Empty;
					}
					else
					{
						dataTable.Rows[0]["puqCountyCode"] = dataTable3.Rows[0].Field<string>("xccCountyCode");
					}
					int num = 0;
					int num2 = 0;
					foreach (DataRow row2 in dataTable3.Rows)
					{
						if (num == 0)
						{
							num++;
							continue;
						}
						num2 += Convert.ToInt32(row2["EmployeeCount"]);
						num++;
					}
					dataTable.Rows[0]["puqOutsideCountyEmployees"] = num2;
				}
				decimal num3 = default(decimal);
				decimal num4 = default(decimal);
				decimal num5 = default(decimal);
				SqlCommand sqlCommand3 = new SqlCommand("Select Top 1 parTaxPercent From IncomeTaxTableRevisions Where parIncomeTaxID = 'TX' And parIncomeTaxTypeID = 'SUT' And parStartDate <= @endDate Order by parStartDate DESC");
				sqlCommand3.Parameters.Add("@endDate", SqlDbType.DateTime).Value = dateTime2;
				DataTable dataTable4 = database.GetDataTable(sqlCommand3);
				if (dataTable4.Rows.Count > 0)
				{
					dataTable.Rows[0]["puqUITaxRate"] = dataTable4.Rows[0].Field<decimal>("parTaxPercent");
				}
				SqlCommand sqlCommand4 = new SqlCommand("Select PayrollHeaders.patPayrollEmployeeID,  lmdEmployeeFirstName, lmdEmployeeMiddleName, lmdEmployeeLastName, lmdTaxFileNumber, (Select IsNull(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollHeaderTotals On A.patPayrollSessionID = pagPayrollSessionID And A.patPayrollHeaderID = pagPayrollHeaderID Where pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) - (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeDeductions on panEmployeeID = paeEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID inner join Deductions on panDeductionID=padDeductionID Where IsNull(paeDeductionTaxMethod, padDeductionTaxMethod) = 1 And padPaidBy = 2 And pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) + (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeAllowances on panEmployeeID = pawEmployeeID And panEmployeeAllowanceID = pawEmployeeAllowanceID And panAllowanceID = pawAllowanceID inner join Allowances on panAllowanceID=paoAllowanceID Where (IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 1 Or IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 3) And (paoPaidBy = 2 Or (paoPaidBy = 1 And paoIncludeInTaxCalc = -1)) And pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) As TotalTaxablePay, (Select ISNULL(Sum(panAppliedPayAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Where pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " And panIncomeTaxID = 'TX' and panIncomeTaxTypeID = 'SUT' And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) As TotalTaxable, (Select ISNULL(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Where pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " And panIncomeTaxID = 'TX' and panIncomeTaxTypeID = 'SUT' And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) As TotalUITax, IsNull((Select Distinct patPayrollEmployeeID From PayrollSessions Inner Join PayrollHeaders A On pasPayrollSessionID = A.patPayrollSessionID Where pasPayrollStartDate <= @month1 AND pasPayrollEndDate >= @month1 And pasPostedToGL <> 0 " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID), '') As Month1, IsNull((Select Distinct patPayrollEmployeeID From PayrollSessions Inner Join PayrollHeaders A On pasPayrollSessionID = A.patPayrollSessionID Where pasPayrollStartDate <= @month2 AND pasPayrollEndDate >= @month2 And pasPostedToGL <> 0 " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID), '') As Month2, IsNull((Select Distinct patPayrollEmployeeID From PayrollSessions Inner Join PayrollHeaders A On pasPayrollSessionID = A.patPayrollSessionID Where pasPayrollStartDate <= @month3 AND pasPayrollEndDate >= @month3 And pasPostedToGL <> 0 " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID), '') As Month3 From PayrollSessions Inner Join PayrollHeaders on pasPayrollSessionID = patPayrollSessionID Inner Join EmployeePersonalData on patPayrollEmployeeID = lmdEmployeeID Where pasPayrollDate >= @quarterStart AND pasPayrollDate <= @quarterEnd " + text + " Group by PayrollHeaders.patPayrollEmployeeID, lmdEmployeeFirstName, lmdEmployeeMiddleName, lmdEmployeeLastName, lmdTaxFileNumber ");
				sqlCommand4.Parameters.Add("@quarterStart", SqlDbType.DateTime).Value = dateTime;
				sqlCommand4.Parameters.Add("@quarterEnd", SqlDbType.DateTime).Value = dateTime2;
				sqlCommand4.Parameters.Add("@month1", SqlDbType.DateTime).Value = dateTime3;
				sqlCommand4.Parameters.Add("@month2", SqlDbType.DateTime).Value = dateTime4;
				sqlCommand4.Parameters.Add("@month3", SqlDbType.DateTime).Value = dateTime5;
				DataTable dataTable5 = database.GetDataTable(sqlCommand4);
				if (dataTable5.Rows.Count > 0)
				{
					int num6 = 0;
					foreach (DataRow row3 in dataTable5.Rows)
					{
						dataRow = dataTable2.NewRow().BlankRow();
						dataRow.BeginEdit();
						dataRow["putStateUITaxYearID"] = yearId;
						dataRow["putPlantID"] = plantId;
						dataRow["putStateUITaxYearQuarterID"] = quarterId;
						dataRow["putStateUITaxYearQtrTotalID"] = ++num6;
						dataRow["putEmployeeID"] = row3.Field<string>("patPayrollEmployeeID").Trim();
						dataRow["putEmployeeSSN"] = row3.Field<string>("lmdTaxFileNumber").PadRight(11, ' ').Substring(0, 11)
							.Trim();
						dataRow["putEmployeeLastName"] = row3.Field<string>("lmdEmployeeLastName").Trim();
						dataRow["putEmployeeFirstName"] = row3.Field<string>("lmdEmployeeFirstName").Trim();
						if (row3.Field<string>("lmdEmployeeMiddleName").Trim().Length > 0)
						{
							dataRow["putEmployeeFirstName"] = dataRow["putEmployeeFirstName"]?.ToString() + " " + row3.Field<string>("lmdEmployeeMiddleName").Substring(0, 1);
						}
						dataRow["putWages"] = row3.Field<decimal>("TotalTaxablePay");
						dataRow["putTaxableWages"] = row3.Field<decimal>("TotalTaxable");
						dataRow["putTaxes"] = row3.Field<decimal>("TotalUITax");
						dataRow["putMonth1Employment"] = ((row3.Field<string>("Month1").Trim().Length > 0) ? (-1) : 0);
						dataRow["putMonth2Employment"] = ((row3.Field<string>("Month2").Trim().Length > 0) ? (-1) : 0);
						dataRow["putMonth3Employment"] = ((row3.Field<string>("Month3").Trim().Length > 0) ? (-1) : 0);
						num3 += Convert.ToDecimal(dataRow["putWages"]);
						num4 += Convert.ToDecimal(dataRow["putTaxableWages"]);
						num5 += Convert.ToDecimal(dataRow["putTaxes"]);
						dataRow.EndEdit();
						dataTable2.Rows.Add(dataRow);
					}
				}
				dataTable.Rows[0]["puqTotalWages"] = num3;
				dataTable.Rows[0]["puqTotalTaxableWages"] = num4;
				dataTable.Rows[0]["puqUITaxesDue"] = num5;
				dataTable.Rows[0]["puqInterest"] = 0;
				dataTable.Rows[0]["puqPenalty"] = 0;
				dataTable.Rows[0]["puqBalanceDuePriorPeriod"] = 0;
				dataTable.Rows[0]["puqTotalDue"] = num5;
				database.UpdateData(dataTable, adapter);
				if (dataTable2.Rows.Count <= 0)
				{
					return false;
				}
				database.UpdateData(dataTable2, adapter2);
			}
		}
		catch (Exception ex)
		{
			database.RollbackTransaction(transaction);
			throw new M1Exception(ex.Message);
		}
		database.CommitTransaction(transaction);
		return true;
	}

	public bool ExportStateUITaxYearQuarter(M1Database database, int year, string plant, int quarter)
	{
		StreamWriter streamWriter = null;
		if (year == 0 || quarter == 0)
		{
			return false;
		}
		try
		{
			_ = string.Empty;
			M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Filter = "All Files (*.*)|*.*",
				FilterIndex = 2,
				RestoreDirectory = true,
				Title = "Save As",
				FileName = "TWCWAGES.ICE",
				CheckPathExists = true,
				ValidateNames = true,
				AutoUpgradeEnabled = (appContext == null || !appContext.DisableOpenFileHelp)
			};
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				streamWriter = File.CreateText(saveFileDialog.FileName);
			}
			saveFileDialog.Dispose();
			if (streamWriter != null)
			{
				string text = string.Empty;
				switch (quarter)
				{
				case 1:
					text = "03";
					break;
				case 2:
					text = "06";
					break;
				case 3:
					text = "09";
					break;
				case 4:
					text = "12";
					break;
				}
				using (SqlCommand sqlCommand = database.NewSqlCommand("Select * From StateUITaxYears Inner Join StateUITaxYearQuarters On puyStateUITaxYearID = puqStateUITaxYearID And puyPlantID = puqPlantID Where puyStateUITaxYearID = @yearID And puyPlantID = @plantID And puqStateUITaxYearQuarterID = @quarterID"))
				{
					sqlCommand.Parameters.Add(new SqlParameter("@yearID", SqlDbType.Int)).Value = year;
					sqlCommand.Parameters.Add(new SqlParameter("@plantID", SqlDbType.Char, 5)).Value = plant;
					sqlCommand.Parameters.Add(new SqlParameter("@quarterID", SqlDbType.Int)).Value = quarter;
					DataTable dataTable = database.GetDataTable(sqlCommand);
					int num = 0;
					if (dataTable.Rows.Count <= 0)
					{
						throw new M1Exception("Record does not exist.");
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("A");
					stringBuilder.Append(year);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerIDNumber")).PadRight(9, ' ').Substring(0, 9));
					stringBuilder.Append("UTAX");
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerName").ToUpper().PadRight(50)
						.Substring(0, 50));
					stringBuilder.Append((dataTable.Rows[0].Field<string>("puyEmployerAddressLine1").ToUpper() + " " + dataTable.Rows[0].Field<string>("puyEmployerAddressLine2").ToUpper()).PadRight(40).Substring(0, 40));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerCity").ToUpper().PadRight(25)
						.Substring(0, 25));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerState").ToUpper().PadRight(2)
						.Substring(0, 2));
					stringBuilder.Append(' ', 13);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerPostCode")).ToUpper().PadRight(5)
						.Substring(0, 5));
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyContactPerson").ToUpper().PadRight(30)
						.Substring(0, 30));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyContactPhoneNumber").ToUpper().PadRight(10)
						.Substring(0, 10));
					stringBuilder.Append(' ', 10);
					stringBuilder.Append("Y");
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(' ', 10);
					stringBuilder.Append('0', 13);
					stringBuilder.Append(DateTime.Today.ToString("MMddyyyy"));
					stringBuilder.Append(' ', 25);
					streamWriter.WriteLine(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("B");
					stringBuilder.Append(year);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerIDNumber")).PadRight(9, ' ').Substring(0, 9));
					stringBuilder.Append(' ', 8);
					stringBuilder.Append(' ', 2);
					stringBuilder.Append(' ', 1);
					stringBuilder.Append(' ', 2);
					stringBuilder.Append(' ', 3);
					stringBuilder.Append(' ', 2);
					stringBuilder.Append(' ', 2);
					stringBuilder.Append("UTAX");
					stringBuilder.Append(' ', 108);
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerName").ToUpper().PadRight(44)
						.Substring(0, 44));
					stringBuilder.Append((dataTable.Rows[0].Field<string>("puyEmployerAddressLine1").ToUpper() + " " + dataTable.Rows[0].Field<string>("puyEmployerAddressLine2").ToUpper()).PadRight(35).Substring(0, 35));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerCity").ToUpper().PadRight(20)
						.Substring(0, 20));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerState").ToUpper().PadRight(2)
						.Substring(0, 2));
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerPostCode")).ToUpper().PadRight(5)
						.Substring(0, 5));
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(' ', 13);
					streamWriter.WriteLine(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("E");
					stringBuilder.Append(year);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerIDNumber")).PadRight(9, ' ').Substring(0, 9));
					stringBuilder.Append(' ', 9);
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerName").ToUpper().PadRight(50)
						.Substring(0, 50));
					stringBuilder.Append((dataTable.Rows[0].Field<string>("puyEmployerAddressLine1").ToUpper() + " " + dataTable.Rows[0].Field<string>("puyEmployerAddressLine2").ToUpper()).PadRight(40).Substring(0, 40));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerCity").ToUpper().PadRight(25)
						.Substring(0, 25));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyEmployerState").ToUpper().PadRight(2)
						.Substring(0, 2));
					stringBuilder.Append(' ', 8);
					stringBuilder.Append(' ', 5);
					stringBuilder.Append(PayrollHelpers.RemovePunctuation(dataTable.Rows[0].Field<string>("puyEmployerPostCode")).ToUpper().PadRight(5)
						.Substring(0, 5));
					stringBuilder.Append(' ', 8);
					stringBuilder.Append("UTAX");
					stringBuilder.Append("48");
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puyAccountNumber").ToUpper().PadRight(9)
						.Substring(0, 9));
					stringBuilder.Append(' ', 6);
					stringBuilder.Append(text);
					stringBuilder.Append("1");
					stringBuilder.Append(' ', 85);
					streamWriter.WriteLine(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					int num2 = 0;
					using (SqlCommand sqlCommand2 = database.NewSqlCommand("Select * From StateUITaxYearQuarterTotals Where putStateUITaxYearID = @yearID And putPlantID = @plantID And putStateUITaxYearQuarterID = @quarterID"))
					{
						sqlCommand2.Parameters.Add(new SqlParameter("@yearID", SqlDbType.Int)).Value = year;
						sqlCommand2.Parameters.Add(new SqlParameter("@plantID", SqlDbType.Char, 5)).Value = plant;
						sqlCommand2.Parameters.Add(new SqlParameter("@quarterID", SqlDbType.Int)).Value = quarter;
						DataTable dataTable2 = database.GetDataTable(sqlCommand2);
						if (dataTable2.Rows.Count > 0)
						{
							foreach (DataRow row in dataTable2.Rows)
							{
								stringBuilder.Append("S");
								stringBuilder.Append(PayrollHelpers.RemovePunctuation(row.Field<string>("putEmployeeSSN")).ToUpper().PadRight(9)
									.Substring(0, 9));
								stringBuilder.Append(row.Field<string>("putEmployeeLastName").ToUpper().PadRight(20)
									.Substring(0, 20));
								string text2 = row.Field<string>("putEmployeeFirstName").ToUpper();
								int num3 = text2.ToUpper().IndexOf(' ');
								if (num3 != -1)
								{
									stringBuilder.Append(text2.Substring(0, num3 + 1).PadRight(12).Substring(0, 12));
									stringBuilder.Append(text2.Substring(num3 + 1, 1));
								}
								else
								{
									stringBuilder.Append(text2.PadRight(12).Substring(0, 12));
									stringBuilder.Append(" ");
								}
								stringBuilder.Append("48");
								stringBuilder.Append(' ', 4);
								stringBuilder.Append('0', 14);
								stringBuilder.Append(FormatCurrencyForExportUS(row.Field<decimal>("putWages")).PadLeft(14, '0').Substring(0, 14));
								stringBuilder.Append('0', 14);
								stringBuilder.Append(FormatCurrencyForExportUS(row.Field<decimal>("putTaxableWages")).PadLeft(14, '0').Substring(0, 14));
								stringBuilder.Append('0', 24);
								stringBuilder.Append(' ', 13);
								stringBuilder.Append("UTAX");
								stringBuilder.Append(dataTable.Rows[0].Field<string>("puyAccountNumber").ToUpper().PadRight(9)
									.Substring(0, 9));
								stringBuilder.Append(' ', 16);
								stringBuilder.Append('0', 5);
								stringBuilder.Append('0', 28);
								stringBuilder.Append(' ', 7);
								stringBuilder.Append((!row.Field<bool>("putMonth1Employment")) ? "0" : "1");
								stringBuilder.Append((!row.Field<bool>("putMonth2Employment")) ? "0" : "1");
								stringBuilder.Append((!row.Field<bool>("putMonth3Employment")) ? "0" : "1");
								stringBuilder.Append(text + year);
								stringBuilder.Append(' ', 12);
								stringBuilder.Append(' ', 43);
								streamWriter.WriteLine(stringBuilder.ToString());
								stringBuilder.Length = 0;
								stringBuilder.Capacity = 0;
								num++;
								num2++;
							}
						}
					}
					stringBuilder.Append("T");
					stringBuilder.Append(num2.ToString().PadLeft(7, '0').Substring(0, 7));
					stringBuilder.Append("UTAX");
					stringBuilder.Append('0', 14);
					stringBuilder.Append(FormatCurrencyForExportUS(dataTable.Rows[0].Field<decimal>("puqTotalWages")).PadLeft(14, '0').Substring(0, 14));
					stringBuilder.Append('0', 14);
					stringBuilder.Append(FormatCurrencyForExportUS(dataTable.Rows[0].Field<decimal>("puqTotalTaxableWages")).PadLeft(14, '0').Substring(0, 14));
					stringBuilder.Append('0', 13);
					stringBuilder.Append((dataTable.Rows[0].Field<decimal>("puqUITaxRate") / 100m).ToString().PadRight(7, '0').Substring(1, 6));
					stringBuilder.Append(FormatCurrencyForExportUS(dataTable.Rows[0].Field<decimal>("puqUITaxesDue")).PadLeft(13, '0').Substring(0, 13));
					stringBuilder.Append('0', 44);
					stringBuilder.Append(' ', 4);
					stringBuilder.Append('0', 11);
					stringBuilder.Append(' ', 4);
					stringBuilder.Append('0', 22);
					stringBuilder.Append('0', 13);
					stringBuilder.Append('0', 28);
					stringBuilder.Append(Convert.ToInt32(dataTable.Rows[0].Field<int>("puqMonth1Employment")).ToString().PadLeft(7, '0')
						.Substring(0, 7));
					stringBuilder.Append(Convert.ToInt32(dataTable.Rows[0].Field<int>("puqMonth2Employment")).ToString().PadLeft(7, '0')
						.Substring(0, 7));
					stringBuilder.Append(Convert.ToInt32(dataTable.Rows[0].Field<int>("puqMonth3Employment")).ToString().PadLeft(7, '0')
						.Substring(0, 7));
					stringBuilder.Append(dataTable.Rows[0].Field<string>("puqCountyCode").PadRight(3, ' ').Substring(0, 3));
					stringBuilder.Append(Convert.ToInt32(dataTable.Rows[0].Field<int>("puqOutsideCountyEmployees")).ToString().PadLeft(7, '0')
						.Substring(0, 7));
					stringBuilder.Append(' ', 10);
					stringBuilder.Append(' ', 8);
					streamWriter.WriteLine(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Capacity = 0;
					num++;
					stringBuilder.Append("F");
					stringBuilder.Append(num2.ToString().PadLeft(10, '0').Substring(0, 10));
					stringBuilder.Append("0000000001");
					stringBuilder.Append("UTAX");
					stringBuilder.Append('0', 15);
					stringBuilder.Append(FormatCurrencyForExportUS(dataTable.Rows[0].Field<decimal>("puqTotalWages")).PadLeft(15, '0').Substring(0, 15));
					stringBuilder.Append('0', 15);
					stringBuilder.Append(FormatCurrencyForExportUS(dataTable.Rows[0].Field<decimal>("puqTotalTaxableWages")).PadLeft(15, '0').Substring(0, 15));
					stringBuilder.Append('0', 30);
					stringBuilder.Append('0', 24);
					stringBuilder.Append(' ', 136);
					streamWriter.WriteLine(stringBuilder.ToString());
				}
				streamWriter.Close();
				streamWriter.Dispose();
			}
		}
		catch (Exception ex)
		{
			throw new M1Exception(ex.Message);
		}
		return true;
	}

	private string FormatCurrencyForExportUS(decimal amount)
	{
		return Convert.ToInt64(amount * 100m).ToString();
	}

	public bool EmployeeHasPayrollSession(M1Database database, string employeeId, bool verifyPostedSession, bool verifyTransferredSessionToSTP)
	{
		string text = (verifyPostedSession ? "pasPostedToGL = 1" : "pasPostedToGL = 0");
		string text2 = (verifyTransferredSessionToSTP ? "pasTransferredToSTP = 1" : "pasTransferredToSTP = 0");
		using SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(pasPayrollSessionID) FROM PayrollSessions INNER JOIN PayrollHeaders ON pasPayrollSessionID = patPayrollSessionID WHERE " + text + " AND " + text2 + " AND patPayrollEmployeeID = @EmployeeId");
		sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand)) != 0m;
	}

	public bool CalculateForm940(M1Database database, SqlTransaction transaction, int yearID, string plantID)
	{
		if (yearID == 0)
		{
			return false;
		}
		if (transaction == null)
		{
			transaction = database.BeginTransaction();
		}
		try
		{
			using (SqlCommand sqlCommand = database.NewSqlCommand("Select * From Form940Years Where pfyForm940YearID = @yearID"))
			{
				sqlCommand.Parameters.Add(new SqlParameter("@yearID", SqlDbType.Int)).Value = yearID;
				if (database.GetDataTable(sqlCommand, transaction).Rows.Count > 0)
				{
					SqlDataAdapter adapter = new SqlDataAdapter();
					DataTable dataTable = database.GetDataTable("SELECT * FROM Form940YearTotals WHERE 0=1", fillSchema: true, out adapter, transaction);
					DataRow dataRow = null;
					SqlDataAdapter adapter2 = new SqlDataAdapter();
					DataTable dataTable2 = database.GetDataTable("SELECT * FROM Form940YearTotalStates WHERE 0=1", fillSchema: true, out adapter2, transaction);
					DataRow dataRow2 = null;
					string text = string.Empty;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("Delete From Form940YearTotals Where pftForm940YearID = " + yearID.ToSql() + " And pftPlantID = " + plantID.ToSql());
					stringBuilder.Append("\r\n");
					stringBuilder.Append("Delete From Form940YearTotalStates Where pfsForm940YearID = " + yearID.ToSql() + " And pfsPlantID = " + plantID.ToSql());
					database.ExecuteCommand(stringBuilder.ToString(), transaction);
					if ((database.GetService(typeof(M1DataDictionary)) as M1DataDictionary).ProductCode.IsModulePurchased("MP", database))
					{
						text = " And pasPlantID = @plantID ";
					}
					int num = 0;
					dataRow = dataTable.NewRow().BlankRow();
					dataRow.BeginEdit();
					dataRow["pftForm940YearID"] = yearID;
					dataRow["pftPlantID"] = plantID;
					dataRow["pftForm940YearTotalID"] = ++num;
					using (SqlCommand sqlCommand2 = database.NewSqlCommand("Select paxState From PayrollSessions Inner Join PayrollLines On pasPayrollSessionID = panPayrollSessionID Inner Join IncomeTaxes On panIncomeTaxID = paxIncomeTaxID Where pasTaxYear = @year " + text + " And panIncomeTaxTypeID = 'SUT' Group By paxState"))
					{
						sqlCommand2.Parameters.Add(new SqlParameter("@year", SqlDbType.Int)).Value = yearID;
						sqlCommand2.Parameters.Add(new SqlParameter("@plantID", SqlDbType.VarChar)).Value = plantID;
						DataTable dataTable3 = database.GetDataTable(sqlCommand2, transaction);
						if (dataTable3.Rows.Count > 0)
						{
							if (dataTable3.Rows.Count == 1)
							{
								dataRow["pftStateID"] = dataTable3.Rows[0].Field<string>("paxState").Substring(0, 2);
							}
							else
							{
								dataRow["pftMultiStateEmployer"] = -1;
							}
						}
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append("Select ISNULL(SUM(TotalTaxablePay),0) As TotalTaxablePay, ISNULL(SUM(TotalExemption),0) As TotalExemption, ISNULL(Sum((Case When TotalTaxablePay - TotalExemption > 7000 Then TotalTaxablePay - TotalExemption - 7000 Else 0 End)),0) As TotalFUTAExcess From (Select PayrollHeaders.patPayrollEmployeeID, ");
					stringBuilder2.Append("(Select IsNull(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollHeaderTotals On A.patPayrollSessionID = pagPayrollSessionID And A.patPayrollHeaderID = pagPayrollHeaderID Where pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) ");
					stringBuilder2.Append("- (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeDeductions on panEmployeeID = paeEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID inner join Deductions on panDeductionID=padDeductionID Where IsNull(paeDeductionTaxMethod, padDeductionTaxMethod) = 1 And padPaidBy = 2 And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) ");
					stringBuilder2.Append("+ (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeAllowances on panEmployeeID = pawEmployeeID And panEmployeeAllowanceID = pawEmployeeAllowanceID And panAllowanceID = pawAllowanceID inner join Allowances on panAllowanceID=paoAllowanceID Where (IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 1 Or IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 3) And (paoPaidBy = 2 Or (paoPaidBy = 1 And paoIncludeInTaxCalc = -1)) And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) As TotalTaxablePay, ");
					stringBuilder2.Append("(Select ISNULL(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollHeaderTotals On A.patPayrollSessionID = pagPayrollSessionID And A.patPayrollHeaderID = pagPayrollHeaderID Inner Join PayrollRateTaxExemptions on pagPayrollRateID = pavPayrollRateID Where pasTaxYear = @year " + text + " And pavIncomeTaxID = 'FED' and pavIncomeTaxTypeID = 'FUT' And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) ");
					stringBuilder2.Append("+(Select ISNULL(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Inner Join DeductionTaxExemptions on panDeductionID = pauDeductionID Inner Join Deductions on pauDeductionID = padDeductionID left outer Join EmployeeDeductions on panEmployeeID = paeEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID Where IsNull(paeDeductionTaxMethod,padDeductionTaxMethod)=3 And pauIncomeTaxID = 'FED' And pauIncomeTaxTypeID = 'FUT' And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) ");
					stringBuilder2.Append("+(Select ISNULL(SUM(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Inner Join AllowanceTaxExemptions on panAllowanceID = lnoAllowanceID Inner Join Allowances on lnoAllowanceID = paoAllowanceID left outer Join EmployeeAllowances on panEmployeeID = pawEmployeeID And panEmployeeAllowanceID = pawEmployeeAllowanceID And panAllowanceID = pawAllowanceID Where IsNull(pawAllowanceTaxMethod,paoAllowanceTaxMethod)=3 And lnoIncomeTaxID = 'FED' And lnoIncomeTaxTypeID = 'FUT' And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID) ");
					stringBuilder2.Append("As TotalExemption From PayrollSessions Inner Join PayrollHeaders on pasPayrollSessionID = patPayrollSessionID Inner Join PayrollHeaderTotals on patPayrollSessionID = pagPayrollSessionID And patPayrollHeaderID = pagPayrollHeaderID ");
					stringBuilder2.Append("Where pasTaxYear = @year " + text + " Group by PayrollHeaders.patPayrollEmployeeID) As TempTable");
					using (SqlCommand sqlCommand3 = database.NewSqlCommand(stringBuilder2.ToString()))
					{
						sqlCommand3.Parameters.Add(new SqlParameter("@year", SqlDbType.Int)).Value = yearID;
						sqlCommand3.Parameters.Add(new SqlParameter("@plantID", SqlDbType.VarChar)).Value = plantID;
						DataTable dataTable4 = database.GetDataTable(sqlCommand3, transaction);
						if (dataTable4.Rows.Count > 0)
						{
							dataRow["pftTotalPayments"] = dataTable4.Rows[0].Field<decimal>("TotalTaxablePay");
							dataRow["pftExemptFromFUTA"] = dataTable4.Rows[0].Field<decimal>("TotalExemption");
							dataRow["pftPaymentExcess"] = dataTable4.Rows[0].Field<decimal>("TotalFUTAExcess");
							dataRow["pftExemptSubtotal"] = dataTable4.Rows[0].Field<decimal>("TotalExemption") + dataTable4.Rows[0].Field<decimal>("TotalFUTAExcess");
							dataRow["pftTotalTaxableFUTA"] = dataTable4.Rows[0].Field<decimal>("TotalTaxablePay") - (dataTable4.Rows[0].Field<decimal>("TotalExemption") + dataTable4.Rows[0].Field<decimal>("TotalFUTAExcess"));
							dataRow["pftFUTABeforeAdjustments"] = Math.Round(Convert.ToDouble(dataRow["pftTotalTaxableFUTA"]) * 0.006, 2);
						}
					}
					stringBuilder2.Length = 0;
					stringBuilder2.Capacity = 0;
					stringBuilder2.Append("Select paxState, ISNULL(SUM(TotalTaxablePay),0) As TotalTaxablePay, ISNULL(SUM(TotalExemption),0) As TotalExemption, ISNULL(Sum((Case When TotalTaxablePay - TotalExemption > 7000 Then TotalTaxablePay - TotalExemption - 7000 Else 0 End)),0) As TotalFUTAExcess From ");
					stringBuilder2.Append("(Select IncomeTaxID, TempTable.patPayrollEmployeeID, SUM(TempTable.TotalTaxablePay) As TotalTaxablePay, SUM(TempTable.TotalExemption) As TotalExemption From (Select PH.patPayrollEmployeeID, ");
					stringBuilder2.Append("(Select IsNull(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollHeaderTotals On A.patPayrollSessionID = pagPayrollSessionID And A.patPayrollHeaderID = pagPayrollHeaderID Where pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) ");
					stringBuilder2.Append("- (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeDeductions on panEmployeeID = paeEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID inner join Deductions on panDeductionID=padDeductionID Where IsNull(paeDeductionTaxMethod, padDeductionTaxMethod) = 1 And padPaidBy = 2 And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) ");
					stringBuilder2.Append("+ (Select IsNull(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID left outer join EmployeeAllowances on panEmployeeID = pawEmployeeID And panEmployeeAllowanceID = pawEmployeeAllowanceID And panAllowanceID = pawAllowanceID inner join Allowances on panAllowanceID=paoAllowanceID Where (IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 1 Or IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 3) And (paoPaidBy = 2 Or (paoPaidBy = 1 And paoIncludeInTaxCalc = -1)) And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) As TotalTaxablePay, ");
					stringBuilder2.Append("(Select ISNULL(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollHeaderTotals On A.patPayrollSessionID = pagPayrollSessionID And A.patPayrollHeaderID = pagPayrollHeaderID Inner Join PayrollRateTaxExemptions on pagPayrollRateID = pavPayrollRateID Where pasTaxYear = @year " + text + " And pavIncomeTaxID = 'FED' and pavIncomeTaxTypeID = 'FUT' And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) ");
					stringBuilder2.Append("+(Select ISNULL(Sum(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Inner Join DeductionTaxExemptions on panDeductionID = pauDeductionID Inner Join Deductions on pauDeductionID = padDeductionID left outer Join EmployeeDeductions on panEmployeeID = paeEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID Where IsNull(paeDeductionTaxMethod,padDeductionTaxMethod)=3 And pauIncomeTaxID = 'FED' And pauIncomeTaxTypeID = 'FUT' And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) ");
					stringBuilder2.Append("+(Select ISNULL(SUM(panAmount),0) From PayrollSessions Inner Join PayrollHeaders A on pasPayrollSessionID = A.patPayrollSessionID Inner Join PayrollLines On A.patPayrollSessionID = panPayrollSessionID And A.patPayrollHeaderID = panPayrollHeaderID Inner Join AllowanceTaxExemptions on panAllowanceID = lnoAllowanceID Inner Join Allowances on lnoAllowanceID = paoAllowanceID left outer Join EmployeeAllowances on panEmployeeID = pawEmployeeID And panEmployeeAllowanceID = pawEmployeeAllowanceID And panAllowanceID = pawAllowanceID Where IsNull(pawAllowanceTaxMethod,paoAllowanceTaxMethod)=3 And lnoIncomeTaxID = 'FED' And lnoIncomeTaxTypeID = 'FUT' And pasTaxYear = @year " + text + " And A.patPayrollEmployeeID = PH.patPayrollEmployeeID) As TotalExemption, ");
					stringBuilder2.Append("ISNULL((Select distinct top 1 panIncomeTaxID From PayrollLines A Where A.panIncomeTaxID <> 'FED' And A.panPayrollSessionID = PH.patPayrollSessionID And A.panPayrollHeaderID = PH.patPayrollHeaderID),'') As IncomeTaxID ");
					stringBuilder2.Append("From PayrollSessions Inner Join PayrollHeaders PH on pasPayrollSessionID = patPayrollSessionID Inner Join PayrollHeaderTotals on patPayrollSessionID = pagPayrollSessionID And patPayrollHeaderID = pagPayrollHeaderID Where pasTaxYear = @year " + text + " Group by PH.patPayrollEmployeeID, PH.patPayrollSessionID, PH.patPayrollHeaderID) As TempTable ");
					stringBuilder2.Append("Group by IncomeTaxID, patPayrollEmployeeID) As TempTable2 Inner Join IncomeTaxes On IncomeTaxID = paxIncomeTaxID Where paxTaxAuthority = 2 Group By paxState");
					using (SqlCommand sqlCommand4 = database.NewSqlCommand(stringBuilder2.ToString()))
					{
						sqlCommand4.Parameters.Add(new SqlParameter("@year", SqlDbType.Int)).Value = yearID;
						sqlCommand4.Parameters.Add(new SqlParameter("@plantID", SqlDbType.VarChar)).Value = plantID;
						DataTable dataTable5 = database.GetDataTable(sqlCommand4, transaction);
						if (dataTable5.Rows.Count > 0)
						{
							decimal num2 = default(decimal);
							int num3 = 0;
							foreach (DataRow row in dataTable5.Rows)
							{
								dataRow2 = dataTable2.NewRow().BlankRow();
								dataRow2.BeginEdit();
								dataRow2["pfsForm940YearID"] = yearID;
								dataRow2["pfsPlantID"] = plantID;
								dataRow2["pfsForm940YearTotalID"] = num;
								dataRow2["pfsForm940YearTotalStateID"] = ++num3;
								dataRow2["pfsState"] = row.Field<string>("paxState").Substring(0, 2);
								dataRow2["pfsFUTATaxableWages"] = row.Field<decimal>("TotalTaxablePay") - row.Field<decimal>("TotalExemption") - row.Field<decimal>("TotalFUTAExcess");
								dataRow2["pfsReductionRate"] = ReductionRate.GetReductionRate(row.Field<string>("paxState").Substring(0, 2));
								dataRow2["pfsCreditReduction"] = Math.Round(Convert.ToDecimal(dataRow2["pfsReductionRate"]) * Convert.ToDecimal(dataRow2["pfsFUTATaxableWages"]), 2);
								num2 += Convert.ToDecimal(dataRow2["pfsCreditReduction"]);
								dataRow2.EndEdit();
								dataTable2.Rows.Add(dataRow2);
							}
							dataRow["pftAdjustCreditReduction"] = num2;
						}
					}
					dataRow["pftFUTAAfterAdjustments"] = Convert.ToDecimal(dataRow["pftFUTABeforeAdjustments"]) + Convert.ToDecimal(dataRow["pftAdjustAllExcludeState"]) + Convert.ToDecimal(dataRow["pftAdjustSomeExcludeState"]) + Convert.ToDecimal(dataRow["pftAdjustCreditReduction"]);
					dataRow["pftBalanceDue"] = Convert.ToDecimal(dataRow["pftFUTAAfterAdjustments"]) - Convert.ToDecimal(dataRow["pftFUTADeposited"]);
					stringBuilder2.Length = 0;
					stringBuilder2.Capacity = 0;
					stringBuilder2.Append("Select ISNULL((Select SUM(panAmount) From PayrollSessions Inner Join PayrollLines On pasPayrollSessionID = panPayrollSessionID Where pasTaxYear = @year " + text + " And panIncomeTaxID = 'FED' And panIncomeTaxTypeID = 'FUT' And pasPayrollDate >= @Q1Start And pasPayrollDate <= @Q1Stop),0) As Q1Liability, ");
					stringBuilder2.Append("ISNULL((Select SUM(panAmount) From PayrollSessions Inner Join PayrollLines On pasPayrollSessionID = panPayrollSessionID Where pasTaxYear = @year " + text + " And panIncomeTaxID = 'FED' And panIncomeTaxTypeID = 'FUT' And pasPayrollDate >= @Q2Start And pasPayrollDate <= @Q2Stop),0) As Q2Liability, ");
					stringBuilder2.Append("ISNULL((Select SUM(panAmount) From PayrollSessions Inner Join PayrollLines On pasPayrollSessionID = panPayrollSessionID Where pasTaxYear = @year " + text + " And panIncomeTaxID = 'FED' And panIncomeTaxTypeID = 'FUT' And pasPayrollDate >= @Q3Start And pasPayrollDate <= @Q3Stop),0) As Q3Liability, ");
					stringBuilder2.Append("ISNULL((Select SUM(panAmount) From PayrollSessions Inner Join PayrollLines On pasPayrollSessionID = panPayrollSessionID Where pasTaxYear = @year " + text + " And panIncomeTaxID = 'FED' And panIncomeTaxTypeID = 'FUT' And pasPayrollDate >= @Q4Start And pasPayrollDate <= @Q4Stop),0) As Q4Liability ");
					using (SqlCommand sqlCommand5 = database.NewSqlCommand(stringBuilder2.ToString()))
					{
						sqlCommand5.Parameters.Add(new SqlParameter("@year", SqlDbType.Int)).Value = yearID;
						sqlCommand5.Parameters.Add(new SqlParameter("@plantID", SqlDbType.VarChar)).Value = plantID;
						sqlCommand5.Parameters.Add(new SqlParameter("@Q1Start", SqlDbType.DateTime)).Value = new DateTime(yearID, 1, 1);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q1Stop", SqlDbType.DateTime)).Value = new DateTime(yearID, 3, 31);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q2Start", SqlDbType.DateTime)).Value = new DateTime(yearID, 4, 1);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q2Stop", SqlDbType.DateTime)).Value = new DateTime(yearID, 6, 30);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q3Start", SqlDbType.DateTime)).Value = new DateTime(yearID, 7, 1);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q3Stop", SqlDbType.DateTime)).Value = new DateTime(yearID, 9, 30);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q4Start", SqlDbType.DateTime)).Value = new DateTime(yearID, 10, 1);
						sqlCommand5.Parameters.Add(new SqlParameter("@Q4Stop", SqlDbType.DateTime)).Value = new DateTime(yearID, 12, 31);
						DataTable dataTable6 = database.GetDataTable(sqlCommand5, transaction);
						if (dataTable6.Rows.Count > 0)
						{
							dataRow["pftFUTALiabilityQ1"] = dataTable6.Rows[0].Field<decimal>("Q1Liability");
							dataRow["pftFUTALiabilityQ2"] = dataTable6.Rows[0].Field<decimal>("Q2Liability");
							dataRow["pftFUTALiabilityQ3"] = dataTable6.Rows[0].Field<decimal>("Q3Liability");
							dataRow["pftFUTALiabilityQ4"] = dataTable6.Rows[0].Field<decimal>("Q4Liability");
							dataRow["pftTotalTaxLiability"] = dataTable6.Rows[0].Field<decimal>("Q1Liability") + dataTable6.Rows[0].Field<decimal>("Q2Liability") + dataTable6.Rows[0].Field<decimal>("Q3Liability") + dataTable6.Rows[0].Field<decimal>("Q4Liability");
						}
					}
					dataRow.EndEdit();
					dataTable.Rows.Add(dataRow);
					if (dataTable.Rows.Count <= 0)
					{
						return false;
					}
					database.UpdateData(dataTable, adapter, transaction);
					if (dataTable2.Rows.Count > 0)
					{
						database.UpdateData(dataTable2, adapter2, transaction);
					}
				}
			}
			database.CommitTransaction(transaction);
		}
		catch (Exception ex)
		{
			database.RollbackTransaction(transaction);
			throw new M1Exception(ex.Message);
		}
		return true;
	}
}
