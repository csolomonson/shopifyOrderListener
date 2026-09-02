using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert NonConformanceCauses to support unicode", "2013-10-17")]
public class v810RebuildNonConformanceCauses
{
	public v810RebuildNonConformanceCauses(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformanceCauses", new DmoField[5]
		{
			new DmoField("qauNonConformanceCauseID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qauDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qauCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qauCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qauUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("QAUNONCONFORMANCECAUSEID", unique: true),
			new DmoIndex("QAUUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
