using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.161", "Add Unit Cost fields to PartTransactions", "2011-09-02")]
public class v800161
{
	public v800161(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostAverage"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostAverage", "numeric", 15, 5, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostLast"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostLast", "numeric", 15, 5, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostStandard"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostStandard", "numeric", 15, 5, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
