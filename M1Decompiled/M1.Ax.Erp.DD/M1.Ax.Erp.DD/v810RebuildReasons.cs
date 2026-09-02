using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Reasons to support unicode", "2013-10-17")]
public class v810RebuildReasons
{
	public v810RebuildReasons(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Reasons", new DmoField[7]
		{
			new DmoField("xarReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xarReasonType", "nvarchar", 1, 0, nullable: false),
			new DmoField("xarDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xarReasonGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xarCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xarCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xarUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("XARREASONID", unique: true),
			new DmoIndex("XARUNIQUEID", unique: true),
			new DmoIndex("xarReasonType", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
