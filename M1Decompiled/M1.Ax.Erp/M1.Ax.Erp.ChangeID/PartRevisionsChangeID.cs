using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using M1.Core;
using M1.Core.Script;
using M1.Extensions;
using M1.Forms.Controls;
using M1Classes92;

namespace M1.Ax.Erp.ChangeID;

[ChangeIDProcessing("PartRevisions")]
public class PartRevisionsChangeID : IChangeIDProcessing
{
	public void PreProcessChangeID(ChangeIDProcessingParms parm)
	{
		parm.DeleteStatements.AppendLine("DELETE FROM PartWarehouseLocations WHERE imlPartID = " + parm.OldKeyValues[0].ToSql() + " AND imlPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imlPartWarehouseID IN (SELECT imlPartWarehouseID FROM PartWarehouseLocations WHERE imlPartID = " + parm.NewKeyValues[0].ToSql() + " AND imlPartRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartBins WHERE imbPartID = " + parm.OldKeyValues[0].ToSql() + " AND imbPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imbWarehouseID+imbPartBinID IN (SELECT imbWarehouseID+imbPartBinID FROM PartBins WHERE imbPartID = " + parm.NewKeyValues[0].ToSql() + " AND imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartAssemblies WHERE imaMethodID = " + parm.OldKeyValues[0].ToSql() + " AND imaMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imaMethodAssemblyID IN (SELECT imaMethodAssemblyID FROM PartAssemblies WHERE imaMethodID = " + parm.NewKeyValues[0].ToSql() + " AND imaMethodRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartMaterials WHERE immMethodID = " + parm.OldKeyValues[0].ToSql() + " AND immMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND CONVERT(CHAR(6),immMethodAssemblyID)+CONVERT(CHAR(6),immMethodMaterialID) IN (SELECT CONVERT(CHAR(6),immMethodAssemblyID)+CONVERT(CHAR(6),immMethodMaterialID) FROM PartMaterials WHERE immMethodID = " + parm.NewKeyValues[0].ToSql() + " AND immMethodRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartOperations WHERE imoMethodID = " + parm.OldKeyValues[0].ToSql() + " AND imoMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND CONVERT(CHAR(6),imoMethodAssemblyID)+CONVERT(CHAR(6),imoMethodOperationID) IN (SELECT CONVERT(CHAR(6),imoMethodAssemblyID)+CONVERT(CHAR(6),imoMethodOperationID) FROM PartOperations WHERE imoMethodID = " + parm.NewKeyValues[0].ToSql() + " AND imoMethodRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartOrgReferences WHERE imzPartID = " + parm.OldKeyValues[0].ToSql() + " AND imzPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imzOrganizationID IN (SELECT imzOrganizationID FROM PartOrgReferences WHERE imzPartID = " + parm.NewKeyValues[0].ToSql() + " AND imzPartRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartCrossReferences WHERE imxPartID = " + parm.OldKeyValues[0].ToSql() + " AND imxPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND imxOrganizationID+imxLocationID IN (SELECT imxOrganizationID+imxLocationID FROM PartCrossReferences WHERE imxPartID = " + parm.NewKeyValues[0].ToSql() + " AND imxPartRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		parm.DeleteStatements.AppendLine("DELETE FROM PartRules WHERE pcrMethodID = " + parm.OldKeyValues[0].ToSql() + " AND pcrMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " AND CONVERT(CHAR(6),pcrMethodAssemblyID) IN (SELECT CONVERT(CHAR(6),pcrMethodAssemblyID) FROM PartRules WHERE pcrMethodID = " + parm.NewKeyValues[0].ToSql() + " AND pcrMethodRevisionID = " + parm.NewKeyValues[1].ToSql() + ")");
		clsPartFunctions obj = (clsPartFunctions)((ScriptApp)parm.Database.GetService(typeof(ScriptApp))).Ax("PartFunctions");
		string s = obj.GenerateFormIDForPart(parm.OldKeyValues[0].ToString(), parm.OldKeyValues[1].ToString());
		string s2 = obj.GenerateFormIDForPart(parm.NewKeyValues[0].ToString(), parm.NewKeyValues[1].ToString());
		parm.UpdateStatements.AppendLine("UPDATE FormDefinitions SET xaoFormID = " + s2.ToSql() + " Where xaoFormID = " + s.ToSql());
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiFormID = " + s2.ToSql() + " Where xaiFormID = " + s.ToSql());
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiTopLevelFormID = " + s2.ToSql() + " Where xaiTopLevelFormID = " + s.ToSql());
		parm.UpdateStatements.AppendLine("UPDATE FormInputValues SET xaiParentFormID = " + s2.ToSql() + " Where xaiParentFormID = " + s.ToSql());
		parm.UpdateStatements.AppendLine("UPDATE PartRevisions SET imrFormID = " + s2.ToSql() + " Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrPartRevisionID = " + parm.OldKeyValues[1].ToSql() + " And imrFormID <> ''");
		parm.UpdateStatements.AppendLine("UPDATE PartAssemblies SET imaPartID = " + parm.NewKeyValues[0].ToSql() + ", imaPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " Where imaMethodID = " + parm.OldKeyValues[0].ToSql() + " And imaMethodRevisionID = " + parm.OldKeyValues[1].ToSql() + " And imaMethodAssemblyID = 0");
		if (parm.NewIDExists)
		{
			parm.UpdateStatements.AppendLine("UPDATE PartRevisions SET PartRevisions.imrQuantityOnHand = PartRevisions.imrQuantityOnHand + temppart.imrQuantityOnHand,PartRevisions.imrQuantityAllocated = PartRevisions.imrQuantityAllocated + temppart.imrQuantityAllocated  From PartRevisions Inner Join (select " + parm.NewKeyValues[0].ToSql() + " as imrPartID, " + parm.NewKeyValues[1].ToSql() + " As imrPartRevisionID, imrQuantityOnHand, imrQuantityAllocated from PartRevisions Where imrPartID = " + parm.OldKeyValues[0].ToSql() + " And imrPartRevisionID = " + parm.OldKeyValues[1].ToSql() + ") as temppart On PartRevisions.imrPartID = temppart.imrPartID and PartRevisions.imrPartRevisionID = temppart.imrPartRevisionID");
			parm.UpdateStatements.AppendLine("UPDATE PartBins SET PartBins.imbQuantityOnHand = PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand, imbBinQuantityOnHand = CASE WHEN imbConversionFactor = 0 THEN PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand ELSE (PartBins.imbQuantityOnHand + temppart.imbQuantityOnHand) / imbConversionFactor END, PartBins.imbQuantityAllocated = PartBins.imbQuantityAllocated + temppart.imbQuantityAllocated  From PartBins Inner Join (select " + parm.NewKeyValues[0].ToSql() + " as imbPartID," + parm.NewKeyValues[1].ToSql() + " as imbPartRevisionID,imbWarehouseID,imbPartBinID, imbQuantityOnHand, imbQuantityAllocated from PartBins Where imbPartID = " + parm.OldKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.OldKeyValues[1].ToSql() + ") as temppart On PartBins.imbPartID = temppart.imbPartID and PartBins.imbPartRevisionID = temppart.imbPartRevisionID and PartBins.imbWarehouseID = temppart.imbWarehouseID and PartBins.imbPartBinID = temppart.imbPartBinID");
			if (parm.ChangeIDType != 1)
			{
				mergePartBinDetailsAtPartRevisionLevel(parm);
				resetDefaultBin(parm);
			}
		}
	}

	private void resetDefaultBin(ChangeIDProcessingParms parm)
	{
		string text = parm.OldKeyValues[0].ToString();
		string text2 = parm.NewKeyValues[1].ToString();
		string text3 = parm.OldKeyValues[1].ToString();
		using SqlCommand sqlCommand = new SqlCommand("SELECT imbPartID FROM PartBins WHERE imbPartID = @partID and (imbPartRevisionID = @oldPartRevID or imbPartRevisionID = @newPartRevID) and imbDefaultBin =1");
		sqlCommand.Parameters.AddWithValue("@partID", text);
		sqlCommand.Parameters.AddWithValue("@oldPartRevID", text3);
		sqlCommand.Parameters.AddWithValue("@newPartRevID", text2);
		DataTable dataTable = parm.Database.GetDataTable(sqlCommand, parm.SqlTransaction);
		if (dataTable != null && dataTable.Rows.Count > 1 && !new Part().HasTheSameDefaultBin(parm.Database, text, text3, text, text2, parm.SqlTransaction))
		{
			if (parm.ChangeIDType == 2)
			{
				string queryString = $"UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = {text.ToSql()} AND imbPartRevisionID = {text2.ToSql()} ";
				parm.Database.ExecuteCommand(queryString, parm.SqlTransaction);
			}
			if (parm.ChangeIDType == 3)
			{
				string queryString2 = $"UPDATE PartBins SET imbDefaultBin = 0 WHERE imbPartID = {text.ToSql()} AND imbPartRevisionID = {text3.ToSql()} ";
				parm.Database.ExecuteCommand(queryString2, parm.SqlTransaction);
			}
		}
	}

	private void mergePartBinDetailsAtPartRevisionLevel(ChangeIDProcessingParms parm)
	{
		string text = parm.NewKeyValues[0].ToString();
		string text2 = parm.NewKeyValues[1].ToString();
		string value = parm.OldKeyValues[0].ToString();
		string value2 = parm.OldKeyValues[1].ToString();
		using SqlCommand sqlCommand = new SqlCommand("SELECT * FROM PartBinDetails WHERE (imgPartID = @newPartID And imgPartRevisionID = @newPartRevisionID) Or (imgPartID = @oldPartID And imgPartRevisionID = @oldPartRevisionID) order by imgTransactionDate");
		sqlCommand.Parameters.AddWithValue("@newPartID", text);
		sqlCommand.Parameters.AddWithValue("@newPartRevisionID", text2);
		sqlCommand.Parameters.AddWithValue("@oldPartID", value);
		sqlCommand.Parameters.AddWithValue("@oldPartRevisionID", value2);
		DataTable dataTable = parm.Database.GetDataTable(sqlCommand, parm.SqlTransaction);
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			Dictionary<DataRow, int> dictionary = new Dictionary<DataRow, int>();
			List<DataRow> list = dataTable.AsEnumerable().ToList();
			mergePartBinDetails(parm.Database, parm.SqlTransaction, list, dictionary, text, text2);
		}
	}

	private void mergePartBinDetails(M1Database database, SqlTransaction transaction, List<DataRow> list, Dictionary<DataRow, int> dictionary, string newPartID, string newPartRevisionID)
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
			if (dataRow.Field<string>("imgWarehouseID").Equals(dataRow2.Field<string>("imgWarehouseID"), StringComparison.OrdinalIgnoreCase) && dataRow.Field<string>("imgPartBinID").Equals(dataRow2.Field<string>("imgPartBinID"), StringComparison.OrdinalIgnoreCase))
			{
				dictionary.Add(dataRow2, ++num);
				list2.Add(dataRow2);
			}
		}
		updatePartBinDetailsTable(database, transaction, dictionary, newPartID, newPartRevisionID);
		dictionary.Clear();
		foreach (DataRow item in list2)
		{
			list.Remove(item);
		}
		mergePartBinDetails(database, transaction, list, dictionary, newPartID, newPartRevisionID);
	}

	private void updatePartBinDetailsTable(M1Database database, SqlTransaction transaction, Dictionary<DataRow, int> dictionary, string newPartID, string newPartRevisionID)
	{
		for (int i = 0; i < dictionary.Count; i++)
		{
			KeyValuePair<DataRow, int> keyValuePair = dictionary.ElementAt(i);
			using SqlCommand sqlCommand = new SqlCommand("SELECT * FROM PartBinDetails WHERE imgPartID = @newKeyValue AND imgPartRevisionID = @PartRevisionID AND imgWarehouseID = @WarehouseID AND imgPartBinID = @PartBinID AND imgPartBinDetailID = @PartBinDetailID");
			sqlCommand.Parameters.AddWithValue("@newKeyValue", newPartID);
			sqlCommand.Parameters.AddWithValue("@PartRevisionID", newPartRevisionID);
			sqlCommand.Parameters.AddWithValue("@WarehouseID", keyValuePair.Key.Field<string>("imgWarehouseID"));
			sqlCommand.Parameters.AddWithValue("@PartBinID", keyValuePair.Key.Field<string>("imgPartBinID"));
			sqlCommand.Parameters.AddWithValue("@PartBinDetailID", keyValuePair.Value);
			DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
			if (dataTable == null || dataTable.Rows.Count == 0)
			{
				string queryString = string.Format("UPDATE PartBinDetails SET imgPartID = {0}, imgPartRevisionID = {1}, imgPartBinDetailID = {2} WHERE imgPartID = {3} AND imgPartRevisionID = {4} AND imgWarehouseID = {5} AND imgPartBinID = {6} AND imgPartBinDetailID = {7}", newPartID.ToSql(), newPartRevisionID.ToSql(), keyValuePair.Value, keyValuePair.Key.Field<string>("imgPartID").ToSql(), keyValuePair.Key.Field<string>("imgPartRevisionID").ToSql(), keyValuePair.Key.Field<string>("imgWarehouseID").ToSql(), keyValuePair.Key.Field<string>("imgPartBinID").ToSql(), keyValuePair.Key.Field<int>("imgPartBinDetailID"));
				database.ExecuteCommand(queryString, transaction);
				dictionary.Remove(keyValuePair.Key);
				updatePartBinDetailsTable(database, transaction, dictionary, newPartID, newPartRevisionID);
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
			new Part().RefreshPreviousQOH(parm.Database, parm.SqlTransaction, " AND imtPartID = " + parm.NewKeyValues[0].ToSql() + " AND imtPartRevisionID = " + parm.NewKeyValues[1].ToSql());
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		string text = "Select imbPartID, imbPartRevisionID, imbWarehouseID, imbPartBinID, imbQuantityOnHand, imbDefaultBin From PartBins ";
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " ";
		text += "and imbInactiveBin = 1 and imbQuantityOnHand < 0 ";
		text += "Order by imbWarehouseID, imbPartBinID";
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
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " ";
		text += "and imbInactiveBin = 1 and imbDefaultBin = 1 ";
		text += "Order by imbWarehouseID, imbPartBinID";
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
		text = text + "Where imbPartID = " + parm.NewKeyValues[0].ToSql() + " and imbPartRevisionID = " + parm.NewKeyValues[1].ToSql() + " ";
		text += "and imbInactiveBin = 0 and inbInactive = 1 ";
		text += "Order by imbWarehouseID, imbPartBinID";
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
