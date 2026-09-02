using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.100", "Add new fields for leave types on STP Lines Entry (STP)", "2021-10-01")]
public class v95100e
{
	public v95100e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlCashOutLeave"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlCashOutLeave", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlUnusedLeave"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlUnusedLeave", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlPaidParentalLeave"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlPaidParentalLeave", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlWorkersComp"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlWorkersComp", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlAncillaryDefenceLeave"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlAncillaryDefenceLeave", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "STPLines", "stlOtherPaidLeave"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "STPLines", "stlOtherPaidLeave", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
