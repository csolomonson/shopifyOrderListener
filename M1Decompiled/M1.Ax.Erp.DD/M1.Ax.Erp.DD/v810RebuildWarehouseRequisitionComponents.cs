using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert WarehouseRequisitionComponents to support unicode", "2013-10-17")]
public class v810RebuildWarehouseRequisitionComponents
{
	public v810RebuildWarehouseRequisitionComponents(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WarehouseRequisitionComponents", new DmoField[19]
		{
			new DmoField("wqoWarehouseRequisitionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("wqoWarehouseRequisitionLineID", "smallint", 4, 0, nullable: false),
			new DmoField("wqoWarehouseReqComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("wqoPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("wqoPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wqoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("wqoPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("wqoQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("wqoAdditionalQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("wqoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("wqoDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("wqoWeight", "numeric", 15, 5, nullable: false),
			new DmoField("wqoQuantityRequested", "numeric", 15, 5, nullable: false),
			new DmoField("wqoTransferredComplete", "bit", 1, 0, nullable: false),
			new DmoField("wqoClosed", "bit", 1, 0, nullable: false),
			new DmoField("wqoQuantityTransferred", "numeric", 15, 5, nullable: false),
			new DmoField("wqoCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("wqoCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("wqoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("WQOWAREHOUSEREQUISITIONID,WQOWAREHOUSEREQUISITIONLINEID,WQOWAREHOUSEREQCOMPONENTID", unique: true),
			new DmoIndex("WQOUNIQUEID", unique: true),
			new DmoIndex("wqoWarehouseRequisitionID", unique: false),
			new DmoIndex("wqoWarehouseRequisitionLineID", unique: false),
			new DmoIndex("wqoWarehouseReqComponentID", unique: false),
			new DmoIndex("wqoPartID", unique: false),
			new DmoIndex("wqoPartRevisionID", unique: false),
			new DmoIndex("wqoTransferredComplete", unique: false),
			new DmoIndex("wqoClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
