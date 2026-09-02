using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RFQs to support unicode", "2013-10-17")]
public class v810RebuildRFQs
{
	public v810RebuildRFQs(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RFQs", new DmoField[13]
		{
			new DmoField("rqpRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rqpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rqpRFQDate", "date", 14, 0, nullable: true),
			new DmoField("rqpBuyerEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqpDueDate", "date", 14, 0, nullable: true),
			new DmoField("rqpReadyToPrint", "bit", 1, 0, nullable: false),
			new DmoField("rqpStandardMessageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqpClosed", "bit", 1, 0, nullable: false),
			new DmoField("rqpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("rqpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rqpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("RQPRFQID", unique: true),
			new DmoIndex("RQPUNIQUEID", unique: true),
			new DmoIndex("rqpPlantDepartmentID", unique: false),
			new DmoIndex("rqpPlantID", unique: false),
			new DmoIndex("rqpReadyToPrint", unique: false),
			new DmoIndex("rqpClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
