using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DMRShipmentLines to support unicode", "2013-10-17")]
public class v810RebuildDMRShipmentLines
{
	public v810RebuildDMRShipmentLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentLines", new DmoField[23]
		{
			new DmoField("dslDMRShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dslDMRShipmentLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dslPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("dslPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dslPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dslPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dslQuantityShipped", "numeric", 15, 5, nullable: false),
			new DmoField("dslInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("dslUnitPrice", "numeric", 15, 5, nullable: false),
			new DmoField("dslShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("dslProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dslProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("dslUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("dslDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("dslDMRClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dslDMRClaimLineID", "smallint", 4, 0, nullable: false),
			new DmoField("dslPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dslPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dslUnitPriceForeign", "numeric", 15, 5, nullable: false),
			new DmoField("dslClosed", "bit", 1, 0, nullable: false),
			new DmoField("dslCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("dslCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dslUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("DSLDMRSHIPMENTID,DSLDMRSHIPMENTLINEID", unique: true),
			new DmoIndex("DSLUNIQUEID", unique: true),
			new DmoIndex("dslDMRShipmentID", unique: false),
			new DmoIndex("dslDMRShipmentLineID", unique: false),
			new DmoIndex("dslPartID", unique: false),
			new DmoIndex("dslPartRevisionID", unique: false),
			new DmoIndex("dslShippedComplete", unique: false),
			new DmoIndex("dslProjectID", unique: false),
			new DmoIndex("dslProjectAreaID", unique: false),
			new DmoIndex("dslDMRClaimID", unique: false),
			new DmoIndex("dslDMRClaimLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
