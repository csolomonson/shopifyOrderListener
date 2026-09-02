using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WebGearLog to support unicode", "2013-10-17")]
public class v810RebuildWebGearLog
{
	public v810RebuildWebGearLog(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WebGearLog", new DmoField[12]
		{
			new DmoField("wglWebGearLogID", "identity", 4, 0, nullable: false),
			new DmoField("wglSessionID", "nvarchar", 24, 0, nullable: false),
			new DmoField("wglWebGearUserID", "nvarchar", 50, 0, nullable: false),
			new DmoField("wglActvityTimestamp", "datetime", 14, 0, nullable: true),
			new DmoField("wglAnonymousUser", "bit", 1, 0, nullable: false),
			new DmoField("wglSecureConnection", "bit", 1, 0, nullable: false),
			new DmoField("wglWebSimpleURL", "nvarchar", 50, 0, nullable: false),
			new DmoField("wglWebRequestInfo", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wglReferrer", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wglResultCode", "int", 4, 0, nullable: false),
			new DmoField("wglResult", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("wglBrowserIPAddress", "nvarchar", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("WGLWEBGEARLOGID", unique: true),
			new DmoIndex("wglSessionID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
