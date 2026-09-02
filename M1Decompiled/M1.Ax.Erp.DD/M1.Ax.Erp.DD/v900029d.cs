using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.029", "Add fields to DatasetProperties table", "2015-04-10")]
public class v900029d
{
	public v900029d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadIntraCompanyOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadIntraCompanyOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadAllowIntraCompanyTrans"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadAllowIntraCompanyTrans", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
