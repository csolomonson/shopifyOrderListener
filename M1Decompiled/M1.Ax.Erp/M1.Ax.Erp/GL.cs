using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class GL
{
	protected enum ImportFileType
	{
		WinPay = 1,
		RedWing
	}

	public enum ImportLogDataType
	{
		Error = 1,
		Warning,
		Information
	}

	[ComVisible(true)]
	public class ImportLogData
	{
		public ImportLogDataType LogDataType { get; set; }

		public string Description { get; set; }

		public ImportLogData()
		{
		}

		public ImportLogData(ImportLogDataType logType, string description)
		{
			LogDataType = logType;
			Description = description;
		}
	}

	private DataTable dtM1AccountsData = new DataTable();

	private byte GLReferenceColumnIndex;

	private byte GLExtAccountCodeColumnIndex = 1;

	private byte GLTransactionDateColumnIndex = 2;

	private byte GLDescriptionColumnIndex = 3;

	private byte GLTransactionAmountColumnIndex = 5;

	public decimal GetAccountBalance(M1Database database, string accountID)
	{
		if (!string.IsNullOrWhiteSpace(accountID))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select isnull((select glyYearOpeningBalance from GLFiscalYearOpeningBalances Where glyGLAccountID = @AccountID And glyGLFiscalYearID = @Year),0) + isnull((select sum(gllTransactionAmount) from GLJournalLines Inner Join GLJournals On gllGLJournalID = glpGLJournalID where gllGLAccountID = @AccountID and glpGLFiscalYearID >= @Year),0) + isnull((select sum(glePaymentAmount) from BankEntries Inner Join BankStatements On gleBankStatementID=glsBankStatementID Where gleEntryType = 3 and glePostedToGL = 0 and gleDoNotUpdateGL = 0 and glsCashGLAccountID = @AccountID),0) as balance");
			sqlCommand.Parameters.Add(new SqlParameter("@AccountID", SqlDbType.NVarChar)).Value = accountID;
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = database.Props("FN").Field<short>("xafGLFiscalYearID");
			return Convert.ToDecimal(database.ExecuteScalar(sqlCommand));
		}
		return 0m;
	}

	public void ClosePeriod(M1Database database)
	{
		short num = database.Props("GL").Field<short>("xafGLFiscalYearID");
		byte b = database.Props("GL").Field<byte>("xafGLFiscalYearPeriodID");
		string text = CanPeriodBeClosed(database, num, b);
		if (!string.IsNullOrWhiteSpace(text))
		{
			throw new M1MissingOrInvalidDataException(text.ToString());
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select Top 1 IsNull(glfGLFiscalYearPeriodID,0) from GLFiscalYearPeriods where glfGLFiscalYearID = @Year and glfGLFiscalYearPeriodID > @Period order by glfGLFiscalYearPeriodID");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
		sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = b;
		byte b2 = Convert.ToByte(database.ExecuteScalar(sqlCommand));
		if (b2 == 0)
		{
			throw new M1MissingOrInvalidDataException("The next period has not been defined in the fiscal periods table.");
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			processClosePeriod(database, sqlTransaction, num, b, num, b2);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
		database.PropsRefresh();
	}

	private void processClosePeriod(M1Database database, SqlTransaction transaction, short year, byte period, short nextYear, byte nextPeriod)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("UPDATE GLFiscalYearPeriods SET glfARClosed = 1, glfAPClosed = 1, glfGLClosed = 1 WHERE glfGLFiscalYearID = @Year AND glfGLFiscalYearPeriodID = @Period");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
		sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = period;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("UPDATE FinancialProperties SET xafGLFiscalYearID = @Year, xafGLFiscalYearPeriodID = @Period");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = nextYear;
		sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = nextPeriod;
		database.ExecuteCommand(sqlCommand, transaction);
	}

	public void CloseYear(M1Database database)
	{
		short num = database.Props("GL").Field<short>("xafGLFiscalYearID");
		byte b = database.Props("GL").Field<byte>("xafGLFiscalYearPeriodID");
		string text = CanPeriodBeClosed(database, num, b);
		if (!string.IsNullOrWhiteSpace(text))
		{
			throw new M1MissingOrInvalidDataException(text.ToString());
		}
		short num2 = (short)(num + 1);
		SqlCommand sqlCommand = database.NewSqlCommand("select Top 1 IsNull(glfGLFiscalYearPeriodID,0) from GLFiscalYearPeriods where glfGLFiscalYearID = @Year and glfGLFiscalYearPeriodID > @Period order by glfGLFiscalYearPeriodID");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num;
		sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = b;
		if (Convert.ToByte(database.ExecuteScalar(sqlCommand)) != 0)
		{
			throw new M1MissingOrInvalidDataException("You must close the last period of the current year before you may close the year.");
		}
		sqlCommand = database.NewSqlCommand("select Top 1 IsNull(glfGLFiscalYearPeriodID,0) from GLFiscalYearPeriods where glfGLFiscalYearID = @Year order by glfGLFiscalYearPeriodID");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = num2;
		byte b2 = Convert.ToByte(database.ExecuteScalar(sqlCommand));
		if (b2 == 0)
		{
			throw new M1MissingOrInvalidDataException($"No fiscal periods have been set up for year {num2}. Please set up your fiscal periods for the new year before closing the current year.");
		}
		SqlTransaction sqlTransaction = database.BeginTransaction();
		try
		{
			GenerateYearOpeningBalances(database, sqlTransaction, num2);
			processClosePeriod(database, sqlTransaction, num, b, num2, b2);
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
		database.PropsRefresh();
	}

	public void GenerateYearOpeningBalances(M1Database database, SqlTransaction transaction, short year)
	{
		string value = database.Props("GL").Field<string>("xafGLRetainedEarningsAccountID");
		SqlCommand sqlCommand;
		if (string.IsNullOrWhiteSpace(value))
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (DataRow row in database.GetDataTable("Select glvGLDivisionID From GLDivisions Where glvRetainedEarningsAccountID = ''", transaction).Rows)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(row.Field<string>("glvGLDivisionID"));
			}
			if (stringBuilder.Length != 0)
			{
				throw new M1MissingOrInvalidDataException($"No retained earnings account has been set up in the Financial Properties, and Division {stringBuilder.ToString()} does not have Retained Earnings Account set up.\r\nPlease select a valid retained earnings account before running this option again.");
			}
		}
		else
		{
			sqlCommand = database.NewSqlCommand("select glaGLAccountID from GLAccounts where glaGLAccountID = @Account");
			sqlCommand.Parameters.Add(new SqlParameter("@Account", SqlDbType.NVarChar)).Value = value;
			if (string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand, transaction))))
			{
				throw new M1MissingOrInvalidDataException("An invalid retained earnings account has been set up in the Financial Properties. Please select a valid retained earnings account before running this option again.");
			}
		}
		sqlCommand = database.NewSqlCommand("DELETE FROM GLFiscalYearOpeningBalances WHERE glyGLFiscalYearID = @Year");
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("INSERT INTO GLFiscalYearOpeningBalances (glyGLAccountID,glyGLFiscalYearID,glyYearOpeningBalance) SELECT glyGLAccountID,glyGLFiscalYearID+1 as glyGLFiscalYearID,glyYearOpeningBalance FROM GLFiscalYearOpeningBalances INNER JOIN GLAccounts ON glyGLAccountID=glaGLAccountID INNER JOIN GLCharts ON glaGLChartID=glcGLChartID WHERE glyGLFiscalYearID = @PreviousYear AND glcAccountType = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@PreviousYear", SqlDbType.SmallInt)).Value = year - 1;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("UPDATE GLFiscalYearOpeningBalances SET glyYearOpeningBalance = glyYearOpeningBalance + ISNULL((SELECT SUM(gllTransactionAmount) FROM GLJournalLines WHERE gllGLAccountID=glyGLAccountID And gllPosted = 1 AND gllGLFiscalYearID=@PreviousYear),0) FROM GLFiscalYearOpeningBalances LEFT OUTER JOIN GLAccounts ON glyGLAccountID=glaGLAccountID LEFT OUTER JOIN GLCharts ON glaGLChartID=glcGLChartID WHERE glyGLFiscalYearID = @Year AND glcAccountType = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@PreviousYear", SqlDbType.SmallInt)).Value = year - 1;
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("INSERT INTO GLFiscalYearOpeningBalances (glyGLAccountID,glyGLFiscalYearID,glyYearOpeningBalance) SELECT gllGLAccountID as glyGLAccountID,@Year as glyGLFiscalYearID,SUM(gllTransactionAmount) as glyYearOpeningBalance FROM GLJournalLines INNER JOIN GLAccounts ON gllGLAccountID=glaGLAccountID INNER JOIN GLCharts ON glaGLChartID=glcGLChartID WHERE gllGLFiscalYearID = @PreviousYear AND glcAccountType = 1 And gllPosted = 1 AND gllGLAccountID NOT IN (SELECT glyGLAccountID FROM GLFiscalYearOpeningBalances WHERE glyGLFiscalYearID = @Year) GROUP BY gllGLAccountID");
		sqlCommand.Parameters.Add(new SqlParameter("@PreviousYear", SqlDbType.SmallInt)).Value = year - 1;
		sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
		database.ExecuteCommand(sqlCommand, transaction);
		sqlCommand = database.NewSqlCommand("SELECT (Case When glvRetainedEarningsAccountID <> '' Then glvRetainedEarningsAccountID Else xafGLRetainedEarningsAccountID End) As GLAccount,SUM(gllTransactionAmount) AS Amount FROM GLJournalLines INNER JOIN GLAccounts ON gllGLAccountID=glaGLAccountID INNER JOIN GLDivisions ON glaGLDivisionID = glvGLDivisionID INNER JOIN GLCharts ON glaGLChartID=glcGLChartID, FinancialProperties WHERE gllGLFiscalYearID = @PreviousYear AND glcAccountType = 2 And gllPosted = 1 Group By (Case When glvRetainedEarningsAccountID <> '' Then glvRetainedEarningsAccountID Else xafGLRetainedEarningsAccountID End)");
		sqlCommand.Parameters.Add(new SqlParameter("@PreviousYear", SqlDbType.SmallInt)).Value = year - 1;
		SqlCommand sqlCommand2 = database.NewSqlCommand("SELECT * FROM GLFiscalYearOpeningBalances WHERE glyGLFiscalYearID = @Year AND glyGLAccountID = @Account");
		sqlCommand2.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
		sqlCommand2.Parameters.Add(new SqlParameter("@Account", SqlDbType.NVarChar));
		foreach (DataRow row2 in database.GetDataTable(sqlCommand, transaction).Rows)
		{
			sqlCommand2.Parameters["@Account"].Value = row2.Field<string>("GLAccount");
			SqlDataAdapter adapter;
			DataTable dataTable = database.GetDataTable(sqlCommand2, fillSchema: false, out adapter, transaction);
			DataRow dataRow2;
			if (dataTable.Rows.Count == 0)
			{
				dataRow2 = dataTable.NewRow().BlankRow();
				dataRow2["glyGLAccountID"] = row2["glAccount"];
				dataRow2["glyGLFiscalYearID"] = year;
				dataTable.Rows.Add(dataRow2);
			}
			dataRow2 = dataTable.Rows[0];
			dataRow2["glyYearOpeningBalance"] = dataRow2.Field<decimal>("glyYearOpeningBalance") + row2.Field<decimal>("amount");
			database.UpdateData(dataTable, adapter, transaction);
		}
	}

	public string CanPeriodBeClosed(M1Database database, short year, byte period)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (year > 0 && period > 0)
		{
			SqlCommand sqlCommand = database.NewSqlCommand("select count(*) as rec_count from ARInvoices where arpGLFiscalYearID = @Year and arpGLFiscalYearPeriodID = @Period and arpPostedToGL = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
			sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = period;
			int num = Convert.ToInt32(database.ExecuteScalar(sqlCommand));
			if (num != 0)
			{
				stringBuilder.AppendFormat("There are {0} open AR Invoice(s) for the current period.\r\n", num);
			}
			sqlCommand = database.NewSqlCommand("select count(*) as rec_count from ARPaymentSessions where arsGLFiscalYearID = @Year and arsGLFiscalYearPeriodID = @Period and arsPostedToGL = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
			sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = period;
			num = Convert.ToInt32(database.ExecuteScalar(sqlCommand));
			if (num != 0)
			{
				stringBuilder.AppendFormat("There are {0} open AR Payment Session(s) for the current period.\r\n", num);
			}
			sqlCommand = database.NewSqlCommand("select count(*) as rec_count from APInvoices where appGLFiscalYearID = @Year and appGLFiscalYearPeriodID = @Period and appPostedToGL = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
			sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = period;
			num = Convert.ToInt32(database.ExecuteScalar(sqlCommand));
			if (num != 0)
			{
				stringBuilder.AppendFormat("There are {0} open AP Invoice(s) for the current period.\r\n", num);
			}
			sqlCommand = database.NewSqlCommand("select count(*) as rec_count from APPaymentSessions where apsGLFiscalYearID = @Year and apsGLFiscalYearPeriodID = @Period and apsPostedToGL = 0");
			sqlCommand.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt)).Value = year;
			sqlCommand.Parameters.Add(new SqlParameter("@Period", SqlDbType.TinyInt)).Value = period;
			num = Convert.ToInt32(database.ExecuteScalar(sqlCommand));
			if (num != 0)
			{
				stringBuilder.AppendFormat("There are {0} open AP Payment Session(s) for the current period.\r\n", num);
			}
		}
		else
		{
			if (year <= 0)
			{
				stringBuilder.AppendFormat("The year is invalid\r\n");
			}
			if (period <= 0)
			{
				stringBuilder.AppendFormat("The period is invalid\r\n");
			}
		}
		return stringBuilder.ToString();
	}

	public bool ValidateSelectedJournals(M1Database database, SqlTransaction transaction, string journalList, string sqlFilter, bool showMessage, bool forceNoMessage)
	{
		bool result = false;
		bool flag = false;
		bool flag2 = false;
		string text = string.Empty;
		string text2 = string.Empty;
		int num = 0;
		journalList = journalList.ToString().Trim();
		if (!(journalList == ""))
		{
			journalList = ((!journalList.Contains(",")) ? (" and glpGLJournalID = " + journalList) : (" and glpGLJournalID IN (" + journalList + ")"));
		}
		sqlFilter = sqlFilter.ToString().Trim();
		if (sqlFilter != "")
		{
			sqlFilter = " (" + sqlFilter + ") ";
			journalList = journalList + " AND " + sqlFilter;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select count(*) as rec_Count from GLJournals where glpPosted = 0 " + journalList);
		int num2 = Convert.ToInt32(database.ExecuteScalar(sqlCommand, transaction));
		if (num2 == 0)
		{
			MessageBox.Show("There are no pending journal entries to update.", "Confirm Validation Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			sqlCommand = database.NewSqlCommand("select gllGLJournalID,gllGLJournalLineID from GLJournalLines inner join GLJournals on gllGLJournalID = glpGLJournalID where gllPosted = 0 " + journalList + " and (gllGLAccountID='' or gllGLAccountID not in (select glaGLAccountID from GLAccounts))");
			foreach (DataRow row in database.GetDataTable(sqlCommand, transaction).Rows)
			{
				num++;
				text2 = text2 + row["gllGLJournalID"].ToString().Trim() + "-" + row["gllGLJournalLineID"].ToString().Trim() + ", ";
			}
			if (text2 != "" && text2.Substring(text2.Length - 2) == ", ")
			{
				text2 = text2.Substring(0, text2.Length - 2);
				text = text + "\n\n***Error***\nThe following " + num.ToString().Trim() + " journal line(s) have empty or invalid accounts: " + text2 + ".";
				flag2 = true;
				flag = true;
			}
			sqlCommand = database.NewSqlCommand("select gllGLJournalID from(select gllGLJournalID, sum(gllTransactionAmount) as AmountTotal from GLJournalLines inner join GLJournals on gllGLJournalID = glpGLJournalID where gllPosted = 0 " + journalList + " group by gllGLJournalID) As Test where AmountTotal <> 0");
			text2 = string.Empty;
			num = 0;
			foreach (DataRow row2 in database.GetDataTable(sqlCommand, transaction).Rows)
			{
				num++;
				text2 = text2 + row2["gllGLJournalID"].ToString().Trim() + ", ";
			}
			if (text2 != "" && text2.Substring(text2.Length - 2) == ", ")
			{
				text2 = text2.Substring(0, text2.Length - 2);
				text = text + "\n\n***Error***\nThe following " + num.ToString().Trim() + " journal(s) are not in balance: " + text2 + ".";
				flag2 = true;
				flag = true;
			}
			sqlCommand = database.NewSqlCommand("select gllGLJournalID,gllGLJournalLineID from GLJournalLines inner join GLJournals on gllGLJournalID = glpGLJournalID where gllPosted = 0 " + journalList + " and gllDescription = '' ");
			text2 = string.Empty;
			foreach (DataRow row3 in database.GetDataTable(sqlCommand, transaction).Rows)
			{
				text2 = text2 + row3["gllGLJournalID"].ToString().Trim() + "-" + row3["gllGLJournalLineID"].ToString().Trim() + ", ";
			}
			if (text2 != "" && text2.Substring(text2.Length - 2) == ", ")
			{
				text2 = text2.Substring(0, text2.Length - 2);
				text = text + "\n\n***Error***\nThe following journal lines(s) have empty descriptions: " + text2 + ".";
				flag2 = true;
				flag = true;
			}
			sqlCommand = database.NewSqlCommand("select glpGLJournalID,glpReference,glpDescription,glpGLFiscalYearID,glpGLFiscalYearPeriodID,glpSource,glfGLFiscalYearID,glfGLFiscalYearPeriodID,glfGLClosed from GLJournals left outer join GLFiscalYearPeriods on glpGLFiscalYearID = glfGLFiscalYearID and glpGLFiscalYearPeriodID = glfGLFiscalYearPeriodID where glpPosted = 0 " + journalList);
			if (!string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand, transaction))))
			{
				foreach (DataRow row4 in database.GetDataTable(sqlCommand, transaction).Rows)
				{
					if (!row4.IsNull("glfGLFiscalYearPeriodID"))
					{
						if (row4.Field<bool>("glfGLClosed"))
						{
							text2 = text2 + row4["glpGLJournalID"].ToString().Trim() + ", ";
						}
					}
					else
					{
						text2 += (text2 = text2 + row4["glpGLJournalID"].ToString().Trim() + ", ");
					}
				}
				if (text2 != "" && text2.Substring(text2.Length - 2) == ", ")
				{
					text2 = text2.Substring(0, text2.Length - 2);
					text = text + "/n/n***Error***/nThe following journal(s) are for a fiscal period that is either closed or does not exist: " + text2 + ".";
					flag2 = true;
					flag = true;
				}
			}
			sqlCommand = database.NewSqlCommand("select glpGLJournalID,glpReference,glpDescription,glpGLFiscalYearID,glpGLFiscalYearPeriodID,glpSource from GLJournals where glpPosted = 0 and glpReversingEntry = 1 " + journalList);
			if (!string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand, transaction))))
			{
				SqlCommand sqlCommand2 = database.NewSqlCommand("select glfGLFiscalYearID,glfGLFiscalYearPeriodID,glfStartDate,glfGLClosed from GLFiscalYearPeriods order by glfGLFiscalYearID,glfGLFiscalYearPeriodID");
				if (!string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand2, transaction))))
				{
					text2 = string.Empty;
					DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
					DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
					dataTable2.PrimaryKey = new DataColumn[2]
					{
						dataTable2.Columns["glfGLFiscalYearID"],
						dataTable2.Columns["glfGLFiscalYearPeriodID"]
					};
					foreach (DataRow row5 in dataTable.Rows)
					{
						object[] keys = new object[2]
						{
							row5["glpGLFiscalYearID"],
							row5["glpGLFiscalYearPeriodID"]
						};
						DataRow dataRow6 = dataTable2.Rows.Find(keys);
						if (!(dataRow6.ToString().Trim() != string.Empty))
						{
							continue;
						}
						int num3 = dataTable2.Rows.IndexOf(dataRow6);
						DataRow dataRow7 = dataTable2.Rows[++num3];
						if (!dataRow7.IsNull("glfGLFiscalYearPeriodID"))
						{
							if (dataRow7.Field<bool>("glfGLClosed"))
							{
								text2 = text2 + row5["glpGLJournalID"].ToString().Trim() + ", ";
							}
						}
						else
						{
							text2 += (text2 = text2 + row5["glpGLJournalID"].ToString().Trim() + ", ");
						}
					}
					if (text2 != "" && text2.Substring(text2.Length - 2) == ", ")
					{
						text2 = text2.Substring(0, text2.Length - 2);
						text = text + "/n/n***Error***/nThe following journal(s) are marked as Reversing entries but the next fiscal period is either closed or does not exists: " + text2 + ".";
						flag2 = true;
						flag = true;
					}
				}
			}
			short num4 = database.Props("GL").Field<short>("xafGLFiscalYearID");
			byte b = database.Props("GL").Field<byte>("xafGLFiscalYearPeriodID");
			int num5 = 0;
			sqlCommand = database.NewSqlCommand("select count(*) as rec_Count from GLJournals where glpPosted = 0 and glpGLFiscalYearID < @CurrentYear " + journalList);
			sqlCommand.Parameters.Add(new SqlParameter("@CurrentYear", SqlDbType.SmallInt)).Value = num4;
			num5 = Convert.ToInt32(database.ExecuteScalar(sqlCommand, transaction));
			if (num5 > 0)
			{
				string accountID = database.Props("GL").Field<string>("xafGLRetainedEarningsAccountID");
				string accountDescription = GetAccountDescription(database, transaction, accountID);
				text = text + "\n\n***Warning***\nYou are updating " + num5.ToString().Trim() + " entries to a previous year. Changes to income statement accounts will be updated to Retained Earnings Account " + FormatAccount(accountID) + ", " + accountDescription + ".";
				flag = true;
			}
			sqlCommand = database.NewSqlCommand("select count(*) as rec_Count from GLJournals where glpPosted = 0 and not (glpGLFiscalYearID = @CurrentYear and glpGLFiscalYearPeriodID = @CurrentPeriod) " + journalList);
			sqlCommand.Parameters.Add(new SqlParameter("@CurrentYear", SqlDbType.SmallInt)).Value = num4;
			sqlCommand.Parameters.Add(new SqlParameter("@CurrentPeriod", SqlDbType.SmallInt)).Value = b;
			int num6 = Convert.ToInt32(database.ExecuteScalar(sqlCommand, transaction));
			if (num6 > 0)
			{
				text = text + "\n\n***Warning***\nYou are updating " + num6.ToString().Trim() + " entries to other than the current period (" + num4.ToString().Trim() + "/" + b.ToString().Trim() + ").";
				flag = true;
			}
			if (flag2)
			{
				text = "The " + num2 + " selected pending journals may not be posted for the following reasons." + text;
				if (!forceNoMessage)
				{
					MessageBox.Show(text, "Confirm Validation Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			else
			{
				text = ((num6 != 0 || num5 != 0) ? ("This will post the " + num2 + " selected pending journal entries. Once a journal has been posted, you will be unable to edit that journal. Are you sure you want to continue?" + text) : ("This will post the " + num2 + " selected pending journal entries to the current period (" + num4.ToString().Trim() + "/" + b.ToString().Trim() + "). Once a journal has been posted, you will be unable to edit that journal. Are you sure you want to continue?" + text));
				if ((flag || showMessage) && !forceNoMessage)
				{
					switch (MessageBox.Show(text, "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
					{
					case DialogResult.Yes:
						result = true;
						break;
					case DialogResult.No:
						result = false;
						break;
					}
				}
				else
				{
					result = true;
				}
			}
		}
		return result;
	}

	public void PostJournal(M1Database database, SqlTransaction transaction, string journalID)
	{
		if (!string.IsNullOrWhiteSpace(journalID) && ValidateSelectedJournals(database, transaction, journalID, "", showMessage: false, forceNoMessage: true))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("UPDATE GLJournals SET glpPosted = 1, glpPostedDate = @Date WHERE glpGLJournalID = @JournalID");
			sqlCommand.Parameters.Add(new SqlParameter("@JournalID", SqlDbType.Int)).Value = journalID;
			sqlCommand.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime)).Value = DateTime.Today;
			database.ExecuteCommand(sqlCommand, transaction);
			sqlCommand = database.NewSqlCommand("UPDATE GLJournalLines SET gllPosted = 1 WHERE gllGLJournalID = @JournalID");
			sqlCommand.Parameters.Add(new SqlParameter("@JournalID", SqlDbType.Int)).Value = journalID;
			database.ExecuteCommand(sqlCommand, transaction);
		}
	}

	public string FormatAccount(string accountID)
	{
		return accountID.Substring(0, 3) + "-" + accountID.Substring(3, 5) + "-" + accountID.Substring(8);
	}

	public string GetAccountDescription(M1Database database, SqlTransaction transaction, string accountID)
	{
		if (accountID.ToString().Trim() == "")
		{
			return string.Empty;
		}
		string value = accountID.Substring(3, 5);
		string value2 = accountID.Substring(accountID.Length - 3);
		string value3 = accountID.Substring(0, 3);
		SqlCommand sqlCommand = database.NewSqlCommand("select glcDescription,gldDescription,glvDescription from GLCharts,GLDepartments,GLDivisions where glcGLChartID = @ChartID and gldGLDepartmentID = @DepartmentID and glvGLDivisionID = @DivisionID");
		sqlCommand.Parameters.Add(new SqlParameter("@ChartID", SqlDbType.NVarChar)).Value = value;
		sqlCommand.Parameters.Add(new SqlParameter("@DepartmentID", SqlDbType.NVarChar)).Value = value2;
		sqlCommand.Parameters.Add(new SqlParameter("@DivisionID", SqlDbType.NVarChar)).Value = value3;
		if (string.IsNullOrWhiteSpace(Convert.ToString(database.ExecuteScalar(sqlCommand, transaction))))
		{
			return string.Empty;
		}
		DataRow row = database.GetDataTable(sqlCommand, transaction).Rows[0];
		return row.Field<string>("glcDescription").Trim() + " - " + row.Field<string>("gldDescription").Trim() + " - " + row.Field<string>("glvDescription").Trim();
	}

	public bool GLJournalPostedCheck(M1Database database, SqlTransaction transaction, int glJournalID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(glpPosted,0) As glpPosted From GLJournals Where glpGLJournalID = @glJournalID");
		sqlCommand.Parameters.Add(new SqlParameter("@glJournalID", SqlDbType.Int)).Value = glJournalID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}

	private void SetColumnIndexes(ImportFileType importFileType)
	{
		switch (importFileType)
		{
		case ImportFileType.WinPay:
			GLTransactionDateColumnIndex = 1;
			GLReferenceColumnIndex = 2;
			GLExtAccountCodeColumnIndex = 3;
			GLTransactionAmountColumnIndex = 4;
			GLDescriptionColumnIndex = 0;
			break;
		case ImportFileType.RedWing:
			GLReferenceColumnIndex = 0;
			GLExtAccountCodeColumnIndex = 1;
			GLTransactionDateColumnIndex = 2;
			GLDescriptionColumnIndex = 3;
			GLTransactionAmountColumnIndex = 5;
			break;
		}
	}

	private string GetFolderPath(string fileNameWithPath)
	{
		return Path.GetDirectoryName(fileNameWithPath) + "\\";
	}

	private DataTable CreateImportTableDefinition()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"GLReference",
					typeof(string)
				},
				{
					"GLPayNo",
					typeof(string)
				},
				{
					"GLM1AccountCode",
					typeof(string)
				},
				{
					"GLExtAccountCode",
					typeof(string)
				},
				{
					"GLTransactionDate",
					typeof(DateTime)
				},
				{
					"GLDescription",
					typeof(string)
				},
				{
					"GLTransactionAmount",
					typeof(decimal)
				}
			}
		};
	}

	private DataRow GetM1GLAccountsRow(string externalGlCode)
	{
		return (from r in dtM1AccountsData.AsEnumerable()
			where r.Field<string>("glaExternalGLCode").Trim().Equals(externalGlCode.Trim(), StringComparison.CurrentCultureIgnoreCase)
			select r).FirstOrDefault();
	}

	private DataTable GetM1GLAccountInfo(M1Database database, SqlTransaction transaction)
	{
		DataTable dataTable = new DataTable();
		using SqlCommand sqlCommand = new SqlCommand("SELECT glaGLAccountID,glaExternalGLCode FROM GLAccounts WHERE glaInactive=0");
		return database.GetDataTable(sqlCommand, transaction);
	}

	private void UpdateDbOptions(M1Database database, SqlTransaction transaction, int selectedfilterIndex, string selectedImportFileLocation)
	{
		using SqlCommand sqlCommand = new SqlCommand("UPDATE FinancialProperties SET xafGLPayrollImportFormat=@GLPayrollImportFormat, xafGLPayrollImportLocation=@GLPayrollImportLocation");
		sqlCommand.Parameters.AddWithValue("@GLPayrollImportFormat", selectedfilterIndex);
		sqlCommand.Parameters.AddWithValue("@GLPayrollImportLocation", selectedImportFileLocation);
		database.ExecuteCommand(sqlCommand, transaction);
		database.PropsRefresh();
	}

	private DataTable FillImportDataTable(M1Database database, SqlTransaction transaction, string fullFileName, ImportFileType importFileType, IList<ImportLogData> lstErrors)
	{
		DataTable dataTable = CreateImportTableDefinition();
		dtM1AccountsData = GetM1GLAccountInfo(database, transaction);
		string[] array = File.ReadAllLines(fullFileName);
		int num = 1;
		if (array != null && array.Count() > 0)
		{
			string[] array2 = array;
			foreach (string text in array2)
			{
				DataRow dataRow = dataTable.NewRow();
				string[] array3 = new string[5];
				if (importFileType == ImportFileType.WinPay)
				{
					if (num == 1 && text.IndexOf('\t', 0) == -1)
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Error, "Invalid file. File does not adhere to the WinPay format."));
						break;
					}
					array3 = text.Replace("\"", "").Split('\t');
				}
				else
				{
					if (num == 1 && text.IndexOf(',', 0) == -1)
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Error, "Invalid file. File does not adhere to the RedWing format."));
						break;
					}
					array3 = text.Replace("\"", "").Split(',');
				}
				if (array3 != null && array3.Count() > 0)
				{
					if (importFileType == ImportFileType.RedWing && array3 != null && array3.Count() < 6)
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Error, $"Invalid file structure.Invalid number of columns [{array3?.Count()}] in line [{num}]."));
						num++;
						continue;
					}
					if (importFileType == ImportFileType.WinPay && array3 != null && array3.Count() < 5)
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Error, $"Invalid file structure.Invalid number of columns [{array3?.Count()}] in line [{num}]."));
						num++;
						continue;
					}
					if (!string.IsNullOrWhiteSpace(array3[GLReferenceColumnIndex]))
					{
						dataRow["GLReference"] = array3[GLReferenceColumnIndex];
						if (importFileType == ImportFileType.RedWing)
						{
							dataRow["GLPayNo"] = array3[GLReferenceColumnIndex];
						}
						else
						{
							dataRow["GLPayNo"] = string.Empty;
						}
					}
					else
					{
						dataRow["GLReference"] = string.Empty;
						dataRow["GLPayNo"] = string.Empty;
						lstErrors.Add(new ImportLogData(ImportLogDataType.Information, $"GL Reference in line [{num}] is empty."));
					}
					if (!string.IsNullOrWhiteSpace(array3[GLExtAccountCodeColumnIndex]))
					{
						dataRow["GLM1AccountCode"] = string.Empty;
						dataRow["GLExtAccountCode"] = array3[GLExtAccountCodeColumnIndex];
						DataRow m1GLAccountsRow = GetM1GLAccountsRow(array3[GLExtAccountCodeColumnIndex]);
						if (m1GLAccountsRow != null && m1GLAccountsRow["glaGLAccountID"] != null && !string.IsNullOrWhiteSpace(m1GLAccountsRow["glaGLAccountID"].ToString()))
						{
							dataRow["GLM1AccountCode"] = m1GLAccountsRow["glaGLAccountID"];
						}
						else
						{
							dataRow["GLM1AccountCode"] = string.Empty;
							lstErrors.Add(new ImportLogData(ImportLogDataType.Warning, $"GL External Code [{array3[GLExtAccountCodeColumnIndex]}] in line [{num}] could not be matched with M1 GL Account codes."));
						}
					}
					else
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Warning, $"GL External Code in line [{num}] is empty."));
					}
					if (!string.IsNullOrWhiteSpace(array3[GLTransactionDateColumnIndex]))
					{
						DateTime result = DateTime.Today;
						string[] formats = new string[2] { "MM/dd/yyyy", "M/dd/yyyy" };
						if (DateTime.TryParseExact(array3[GLTransactionDateColumnIndex], formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
						{
							dataRow["GLTransactionDate"] = result;
						}
						else
						{
							lstErrors.Add(new ImportLogData(ImportLogDataType.Error, $"Transaction Date [{array3[GLTransactionDateColumnIndex]}] in line [{num}] is invalid."));
						}
					}
					else
					{
						lstErrors.Add(new ImportLogData(ImportLogDataType.Error, $"Transaction Date in line [{num}] is empty."));
					}
					if (importFileType == ImportFileType.RedWing)
					{
						dataRow["GLDescription"] = string.Empty;
						if (!string.IsNullOrWhiteSpace(array3[GLDescriptionColumnIndex]))
						{
							dataRow["GLDescription"] = array3[GLDescriptionColumnIndex];
						}
						else
						{
							lstErrors.Add(new ImportLogData(ImportLogDataType.Information, $"Description in line [{num}] is empty."));
						}
					}
					else
					{
						dataRow["GLDescription"] = string.Empty;
					}
					if (!string.IsNullOrWhiteSpace(array3[GLTransactionAmountColumnIndex]))
					{
						decimal result2 = default(decimal);
						if (decimal.TryParse(array3[GLTransactionAmountColumnIndex], out result2))
						{
							dataRow["GLTransactionAmount"] = result2;
						}
						else
						{
							lstErrors.Add(new ImportLogData(ImportLogDataType.Error, $"Transaction Amount[{array3[GLTransactionAmountColumnIndex]}] in line [{num}] is invalid."));
						}
					}
					else
					{
						dataRow["GLTransactionAmount"] = 0;
						lstErrors.Add(new ImportLogData(ImportLogDataType.Warning, $"Transaction Amount in line [{num}] is empty."));
					}
					dataTable.Rows.Add(dataRow);
				}
				num++;
			}
		}
		else
		{
			lstErrors.Add(new ImportLogData(ImportLogDataType.Error, "File contains no data."));
		}
		return dataTable;
	}

	private bool CreateJournalLines(M1BindingSource glJournalBS, string fullFileName, ImportFileType importFileType, IList<ImportLogData> lstErrors, out DateTime journalDate)
	{
		DataTable dataTable = new DataTable();
		journalDate = DateTime.Today;
		SetColumnIndexes(importFileType);
		dataTable = FillImportDataTable(glJournalBS.Database, glJournalBS.Transaction, fullFileName, importFileType, lstErrors);
		if (lstErrors.Where((ImportLogData x) => x.LogDataType.Equals(ImportLogDataType.Error)).Count() > 0)
		{
			return false;
		}
		if (dataTable.Rows.Count > 0)
		{
			decimal num = 1m;
			M1BindingSource childBindingSource = glJournalBS.PrimaryTable.GetChildBindingSource("GLJournalLines");
			childBindingSource.ClearCache();
			short year = new Financial().GetYearAndPeriod(glJournalBS.Database, dataTable.Rows[0].Field<DateTime>("GLTransactionDate"), "GL", IgnoreClosed: true).Year;
			byte period = new Financial().GetYearAndPeriod(glJournalBS.Database, dataTable.Rows[0].Field<DateTime>("GLTransactionDate"), "GL", IgnoreClosed: true).Period;
			foreach (DataRow row in dataTable.Rows)
			{
				journalDate = row.Field<DateTime>("GLTransactionDate");
				DataRow obj = childBindingSource.AddNew() as DataRow;
				obj.BeginEdit();
				obj["gllDescription"] = (string.IsNullOrWhiteSpace(row.Field<string>("GLDescription")) ? "Wages" : row.Field<string>("GLDescription"));
				obj["gllReference"] = (string.IsNullOrWhiteSpace(row.Field<string>("GLReference")) ? string.Empty : row.Field<string>("GLReference"));
				obj["gllGLJournalLineID"] = num;
				obj["gllGLAccountID"] = (string.IsNullOrWhiteSpace(row.Field<string>("GLM1AccountCode")) ? string.Empty : row.Field<string>("GLM1AccountCode"));
				obj["gllTransactionAmount"] = Math.Round(row.Field<decimal>("GLTransactionAmount"), 2);
				obj["gllTransactionDate"] = row.Field<DateTime>("GLTransactionDate");
				obj["gllGLFiscalYearID"] = year;
				obj["gllGLFiscalYearPeriodID"] = period;
				obj.EndEdit();
				num += 1m;
			}
			childBindingSource.OnValidate(new M1BindingSource.ValidateArgs
			{
				Errors = childBindingSource.Errors
			});
			return true;
		}
		lstErrors.Add(new ImportLogData(ImportLogDataType.Error, "Invalid file or file contains no data."));
		return false;
	}

	public ImportLogData ImportJournalData(M1BindingSource glJournalBS, string fullFileName, int selectedFilterIndex)
	{
		List<ImportLogData> list = new List<ImportLogData>();
		string empty = string.Empty;
		DateTime journalDate = DateTime.Today;
		StringBuilder stringBuilder = new StringBuilder();
		ImportLogData result = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(fullFileName))
			{
				using (new HourGlass())
				{
					if (CreateJournalLines(glJournalBS, fullFileName, (ImportFileType)selectedFilterIndex, list, out journalDate))
					{
						empty = "Payroll Import Jrnl - " + journalDate.ToShortDateString();
						DataRow currentAsDataRow = glJournalBS.CurrentAsDataRow;
						currentAsDataRow.BeginEdit();
						currentAsDataRow["glpDescription"] = empty;
						currentAsDataRow["glpTransactionDate"] = journalDate;
						currentAsDataRow["glpPosted"] = 0;
						currentAsDataRow.EndEdit();
						string folderPath = GetFolderPath(fullFileName);
						UpdateDbOptions(glJournalBS.Database, glJournalBS.Transaction, selectedFilterIndex, folderPath);
						if (list.Where((ImportLogData x) => x.LogDataType.Equals(ImportLogDataType.Warning)).Count() > 0)
						{
							stringBuilder.AppendLine("File processed with following warning(s).\n");
							stringBuilder.AppendLine(string.Join("\n", from x in list
								where x.LogDataType == ImportLogDataType.Warning
								select $"{x.LogDataType} : {x.Description}"));
							result = new ImportLogData(ImportLogDataType.Warning, stringBuilder.ToString());
						}
					}
					else
					{
						if (glJournalBS.Transaction != null)
						{
							glJournalBS.Database.RollbackTransaction(glJournalBS.Transaction);
						}
						stringBuilder.AppendLine("Cannot proceed.File contains following error(s)\n");
						stringBuilder.AppendLine(string.Join("\n", from x in list
							where x.LogDataType == ImportLogDataType.Error
							select $"{x.LogDataType} : {x.Description}"));
						result = new ImportLogData(ImportLogDataType.Error, stringBuilder.ToString());
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (glJournalBS?.Transaction != null)
			{
				glJournalBS?.Database?.RollbackTransaction(glJournalBS.Transaction);
			}
			stringBuilder.AppendLine("Error occurred.\n");
			stringBuilder.AppendLine(ex.Message);
			result = new ImportLogData(ImportLogDataType.Error, stringBuilder.ToString());
		}
		return result;
	}
}
