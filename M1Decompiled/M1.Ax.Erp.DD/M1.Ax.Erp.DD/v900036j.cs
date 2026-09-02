using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Add fields to ShipmentPackageDetails table", "2015-05-19")]
public class v900036j
{
	public v900036j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ShipmentPackageDetails"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentPackageDetails", new DmoField[14]
			{
				new DmoField("spdShipmentID", "nvarchar", 10, 0, nullable: false),
				new DmoField("spdShipmentLineID", "smallint", 4, 0, nullable: false),
				new DmoField("spdShipmentPackageID", "int", 5, 0, nullable: false),
				new DmoField("spdPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("spdPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("spdQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("spdWeight", "numeric", 15, 5, nullable: false),
				new DmoField("spdTotalPriceBase", "money", 12, 2, nullable: false),
				new DmoField("spdTotalPriceForeign", "money", 12, 2, nullable: false),
				new DmoField("spdCommodityDescription", "nvarchar", 35, 0, nullable: false),
				new DmoField("spdCountryOfManufacture", "nvarchar", 2, 0, nullable: false),
				new DmoField("spdCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("spdCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("spdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("SPDSHIPMENTID,SPDSHIPMENTLINEID,SPDSHIPMENTPACKAGEID", unique: true),
				new DmoIndex("SPDUNIQUEID", unique: true),
				new DmoIndex("spdShipmentID", unique: false),
				new DmoIndex("spdShipmentLineID", unique: false),
				new DmoIndex("spdShipmentPackageID", unique: false),
				new DmoIndex("spdPartRevisionID", unique: false)
			});
		}
	}
}
