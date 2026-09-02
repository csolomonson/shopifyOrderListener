using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.012", "Add Tax Code Plants table", "2012-10-19")]
public class v810012
{
	public v810012(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "TaxCodePlants"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodePlants");
		}
	}
}
