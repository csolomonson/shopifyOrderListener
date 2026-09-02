using System;
using System.Data;
using System.Data.SqlClient;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class Financial
{
	public bool IsPaypalActivated(M1Database database)
	{
		return false;
	}

	public bool IsNET1Activated(M1Database database)
	{
		DataTable dataTable = database.GetDataTable("SELECT xafARNET1GatewayID, xafARNET1MerchantKey, xafCreditCardMethod FROM FinancialProperties");
		if (dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<string>("xafARNET1GatewayID").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafARNET1MerchantKey").Trim().Length > 0 && dataTable.Rows[0].Field<byte>("xafCreditCardMethod") == 2)
		{
			return true;
		}
		return false;
	}

	public bool IsAvalaraActivated(M1Database database)
	{
		if (database.Security.IsInRole("CUSTOMMODULE:5"))
		{
			DataTable dataTable = database.GetDataTable("SELECT xafAvalaraAccountID, xafAvalaraURL, xafAvalaraCompanyCode, xafAvalaraLicenseKey FROM FinancialProperties");
			if (dataTable.Rows.Count != 0 && dataTable.Rows[0].Field<string>("xafAvalaraAccountID").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraURL").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraCompanyCode").Trim().Length > 0 && dataTable.Rows[0].Field<string>("xafAvalaraLicenseKey").Trim().Length > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void GetFiscalYearDates(M1Database database, DateTime? dateToCheck, ref DateTime startDate, ref DateTime endDate)
	{
		if (!dateToCheck.HasValue)
		{
			dateToCheck = DateTime.Today;
		}
		startDate = new DateTime(DateTime.Today.Year, 1, 1);
		endDate = new DateTime(DateTime.Today.Year, 12, 31);
		SqlCommand sqlCommand = database.NewSqlCommand("select top 1 glzStartDate,glzEndDate from GLFiscalYears where glzStartDate <= @QueryDate and glzEndDate >= @QueryDate");
		sqlCommand.Parameters.Add(new SqlParameter("@QueryDate", SqlDbType.DateTime)).Value = dateToCheck;
		DataTable dataTable = database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count != 0)
		{
			if (dataTable.Rows[0]["glzStartDate"] != DBNull.Value)
			{
				startDate = dataTable.Rows[0].Field<DateTime>("glzStartDate");
			}
			if (dataTable.Rows[0]["glzEndDate"] != DBNull.Value)
			{
				endDate = dataTable.Rows[0].Field<DateTime>("glzEndDate");
			}
		}
		endDate = endDate.AddDays(1.0);
	}

	public YearAndPeriod GetYearAndPeriod(M1Database database, object Value, string Module, bool IgnoreClosed = false, SqlTransaction sqlTrans = null)
	{
		YearAndPeriod yearAndPeriod = new YearAndPeriod();
		DataTable dataTable = null;
		bool flag = false;
		if (Value == null || Value == DBNull.Value)
		{
			yearAndPeriod.Message = "Date must be specified.";
			return yearAndPeriod;
		}
		DateTime dateTime = Convert.ToDateTime(Value);
		Module = Module.ToUpper().Trim();
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT glfGLFiscalYearID, glfGLFiscalYearPeriodID, glfARClosed, glfAPClosed, glfGLClosed FROM GLFiscalYearPeriods WHERE glfStartDate <= @DateValue and glfEndDate >= @DateValue ORDER BY glfGLFiscalYearID, glfGLFiscalYearPeriodID");
		sqlCommand.Parameters.Add(new SqlParameter("@DateValue", SqlDbType.Date)).Value = dateTime;
		dataTable = database.GetDataTable(sqlCommand, sqlTrans);
		if (dataTable.Rows.Count != 0)
		{
			flag = true;
			if (!(Module == "AR"))
			{
				if (Module == "AP")
				{
					if (dataTable.Rows[0].Field<bool>("glfAPClosed"))
					{
						flag = false;
					}
				}
				else if (dataTable.Rows[0].Field<bool>("glfGLClosed"))
				{
					flag = false;
				}
			}
			else if (dataTable.Rows[0].Field<bool>("glfARClosed"))
			{
				flag = false;
			}
			if (flag)
			{
				yearAndPeriod.Year = dataTable.Rows[0].Field<short>("glfGLFiscalYearID");
				yearAndPeriod.Period = dataTable.Rows[0].Field<byte>("glfGLFiscalYearPeriodID");
				yearAndPeriod.Success = true;
				return yearAndPeriod;
			}
			yearAndPeriod.Message = "The period (" + dataTable.Rows[0].Field<short>("glfGLFiscalYearID") + "/" + dataTable.Rows[0].Field<byte>("glfGLFiscalYearPeriodID").ToSql() + ") that contains " + dateTime.ToString("d") + " is marked as closed in Fiscal Year Maintenance";
			return yearAndPeriod;
		}
		yearAndPeriod.Message = "There are no fiscal periods set up for " + dateTime.Date.ToString("d");
		return yearAndPeriod;
	}
}
