using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class TransferGLRecurringJournalToJournalProcess : ProcessParameters
{
	public TransferGLRecurringJournalToJournalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "glrRecurringJournalID" };
		KeyValueTableName = "GLRecurringJournals";
		Description = "Use this option to create new GL Journals from the entries you have set up as 'recurring'.";
		GridID = "M1ADDFROMGLJOURNALRECURRING";
		SecurityRole = "GLRecurring";
		HelpLink = "gl_processRecurJournals.htm";
		ContinueMessage = "This will create gl journals from the {0} selected recurring gl journals. Are you sure you want to continue?";
		BindingSourceTable = "GLJournals";
		MultipleDestinationRowsCreated = true;
		DefaultValueFieldNames = new string[3] { "glpTransactionDate", "glpGLFiscalYearID", "glpGLFiscalYearPeriodID" };
		DefaultValueFilterExpression = "(glrInactive = 0 Or (glrInactive = 1 And glrInactiveDate > glpTransactionDate)) And (glrStartGLFiscalYearID <= glpGLFiscalYearID And (glrStartGLFiscalYearID <> glpGLFiscalYearID Or glrStartGLFiscalYearID = glpGLFiscalYearID And glrStartGLFiscalYearPeriodID <= glpGLFiscalYearPeriodID)) And (glrEndGLFiscalYearID = 0 Or (glrEndGLFiscalYearID >= glpGLFiscalYearID And (glrEndGLFiscalYearID <> glpGLFiscalYearID Or glrEndGLFiscalYearID = glpGLFiscalYearID And glrEndGLFiscalYearPeriodID >= glpGLFiscalYearPeriodID))) And ((glrPeriod01=1 And glpGLFiscalYearPeriodID=1) Or (glrPeriod02=1 And glpGLFiscalYearPeriodID=2) Or (glrPeriod03=1 And glpGLFiscalYearPeriodID=3) Or (glrPeriod04=1 And glpGLFiscalYearPeriodID=4) Or (glrPeriod05=1 And glpGLFiscalYearPeriodID=5) Or (glrPeriod06=1 And glpGLFiscalYearPeriodID=6) Or (glrPeriod07=1 And glpGLFiscalYearPeriodID=7) Or (glrPeriod08=1 And glpGLFiscalYearPeriodID=8) Or (glrPeriod09=1 And glpGLFiscalYearPeriodID=9) Or (glrPeriod10=1 And glpGLFiscalYearPeriodID=10) Or (glrPeriod11=1 And glpGLFiscalYearPeriodID=11) Or (glrPeriod12=1 And glpGLFiscalYearPeriodID=12) Or (glrPeriod13=1 And glpGLFiscalYearPeriodID=13))";
		HeaderSourceFields = new string[3] { "glrReference", "glrDescription", "glrReversingEntry" };
		HeaderDestinationFields = new string[3] { "glpReference", "glpDescription", "glpReversingEntry" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.DefaultFieldValues;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow dataRow = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("GLRecurringJournals", "GLJournals", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("GLRecurringJournalLines", "GLJournalLines", new string[3] { "gljDescription", "gljReference", "gljGLAccountID" }, new string[3] { "gllDescription", "gllReference", "gllGLAccountID" });
		DataTable dataTable = database.GetDataTable("select gljAmount, gljRecurringJournalID, glrRecurringJournalID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from GLRecurringJournalLines inner join GLRecurringJournals on gljRecurringJournalID=glrRecurringJournalID where " + text + " order by gljRecurringJournalID,gljRecurringJournalLineID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("GLJournalLines");
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow row in dataTable.Rows)
		{
			if (!row.Field<int>("gljRecurringJournalID").Equals(num))
			{
				num = 0;
				num2 = 0;
			}
			if (num2 == 0)
			{
				dataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(dataRow);
				SetDefaultFieldValues(arg, dataRow);
				BindingSource.ActivateRow(dataRow, null, doFlash: false);
				num3 = dataRow.Field<int>("glpGLJournalID");
			}
			else
			{
				num3 = num2;
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow);
			addJournalLine(database, dataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
			num = row.Field<int>("gljRecurringJournalID");
			if (!num2.Equals(num3))
			{
				num2 = num3;
			}
			if (num3 != 0)
			{
				arg.KeysCreated.Add(new object[1] { num3 });
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append(num3.ToString());
			DateTime d = (dataRow.IsNull("glpTransactionDate") ? DateTime.Now : ((DateTime)dataRow["glpTransactionDate"]));
			database.ExecuteScalar("UPDATE GLRecurringJournals SET glrLastTransferredDate = " + d.ToSql() + " WHERE glrRecurringJournalID = " + num.ToSql());
		}
		if (arg.KeysCreated.Count != 0)
		{
			clsGLFunctionsClass clsGLFunctionsClass2 = new clsGLFunctionsClass();
			clsGLFunctionsClass2.SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
			if (clsGLFunctionsClass2.IsGLExpressPost() && stringBuilder.Length != 0)
			{
				clsGLFunctionsClass2.PostSelectedJournals(stringBuilder.ToString(), string.Empty);
			}
			clsGLFunctionsClass2 = null;
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "GLJournal";
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["glpPosted"] = false;
		currentAsDataRow["glpSource"] = 4;
		currentAsDataRow["glpDetailSource"] = 7;
		if (currentAsDataRow.IsNull("glpTransactionDate"))
		{
			currentAsDataRow["glpTransactionDate"] = DateTime.Today;
		}
	}

	private void addJournalLine(M1Database database, DataRow journalRow, M1BindingSource bsJournalLines, DataRow lineRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsJournalLines, lineMatches, journalRow);
		dataRow["gllPosted"] = false;
		dataRow["gllTransactionAmount"] = lineRow.Field<decimal>("gljAmount");
	}
}
