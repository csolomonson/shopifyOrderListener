using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.149", "Add arcCardSuffix to ARPaymentNET1 table", "2011-07-22")]
public class v800149
{
	public v800149(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentNET1", "arcCardSuffix"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentNET1", "arcCardSuffix", "char", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentNET1", "arcCardType"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentNET1", "arcCardType", "char", 20, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
