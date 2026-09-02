using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.023", "Add Deposit Fields to ARInvoices tables", "2013-04-02")]
public class v810023
{
	public v810023(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlDepositAmountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlDepositAmountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlDepositAmountBase = -1 * Round(arlExtendedPriceBase+arlTaxAmountBase+arlSecondTaxAmountBase+arlFreightAmountBase,2) Where arlDepositLine <> 0");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlDepositAmountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlDepositAmountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARInvoiceLines Set arlDepositAmountForeign = -1 * Round(arlExtendedPriceForeign+arlTaxAmountForeign+arlSecondTaxAmountForeign+arlFreightAmountForeign,2) Where arlDepositLine <> 0");
		}
	}
}
