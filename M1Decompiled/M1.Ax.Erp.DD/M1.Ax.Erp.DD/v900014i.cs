using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to RMAClaimComponents table", "2014-12-15")]
public class v900014i
{
	public v900014i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RMAClaimComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RMAClaimComponents", new DmoField[19]
			{
				new DmoField("raoRMAClaimID", "nvarchar", 10, 0, nullable: false),
				new DmoField("raoRMAClaimLineID", "smallint", 4, 0, nullable: false),
				new DmoField("raoRMAClaimComponentID", "int", 4, 0, nullable: false),
				new DmoField("raoPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("raoPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("raoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("raoPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("raoParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("raoQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("raoAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("raoQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("raoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("raoDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("raoWeight", "numeric", 15, 5, nullable: false),
				new DmoField("raoQuantityReceived", "numeric", 15, 5, nullable: false),
				new DmoField("raoReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("raoCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("raoCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("raoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("raoRMAClaimID,raoRMAClaimLineID,raoRMAClaimComponentID", unique: true),
				new DmoIndex("raoUniqueID", unique: true),
				new DmoIndex("raoReceivedComplete", unique: false)
			});
		}
	}
}
