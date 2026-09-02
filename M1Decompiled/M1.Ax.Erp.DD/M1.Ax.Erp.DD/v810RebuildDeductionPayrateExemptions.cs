using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DeductionPayrateExemptions to support unicode", "2013-10-17")]
public class v810RebuildDeductionPayrateExemptions
{
	public v810RebuildDeductionPayrateExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DeductionPayrateExemptions", new DmoField[6]
		{
			new DmoField("lndDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lndDeductionPayRateExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("lndPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lndCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lndCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lndUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LNDDEDUCTIONID,LNDDEDUCTIONPAYRATEEXEMPTIONID", unique: true),
			new DmoIndex("LNDUNIQUEID", unique: true),
			new DmoIndex("lndDeductionID", unique: false),
			new DmoIndex("lndDeductionPayRateExemptionID", unique: false),
			new DmoIndex("lndPayrollRateID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
