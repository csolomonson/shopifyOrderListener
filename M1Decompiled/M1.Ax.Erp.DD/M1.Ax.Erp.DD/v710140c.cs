using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.140", "Add volume to PartRevisions", "2008-09-25")]
public class v710140c
{
	public v710140c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrVolume"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrVolume", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
