using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using M1.Core;

namespace M1.Ax.Erp;

public class PayrollNZYear
{
	public void GenerateSchedule(M1BindingSource bsSchedules)
	{
		DataRow currentAsDataRow = bsSchedules.CurrentAsDataRow;
		M1BindingSource childBindingSource = bsSchedules.PrimaryTable.GetChildBindingSource("PayrollNZYearScheduleLines");
		childBindingSource.RemoveWhere(string.Empty, currentAsDataRow);
		string queryString = "select patPayrollEmployeeID, lmdEmployeeFirstName, lmdEmployeeLastName, lmdTaxFileNumber, lmeHireDate, lmeTerminationDate, lmdNZTaxCode, IsNull(paeChildSupportCode,'') As paeChildSupportCode, IsNull(paeStudentLoanType,0) AS paeStudentLoanType,  ISNULL(SUM(Case When (pafTaxCategory = 'T' Or pafTaxCategory = 'C') And pafPaidBy = 2 Then panAmount Else 0 End), 0) As TotalPAYE,  ISNULL(SUM(Case When panPayrollLineType = 'D' And ISNULL(paeChildSupport,IsNull(padChildSupport,0)) <> 0 Then panAmount Else 0 End), 0) As TotalChildSupport,  ISNULL(SUM(Case When panPayrollLineType = 'D' And ISNULL(paeStudentLoan,IsNull(padStudentLoan,0)) <> 0 Then panAmount Else 0 End), 0) As TotalStudentLoan,  ISNULL(SUM(Case When panPayrollLineType = 'D' And ISNULL(paeSuperannuation,IsNull(padSuperannuation,0)) <> 0 Then panAmount Else 0 End), 0) As TotalEmployeeKiwiSaver,  ISNULL(SUM(Case When panPayrollLineType = 'L' And ISNULL(paoSuperannuation,0) <> 0 Then panAmount Else 0 End), 0) As TotalEmployerKiwiSaver,  ISNULL(SUM(Case When pafTaxCategory = 'Z' And pafPaidBy = 1 Then panAmount Else 0 End), 0) As TotalESCT,  ISNULL((Select IsNull(Sum(pagSubtotal),0) From PayrollSessions Inner Join PayrollHeaders On pasPayrollSessionID = patPayrollSessionID Inner Join PayrollHeaderTotals on patPayrollSessionID = pagPayrollSessionID And patPayrollHeaderID = pagPayrollHeaderID Where pasPayrollDate >= @StartDate And pasPayrollDate <= @EndDate And patPayrollEmployeeID = lmeEmployeeID),0) As TotalGrossEarnings,  ISNULL(SUM(Case When panPayrollLineType = 'A' And ISNULL(pawAllowanceTaxMethod, ISNULL(paoAllowanceTaxMethod,0)) <> 2 And paoPaidBy = 2 Then panAmount Else 0 End), 0) As TotalTaxableAllowances  From PayrollSessions inner join PayrollHeaders on PasPayrollSessionID = patPayrollSessionID  inner join PayrollLines on patPayrollSessionID = panPayrollSessionID And patPayrollHeaderID = panPayrollHeaderID  inner join Employees on patPayrollEmployeeID = lmeEmployeeID  inner join EmployeePersonalData on patPayrollEmployeeID = lmdEmployeeID  left outer join IncomeTaxes on panIncomeTaxID = paxIncomeTaxID  left outer join IncomeTaxTypes on panIncomeTaxID = pafIncomeTaxID And panIncomeTaxTypeID = pafIncomeTaxTypeID  left outer join Deductions on panDeductionID = padDeductionID  left outer join EmployeeDeductions on panEmployeeID = paeEmployeeID And panDeductionID = paeDeductionID And panEmployeeDeductionID = paeEmployeeDeductionID  left outer join Allowances on panAllowanceID = paoAllowanceID  left outer join EmployeeAllowances on panEmployeeID = pawEmployeeID And panAllowanceID = pawAllowanceID and panEmployeeAllowanceID = pawEmployeeAllowanceID  Where pasTaxYear = @Year And pasPayrollDate >= @StartDate And pasPayrollDate <= @EndDate  Group By patPayrollEmployeeID, lmeEmployeeID, lmdEmployeeFirstName, lmdEmployeeLastName, lmdTaxFileNumber, lmeHireDate, lmeTerminationDate, lmdNZTaxCode, IsNull(paeChildSupportCode,''), IsNull(paeStudentLoanType,0)   Order By patPayrollEmployeeID";
		SqlCommand sqlCommand = bsSchedules.Database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime)).Value = currentAsDataRow["nzsStartDate"];
		sqlCommand.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime)).Value = currentAsDataRow["nzsEndDate"];
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = currentAsDataRow["nzsPayrollNZYearID"];
		DataTable dataTable = bsSchedules.Database.GetDataTable(sqlCommand);
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = default(decimal);
		decimal num4 = default(decimal);
		decimal num5 = default(decimal);
		decimal num6 = default(decimal);
		List<string> list = new List<string>();
		foreach (DataRow row in dataTable.Rows)
		{
			DataRow dataRow2 = (DataRow)childBindingSource.AddNew();
			childBindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2["nzlEmployeeID"] = row["patPayrollEmployeeID"];
			dataRow2["nzlEmployeeIRDNumber"] = row["lmdTaxFileNumber"];
			dataRow2["nzlEmployeeName"] = (row["lmdEmployeeLastName"]?.ToString() + " " + row["lmdEmployeeFirstName"]).PadLeft(childBindingSource.Fields["nzlEmployeeName"].FieldLength).Substring(0, childBindingSource.Fields["nzlEmployeeName"].FieldLength).Trim();
			dataRow2["nzlEmployeeTaxCode"] = row["lmdNZTaxCode"];
			if (!row.IsNull("lmeHireDate") && row.Field<DateTime>("lmeHireDate") >= currentAsDataRow.Field<DateTime>("nzsStartDate") && row.Field<DateTime>("lmeHireDate") <= currentAsDataRow.Field<DateTime>("nzsEndDate"))
			{
				dataRow2["nzlStartDate"] = row["lmeHireDate"];
			}
			if (!row.IsNull("lmeTerminationDate") && row.Field<DateTime>("lmeTerminationDate") >= currentAsDataRow.Field<DateTime>("nzsStartDate") && row.Field<DateTime>("lmeTerminationDate") <= currentAsDataRow.Field<DateTime>("nzsEndDate"))
			{
				dataRow2["nzlEndDate"] = row["lmeTerminationDate"];
			}
			if (!list.Contains(dataRow2.Field<string>("nzlEmployeeID"), StringComparer.CurrentCultureIgnoreCase))
			{
				dataRow2["nzlGrossEarnings"] = row.Field<decimal>("TotalGrossEarnings") + row.Field<decimal>("TotalTaxableAllowances");
				num += dataRow2.Field<decimal>("nzlGrossEarnings");
				list.Add(dataRow2.Field<string>("nzlEmployeeID"));
			}
			dataRow2["nzlPAYE"] = row["TotalPAYE"];
			num2 += dataRow2.Field<decimal>("nzlPAYE");
			dataRow2["nzlChildSupport"] = row["TotalChildSupport"];
			dataRow2["nzlChildSupportCode"] = row["paeChildSupportCode"];
			num3 += dataRow2.Field<decimal>("nzlChildSupport");
			dataRow2["nzlStudentLoan"] = row["TotalStudentLoan"];
			num4 += dataRow2.Field<decimal>("nzlStudentLoan");
			dataRow2["nzlStudentLoanType"] = row["paeStudentLoanType"];
			dataRow2["nzlKiwiSaver"] = row["TotalEmployeeKiwiSaver"];
			num5 += dataRow2.Field<decimal>("nzlKiwiSaver");
			dataRow2["nzlKiwiSaverEmployer"] = row.Field<decimal>("TotalEmployerKiwiSaver") - row.Field<decimal>("TotalESCT");
			num6 += dataRow2.Field<decimal>("nzlKiwiSaverEmployer");
		}
		currentAsDataRow["nzsTotalPAYE"] = num2;
		currentAsDataRow["nzsTotalChildSupport"] = num3;
		currentAsDataRow["nzsTotalStudentLoan"] = num4;
		currentAsDataRow["nzsTotalKiwiSaver"] = num5;
		currentAsDataRow["nzsTotalKiwiSaverEmployer"] = num6;
		currentAsDataRow["nzsTotalGrossEarnings"] = num;
	}

	public void ExportIR348(M1Database database, int year, string plant, int line, string fileName)
	{
		DateTime? dateTime = null;
		SqlCommand sqlCommand = database.NewSqlCommand("Select nzpEmployerIRDNumber,nzpContactPerson,nzpContactPhoneNumber,nzpContactEmailAddress,nzsEndDate,nzsTotalPAYE,nzsTotalChildSupport,nzsTotalStudentLoan,nzsTotalKiwiSaver,nzsTotalKiwiSaverEmployer,nzsTotalTaxCredits,nzsTotalFamilyTaxCredits,nzsTotalGrossEarnings,nzsTotalEarningsNotLiableForEL From PayrollNZYears Inner Join PayrollNZYearSchedules on nzpPayrollNZYearID = nzsPayrollNZYearID And nzpPlantID = nzsPlantID Where nzsPayrollNZYearID = @Year And nzsPlantID = @Plant And nzsPayrollNZYearScheduleID = @Line");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = year;
		sqlCommand.Parameters.Add(new SqlParameter("@Plant", SqlDbType.NVarChar)).Value = plant;
		sqlCommand.Parameters.Add(new SqlParameter("@Line", SqlDbType.Int)).Value = line;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		DataRow row = dataTable.Rows[0];
		string text = row.Field<string>("nzpEmployerIRDNumber").Replace("-", "").Replace(" ", "")
			.Replace("(", "")
			.Replace(")", "")
			.Replace(".", "")
			.Replace(",", "");
		string text2 = row.Field<string>("nzpContactPerson");
		string text3 = row.Field<string>("nzpContactPhoneNumber");
		string text4 = row.Field<string>("nzpContactEmailAddress");
		if (text4 == null)
		{
			text4 = string.Empty;
		}
		dateTime = row.Field<DateTime>("nzsEndDate");
		SqlCommand sqlCommand2 = database.NewSqlCommand("Select nzlStartDate,nzlEndDate,nzlLumpSumIndicator,nzlStudentLoanType,nzlEmployeeTaxCode,nzlEmployeeIRDNumber,nzlEmployeeName,nzlGrossEarnings,nzlEarningsNotLiableForEL,nzlPAYE,nzlChildSupport,nzlChildSupportCode,nzlStudentLoan,nzlKiwiSaver,nzlKiwiSaverEmployer,nzlTaxCredits,nzlFamilyTaxCredits From PayrollNZYearScheduleLines Where nzlPayrollNZYearID = @Year And nzlPlantID = @Plant And nzlPayrollNZYearScheduleID = @Line Order By nzlPayrollNZYearScheduleLineID");
		sqlCommand2.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int)).Value = year;
		sqlCommand2.Parameters.Add(new SqlParameter("@Plant", SqlDbType.NVarChar)).Value = plant;
		sqlCommand2.Parameters.Add(new SqlParameter("@Line", SqlDbType.Int)).Value = line;
		DataTable dataTable2 = database.GetDataTable(sqlCommand2);
		using StreamWriter streamWriter = File.CreateText(fileName);
		streamWriter.Write("HDR," + text.PadRight(9).Substring(0, 9).Trim() + "," + dateTime.Value.ToString("yyyyMMdd") + "," + text2.PadRight(20).Substring(0, 20).Trim() + "," + text3.PadRight(12).Substring(0, 12).Trim() + "," + (row.Field<decimal>("nzsTotalPAYE") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalChildSupport") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalStudentLoan") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalKiwiSaver") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalKiwiSaverEmployer") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalTaxCredits") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalFamilyTaxCredits") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalGrossEarnings") * 100m).ToString("0") + "," + (row.Field<decimal>("nzsTotalEarningsNotLiableForEL") * 100m).ToString("0") + ",ECI M1 VERSION 9.0," + text4.PadRight(60).Substring(0, 60).Trim() + ",0004\r\n");
		foreach (DataRow row2 in dataTable2.Rows)
		{
			string text5 = "";
			string text6 = "";
			if (!row2.IsNull("nzlStartDate"))
			{
				text5 = row2.Field<DateTime>("nzlStartDate").ToString("yyyyMMdd");
			}
			if (!row2.IsNull("nzlEndDate"))
			{
				text6 = row2.Field<DateTime>("nzlEndDate").ToString("yyyyMMdd");
			}
			string text7 = ((!row2.Field<bool>("nzlLumpSumIndicator")) ? "0" : "1");
			string text8 = "";
			if (row2.Field<byte>("nzlStudentLoanType") == 1)
			{
				text8 = "SLBOR";
			}
			else if (row2.Field<byte>("nzlStudentLoanType") == 2)
			{
				text8 = "SLCIR";
			}
			string text9 = "";
			text9 = (string.IsNullOrWhiteSpace(text8) ? row2.Field<string>("nzlEmployeeTaxCode") : text8);
			streamWriter.Write("DTL," + row2.Field<string>("nzlEmployeeIRDNumber").Replace("-", "").Replace(" ", "")
				.Replace("(", "")
				.Replace(")", "")
				.Replace(".", "")
				.Replace(",", "")
				.PadRight(9)
				.Substring(0, 9)
				.Trim() + "," + row2.Field<string>("nzlEmployeeName").PadRight(20).Substring(0, 20)
				.Trim() + "," + text9.PadRight(5).Substring(0, 5).Trim() + "," + text5 + "," + text6 + "," + (row2.Field<decimal>("nzlGrossEarnings") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlEarningsNotLiableForEL") * 100m).ToString("0") + "," + text7 + "," + (row2.Field<decimal>("nzlPAYE") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlChildSupport") * 100m).ToString("0") + "," + row2.Field<string>("nzlChildSupportCode").PadRight(1).Substring(0, 1)
				.Trim() + "," + (row2.Field<decimal>("nzlStudentLoan") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlKiwiSaver") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlKiwiSaverEmployer") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlTaxCredits") * 100m).ToString("0") + "," + (row2.Field<decimal>("nzlFamilyTaxCredits") * 100m).ToString("0") + "\r\n");
		}
		streamWriter.Close();
	}
}
