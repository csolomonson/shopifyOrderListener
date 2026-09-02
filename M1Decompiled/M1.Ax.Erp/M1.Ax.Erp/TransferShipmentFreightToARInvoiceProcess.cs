using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferShipmentFreightToARInvoiceProcess : ProcessParameters
{
	public TransferShipmentFreightToARInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	public TransferShipmentFreightToARInvoiceProcess(IServiceProvider serviceProvider, bool multipleDestinationRowsCreated = false)
		: base(serviceProvider, multipleDestinationRowsCreated)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "smpShipmentID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "smpShipmentID" };
		KeyValueTableName = "Shipments";
		Description = "Select the shipment to have its freight invoiced.";
		GridID = "M1ADDFROMARINVOICESHIPMENTFREIGHT";
		BindingSourceTable = "ARInvoices";
		HeaderSourceFields = new string[13]
		{
			"smpCustomerOrganizationID", "smpARInvoiceLocationID", "smpARInvoiceContactID", "smpShipOrganizationID", "smpShipLocationID", "smpShipContactID", "smpShippingMethodID", "smpShippingPaymentTypeID", "smpPlantID", "smpPlantDepartmentID",
			"smpCurrencyRateID", "smpCustomRate", "smpExchangeRate"
		};
		HeaderDestinationFields = new string[13]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpShippingMethodID", "arpShippingPaymentTypeID", "arpPlantID", "arpPlantDepartmentID",
			"arpCurrencyRateID", "arpCustomRate", "arpExchangeRate"
		};
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
		MatchingFieldsInfo matchingFieldsInfo = (databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary).FindMatchingFields("Shipments", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
		DataTable dataTable = databaseForRow.GetDataTable("select smpShipmentID,isnull(xayDoNotXferShipCostsToAR,0) as xayDoNotXferShipCostsToAR,smpClosed,smpFreightCharge,smpFreightChargeForeign" + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from Shipments left outer join ShippingPaymentTypes on xayShippingPaymentTypeID = smpShippingPaymentTypeID where " + text + " order by smpShipmentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		if (!sourceHeaderRow.Field<bool>("xayDoNotXferShipCostsToAR"))
		{
			DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
			if (HeaderFixForeign)
			{
				currentAsDataRow.SetField("arpFreightAmountForeign", currentAsDataRow.Field<decimal>("arpFreightAmountForeign") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("smpFreightChargeForeign"))) : sourceHeaderRow.Field<decimal>("smpFreightChargeForeign")));
			}
			else
			{
				currentAsDataRow.SetField("arpFreightAmountBase", currentAsDataRow.Field<decimal>("arpFreightAmountBase") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("smpFreightCharge"))) : sourceHeaderRow.Field<decimal>("smpFreightCharge")));
			}
		}
	}
}
