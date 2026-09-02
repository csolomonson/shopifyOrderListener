using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferCallToRMAClaimProcess : ProcessParameters
{
	public TransferCallToRMAClaimProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "kbpCallID" };
		PromptFieldAllowMultiples = true;
		KeyValueFieldNames = new string[1] { "kbpCallID" };
		KeyValueTableName = "Calls";
		Description = "Use this screen to create rma claims from a call.";
		CreatedBindingSourceCaption = "Create RMA Claim from Call";
		GridID = "M1ADDFROMRMACLAIMCALL";
		BindingSourceTable = "RMAClaims";
		HelpLink = "QM_TransferCallToRMAClaim.htm";
		HeaderSourceFields = new string[8] { "kbpOrganizationID", "kbpLocationID", "kbpContactID", "kbpOpenedDate", "kbpCurrencyRateID", "kbpCustomRate", "kbpExchangeRate", "kbpProjectID" };
		HeaderDestinationFields = new string[8] { "rapCustomerOrganizationID", "rapARInvoiceLocationID", "rapARInvoiceContactID", "rapClaimDate", "rapCurrencyRateID", "rapCustomRate", "rapExchangeRate", "rapProjectID" };
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
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("Calls", "RMAClaims", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("Calls", "RMAClaimLines", new string[5] { "kbpPartID", "kbpPartRevisionID", "kbpOrgPartID", "kbpPartShortDescription", "kbpProjectID" }, new string[5] { "ralPartID", "ralPartRevisionID", "ralOrgPartID", "ralPartShortDescription", "ralProjectID" });
		DataTable dataTable = databaseForRow.GetDataTable("select kbpCallID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " from Calls where " + text + " order by kbpCallID");
		MatchingFieldsInfo componentMatch = m1DataDictionary.FindMatchingFields("PartMaterials", "RMAClaimComponents", new string[6] { "immPartID", "immPartRevisionID", "immPartWarehouseLocationID", "immPartBinID", "immUnitOfMeasure", "immPartShortDescription" }, new string[6] { "raoPartID", "raoPartRevisionID", "raoPartWarehouseLocationID", "raoPartBinID", "raoUnitOfMeasure", "raoDescription" });
		DataTable dataTable2 = databaseForRow.GetDataTable("select immPartID, immPartRevisionID, immUnitOfMeasure, immPartShortDescription, immUseDefaultWarehouseAndBin, immPartWarehouseLocationID, immPartBinID  From PartMaterials  inner join PartRevisions on immPartID = imrPartID and immPartRevisionID = imrPartRevisionID  inner join Calls on immMethodID = kbpPartID and immMethodRevisionID = kbpPartRevisionID  inner join parts on impPartID = kbpPartID and impPhantomOrKitPart = 1  where " + text + " order by kbpCallID");
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("RMAClaimLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("RMAClaimComponents");
		foreach (DataRow row in dataTable.Rows)
		{
			CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
			addClaimLine(databaseForRow, currentAsDataRow, childBindingSource, row, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row), childBindingSource2, dataTable2, componentMatch);
		}
	}

	protected override void TransferHeaderOnKeyChange(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		base.TransferHeaderOnKeyChange(parm, sourceHeaderRow, headerFieldMatches, destinationHeaderRow);
		DataRow currentAsDataRow = parm.BindingSource.CurrentAsDataRow;
		currentAsDataRow["rapShipOrganizationID"] = sourceHeaderRow["kbpOrganizationID"];
		currentAsDataRow["rapShipLocationID"] = sourceHeaderRow["kbpLocationID"];
		currentAsDataRow["rapShipContactID"] = sourceHeaderRow["kbpContactID"];
	}

	private void addClaimLine(M1Database database, DataRow claimRow, M1BindingSource bsClaimLines, DataRow callRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues, M1BindingSource bsComponents, DataTable dtComponents, MatchingFieldsInfo componentMatch)
	{
		DataRow dataRow = TransferLineInfo(this, callRow, bsClaimLines, lineMatches);
		string partID = dataRow.Field<string>("ralPartID");
		string partRevisionID = dataRow.Field<string>("ralPartRevisionID");
		string plantID = "";
		FieldDefinition fieldDefinition = bsClaimLines.Fields["ralPartID"];
		if (fieldDefinition.Table.GetDocumentPlantID(database, claimRow, null) != null)
		{
			plantID = fieldDefinition.Table.GetDocumentPlantID(database, claimRow, null);
		}
		string returnWarehouseID = "";
		string returnWarehouseBinID = "";
		string returnMessage = "";
		if (new Part().InitializeWarehouseBinForPartRev(database, partID, partRevisionID, plantID, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage))
		{
			dataRow["ralPartWarehouseLocationID"] = returnWarehouseID;
			dataRow["ralPartBinID"] = returnWarehouseBinID;
		}
		dataRow["ralQuantity"] = 1;
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		foreach (DataRow row in dtComponents.Rows)
		{
			DataRow dataRow3 = TransferLineInfo(this, row, bsComponents, componentMatch, dataRow);
			if (row.Field<bool>("immUseDefaultWarehouseAndBin"))
			{
				partID = dataRow3.Field<string>("raoPartID");
				partRevisionID = dataRow3.Field<string>("raoPartRevisionID");
				if (new Part().InitializeWarehouseBinForPartRev(database, partID, partRevisionID, plantID, ref returnWarehouseID, ref returnWarehouseBinID, ref returnMessage))
				{
					dataRow3["raoPartWarehouseLocationID"] = returnWarehouseID;
					dataRow3["raoPartBinID"] = returnWarehouseBinID;
				}
			}
		}
	}
}
