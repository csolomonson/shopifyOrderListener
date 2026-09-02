using System;
using System.Collections.Generic;
using System.Data;
using M1.Ax.Erp.Financials.Avalara;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class AvalaraSOGetTaxProcess : ProcessParameters
{
	public AvalaraSOGetTaxProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "ompSalesOrderID" };
		KeyValueTableName = "SalesOrders";
		Description = "Evaluate Sales Taxes For Sales Orders via Avalara.";
		GridID = "M1PROCESSAVALARASOGETTAX";
		BindingSourceTable = "SalesOrders";
		ShowRefresh = true;
		HelpLink = "Avalara_SO_Get_Tax.htm";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show Orders Not Calculated Only?")
		{
			AdoFilterExpression = "ompAvalaraTaxCalculated = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "ompAvalaraTaxCalculated"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Order Date")
		{
			AdditionalFields = "ompOrderDate",
			ValueField = "ompOrderDate"
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
			DataTable dataTable = database.GetDataTable("SELECT ompSalesOrderID From SalesOrders Where " + text + " Order By ompSalesOrderID");
			if (dataTable.Rows.Count != 0)
			{
				AvalaraTaxFunctions avalaraTaxFunctions = new AvalaraTaxFunctions(database, user);
				foreach (DataRow row in dataTable.Rows)
				{
					M1BindingSource m1BindingSource = new M1BindingSource(database);
					m1BindingSource.LoadDefinition(string.Empty, "SalesOrders", null, true, loadDataNow: false);
					m1BindingSource.ClearCache();
					m1BindingSource.NavigateTo(database, "ompSalesOrderID = " + M1Util.ConvertToSql(row.Field<string>("ompSalesOrderID")));
					string tax = avalaraTaxFunctions.GetTax("SalesOrders", row.Field<string>("ompSalesOrderID"), postToAvalara: false, m1BindingSource);
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
