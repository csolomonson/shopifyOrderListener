using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PayrollHeaderTotals to support unicode", "2013-10-17")]
public class v810RebuildPayrollHeaderTotals
{
	public v810RebuildPayrollHeaderTotals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PayrollHeaderTotals", new DmoField[25]
		{
			new DmoField("pagPayrollSessionID", "int", 9, 0, nullable: false),
			new DmoField("pagPayrollHeaderID", "int", 7, 0, nullable: false),
			new DmoField("pagPayrollHeaderTotalID", "smallint", 4, 0, nullable: false),
			new DmoField("pagPayrollRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pagRate", "numeric", 8, 4, nullable: false),
			new DmoField("pagHours", "numeric", 9, 2, nullable: false),
			new DmoField("pagRDOHours", "numeric", 9, 2, nullable: false),
			new DmoField("pagSubTotal", "money", 10, 2, nullable: false),
			new DmoField("pagLeaveLoadingRate", "numeric", 8, 4, nullable: false),
			new DmoField("pagLeaveLoadingAmount", "money", 10, 2, nullable: false),
			new DmoField("pagDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pagGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("pagNumberOfPays", "tinyint", 2, 0, nullable: false),
			new DmoField("pagLeaveAccrualID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pagPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("pagTerminationHours", "numeric", 9, 2, nullable: false),
			new DmoField("pagShiftGroup", "tinyint", 1, 0, nullable: false),
			new DmoField("pagCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pagCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pagUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("pagBaseRate", "numeric", 8, 4, nullable: false),
			new DmoField("pagAusLumpSumType", "nvarchar", 1, 0, nullable: false),
			new DmoField("pagAusLumpSumAType", "nvarchar", 1, 0, nullable: false),
			new DmoField("pagAusLeaveTypeID", "nvarchar", 1, 0, nullable: false),
			new DmoField("pagAusLeaveTypeDescription", "nvarchar", 50, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PAGPAYROLLSESSIONID,PAGPAYROLLHEADERID,PAGPAYROLLHEADERTOTALID", unique: true),
			new DmoIndex("PAGUNIQUEID", unique: true),
			new DmoIndex("pagPayrollSessionID", unique: false),
			new DmoIndex("pagPayrollHeaderID", unique: false),
			new DmoIndex("pagPayrollHeaderTotalID", unique: false),
			new DmoIndex("pagPayrollRateID", unique: false),
			new DmoIndex("pagLeaveAccrualID", unique: false),
			new DmoIndex("pagPostedToGL", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
