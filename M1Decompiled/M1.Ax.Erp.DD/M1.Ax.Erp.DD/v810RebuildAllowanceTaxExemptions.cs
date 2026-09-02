using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AllowanceTaxExemptions to support unicode", "2013-10-17")]
public class v810RebuildAllowanceTaxExemptions
{
	public v810RebuildAllowanceTaxExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AllowanceTaxExemptions", new DmoField[7]
		{
			new DmoField("lnoAllowanceID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnoAllowanceTaxExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("lnoIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnoIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lnoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lnoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lnoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("LNOALLOWANCEID,LNOALLOWANCETAXEXEMPTIONID", unique: true),
			new DmoIndex("LNOUNIQUEID", unique: true),
			new DmoIndex("lnoAllowanceID", unique: false),
			new DmoIndex("lnoAllowanceTaxExemptionID", unique: false),
			new DmoIndex("lnoIncomeTaxID", unique: false),
			new DmoIndex("lnoIncomeTaxTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
