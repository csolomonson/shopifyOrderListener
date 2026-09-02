using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferPurchasePlannerToPurchaseOrderProcess : ProcessParameters
{
	public TransferPurchasePlannerToPurchaseOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "ppsSessionID" };
		PromptFieldAllowMultiples = false;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[3] { "ppoSessionID", "ppoLineID", "ppoOrderDetailID" };
		KeyValueTableName = "PurchasePlannerOrderDetails";
		Description = "Select the Purchase Planner lines to create purchase orders from.";
		GridID = "M1ADDFROMPOPURCHPLANNER";
		BindingSourceTable = "PurchaseOrders";
		ContinueMessage = "This will create purchase orders from the {0} selected Purchase Planner Order Details. Are you sure you want to continue?";
		CreatedBindingSourceCaption = "Create Purchase Order from Purchase Planner Order Details";
		HeaderSourceFields = new string[6] { "pplPlantID", "ppoSupplierOrganizationID", "ppoPurchaseLocationID", "cmoDefaultAPInvoiceLocationID", "ppoCurrencyRateID", "ppsBuyerEmployeeID" };
		HeaderDestinationFields = new string[6] { "pmpPlantID", "pmpSupplierOrganizationID", "pmpPurchaseLocationID", "pmpAPInvoiceLocationID", "pmpCurrencyRateID", "pmpBuyerEmployeeID" };
		PromptFieldValidations.Add(new PromptFieldValidationBool("ppsCompleted", fieldValue: false, "Purchase Planner Session is completed."));
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.DefaultFieldValues;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow dataRow = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("PurchasePlannerSessions, PurchasePlannerLines, PurchasePlannerOrderDetails, Organizations", "PurchaseOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("PurchasePlannerOrderDetails, PartRevisions, PartCrossReferences", "PurchaseOrderLines", new string[24]
		{
			"ppoProjectID", "ppoProjectAreaID", "ppoPurchaseType", "ppoJobID", "ppoJobAssemblyID", "ppoJobMaterialID", "ppoSalesOrderID", "ppoSalesOrderLineID", "ppoSalesOrderDeliveryID", "ppoDueDate",
			"ppoPartID", "ppoPartRevisionID", "ppoPartWarehouseLocationID", "ppoPartBinID", "ppoPurchaseUnitOfMeasure", "ppoInventoryUnitOfMeasure", "imrDocuments", "ppoLeadTime", "ppoConversionFactor", "ppoInventoryQuantity",
			"ppoPurchaseQuantity", "ppoUnitCostBase", "ppoUnitCostForeign", "ppoSupplierRequirement"
		}, new string[24]
		{
			"pmlProjectID", "pmlProjectAreaID", "pmlPurchaseType", "pmlJobID", "pmlJobAssemblyID", "pmlJobMaterialID", "pmlSalesOrderID", "pmlSalesOrderLineID", "pmlSalesOrderDeliveryID", "pmlDueDate",
			"pmlPartID", "pmlPartRevisionID", "pmlPartWarehouseLocationID", "pmlPartBinID", "pmlPurchaseUnitOfMeasure", "pmlInventoryUnitOfMeasure", "pmlDocuments", "pmlLeadTime", "pmlConversionFactor", "pmlInventoryQuantity",
			"pmlPurchaseQuantity", "pmlPurchaseUnitCostBase", "pmlPurchaseUnitCostForeign", "pmlSupplierRequirement"
		});
		DataTable dataTable = database.GetDataTable("select ppoSessionID,ppoLineID,ppoOrderDetailID,cmoSupplierPaymentTermID,cmoSupplierTaxable,cmoSupplierTaxCodeID,cmoSupplierSecondTaxCodeID,cmoForm1099Box,cmoSupplierShippingMethodID,IsNull(jmmDocuments, imrDocuments) as imrDocuments,ppoUniqueID,CASE WHEN imxOrgPartID IS NULL THEN CASE WHEN imzOrgPartID IS NULL THEN '' ELSE imzOrgPartID END ELSE imxOrgPartID END AS imxOrgPartID,CASE WHEN imxOrgPartShortDescription IS NULL THEN CASE WHEN imzOrgPartShortDescription IS NULL THEN '' ELSE imzOrgPartShortDescription END ELSE imxOrgPartShortDescription END AS imxOrgPartShortDescription" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from PurchasePlannerSessions inner join PurchasePlannerLines on ppsSessionID = pplSessionID inner join PurchasePlannerOrderDetails on pplSessionID = ppoSessionID and pplLineID = ppoLineID  left outer join Organizations on cmoOrganizationID = ppoSupplierOrganizationID left outer join OrganizationLocations on cmlOrganizationID = ppoSupplierOrganizationID and cmlLocationID = ppoPurchaseLocationID left outer join PartRevisions on ppoPartID = imrPartID and ppoPartRevisionID = imrPartRevisionID Left Outer Join PartOrgReferences on imzPartID=imrPartID and imzPartRevisionID=imrPartRevisionID and imzOrganizationID=ppoSupplierOrganizationID left outer join PartCrossReferences on imxPartID=imrPartID and imxPartRevisionID=imrPartRevisionID and imxOrganizationID=ppoSupplierOrganizationID and imxLocationID=ppoPurchaseLocationID left outer join JobMaterials on ppoJobID=jmmJobID and ppoJobAssemblyID=jmmJobAssemblyID and ppoJobMaterialID=jmmJobMaterialID  where " + text + " and ppoSupplierOrganizationID <> '' order by ppoSupplierOrganizationID,ppoPurchaseLocationID,pplPlantID,ppoCurrencyRateID,ppoSessionID,ppoLineID,ppoOrderDetailID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderLines");
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		string value4 = string.Empty;
		_ = string.Empty;
		string text2 = string.Empty;
		string empty = string.Empty;
		foreach (DataRow row in dataTable.Rows)
		{
			if (!row.Field<string>("ppoSupplierOrganizationID").Equals(value, StringComparison.CurrentCultureIgnoreCase) || !row.Field<string>("ppoPurchaseLocationID").Equals(value2, StringComparison.CurrentCultureIgnoreCase) || !row.Field<string>("pplPlantID").Equals(value3, StringComparison.CurrentCultureIgnoreCase) || !row.Field<string>("ppoCurrencyRateID").Equals(value4, StringComparison.CurrentCultureIgnoreCase))
			{
				value = string.Empty;
				value2 = string.Empty;
				value3 = string.Empty;
				value4 = string.Empty;
				_ = string.Empty;
				text2 = string.Empty;
			}
			if (text2 == string.Empty)
			{
				dataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(dataRow);
				BindingSource.ActivateRow(dataRow, null, doFlash: false);
				empty = dataRow.Field<string>("pmpPurchaseOrderID");
			}
			else
			{
				empty = text2;
			}
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, dataRow);
			addPOLine(childBindingSource, row, dataRow, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row));
			value = row.Field<string>("ppoSupplierOrganizationID");
			value2 = row.Field<string>("ppoPurchaseLocationID");
			value3 = row.Field<string>("pplPlantID");
			value4 = row.Field<string>("ppoCurrencyRateID");
			row.Field<string>("ppoSessionID");
			if (!text2.Equals(empty, StringComparison.CurrentCultureIgnoreCase))
			{
				text2 = empty;
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "PO";
		}
	}

	private void addPOLine(M1BindingSource bsPOLines, DataRow plannerDetailRow, DataRow poRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, plannerDetailRow, bsPOLines, lineMatches, poRow);
		if (!dataRow["pmlJobMaterialID"].Equals(0))
		{
			dataRow["pmlJobType"] = 1;
		}
		dataRow["pmlOrgPartID"] = plannerDetailRow["imxOrgPartID"];
		dataRow["pmlOrgPartShortDescription"] = plannerDetailRow["imxOrgPartShortDescription"];
		dataRow["pmlSourceTableName"] = "PurchasePlannerOrderDetails";
		dataRow["pmlSourceTableUniqueID"] = plannerDetailRow.Field<Guid>("ppoUniqueID");
	}
}
