using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartClassPlants to support unicode", "2013-10-17")]
public class v810RebuildPartClassPlants
{
	public v810RebuildPartClassPlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartClassPlants", new DmoField[6]
		{
			new DmoField("imfPartClassID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imfPartClassPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("imfInventoryGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("imfCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("imfCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("imfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("IMFPARTCLASSID,IMFPARTCLASSPLANTID", unique: true),
			new DmoIndex("IMFUNIQUEID", unique: true),
			new DmoIndex("imfPartClassID", unique: false),
			new DmoIndex("imfPartClassPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
