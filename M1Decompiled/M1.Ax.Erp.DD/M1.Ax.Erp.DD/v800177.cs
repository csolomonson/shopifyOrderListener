using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.177", "Add database forecolor", "2011-10-03")]
public class v800177
{
	public v800177(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadForeColor"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadForeColor", "numeric", 8, 0, verifyIndexes: false, dropTriggers: false, parms.Messages);
		}
	}
}
