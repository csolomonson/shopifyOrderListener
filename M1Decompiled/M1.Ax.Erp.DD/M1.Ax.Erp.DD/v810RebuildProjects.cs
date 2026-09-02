using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Projects to support unicode", "2013-10-17")]
public class v810RebuildProjects
{
	public v810RebuildProjects(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Projects", new DmoField[17]
		{
			new DmoField("prpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("prpOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("prpLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prpContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prpProjectDate", "date", 14, 0, nullable: true),
			new DmoField("prpProjectManagerEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("prpDueDate", "date", 14, 0, nullable: true),
			new DmoField("prpProjectTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("prpShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("prpLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("prpLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("prpClosed", "bit", 1, 0, nullable: false),
			new DmoField("prpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("prpStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("prpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("prpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("prpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("PRPPROJECTID", unique: true),
			new DmoIndex("PRPUNIQUEID", unique: true),
			new DmoIndex("prpOrganizationID", unique: false),
			new DmoIndex("prpLocationID", unique: false),
			new DmoIndex("prpContactID", unique: false),
			new DmoIndex("prpProjectManagerEmployeeID", unique: false),
			new DmoIndex("prpProjectTypeID", unique: false),
			new DmoIndex("prpClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
