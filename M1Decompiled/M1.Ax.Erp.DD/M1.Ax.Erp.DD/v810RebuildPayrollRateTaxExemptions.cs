using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollRateTaxExemptions to support unicode", "2013-10-17")]
public class v810RebuildPayrollRateTaxExemptions
{
	public v810RebuildPayrollRateTaxExemptions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollRateTaxExemptions", new DmoField[7]
		{
			new DmoField("pavPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pavPayrollRateTaxExemptionID", "smallint", 4, 0, nullable: false),
			new DmoField("pavIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pavIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pavCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pavCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pavUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("PAVPAYROLLRATEID,PAVPAYROLLRATETAXEXEMPTIONID", unique: true),
			new DmoIndex("PAVUNIQUEID", unique: true),
			new DmoIndex("pavPayrollRateID", unique: false),
			new DmoIndex("pavPayrollRateTaxExemptionID", unique: false),
			new DmoIndex("pavIncomeTaxID", unique: false),
			new DmoIndex("pavIncomeTaxTypeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
