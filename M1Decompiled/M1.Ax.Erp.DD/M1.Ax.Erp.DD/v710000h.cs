using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.000", "Add fields to RMA Claim Lines table", "2008-05-09")]
public class v710000h
{
	public v710000h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralFullUnitPriceBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralFullUnitPriceBase", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralFullUnitPriceForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralFullUnitPriceForeign", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralDiscountPercent"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralDiscountPercent", "numeric", 6, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralUnitDiscountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralUnitDiscountBase", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralUnitDiscountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralUnitDiscountForeign", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralFullExtendedPriceBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralFullExtendedPriceBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralFullExtendedPriceForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralFullExtendedPriceForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralExtendedDiscountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralExtendedDiscountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "RMAClaimLines", "ralExtendedDiscountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimLines", "ralExtendedDiscountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, parms.Messages);
		}
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE RMAClaimLines SET ralFullUnitPriceBase=ralUnitPrice, ralFullUnitPriceForeign=ralUnitPriceForeign, ralDiscountPercent=0, ralUnitDiscountBase=0, ralUnitDiscountForeign=0, ralFullExtendedPriceBase=ralExtendedPrice, ralFullExtendedPriceForeign=ralExtendedPriceForeign, ralExtendedDiscountBase=0, ralExtendedDiscountForeign=0 ");
	}
}
