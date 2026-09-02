using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.247", "Add Lot Number Transaction fields", "2012-06-07")]
public class v800247
{
	public v800247(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DATASETPROPERTIES", "xadWebAddress"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DATASETPROPERTIES", "xadWebAddress", "varchar", 100, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtTableName"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtTableName", "varchar", 30, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtTableUniqueID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtTableUniqueID", "uniqueidentifier", 16, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtStatus"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LOTNUMBERTRANSACTIONS", "abtStatus", "numeric", 2, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
