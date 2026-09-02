using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RMAReceiptLines to support unicode", "2013-10-17")]
public class v810RebuildRMAReceiptLines
{
	public v810RebuildRMAReceiptLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptLines", new DmoField[29]
		{
			new DmoField("rrlRMAReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrlRMAReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rrlRMAClaimID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrlRMAClaimLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rrlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rrlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rrlOrgPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rrlOrgPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rrlDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rrlPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rrlPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rrlInspectionQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("rrlInspectionUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("rrlConversionFactor", "numeric", 14, 8, nullable: false),
			new DmoField("rrlInventoryQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("rrlInventoryUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("rrlReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("rrlHeatLot", "nvarchar", 50, 0, nullable: false),
			new DmoField("rrlClosed", "bit", 1, 0, nullable: false),
			new DmoField("rrlReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("rrlInvoicedComplete", "bit", 1, 0, nullable: false),
			new DmoField("rrlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rrlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rrlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rrlQualityRegisterID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rrlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rrlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rrlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("RRLRMARECEIPTID,RRLRMARECEIPTLINEID", unique: true),
			new DmoIndex("RRLUNIQUEID", unique: true),
			new DmoIndex("rrlRMAReceiptID", unique: false),
			new DmoIndex("rrlRMAReceiptLineID", unique: false),
			new DmoIndex("rrlRMAClaimID", unique: false),
			new DmoIndex("rrlRMAClaimLineID", unique: false),
			new DmoIndex("rrlPartID", unique: false),
			new DmoIndex("rrlPartRevisionID", unique: false),
			new DmoIndex("rrlOrgPartID", unique: false),
			new DmoIndex("rrlProjectID", unique: false),
			new DmoIndex("rrlProjectAreaID", unique: false),
			new DmoIndex("rrlQualityRegisterID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
