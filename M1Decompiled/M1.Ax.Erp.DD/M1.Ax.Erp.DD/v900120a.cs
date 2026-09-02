using System.Data;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.120", "Refresh lot number statuses/quantites", "2016-01-07")]
public class v900120a
{
	public v900120a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "LotNumberStatuses"))
		{
			DataTable dataTable = parms.Database.GetDataTable("select ablLotNumberID, ablPartID, ablPartRevisionID from LotNumbers");
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					lotNumberDefinition.LoadLotOrSerialNumbers(parms.Database, row3.Field<string>("ablLotNumberID"));
					lotNumberDefinition.RefreshStatuses(parms.Database, null, row3.Field<string>("ablPartID"), row3.Field<string>("ablPartRevisionID"), row3.Field<string>("ablLotNumberID"));
				}
				if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactiveTemp"))
				{
					parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactiveTemp", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
				}
				if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactiveDateTemp"))
				{
					parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactiveDateTemp", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
				}
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LotNumbers Set ablInactiveTemp = ablInactive, ablInactiveDateTemp = ablInactiveDate");
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LotNumbers Set ablInactive = 0, ablInactiveDate = NULL");
				parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LotNumbers Set ablInactive = 1, ablInactiveDate = (SELECT Top 1 IsNull(abtTransactionDate, GETDATE()) FROM LotNumberTransactions WHERE abtPartID = ablPartID AND abtPartRevisionID = ablPartRevisionID AND abtTransactionType = 9 AND abtLotNumberID = ablLotNumberID ORDER BY abtTransactionDate Desc, abtLotNumberTransactionID Desc) From LotNumbers Where (SELECT Top 1 IsNull(abtTransactionType, 0) As abtTransactionType FROM LotNumberTransactions WHERE abtPartID = ablPartID AND abtPartRevisionID = ablPartRevisionID AND abtTransactionType IN(9, 10) AND abtLotNumberID = ablLotNumberID ORDER BY abtTransactionDate Desc, abtLotNumberTransactionID Desc) = 9 And (SELECT IsNull(Count(absLotNumberID), 0) FROM LotNumberStatuses WHERE absPartID = ablPartID AND absPartRevisionID = ablPartRevisionID AND absLotNumberID = ablLotNumberID AND absStatus NOT IN(0, 4, 6) AND absQuantity <> 0) = 0");
				string text = string.Empty;
				dataTable = parms.Database.GetDataTable("select ablLotNumberID, ablPartID, ablPartRevisionID, ablInactive, ablInactiveTemp, ablInactiveDate, ablInactiveDateTemp from LotNumbers Where ablInactive != ablInactiveTemp");
				if (dataTable.Rows.Count != 0)
				{
					foreach (DataRow row4 in dataTable.Rows)
					{
						text = text + row4.Field<string>("ablPartID").Trim() + " - " + row4.Field<string>("ablPartRevisionID").Trim() + " - " + row4.Field<string>("ablLotNumberID").Trim() + " - " + (row4.Field<bool>("ablInactive") ? "Inactive" : "Active") + " - " + row4["ablInactiveDate"].ToString() + "\n\r";
					}
				}
				if (text.Trim().Length != 0 && parms.Messages != null)
				{
					text = "The following lot number records have had their inactive status changed:\n\r" + text;
					parms.Messages.Add(text);
				}
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactiveTemp"))
				{
					parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactiveTemp", dropTriggers: true);
				}
				if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactiveDateTemp"))
				{
					parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactiveDateTemp", dropTriggers: true);
				}
			}
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SerialNumberStatuses"))
		{
			return;
		}
		DataTable dataTable2 = parms.Database.GetDataTable("select imsSerialNumberID, imsPartID, imsPartRevisionID from SerialNumbers");
		if (dataTable2.Rows.Count == 0)
		{
			return;
		}
		SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
		foreach (DataRow row5 in dataTable2.Rows)
		{
			serialNumberDefinition.LoadLotOrSerialNumbers(parms.Database, row5.Field<string>("imsSerialNumberID"));
			serialNumberDefinition.RefreshStatuses(parms.Database, null, row5.Field<string>("imsPartID"), row5.Field<string>("imsPartRevisionID"), row5.Field<string>("imsSerialNumberID"));
		}
	}
}
