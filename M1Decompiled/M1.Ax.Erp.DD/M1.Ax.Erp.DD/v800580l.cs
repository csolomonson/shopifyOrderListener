using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.580", "Add fields to Organizations table", "2015-06-23")]
public class v800580l
{
	public v800580l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoUPSWSBillingOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoUPSWSBillingOption", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
