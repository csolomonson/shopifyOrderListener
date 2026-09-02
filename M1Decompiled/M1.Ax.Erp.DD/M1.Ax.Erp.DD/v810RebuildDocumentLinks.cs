using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DocumentLinks to support unicode", "2013-10-17")]
public class v810RebuildDocumentLinks
{
	public v810RebuildDocumentLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DocumentLinks", new DmoField[12]
		{
			new DmoField("xalDocumentLinkID", "int", 9, 0, nullable: false),
			new DmoField("xalFileName", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xalDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xalPrintDefault", "bit", 1, 0, nullable: false),
			new DmoField("xalEmailDefault", "bit", 1, 0, nullable: false),
			new DmoField("xalReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("xalType", "nvarchar", 5, 0, nullable: false),
			new DmoField("xalAddedByUserID", "nvarchar", 20, 0, nullable: false),
			new DmoField("xalAddedDate", "date", 14, 0, nullable: true),
			new DmoField("xalCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xalCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xalUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("XALDOCUMENTLINKID", unique: true),
			new DmoIndex("XALUNIQUEID", unique: true),
			new DmoIndex("xalType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
