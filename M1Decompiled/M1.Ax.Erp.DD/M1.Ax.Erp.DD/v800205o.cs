using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Add RFQ Source fields to QuoteMaterials", "2011-12-06")]
public class v800205o
{
	public v800205o(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteMaterials", "qmmSourceRFQID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", "qmmSourceRFQID", "char", 10, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
