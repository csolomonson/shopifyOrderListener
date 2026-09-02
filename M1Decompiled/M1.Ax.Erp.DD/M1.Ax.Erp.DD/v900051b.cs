using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to LotNumbers table", "2015-06-25")]
public class v900051b
{
	public v900051b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablInactiveDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablStatus"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update LotNumbers Set ablInactive = 1, ablInactiveDate = (Select IsNull(Max(abtTransactionDate), GetDate()) from LotNumberTransactions Where abtLotNumberID = ablLotNumberID and abtPartID = ablPartID and abtPartRevisionID = ablPartRevisionID and abtPartWarehouseLocationID = ablPartWarehouseLocationID and abtPartBinID = ablPartBinID and abtTransactionType = 9 and abtNegativeTransaction = 0) Where ablStatus = 9");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablStatus", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablQuantityToInspect"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablQuantityToInspect", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablQuantityOnHand"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablQuantityOnHand", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablPartWarehouseLocationID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablPartWarehouseLocationID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablPartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablPartBinID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ablQuantityToReturn"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", "ablQuantityToReturn", dropTriggers: true);
		}
		parms.Dmo.DropIndexes(null, parms.User, parms.DatabaseName, "LotNumbers", new DmoIndex[1]
		{
			new DmoIndex("ABLPARTID,ABLPARTREVISIONID,ABLPARTWAREHOUSELOCATIONID,ABLPARTBINID,ABLLOTNUMBERID", unique: true)
		}, parms.Messages);
		parms.Dmo.RemoveDuplicates(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", parms.Messages);
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LotNumbers", "ABLLOTNUMBERID"))
		{
			parms.Dmo.VerifyIndexes(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LotNumbers", new DmoIndex[1]
			{
				new DmoIndex("ABLPARTID,ABLPARTREVISIONID,ABLLOTNUMBERID", unique: true)
			}, parms.Messages);
		}
	}
}
