using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.664", "Create Mfg Req Planner Supply table", "2018-03-16")]
public class v92664c
{
	public v92664c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPSupply"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSupply", new DmoField[19]
			{
				new DmoField("mrsSessionID", "nvarchar", 10, 0, nullable: false),
				new DmoField("mrsLineID", "int", 4, 0, nullable: false),
				new DmoField("mrsSupplyID", "int", 4, 0, nullable: false),
				new DmoField("mrsPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("mrsPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("mrsPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("mrsPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("mrsDueDate", "date", 14, 0, nullable: true),
				new DmoField("mrsJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("mrsJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("mrsOriginalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("mrsQuantityReceived", "numeric", 15, 5, nullable: false),
				new DmoField("mrsQuantityShipped", "numeric", 15, 5, nullable: false),
				new DmoField("mrsDemandQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("mrsSource", "nvarchar", 20, 0, nullable: false),
				new DmoField("mrsType", "nvarchar", 20, 0, nullable: false),
				new DmoField("mrsCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("mrsCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("mrsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("mrsSessionID,mrsLineID,mrsSupplyID", unique: true),
				new DmoIndex("mrsUniqueID", unique: true)
			});
		}
	}
}
