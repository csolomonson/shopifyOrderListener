using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.579", "Add fields to Allowances table", "2015-06-23")]
public class v800579r
{
	public v800579r(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Allowances", "paoSuperannuationFundID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Allowances", "paoSuperannuationFundID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
