using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class ARInvoicePostProcess : ProcessParameters
{
	public ARInvoicePostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "arpARInvoiceID" };
		KeyValueTableName = "ARInvoices";
		Description = "Use this screen to post your open invoices to the General Ledger. Note that invoices that are on hold will not appear in the list.";
		GridID = "M1ADDFROMPOSTARINVOICE";
		SecurityRole = "ARPost";
		HelpLink = "ar_closeprocess.htm";
		ContinueMessage = "This will post the {0} selected invoice(s) to your General Ledger. Once an invoice has been posted, you will be unable to edit that invoice. Are you sure you want to continue?";
		NotificationGridID = "M1ONHOLDARINVOICES";
		NotificationMessage = "There are {0} invoice(s) on hold";
		NotificationZeroMessage = "There are no invoices on hold";
		BindingSourceTable = string.Empty;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Year/Periods", null, new string[2] { "arpGLFiscalYearID", "arpGLFiscalYearPeriodID" })
		{
			AdditionalFields = "arpGLFiscalYearID,arpGLFiscalYearPeriodID",
			ValueFields = new string[2] { "arpGLFiscalYearID", "arpGLFiscalYearPeriodID" },
			InputSize = 10
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customers", null, new string[1] { "arpCustomerOrganizationID" })
		{
			AdditionalFields = "arpCustomerOrganizationID",
			ValueFields = new string[1] { "arpCustomerOrganizationID" },
			InputSize = 15
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "arpPlantID", "arpPlantDepartmentID" })
		{
			AdditionalFields = "arpPlantID,arpPlantDepartmentID",
			ValueFields = new string[2] { "arpPlantID", "arpPlantDepartmentID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Invoice Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "arpInvoiceDate",
			AdditionalFields = "arpInvoiceDate"
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		if (!selectedItems.Any())
		{
			return;
		}
		string empty = string.Empty;
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = 0;
		StringBuilder stringBuilder2 = new StringBuilder();
		int num3 = 0;
		StringBuilder stringBuilder3 = new StringBuilder();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			string text = item.KeyValues[0].ToString();
			SqlCommand sqlCommand = m1Database.NewSqlCommand("SELECT DISTINCT arlRMAReceiptID FROM ARInvoiceLines WITH(NOLOCK) INNER JOIN RMAReceipts WITH(NOLOCK) ON arlRMAReceiptID = rrpRMAReceiptID WHERE rrpPosted = 0 AND arlARInvoiceID = @invoiceID");
			sqlCommand.Parameters.Add(new SqlParameter("@invoiceID", text));
			DataTable dataTable = m1Database.GetDataTable(sqlCommand);
			SqlCommand sqlCommand2 = m1Database.NewSqlCommand("SELECT DISTINCT arlShipmentID FROM ARInvoiceLines WITH(NOLOCK) INNER JOIN Shipments WITH(NOLOCK) ON arlShipmentID = smpShipmentID WHERE smpPostedToGL = 0 AND arlARInvoiceID = @invoiceID");
			sqlCommand2.Parameters.Add(new SqlParameter("@invoiceID", text));
			DataTable dataTable2 = m1Database.GetDataTable(sqlCommand2);
			if (dataTable.Rows.Count != 0 || dataTable2.Rows.Count != 0)
			{
				if (dataTable.Rows.Count != 0)
				{
					string[] value = (from row in dataTable.AsEnumerable()
						select row.Field<string>("arlRMAReceiptID")).ToArray();
					dictionary.Add(text, string.Join(",", value));
				}
				if (dataTable2.Rows.Count != 0)
				{
					string[] value2 = (from row in dataTable2.AsEnumerable()
						select row.Field<string>("arlShipmentID")).ToArray();
					dictionary2.Add(text, string.Join(",", value2));
				}
				continue;
			}
			if (new AR().ARInvoicePostedCheck(m1Database, null, text))
			{
				num3++;
				if (stringBuilder3.Length != 0)
				{
					stringBuilder3.Append(",");
				}
				stringBuilder3.Append(text);
				continue;
			}
			M1BindingSource m1BindingSource = new M1BindingSource(m1Database);
			m1BindingSource.DataSourceTable = "ARINVOICES";
			m1BindingSource.NavigateTo(m1Database, "arpARInvoiceID = " + M1Util.ConvertToSql(text));
			empty = ((m1BindingSource == null) ? string.Empty : new AR().PostInvoice(m1BindingSource, fromPOS: false, forceNoMsg: true));
			if (string.IsNullOrWhiteSpace(empty))
			{
				num++;
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(text);
			}
			else
			{
				num2++;
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(",");
				}
				stringBuilder2.Append(empty);
			}
		}
		if (m1Database.Props("AR").Field<bool>("xafARExpressPost") && stringBuilder2.Length != 0)
		{
			clsGLFunctionsClass obj = new clsGLFunctionsClass();
			obj.SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
			obj.PostSelectedJournals(stringBuilder2.ToString(), string.Empty, bShowMessage: false);
		}
		if (stringBuilder.Length != 0)
		{
			messages.Add("The following invoices could not be posted because they contain errors.\r\n");
			messages.Add(stringBuilder.ToString());
		}
		if (dictionary2.Any())
		{
			foreach (KeyValuePair<string, string> item2 in dictionary2)
			{
				messages.Add("The AR invoice " + item2.Key + " cannot be posted until the following shipments have been posted: ");
				messages.Add(item2.Value);
			}
		}
		if (dictionary.Any())
		{
			foreach (KeyValuePair<string, string> item3 in dictionary)
			{
				messages.Add("The AR invoice " + item3.Key + " cannot be posted until the following RMA receipts have been posted: ");
				messages.Add(item3.Value);
			}
		}
		if (stringBuilder3.Length != 0)
		{
			messages.Add("The following invoices could not be posted because they are already posted.\r\n");
			messages.Add(stringBuilder3.ToString());
		}
	}
}
