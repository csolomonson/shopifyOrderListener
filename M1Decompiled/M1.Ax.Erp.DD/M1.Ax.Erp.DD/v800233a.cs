using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.233", "Add Do Not Include In Job Costs in Warehouses table", "2012-03-14")]
public class v800233a
{
	public v800233a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Warehouses", "imwDoNotIncludeInJobCosts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Warehouses", "imwDoNotIncludeInJobCosts", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
