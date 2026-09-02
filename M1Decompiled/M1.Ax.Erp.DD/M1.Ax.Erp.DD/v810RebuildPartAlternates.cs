using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartAlternates to support unicode", "2013-10-17")]
public class v810RebuildPartAlternates
{
	public v810RebuildPartAlternates(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartAlternates", new DmoField[8]
		{
			new DmoField("imePartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imePartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imeAlternatePartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("imeAlternatePartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("imeComment", "nvarchar", 70, 0, nullable: false),
			new DmoField("imeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imeCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("IMEPARTID,IMEPARTREVISIONID,IMEALTERNATEPARTID,IMEALTERNATEPARTREVISIONID", unique: true),
			new DmoIndex("IMEUNIQUEID", unique: true),
			new DmoIndex("imePartID", unique: false),
			new DmoIndex("imePartRevisionID", unique: false),
			new DmoIndex("imeAlternatePartID", unique: false),
			new DmoIndex("imeAlternatePartRevisionID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
