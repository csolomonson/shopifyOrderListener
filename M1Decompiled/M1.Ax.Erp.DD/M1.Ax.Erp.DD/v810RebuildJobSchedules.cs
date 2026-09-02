using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobSchedules to support unicode", "2013-10-17")]
public class v810RebuildJobSchedules
{
	public v810RebuildJobSchedules(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobSchedules", new DmoField[36]
		{
			new DmoField("jmsJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmsJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("jmsJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("jmsJobScenarioID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmsQueueStartDate", "date", 14, 0, nullable: true),
			new DmoField("jmsQueueStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmsQueueStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("jmsStartDate", "date", 14, 0, nullable: true),
			new DmoField("jmsStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmsStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("jmsProdStartDate", "date", 14, 0, nullable: true),
			new DmoField("jmsProdStartHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmsProdStartTime", "datetime", 14, 0, nullable: true),
			new DmoField("jmsMoveDueDate", "date", 14, 0, nullable: true),
			new DmoField("jmsMoveDueHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmsMoveDueTime", "datetime", 14, 0, nullable: true),
			new DmoField("jmsDueDate", "date", 14, 0, nullable: true),
			new DmoField("jmsDueHour", "numeric", 5, 2, nullable: false),
			new DmoField("jmsDueTime", "datetime", 14, 0, nullable: true),
			new DmoField("jmsWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("jmsMachineType", "tinyint", 1, 0, nullable: false),
			new DmoField("jmsWorkCenterMachineID", "smallint", 3, 0, nullable: false),
			new DmoField("jmsCrewSize", "smallint", 3, 0, nullable: false),
			new DmoField("jmsQueueTime", "numeric", 6, 2, nullable: false),
			new DmoField("jmsEstimatedSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("jmsEstimatedProductionHours", "numeric", 8, 2, nullable: false),
			new DmoField("jmsMoveTime", "numeric", 6, 2, nullable: false),
			new DmoField("jmsFiniteTolerance", "numeric", 5, 2, nullable: false),
			new DmoField("jmsActualSetupHours", "numeric", 8, 2, nullable: false),
			new DmoField("jmsActualProductionHours", "numeric", 8, 2, nullable: false),
			new DmoField("jmsOverlap", "tinyint", 1, 0, nullable: false),
			new DmoField("jmsChanged", "bit", 1, 0, nullable: false),
			new DmoField("jmsExchangeID", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("jmsCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmsCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("JMSJOBID,JMSJOBASSEMBLYID,JMSJOBOPERATIONID,JMSWORKCENTERMACHINEID,JMSJOBSCENARIOID", unique: true),
			new DmoIndex("JMSUNIQUEID", unique: true),
			new DmoIndex("jmsJobID", unique: false),
			new DmoIndex("jmsJobAssemblyID", unique: false),
			new DmoIndex("jmsJobOperationID", unique: false),
			new DmoIndex("jmsJobScenarioID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
