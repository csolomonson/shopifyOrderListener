using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollHeaderLeaveAccruals to support unicode", "2013-10-17")]
public class v810RebuildPayrollHeaderLeaveAccruals
{
	public v810RebuildPayrollHeaderLeaveAccruals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderLeaveAccruals", new DmoField[19]
		{
			new DmoField("lmfPayrollHeaderLeaveAccrualID", "int", 9, 0, nullable: false),
			new DmoField("lmfPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("lmfPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("lmfLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmfOriginalAccruedAmount", "numeric", 8, 3, nullable: false),
			new DmoField("lmfCurrentLeaveTaken", "numeric", 8, 3, nullable: false),
			new DmoField("lmfAccruedBalance", "numeric", 8, 3, nullable: false),
			new DmoField("lmfOriginalUnawardedAmount", "numeric", 8, 3, nullable: false),
			new DmoField("lmfCurrentLeaveAdded", "numeric", 8, 3, nullable: false),
			new DmoField("lmfUnawardedBalance", "numeric", 8, 3, nullable: false),
			new DmoField("lmfOverallBalance", "numeric", 8, 3, nullable: false),
			new DmoField("lmfPayrollEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmfTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("lmfLeaveType", "nvarchar", 1, 0, nullable: false),
			new DmoField("lmfAdjustment", "bit", 1, 0, nullable: false),
			new DmoField("lmfTerminationProcessed", "bit", 1, 0, nullable: false),
			new DmoField("lmfCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmfCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMFPAYROLLHEADERLEAVEACCRUALID", unique: true),
			new DmoIndex("LMFUNIQUEID", unique: true),
			new DmoIndex("lmfPayrollSessionID", unique: false),
			new DmoIndex("lmfPayrollHeaderID", unique: false),
			new DmoIndex("lmfLeaveAccrualID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
