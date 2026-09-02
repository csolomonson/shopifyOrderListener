using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.579", "Add fields to Deductions table", "2015-06-23")]
public class v800579q
{
	public v800579q(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Deductions", "padSuperannuationFundID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Deductions", "padSuperannuationFundID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
