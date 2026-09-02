using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CustomerGroups to support unicode", "2013-10-17")]
public class v810RebuildCustomerGroups
{
	public v810RebuildCustomerGroups(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CustomerGroups", new DmoField[5]
		{
			new DmoField("cmuCustomerGroupID", "nvarchar", 5, 0, nullable: false),
			new DmoField("cmuDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("cmuCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("cmuCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("cmuUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CMUCUSTOMERGROUPID", unique: true),
			new DmoIndex("CMUUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
