using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class TransferJobMaterialToMaterialIssueProcess : ProcessParameters
{
	public TransferJobMaterialToMaterialIssueProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[2] { "jmmJobID", "jmmJobAssemblyID" };
		PromptFieldAllowMultiples = false;
		KeyValueFieldNames = new string[3] { "jmmJobID", "jmmJobAssemblyID", "jmmJobMaterialID" };
		KeyValueTableName = "JobMaterials";
		Description = "Select the job materials to be issued.";
		CreatedBindingSourceCaption = "Create Material Issue from Job";
		GridID = "M1ADDFROMJOBMATISSUE";
		BindingSourceTable = "MaterialIssues";
		HelpLink = "IM_TransferJobMaterialToMaterialIssue.htm";
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmmClosed", fieldValue: false, "Job is closed."));
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Sales Order ID", null, new string[1] { "omjSalesOrderID" })
		{
			AdditionalFields = "omjSalesOrderID",
			ValueFields = new string[1] { "omjSalesOrderID" },
			IgnoreWhenEmpty = false
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Issue Type", null, new string[1] { "IssueType" })
		{
			ValueFields = new string[1] { "IssueType" },
			IgnoreWhenEmpty = false
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Do not allow parts with open POs")
		{
			AdoFilterExpression = "OpenPOCount = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "OpenPOCount"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Exclude job material/assembly records already received complete?")
		{
			Value = true,
			AdoFilterExpression = "jmmReceivedComplete = 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "jmmReceivedComplete"
		});
	}

	public override void ConstructPromptFieldsWhere(object sender, PromptFieldsWhereEventArgs e)
	{
		if (e.KeyValues.Count != 0)
		{
			e.Where = "jmmJobID = " + M1Util.ConvertToSql(e.KeyValues[0][0]) + " And jmmJobID+Convert(varchar(10),jmmJobAssemblyID) In (Select MyCTE.jmaJobID+Convert(varchar(10),MyCTE.jmaJobAssemblyID) From MyCTE)";
			e.QueryFormat = "With MyCTE As (Select jmaJobID,jmaJobAssemblyID From JobAssemblies Where jmaJobID = " + M1Util.ConvertToSql(e.KeyValues[0][0]) + " And jmaJobAssemblyID = " + M1Util.ConvertToSql(e.KeyValues[0][1]) + " Union All Select JobAssemblies.jmaJobID,JobAssemblies.jmaJobAssemblyID From JobAssemblies Inner Join MyCTE On JobAssemblies.jmaJobID = MyCTE.jmaJobID And JobAssemblies.jmaParentAssemblyID=MyCTE.jmaJobAssemblyID Where JobAssemblies.jmaJobAssemblyID <> 0 ) {0}";
		}
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Lot, value: false);
		arg.SkippedErrors.Add(ErrorItem.ErrorSource.Serial, value: false);
		if (selectedItems.Count == 0)
		{
			return;
		}
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		M1Database databaseForRow = BindingSource.GetDatabaseForRow(currentAsDataRow);
		M1DataDictionary m1DataDictionary = databaseForRow.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo issueLinematches = m1DataDictionary.FindMatchingFields("JobMaterials, PartRevisions, Jobs", "MaterialIssueLines", new string[19]
		{
			"IssueType", "jmmJobID", "JobType", "jmmJobAssemblyID", "jmmJobMaterialID", "jmmPartID", "jmmPartRevisionID", "jmmPartWarehouseLocationID", "jmmPartBinID", "jmmEstimatedQuantity",
			"jmmReceivedComplete", "jmmKitPart", "jmpProjectID", "jmpProjectAreaID", "jmpPlantID", "imrQuantityOnHand", "imrQuantityAllocated", "imrLongDescriptionRTF", "imrLongDescriptionText"
		}, new string[19]
		{
			"injIssueType", "injJobID", "injJobType", "injJobAssemblyID", "injJobMaterialID", "injPartID", "injPartRevisionID", "injPartWarehouseLocationID", "injPartBinID", "injEstimatedQuantity",
			"injIssueComplete", "injKitPart", "injProjectID", "injProjectAreaID", "injPlantID", "injQuantityOnHand", "injQuantityAllocated", "injLongDescriptionRTF", "injLongDescriptionText"
		});
		MatchingFieldsInfo componentMatch = m1DataDictionary.FindMatchingFields("JobMaterialComponents", "MaterialIssueComponents", new string[14]
		{
			"jmtJobID", "jmtJobAssemblyID", "jmtJobMaterialID", "jmtJobMaterialComponentID", "jmtPartID", "jmtPartRevisionID", "jmtPartWarehouseLocationID", "jmtPartBinID", "jmtQuantityPerParent", "jmtAdditionalQuantity",
			"jmtUnitOfMeasure", "jmtDescription", "jmtWeight", "jmtReceivedComplete"
		}, new string[14]
		{
			"inkJobID", "inkJobAssemblyID", "inkJobMaterialID", "inkJobMaterialComponentID", "inkPartID", "inkPartRevisionID", "inkPartWarehouseLocationID", "inkPartBinID", "inkQuantityPerParent", "inkAdditionalQuantity",
			"inkUnitOfMeasure", "inkDescription", "inkWeight", "inkReceivedComplete"
		});
		DataTable dataTable = databaseForRow.GetDataTable("select JobMaterialComponents.* from JobMaterialComponents inner join JobMaterials on jmmJobID=jmtJobID and jmmJobAssemblyID=jmtJobAssemblyID and jmmJobMaterialID=jmtJobMaterialID where " + text + " and jmtReceivedComplete = 0 order by jmtJobID,jmtJobAssemblyID,jmtJobMaterialID,jmtJobMaterialComponentID");
		DataTable dataTable2 = databaseForRow.GetDataTable("Select * from (select 1 As IssueType,jmmJobID,jmmJobAssemblyID,jmmJobMaterialID,1 As JobType,jmmPartID,jmmPartRevisionID,jmmPartWarehouseLocationID,jmmPartBinID,jmmPullFromStockQuantity As jmmEstimatedQuantity,jmmQuantityReceived, jmmReceivedComplete,jmmKitPart,jmmBackflush, Convert(numeric(15,5),case when jmmQuantityPerAssembly * jmaQuantityToMake >= 0 Then Case When jmmPullFromStockQuantity -jmmQuantityReceived < 0 Or jmmReceivedComplete <> 0 Then 0 Else jmmPullFromStockQuantity -jmmQuantityReceived End Else Case When jmmPullFromStockQuantity -jmmQuantityReceived > 0 Or jmmReceivedComplete <> 0 Then 0 Else jmmPullFromStockQuantity -jmmQuantityReceived End End) As OpenQty, IsNull((Select Count(*) From PartAlternates Where imePartID = jmmPartID And imePartRevisionID = jmmPartRevisionID),0) As AlternatePartsCount, jmpProjectID,jmpProjectAreaID,jmpPlantID,(select top 1 omjSalesOrderID from SalesOrderJobLinks Where omjJobID = jmmJobID and omjLinkType = 1) as omjSalesOrderID,imrQuantityOnHand,imrQuantityAllocated,imrLongDescriptionRTF,imrLongDescriptionText from JobMaterials inner join PartRevisions on jmmPartID=imrPartID and jmmPartRevisionID = imrPartRevisionID  Left Outer Join JobOperations On jmmJobID = jmoJobID And jmmJobAssemblyID = jmoJobAssemblyID And jmmRelatedJobOperationID = jmoJobOperationID Inner Join JobAssemblies On jmmJobID = jmaJobID And jmmJobAssemblyID = jmaJobAssemblyID Inner Join Jobs On jmmJobID = jmpJobID union  select 1 As IssueType,jmaJobID As jmmJobID,jmaJobAssemblyID As jmmJobAssemblyID,0 As jmmJobMaterialID,3 As JobType,jmaPartID As jmmPartID,jmaPartRevisionID As jmmPartRevisionID, jmaPartWarehouseLocationID As jmmPartWarehouseLocationID,jmaPartBinID As jmmPartBinID,jmaQuantityToPull As jmmEstimatedQuantity,jmaQuantityIssued As jmmQuantityReceived, jmaIssuedComplete As jmmReceivedComplete,0 As jmmKitPart, 0 As jmmBackflush, Convert(numeric(15,5),case when jmaQuantityToMake >= 0 Then Case When jmaQuantityToPull -jmaQuantityIssued < 0 Or jmaIssuedComplete <> 0 Then 0 Else jmaQuantityToPull -jmaQuantityIssued End Else Case When jmaQuantityToPull -jmaQuantityIssued > 0 Or jmaIssuedComplete <> 0 Then 0 Else jmaQuantityToPull -jmaQuantityIssued End End) As OpenQty, IsNull((Select Count(*) From PartAlternates Where imePartID = jmaPartID And imePartRevisionID = jmaPartRevisionID),0) As AlternatePartsCount, jmpProjectID,jmpProjectAreaID,jmpPlantID,(select top 1 omjSalesOrderID from SalesOrderJobLinks Where omjJobID = jmaJobID and omjLinkType = 1) as omjSalesOrderID,imrQuantityOnHand,imrQuantityAllocated,imrLongDescriptionRTF,imrLongDescriptionText from JobAssemblies inner join PartRevisions on jmaPartID=imrPartID and jmaPartRevisionID = imrPartRevisionID Inner Join Jobs On jmaJobID = jmpJobID  where jmaQuantityToPull > 0) test where " + text + " order by jmmJobID, jmmJobAssemblyID, jmmJobMaterialID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		M1BindingSource childBindingSource = BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
		foreach (DataRow row in dataTable2.Rows)
		{
			if (checkConditions(row, currentAsDataRow, GetItemValuesFromList(selectedItems, row), messages))
			{
				addMaterialIssueLine(childBindingSource, row, childBindingSource2, dataTable, issueLinematches, componentMatch, GetItemValuesFromList(selectedItems, row));
			}
		}
	}

	private void addMaterialIssueLine(M1BindingSource bsMaterialIssueLines, DataRow jobRow, M1BindingSource bsComponents, DataTable dtJobComponents, MatchingFieldsInfo issueLinematches, MatchingFieldsInfo componentMatch, ProcessSelectedItemValues itemValues)
	{
		bool flag = false;
		if (itemValues.EditableValues.ContainsKey("ReturnQty"))
		{
			flag = Convert.ToBoolean(itemValues.EditableValues["ReturnQty"]);
		}
		if (flag)
		{
			jobRow["IssueType"] = 3;
		}
		DataRow dataRow = TransferLineInfo(this, jobRow, bsMaterialIssueLines, issueLinematches);
		decimal value = default(decimal);
		decimal value2 = default(decimal);
		if (itemValues.EditableValues.ContainsKey("IssueQty"))
		{
			value = Convert.ToDecimal(itemValues.EditableValues["IssueQty"]);
		}
		if (itemValues.EditableValues.ContainsKey("ScrapQty"))
		{
			value2 = Convert.ToDecimal(itemValues.EditableValues["ScrapQty"]);
		}
		if (itemValues.EditableValues.ContainsKey("ReceivedComplete"))
		{
			dataRow.SetField("injIssueComplete", Convert.ToBoolean(itemValues.EditableValues["ReceivedComplete"]));
		}
		if (itemValues.EditableValues.ContainsKey("ReturnQty"))
		{
			flag = Convert.ToBoolean(itemValues.EditableValues["ReturnQty"]);
		}
		if (dataRow.Field<byte>("injIssueType") == 1)
		{
			if (dataRow.Field<byte>("injJobType") == 1)
			{
				dataRow.SetField("injJobMatIssueQuantity", value);
				dataRow.SetField("injJobMatScrapQuantity", value2);
			}
			else if (dataRow.Field<byte>("injJobType") == 3)
			{
				dataRow.SetField("injJobAsmIssueQuantity", value);
				dataRow.SetField("injJobAsmScrapQuantity", value2);
			}
		}
		else if (dataRow.Field<byte>("injIssueType") == 2)
		{
			dataRow.SetField("injInvIssueQuantity", value);
			dataRow.SetField("injInvScrapQuantity", value2);
		}
		else if (dataRow.Field<byte>("injIssueType") == 3)
		{
			dataRow.SetField("injJobMatReturnIssueQuantity", value);
			dataRow.SetField("injJobMatReturnScrapQuantity", value2);
		}
		if (bsComponents.Count != 0)
		{
			bsComponents.RemoveWhere(string.Empty, dataRow);
		}
		DataRow[] array = dtJobComponents.Select("jmtJobID = " + dataRow.Field<string>("injJobID").Trim().ToLinq() + " and jmtJobAssemblyID = " + Convert.ToInt32(dataRow["injJobAssemblyID"]).ToLinq() + " and jmtJobMaterialID = " + Convert.ToInt32(dataRow["injJobMaterialID"]).ToLinq());
		foreach (DataRow sourceLineRow in array)
		{
			TransferLineInfo(this, sourceLineRow, bsComponents, componentMatch, dataRow);
		}
	}

	private bool checkConditions(DataRow sourceLineRow, DataRow MaterialIssueRow, ProcessSelectedItemValues itemValues, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = MaterialIssueRow.Field<string>("iniMaterialIssueID").Trim();
		if (MaterialIssueRow.Field<bool>("iniPosted"))
		{
			stringBuilder.Append(", destination material issue " + text + " is already posted");
		}
		if (itemValues.EditableValues.ContainsKey("IssueQty") && Convert.ToDecimal(itemValues.EditableValues["IssueQty"]) < 0m)
		{
			stringBuilder.Append(", issue quantity cannot be less than zero");
		}
		if (itemValues.EditableValues.ContainsKey("ScrapQty") && Convert.ToDecimal(itemValues.EditableValues["ScrapQty"]) < 0m)
		{
			stringBuilder.Append(", scrap quantity cannot be less than zero");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Job Material " + sourceLineRow.Field<string>("jmmJobID").Trim() + "/" + Convert.ToInt32(sourceLineRow["jmmJobAssemblyID"]).ToString().Trim() + "/" + Convert.ToInt32(sourceLineRow["jmmJobMaterialID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			itemValues.DiscardSave = true;
			return false;
		}
		return true;
	}
}
