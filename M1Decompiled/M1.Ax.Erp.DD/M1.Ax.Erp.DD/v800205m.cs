using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.205", "Refresh indexes on AR/AP Revaluation tables", "2011-12-06")]
public class v800205m
{
	public v800205m(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations"))
		{
			parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", parms.Messages, null);
		}
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations"))
		{
			parms.Dmo.VerifyIndexesOnTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", parms.Messages, null);
		}
	}
}
