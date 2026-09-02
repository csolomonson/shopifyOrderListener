using M1.Ax.Erp.DD.Helpers;
using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.401", "Change discount percent format on Quote Quantities and Sales Order Lines tables", "2022-11-11")]
public class v95401
{
	public v95401(DBConversionParms parms)
	{
		string queryString = M1Helpers.ConvertFloatColumnToNumeric("QuoteQuantities", "qmqDiscountPercent");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, queryString);
		string queryString2 = M1Helpers.ConvertFloatColumnToNumeric("SalesOrderLines", "omlDiscountPercent");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, queryString2);
	}
}
