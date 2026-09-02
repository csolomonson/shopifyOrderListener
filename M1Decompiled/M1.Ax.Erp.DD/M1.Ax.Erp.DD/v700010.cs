using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.010", "Add Invoice Revaluations related tables", "2008-02-20")]
public class v700010
{
	public v700010(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ARInvoiceRevaluations"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations");
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "APInvoiceRevaluations"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations");
		}
	}
}
