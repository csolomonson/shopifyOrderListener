using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AgingBuckets to support unicode", "2013-10-17")]
public class v810RebuildAgingBuckets
{
	public v810RebuildAgingBuckets(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AgingBuckets", new DmoField[16]
		{
			new DmoField("xaaAgingBucketID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xaaDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xaaCalculationType", "tinyint", 1, 0, nullable: false),
			new DmoField("xaaBucket1Description", "nvarchar", 10, 0, nullable: false),
			new DmoField("xaaBucket1DaysOver", "int", 5, 0, nullable: false),
			new DmoField("xaaBucket2Description", "nvarchar", 10, 0, nullable: false),
			new DmoField("xaaBucket2DaysOver", "int", 5, 0, nullable: false),
			new DmoField("xaaBucket3Description", "nvarchar", 10, 0, nullable: false),
			new DmoField("xaaBucket3DaysOver", "int", 5, 0, nullable: false),
			new DmoField("xaaBucket4Description", "nvarchar", 10, 0, nullable: false),
			new DmoField("xaaBucket4DaysOver", "int", 5, 0, nullable: false),
			new DmoField("xaaBucket5Description", "nvarchar", 10, 0, nullable: false),
			new DmoField("xaaBucket5DaysOver", "int", 5, 0, nullable: false),
			new DmoField("xaaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xaaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xaaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("XAAAGINGBUCKETID", unique: true),
			new DmoIndex("XAAUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
