using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferCallToARInvoiceProcess : ProcessParameters
{
	public TransferCallToARInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	public TransferCallToARInvoiceProcess(IServiceProvider serviceProvider, bool multipleDestinationRowsCreated = false)
		: base(serviceProvider, multipleDestinationRowsCreated)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "kbpCallID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "kbpCallID" };
		KeyValueTableName = "Calls";
		Description = "Select the Field Service calls to be invoiced.";
		CreatedBindingSourceCaption = "Create Field Service AR Invoice from Call";
		GridID = "M1ADDFROMARINVOICEFIELDSERVICE";
		BindingSourceTable = "ARInvoices";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customer", null, new string[1] { "KbpOrganizationID" })
		{
			ValueFields = new string[1] { "KbpOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Location", null, new string[1] { "KbpLocationID" })
		{
			ValueFields = new string[1] { "KbpLocationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Customer", null, new string[1] { "kbpARInvoiceOrganizationID" })
		{
			ValueFields = new string[1] { "kbpARInvoiceOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Location", null, new string[1] { "kbpARInvoiceLocationID" })
		{
			ValueFields = new string[1] { "kbpARInvoiceLocationID" }
		});
		HeaderSourceFields = new string[1] { "kbpProjectID" };
		HeaderDestinationFields = new string[1] { "arpProjectID" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		_ = string.Empty;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		MatchingFieldsInfo matchingFieldsInfo = (databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary).FindMatchingFields("Calls", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		DataTable dataTable = databaseForRow.GetDataTable("select kbpCallID,kbpOrganizationID,kbpLocationID,kbpContactID,kbpARInvoiceOrganizationID,kbpARInvoiceLocationID,kbpARInvoiceContactID,jmpPlantID,jmpPlantDepartmentID,kbpCurrencyRateID,kbpCustomRate,kbpExchangeRate," + matchingFieldsInfo.GetSourceFieldList(string.Empty, " ") + "from Calls left outer join Jobs on kbpCallID = jmpCallID where " + text + " order by kbpCallID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addInvoiceLine(databaseForRow, currentAsDataRow, childBindingSource, row);
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["arpInvoiceType"] = 1;
		if (string.IsNullOrWhiteSpace(sourceHeaderRow.Field<string>("kbpARInvoiceOrganizationID")))
		{
			currentAsDataRow["arpCustomerOrganizationID"] = sourceHeaderRow["KbpOrganizationID"];
			currentAsDataRow["arpShipOrganizationID"] = sourceHeaderRow["KbpOrganizationID"];
			currentAsDataRow["arpARInvoiceLocationID"] = sourceHeaderRow["KbpLocationID"];
			currentAsDataRow["arpShipLocationID"] = sourceHeaderRow["KbpLocationID"];
			currentAsDataRow["arpARInvoiceContactID"] = sourceHeaderRow["KbpContactID"];
			currentAsDataRow["arpShipContactID"] = sourceHeaderRow["KbpContactID"];
		}
		else
		{
			currentAsDataRow["arpCustomerOrganizationID"] = sourceHeaderRow["kbpARInvoiceOrganizationID"];
			currentAsDataRow["arpShipOrganizationID"] = sourceHeaderRow["KbpOrganizationID"];
			currentAsDataRow["arpARInvoiceLocationID"] = sourceHeaderRow["kbpARInvoiceLocationID"];
			currentAsDataRow["arpShipLocationID"] = sourceHeaderRow["KbpLocationID"];
			currentAsDataRow["arpARInvoiceContactID"] = sourceHeaderRow["kbpARInvoiceContactID"];
			currentAsDataRow["arpShipContactID"] = sourceHeaderRow["KbpContactID"];
		}
		AppAxProduction appAxProduction = new AppAxProduction(parm.BindingSource.Database);
		if (sourceHeaderRow["jmpPlantID"] != DBNull.Value)
		{
			currentAsDataRow["arpPlantID"] = sourceHeaderRow["jmpPlantID"];
		}
		else
		{
			currentAsDataRow["arpPlantID"] = appAxProduction.PlantID;
		}
		if (sourceHeaderRow["jmpPlantDepartmentID"] != DBNull.Value)
		{
			currentAsDataRow["arpPlantDepartmentID"] = sourceHeaderRow["jmpPlantDepartmentID"];
		}
		else
		{
			currentAsDataRow["arpPlantDepartmentID"] = appAxProduction.PlantDepartmentID;
		}
		if (!string.IsNullOrWhiteSpace(sourceHeaderRow.Field<string>("kbpCurrencyRateID")))
		{
			currentAsDataRow["arpCurrencyRateID"] = sourceHeaderRow["kbpCurrencyRateID"];
			currentAsDataRow["arpCustomRate"] = sourceHeaderRow["kbpCustomRate"];
			currentAsDataRow["arpExchangeRate"] = sourceHeaderRow["kbpExchangeRate"];
		}
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow callRow)
	{
		new AR().AddTimeAndMaterial(database, bsInvoiceLines, string.Empty, 0, string.Empty, 0, callRow.Field<string>("kbpCallID"));
	}
}
