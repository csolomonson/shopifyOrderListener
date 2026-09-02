using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AllowancePayRateExemptions to support unicode", "2013-10-17")]
public class v810RebuildAllowancePayRateExemptions
{
	public v810RebuildAllowancePayRateExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AllowancePayRateExemptions", new DmoField[6]
		{
			new DmoField("lmqAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmqAllowancePayRateExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("lmqPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMQALLOWANCEID,LMQALLOWANCEPAYRATEEXEMPTIONID", unique: true),
			new DmoIndex("LMQUNIQUEID", unique: true),
			new DmoIndex("lmqAllowanceID", unique: false),
			new DmoIndex("lmqAllowancePayRateExemptionID", unique: false),
			new DmoIndex("lmqPayrollRateID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
