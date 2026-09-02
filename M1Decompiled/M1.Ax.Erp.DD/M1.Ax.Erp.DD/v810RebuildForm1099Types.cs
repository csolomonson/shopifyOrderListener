using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form1099Types to support unicode", "2013-10-17")]
public class v810RebuildForm1099Types
{
	public v810RebuildForm1099Types(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Types", new DmoField[19]
		{
			new DmoField("apaForm1099TypeID", "smallint", 4, 0, nullable: false),
			new DmoField("apaDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("apaStartDate", "date", 14, 0, nullable: true),
			new DmoField("apaInactive", "bit", 1, 0, nullable: false),
			new DmoField("apaEndDate", "date", 14, 0, nullable: true),
			new DmoField("apaBox1", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox2", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox3", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox5", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox6", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox8", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox9", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox10", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox11", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox13", "numeric", 10, 2, nullable: false),
			new DmoField("apaBox1B", "numeric", 10, 2, nullable: false),
			new DmoField("apaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apaCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("apaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("APAFORM1099TYPEID", unique: true),
			new DmoIndex("APAUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
