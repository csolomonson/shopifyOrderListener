using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class CsvQueries
{
	public static DataTable GetTerminationData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT * \r\n                                                FROM STPLines \r\n                                                INNER JOIN STPTerminationPayment ON sttSessionID = stlSessionID AND sttLineID = stlLineID \r\n                                                WHERE stlSessionID = @SessionID and stlLineID = @LineID \r\n                                                ORDER BY stlLineID, sttTerminationID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetAllowancesData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT * \r\n                                                FROM STPLines \r\n                                                INNER JOIN STPAllowances ON staSessionID = stlSessionID AND staLineID = stlLineID \r\n                                                WHERE stlSessionID = @SessionID and stlLineID = @LineID \r\n                                                ORDER BY stlLineID, staAllowanceID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetDeductionsData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT * \r\n                                                FROM STPLines \r\n                                                INNER JOIN STPDeductions ON stdSessionID = stlSessionID AND stdLineID = stlLineID \r\n                                                WHERE stlSessionID = @SessionID and stlLineID = @LineID \r\n                                                ORDER BY stlLineID, stdDeductionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetPaidLeavesData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT src.stlSessionID stlSessionID, src.stlLineID stlLineID, src.PaidLeavePaymentCode PaidLeavePaymentCode, src.PaidLeavePaymentAmount PaidLeavePaymentAmount \r\n                                                FROM( \r\n                                                        SELECT stlSessionID, stlLineID, 'C' PaidLeavePaymentCode, stlCashOutLeave PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlCashOutLeave <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'U' PaidLeavePaymentCode, stlUnusedLeave PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlUnusedLeave <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'P' PaidLeavePaymentCode, stlPaidParentalLeave PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPaidParentalLeave <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'W' PaidLeavePaymentCode, stlWorkersComp PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlWorkersComp <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'A' PaidLeavePaymentCode, stlAncillaryDefenceLeave PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlAncillaryDefenceLeave <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'O' PaidLeavePaymentCode, stlOtherPaidLeave PaidLeavePaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlOtherPaidLeave <> 0 \r\n                                                    ) src");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetEntitlementsData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT src.stlSessionID stlSessionID, src.stlLineID stlLineID, src.SuperEntitlementTypeCode SuperEntitlementTypeCode, src.SuperEntitlementAmount SuperEntitlementAmount \r\n                                                FROM( \r\n                                                        SELECT stlSessionID, stlLineID, 'L' SuperEntitlementTypeCode, stlSuperLiabilityAmount SuperEntitlementAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlSuperLiabilityAmount <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'O' SuperEntitlementTypeCode, stlOrdinaryTimeEarningsAmount SuperEntitlementAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID\r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'R' SuperEntitlementTypeCode, stlReportableEmpSuperContrib SuperEntitlementAmount \r\n                                                        FROM STPLines WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlReportableEmpSuperContrib <> 0 \r\n                                                    ) src");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetLumpSumsData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT src.stlSessionID stlSessionID, src.stlLineID stlLineID, src.LumpSumTypeCode LumpSumTypeCode, src.TaxYear TaxYear, src.LumpSumPaymentAmount LumpSumPaymentAmount \r\n                                                FROM( \r\n                                                        SELECT stlSessionID, stlLineID, stlPayeeLumpSumPaymentAType LumpSumTypeCode, 0 TaxYear, stlPayeeLumpSumPaymentA LumpSumPaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPayeeLumpSumPaymentA <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'B' LumpSumTypeCode, 0 TaxYear, stlPayeeLumpSumPaymentB LumpSumPaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPayeeLumpSumPaymentB <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'D' LumpSumTypeCode, 0 TaxYear, stlPayeeLumpSumPaymentD LumpSumPaymentAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPayeeLumpSumPaymentD <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'E' LumpSumTypeCode, ISNULL(s.stpTaxYear, 0) TaxYear, stlPayeeLumpSumPaymentE LumpSumPaymentAmount \r\n                                                        FROM STPLines l\t\r\n                                                        LEFT JOIN STPSessions s ON s.stpSessionID = l.stlSessionID \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPayeeLumpSumPaymentE <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'W' LumpSumTypeCode, 0 TaxYear, stlPayeeLumpSumPaymentW LumpSumPaymentAmount \r\n                                                        FROM STPLines WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlPayeeLumpSumPaymentW <> 0 \r\n                                                    ) src");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetSalarySacrificesData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT src.stlSessionID stlSessionID, src.stlLineID stlLineID, src.SalarySacrificeTypeCode SalarySacrificeTypeCode, src.SalarySacrificeAmount SalarySacrificeAmount \r\n                                                FROM( \r\n                                                        SELECT stlSessionID, stlLineID, 'S' SalarySacrificeTypeCode, stlSalarySacrificeSuper SalarySacrificeAmount \r\n                                                        FROM STPLines \r\n                                                        WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlSalarySacrificeSuper <> 0 \r\n                                                        UNION ALL \r\n                                                        SELECT stlSessionID, stlLineID, 'O' SalarySacrificeTypeCode, stlSalarySacrificeOther SalarySacrificeAmount \r\n                                                        FROM STPLines WHERE stlSessionID = @SessionID AND stlLineID = @LineID AND stlSalarySacrificeOther <> 0 \r\n                                                    ) src");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetAllowancesIncomeStreamCollectionsData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT stlSessionID, stlLineID, stlOvertimeAmount, stlBonusAmount, stlDirectorsFees, stlHomeCountry \r\n                                                FROM STPLines \r\n                                                WHERE stlSessionID = @SessionID AND stlLineID = @LineID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetWithoutPayeeServicesData(M1Database database, string sessionId, string lineId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT * \r\n                                                FROM STPLines stpl LEFT JOIN STPTerminationPayment stpt ON stpl.stlSessionID = stpt.sttSessionID AND stpl.stlLineID = stpt.sttLineID \r\n                                                LEFT JOIN STPAllowances stpa ON stpl.stlSessionID = stpa.staSessionID AND stpl.stlLineID = stpa.staLineID \r\n                                                LEFT JOIN STPDeductions stpd on stpl.stlSessionID = stpd.stdSessionID AND stpl.stlLineID = stpd.stdLineID \r\n                                                WHERE stpt.sttSessionID IS NULL AND stpt.sttLineID IS NULL AND stpa.staSessionID IS NULL AND stpa.staLineID IS NULL AND stpd.stdSessionID IS NULL \r\n                                                    AND stpd.stdLineID IS NULL AND stpl.stlSessionID = @SessionID and stpl.stlLineID = @LineID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		return database.GetDataTable(sqlCommand);
	}

	public static DataTable GetIncomeStreamTypeData(M1Database database, string sessionId, string lineId, string employeeId)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT src.stlSessionID, src.stlLineID, src.IncomeStreamTypeCode, src.GrossAmount GrossAmount, src.PaygwAmount PaygwAmount, src.stlWorkingHolidayMaker, src.stlHomeCountry, src.stlEmployeeID, src.stlTaxFileNumber, src.stlContractorABN, src.WasResident\r\n                                                FROM( \r\n                                                    SELECT stlSessionID, stlLineID, 'SAW' AS IncomeStreamTypeCode, ISNULL(SUM(stlGrossPayments),0) AS GrossAmount, ISNULL(SUM(stlTotalINBPAYGWAmount),0) AS PaygwAmount, stlWorkingHolidayMaker, '' AS stlHomeCountry, stlEmployeeID, stlTaxFileNumber, stlContractorABN, ISNULL((SELECT TOP 1 stlSessionID\r\n                                                        FROM STPLines\r\n                                                        INNER JOIN STPSessions ON stlSessionID = stpSessionID\r\n                                                        WHERE stlEmployeeID = @EmployeeID AND stlWorkingHolidayMaker = 0\r\n                                                        ORDER BY stlSessionID DESC), 0) AS WasResident\r\n                                                    FROM STPLines \r\n                                                    WHERE stlSessionID = @SessionID AND stlLineID = @LineID\r\n                                                    GROUP BY stlSessionID, stlLineID, stlEmployeeID, stlTaxFileNumber, stlContractorABN, stlWorkingHolidayMaker\r\n                                                    UNION ALL \r\n                                                    SELECT stlSessionID, stlLineID, 'WHM' AS IncomeStreamTypeCode, ISNULL(SUM(stlWorkingHolidayGrossPay),0) AS GrossAmount, ISNULL(SUM(stlWorkingHolidayPAYGWAmount),0) AS PaygwAmount, stlWorkingHolidayMaker, CASE WHEN stlHomeCountry = '' THEN ISNULL((SELECT TOP 1 stlHomeCountry\r\n                                                        FROM STPLines\r\n                                                        INNER JOIN STPSessions ON stlSessionID = stpSessionID\r\n                                                        WHERE stlEmployeeID = @EmployeeID AND stlWorkingHolidayMaker = 1\r\n                                                        ORDER BY stlSessionID DESC), '') ELSE stlHomeCountry END AS stlHomeCountry, stlEmployeeID, stlTaxFileNumber, stlContractorABN, 0 AS WasResident\r\n                                                    FROM STPLines \r\n                                                    WHERE stlSessionID = @SessionID AND stlLineID = @LineID \r\n                                                    GROUP BY stlSessionID, stlLineID, stlEmployeeID, stlTaxFileNumber, stlContractorABN, stlWorkingHolidayMaker, stlHomeCountry\r\n                                                ) src");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.NVarChar)).Value = sessionId;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.NVarChar)).Value = lineId;
		sqlCommand.Parameters.Add(new SqlParameter("@EmployeeID", SqlDbType.NVarChar)).Value = employeeId;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		foreach (DataRow row in dataTable.Rows)
		{
			string text = row.Field<string>("IncomeStreamTypeCode").Trim();
			string value = row.Field<string>("stlHomeCountry").Trim();
			int num = row.Field<int>("WasResident");
			if (text == "WHM" && string.IsNullOrEmpty(value))
			{
				row.Delete();
			}
			if (text == "SAW" && num == 0)
			{
				row.Delete();
			}
		}
		dataTable.AcceptChanges();
		return dataTable;
	}
}
