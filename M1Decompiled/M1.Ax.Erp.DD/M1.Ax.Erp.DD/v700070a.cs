using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.070", "Add Auto Create Revision fields to Produciton Prop", "2008-03-13")]
public class v700070a
{
	public v700070a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMAutoCreateRevision"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMAutoCreateRevision", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMAutoCreateRevisionID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMAutoCreateRevisionID", "char", 15, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
