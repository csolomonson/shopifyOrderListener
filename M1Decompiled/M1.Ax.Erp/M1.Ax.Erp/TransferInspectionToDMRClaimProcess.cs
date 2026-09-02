using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferInspectionToDMRClaimProcess : ProcessParameters
{
	public TransferInspectionToDMRClaimProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "qalInspectionID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[2] { "qalInspectionID", "qalInspectionLineID" };
		KeyValueTableName = "InspectionLines";
		Description = "Select the inspections to be transferred to DMR claims.";
		GridID = "M1ADDFRODMRCLAIMINSPECTION";
		BindingSourceTable = "DMRClaims";
		HelpLink = "QM_CreateDMRFromInsp.htm";
		CreatedBindingSourceCaption = "Create DMR Claim from Inspection";
		PromptFieldValidations.Add(new PromptFieldValidationBool("qalTransferredToDMR", fieldValue: false, "Inspection is already transferred."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("qalInspectionComplete", fieldValue: false, "Inspection is already complete."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("qalPosted", fieldValue: true, "Inspection is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("qalReversed", fieldValue: false, "Inspection has been reversed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Supplier", null, new string[1] { "qalSupplierOrganizationID" })
		{
			ValueFields = new string[1] { "qalSupplierOrganizationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Location", null, new string[1] { "qalPurchaseLocationID" })
		{
			ValueFields = new string[1] { "qalPurchaseLocationID" }
		});
		HeaderSourceFields = new string[5] { "qalSupplierOrganizationID", "qalPurchaseLocationID", "qapProjectID", "qapPlantID", "qapPlantDepartmentID" };
		HeaderDestinationFields = new string[5] { "dmpSupplierOrganizationID", "dmpAPInvoiceLocationID", "dmpProjectID", "dmpPlantID", "dmpPlantDepartmentID" };
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		_ = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		BindingSource.SetKeyToNextAvailable(currentAsDataRow);
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("Inspections, InspectionLines", "DMRClaims", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("InspectionLines", "DMRClaimLines", new string[17]
		{
			"qalInspectionID", "qalInspectionLineID", "qalJobID", "qalJobAssemblyID", "qalJobMaterialID", "qalJobOperationID", "qalPartID", "qalPartRevisionID", "qalPartWarehouseLocationID", "qalPartBinID",
			"qalUnitOfMeasure", "qalPartShortDescription", "qalPartLongDescriptionRTF", "qalPartLongDescriptionText", "qalProjectID", "qalProjectAreaID", "qalKitPart"
		}, new string[17]
		{
			"dmlInspectionID", "dmlInspectionLineID", "dmlJobID", "dmlJobAssemblyID", "dmlJobMaterialID", "dmlJobOperationID", "dmlPartID", "dmlPartRevisionID", "dmlPartWarehouseLocationID", "dmlPartBinID",
			"dmlUnitOfMeasure", "dmlPartShortDescription", "dmlPartLongDescriptionRTF", "dmlPartLongDescriptionText", "dmlProjectID", "dmlProjectAreaID", "dmlKitPart"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("InspectionComponents, InspectionLines", "DMRClaimComponents", new string[16]
		{
			"qamInspectionID", "qamInspectionLineID", "qamInspectionComponentID", "qamJobID", "qamJobAssemblyID", "qamJobMaterialID", "qamJobMaterialComponentID", "qamPartID", "qamPartRevisionID", "qamPartWarehouseLocationID",
			"qamPartBinID", "qamQuantityPerParent", "qamAdditionalQuantity", "qamUnitOfMeasure", "qamDescription", "qamWeight"
		}, new string[16]
		{
			"dmoInspectionID", "dmoInspectionLineID", "dmoInspectionComponentID", "dmoJobID", "dmoJobAssemblyID", "dmoJobMaterialID", "dmoJobMaterialComponentID", "dmoPartID", "dmoPartRevisionID", "dmoPartWarehouseLocationID",
			"dmoPartBinID", "dmoQuantityPerParent", "dmoAdditionalQuantity", "dmoUnitOfMeasure", "dmoDescription", "dmoWeight"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select pmlPurchaseOrderID, pmlPurchaseOrderLineID, IsNull(rmpCurrencyRateID, '') As rmpCurrencyRateID, IsNull(rmpCustomRate, 0) As rmpCustomRate, IsNull(rmpExchangeRate, 1) As rmpExchangeRate, rmlReceiptID, rmlReceiptLineID,  isnull(rmlPurchaseUnitCost,0) as rmlPurchaseUnitCost, pmlOrgPartID, pmlOrgPartShortDescription, qalInspectionType, qalTransferredToDMR, qalInvQuantityToReturn, qalMfgReceiptQuantityToReturn, qalJobMatQuantityToReturn, qalJobOprQuantityToReturn,  rmlJobType, isnull(rmlSetupCharge,0) as rmlSetupCharge, isnull(rmlPurchaseQuantityReceived,0) as rmlPurchaseQuantityReceived " + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from InspectionLines inner join Inspections on qalInspectionID = qapInspectionID  left outer join ReceiptLines on qalSourceTableUniqueID = rmlUniqueID  left outer join Receipts on rmpReceiptID = rmlReceiptID  left outer join PurchaseOrderLines on rmlPurchaseOrderID = pmlPurchaseOrderID and rmlPurchaseOrderLineID = pmlPurchaseOrderLineID  left outer join PurchaseOrders on pmlPurchaseOrderID = pmpPurchaseOrderID  where " + text + " order by qalInspectionID, qalInspectionLineID");
		DataTable dataTable2 = databaseForRow.GetDataTable("select " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " from InspectionComponents inner join InspectionLines on qamInspectionID=qalInspectionID and qamInspectionLineID=qalInspectionLineID where " + text + " order by qamInspectionID,qamInspectionLineID,qamInspectionComponentID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("DMRClaimLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("DMRClaimComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addDMRShipmentLine(childBindingSource, row, currentAsDataRow, childBindingSource2, dataTable2, matchingFieldsInfo2, matchingFieldsInfo3, databaseForRow);
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderInfo(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		destinationHeaderRow["dmpCurrencyRateID"] = sourceHeaderRow["rmpCurrencyRateID"];
		destinationHeaderRow["dmpCustomRate"] = sourceHeaderRow["rmpCustomRate"];
		destinationHeaderRow["dmpExchangeRate"] = sourceHeaderRow["rmpExchangeRate"];
	}

	private void addDMRShipmentLine(M1BindingSource bsDMRClaimLines, DataRow inspectionLineRow, DataRow dmrClaimRow, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo lineMatches, MatchingFieldsInfo componentMatch, M1Database database)
	{
		DataRow dataRow = TransferLineInfo(this, inspectionLineRow, bsDMRClaimLines, lineMatches);
		if (inspectionLineRow.Field<byte>("qalInspectionType").Equals(1))
		{
			dataRow.SetField("dmlInventoryQuantity", inspectionLineRow.Field<decimal>("qalInvQuantityToReturn"));
		}
		else if (inspectionLineRow.Field<byte>("qalInspectionType").Equals(2))
		{
			if (!string.IsNullOrEmpty(dataRow.Field<string>("dmlJobID")))
			{
				if (dataRow.Field<int>("dmlJobMaterialID") != 0)
				{
					dataRow.SetField("dmlInventoryQuantity", inspectionLineRow.Field<decimal>("qalJobMatQuantityToReturn"));
				}
				else if (dataRow.Field<int>("dmlJobOperationID") != 0)
				{
					dataRow.SetField("dmlInventoryQuantity", inspectionLineRow.Field<decimal>("qalJobOprQuantityToReturn"));
				}
			}
		}
		else if (inspectionLineRow.Field<byte>("qalInspectionType").Equals(3))
		{
			dataRow.SetField("dmlInventoryQuantity", inspectionLineRow.Field<decimal>("qalMfgReceiptQuantityToReturn"));
		}
		if (!string.IsNullOrEmpty(inspectionLineRow.Field<string>("pmlPurchaseOrderID")))
		{
			dataRow.SetField("dmlPurchaseOrderID", inspectionLineRow.Field<string>("pmlPurchaseOrderID"));
		}
		if (inspectionLineRow["pmlPurchaseOrderLineID"] != DBNull.Value && inspectionLineRow.Field<short>("pmlPurchaseOrderLineID") != 0)
		{
			dataRow.SetField("dmlPurchaseOrderLineID", inspectionLineRow.Field<short>("pmlPurchaseOrderLineID"));
		}
		if (!string.IsNullOrEmpty(inspectionLineRow.Field<string>("rmlReceiptID")))
		{
			dataRow.SetField("dmlReceiptID", inspectionLineRow.Field<string>("rmlReceiptID"));
		}
		if (inspectionLineRow["rmlReceiptLineID"] != DBNull.Value && inspectionLineRow.Field<short>("rmlReceiptLineID") != 0)
		{
			dataRow.SetField("dmlReceiptLineID", inspectionLineRow.Field<short>("rmlReceiptLineID"));
		}
		Part part = new Part();
		decimal num = part.GetConversionFactor(database, dataRow.Field<string>("dmlPartID"), dataRow.Field<string>("dmlPartRevisionID"), "", "");
		if (num == 0m)
		{
			num = 1m;
		}
		dataRow.SetField("dmlConversionfactor", num);
		if (string.IsNullOrEmpty(dataRow.Field<string>("dmlPurchaseOrderID")))
		{
			dataRow.SetField("dmlUnitCost", part.GetPurchasePrice(database, dataRow.Field<string>("dmlPartID"), dataRow.Field<string>("dmlPartRevisionID"), "", "", 1m, "", "", null, 0m, null).FullPrice);
		}
		else if (inspectionLineRow.Field<byte>("rmlJobType").Equals(2))
		{
			dataRow.SetField("dmlUnitCost", inspectionLineRow.Field<decimal>("rmlPurchaseUnitCost") + Math.Round(inspectionLineRow.Field<decimal>("rmlSetupCharge") / inspectionLineRow.Field<decimal>("rmlPurchaseQuantityReceived"), 5));
		}
		else
		{
			dataRow.SetField("dmlUnitCost", inspectionLineRow.Field<decimal>("rmlPurchaseUnitCost"));
		}
		part = null;
		if (!string.IsNullOrEmpty(inspectionLineRow.Field<string>("pmlOrgPartID")))
		{
			dataRow.SetField("dmlOrgPartID", inspectionLineRow.Field<string>("pmlOrgPartID"));
		}
		if (!string.IsNullOrEmpty(inspectionLineRow.Field<string>("pmlOrgPartShortDescription")))
		{
			dataRow.SetField("dmlOrgPartShortDescription", inspectionLineRow.Field<string>("pmlOrgPartShortDescription"));
		}
		inspectionLineRow.SetField("qalTransferredToDMR", value: true);
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtComponents.Select("qamInspectionID = " + dataRow.Field<string>("dmlInspectionID").Trim().ToLinq() + " and qamInspectionLineID = " + Convert.ToInt32(dataRow["dmlInspectionLineID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}
}
