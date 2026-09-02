using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert TaxCodePlants to support unicode", "2013-10-17")]
public class v810RebuildTaxCodePlants
{
	public v810RebuildTaxCodePlants(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodePlants", new DmoField[6]
		{
			new DmoField("xtpTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xtpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xtpAccrualGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("xtpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xtpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xtpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XTPTAXCODEID,XTPPLANTID", unique: true),
			new DmoIndex("XTPUNIQUEID", unique: true),
			new DmoIndex("xtpTaxCodeID", unique: false),
			new DmoIndex("xtpPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
