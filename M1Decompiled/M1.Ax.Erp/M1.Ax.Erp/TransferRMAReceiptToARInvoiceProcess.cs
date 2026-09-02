using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferRMAReceiptToARInvoiceProcess : ProcessParameters
{
	public TransferRMAReceiptToARInvoiceProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	public TransferRMAReceiptToARInvoiceProcess(IServiceProvider serviceProvider, bool multipleDestinationRowsCreated = false)
		: base(serviceProvider, multipleDestinationRowsCreated)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "rrlRMAReceiptID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "rrlRMAReceiptID", "rrlRMAReceiptLineID" };
		KeyValueTableName = "RMAReceiptLines";
		Description = "Select the RMA Receipt lines to be invoiced.";
		GridID = "M1ADDFROMARINVOICERMARECEIPT";
		BindingSourceTable = "ARInvoices";
		PromptFieldValidations.Add(new PromptFieldValidationBool("rrpClosed", fieldValue: false, "RMA Receipt is closed."));
		HeaderSourceFields = new string[13]
		{
			"rrpCustomerOrganizationID", "rrpARInvoiceLocationID", "rrpARInvoiceContactID", "rrpShipOrganizationID", "rrpShipLocationID", "rrpShipContactID", "rrpProjectID", "rrpPlantID", "rrpPlantDepartmentID", "rrpCurrencyRateID",
			"rrpCustomRate", "rrpExchangeRate", "rrpShippingMethodID"
		};
		HeaderDestinationFields = new string[13]
		{
			"arpCustomerOrganizationID", "arpARInvoiceLocationID", "arpARInvoiceContactID", "arpShipOrganizationID", "arpShipLocationID", "arpShipContactID", "arpProjectID", "arpPlantID", "arpPlantDepartmentID", "arpCurrencyRateID",
			"arpCustomRate", "arpExchangeRate", "arpShippingMethodID"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		_ = string.Empty;
		List<string> list = new List<string>();
		M1Database m1Database = (M1Database)ServiceProvider.GetService(typeof(M1Database));
		string value = selectedItems[0].KeyValues[0].ToString();
		SqlCommand sqlCommand = m1Database.NewSqlCommand("SELECT DISTINCT rrpRMAReceiptID FROM RMAReceipts WITH(NOLOCK) WHERE rrpPosted = 0 AND rrpRMAReceiptID = @rmaReceiptId");
		sqlCommand.Parameters.Add(new SqlParameter("@rmaReceiptId", value));
		DataTable dataTable = m1Database.GetDataTable(sqlCommand);
		if (m1Database.Props("FN").Field<bool>("xafGLCreateStockJournals") && dataTable.Rows.Count > 0)
		{
			list.Add(dataTable.Rows[0].Field<string>("rrpRMAReceiptID"));
		}
		else
		{
			string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
			if (text.Length != 0)
			{
				DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
				m1Database = BindingSource.GetDatabaseForRow(currentAsDataRow);
				M1DataDictionary obj = m1Database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
				MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("RMAReceipts, Organizations, OrganizationLocations", "ARInvoices", HeaderSourceFields, HeaderDestinationFields);
				MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("RMAReceiptLines, RMAClaimLines", "ARInvoiceLines", new string[14]
				{
					"rrlPartID", "rrlPartRevisionID", "rrlOrgPartID", "rrlOrgPartShortDescription", "rrlDescription", "rrlPartLongDescriptionRTF", "rrlPartLongDescriptionText", "rrlProjectID", "rrlProjectAreaID", "rrlReceivedComplete",
					"rrlRMAClaimID", "rrlRMAClaimLineID", "rrlRMAReceiptID", "rrlRMAReceiptLineID"
				}, new string[14]
				{
					"arlPartID", "arlPartRevisionID", "arlOrgPartID", "arlOrgPartShortDescription", "arlPartShortDescription", "arlPartLongDescriptionRTF", "arlPartLongDescriptionText", "arlProjectID", "arlProjectAreaID", "arlDeliveryInvoicedComplete",
					"arlRMAClaimID", "arlRMAClaimLineID", "arlRMAReceiptID", "arlRMAReceiptLineID"
				});
				DataTable dataTable2 = m1Database.GetDataTable("select rrlSalesQuantityReceived,ralFullUnitPriceBase,ralUnitDiscountBase,ralUnitPrice,ralFullUnitPriceForeign,ralUnitDiscountForeign,ralUnitPriceForeign,ralUnitOfMeasure,ralPartGroupID,ralCustomerPO,rapARInvoiceContactID,rapShipContactID,rapAuthorizationNumber,rapClaimDate,rapResellerOrganizationID,rapResellerLocationID,rapResellerContactID," + matchingFieldsInfo2.GetSourceFieldList(string.Empty, ",") + "rrpClosed,rrpFreightCharge,rrpFreightChargeForeign," + matchingFieldsInfo.GetSourceFieldList(string.Empty, " ") + "from RMAReceiptLines inner join RMAReceipts on rrlRMAReceiptID = rrpRMAReceiptID left outer join RMAClaimLines on ralRMAClaimID = rrlRMAClaimID and ralRMAClaimLineID = rrlRMAClaimLineID left outer join RMAClaims on ralRMAClaimID = rapRMAClaimID where " + text + " order by rrlRMAReceiptID, rrlRMAReceiptLineID");
				if (dataTable2.Rows.Count != 0)
				{
					M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("ARInvoiceLines");
					foreach (DataRow row in dataTable2.Rows)
					{
						CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
						addInvoiceLine(m1Database, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2);
					}
				}
			}
		}
		if (list.Count > 0)
		{
			string text2 = string.Join(", ", list.ToArray());
			messages.Add("RMA Receipt " + text2 + " cannot be added to the AR invoice because it has not been posted.");
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["arpInvoiceType"] = 2;
		if (HeaderFixForeign)
		{
			currentAsDataRow.SetField("arpFreightAmountForeign", currentAsDataRow.Field<decimal>("arpFreightAmountForeign") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("rrpFreightChargeForeign"))) : sourceHeaderRow.Field<decimal>("rrpFreightChargeForeign")));
		}
		else
		{
			currentAsDataRow.SetField("arpFreightAmountBase", currentAsDataRow.Field<decimal>("arpFreightAmountBase") + ((currentAsDataRow.Field<byte>("arpInvoiceType") == 2) ? (-Math.Abs(sourceHeaderRow.Field<decimal>("rrpFreightCharge"))) : sourceHeaderRow.Field<decimal>("rrpFreightCharge")));
		}
		if (sourceHeaderRow["rapARInvoiceContactID"] != DBNull.Value)
		{
			currentAsDataRow["arpARInvoiceContactID"] = sourceHeaderRow["rapARInvoiceContactID"];
		}
		if (sourceHeaderRow["rapShipContactID"] != DBNull.Value)
		{
			currentAsDataRow["arpShipContactID"] = sourceHeaderRow["rapShipContactID"];
		}
		if (sourceHeaderRow["rapClaimDate"] != DBNull.Value)
		{
			currentAsDataRow["arpOrderDate"] = sourceHeaderRow["rapClaimDate"];
		}
		if (sourceHeaderRow["rapResellerOrganizationID"] != DBNull.Value)
		{
			currentAsDataRow["arpResellerOrganizationID"] = sourceHeaderRow["rapResellerOrganizationID"];
		}
		if (sourceHeaderRow["rapResellerLocationID"] != DBNull.Value)
		{
			currentAsDataRow["arpResellerLocationID"] = sourceHeaderRow["rapResellerLocationID"];
		}
		if (sourceHeaderRow["rapResellerContactID"] != DBNull.Value)
		{
			currentAsDataRow["arpResellerContactID"] = sourceHeaderRow["rapResellerContactID"];
		}
	}

	private void addInvoiceLine(M1Database database, DataRow invoiceRow, M1BindingSource bsInvoiceLines, DataRow rmaReceiptLineRow, MatchingFieldsInfo rmaReceiptLineMatches)
	{
		DataRow dataRow = TransferLineInfo(this, rmaReceiptLineRow, bsInvoiceLines, rmaReceiptLineMatches);
		if (rmaReceiptLineRow["rrlSalesQuantityReceived"] != DBNull.Value)
		{
			if (invoiceRow.Field<byte>("arpInvoiceType") == 2)
			{
				dataRow["arlInvoiceQuantity"] = -Math.Abs(rmaReceiptLineRow.Field<decimal>("rrlSalesQuantityReceived"));
			}
			else
			{
				dataRow["arlInvoiceQuantity"] = rmaReceiptLineRow.Field<decimal>("rrlSalesQuantityReceived");
			}
		}
		if (HeaderFixForeign)
		{
			if (rmaReceiptLineRow["ralFullUnitPriceForeign"] != DBNull.Value)
			{
				dataRow["arlFullUnitPriceForeign"] = rmaReceiptLineRow["ralFullUnitPriceForeign"];
				dataRow["arlUnitPriceForeign"] = rmaReceiptLineRow["ralUnitPriceForeign"];
			}
		}
		else if (rmaReceiptLineRow["ralFullUnitPriceBase"] != DBNull.Value)
		{
			dataRow["arlFullUnitPriceBase"] = rmaReceiptLineRow["ralFullUnitPriceBase"];
			dataRow["arlUnitPriceBase"] = rmaReceiptLineRow["ralUnitPrice"];
		}
		if (rmaReceiptLineRow["ralPartGroupID"] != DBNull.Value)
		{
			dataRow["arlPartGroupID"] = rmaReceiptLineRow["ralPartGroupID"];
		}
		if (rmaReceiptLineRow["ralUnitOfMeasure"] != DBNull.Value)
		{
			dataRow["arlUnitOfMeasure"] = rmaReceiptLineRow["ralUnitOfMeasure"];
		}
		if (rmaReceiptLineRow["ralCustomerPO"] != DBNull.Value)
		{
			dataRow["arlCustomerPO"] = rmaReceiptLineRow["ralCustomerPO"];
		}
	}
}
