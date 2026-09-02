using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.336", "Add fields to ProductionProperties table", "2014-11-01")]
public class v800336a
{
	public v800336a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapOMUseQuotingMarkupTM"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapOMUseQuotingMarkupTM", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
