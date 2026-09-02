using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseTransferComponents to support unicode", "2013-10-17")]
public class v810RebuildWarehouseTransferComponents
{
	public v810RebuildWarehouseTransferComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseTransferComponents", new DmoField[23]
		{
			new DmoField("mwoWarehouseTransferID", "nvarchar", 10, 0, nullable: false),
			new DmoField("mwoWarehouseTransferLineID", "smallint", 4, 0, nullable: false),
			new DmoField("mwoWarehouseTransComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("mwoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("mwoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("mwoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("mwoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("mwoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("mwoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("mwoShipQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("mwoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("mwoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("mwoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("mwoReceivedQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("mwoReceivedComplete", "bit", 1, 0, nullable: false),
			new DmoField("mwoClosed", "bit", 1, 0, nullable: false),
			new DmoField("mwoWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("mwoWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("mwoWarehouseReqComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("mwoShippedComplete", "bit", 1, 0, nullable: false),
			new DmoField("mwoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("mwoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("mwoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[13]
		{
			new DmoIndex("MWOWAREHOUSETRANSFERID,MWOWAREHOUSETRANSFERLINEID,MWOWAREHOUSETRANSCOMPONENTID", unique: true),
			new DmoIndex("MWOUNIQUEID", unique: true),
			new DmoIndex("mwoWarehouseTransferID", unique: false),
			new DmoIndex("mwoWarehouseTransferLineID", unique: false),
			new DmoIndex("mwoWarehouseTransComponentID", unique: false),
			new DmoIndex("mwoPartID", unique: false),
			new DmoIndex("mwoPartRevisionID", unique: false),
			new DmoIndex("mwoReceivedComplete", unique: false),
			new DmoIndex("mwoClosed", unique: false),
			new DmoIndex("mwoWarehouseRequisitionID", unique: false),
			new DmoIndex("mwoWarehouseRequisitionLineID", unique: false),
			new DmoIndex("mwoWarehouseReqComponentID", unique: false),
			new DmoIndex("mwoShippedComplete", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
