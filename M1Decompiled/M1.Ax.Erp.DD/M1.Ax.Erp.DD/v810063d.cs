using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.063", "Add fields to QuoteLines table", "2013-12-23")]
public class v810063d
{
	public v810063d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlTaxDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlTaxDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlQuantityToTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlQuantityToTotal", "tinyint", 1, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlTaxesCalculated"))
		{
			parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlTaxesCalculated", dropTriggers: true);
		}
	}
}
