using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Remove landed cost fields from ARInvoiceLines/ReceiptLines", "2011-12-06")]
public class v800205n
{
	public v800205n(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlEstUnitLandedCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlEstUnitLandedCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlEstTotalLandedCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlEstTotalLandedCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlActualUnitLandedCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlActualUnitLandedCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlActualTotalLandedCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlActualTotalLandedCost", dropTriggers: true);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ReceiptLines", "rmlLandedUnitCost"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ReceiptLines", "rmlLandedUnitCost", dropTriggers: true);
		}
	}
}
