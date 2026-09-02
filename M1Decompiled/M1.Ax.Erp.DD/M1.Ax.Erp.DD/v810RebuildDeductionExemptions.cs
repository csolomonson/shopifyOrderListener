using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DeductionExemptions to support unicode", "2013-10-17")]
public class v810RebuildDeductionExemptions
{
	public v810RebuildDeductionExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DeductionExemptions", new DmoField[6]
		{
			new DmoField("lmuDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmuDeductionExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("lmuExemptDeductionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmuUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMUDEDUCTIONID,LMUDEDUCTIONEXEMPTIONID", unique: true),
			new DmoIndex("LMUUNIQUEID", unique: true),
			new DmoIndex("lmuDeductionID", unique: false),
			new DmoIndex("lmuDeductionExemptionID", unique: false),
			new DmoIndex("lmuExemptDeductionID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
