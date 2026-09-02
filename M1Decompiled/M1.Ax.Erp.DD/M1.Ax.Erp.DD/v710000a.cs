using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add Documents fields to SalesOrderLines/JobMaterials", "2008-03-25")]
public class v710000a
{
	public v710000a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlDocuments"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlDocuments", "text", 50, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "JobMaterials", "jmmDocuments"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobMaterials", "jmmDocuments", "text", 50, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
