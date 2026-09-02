using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert TimecardLines to support unicode", "2013-10-17")]
public class v810RebuildTimecardLines
{
	public v810RebuildTimecardLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", new DmoField[44]
		{
			new DmoField("lmlTimecardID", "int", 9, 0, nullable: false),
			new DmoField("lmlTimecardLineID", "smallint", 4, 0, nullable: false),
			new DmoField("lmlTimecardType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmlIndirectLaborID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmlJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("lmlJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("lmlWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlProcessID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmlShiftID", "smallint", 3, 0, nullable: false),
			new DmoField("lmlLaborHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmlMachineHours", "numeric", 8, 2, nullable: false),
			new DmoField("lmlWorkType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmlGoodQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lmlScrapQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lmlScrapReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlSetupPercentCompleted", "smallint", 3, 0, nullable: false),
			new DmoField("lmlCompletionType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmlReworkReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlReworkQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("lmlLaborCost", "numeric", 10, 2, nullable: false),
			new DmoField("lmlOverheadCost", "numeric", 10, 2, nullable: false),
			new DmoField("lmlExpenseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmlRoundedStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmlRoundedEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmlActualStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmlActualEndTime", "datetime", 14, 0, nullable: true),
			new DmoField("lmlActive", "bit", 1, 0, nullable: false),
			new DmoField("lmlSuspended", "bit", 1, 0, nullable: false),
			new DmoField("lmlSource", "tinyint", 1, 0, nullable: false),
			new DmoField("lmlLaborDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmlLaborDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmlTransferredToPayroll", "bit", 1, 0, nullable: false),
			new DmoField("lmlCreatedFromPayrollSession", "bit", 1, 0, nullable: false),
			new DmoField("lmlPostedToWIP", "bit", 1, 0, nullable: false),
			new DmoField("lmlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("lmlMachineHoursCalculated", "bit", 1, 0, nullable: false),
			new DmoField("lmlLaborHoursCalculated", "bit", 1, 0, nullable: false),
			new DmoField("lmlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmlUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("lmlCreatedFromMobile", "bit", 1, 0, nullable: false)
		}, new DmoIndex[20]
		{
			new DmoIndex("LMLTIMECARDID,LMLTIMECARDLINEID", unique: true),
			new DmoIndex("LMLUNIQUEID", unique: true),
			new DmoIndex("lmlTimecardID", unique: false),
			new DmoIndex("lmlTimecardLineID", unique: false),
			new DmoIndex("lmlIndirectLaborID", unique: false),
			new DmoIndex("lmlJobID", unique: false),
			new DmoIndex("lmlJobAssemblyID", unique: false),
			new DmoIndex("lmlJobOperationID", unique: false),
			new DmoIndex("lmlWorkCenterID", unique: false),
			new DmoIndex("lmlEmployeeID", unique: false),
			new DmoIndex("lmlWorkType", unique: false),
			new DmoIndex("lmlCompletionType", unique: false),
			new DmoIndex("lmlRoundedStartTime", unique: false),
			new DmoIndex("lmlRoundedEndTime", unique: false),
			new DmoIndex("lmlActualStartTime", unique: false),
			new DmoIndex("lmlActive", unique: false),
			new DmoIndex("lmlSuspended", unique: false),
			new DmoIndex("lmlTransferredToPayroll", unique: false),
			new DmoIndex("lmlProjectID", unique: false),
			new DmoIndex("lmlProjectAreaID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
