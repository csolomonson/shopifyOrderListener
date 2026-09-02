using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FreightPackageLinks to support unicode", "2013-10-17")]
public class v810RebuildFreightPackageLinks
{
	public v810RebuildFreightPackageLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightPackageLinks", new DmoField[6]
		{
			new DmoField("fplFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fplFreightPackageID", "smallint", 4, 0, nullable: false),
			new DmoField("fplUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("fplFreightPackageLineID", "smallint", 4, 0, nullable: false),
			new DmoField("fplCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fplCreatedDate", "date", 14, 0, nullable: true)
		}, new DmoIndex[2]
		{
			new DmoIndex("FPLFREIGHTSHIPMENTID,FPLFREIGHTPACKAGEID,FPLFREIGHTPACKAGELINEID", unique: true),
			new DmoIndex("FPLUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
