using M1.Ax.Erp.DD.Helpers;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.6.100", "Add HasQOHQTI to WarehouseBins", "2022-12-09")]
public class v96100b
{
	public v96100b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "WarehouseBins", "inbHasQOHQTI"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseBins", "inbHasQOHQTI", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE WarehouseBins SET inbHasQOHQTI = 0");
			string queryString = M1Helpers.UpdateWarehouseBinsQohQtiFlagToTrue();
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, queryString);
		}
	}
}
