using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.139", "Add ARPaymentNET1 table", "2011-06-01")]
public class v800139
{
	public v800139(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ARPaymentNET1"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentNET1");
		}
	}
}
