using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add Part Description WebGear Suppression field", "2011-12-06")]
public class v800205g
{
	public v800205g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrSuppressShortDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrSuppressShortDescription", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
