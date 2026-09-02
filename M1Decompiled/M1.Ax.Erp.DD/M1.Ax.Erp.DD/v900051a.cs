using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.051", "Add fields to SerialNumbers table", "2015-06-25")]
public class v900051a
{
	public v900051a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsInactive"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsInactive", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsInactiveDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsInactiveDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsStatus"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SerialNumbers Set imsInactive = 1, imsInactiveDate = (Select IsNull(Max(sntTransactionDate), GetDate()) from SerialNumberTransactions Where sntSerialNumberID = imsSerialNumberID and sntPartID = imsPartID and sntPartRevisionID = imsPartRevisionID and sntPartWarehouseLocationID = imsPartWarehouseLocationID and sntPartBinID = imsPartBinID and sntTransactionType = 9) Where imsStatus = 9");
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsStatus", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsPartWarehouseLocationID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsPartWarehouseLocationID", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SerialNumbers", "imsPartBinID"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SerialNumbers", "imsPartBinID", dropTriggers: true);
		}
	}
}
