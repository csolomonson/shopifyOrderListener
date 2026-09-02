using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.828", "Add fields to DatasetProperties table", "2020-10-01")]
public class v92828c
{
	public v92828c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadUpdateMasterDataInFinPkg"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadUpdateMasterDataInFinPkg", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
