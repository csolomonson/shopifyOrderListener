using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.063", "Add fields to Quotes table", "2013-12-23")]
public class v810063g
{
	public v810063g(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Quotes", "qmpTaxDate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Quotes", "qmpTaxDate", "date", 14, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Quotes", "qmpTaxDate"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update Quotes Set qmpTaxDate = IsNull(qmpQuoteDate,qmpDueDate)");
		}
	}
}
