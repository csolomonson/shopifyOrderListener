using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.074", "Add fields to MfgReceiptComponents table", "2015-08-14")]
public class v900074b
{
	public v900074b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MfgReceiptComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MfgReceiptComponents", new DmoField[24]
			{
				new DmoField("rmnMfgReceiptID", "nvarchar", 10, 0, nullable: false),
				new DmoField("rmnMfgReceiptComponentID", "int", 4, 0, nullable: false),
				new DmoField("rmnPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("rmnPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rmnPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("rmnPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("rmnInvParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmnJobMatParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmnQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("rmnAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmnInvReceiptQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmnJobMatReceiptQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("rmnReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("rmnUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("rmnDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("rmnWeight", "numeric", 15, 5, nullable: false),
				new DmoField("rmnJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("rmnJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("rmnJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("rmnJobMaterialComponentID", "int", 5, 0, nullable: false),
				new DmoField("rmnPosted", "bit", 1, 0, nullable: false),
				new DmoField("rmnCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("rmnCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("rmnUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("rmnMfgReceiptID,rmnMfgReceiptComponentID", unique: true),
				new DmoIndex("rmnUniqueID", unique: true),
				new DmoIndex("rmnReceivedComplete", unique: false)
			});
		}
	}
}
