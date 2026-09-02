using M1.Core;

namespace M1.Ax.Core.DD.DBConversions;

[DBConversion("8.10.040", "Add ExtensionVersions to DatasetProperties", "2013-09-22")]
public class v810040
{
	public v810040(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "DatasetProperties", "xadExtensionVersions"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DatasetProperties", "xadExtensionVersions", "text", 50, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
