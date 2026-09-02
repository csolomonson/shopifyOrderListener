using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.521", "Add fields to DATASETPROPERTIES table", "2015-05-19")]
public class v800521ad
{
	public v800521ad(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DATASETPROPERTIES", "xadIgnoreSSLCertValidate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DATASETPROPERTIES", "xadIgnoreSSLCertValidate", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DATASETPROPERTIES", "xadTINType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DATASETPROPERTIES", "xadTINType", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
