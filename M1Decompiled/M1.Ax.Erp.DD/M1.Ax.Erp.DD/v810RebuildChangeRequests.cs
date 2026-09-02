using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ChangeRequests to support unicode", "2013-10-17")]
public class v810RebuildChangeRequests
{
	public v810RebuildChangeRequests(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ChangeRequests", new DmoField[30]
		{
			new DmoField("chpChangeRequestID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpChangeRequestTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("chpJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("chpPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("chpPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("chpShortDescription", "nvarchar", 70, 0, nullable: false),
			new DmoField("chpLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("chpLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("chpPriorityID", "tinyint", 2, 0, nullable: false),
			new DmoField("chpOpenedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpOpenedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpAuthorizedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpAuthorizedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpAssignedToEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpAssignedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpDueDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpEstimatedHours", "numeric", 8, 2, nullable: false),
			new DmoField("chpActualHours", "numeric", 8, 2, nullable: false),
			new DmoField("chpClosedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpClosedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpClosedReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("chpResolvedPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("chpResolvedPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("chpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("chpProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("chpNonConformanceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("chpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("chpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("chpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[16]
		{
			new DmoIndex("CHPCHANGEREQUESTID", unique: true),
			new DmoIndex("CHPUNIQUEID", unique: true),
			new DmoIndex("chpChangeRequestTypeID", unique: false),
			new DmoIndex("chpJobID", unique: false),
			new DmoIndex("chpPartID", unique: false),
			new DmoIndex("chpPartRevisionID", unique: false),
			new DmoIndex("chpPriorityID", unique: false),
			new DmoIndex("chpAuthorizedByEmployeeID", unique: false),
			new DmoIndex("chpDueDate", unique: false),
			new DmoIndex("chpClosedReasonID", unique: false),
			new DmoIndex("chpResolvedPartID", unique: false),
			new DmoIndex("chpResolvedPartRevisionID", unique: false),
			new DmoIndex("chpProjectID", unique: false),
			new DmoIndex("chpStatus", unique: false),
			new DmoIndex("chpProjectAreaID", unique: false),
			new DmoIndex("chpNonConformanceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
