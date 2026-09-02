using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ARInvoiceRevaluations to support unicode", "2013-10-17")]
public class v810RebuildARInvoiceRevaluations
{
	public v810RebuildARInvoiceRevaluations(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceRevaluations", new DmoField[13]
		{
			new DmoField("arvARInvoiceRevaluationID", "int", 9, 0, nullable: false),
			new DmoField("arvARInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("arvRevalueDate", "datetime", 14, 0, nullable: true),
			new DmoField("arvExchangeAmount", "money", 12, 2, nullable: false),
			new DmoField("arvExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("arvPrevExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("arvPrevAmountBase", "money", 12, 2, nullable: false),
			new DmoField("arvPrevAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("arvGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("arvGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("arvCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("arvCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("arvUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("ARVARINVOICEREVALUATIONID", unique: true),
			new DmoIndex("ARVUNIQUEID", unique: true),
			new DmoIndex("arvARInvoiceID", unique: false),
			new DmoIndex("arvGLFiscalYearID", unique: false),
			new DmoIndex("arvGLFiscalYearPeriodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
