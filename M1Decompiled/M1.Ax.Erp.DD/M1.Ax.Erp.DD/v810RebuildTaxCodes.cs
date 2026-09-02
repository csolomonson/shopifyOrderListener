using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert TaxCodes to support unicode", "2013-10-17")]
public class v810RebuildTaxCodes
{
	public v810RebuildTaxCodes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodes", new DmoField[15]
		{
			new DmoField("xaxTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xaxTaxType", "tinyint", 1, 0, nullable: false),
			new DmoField("xaxTaxOption", "nvarchar", 1, 0, nullable: false),
			new DmoField("xaxDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaxAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xaxIncludePrimaryTax", "bit", 1, 0, nullable: false),
			new DmoField("xaxWGShowCorporateOption", "tinyint", 1, 0, nullable: false),
			new DmoField("xaxWGShowConsumerOption", "tinyint", 1, 0, nullable: false),
			new DmoField("xaxWGShowCorporateDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaxWGShowConsumerDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaxInactive", "bit", 1, 0, nullable: false),
			new DmoField("xaxInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xaxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xaxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xaxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XAXTAXCODEID", unique: true),
			new DmoIndex("XAXUNIQUEID", unique: true),
			new DmoIndex("xaxTaxType", unique: false),
			new DmoIndex("xaxTaxOption", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
