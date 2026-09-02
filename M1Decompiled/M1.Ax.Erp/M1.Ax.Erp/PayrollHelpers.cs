using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class PayrollHelpers
{
	private const string PazDescriptionField = "pazDescription";

	private const string ChildSupportGarnishees = "G";

	private const string ChildSupportDeductions = "D";

	public static DateTime GetMaxStpSessionDate(int stpTaxYear)
	{
		return new DateTime((stpTaxYear <= 0) ? 1 : stpTaxYear, 6, 30);
	}

	public static DateTime GetMinStpSessionDate(int stpTaxYear)
	{
		return new DateTime((stpTaxYear <= 1) ? 1 : (stpTaxYear - 1), 7, 1);
	}

	public static string RemovePunctuation(string data)
	{
		return data.Replace("-", "").Replace(" ", "").Replace("(", "")
			.Replace(")", "")
			.Replace(".", "")
			.Replace(",", "");
	}

	public static string AddDoubleQuotesToString(string inputString)
	{
		return "\"" + inputString + "\"";
	}

	public static string FormatAmount(decimal amount, bool suppressWhenZero = false)
	{
		if (suppressWhenZero && amount == 0m)
		{
			return string.Empty;
		}
		if (amount != 0m)
		{
			return M1Math.Round(amount, 2).ToString(CultureInfo.InvariantCulture);
		}
		return "0";
	}

	public static string GetUTCISO8601FormatString(DateTime dateTimeStamp)
	{
		return TimeZoneInfo.ConvertTimeToUtc(dateTimeStamp).ToString("s") + "Z";
	}

	public static string GetTaxTreatmentCode(M1Database database, string employeeId, string taxFileNumber, bool isHolidayMaker)
	{
		List<string> taxFileNumbersAto = new List<string> { "000000000" };
		string @char = GetChar1(taxFileNumber, isHolidayMaker, taxFileNumbersAto);
		string char2 = GetChar2(database, employeeId, "Tax Free Threshold", "No Tax Free Threshold", "Foreign Resident", @char, taxFileNumbersAto, taxFileNumber);
		string char3 = GetChar3(database, employeeId, "Study & Training Supp. Loans (Tax Free Threshold)", "Study & Training Supp. Loans(No Tax Free Threshold", @char);
		string char4 = GetChar4(database, employeeId, @char);
		string char5 = GetChar5(database, employeeId, "Full Medicare Levy Exemption", "Half Medicare Levy Exemption", @char, char4);
		string char6 = GetChar6(database, "Half Medicare Levy Exemption", employeeId, @char, char5);
		return @char + char2 + char3 + char4 + char5 + char6;
	}

	private static string GetChar1(string taxFileNumber, bool isHolidayMaker, ICollection<string> taxFileNumbersAto)
	{
		if (isHolidayMaker)
		{
			return "H";
		}
		if (taxFileNumbersAto.Contains(taxFileNumber))
		{
			return "N";
		}
		return "R";
	}

	private static string GetChar2(M1Database database, string employeeId, string taxFreeThreshold, string noTaxFreeThreshold, string foreignResident, string firstChar, ICollection<string> taxFileNumbersAto, string taxFileNumber)
	{
		string result = string.Empty;
		DataTable dataToChar = GetDataToChar2(database, employeeId);
		switch (firstChar)
		{
		case "R":
			if (!dataToChar.Rows.Count.Equals(0))
			{
				foreach (DataRow row in dataToChar.Rows)
				{
					if (row.Field<string>("pazDescription").ToUpper().Equals(taxFreeThreshold.ToUpper()))
					{
						result = "T";
						break;
					}
					if (row.Field<string>("pazDescription").ToUpper().Equals(noTaxFreeThreshold.ToUpper()))
					{
						result = "N";
						break;
					}
					result = "N";
				}
			}
			else
			{
				result = "N";
			}
			break;
		case "N":
			if (!dataToChar.Rows.Count.Equals(0))
			{
				foreach (DataRow row2 in dataToChar.Rows)
				{
					if (row2.Field<string>("pazDescription").ToUpper().Equals(foreignResident.ToUpper()))
					{
						result = "F";
						break;
					}
					result = "A";
				}
			}
			else
			{
				result = "A";
			}
			break;
		case "H":
			result = ((!taxFileNumbersAto.Contains(taxFileNumber)) ? "R" : "F");
			break;
		}
		return result;
	}

	private static DataTable GetDataToChar2(M1Database database, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pazDescription FROM EmployeeIncomeTaxes JOIN IncomeTaxTables on pazIncomeTaxTableID = pamIncomeTaxTableID WHERE pamInactive = 0 AND pamEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		return database.GetDataTable(sqlCommand);
	}

	private static string GetChar3(M1Database database, string employeeId, string studyTrainingTax, string studyTrainingNoTax, string firstChar)
	{
		string result = string.Empty;
		if (!firstChar.Equals("R"))
		{
			result = "X";
		}
		else if (GetDataToChar3(database, employeeId).Rows.Count == 0)
		{
			result = "X";
		}
		else
		{
			foreach (DataRow row in GetDataToChar3(database, employeeId).Rows)
			{
				if (row.Field<string>("pazDescription").ToUpper().Equals(studyTrainingTax.ToUpper()) || row.Field<string>("pazDescription").ToUpper().Equals(studyTrainingNoTax.ToUpper()))
				{
					result = "S";
					break;
				}
				result = "X";
			}
		}
		return result;
	}

	private static DataTable GetDataToChar3(M1Database database, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT itt.pazDescription FROM EmployeeIncomeTaxes eit INNER JOIN IncomeTaxTables itt ON itt.pazIncomeTaxTableID = eit.pamIncomeTaxTableID AND itt.pazIncomeTaxID = eit.pamIncomeTaxID WHERE eit.pamInactive = 0 AND eit.pamEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		return database.GetDataTable(sqlCommand);
	}

	private static string GetChar4(M1Database database, string employeeId, string firstChar)
	{
		string result = string.Empty;
		if (!firstChar.Equals("R"))
		{
			result = "X";
		}
		else if (GetDataToChar4(database, employeeId).Rows.Count == 0)
		{
			result = "X";
		}
		else
		{
			foreach (DataRow row in GetDataToChar4(database, employeeId).Rows)
			{
				if (Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) <= 90000m)
				{
					result = "X";
				}
				else if (Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) >= 90001m && Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) <= 105000m)
				{
					result = "1";
				}
				else if (Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) >= 105001m && Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) <= 140000m)
				{
					result = "2";
				}
				else if (Math.Ceiling(row.Field<decimal>("lnrSalaryAmount")) >= 140001m)
				{
					result = "3";
				}
			}
		}
		return result;
	}

	private static DataTable GetDataToChar4(M1Database database, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT TOP 1 lnrSalaryAmount FROM EmployeePayRates WHERE lnrEmployeeID = @EmployeeID ORDER BY lnrStartDate DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		return database.GetDataTable(sqlCommand);
	}

	private static string GetChar5(M1Database database, string employeeId, string fullMedicareLevyExemption, string halfMedicareLevyExemption, string firstChar, string fourthChar)
	{
		string result = string.Empty;
		if (!firstChar.Equals("R"))
		{
			result = "X";
		}
		else if (!fourthChar.Equals("X"))
		{
			result = "X";
		}
		else if (GetDataToChar5(database, employeeId).Rows.Count == 0)
		{
			result = "X";
		}
		else
		{
			foreach (DataRow row in GetDataToChar5(database, employeeId).Rows)
			{
				if (row.Field<string>("pazDescription").ToUpper().Equals(fullMedicareLevyExemption.ToUpper()))
				{
					result = "F";
					break;
				}
				if (row.Field<string>("pazDescription").ToUpper().Equals(halfMedicareLevyExemption.ToUpper()))
				{
					result = "H";
					break;
				}
				result = "X";
			}
		}
		return result;
	}

	private static DataTable GetDataToChar5(M1Database database, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT pazDescription FROM EmployeeIncomeTaxes JOIN IncomeTaxTables on pazIncomeTaxTableID = pamIncomeTaxTableID WHERE pamInactive = 0 AND pamEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		return database.GetDataTable(sqlCommand);
	}

	private static string GetChar6(M1Database database, string halfMedicareLevyExemption, string employeeId, string firstChar, string fifthChar)
	{
		string result = string.Empty;
		if (!firstChar.Equals("R"))
		{
			result = "X";
		}
		else if (fifthChar.Equals("H"))
		{
			if (GetDataToChar6(database, halfMedicareLevyExemption, employeeId).Rows.Count == 0)
			{
				result = "X";
			}
			else
			{
				foreach (DataRow row in GetDataToChar6(database, halfMedicareLevyExemption, employeeId).Rows)
				{
					result = ((row.ItemArray[1].ToString().Equals("M") && int.Parse(row.ItemArray[0].ToString()) == 0) ? "0" : ((!row.ItemArray[1].ToString().Equals("M") && int.Parse(row.ItemArray[0].ToString()) == 0) ? "X" : ((int.Parse(row.ItemArray[0].ToString()) >= 10) ? ((int.Parse(row.ItemArray[0].ToString()) < 10) ? "X" : "A") : row.ItemArray[0].ToString())));
				}
			}
		}
		else
		{
			result = "X";
		}
		return result;
	}

	private static DataTable GetDataToChar6(M1Database database, string halfMedicareLevyExemption, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT eit.pamDependentExemptions, epd.lmdMaritalStatus FROM EmployeeIncomeTaxes eit INNER JOIN IncomeTaxTables itt ON itt.pazIncomeTaxTableID = eit.pamIncomeTaxTableID AND itt.pazIncomeTaxTypeID = eit.pamIncomeTaxTypeID AND itt.pazIncomeTaxID = eit.pamIncomeTaxID INNER JOIN EmployeePersonalData epd ON epd.lmdEmployeeID = eit.pamEmployeeID WHERE itt.pazIncomeTaxID = 'AFIT' AND itt.pazDescription IN (@HalfMedicareLevyExemption) AND eit.pamInactive = 0 AND eit.pamEmployeeID = @EmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@HalfMedicareLevyExemption", SqlDbType.NVarChar)).Value = halfMedicareLevyExemption;
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		return database.GetDataTable(sqlCommand);
	}

	public static decimal TotalChildSupportGarnishees(M1Database database, int sessionId, int taxYear)
	{
		return CalculateChildSupportAmount(database, sessionId, taxYear, "G");
	}

	public static decimal TotalChildSupportDeductions(M1Database database, int sessionId, int taxYear)
	{
		return CalculateChildSupportAmount(database, sessionId, taxYear, "D");
	}

	private static decimal CalculateChildSupportAmount(M1Database database, int sessionId, int taxYear, string deductionType)
	{
		double num = 0.0;
		double num2 = 0.0;
		DataRow currentSession = GetCurrentSession(database, sessionId, taxYear);
		if (currentSession != null)
		{
			bool flag = currentSession.Field<bool>("stpSTPSubmitted");
			bool flag2 = currentSession.Field<bool>("stpFullFileReplacement");
			bool flag3 = currentSession.Field<bool>("stpSTPFFRSubmitted");
			if (!flag || (flag2 && !flag3))
			{
				int sessionID = currentSession.Field<int>("stpSessionID");
				double num3 = AmountChildSupport(database, sessionID, deductionType, flag, taxYear);
				num += num3;
			}
		}
		DataRow lastSession = GetLastSession(database, taxYear);
		if (lastSession != null)
		{
			int num4 = lastSession.Field<int>("stpSessionID");
			bool isSubmitted = lastSession.Field<bool>("stpSTPSubmitted");
			if (sessionId.Equals(num4))
			{
				double num5 = AmountChildSupport(database, num4, deductionType, isSubmitted, taxYear);
				num2 += num5;
			}
			else if (currentSession != null)
			{
				double num6 = AmountChildSupport(database, num4, deductionType, isSubmitted, taxYear);
				num2 += num6;
			}
		}
		return (decimal)(num - num2);
	}

	public static DataRow GetCurrentSession(M1Database database, int sessionID, int taxYear)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT stpSessionID, stpSTPSubmitted, stpFullFileReplacement, stpSTPFFRSubmitted FROM STPSessions WHERE stpSessionID = @SessionId AND stpTaxYear = @TaxYear");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int)).Value = sessionID;
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count > 0)
		{
			return rows[0];
		}
		return null;
	}

	public static DataRow GetLastSession(M1Database database, int taxYear)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT TOP 1 stpSessionID, stpSTPSubmitted FROM STPSessions WHERE stpSTPSubmitted = 1 AND stpTaxYear = @TaxYear AND (stpFullFileReplacement = 0 OR (stpFullFileReplacement = 1 AND stpSTPFFRSubmitted = 1)) ORDER BY stpSessionID DESC");
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count > 0)
		{
			return rows[0];
		}
		return null;
	}

	public static double AmountChildSupport(M1Database database, int sessionID, string deductionType, bool isSubmitted, int taxYear)
	{
		double result = 0.0;
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT stdDeductionType, ISNULL(SUM(CAST(stdPayeeDeductionAmount AS FLOAT)), 0) AS totalPayerDeductionChildSupport FROM STPSessions INNER JOIN STPDeductions ON stdSessionID = stpSessionID WHERE stpSessionID = @SessionId AND stdDeductionType = @DeductionType AND stdSTPSubmitted = @IsSubmitted AND stpTaxYear = @TaxYear GROUP BY stdDeductionType");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int)).Value = sessionID;
		sqlCommand.Parameters.Add(new SqlParameter("@DeductionType", SqlDbType.NVarChar)).Value = deductionType;
		sqlCommand.Parameters.Add(new SqlParameter("@IsSubmitted", SqlDbType.Bit)).Value = isSubmitted;
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count > 0)
		{
			result = rows[0].Field<double>("totalPayerDeductionChildSupport");
		}
		return result;
	}

	public static DataTable EmployeeStpLine(M1Database database, string employeeId, bool isWorkingHolidayMaker, int taxYear)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT stlSessionID, stlLineID, stlEmployeeID, stlGrossPayments, stlWorkingHolidayGrossPay, stlTotalINBPAYGWAmount, stlWorkingHolidayPAYGWAmount, stlWorkingHolidayMaker\r\n                                                FROM STPSessions \r\n                                                INNER JOIN STPLines on stlSessionID = stpSessionID \r\n                                                WHERE stlWorkingHolidayMaker = @IsWorkingHolidayMaker \r\n                                                AND stpTaxYear = @TaxYear \r\n                                                AND stlEmployeeID = @EmployeeId");
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.NVarChar)).Value = employeeId;
		sqlCommand.Parameters.Add(new SqlParameter("@IsWorkingHolidayMaker", SqlDbType.Bit)).Value = isWorkingHolidayMaker;
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		return database.GetDataTable(sqlCommand);
	}

	public static decimal TotalPayeeGrossAmount(M1Database database, int sessionId, string employeeId, bool isWorkingHolidayMaker, int taxYear)
	{
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT patPayrollEmployeeID, SUM(ISNULL((CASE WHEN payPayType = 'S' THEN pagSubTotal ELSE CASE WHEN payPayType = 'R' AND payLeaveType = '' THEN pagSubTotal ELSE 0 END END), 0)) AS AusGrossPayments \r\n                                                FROM PayrollSessions \r\n                                                INNER JOIN PayrollHeaders ON patPayrollSessionID = pasPayrollSessionID \r\n                                                INNER JOIN PayrollHeaderTotals ON pagPayrollSessionID = patPayrollSessionID AND pagPayrollHeaderID = patPayrollHeaderID \r\n                                                INNER JOIN PayrollRates ON payPayrollRateID = pagPayrollRateID \r\n                                                LEFT JOIN STPLines ON stlSessionID = pasSTPSessionID AND stlEmployeeID = patPayrollEmployeeID  \r\n                                                WHERE pasPostedToGL <> 0 AND pasTaxYear = @TaxYear \r\n                                                AND pagAusLumpSumType = '' \r\n                                                AND pasSTPSessionID <> @SessionId \r\n                                                AND patPayrollEmployeeID = @EmployeeId\r\n                                                AND stlWorkingHolidayMaker = @IsWorkingHolidayMaker \r\n                                                AND pagAusIsETP <> 1\r\n                                                GROUP BY patPayrollEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.NVarChar)).Value = employeeId;
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		sqlCommand.Parameters.Add(new SqlParameter("@IsWorkingHolidayMaker", SqlDbType.Bit)).Value = isWorkingHolidayMaker;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count != 0)
		{
			num = rows[0].Field<decimal>("AusGrossPayments");
		}
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT ISNULL(SUM(panAmount),0) As panAmount, ISNULL(SUM(panAppliedPayAmount),0) As panAppliedPayAmount\r\n                                                                    FROM PayrollSessions\r\n                                                                    INNER JOIN PayrollHeaders on pasPayrollSessionID = patPayrollSessionID\r\n                                                                    INNER JOIN PayrollLines on patPayrollSessionID = panPayrollSessionID AND patPayrollHeaderID = panPayrollHeaderID\r\n                                                                    INNER JOIN Allowances on panAllowanceID = paoAllowanceID\r\n                                                                    LEFT JOIN STPLines ON stlSessionID = pasSTPSessionID AND stlEmployeeID = patPayrollEmployeeID  \r\n                                                                    WHERE pasPostedToGL = 1\r\n                                                                    AND pasTaxYear = @TaxYear \r\n                                                                    AND pasSTPSessionID <> @SessionId \r\n                                                                    AND panPayrollLineType = 'A'\r\n                                                                    AND paoIncludeInGrossPAYG = 1 \r\n                                                                    AND patPayrollEmployeeID = @EmployeeId\r\n                                                                    AND stlWorkingHolidayMaker = @IsWorkingHolidayMaker \r\n                                                                    GROUP BY patPayrollEmployeeID");
		sqlCommand2.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int)).Value = sessionId;
		sqlCommand2.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.NVarChar)).Value = employeeId;
		sqlCommand2.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		sqlCommand2.Parameters.Add(new SqlParameter("@IsWorkingHolidayMaker", SqlDbType.Bit)).Value = isWorkingHolidayMaker;
		DataRowCollection rows2 = database.GetDataTable(sqlCommand2).Rows;
		if (rows2.Count != 0)
		{
			num2 = rows2[0].Field<decimal>("panAmount");
		}
		return num + num2;
	}

	public static decimal TotalPayeeTotalPayGwAmount(M1Database database, int sessionId, string employeeId, bool isWorkingHolidayMaker, int taxYear)
	{
		decimal result = default(decimal);
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT patPayrollEmployeeID, SUM(ISNULL(CASE WHEN pafTaxCategory = 'T' AND paxTaxAuthority = 1 THEN panAmount ELSE 0 END, 0)) AS AusTotalTaxWithheld\r\n                                                FROM PayrollSessions \r\n                                                INNER JOIN PayrollHeaders on patPayrollSessionID = pasPayrollSessionID \r\n                                                INNER JOIN PayrollLines on panPayrollSessionID = patPayrollSessionID AND panPayrollHeaderID = patPayrollHeaderID \r\n                                                INNER JOIN IncomeTaxes on paxIncomeTaxID = panIncomeTaxID \r\n                                                INNER JOIN IncomeTaxTypes on pafIncomeTaxID = paxIncomeTaxID AND pafIncomeTaxTypeID = panIncomeTaxTypeID \r\n                                                LEFT JOIN STPLines ON stlSessionID = pasSTPSessionID AND stlEmployeeID = patPayrollEmployeeID  \r\n                                                WHERE pasPostedToGL <> 0 AND pasTaxYear = @TaxYear \r\n                                                AND panPayrollLineType = 'E' AND panAusETPCode = '' \r\n                                                AND pasSTPSessionID <> @SessionId\r\n                                                AND patPayrollEmployeeID = @EmployeeId\r\n                                                AND stlWorkingHolidayMaker = @IsWorkingHolidayMaker \r\n                                                GROUP BY patPayrollEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeId", SqlDbType.NVarChar)).Value = employeeId;
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = taxYear;
		sqlCommand.Parameters.Add(new SqlParameter("@IsWorkingHolidayMaker", SqlDbType.Bit)).Value = isWorkingHolidayMaker;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count != 0)
		{
			return rows[0].Field<decimal>("AusTotalTaxWithheld");
		}
		return result;
	}
}
