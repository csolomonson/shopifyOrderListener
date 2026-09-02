using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("GL")]
[ComVisible(true)]
public class AppAxGL
{
	private IServiceProvider provider;

	private M1Database _Database;

	public AppAxGL(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		_Database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public decimal GetAccountBalance(string accountID)
	{
		return new GL().GetAccountBalance(_Database, accountID);
	}

	public void PostJournal(object transaction, string journalID)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		new GL().PostJournal(_Database, (SqlTransaction)transaction, journalID);
	}

	public bool ValidateSelectedJournals(object transaction, string journalList, string sqlFilter, bool showMessage, bool forceNoMessage)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new GL().ValidateSelectedJournals(_Database, (SqlTransaction)transaction, journalList, sqlFilter, showMessage, forceNoMessage);
	}

	public GL.ImportLogData ImportJournalData(M1BindingSource bsJournal, string fullFileName, int selectedFilterIndex)
	{
		return new GL().ImportJournalData(bsJournal, fullFileName, selectedFilterIndex);
	}

	public bool GLJournalPostedCheck(object transaction, int glJournalID)
	{
		if (glJournalID != 0)
		{
			if (transaction == DBNull.Value)
			{
				transaction = null;
			}
			return new GL().GLJournalPostedCheck(_Database, (SqlTransaction)transaction, glJournalID);
		}
		return false;
	}

	public bool AccountHasJournalLinesRelatedRecords(string accountId)
	{
		SqlCommand sqlCommand = _Database.NewSqlCommand("SELECT COUNT(*) As RecordCount FROM GLJournalLines WHERE GLLGLACCOUNTID = @AccountId");
		sqlCommand.Parameters.Add(new SqlParameter("@AccountId", SqlDbType.NVarChar)).Value = accountId;
		DataTable dataTable = _Database.GetDataTable(sqlCommand);
		if (dataTable.Rows.Count > 0)
		{
			return dataTable.Rows[0].Field<int>("RecordCount") > 0;
		}
		return false;
	}
}
