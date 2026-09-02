using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class MaterialIssueReversalProcess : ProcessParameters
{
	public MaterialIssueReversalProcess(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		PromptFieldNames = new string[1] { "iniMaterialIssueID" };
		PromptFieldAllowMultiples = true;
		MultipleDestinationRowsCreated = true;
		KeyValueFieldNames = new string[1] { "iniMaterialIssueID" };
		KeyValueTableName = "MaterialIssues";
		Description = "Use this screen to reverse your posted Material Issues.";
		GridID = "M1ADDFROMREVERSALMATERIALISSUES";
		SecurityRole = "REVERSALS";
		ContinueMessage = "This will reverse the {0} selected Material Issue(s). Are you sure you want to continue?";
		BindingSourceTable = "MaterialIssues";
		PromptFieldValidations.Add(new PromptFieldValidationBool("iniPosted", fieldValue: true, "Material Issue is not posted."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("iniReversalEntry", fieldValue: false, "Material Issue has already been reversed."));
		PromptFieldValidations.Add(new PromptFieldValidationBool("iniReversed", fieldValue: false, "Material Issue has already been reversed."));
		HeaderSourceFields = new string[3] { "iniPosted=Convert(bit,0)", "iniReversalEntry=Convert(bit,1)", "iniReversed=Convert(bit,0)" };
		HeaderDestinationFields = new string[3] { "iniPosted", "iniReversalEntry", "iniReversed" };
		DateTime today = DateTime.Today;
		today = today.AddDays(-today.Day + 1);
		DateTime value = today.AddMonths(1).AddDays(-1.0);
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Issue Date")
		{
			IgnoreWhenEmpty = true,
			ValueField = "iniMaterialIssueDate",
			AdditionalFields = "iniMaterialIssueDate",
			ValueStart = today,
			ValueEnd = value
		});
	}

	protected override void OnRun(StartProcessEventArgs arg)
	{
		_ = arg.PromptFieldValues;
		List<ProcessSelectedItemValues> selectedItems = arg.SelectedItems;
		List<string> messages = arg.Messages;
		string text = ConstructWhereClause(KeyValueFieldNames, selectedItems);
		if (text.Length == 0)
		{
			return;
		}
		M1Database database = BindingSource.Database;
		M1DataDictionary m1DataDictionary = database.GetService(typeof(M1DataDictionary)) as M1DataDictionary;
		MatchingFieldsInfo matchingFieldsInfo = m1DataDictionary.FindMatchingFields("MaterialIssues", "MaterialIssues", HeaderSourceFields, HeaderDestinationFields);
		MatchingFieldsInfo matchingFieldsInfo2 = m1DataDictionary.FindMatchingFields("MaterialIssueLines", "MaterialIssueLines", new string[34]
		{
			"injIssueType", "injJobID", "injJobAssemblyID", "injJobMaterialID", "injCreateJobSeq", "injJobType", "injEstimatedQuantity", "injJobOpenQuantity", "injIssueComplete=Convert(bit,0)", "injPartID",
			"injPartRevisionID", "injPartWarehouseLocationID", "injPartBinID", "injKitPart", "injQuantityOnHand", "injQuantityAllocated", "injLongDescriptionRTF", "injLongDescriptionText", "-injJobMatIssueQuantity", "-injJobAsmIssueQuantity",
			"-injInvIssueQuantity", "-injInvScrapQuantity", "-injJobAsmScrapQuantity", "-injJobMatScrapQuantity", "injReference", "injHeatLot", "injMiscIssueReasonID", "injPlantID", "injProjectID", "injProjectAreaID",
			"injPosted=Convert(bit,0)", "injReversed=Convert(bit,0)", "injMaterialIssueID", "injMaterialIssueLineID"
		}, new string[34]
		{
			"injIssueType", "injJobID", "injJobAssemblyID", "injJobMaterialID", "injCreateJobSeq", "injJobType", "injEstimatedQuantity", "injJobOpenQuantity", "injIssueComplete", "injPartID",
			"injPartRevisionID", "injPartWarehouseLocationID", "injPartBinID", "injKitPart", "injQuantityOnHand", "injQuantityAllocated", "injLongDescriptionRTF", "injLongDescriptionText", "injJobMatIssueQuantity", "injJobAsmIssueQuantity",
			"injInvIssueQuantity", "injInvScrapQuantity", "injJobAsmScrapQuantity", "injJobMatScrapQuantity", "injReference", "injHeatLot", "injMiscIssueReasonID", "injPlantID", "injProjectID", "injProjectAreaID",
			"injPosted", "injReversed", "injReverseMaterialIssueID", "injReverseMaterialIssueLineID"
		});
		MatchingFieldsInfo matchingFieldsInfo3 = m1DataDictionary.FindMatchingFields("MaterialIssueComponents,MaterialIssueLines", "MaterialIssueComponents", new string[27]
		{
			"inkPartID", "inkPartRevisionID", "inkPartWarehouseLocationID", "inkPartBinID", "inkInvParentQuantity", "inkQuantityPerParent", "-inkAdditionalQuantity", "inkReceivedComplete=Convert(bit,0)", "inkUnitOfMeasure", "inkDescription",
			"inkJobID", "inkJobAssemblyID", "inkJobMaterialID", "inkJobMaterialComponentID", "inkWeight", "inkPosted=Convert(bit,0)", "-inkInvParentQuantityScrap", "-inkInvIssueQuantity", "-inkInvScrapQuantity", "-inkJobMatIssueQuantity",
			"-inkJobMatScrapQuantity", "inkReversed=Convert(bit,0)", "inkJobMatParentQuantity", "inkJobMatParentQuantityScrap", "inkMaterialIssueID", "inkMaterialIssueLineID", "inkMaterialIssueComponentID"
		}, new string[27]
		{
			"inkPartID", "inkPartRevisionID", "inkPartWarehouseLocationID", "inkPartBinID", "inkInvParentQuantity", "inkQuantityPerParent", "inkAdditionalQuantity", "inkReceivedComplete", "inkUnitOfMeasure", "inkDescription",
			"inkJobID", "inkJobAssemblyID", "inkJobMaterialID", "inkJobMaterialComponentID", "inkWeight", "inkPosted", "inkInvParentQuantityScrap", "inkInvIssueQuantity", "inkInvScrapQuantity", "inkJobMatIssueQuantity",
			"inkJobMatScrapQuantity", "inkReversed", "inkJobMatParentQuantity", "inkJobMatParentQuantityScrap", "inkReverseMaterialIssueID", "inkReverseMaterialIssueLineID", "inkReverseMaterialIssueCompID"
		});
		DataTable dataTable = database.GetDataTable("Select inkUniqueID,Parts.impTrackSerialNumbers, Parts.impTrackLotNumbers, " + matchingFieldsInfo3.GetSourceFieldList(string.Empty, string.Empty) + " From MaterialIssueComponents INNER JOIN MaterialIssueLines ON inkMaterialIssueID = injMaterialIssueID AND inkMaterialIssueLineID = injMaterialIssueLineID INNER JOIN MaterialIssues ON injMaterialIssueID = iniMaterialIssueID  INNER JOIN Parts ON MaterialIssueComponents.inkPartID = Parts.impPartID WHERE " + text + "AND inkPosted = 1 ORDER BY inkMaterialIssueID, inkMaterialIssueLineID,inkMaterialIssueComponentID ");
		DataTable dataTable2 = database.GetDataTable("SELECT iniMaterialIssueID,iniMaterialIssueDate," + matchingFieldsInfo.GetSourceFieldList(string.Empty, ",") + matchingFieldsInfo2.GetSourceFieldList(string.Empty, "") + " FROM MaterialIssueLines INNER JOIN MaterialIssues ON injMaterialIssueID = iniMaterialIssueID WHERE " + text + " AND (iniPosted = 1) AND (iniReversalEntry = 0) AND (iniReversed=0)  ORDER BY injMaterialIssueID, injMaterialIssueLineID");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		using M1BindingSource m1BindingSource = BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueLines");
		using M1BindingSource m1BindingSource2 = m1BindingSource.PrimaryTable.GetChildBindingSource("MaterialIssueComponents");
		DataRow currentAsDataRow = BindingSource.CurrentAsDataRow;
		string empty = string.Empty;
		string text2 = string.Empty;
		foreach (DataRow row in dataTable2.Rows)
		{
			if (!CheckConditions(row, GetItemValuesFromList(selectedItems, row), messages))
			{
				continue;
			}
			if (!text2.Equals(row.Field<string>("injMaterialIssueID").Trim(), StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow = (DataRow)BindingSource.AddNew();
				BindingSource.SetKeyToNextAvailable(currentAsDataRow);
				SetDefaultFieldValues(arg, currentAsDataRow);
				BindingSource.ActivateRow(currentAsDataRow, null, doFlash: false);
				empty = currentAsDataRow.Field<string>("iniMaterialIssueID").Trim();
				text2 = row.Field<string>("injMaterialIssueID").Trim();
				base.TransferHeaderInfo(this, row, matchingFieldsInfo, currentAsDataRow);
				CheckForHeaderKeyChange(this, row, matchingFieldsInfo, currentAsDataRow);
				if (!string.IsNullOrWhiteSpace(empty))
				{
					List<object[]> keysCreated = arg.KeysCreated;
					object[] item = new string[1] { empty };
					keysCreated.Add(item);
				}
			}
			DataRow dataRow2 = TransferLineInfo(this, row, m1BindingSource, matchingFieldsInfo2);
			if (m1BindingSource2.Count != 0)
			{
				m1BindingSource2.RemoveWhere(string.Empty, row);
			}
			DataRow[] array = dataTable.Select("inkMaterialIssueID = " + row.Field<string>("injMaterialIssueID").Trim().ToLinq() + " and inkMaterialIssueLineID = " + Convert.ToInt32(row["injMaterialIssueLineID"]).ToLinq());
			foreach (DataRow dataRow3 in array)
			{
				DataRow dataRow4 = TransferLineInfo(this, dataRow3, m1BindingSource2, matchingFieldsInfo3, dataRow2);
				m1BindingSource2.SetKeyToNextAvailable(dataRow4);
				if (dataRow3.Field<bool>("impTrackSerialNumbers") || dataRow3.Field<bool>("impTrackLotNumbers"))
				{
					AddSerialAndLotTransactionsComponents(database, null, row.Field<string>("injMaterialIssueID"), row.Field<short>("injMaterialIssueLineID"), dataRow4.Field<string>("inkPartID"), dataRow4.Field<Guid>("inkUniqueID"));
				}
			}
			AddSerialAndLotTransactionsLine(database, null, row.Field<string>("injMaterialIssueID"), row.Field<short>("injMaterialIssueLineID"), dataRow2.Field<Guid>("injUniqueID"));
		}
		if (arg.KeysCreated.Count != 0)
		{
			BindingSource.SaveData();
			arg.OpenKeysWithObjectID = "MaterialIssue";
		}
	}

	protected override void TransferHeaderInfo(ProcessParameters parm, DataRow sourceHeaderRow, MatchingFieldsInfo headerFieldMatches, DataRow destinationHeaderRow)
	{
		destinationHeaderRow["iniMaterialIssueDate"] = sourceHeaderRow.Field<DateTime>("iniMaterialIssueDate").AddMinutes(1.0);
		destinationHeaderRow["iniReversalEntry"] = true;
	}

	private void AddSerialAndLotTransactionsLine(M1Database database, SqlTransaction transaction, string id, int lineId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, injUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MaterialIssueLines inner join SerialNumberTransactions on injUniqueID = sntTableUniqueID where injMaterialIssueID = @ID and injMaterialIssueLineID = @LineID and injPosted = 1 and injReversed=0order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool negativeTrans = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 21:
					status = 11;
					b = 28;
					break;
				case 17:
					status = 11;
					b = 29;
					break;
				case 20:
					status = 11;
					b = 30;
					break;
				case 23:
					status = 11;
					b = 31;
					break;
				case 4:
					status = 11;
					b = 32;
					break;
				case 22:
					status = 11;
					b = 33;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "MaterialIssueLines", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), 0, negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, injUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MaterialIssueLines inner join LotNumberTransactions on injUniqueID = abtTableUniqueID where injMaterialIssueID = @ID and injMaterialIssueLineID = @LineID and injPosted = 1 And injReversed=0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
		foreach (DataRow row2 in dataTable.Rows)
		{
			byte status2 = 0;
			byte b2 = 0;
			bool negativeTrans2 = true;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 21:
				status2 = 11;
				b2 = 28;
				break;
			case 17:
				status2 = 11;
				b2 = 29;
				break;
			case 20:
				status2 = 11;
				b2 = 30;
				break;
			case 23:
				status2 = 11;
				b2 = 31;
				break;
			case 4:
				status2 = 11;
				b2 = 32;
				break;
			case 22:
				status2 = 11;
				b2 = 33;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "MaterialIssueLines", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}

	private void AddSerialAndLotTransactionsComponents(M1Database database, SqlTransaction transaction, string id, int lineId, string partId, Guid destUniqueID)
	{
		if (string.IsNullOrWhiteSpace(id))
		{
			return;
		}
		SqlCommand sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, inkUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction from MaterialIssueComponents inner join SerialNumberTransactions on inkUniqueID = sntTableUniqueID where inkMaterialIssueID = @ID and inkMaterialIssueLineID = @LineID and inkPosted = 1 and inkPartID=@PartID order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
			foreach (DataRow row in dataTable.Rows)
			{
				byte status = 0;
				byte b = 0;
				bool negativeTrans = true;
				serialNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row.Field<string>("sntSerialNumberID"));
				switch (row.Field<byte>("sntTransactionType"))
				{
				case 21:
					status = 11;
					b = 28;
					break;
				case 17:
					status = 11;
					b = 29;
					break;
				case 20:
					status = 11;
					b = 30;
					break;
				case 23:
					status = 11;
					b = 31;
					break;
				case 4:
					status = 11;
					b = 32;
					break;
				case 22:
					status = 11;
					b = 33;
					break;
				}
				if (b > 0)
				{
					serialNumberDefinition.AddSerialTransaction(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), -row.Field<decimal>("sntQuantity"), status, b, "MaterialIssueComponents", destUniqueID, row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), negativeTrans, row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, transaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
		}
		sqlCommand = database.NewSqlCommand("select DateAdd(mi, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, inkUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction from MaterialIssueComponents inner join LotNumberTransactions on inkUniqueID = abtTableUniqueID where inkMaterialIssueID = @ID and inkMaterialIssueLineID = @LineID and inkPosted = 1 and inkPartID=@PartID order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = id;
		sqlCommand.Parameters.Add(new SqlParameter("@LineID", SqlDbType.Int)).Value = lineId;
		sqlCommand.Parameters.AddWithValue("@PartID", partId);
		dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
		foreach (DataRow row2 in dataTable.Rows)
		{
			byte status2 = 0;
			byte b2 = 0;
			bool negativeTrans2 = true;
			lotNumberDefinition.LoadLotOrSerialNumbers(database, transaction, row2.Field<string>("abtLotNumberID"));
			switch (row2.Field<byte>("abtTransactionType"))
			{
			case 21:
				status2 = 11;
				b2 = 28;
				break;
			case 17:
				status2 = 11;
				b2 = 29;
				break;
			case 20:
				status2 = 11;
				b2 = 30;
				break;
			case 23:
				status2 = 11;
				b2 = 31;
				break;
			case 4:
				status2 = 11;
				b2 = 32;
				break;
			case 22:
				status2 = 11;
				b2 = 33;
				break;
			}
			if (b2 > 0)
			{
				lotNumberDefinition.AddLotTransaction(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtPartWarehouseLocationID"), row2.Field<string>("abtPartBinID"), row2.Field<string>("abtLotNumberID"), -row2.Field<decimal>("abtQuantity"), status2, b2, "MaterialIssueComponents", destUniqueID, row2.Field<string>("abtJobID"), Convert.ToInt32(row2["abtJobAssemblyID"]), Convert.ToInt32(row2["abtJobMaterialID"]), Convert.ToInt32(row2["abtJobMaterialComponentID"]), negativeTrans2, row2.Field<DateTime>("abtTransactionDate"));
				lotNumberDefinition.RefreshStatuses(database, transaction, row2.Field<string>("abtPartID"), row2.Field<string>("abtPartRevisionID"), row2.Field<string>("abtLotNumberID"));
			}
		}
	}

	private bool CheckConditions(DataRow sourceLineRow, ProcessSelectedItemValues itemValues, List<string> messages)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (Convert.ToByte(sourceLineRow["injIssueType"]) == 3)
		{
			stringBuilder.Append(", you cannot reverse a return from job issue type");
		}
		if (stringBuilder.Length != 0)
		{
			stringBuilder.Remove(0, 2);
			messages.Add("Material Issue " + sourceLineRow.Field<string>("injMaterialIssueID").Trim() + "/" + Convert.ToInt32(sourceLineRow["injMaterialIssueLineID"]).ToString().Trim() + " was not added because " + stringBuilder.ToString() + ".");
			itemValues.DiscardSave = true;
			return false;
		}
		return true;
	}
}
