using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Remove COGS fields from Financial Properties", "2008-05-15")]
public class v710000p
{
	public v710000p(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafARCreateStockJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafARCreateStockJournals", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreateShipmentJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCreateShipmentJournals", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreateMfgReceiptJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCreateMfgReceiptJournals", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreatePurchaseJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCreatePurchaseJournals", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreateMaterialIssueJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCreateMaterialIssueJournals", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafCreateAdjustmentJournals"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafCreateAdjustmentJournals", dropTriggers: true);
		}
	}
}
