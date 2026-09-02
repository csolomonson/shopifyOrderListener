using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartRules to support unicode", "2013-10-17")]
public class v810RebuildPartRules
{
	public v810RebuildPartRules(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartRules", new DmoField[12]
		{
			new DmoField("pcrMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("pcrMethodRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("pcrMethodAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("pcrMethodMaterialID", "int", 5, 0, nullable: false),
			new DmoField("pcrMethodOperationID", "int", 5, 0, nullable: false),
			new DmoField("pcrMethodType", "tinyint", 1, 0, nullable: false),
			new DmoField("pcrField", "nvarchar", 30, 0, nullable: false),
			new DmoField("pcrCode", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("pcrProcessSequence", "smallint", 3, 0, nullable: false),
			new DmoField("pcrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pcrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pcrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("pcrUniqueID", unique: true),
			new DmoIndex("pcrMethodID", unique: false),
			new DmoIndex("pcrMethodRevisionID", unique: false),
			new DmoIndex("pcrMethodAssemblyID", unique: false),
			new DmoIndex("pcrMethodMaterialID", unique: false),
			new DmoIndex("pcrMethodOperationID", unique: false),
			new DmoIndex("pcrMethodType", unique: false),
			new DmoIndex("pcrField", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
