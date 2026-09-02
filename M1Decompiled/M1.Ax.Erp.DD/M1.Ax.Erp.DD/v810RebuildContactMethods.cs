using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ContactMethods to support unicode", "2013-10-17")]
public class v810RebuildContactMethods
{
	public v810RebuildContactMethods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ContactMethods", new DmoField[5]
		{
			new DmoField("kbcContactMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("kbcDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("KBCCONTACTMETHODID", unique: true),
			new DmoIndex("KBCUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
