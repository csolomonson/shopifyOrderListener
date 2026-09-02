using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LaserMaterialTypes to support unicode", "2013-10-17")]
public class v810RebuildLaserMaterialTypes
{
	public v810RebuildLaserMaterialTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LaserMaterialTypes", new DmoField[10]
		{
			new DmoField("ccmLaserMaterialTypeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("ccmDescription", "nvarchar", 30, 0, nullable: false),
			new DmoField("ccmThickness", "numeric", 11, 3, nullable: false),
			new DmoField("ccmLeadInOutFeed", "numeric", 11, 3, nullable: false),
			new DmoField("ccmExternalFeed", "numeric", 11, 3, nullable: false),
			new DmoField("ccmRate", "numeric", 11, 3, nullable: false),
			new DmoField("ccmPierceTime", "numeric", 12, 3, nullable: false),
			new DmoField("ccmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ccmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("ccmUniqueid", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("CCMLASERMATERIALTYPEID", unique: true),
			new DmoIndex("CCMUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
