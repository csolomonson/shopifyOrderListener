using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferRMAClaimToSalesOrderProcess : ProcessParameters
{
	public TransferRMAClaimToSalesOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "ralRMAClaimID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "ralRMAClaimID", "ralRMAClaimLineID" };
		KeyValueTableName = "RMAClaimLines";
		Description = "Use this screen to create sales orders from rma claims.";
		GridID = "M1ADDFROMSORMACLAIM";
		BindingSourceTable = "SalesOrders";
		CreatedBindingSourceCaption = "Create Sales Order From RMA Claim";
		HelpLink = "OM_CreateRMAOrder.htm";
		ContinueMessage = "This will create a sales order from the {0} selected rma claim lines. Are you sure you want to continue?";
		PromptFieldValidations.Add(new PromptFieldValidationBool("ralTransferredToSalesOrder", fieldValue: false, "RMA Claim already transferred to Sales Order."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("ralInvoicedComplete", fieldValue: false, "RMA Claim is invoiced complete."));
		HeaderSourceFields = new string[16]
		{
			"rapCustomerOrganizationID", "rapARInvoiceLocationID", "rapARInvoiceContactID", "rapShipOrganizationID", "rapShipLocationID", "rapShipContactID", "rapClaimDate", "rapResellerOrganizationID", "rapResellerLocationID", "rapResellerContactID",
			"rapPlantID", "rapPlantDepartmentID", "rapCurrencyRateID", "rapCustomRate", "rapExchangeRate", "rapProjectID"
		};
		HeaderDestinationFields = new string[16]
		{
			"ompCustomerOrganizationID", "ompARInvoiceLocationID", "ompARInvoiceContactID", "ompShipOrganizationID", "ompShipLocationID", "ompShipContactID", "ompOrderDate", "ompResellerOrganizationID", "ompResellerLocationID", "ompResellerContactID",
			"ompPlantID", "ompPlantDepartmentID", "ompCurrencyRateID", "ompCustomRate", "ompExchangeRate", "ompProjectID"
		};
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		if (!(databaseForRow.GetService(typeof(M1DataDictionary)) is M1DataDictionary m1DataDictionary))
		{
			return;
		}
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("RMAClaims", "SalesOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("RMAClaimLines", "SalesOrderLines", new string[13]
		{
			"ralPartID", "ralPartRevisionID", "ralOrgPartID", "ralOrgPartShortDescription", "ralUnitOfMeasure", "ralPartShortDescription", "ralPartLongDescriptionRTF", "ralPartLongDescriptionText", "ralPartGroupID", "ralProjectID",
			"ralProjectAreaID", "ralRMAClaimID", "ralRMAClaimLineID"
		}, new string[13]
		{
			"omlPartID", "omlPartRevisionID", "omlOrgPartID", "omlOrgPartShortDescription", "omlUnitOfMeasure", "omlPartShortDescription", "omlPartLongDescriptionRTF", "omlPartLongDescriptionText", "omlPartGroupID", "omlProjectID",
			"omlProjectAreaID", "omlRMAClaimID", "omlRMAClaimLineID"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("RMAClaimComponents, RMAClaimLines", "SalesOrderComponents", new string[9] { "raoPartID", "raoPartRevisionID", "raoPartWarehouseLocationID", "raoPartBinID", "raoQuantityPerParent", "raoAdditionalQuantity", "raoUnitOfMeasure", "raoDescription", "raoWeight" }, new string[9] { "omoPartID", "omoPartRevisionID", "omoPartWarehouseLocationID", "omoPartBinID", "omoQuantityPerParent", "omoAdditionalQuantity", "omoUnitOfMeasure", "omoDescription", "omoWeight" });
		DataTable dataTable = databaseForRow.GetDataTable("select ralPartWarehouseLocationID, ralPartBinID, ralQuantity, ralUnitPrice, ralCustomerPO " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from RMAClaimLines Inner Join RMAClaims On rapRMAClaimID = ralRMAClaimID where " + text + " order by ralRMAClaimID,ralRMAClaimLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select raoRMAClaimID, raoRMAClaimLineID, raoRMAClaimComponentID, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from RMAClaimComponents inner join RMAClaimLines on ralRMAClaimID=raoRMAClaimID and ralRMAClaimLineID=raoRMAClaimLineID where " + text + " and ralReceivedComplete = 0  order by raoRMAClaimID,raoRMAClaimLineID,raoRMAClaimComponentID");
		List<string> list = new List<string>();
		if (dataTable.Rows.Count != 0)
		{
			M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("SalesOrderLines");
			M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("SalesOrderDeliveries");
			M1BindingSource childBindingSource3 = childBindingSource2.PrimaryTable.GetChildBindingSource("SalesOrderComponents");
			foreach (DataRow row in dataTable.Rows)
			{
				if (!list.Contains(row.Field<string>("ralRMAClaimID")))
				{
					list.Add(row.Field<string>("ralRMAClaimID"));
				}
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
				BindingSource.SetKeyToNextAvailable(currentAsDataRow);
				addOrderLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, childBindingSource2, childBindingSource3, dataTable2, matchingFieldsInfo3);
				BindingSource.SaveData();
			}
		}
		object[] item;
		if (currentAsDataRow.RowState != DataRowState.Detached)
		{
			List<object[]> keysCreated = arg.KeysCreated;
			item = new string[1] { currentAsDataRow.Field<string>("ompSalesOrderID") };
			keysCreated.Add(item);
		}
		arg.OpenKeysWithObjectID = BindingSource.PrimaryTable.DefaultFormCollectionID;
		object[] parameters = ((currentAsDataRow.RowState == DataRowState.Detached) ? null : new object[1] { currentAsDataRow.Field<string>("ompSalesOrderID") });
		item = list.ToArray();
		arg.ActionMessagesArgs = new ActionMessagesEventArgs("PULLFROMRMACLAIM_FINISHED", parameters, item);
	}

	private void addOrderLine(M1Database database, DataRow soRow, M1BindingSource bsSOLines, DataRow lineRow, MatchingFieldsInfo lineMatches, M1BindingSource bsDeliveries, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo componentMatch)
	{
		DataRow dataRow = TransferLineInfo(this, lineRow, bsSOLines, lineMatches, soRow);
		decimal num = ((lineRow.Field<decimal>("ralQuantity") == 0m) ? 1m : lineRow.Field<decimal>("ralQuantity"));
		dataRow["omlOrderQuantity"] = num;
		dataRow["omlFullUnitPriceBase"] = lineRow["ralUnitPrice"];
		dataRow["omlUnitDiscountBase"] = 0;
		dataRow["omlUnitPriceBase"] = lineRow["ralUnitPrice"];
		DataRow[] array = bsDeliveries.GetDataTable().Select("omdSalesOrderID = " + dataRow.Field<string>("omlSalesOrderID").ToLinq() + " And omdSalesOrderLineID = " + dataRow.Field<short>("omlSalesOrderLineID").ToLinq());
		DataRow dataRow2 = ((array.Length != 0) ? array[0] : (bsDeliveries.AddNew(database, dataRow, null, null) as DataRow));
		if (dataRow2 != null)
		{
			dataRow2.SetField("omdDeliveryQuantity", num);
			dataRow2.SetField("omdPartWarehouseLocationID", lineRow.Field<string>("ralPartWarehouseLocationID"));
			dataRow2.SetField("omdPartBinID", lineRow.Field<string>("ralPartBinID"));
			if (bsComponents.Count != 0)
			{
				bsComponents.RemoveWhere(string.Empty, dataRow2);
			}
			DataRow[] array2 = dtComponents.Select("raoRMAClaimID = " + lineRow.Field<string>("ralRMAClaimID").Trim().ToLinq() + " and raoRMAClaimLineID = " + Convert.ToInt32(lineRow["ralRMAClaimLineID"]).ToLinq());
			foreach (DataRow sourceLineRow in array2)
			{
				TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow2);
			}
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		if (sourceHeaderRow["ralCustomerPO"] != DBNull.Value && (destinationHeaderRow["ompCustomerPO"] == DBNull.Value || string.IsNullOrWhiteSpace(destinationHeaderRow["ompCustomerPO"].ToString())))
		{
			destinationHeaderRow["ompCustomerPO"] = sourceHeaderRow["ralCustomerPO"];
		}
	}
}
