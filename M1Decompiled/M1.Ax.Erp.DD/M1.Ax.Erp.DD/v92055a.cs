using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.055", "Add fields to RMAReceiptLines table", "2016-12-19")]
public class v92055a
{
	public v92055a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlTotalComponentCosts"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptLines", "rrlTotalComponentCosts", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlTotalExtendedCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptLines", "rrlTotalExtendedCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAReceiptLines", "rrlTotalExtendedCostForeign"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptLines", "rrlTotalExtendedCostForeign", dropTriggers: true);
		}
	}
}
