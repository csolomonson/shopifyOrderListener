using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert NonConformanceCategories to support unicode", "2013-10-17")]
public class v810RebuildNonConformanceCategories
{
	public v810RebuildNonConformanceCategories(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformanceCategories", new DmoField[5]
		{
			new DmoField("qagNonConformanceCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qagDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qagCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qagCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qagUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("QAGNONCONFORMANCECATEGORYID", unique: true),
			new DmoIndex("QAGUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
