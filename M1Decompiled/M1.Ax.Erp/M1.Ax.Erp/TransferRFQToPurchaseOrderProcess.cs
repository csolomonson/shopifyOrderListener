using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferRFQToPurchaseOrderProcess : ProcessParameters
{
	public TransferRFQToPurchaseOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "rqsRFQID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[3] { "rqsRFQID", "rqsRFQLineID", "rqsRFQSupplierID" };
		KeyValueTableName = "PurchaseOrderLines";
		Description = "Select the RFQ supplier lines to create purchase orders from.";
		GridID = "M1ADDFROMPORFQSUPPLIERS";
		BindingSourceTable = "PurchaseOrders";
		ContinueMessage = "This will create purchase orders from the {0} selected RFQ(s). Are you sure you want to continue?";
		HelpLink = "PM_CreatePOfromRFQ.htm";
		CreatedBindingSourceCaption = "Create Purchase Order from RFQ";
		PromptFieldValidations.Add(new PromptFieldValidationBool("rqsClosed", fieldValue: false, "RFQ is closed."));
		HeaderSourceFields = new string[12]
		{
			"rqsSupplierOrganizationID", "rqsPurchaseLocationID", "cmoDefaultAPInvoiceLocationID", "rqsPurchaseContactID", "rqsCurrencyRateID", "rqsCustomRate", "rqsExchangeRate", "rqlProjectID", "rqpPlantID", "rqpPlantDepartmentID",
			"cmoSupplierPaymentTermID", "cmoSupplierShippingMethodID"
		};
		HeaderDestinationFields = new string[12]
		{
			"pmpSupplierOrganizationID", "pmpPurchaseLocationID", "pmpAPInvoiceLocationID", "pmpPurchaseContactID", "pmpCurrencyRateID", "pmpCustomRate", "pmpExchangeRate", "pmpProjectID", "pmpPlantID", "pmpPlantDepartmentID",
			"pmpPaymentTermID", "pmpShippingMethodID"
		};
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Part ID", null, new string[1] { "rqlPartID" })
		{
			ValueFields = new string[1] { "rqlPartID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Show completed details only?")
		{
			AdoFilterExpression = "rqsComplete <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "rqsComplete"
		});
		DefaultValueFieldNames = new string[2] { "ProductionProperties.xapPMDefaultDueDate", "ProductionProperties.xapRQGroupPObyRFQ" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		Dictionary<string, object> defaultFieldValues = arg.DefaultFieldValues;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow dataRow = BindingSource.CurrentAsDataRow;
		M1Database database = BindingSource.Database;
		DateTime defaultDueDate = Convert.ToDateTime(defaultFieldValues["xapPMDefaultDueDate"]);
		bool flag = Convert.ToBoolean(defaultFieldValues["xapRQGroupPObyRFQ"]);
		M1DataDictionary obj = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("RFQs, RFQLines, RFQSuppliers, Organizations", "PurchaseOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("RFQLines, RFQSuppliers", "PurchaseOrderLines", new string[14]
		{
			"rqsRFQID", "rqsRFQLineID", "rqlPartID", "rqlPartRevisionID", "rqsOrgPartID", "rqlPartShortDescription", "rqlPartLongDescriptionRTF", "rqlPartLongDescriptionText", "rqlPurchaseUnitOfMeasure", "rqlInventoryUnitOfMeasure",
			"cmoForm1099Box", "rqlDocuments", "rqlProjectID", "rqlProjectAreaID"
		}, new string[14]
		{
			"pmlRFQID", "pmlRFQLineID", "pmlPartID", "pmlPartRevisionID", "pmlOrgPartID", "pmlPartShortDescription", "pmlPartLongDescriptionRTF", "pmlPartLongDescriptionText", "pmlPurchaseUnitOfMeasure", "pmlInventoryUnitOfMeasure",
			"pmlForm1099Box", "pmlDocuments", "pmlProjectID", "pmlProjectAreaID"
		});
		DataTable dataTable = database.GetDataTable("select rqsClosed,rqlRFQType,rqlPartID,rqlPartRevisionID,rqlPartShortDescription,rqlPartLongDescriptionRTF,rqlPartLongDescriptionText,rqlPurchaseUnitOfMeasure,rqlInventoryUnitOfMeasure,rqsRFQSupplierID,rqsPurchaseContactID,rqsOrgPartID,rqsCurrencyRateID,rqsCustomRate,rqsExchangeRate,cmoSupplierPaymentTermID,IsNull((Select Top 1 xazExpenseGLAccountID From ExpenseAccountSplits Where xazSupplierOrganizationID = cmoOrganizationID Order By xazExpenseGLAccountID),'') As xazExpenseGLAccountID,cmoSupplierTaxable,cmoSupplierTaxCodeID,cmoSupplierSecondTaxCodeID,cmoForm1099Box,cmoSupplierShippingMethodID,rqsDueDate,rqsComplete,rqlJobID,rqlJobAssemblyID,rqlJobMaterialID,rqlJobOperationID,IsNull(IsNull(imxConversionFactor,IsNull(imzConversionFactor,imrConversionFactor)),1) as imrConversionFactor,imrLeadTime,rqlDocuments,rqpPlantDepartmentID,rqpPlantID,IsNull((Select Top 1 xazExpenseGLAccountID From ExpenseAccountSplits Where xazPartID = imrPartID And xazPartRevisionID = imrPartRevisionID Order By xazExpenseGLAccountID),'') As xazExpenseGLAccountIDPart,rqlProjectID,rqlProjectAreaID, cmoOrganizationID,cmlLocationID,cmoPurchaseContactID,rqpPlantID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from RFQSuppliers inner join RFQLines on rqsRFQID = rqlRFQID and rqsRFQLineID = rqlRFQLineID LEFT OUTER JOIN RFQs on rqlRFQID = rqpRFQID left outer join Organizations on cmoOrganizationID = rqsSupplierOrganizationID left outer join OrganizationLocations on cmlOrganizationID = rqsSupplierOrganizationID and cmlLocationID = rqsPurchaseLocationID left outer join PartRevisions on rqlPartID = imrPartID and rqlPartRevisionID = imrPartRevisionID Left Outer Join PartOrgReferences on imzPartID=imrPartID and imzPartRevisionID=imrPartRevisionID and imzOrganizationID=rqsSupplierOrganizationID left outer join PartCrossReferences on imxPartID=imrPartID and imxPartRevisionID=imrPartRevisionID and imxOrganizationID=rqsSupplierOrganizationID and imxLocationID=rqsPurchaseLocationID where " + text + " order by rqsSupplierOrganizationID,rqsPurchaseLocationID,rqsRFQID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("PurchaseOrderLines");
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		string text2 = string.Empty;
		string empty = string.Empty;
		foreach (DataRow row in dataTable.Rows)
		{
			if (Convert.ToDecimal(GetItemValuesFromList(selectedItems, row).EditableValues["PurchaseQty"]) != 0m)
			{
				if (!row.Field<string>("rqsSupplierOrganizationID").Equals(value, StringComparison.CurrentCultureIgnoreCase) || !row.Field<string>("rqsPurchaseLocationID").Equals(value2, StringComparison.CurrentCultureIgnoreCase))
				{
					value = string.Empty;
					value2 = string.Empty;
					value3 = string.Empty;
					text2 = string.Empty;
				}
				else if (flag && !row.Field<string>("rqsRFQID").Equals(value3, StringComparison.CurrentCultureIgnoreCase))
				{
					value = string.Empty;
					value2 = string.Empty;
					value3 = string.Empty;
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
				addPOLine(childBindingSource, row, dataRow, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row), defaultDueDate);
				value = row.Field<string>("rqsSupplierOrganizationID");
				value2 = row.Field<string>("rqsPurchaseLocationID");
				value3 = row.Field<string>("rqsRFQID");
				if (!text2.Equals(empty, StringComparison.CurrentCultureIgnoreCase))
				{
					text2 = empty;
				}
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
			else
			{
				messages.Add("RFQ Supplier " + row.Field<string>("rqsRFQID").Trim() + "/" + row.Field<short>("rqsRFQLineID") + "/" + row.Field<short>("rqsRFQSupplierID") + " was not added because no purchase quantity was entered.");
			}
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "PO";
		}
	}

	private void addPOLine(M1BindingSource bsPOLines, DataRow rfqRow, DataRow poRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues, DateTime defaultDueDate)
	{
		DataRow dataRow = TransferLineInfo(this, rfqRow, bsPOLines, lineMatches, poRow);
		dataRow["pmlPartWarehouseLocationID"] = new Part().GetPreferredWarehouse(bsPOLines.Database, dataRow["pmlPartID"].ToString(), dataRow["pmlPartRevisionID"].ToString(), rfqRow["rqpPlantID"].ToString());
		dataRow["pmlPartBinID"] = new Part().GetPreferredWarehouseBin(bsPOLines.Database, dataRow["pmlPartID"].ToString(), dataRow["pmlPartRevisionID"].ToString(), dataRow["pmlPartWarehouseLocationID"].ToString(), rfqRow["rqpPlantID"].ToString());
		if (!string.IsNullOrWhiteSpace(rfqRow.Field<string>("rqlJobID")))
		{
			dataRow["pmlJobID"] = rfqRow["rqlJobID"];
			dataRow["pmlJobAssemblyID"] = rfqRow["rqlJobAssemblyID"];
			dataRow["pmlPurchaseType"] = 1;
			if (rfqRow.Field<byte>("rqlRFQType") == 1)
			{
				dataRow["pmlJobType"] = 1;
				dataRow["pmlJobMaterialID"] = rfqRow["rqlJobMaterialID"];
			}
			else
			{
				dataRow["pmlJobType"] = 2;
				dataRow["pmlJobOperationID"] = rfqRow["rqlJobOperationID"];
			}
		}
		else
		{
			dataRow["pmlPurchaseType"] = 2;
		}
		if (!rfqRow.IsNull("cmoSupplierTaxable"))
		{
			dataRow["pmlTaxable"] = rfqRow["cmoSupplierTaxable"];
			dataRow["pmlTaxCodeID"] = rfqRow["cmoSupplierTaxCodeID"];
			dataRow["pmlSecondTaxCodeID"] = rfqRow["cmoSupplierSecondTaxCodeID"];
		}
		if (itemValues.EditableValues.ContainsKey("PurchaseQty"))
		{
			dataRow.SetField("pmlPurchaseQuantity", Convert.ToDecimal(itemValues.EditableValues["PurchaseQty"]));
		}
		if (itemValues.EditableValues.ContainsKey("ConversionFactor"))
		{
			dataRow.SetField("pmlConversionFactor", Convert.ToDecimal(itemValues.EditableValues["ConversionFactor"]));
		}
		if (itemValues.EditableValues.ContainsKey("PurchaseUnitCost"))
		{
			dataRow.SetField("pmlPurchaseUnitCostBase", Convert.ToDecimal(itemValues.EditableValues["PurchaseUnitCost"]));
		}
		if (itemValues.EditableValues.ContainsKey("PurchaseLeadTime"))
		{
			dataRow.SetField("pmlLeadTime", Convert.ToInt16(itemValues.EditableValues["PurchaseLeadTime"]));
		}
		poRow["pmpDueDate"] = defaultDueDate;
		dataRow["pmlDueDate"] = poRow["pmpDueDate"];
	}
}
