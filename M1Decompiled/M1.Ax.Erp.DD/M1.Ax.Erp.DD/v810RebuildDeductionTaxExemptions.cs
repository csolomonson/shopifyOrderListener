using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DeductionTaxExemptions to support unicode", "2013-10-17")]
public class v810RebuildDeductionTaxExemptions
{
	public v810RebuildDeductionTaxExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DeductionTaxExemptions", new DmoField[7]
		{
			new DmoField("pauDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pauDeductionTaxExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("pauIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pauIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pauCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pauCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pauUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PAUDEDUCTIONID,PAUDEDUCTIONTAXEXEMPTIONID", unique: true),
			new DmoIndex("PAUUNIQUEID", unique: true),
			new DmoIndex("pauDeductionID", unique: false),
			new DmoIndex("pauDeductionTaxExemptionID", unique: false),
			new DmoIndex("pauIncomeTaxID", unique: false),
			new DmoIndex("pauIncomeTaxTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
