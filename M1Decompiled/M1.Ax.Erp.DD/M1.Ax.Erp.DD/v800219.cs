using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.219", "Alter NET1 field type in FinancialProperties table", "2012-02-10")]
public class v800219
{
	public v800219(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARNET1GatewayID"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARNET1GatewayID", "varchar", 20, 0, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARNET1MerchantKey"))
		{
			parms.Dmo.AlterColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARNET1MerchantKey", "varchar", 20, 0, parms.Messages);
		}
	}
}
