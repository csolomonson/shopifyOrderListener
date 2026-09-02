using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollRateExpenseLinks to support unicode", "2013-10-17")]
public class v810RebuildPayrollRateExpenseLinks
{
	public v810RebuildPayrollRateExpenseLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollRateExpenseLinks", new DmoField[7]
		{
			new DmoField("paqPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paqPayrollRateExpenseLinkID", "smallint", 4, 0, nullable: false),
			new DmoField("paqExpenseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paqGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("paqCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paqCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("PAQPAYROLLRATEID,PAQPAYROLLRATEEXPENSELINKID", unique: true),
			new DmoIndex("PAQUNIQUEID", unique: true),
			new DmoIndex("paqPayrollRateID", unique: false),
			new DmoIndex("paqPayrollRateExpenseLinkID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
