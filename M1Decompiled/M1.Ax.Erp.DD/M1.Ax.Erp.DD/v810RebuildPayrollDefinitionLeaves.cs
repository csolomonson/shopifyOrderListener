using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollDefinitionLeaves to support unicode", "2013-10-17")]
public class v810RebuildPayrollDefinitionLeaves
{
	public v810RebuildPayrollDefinitionLeaves(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollDefinitionLeaves", new DmoField[5]
		{
			new DmoField("lmcPayrollDefinitionID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmcLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LMCPAYROLLDEFINITIONID,LMCLEAVEACCRUALID", unique: true),
			new DmoIndex("LMCUNIQUEID", unique: true),
			new DmoIndex("lmcPayrollDefinitionID", unique: false),
			new DmoIndex("lmcLeaveAccrualID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
