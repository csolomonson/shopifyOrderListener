using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Alter Unit Cost fields in PartTransactions", "2011-12-06")]
public class v800205f
{
	public v800205f(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostAverage"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostAverage", "numeric", 15, 5, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostLast"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostLast", "numeric", 15, 5, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartTransactions", "imtUnitCostStandard"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactions", "imtUnitCostStandard", "numeric", 15, 5, parms.Messages);
		}
	}
}
