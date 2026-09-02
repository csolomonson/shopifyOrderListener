using System;
using System.Collections.Generic;
using System.Data;
using M1.Ax.Erp.Financials.Avalara;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class AvalaraGetTaxProcess : ProcessParameters
{
	public AvalaraGetTaxProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "arpARInvoiceID" };
		KeyValueTableName = "ARInvoices";
		Description = "Evaluate Sales Taxes For AR Invoices via Avalara.";
		GridID = "M1PROCESSAVALARAARGETTAX";
		BindingSourceTable = "ARInvoices";
		ShowRefresh = true;
		HelpLink = "Avalara_AR_Get_Tax.htm";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show Invoices Not Calculated Only?")
		{
			AdoFilterExpression = "arpAvalaraTaxCalculated = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "arpAvalaraTaxCalculated"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Invoice Date")
		{
			AdditionalFields = "arpInvoiceDate",
			ValueField = "arpInvoiceDate"
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length != 0)
		{
			M1Database database = BindingSource.Database;
			M1User user = BindingSource.User;
			new List<string>();
			new List<string>();
			DataTable dataTable = database.GetDataTable("SELECT arpARInvoiceID From ARInvoices Where " + text + " Order By arpARInvoiceID");
			if (dataTable.Rows.Count != 0)
			{
				AvalaraTaxFunctions avalaraTaxFunctions = new AvalaraTaxFunctions(database, user);
				foreach (DataRow row in dataTable.Rows)
				{
					M1BindingSource m1BindingSource = new M1BindingSource(database);
					m1BindingSource.LoadDefinition(string.Empty, "ARInvoices", null, true, loadDataNow: false);
					m1BindingSource.ClearCache();
					m1BindingSource.NavigateTo(database, "arpARInvoiceID = " + M1Util.ConvertToSql(row.Field<string>("arpARInvoiceID")));
					string tax = avalaraTaxFunctions.GetTax("ARInvoices", row.Field<string>("arpARInvoiceID"), postToAvalara: false, m1BindingSource);
					if (tax.Trim().Length > 0)
					{
						messages.Add(tax);
					}
				}
			}
		}
		BindingSource = null;
	}
}
