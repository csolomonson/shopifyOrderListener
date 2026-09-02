using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.107", "Add expense account total fields to various tables", "2015-11-24")]
public class v900107b
{
	public v900107b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoExpenseSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoExpenseSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlExpenseSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlExpenseSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartRevisions", "imrExpenseSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRevisions", "imrExpenseSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "LandedCostCategories", "rmaExpenseSplitPercentTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LandedCostCategories", "rmaExpenseSplitPercentTotal", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
