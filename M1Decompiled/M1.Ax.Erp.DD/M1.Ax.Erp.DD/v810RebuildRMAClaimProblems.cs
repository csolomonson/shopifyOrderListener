using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RMAClaimProblems to support unicode", "2013-10-17")]
public class v810RebuildRMAClaimProblems
{
	public v810RebuildRMAClaimProblems(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RMAClaimProblems"))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimProblems", new DmoField[24]
			{
				new DmoField("rarRMAClaimID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rarRMAClaimLineID", "smallint", 4, 0, nullable: false),
				new DmoField("rarRMAClaimProblemID", "smallint", 4, 0, nullable: false),
				new DmoField("rarNonConformanceCategoryID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rarNonConformanceCodeID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rarNonConformanceCauseID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rarRepairType", "tinyint", 1, 0, nullable: false),
				new DmoField("rarRepairsComplete", "bit", 1, 0, nullable: false),
				new DmoField("rarCorrectiveActionCategoryID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rarCorrectiveActionCodeID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rarRepairedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rarHoursAllowed", "numeric", 8, 2, nullable: false),
				new DmoField("rarHoursRequested", "numeric", 8, 2, nullable: false),
				new DmoField("rarActualHours", "numeric", 8, 2, nullable: false),
				new DmoField("rarRepairedByOrganizationID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rarSubcontractAmount", "money", 12, 2, nullable: false),
				new DmoField("rarNonConformanceText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rarNonConformanceRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rarCorrectiveActionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rarCorrectiveActionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("rarSubcontractAmtForeign", "money", 12, 2, nullable: false),
				new DmoField("rarCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("rarCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rarUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[11]
			{
				new DmoIndex("RARRMACLAIMID,RARRMACLAIMLINEID,RARRMACLAIMPROBLEMID", unique: true),
				new DmoIndex("RARUNIQUEID", unique: true),
				new DmoIndex("rarRMAClaimID", unique: false),
				new DmoIndex("rarRMAClaimLineID", unique: false),
				new DmoIndex("rarRMAClaimProblemID", unique: false),
				new DmoIndex("rarNonConformanceCategoryID", unique: false),
				new DmoIndex("rarNonConformanceCodeID", unique: false),
				new DmoIndex("rarNonConformanceCauseID", unique: false),
				new DmoIndex("rarCorrectiveActionCategoryID", unique: false),
				new DmoIndex("rarCorrectiveActionCodeID", unique: false),
				new DmoIndex("rarRepairedByOrganizationID", unique: false)
			}, mergeCustomFields: true, disableTriggers: true);
		}
	}
}
