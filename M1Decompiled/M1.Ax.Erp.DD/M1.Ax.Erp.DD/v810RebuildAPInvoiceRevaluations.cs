using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APInvoiceRevaluations to support unicode", "2013-10-17")]
public class v810RebuildAPInvoiceRevaluations
{
	public v810RebuildAPInvoiceRevaluations(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceRevaluations", new DmoField[13]
		{
			new DmoField("apvAPInvoiceRevaluationID", "int", 9, 0, nullable: false),
			new DmoField("apvAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apvRevalueDate", "datetime", 14, 0, nullable: true),
			new DmoField("apvExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("apvExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("apvPrevExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("apvPrevAmountBase", "money", 12, 2, nullable: false),
			new DmoField("apvPrevAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("apvGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("apvGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("apvCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apvCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("apvUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("APVAPINVOICEREVALUATIONID", unique: true),
			new DmoIndex("APVUNIQUEID", unique: true),
			new DmoIndex("apvAPInvoiceID", unique: false),
			new DmoIndex("apvGLFiscalYearID", unique: false),
			new DmoIndex("apvGLFiscalYearPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
