using System;
using System.Collections.Generic;
using System.Data;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferJobOperationToPurchaseOrderProcess : ProcessParameters
{
	public TransferJobOperationToPurchaseOrderProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[2] { "jmoJobID", "jmoJobAssemblyID" };
		PromptFieldAllowMultiples = false;
		KeyValueFieldNames = new string[3] { "jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID" };
		KeyValueTableName = "JobOperations";
		Description = "Select the outside job operations to be purchased.";
		CreatedBindingSourceCaption = "Create Purchase Orders from Job Operations";
		GridID = "M1ADDFROMPOJOBOPR";
		BindingSourceTable = "PurchaseOrders";
		HelpLink = "PM_CreatePO_JobOps.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmoClosed", fieldValue: false, "Job is closed."));
		ContinueMessage = "This will create purchase orders from the {0} selected job operations. Are you sure you want to continue?";
		MultipleDestinationRowsCreated = true;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Due Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "jmoDueDate",
			AdditionalFields = "jmoDueDate"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Add Firm Operations Only?")
		{
			AdoFilterExpression = "jmoFirm <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "jmoFirm"
		});
		HeaderSourceFields = new string[4] { "jmpPlantID", "jmpProjectID", "jmoSupplierOrganizationID", "jmoPurchaseLocationID" };
		HeaderDestinationFields = new string[4] { "pmpPlantID", "pmpProjectID", "pmpSupplierOrganizationID", "pmpPurchaseLocationID" };
	}

	public override void ConstructPromptFieldsWhere(object sender, PromptFieldsWhereEventArgs e)
	{
		if (e.KeyValues.Count != 0)
		{
			e.Where = "jmoJobID = " + M1Util.ConvertToSql(e.KeyValues[0][0]) + " And jmoJobID+Convert(varchar(10),jmoJobAssemblyID) In (Select MyCTE.jmaJobID+Convert(varchar(10),MyCTE.jmaJobAssemblyID) From MyCTE)";
			e.QueryFormat = "With MyCTE As (Select jmaJobID,jmaJobAssemblyID From JobAssemblies Where jmaJobID = " + M1Util.ConvertToSql(e.KeyValues[0][0]) + " And jmaJobAssemblyID = " + M1Util.ConvertToSql(e.KeyValues[0][1]) + " Union All Select JobAssemblies.jmaJobID,JobAssemblies.jmaJobAssemblyID From JobAssemblies Inner Join MyCTE On JobAssemblies.jmaJobID = MyCTE.jmaJobID And JobAssemblies.jmaParentAssemblyID=MyCTE.jmaJobAssemblyID Where JobAssemblies.jmaJobAssemblyID <> 0 ) {0}";
		}
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
		MatchingFieldsInfo matchingFieldsInfo = obj.FindMatchingFields("Jobs, JobOperations", "PurchaseOrders", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = obj.FindMatchingFields("Jobs, JobOperations", "PurchaseOrderLines", new string[7] { "PurchaseType=Convert(int, 1)", "jmpProjectID", "jmpProjectAreaID", "jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID", "JobType=Convert(int, 2)" }, new string[7] { "pmlPurchaseType", "pmlProjectID", "pmlProjectAreaID", "pmlJobID", "pmlJobAssemblyID", "pmlJobOperationID", "pmlJobType" });
		DataTable dataTable = database.GetDataTable("select jmoUniqueID" + matchingFieldsInfo2.GetSourceFieldList(",", string.Empty) + matchingFieldsInfo.GetSourceFieldList(",", string.Empty) + " FROM JobOperations  left outer join Jobs On jmoJobID = jmpJobID  left join PurchaseOrderLines on jmoPurchaseOrderID = pmlPurchaseOrderID  AND jmoJobID = pmlJobID AND jmoJobAssemblyID = pmlJobAssemblyID AND jmoJobOperationID = pmlJobOperationID  WHERE jmoClosed = 0 AND jmoProductionComplete = 0 AND jmoOperationType = 2  AND (jmoOperationQuantity - jmoQuantityComplete) > (SELECT ISNULL(SUM(pmlInventoryQuantity - pmlInventoryQuantityReceived), 0)  FROM PurchaseOrderLines WHERE pmlJobID = jmoJobID AND pmlJobAssemblyID = jmoJobAssemblyID AND pmlJobOperationID = jmoJobOperationID)  AND " + text + " ");
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
			ProcessSelectedItemValues itemValuesFromList = GetItemValuesFromList(selectedItems, row);
			if (itemValuesFromList.EditableValues != null && itemValuesFromList.EditableValues.ContainsKey("jmoSupplierOrganizationID") && itemValuesFromList.EditableValues.ContainsKey("jmoPurchaseLocationID"))
			{
				row["jmoSupplierOrganizationID"] = Convert.ToString(itemValuesFromList.EditableValues["jmoSupplierOrganizationID"]);
				row["jmoPurchaseLocationID"] = Convert.ToString(itemValuesFromList.EditableValues["jmoPurchaseLocationID"]);
			}
		}
		DataView defaultView = dataTable.DefaultView;
		defaultView.Sort = "jmoSupplierOrganizationID ASC, jmoPurchaseLocationID ASC";
		foreach (DataRow row2 in defaultView.ToTable().Rows)
		{
			if (!row2.Field<string>("jmoSupplierOrganizationID").Equals(value, StringComparison.CurrentCultureIgnoreCase) || !row2.Field<string>("jmoPurchaseLocationID").Equals(value2, StringComparison.CurrentCultureIgnoreCase) || !row2.Field<string>("jmpPlantID").Equals(value3, StringComparison.CurrentCultureIgnoreCase))
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
			CheckForHeaderKeyChange(this, row2, matchingFieldsInfo, dataRow);
			addPOLine(childBindingSource, row2, dataRow, matchingFieldsInfo2, GetItemValuesFromList(selectedItems, row2));
			value = row2.Field<string>("jmoSupplierOrganizationID");
			value2 = row2.Field<string>("jmoPurchaseLocationID");
			value3 = row2.Field<string>("jmpPlantID");
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

	private void addPOLine(M1BindingSource bsPOLines, DataRow operationsRow, DataRow poRow, MatchingFieldsInfo lineMatches, ProcessSelectedItemValues itemValues)
	{
		DataRow dataRow = TransferLineInfo(this, operationsRow, bsPOLines, lineMatches, poRow);
		decimal num = 0m;
		DateTime? dateTime = null;
		if (itemValues.EditableValues != null)
		{
			if (itemValues.EditableValues.ContainsKey("Quantity"))
			{
				num = Convert.ToDecimal(itemValues.EditableValues["Quantity"]);
			}
			if (itemValues.EditableValues.ContainsKey("jmoDueDate"))
			{
				dateTime = Convert.ToDateTime(itemValues.EditableValues["jmoDueDate"]);
			}
		}
		dataRow["pmlPurchaseQuantity"] = num;
		dataRow["pmlDueDate"] = dateTime;
		dataRow["pmlSourceTableName"] = "JobOperations";
		dataRow["pmlSourceTableUniqueID"] = operationsRow.Field<Guid>("jmoUniqueID");
	}
}
