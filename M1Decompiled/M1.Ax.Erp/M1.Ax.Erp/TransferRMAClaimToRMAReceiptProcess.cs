using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferRMAClaimToRMAReceiptProcess : ProcessParameters
{
	public TransferRMAClaimToRMAReceiptProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "ralRMAClaimID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "ralRMAClaimID", "ralRMAClaimLineID" };
		KeyValueTableName = "RMAClaimLines";
		Description = "Select the RMA claim lines to be receipted.";
		GridID = "M1ADDFROMRMARECEIPTCLAIM";
		BindingSourceTable = "RMAReceipts";
		HelpLink = "QM_TransferRMAClaimToRMAReceipt.htm";
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Customer", null, new string[1] { "rapCustomerOrganizationID" })
		{
			ValueFields = new string[1] { "rapCustomerOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Location", null, new string[1] { "rapARInvoiceLocationID" })
		{
			ValueFields = new string[1] { "rapARInvoiceLocationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Ship Location", null, new string[1] { "rapShipLocationID" })
		{
			ValueFields = new string[1] { "rapShipLocationID" }
		});
		HeaderSourceFields = new string[12]
		{
			"rapCustomerOrganizationID", "rapARInvoiceLocationID", "rapShipOrganizationID", "rapShipLocationID", "rapARInvoiceContactID", "rapShipContactID", "rapProjectID", "rapCurrencyRateID", "rapCustomRate", "rapExchangeRate",
			"rapPlantID", "rapPlantDepartmentID"
		};
		HeaderDestinationFields = new string[12]
		{
			"rrpCustomerOrganizationID", "rrpARInvoiceLocationID", "rrpShipOrganizationID", "rrpShipLocationID", "rrpARInvoiceContactID", "rrpShipContactID", "rrpProjectID", "rrpCurrencyRateID", "rrpCustomRate", "rrpExchangeRate",
			"rrpPlantID", "rrpPlantDepartmentID"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		arg.FilterErrorRegex.Add(" is inactive");
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		bool flag = false;
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Lot, value: false);
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Serial, value: false);
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		DataTable dataTable = databaseForRow.GetDataTable("SELECT ralSalesQuantity, ralRMAClaimID, ralRMAClaimLineID, IsNull((Select Sum(rrlSalesQuantityReceived) From RMAReceiptLines Where rrlRMAClaimID = ralRMAClaimID And rrlRMAClaimLineID = ralRMAClaimLineID),0) As alreadyReceiptedQuantity FROM RMAClaimLines WHERE " + text);
		if (dataTable.Rows.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				decimal num = Convert.ToDecimal(GetItemValuesFromList(selectedItems, row).EditableValues["QtyReceipted"]);
				if (row.Field<decimal>("ralSalesQuantity") == row.Field<decimal>("alreadyReceiptedQuantity") && num > 0m)
				{
					messages.Add(string.Format("RMA Receipt for line {0}/{1} was not added because Sales Quantity has already been completed", row.Field<string>("ralRMAClaimID"), row.Field<short>("ralRMAClaimLineID")));
				}
				else if (row.Field<decimal>("ralSalesQuantity") < row.Field<decimal>("alreadyReceiptedQuantity") + num)
				{
					messages.Add(string.Format("RMA Receipt for line {0}/{1} was not added because Qty Receipted value is greater than Sales Quantity received", row.Field<string>("ralRMAClaimID"), row.Field<short>("ralRMAClaimLineID")));
					flag = true;
				}
			}
		}
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("RMAClaims", "RMAReceipts", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("RMAClaimLines", "RMAReceiptLines", new string[18]
		{
			"ralRMAClaimID", "ralRMAClaimLineID", "ralPartID", "ralPartRevisionID", "ralPartWarehouseLocationID", "ralPartBinID", "ralPartShortDescription", "ralPartLongDescriptionRTF", "ralPartLongDescriptionText", "ralUnitOfMeasure",
			"ralSalesUnitOfMeasure", "ralProjectID", "ralProjectAreaID", "ralOrgPartID", "ralOrgPartShortDescription", "ralConversionFactor", "ralRequiresInspection", "ralKitPart"
		}, new string[18]
		{
			"rrlRMAClaimID", "rrlRMAClaimLineID", "rrlPartID", "rrlPartRevisionID", "rrlPartWarehouseLocationID", "rrlPartBinID", "rrlDescription", "rrlPartLongDescriptionRTF", "rrlPartLongDescriptionText", "rrlInventoryUnitOfMeasure",
			"rrlSalesUnitOfMeasure", "rrlProjectID", "rrlProjectAreaID", "rrlOrgPartID", "rrlOrgPartShortDescription", "rrlConversionFactor", "rrlRequiresInspection", "rrlKitPart"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("RMAClaimComponents, RMAClaimLines", "RMAReceiptComponents", new string[12]
		{
			"raoRMAClaimID", "raoRMAClaimLineID", "raoRMAClaimComponentID", "raoPartID", "raoPartRevisionID", "raoPartWarehouseLocationID", "raoPartBinID", "raoQuantityPerParent", "raoAdditionalQuantity", "raoUnitOfMeasure",
			"raoDescription", "raoWeight"
		}, new string[12]
		{
			"rroRMAClaimID", "rroRMAClaimLineID", "rroRMAClaimComponentID", "rroPartID", "rroPartRevisionID", "rroPartWarehouseLocationID", "rroPartBinID", "rroQuantityPerParent", "rroAdditionalQuantity", "rroUnitOfMeasure",
			"rroDescription", "rroWeight"
		});
		DataTable dataTable2 = databaseForRow.GetDataTable("select ralUnitCost, ralUnitCostForeign, ralSalesQuantity, IsNull((Select Sum(rrlSalesQuantityReceived) From RMAReceiptLines Where rrlRMAClaimID = ralRMAClaimID And rrlRMAClaimLineID = ralRMAClaimLineID),0) As alreadyReceiptedQuantity, " + matchingFieldsInfo2.GetSourceFieldList("", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from RMAClaimLines inner join RMAClaims on ralRMAClaimID = rapRMAClaimID  where " + text + " order by ralRMAClaimID, ralRMAClaimLineID");
		DataTable dataTable3 = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from RMAClaimComponents inner join RMAClaimLines on ralRMAClaimID=raoRMAClaimID and ralRMAClaimLineID=raoRMAClaimLineID where " + text + " and ralReceivedComplete = 0  order by raoRMAClaimID,raoRMAClaimLineID,raoRMAClaimComponentID");
		if (dataTable2.Rows.Count == 0 || flag)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RMAReceiptComponents");
		foreach (DataRow row2 in dataTable2.Rows)
		{
			decimal num2 = Convert.ToDecimal(GetItemValuesFromList(selectedItems, row2).EditableValues["QtyReceipted"]);
			if (row2.Field<decimal>("ralSalesQuantity") >= row2.Field<decimal>("alreadyReceiptedQuantity") + num2)
			{
				CheckForHeaderKeyChange(this, row2, matchingFieldsInfo, currentAsDataRow);
				addRMAReceiptLine(childBindingSource, row2, childBindingSource2, dataTable3, matchingFieldsInfo2, matchingFieldsInfo3, GetItemValuesFromList(selectedItems, row2));
			}
		}
		if (!currentAsDataRow.Field<bool>("rrpCustomRate"))
		{
			currentAsDataRow.SetField("rrpExchangeRate", databaseForRow.GetExchangeRate(currentAsDataRow.Field<string>("rrpCurrencyRateID"), currentAsDataRow.Field<DateTime>("rrpReceiptDate")));
		}
	}

	private void addRMAReceiptLine(M1BindingSource bsRMAReceiptLines, DataRow claimRow, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo claimLinematches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, claimRow, bsRMAReceiptLines, claimLinematches);
		dataRow["rrlReference"] = "Claim " + claimRow.Field<string>("ralRMAClaimID").Trim() + "-" + claimRow.Field<short>("ralRMACLaimLineID").ToString().Trim();
		if (itemValues.EditableValues != null && itemValues.EditableValues.ContainsKey("QtyReceipted"))
		{
			decimal value = Convert.ToDecimal(itemValues.EditableValues["QtyReceipted"]);
			dataRow.SetField("rrlSalesQuantityReceived", value);
		}
		if (!HeaderFixForeign)
		{
			dataRow.SetField("rrlUnitCost", claimRow.Field<decimal>("ralUnitCost"));
		}
		else
		{
			dataRow.SetField("rrlUnitCostForeign", claimRow.Field<decimal>("ralUnitCostForeign"));
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("raoRMAClaimID = " + dataRow.Field<string>("rrlRMAClaimID").Trim().ToLinq() + " and raoRMAClaimLineID = " + Convert.ToInt32(dataRow["rrlRMAClaimLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}
}
