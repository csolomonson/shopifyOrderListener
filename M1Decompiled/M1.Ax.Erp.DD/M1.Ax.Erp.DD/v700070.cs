using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.070", "Add Created By fields to ARPaymentEpays", "2008-03-12")]
public class v700070
{
	public v700070(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentEPays", "areCreatedBy"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentEPays", "areCreatedBy", "char", 20, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentEPays", "areCreatedDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentEPays", "areCreatedDate", "datetime", 14, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
	}
}
