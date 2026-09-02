using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to RMAReceiptComponents table", "2014-12-15")]
public class v900014k
{
	public v900014k(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RMAReceiptComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAReceiptComponents", new DmoField[25]
			{
				new DmoField("rroRMAReceiptID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rroRMAReceiptLineID", "smallint", 4, 0, nullable: false),
				new DmoField("rroRMAReceiptComponentID", "int", 4, 0, nullable: false),
				new DmoField("rroPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("rroPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rroPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rroPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rroParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rroInspParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rroQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("rroAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rroQuantityReceived", "numeric", 15, 5, nullable: false),
				new DmoField("rroQuantityToInspect", "numeric", 15, 5, nullable: false),
				new DmoField("rroUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("rroDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("rroWeight", "numeric", 15, 5, nullable: false),
				new DmoField("rroReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("rroInspectionComplete", "bit", 1, 0, nullable: false),
				new DmoField("rroClosed", "bit", 1, 0, nullable: false),
				new DmoField("rroRMAClaimID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rroRMAClaimLineID", "smallint", 4, 0, nullable: false),
				new DmoField("rroRMAClaimComponentID", "int", 4, 0, nullable: false),
				new DmoField("rroCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("rroCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rroUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[4]
			{
				new DmoIndex("rroRMAReceiptID,rroRMAReceiptLineID,rroRMAReceiptComponentID", unique: true),
				new DmoIndex("rroUniqueID", unique: true),
				new DmoIndex("rroReceivedComplete", unique: false),
				new DmoIndex("rroInspectionComplete", unique: false)
			});
		}
	}
}
