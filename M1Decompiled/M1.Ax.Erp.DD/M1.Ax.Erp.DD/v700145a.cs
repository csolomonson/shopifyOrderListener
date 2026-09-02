using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.145", "Update QuoteQuantities table", "2008-05-16")]
public class v700145a
{
	public v700145a(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteQuantities Set qmqFullRevisedUnitPriceBase = qmqRevisedUnitPriceBase, qmqFullRevisedUnitPriceForeign = qmqRevisedUnitPriceForeign Where qmqDiscountPercent = 0 And qmqFullRevisedUnitPriceBase = 0 And qmqFullRevisedUnitPriceForeign = 0 And qmqFullRevisedUnitPriceBase <> qmqRevisedUnitPriceBase And qmqFullRevisedUnitPriceForeign <> qmqRevisedUnitPriceForeign");
	}
}
