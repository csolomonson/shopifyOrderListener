using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to JobOperations table", "2015-08-14")]
public class v900074c
{
	public v900074c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoScrapQuantityReceived"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoScrapQuantityReceived", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoQuantityToInspect"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoQuantityToInspect", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoPartBinID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoPartBinID", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoPartWarehouseLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoPartWarehouseLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobOperations", "jmoQuantityToReturn"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoQuantityToReturn", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
