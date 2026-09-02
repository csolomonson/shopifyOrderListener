using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert NonConformanceCodes to support unicode", "2013-10-17")]
public class v810RebuildNonConformanceCodes
{
	public v810RebuildNonConformanceCodes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "NonConformanceCodes", new DmoField[6]
		{
			new DmoField("qacNonConformanceCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qacDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qacNonConformanceCategoryID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qacCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qacCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qacUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("QACNONCONFORMANCECODEID", unique: true),
			new DmoIndex("QACUNIQUEID", unique: true),
			new DmoIndex("qacNonConformanceCategoryID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
