using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShipmentFreightReferences to support unicode", "2013-10-17")]
public class v810RebuildShipmentFreightReferences
{
	public v810RebuildShipmentFreightReferences(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentFreightReferences", new DmoField[6]
		{
			new DmoField("smrShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smrShipmentFreightReferenceID", "smallint", 4, 0, nullable: false),
			new DmoField("smrFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smrCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("smrCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("smrUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("SMRSHIPMENTID,SMRSHIPMENTFREIGHTREFERENCEID", unique: true),
			new DmoIndex("SMRUNIQUEID", unique: true),
			new DmoIndex("smrShipmentID", unique: false),
			new DmoIndex("smrShipmentFreightReferenceID", unique: false),
			new DmoIndex("smrFreightShipmentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
