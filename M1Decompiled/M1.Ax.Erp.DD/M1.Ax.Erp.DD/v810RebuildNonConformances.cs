using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert NonConformances to support unicode", "2013-10-17")]
public class v810RebuildNonConformances
{
	public v810RebuildNonConformances(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformances", new DmoField[35]
		{
			new DmoField("qarNonConformanceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qarInspectionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qarInspectionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qarNonConformanceCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarNonConformanceCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarNonConformanceCauseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarCorrectiveActionType", "tinyint", 1, 0, nullable: false),
			new DmoField("qarCorrectiveActionComplete", "bit", 1, 0, nullable: false),
			new DmoField("qarCorrectiveActionCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarCorrectiveActionCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarCorrectiveActionDate", "datetime", 14, 0, nullable: true),
			new DmoField("qarHoursAllowed", "numeric", 8, 2, nullable: false),
			new DmoField("qarHoursRequested", "numeric", 8, 2, nullable: false),
			new DmoField("qarActualHours", "numeric", 8, 2, nullable: false),
			new DmoField("qarRepairedByOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qarSubcontractAmount", "money", 12, 2, nullable: false),
			new DmoField("qarNonConformanceText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qarCorrectiveActionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qarNonConformanceRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qarCorrectiveActionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qarQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("qarPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qarPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qarPartWareHouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qarPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qarPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qarUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("qarJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("qarJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("qarJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("qarJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("qarReportedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qarCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qarCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qarUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[14]
		{
			new DmoIndex("QARNONCONFORMANCEID", unique: true),
			new DmoIndex("QARUNIQUEID", unique: true),
			new DmoIndex("qarInspectionID", unique: false),
			new DmoIndex("qarInspectionLineID", unique: false),
			new DmoIndex("qarNonConformanceCategoryID", unique: false),
			new DmoIndex("qarNonConformanceCodeID", unique: false),
			new DmoIndex("qarNonConformanceCauseID", unique: false),
			new DmoIndex("qarCorrectiveActionCategoryID", unique: false),
			new DmoIndex("qarCorrectiveActionCodeID", unique: false),
			new DmoIndex("qarRepairedByOrganizationID", unique: false),
			new DmoIndex("qarPartID", unique: false),
			new DmoIndex("qarPartRevisionID", unique: false),
			new DmoIndex("qarPartWareHouseLocationID", unique: false),
			new DmoIndex("qarPartBinID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
