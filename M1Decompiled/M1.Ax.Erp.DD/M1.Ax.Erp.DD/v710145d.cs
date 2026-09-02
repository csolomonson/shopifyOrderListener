using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.145", "Add Shipment Awaiting Invoice GL Account to tables", "2008-07-23")]
public class v710145d
{
	public v710145d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafShipAwaitInvoiceGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafShipAwaitInvoiceGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauShipAwaitInvoiceGLAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauShipAwaitInvoiceGLAccountID", "char", 11, 0, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafShipmentJournalsDate"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafShipmentJournalsDate", dropTriggers: true);
		}
	}
}
