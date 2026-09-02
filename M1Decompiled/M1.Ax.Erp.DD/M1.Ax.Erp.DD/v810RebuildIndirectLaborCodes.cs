using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IndirectLaborCodes to support unicode", "2013-10-17")]
public class v810RebuildIndirectLaborCodes
{
	public v810RebuildIndirectLaborCodes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IndirectLaborCodes", new DmoField[8]
		{
			new DmoField("lmiIndirectLaborID", "nvarchar", 5, 0, nullable: false),
			new DmoField("lmiDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmiInactive", "bit", 1, 0, nullable: false),
			new DmoField("lmiInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("lmiIndirectLaborType", "tinyint", 1, 0, nullable: false),
			new DmoField("lmiCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmiCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmiUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("LMIINDIRECTLABORID", unique: true),
			new DmoIndex("LMIUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
