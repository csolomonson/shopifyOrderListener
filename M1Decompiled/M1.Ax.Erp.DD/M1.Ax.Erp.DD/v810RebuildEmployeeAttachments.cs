using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeAttachments to support unicode", "2013-10-17")]
public class v810RebuildEmployeeAttachments
{
	public v810RebuildEmployeeAttachments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeAttachments", new DmoField[12]
		{
			new DmoField("lmaEmployeeAttachmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmaEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmaAttachmentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmaDate", "date", 14, 0, nullable: true),
			new DmoField("lmaShortDescription", "nvarchar", 70, 0, nullable: false),
			new DmoField("lmaLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmaLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmaFileLocation", "nvarchar", byte.MaxValue, 0, nullable: false),
			new DmoField("lmaFileName", "nvarchar", byte.MaxValue, 0, nullable: false),
			new DmoField("lmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("LMAEMPLOYEEATTACHMENTID", unique: true),
			new DmoIndex("LMAUNIQUEID", unique: true),
			new DmoIndex("lmaEmployeeID", unique: false),
			new DmoIndex("lmaAttachmentTypeID", unique: false),
			new DmoIndex("lmaDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
