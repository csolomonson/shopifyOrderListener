using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FreightReferences to support unicode", "2013-10-17")]
public class v810RebuildFreightReferences
{
	public v810RebuildFreightReferences(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightReferences", new DmoField[5]
		{
			new DmoField("frcFreightReferenceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("frcUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("frcQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("frcShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("frcFreightShipmentID", "nvarchar", 10, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("FRCFREIGHTREFERENCEID", unique: true),
			new DmoIndex("FRCUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
