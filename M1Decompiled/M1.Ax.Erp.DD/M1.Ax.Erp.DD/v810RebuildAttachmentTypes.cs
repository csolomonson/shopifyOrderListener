using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AttachmentTypes to support unicode", "2013-10-17")]
public class v810RebuildAttachmentTypes
{
	public v810RebuildAttachmentTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AttachmentTypes", new DmoField[7]
		{
			new DmoField("cmtAttachmentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmtRequiresServiceContract", "bit", 1, 0, nullable: false),
			new DmoField("cmtRequiresLogin", "bit", 1, 0, nullable: false),
			new DmoField("cmtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMTATTACHMENTTYPEID", unique: true),
			new DmoIndex("CMTUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
