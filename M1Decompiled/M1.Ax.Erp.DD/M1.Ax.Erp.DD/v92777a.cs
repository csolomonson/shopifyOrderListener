using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.777", "Updating quantity to total field", "2018-08-20")]
public class v92777a
{
	public v92777a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlQuantityToTotal"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QuoteLines Set qmlQuantityToTotal = 1 Where qmlQuantityToTotal = 0");
		}
	}
}
