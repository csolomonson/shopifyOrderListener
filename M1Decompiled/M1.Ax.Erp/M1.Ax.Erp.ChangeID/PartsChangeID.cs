using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Extensions;
using M1.Forms.Controls;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("Parts")]
public class PartsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		parm.DeleteStatements.AppendLine("DELETE FROM PartRevisions WHERE imrPartID = " + parm.OldKeyValues[0].ToSql() + " AND imrPartRevisionID IN (SELECT imrPartRevisionID FROM PartRevisions WHERE imrPartID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartWarehouseLocations WHERE imlPartID = " + parm.OldKeyValues[0].ToSql() + " AND imlPartRevisionID+imlPartWarehouseID IN (SELECT imlPartRevisionID+imlPartWarehouseID FROM PartWarehouseLocations WHERE imlPartID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartBins WHERE imbPartID = " + parm.OldKeyValues[0].ToSql() + " AND imbPartRevisionID+imbWarehouseID+imbPartBinID IN (SELECT imbPartRevisionID+imbWarehouseID+imbPartBinID FROM PartBins WHERE imbPartID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartAssemblies WHERE imaMethodID = " + parm.OldKeyValues[0].ToSql() + " AND imaMethodRevisionID+CONVERT(CHAR(6),imaMethodAssemblyID) IN (SELECT (imaMethodRevisionID+CONVERT(CHAR(6),imaMethodAssemblyID)) FROM PartAssemblies WHERE imaMethodID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartMaterials WHERE immMethodID = " + parm.OldKeyValues[0].ToSql() + " AND immMethodRevisionID+CONVERT(CHAR(6),immMethodAssemblyID)+CONVERT(CHAR(6),immMethodMaterialID) IN (SELECT immMethodRevisionID+CONVERT(CHAR(6),immMethodAssemblyID)+CONVERT(CHAR(6),immMethodMaterialID) FROM PartMaterials WHERE immMethodID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartOperations WHERE imoMethodID = " + parm.OldKeyValues[0].ToSql() + " AND imoMethodRevisionID+CONVERT(CHAR(6),imoMethodAssemblyID)+CONVERT(CHAR(6),imoMethodOperationID) IN (SELECT imoMethodRevisionID+CONVERT(CHAR(6),imoMethodAssemblyID)+CONVERT(CHAR(6),imoMethodOperationID) FROM PartOperations WHERE imoMethodID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartOrgReferences WHERE imzPartID = " + parm.OldKeyValues[0].ToSql() + " AND imzPartRevisionID+imzOrganizationID IN (SELECT imzPartRevisionID+imzOrganizationID FROM PartOrgReferences WHERE imzPartID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartCrossReferences WHERE imxPartID = " + parm.OldKeyValues[0].ToSql() + " AND imxPartRevisionID+imxOrganizationID+imxLocationID IN (SELECT imxPartRevisionID+imxOrganizationID+imxLocationID FROM PartCrossReferences WHERE imxPartID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartRules WHERE pcrMethodID = " + parm.OldKeyValues[0].ToSql() + " AND pcrMethodRevisionID+CONVERT(CHAR(6),pcrMethodAssemblyID) IN (SELECT pcrMethodRevisionID+CONVERT(CHAR(6),pcrMethodAssemblyID) FROM PartRules WHERE pcrMethodID = " + parm.NewKeyValues[0].ToSql() + ")");
		parm.UpdateStatements.AppendLine("UPDATE FormDefinitions SET xaoFormID = 'PART-' + " + parm.NewKeyValues[0].ToSql() + " + '-REV-' + RTrim(LTrim(imrPartRevisionID)) From FormDefinitions Inner Join PartRevisions On xaoFormID = imrFormID Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiFormID = 'PART-' + " + parm.NewKeyValues[0].ToSql() + " + '-REV-' + RTrim(LTrim(imrPartRevisionID)) From FormInputValues Inner Join PartRevisions On xaiFormID = imrFormID Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiTopLevelFormID = 'PART-' + " + parm.NewKeyValues[0].ToSql() + " + '-REV-' + RTrim(LTrim(imrPartRevisionID)) From FormInputValues Inner Join PartRevisions On xaiTopLevelFormID = imrFormID Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiParentFormID = 'PART-' + " + parm.NewKeyValues[0].ToSql() + " + '-REV-' + RTrim(LTrim(imrPartRevisionID)) From FormInputValues Inner Join PartRevisions On xaiParentFormID = imrFormID Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE PartRevisions SET imrFormID = 'PART-' + " + parm.NewKeyValues[0].ToSql() + " + '-REV-' + RTrim(LTrim(imrPartRevisionID)) Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE PartAssemblies SET imaPartID = " + parm.NewKeyValues[0].ToSql() + " Where imaMethodID = " + parm.OldKeyValues[0].ToSql() + " And imaMethodAssemblyID = 0");
		if (parm.NewIDExists)
		{
			parm.UpdateStatements.AppendLine("UPDATE PartRevisions SET PartRevisions.imrQuantityOnHand = PartRevisions.imrQuantityOnHand + temppart.imrQuantityOnHand,PartRevisions.imrQuantityAllocated = PartRevisions.imrQuantityAllocated + temppart.imrQuantityAllocated  From PartRevisions Inner Join (select " + parm.NewKeyValues[0].ToSql() + " as imrPartID, imrPartRevisionID, imrQuantityOnHand, imrQuantityAllocated from PartRevisions Where imrPartID = " + parm.OldKeyValues[0].ToSql() + ") as temppart On PartRevisions.imrPartID = temppart.imrPartID and PartRevisions.imrPartRevisionID = temppart.imrPartRevisionID");
			parm.UpdateStatements.AppendLine("UPDATE PartBins SET PartBins.imbQuantityOnHand = PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand, imbBinQuantityOnHand = CASE WHEN imbConversionFactor = 0 THEN PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand ELSE (PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand) / imbConversionFactor END, PartBins.imbQuantityAllocated = PartBins.imbQuantityAllocated + temppart.imbQuantityAllocated  From PartBins Inner Join (select " + parm.NewKeyValues[0].ToSql() + " as imbPartID,imbPartRevisionID,imbWarehouseID,imbPartBinID, imbQuantityOnHand, imbQuantityAllocated from PartBins Where imbPartID = " + parm.OldKeyValues[0].ToSql() + " ) as temppart On PartBins.imbPartID = temppart.imbPartID and PartBins.imbPartRevisionID = temppart.imbPartRevisionID and PartBins.imbWarehouseID = temppart.imbWarehouseID and PartBins.imbPartBinID = temppart.imbPartBinID");
			if (parm.ChangeIDType != 1)
			{
				mergePartBinDetailsAtPartLevel(parm);
				resetDefaultBin(parm);
			}
		}
	}

	private void mergePartBinDetailsAtPartLevel(ChangeIDProcessingParms parm)
	{
		string text = parm.NewKeyValues[0].ToString();
		string value = parm.OldKeyValues[0].ToString();
		using SqlCommand sqlCommand = new SqlCommand("SELECT * FROM PartBinDetails WHERE imgPartID = @newPartID Or imgPartID = @oldPartID order by imgTransactionDate");
		sqlCommand.Parameters.AddWithValue("@newPartID", text);
		sqlCommand.Parameters.AddWithValue("@oldPartID", value);
		DataTable dataTable = parm.Database.GetDataTable(sqlCommand, parm.SqlTransaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			Dictionary<DataRow, int> dictionary = new Dictionary<DataRow, int>();
			List<DataRow> list = dataTable.AsEnumerable().ToList();
			mergePartBinDetails(parm.Database, parm.SqlTransaction, list, dictionary, text);
		}
	}

	private void mergePartBinDetails(M1Database database, SqlTransaction transaction, List<DataRow> list, Dictionary<DataRow, int> dictionary, string newPartID)
	{
		if (list.Count == 0)
		{
			return;
		}
		int num = 1;
		DataRow dataRow = list.ElementAt(0);
		dictionary.Add(dataRow, num);
		List<DataRow> list2 = new List<DataRow>();
		list2.Add(dataRow);
		for (int i = 1; i < list.Count; i++)
		{
			DataRow dataRow2 = list.ElementAt(i);
			if (dataRow.Field<string>("imgPartRevisionID").Equals(dataRow2.Field<string>("imgPartRevisionID"), StringComparison.OrdinalIgnoreCase) && dataRow.Field<string>("imgWarehouseID").Equals(dataRow2.Field<string>("imgWarehouseID"), StringComparison.OrdinalIgnoreCase) && dataRow.Field<string>("imgPartBinID").Equals(dataRow2.Field<string>("imgPartBinID"), StringComparison.OrdinalIgnoreCase))
			{
				dictionary.Add(dataRow2, ++num);
				list2.Add(dataRow2);
			}
		}
		updatePartBinDetailsTable(database, transaction, dictionary, newPartID);
		dictionary.Clear();
		foreach (DataRow item in list2)
		{
			list.Remove(item);
		}
		mergePartBinDetails(database, transaction, list, dictionary, newPartID);
	}

	private void updatePartBinDetailsTable(M1Database database, SqlTransaction transaction, Dictionary<DataRow, int> dictionary, string newKeyValue)
	{
		for (int i = 0; i < dictionary.Count; i++)
		{
			KeyValuePair<DataRow, int> keyValuePair = dictionary.ElementAt(i);
			using SqlCommand sqlCommand = new SqlCommand("SELECT * FROM PartBinDetails WHERE imgPartID = @newKeyValue AND imgPartRevisionID = @PartRevisionID AND imgWarehouseID = @WarehouseID AND imgPartBinID = @PartBinID AND imgPartBinDetailID = @PartBinDetailID");
			sqlCommand.Parameters.AddWithValue("@newKeyValue", newKeyValue);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", keyValuePair.Key.Field<string>("imgPartRevisionID"));
			sqlCommand.Parameters.AddWithValue("@WarehouseID", keyValuePair.Key.Field<string>("imgWarehouseID"));
			sqlCommand.Parameters.AddWithValue("@PartBinID", keyValuePair.Key.Field<string>("imgPartBinID"));
			sqlCommand.Parameters.AddWithValue("@PartBinDetailID", keyValuePair.Value);
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable == null || dataTable.Rows.Count == 0)
			{
				string queryString = string.Format("UPDATE PartBinDetails SET imgPartID = {0}, imgPartBinDetailID = {1} WHERE imgPartID = {2} AND imgPartRevisionID = {3} AND imgWarehouseID = {4} AND imgPartBinID = {5} AND imgPartBinDetailID = {6}", newKeyValue.ToSql(), keyValuePair.Value, keyValuePair.Key.Field<string>("imgPartID").ToSql(), keyValuePair.Key.Field<string>("imgPartRevisionID").ToSql(), keyValuePair.Key.Field<string>("imgWarehouseID").ToSql(), keyValuePair.Key.Field<string>("imgPartBinID").ToSql(), keyValuePair.Key.Field<int>("imgPartBinDetailID"));
				database.ExecuteCommand(queryString, transaction);
				dictionary.Remove(keyValuePair.Key);
				updatePartBinDetailsTable(database, transaction, dictionary, newKeyValue);
			}
		}
	}

	private void resetDefaultBin(ChangeIDProcessingParms parm)
	{
		string text = parm.NewKeyValues[0].ToString();
		string text2 = parm.OldKeyValues[0].ToString();
		string empty = string.Empty;
		Part part = new Part();
		using SqlCommand sqlCommand = new SqlCommand("SELECT imbPartRevisionID FROM PartBins WHERE imbPartID = @oldPartID and imbDefaultBin = 1 order by imbPartRevisionID");
		sqlCommand.Parameters.AddWithValue("@oldPartID", text2);
		DataTable dataTable = parm.Database.GetDataTable(sqlCommand, parm.SqlTransaction);
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			empty = row.Field<string>("imbPartRevisionID");
			using SqlCommand sqlCommand2 = new SqlCommand("SELECT imbPartRevisionID, imbWarehouseID, imbPartBinID FROM PartBins WHERE imbPartID = @newPartID and imbPartRevisionID = @PartRevisionID and imbDefaultBin = 1 ");
			sqlCommand2.Parameters.AddWithValue("@newPartID", text);
			sqlCommand2.Parameters.AddWithValue("@PartRevisionID", empty);
			DataTable dataTable2 = parm.Database.GetDataTable(sqlCommand2, parm.SqlTransaction);
			if (dataTable2 != null && dataTable2.Rows.Count != 0 && !part.HasTheSameDefaultBin(parm.Database, text2, empty, text, empty, parm.SqlTransaction))
			{
				if (parm.ChangeIDType == 2)
				{
					string queryString = $"UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = {text.ToSql()} AND imbPartRevisionID = {empty.ToSql()} ";
					parm.Database.ExecuteCommand(queryString, parm.SqlTransaction);
				}
				if (parm.ChangeIDType == 3)
				{
					string queryString2 = $"UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = {text2.ToSql()} AND imbPartRevisionID = {empty.ToSql()} ";
					parm.Database.ExecuteCommand(queryString2, parm.SqlTransaction);
				}
			}
		}
	}

	public void ProcessChangeID(ChangeIDProcessingParms parm)
	{
	}

	public void PostProcessChangeID(ChangeIDProcessingParms parm)
	{
		if (parm.ChangeIDType != 1)
		{
			new Part().RefreshPreviousQOH(parm.Database, parm.SqlTransaction, " AND imtPartID = " + parm.NewKeyValues[0].ToSql());
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		string text = "Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbDefaultBin From PartBins ";
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbInactiveBin = 1 and imbQuantityOnHand < 0 Order by imbPartRevisionID, imbWarehouseID, imbPartBinID";
		DataTable dataTable = parm.Database.GetDataTable(text);
		if (dataTable.Rows.Count > 0)
		{
			flag = true;
			stringBuilder.AppendLine("The following Part Bins cannot be inactive and have a negative quantity on hand as a result of this Change ID process and MUST be corrected: \r\n");
			foreach (DataRow row4 in dataTable.Rows)
			{
				stringBuilder.AppendLine(string.Format("[Part ID: '{0}', Revision: '{1}', Warehouse: '{2}', Bin: '{3}', Quantity on Hand: '{4}', Status: 'Inactive']", row4.Field<string>("imbPartID"), row4.Field<string>("imbPartRevisionID"), row4.Field<string>("imbWarehouseID"), row4.Field<string>("imbPartBinID"), row4.Field<decimal>("imbQuantityOnHand")));
			}
		}
		text = "Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID From PartBins ";
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbInactiveBin = 1 and imbDefaultBin = 1 Order by imbPartRevisionID, imbWarehouseID, imbPartBinID";
		dataTable = parm.Database.GetDataTable(text);
		if (dataTable.Rows.Count > 0)
		{
			flag = true;
			if (flag && stringBuilder != null && stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine("\r\n\r\n");
			}
			stringBuilder.AppendLine("The following Part Bins cannot be inactive and assigned as the default bin as a result of this Change ID process and MUST be corrected: \r\n");
			foreach (DataRow row5 in dataTable.Rows)
			{
				stringBuilder.AppendLine("[Part ID: '" + row5.Field<string>("imbPartID") + "', Revision: '" + row5.Field<string>("imbPartRevisionID") + "', Warehouse: '" + row5.Field<string>("imbWarehouseID") + "', Bin: '" + row5.Field<string>("imbPartBinID") + "', Default Bin: 'Yes', Status: 'Inactive']");
			}
		}
		text = "Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID From PartBins ";
		text += "Inner Join WarehouseBins On imbWarehouseID = inbWarehouseID and imbPartBinID = inbWarehouseBinID ";
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbInactiveBin = 0 and inbInactive = 1 Order by imbPartRevisionID, imbWarehouseID, imbPartBinID";
		dataTable = parm.Database.GetDataTable(text);
		if (dataTable.Rows.Count > 0)
		{
			flag = true;
			if (flag && stringBuilder != null && stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine("\r\n\r\n");
			}
			stringBuilder.AppendLine("The following Part Bins cannot be active when the warehouse bin is inactive as a result of this Change ID process and MUST be corrected: \r\n");
			foreach (DataRow row6 in dataTable.Rows)
			{
				stringBuilder.AppendLine("[Part ID: '" + row6.Field<string>("imbPartID") + "', Revision: '" + row6.Field<string>("imbPartRevisionID") + "', Warehouse: '" + row6.Field<string>("imbWarehouseID") + "', Bin: '" + row6.Field<string>("imbPartBinID") + "', Part Bin Status: Active, Warehouse Bin Status: Inactive]");
			}
		}
		if (flag && stringBuilder != null && stringBuilder.Length > 0)
		{
			using (LongMsgDialog longMsgDialog = new LongMsgDialog(parm.Database))
			{
				longMsgDialog.DefaultSaveFileName = DateTime.Now.ToString("yyyyddMMM_HH.mm.ss") + "_ChangePartRevisionID";
				longMsgDialog.HeaderText = "Resulting Part Bins from this Change ID process must be corrected.";
				longMsgDialog.MessageText = stringBuilder.ToString();
				longMsgDialog.ShowYesNoButtons = false;
				longMsgDialog.ControlBox = false;
				longMsgDialog.ShowDialog();
			}
		}
	}
}
