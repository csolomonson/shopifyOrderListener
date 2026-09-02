using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeLeaveAccruals to support unicode", "2013-10-17")]
public class v810RebuildEmployeeLeaveAccruals
{
	public v810RebuildEmployeeLeaveAccruals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeLeaveAccruals", new DmoField[9]
		{
			new DmoField("lmnEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmnLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmnCurrentUnawardedAmount", "numeric", 8, 3, nullable: false),
			new DmoField("lmnCurrentAccruedAmount", "numeric", 8, 3, nullable: false),
			new DmoField("lmnInactive", "bit", 1, 0, nullable: false),
			new DmoField("lmnInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("lmnCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmnCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LMNEMPLOYEEID,LMNLEAVEACCRUALID", unique: true),
			new DmoIndex("LMNUNIQUEID", unique: true),
			new DmoIndex("lmnEmployeeID", unique: false),
			new DmoIndex("lmnLeaveAccrualID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
