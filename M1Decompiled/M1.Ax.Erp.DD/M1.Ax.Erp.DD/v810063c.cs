using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.063", "Add fields to SalesOrderLines table", "2013-12-23")]
public class v810063c
{
	public v810063c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlExtendedWeight"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlExtendedWeight", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlExtendedWeight"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderLines Set omlExtendedWeight = omlWeight*omlOrderQuantity");
		}
	}
}
