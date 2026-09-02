using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.029", "Add fields to Organizations table", "2015-04-10")]
public class v900029e
{
	public v900029e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoIntraCompanyDatasetID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoIntraCompanyDatasetID", "nvarchar", 40, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
