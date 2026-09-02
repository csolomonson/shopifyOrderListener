using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.400", "Add fields to ProductionProperties table to address Negative Quantity on Hand", "2022-08-08")]
public class v95400
{
	public v95400(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMAllowNegativeQtyOnHand"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMAllowNegativeQtyOnHand", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapIMEnableWarningWhenNegative"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapIMEnableWarningWhenNegative", "bit", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ProductionProperties", "xapAllowNegQtyOnHandHistory"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProductionProperties", "xapAllowNegQtyOnHandHistory", "nvarchar(max)", 50, 0, verifyIndexes: false, dropTriggers: true, isNullable: true, parms.Messages);
		}
	}
}
