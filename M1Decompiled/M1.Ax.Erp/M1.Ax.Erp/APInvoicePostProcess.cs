using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Script.Interfaces;
using M1Classes92;

namespace M1.Ax.Erp;

public class APInvoicePostProcess : ProcessParameters
{
	public APInvoicePostProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[1] { "appAPInvoiceID" };
		KeyValueTableName = "APInvoices";
		Description = "Use this screen to post your open invoices to the General Ledger. Note that invoices that are on hold will not appear in the list.";
		GridID = "M1ADDFROMPOSTAPINVOICE";
		SecurityRole = "APPost";
		HelpLink = "ap_closeprocess.htm";
		ContinueMessage = "This will post the {0} selected invoice(s) to your General Ledger. Once an invoice has been posted, you will be unable to edit that invoice. Are you sure you want to continue?";
		NotificationGridID = "M1ONHOLDAPINVOICES";
		NotificationMessage = "There are {0} invoice(s) on hold";
		NotificationZeroMessage = "There are no invoices on hold";
		BindingSourceTable = string.Empty;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Year/Periods", null, new string[2] { "appGLFiscalYearID", "appGLFiscalYearPeriodID" })
		{
			AdditionalFields = "appGLFiscalYearID,appGLFiscalYearPeriodID",
			ValueFields = new string[2] { "appGLFiscalYearID", "appGLFiscalYearPeriodID" },
			InputSize = 10
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customers", null, new string[1] { "appSupplierOrganizationID" })
		{
			AdditionalFields = "appSupplierOrganizationID",
			ValueFields = new string[1] { "appSupplierOrganizationID" },
			InputSize = 15
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Plants", null, new string[2] { "appPlantID", "appPlantDepartmentID" })
		{
			AdditionalFields = "appPlantID,appPlantDepartmentID",
			ValueFields = new string[2] { "appPlantID", "appPlantDepartmentID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Invoice Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "appInvoiceDate",
			AdditionalFields = "appInvoiceDate"
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		if (selectedItems.Count == 0)
		{
			return;
		}
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		List<string> list = new List<string>();
		int num2 = 0;
		List<string> list2 = new List<string>();
		int num3 = 0;
		int num4 = 0;
		StringBuilder stringBuilder2 = new StringBuilder();
		clsAPFunctionsClass clsAPFunctionsClass2 = new clsAPFunctionsClass();
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		clsAPFunctionsClass2.SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
		foreach (ProcessSelectedItemValues item in selectedItems)
		{
			string text = item.KeyValues[0].ToString();
			SqlCommand sqlCommand = m1Database.NewSqlCommand("Select DISTINCT rmpReceiptID From APInvoiceLines Inner Join Receipts On aplReceiptID = rmpReceiptID Where aplAPInvoiceID = @InvoiceID And rmpPostedToGL = 0 Order By rmpReceiptID");
			sqlCommand.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.VarChar)).Value = text;
			DataTable dataTable = m1Database.GetDataTable(sqlCommand);
			SqlCommand sqlCommand2 = m1Database.NewSqlCommand("Select DISTINCT dspDMRShipmentID From APInvoiceLines Inner Join DMRShipments On aplDMRShipmentID = dspDMRShipmentID Where aplAPInvoiceID = @InvoiceID And dspPosted = 0 Order By dspDMRShipmentID");
			sqlCommand2.Parameters.Add(new SqlParameter("@InvoiceID", SqlDbType.VarChar)).Value = text;
			DataTable dataTable2 = m1Database.GetDataTable(sqlCommand2);
			int num5;
			if (dataTable.Rows.Count > 0)
			{
				foreach (DataRow row3 in dataTable.Rows)
				{
					list.Add(row3.Field<string>("rmpReceiptID"));
					num2++;
				}
				num5 = 0;
			}
			else if (dataTable2.Rows.Count > 0)
			{
				foreach (DataRow row4 in dataTable2.Rows)
				{
					list2.Add(row4.Field<string>("dspDMRShipmentID"));
					num3++;
				}
				num5 = 0;
			}
			else
			{
				num5 = clsAPFunctionsClass2.PostInvoice(text, bForceNoMessage: true);
			}
			if (num5 == 0)
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
				num4++;
				if (stringBuilder2.Length != 0)
				{
					stringBuilder2.Append(",");
				}
				stringBuilder2.Append(num5.ToString());
			}
		}
		clsAPFunctionsClass2 = null;
		if (m1Database.Props("AP").Field<bool>("xafAPExpressPost") && stringBuilder2.Length != 0)
		{
			clsGLFunctionsClass obj = new clsGLFunctionsClass();
			obj.SetReferences(ServiceProvider.GetService(typeof(ScriptApp)), ServiceProvider.GetService(typeof(IForms)));
			obj.PostSelectedJournals(stringBuilder2.ToString(), string.Empty, bShowMessage: false);
		}
		if (stringBuilder.Length == 0)
		{
			return;
		}
		messages.Add("The following invoices could not be posted because they contain errors.");
		messages.Add(stringBuilder.ToString());
		if (num2 > 0)
		{
			messages.Add("\r\n");
			messages.Add("The AP Invoices cannot be posted until the following receipts have been posted:");
			foreach (string item2 in list)
			{
				messages.Add(item2);
			}
		}
		if (num3 <= 0)
		{
			return;
		}
		messages.Add("\r\n");
		messages.Add("The AP Invoices cannot be posted until the following DRM Shipments have been posted:");
		foreach (string item3 in list2)
		{
			messages.Add(item3);
		}
	}
}
