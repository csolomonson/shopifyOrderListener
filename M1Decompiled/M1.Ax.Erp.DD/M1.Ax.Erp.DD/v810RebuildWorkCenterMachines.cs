using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WorkCenterMachines to support unicode", "2013-10-17")]
public class v810RebuildWorkCenterMachines
{
	public v810RebuildWorkCenterMachines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenterMachines", new DmoField[4]
		{
			new DmoField("xaqWorkCenterID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xaqWorkCenterMachineID", "smallint", 3, 0, nullable: false),
			new DmoField("xaqDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XAQWORKCENTERID,XAQWORKCENTERMACHINEID", unique: true),
			new DmoIndex("XAQUNIQUEID", unique: true),
			new DmoIndex("xaqWorkCenterID", unique: false),
			new DmoIndex("xaqWorkCenterMachineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
