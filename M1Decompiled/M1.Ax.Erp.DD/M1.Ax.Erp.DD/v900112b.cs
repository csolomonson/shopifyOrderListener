using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.112", "Add Non-Nettable Type to Warehouses table", "2015-01-27")]
public class v900112b
{
	public v900112b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Warehouses", "imwNonNettableType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Warehouses", "imwNonNettableType", "tinyint", 1, 0, verifyIndexes: false, dropTriggers: true, isNullable: false, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE Warehouses SET imwNonNettableType = 1 WHERE imwDoNotIncludeInJobCosts <> 0");
		}
	}
}
