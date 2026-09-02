using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FormDefinitions to support unicode", "2013-10-17")]
public class v810RebuildFormDefinitions
{
	public v810RebuildFormDefinitions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FormDefinitions", new DmoField[6]
		{
			new DmoField("xaoFormID", "nvarchar", 75, 0, nullable: false),
			new DmoField("xaoControlName", "nvarchar", 70, 0, nullable: false),
			new DmoField("xaoClassID", "nvarchar", 35, 0, nullable: false),
			new DmoField("xaoType", "tinyint", 1, 0, nullable: false),
			new DmoField("xaoProperties", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xaoCode", "nvarchar(max)", 50, 0, nullable: true)
		}, new DmoIndex[3]
		{
			new DmoIndex("XAOFORMID,XAOCONTROLNAME", unique: true),
			new DmoIndex("xaoFormID", unique: false),
			new DmoIndex("xaoControlName", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
