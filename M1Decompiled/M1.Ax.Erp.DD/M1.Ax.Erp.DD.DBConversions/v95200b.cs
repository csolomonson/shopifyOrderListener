using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.200", "Add mrlMfgLotSize field to MRPLines table", "2021-11-30")]
public class v95200b
{
	public v95200b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "MRPLines", "mrlMfgLotSize"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPLines", "mrlMfgLotSize", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE MRPLines SET mrlMfgLotSize = IsNull(imrManufacturingLotSize,0) FROM MRPLines INNER JOIN PartRevisions ON mrlPartID = imrPartID AND mrlPartRevisionID = imrPartRevisionID");
		}
	}
}
