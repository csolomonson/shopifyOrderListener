using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.523", "Add fields to PAYROLLHEADERBANKACCOUNTS table", "2015-05-19")]
public class v800523ag
{
	public v800523ag(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PAYROLLHEADERBANKACCOUNTS", "paaEFTDescription"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PAYROLLHEADERBANKACCOUNTS", "paaEFTDescription", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
