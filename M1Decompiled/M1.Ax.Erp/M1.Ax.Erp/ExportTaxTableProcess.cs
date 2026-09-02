using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using M1.Core;

namespace M1.Ax.Erp;

public class ExportTaxTableProcess : ProcessParameters
{
	public ExportTaxTableProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "paxIncomeTaxID" };
		KeyValueTableName = "IncomeTaxes";
		Description = "Use this tool to export the selected taxes to the Tools\\TaxTables folder where M1 is installed.";
		GridID = "M1INCOMETAXESEXPORT";
		BindingSourceTable = string.Empty;
		HelpLink = "payroll_exportTaxTables.htm";
		SecurityRole = "PayrollAdmin";
		ContinueMessage = "This will export the {0} selected taxes to the Tools\\TaxTables folder. Are you sure you want to continue?";
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length != 0)
		{
			M1.Core.AppContext appContext = ServiceProvider.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
			M1Database database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
			DataSet obj = new DataSet("IncomeTaxes")
			{
				Tables = 
				{
					getTable(database, "Select paxIncomeTaxID,paxDescription,paxTaxAuthority,paxCountry,paxState,paxLocalityName From IncomeTaxes Where " + text + " Order By paxIncomeTaxID", "IncomeTaxes"),
					getTable(database, "Select pafIncomeTaxID,pafIncomeTaxTypeID,pafDescription,pafPaidBy,pafDeductIncomeTaxID,pafDeductIncomeTaxTypeID,pafDeductMethod,pafSecondDeductIncomeTaxID,pafSecondDeductIncomeTaxTypeID,pafSecondDeductMethod,pafPrintOnPaySlip,pafThirdDeductIncomeTaxID,pafThirdDeductIncomeTaxTypeID,pafThirdDeductMethod,pafFourthDeductIncomeTaxID,pafFourthDeductIncomeTaxTypeID,pafFourthDeductMethod From IncomeTaxTypes Inner Join IncomeTaxes On pafIncomeTaxID = paxIncomeTaxID Where " + text + " Order By paxIncomeTaxID", "IncomeTaxTypes"),
					getTable(database, "Select pazIncomeTaxID,pazIncomeTaxTypeID,pazIncomeTaxTableID,pazDescription,pazFilingStatus From IncomeTaxTables Inner Join IncomeTaxes On pazIncomeTaxID = paxIncomeTaxID Where " + text + " Order By paxIncomeTaxID", "IncomeTaxTables"),
					getTable(database, "Select parIncomeTaxID,parIncomeTaxTypeID,parIncomeTaxTableID,parIncomeTaxTableRevisionID,parDescription,parCalculationMethod,parStartDate,parPersonalExemptionAmount,parPersonalSubsequentAmount,parDependentExemptionAmount,parDependentSubsequentAmount,parFixedExemptionAmount,parLeaveLoadingExemptionAmount,parDeductionPercent,parDeductionLimit,parPersonalTaxCredit,parDependentTaxCredit,parTaxPercent,parTaxAmount,parSupplementalWagesTaxPercent,parWageExcess,parWageLimit,parTaxLimit,parRelatedIncomeTaxID,parRelatedIncomeTaxTypeID,parDeductTaxLimit,parSecondDeductTaxLimit,parThirdDeductTaxLimit,parFourthDeductTaxLimit,parStandardAdjustmentAmount,parCAEmploymentCredit,parTaxAbatementPercent,parUseYTDAmount From IncomeTaxTableRevisions Inner Join IncomeTaxes On parIncomeTaxID = paxIncomeTaxID Where " + text + " Order By paxIncomeTaxID", "IncomeTaxTableRevisions"),
					getTable(database, "Select palIncomeTaxID,palIncomeTaxTypeID,palIncomeTaxTableID,palIncomeTaxTableRevisionID,palIncomeTaxTableLineID,palEarningsOver,palEarningsNotOver,palTaxAmount,palTaxPercent,palMultiplier,palTaxCredit,palTaxLimit From IncomeTaxTableLines Inner Join IncomeTaxes On palIncomeTaxID = paxIncomeTaxID Where " + text + " Order By paxIncomeTaxID", "IncomeTaxTableLines"),
					getTable(database, "Select pacIncomeTaxID,pacIncomeTaxTypeID,pacIncomeTaxTableID,pacIncomeTaxTableRevisionID,pacIncomeTaxTableSurtaxID,pacTaxOver,pacTaxNotOver,pacTaxAmount,pacTaxPercent From IncomeTaxTableSurtaxes Inner Join IncomeTaxes On pacIncomeTaxID = paxIncomeTaxID Where " + text + " Order By paxIncomeTaxID", "IncomeTaxTableSurtaxes")
				}
			};
			string text2 = Path.Combine(appContext.IsHosted ? appContext.Metadata.FileShareLocation : appContext.Server.Location, "Tools\\TaxTables");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			string text3 = Path.Combine(text2, "Taxes.xml");
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			obj.WriteXml(text3, XmlWriteMode.WriteSchema);
		}
	}

	private DataTable getTable(M1Database database, string query, string name)
	{
		DataTable dataTable = database.GetDataTable(query);
		dataTable.TableName = name;
		return dataTable;
	}
}
