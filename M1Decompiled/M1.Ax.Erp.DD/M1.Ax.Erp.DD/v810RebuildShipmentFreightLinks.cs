using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShipmentFreightLinks to support unicode", "2013-10-17")]
public class v810RebuildShipmentFreightLinks
{
	public v810RebuildShipmentFreightLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentFreightLinks", new DmoField[13]
		{
			new DmoField("smxFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smxFreightPackageID", "smallint", 4, 0, nullable: false),
			new DmoField("smxShipmentFreightLinkID", "smallint", 4, 0, nullable: false),
			new DmoField("smxShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("smxShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("smxPackagePartialWeight", "numeric", 6, 2, nullable: false),
			new DmoField("smxFreightCharges", "numeric", 9, 2, nullable: false),
			new DmoField("smxPackagePartialCount", "numeric", 11, 2, nullable: false),
			new DmoField("smxLinkPctCharge", "numeric", 6, 2, nullable: false),
			new DmoField("smxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("smxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("smxClosed", "bit", 1, 0, nullable: false),
			new DmoField("smxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("SMXFREIGHTSHIPMENTID,SMXFREIGHTPACKAGEID,SMXSHIPMENTFREIGHTLINKID", unique: true),
			new DmoIndex("SMXUNIQUEID", unique: true),
			new DmoIndex("smxFreightShipmentID", unique: false),
			new DmoIndex("smxFreightPackageID", unique: false),
			new DmoIndex("smxShipmentFreightLinkID", unique: false),
			new DmoIndex("smxShipmentID", unique: false),
			new DmoIndex("smxShipmentLineID", unique: false),
			new DmoIndex("smxClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
