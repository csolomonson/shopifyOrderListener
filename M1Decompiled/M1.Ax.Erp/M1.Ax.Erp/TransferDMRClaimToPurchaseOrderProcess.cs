using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferDMRClaimToPurchaseOrderProcess : ProcessParameters
{
	public TransferDMRClaimToPurchaseOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "dmlDMRClaimID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "dmlDMRClaimID", "dmlDMRClaimLineID" };
		KeyValueTableName = "DMRClaimLines";
		Description = "Select the DMR claim lines to be transferred to a purchase order.";
		GridID = "M1ADDFROMPODMRCLAIM";
		BindingSourceTable = "PurchaseOrders";
		HelpLink = "PM_CreatePOfromDMR.htm";
		ContinueMessage = "This will create purchase orders from the {0} selected DMR Claim(s). Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create Purchase Order from DMR Claim";
		PromptFieldValidations.Add(new PromptFieldValidationBool("dmlTransferredtoDMRshipment", fieldValue: false, "DMR Claim is already shipped."));
		HeaderSourceFields = new string[11]
		{
			"dmpSupplierOrganizationID", "dmpPurchaseLocationID", "dmpPurchaseContactID", "dmpAPInvoiceLocationID", "dmpAPInvoiceContactID", "dmpProjectID", "dmpPlantID", "dmpPlantDepartmentID", "dmpCurrencyRateID", "dmpCustomRate",
			"dmpExchangeRate"
		};
		HeaderDestinationFields = new string[11]
		{
			"pmpSupplierOrganizationID", "pmpPurchaseLocationID", "pmpPurchaseContactID", "pmpAPInvoiceLocationID", "pmpAPInvoiceContactID", "pmpProjectID", "pmpPlantID", "pmpPlantDepartmentID", "pmpCurrencyRateID", "pmpCustomRate",
			"pmpExchangeRate"
		};
		DefaultValueFieldNames = new string[1] { "ProductionProperties.xapPMDefaultDueDate" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		Dictionary<string, object> defaultFieldValues = arg.DefaultFieldValues;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		DateTime defaultDueDate = Convert.ToDateTime(defaultFieldValues["xapPMDefaultDueDate"]);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("DMRClaimLines, DMRClaims, PurchaseOrderLines", "PurchaseOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("RFQLines, RFQSuppliers", "PurchaseOrderLines", new string[15]
		{
			"dmlDMRClaimID", "dmlDMRClaimLineID", "dmlPartID", "dmlPartRevisionID", "dmlPartWarehouseLocationID", "dmlPartBinID", "dmlOrgPartID", "dmlOrgPartShortDescription", "dmlPartShortDescription", "dmlPartLongDescriptionRTF",
			"dmlPartLongDescriptionText", "dmlInventoryUnitOfMeasure", "dmlProjectID", "dmlProjectAreaID", "dmlKitPart"
		}, new string[15]
		{
			"pmlDMRClaimID", "pmlDMRClaimLineID", "pmlPartID", "pmlPartRevisionID", "pmlPartWarehouseLocationID", "pmlPartBinID", "pmlOrgPartID", "pmlOrgPartShortDescription", "pmlPartShortDescription", "pmlPartLongDescriptionRTF",
			"pmlPartLongDescriptionText", "pmlInventoryUnitOfMeasure", "pmlProjectID", "pmlProjectAreaID", "pmlKitPart"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("DMRClaimComponents, DMRClaimLines", "PurchaseOrderComponents", new string[13]
		{
			"dmoJobID", "dmoJobAssemblyID", "dmoJobMaterialID", "dmoJobMaterialComponentID", "dmoPartID", "dmoPartRevisionID", "dmoPartWarehouseLocationID", "dmoPartBinID", "dmoQuantityPerParent", "dmoAdditionalQuantity",
			"dmoUnitOfMeasure", "dmoDescription", "dmoWeight"
		}, new string[13]
		{
			"pmoJobID", "pmoJobAssemblyID", "pmoJobMaterialID", "pmoJobMaterialComponentID", "pmoPartID", "pmoPartRevisionID", "pmoPartWarehouseLocationID", "pmoPartBinID", "pmoQuantityPerParent", "pmoAdditionalQuantity",
			"pmoUnitOfMeasure", "pmoDescription", "pmoWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select dmlUnitCostForeign, pmlPurchaseUnitCostForeign, dmlQuantity, pmlPurchaseQuantity, pmlPurchaseType, pmlJobID, pmlJobAssemblyID, pmlJobMaterialID, pmlJobOperationID, pmlJobType,  dmlJobID, dmlJobAssemblyID, dmlJobMaterialID, pmlPurchaseUnitOfMeasure, dmlUnitOfMeasure, pmlPurchaseOrderID, pmlPurchaseOrderLineID,  pmlTaxable, pmlTaxCodeID, pmlSecondTaxCodeID, pmlLeadTime, pmlDocuments, pmlConversionFactor, dmlConversionFactor, pmlPurchaseOrderID, pmlPurchaseOrderLineID  pmlSourcePurchaseOrderLineID, pmlSourcePurchaseOrderLineID, dmlPurchaseOrderID, dmlPurchaseOrderLineID, pmlSalesOrderID, pmlSalesOrderLineID, pmlSalesOrderDeliveryID " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from DMRClaimLines inner join DMRClaims on dmlDMRClaimID = dmpDMRClaimID  left outer join PurchaseOrderLines on dmlPurchaseOrderID = pmlPurchaseOrderID and dmlPurchaseOrderLineID = pmlPurchaseOrderLineID  where " + text + " order by dmlDMRClaimID, dmlDMRClaimLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select dmoDMRClaimID, dmoDMRClaimLineID, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from DMRClaimComponents inner join DMRClaimLines on dmoDMRClaimID=dmlDMRClaimID and dmoDMRClaimLineID=dmlDMRClaimLineID where " + text + " and dmlInvoicedComplete = 0 order by dmoDMRClaimID,dmoDMRClaimLineID,dmoDMRClaimComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderComponents");
		bool flag = false;
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			flag = ((!string.IsNullOrWhiteSpace(row.Field<string>("dmlPurchaseOrderID")) && row.Field<short>("dmlPurchaseOrderLineID") != 0) ? true : false);
			addPOLine(childBindingSource, row, currentAsDataRow, matchingFieldsInfo2, childBindingSource2, dataTable2, matchingFieldsInfo3, flag, defaultDueDate);
		}
	}

	private void addPOLine(M1BindingSource bsPOLines, DataRow claimLineRow, DataRow poRow, MatchingFieldsInfo lineMatches, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo componentMatch, bool addFromPO, DateTime defaultDueDate)
	{
		DataRow dataRow = TransferLineInfo(this, claimLineRow, bsPOLines, lineMatches, poRow);
		if (addFromPO)
		{
			dataRow["pmlPurchaseType"] = claimLineRow["pmlPurchaseType"];
			dataRow["pmlJobID"] = claimLineRow["pmlJobID"];
			dataRow["pmlJobAssemblyID"] = claimLineRow["pmlJobAssemblyID"];
			dataRow["pmlJobType"] = claimLineRow["pmlJobType"];
			if (claimLineRow.Field<byte>("pmlJobType") == 1)
			{
				dataRow["pmlJobMaterialID"] = claimLineRow["pmlJobMaterialID"];
			}
			else
			{
				dataRow["pmlJobOperationID"] = claimLineRow["pmlJobOperationID"];
			}
			dataRow["pmlSalesOrderID"] = claimLineRow["pmlSalesOrderID"];
			dataRow["pmlSalesOrderLineID"] = claimLineRow["pmlSalesOrderLineID"];
			dataRow["pmlSalesOrderDeliveryID"] = claimLineRow["pmlSalesOrderDeliveryID"];
			dataRow["pmlPurchaseUnitOfMeasure"] = claimLineRow["pmlPurchaseUnitOfMeasure"];
			dataRow["pmlSourcePurchaseOrderID"] = claimLineRow["pmlPurchaseOrderID"];
			dataRow["pmlSourcePurchaseOrderLineID"] = claimLineRow["pmlPurchaseOrderLineID"];
			dataRow["pmlPurchaseQuantity"] = claimLineRow["dmlQuantity"];
			dataRow["pmlPurchaseUnitCostForeign"] = M1Math.Round(Convert.ToDecimal(claimLineRow["pmlPurchaseUnitCostForeign"]), 5);
			dataRow["pmlTaxable"] = claimLineRow["pmlTaxable"];
			dataRow["pmlTaxCodeID"] = claimLineRow["pmlTaxCodeID"];
			dataRow["pmlSecondTaxCodeID"] = claimLineRow["pmlSecondTaxCodeID"];
			dataRow["pmlLeadTime"] = claimLineRow["pmlLeadTime"];
			dataRow["pmlDocuments"] = claimLineRow["pmlDocuments"];
			dataRow["pmlConversionFactor"] = claimLineRow["pmlConversionFactor"];
		}
		else
		{
			dataRow["pmlJobID"] = claimLineRow["dmlJobID"];
			dataRow["pmlJobAssemblyID"] = claimLineRow["dmlJobAssemblyID"];
			dataRow["pmlJobMaterialID"] = claimLineRow["dmlJobMaterialID"];
			if (claimLineRow.Field<int>("dmlJobMaterialID") != 0)
			{
				dataRow["pmlPurchaseType"] = 1;
				dataRow["pmlJobType"] = 1;
			}
			else
			{
				dataRow["pmlPurchaseType"] = 2;
			}
			dataRow["pmlPurchaseUnitOfMeasure"] = claimLineRow["dmlUnitOfMeasure"];
			dataRow["pmlPurchaseQuantity"] = claimLineRow["dmlQuantity"];
			dataRow["pmlPurchaseUnitCostForeign"] = claimLineRow["dmlUnitCostForeign"];
			dataRow["pmlConversionFactor"] = claimLineRow["dmlConversionFactor"];
		}
		poRow["pmpDueDate"] = defaultDueDate;
		dataRow["pmlDueDate"] = defaultDueDate;
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("dmoDMRClaimID = " + dataRow.Field<string>("pmlDMRClaimID").Trim().ToLinq() + " and dmoDMRClaimLineID = " + Convert.ToInt32(dataRow["pmlDMRClaimLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}
}
