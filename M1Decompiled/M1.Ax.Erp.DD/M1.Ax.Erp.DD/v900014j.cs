using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to DMRClaimComponents table", "2014-12-15")]
public class v900014j
{
	public v900014j(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "DMRClaimComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRClaimComponents", new DmoField[26]
			{
				new DmoField("dmoDMRClaimID", "nvarchar", 10, 0, nullable: false),
				new DmoField("dmoDMRClaimLineID", "smallint", 4, 0, nullable: false),
				new DmoField("dmoDMRClaimComponentID", "int", 4, 0, nullable: false),
				new DmoField("dmoPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("dmoPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("dmoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("dmoPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("dmoParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dmoQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("dmoAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dmoQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dmoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("dmoDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("dmoWeight", "numeric", 15, 5, nullable: false),
				new DmoField("dmoQuantityShipped", "numeric", 15, 5, nullable: false),
				new DmoField("dmoShippedComplete", "bit", 1, 0, nullable: false),
				new DmoField("dmoInspectionID", "nvarchar", 10, 0, nullable: false),
				new DmoField("dmoInspectionLineID", "smallint", 4, 0, nullable: false),
				new DmoField("dmoInspectionComponentID", "int", 4, 0, nullable: false),
				new DmoField("dmoJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("dmoJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("dmoJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("dmoJobMaterialComponentID", "int", 5, 0, nullable: false),
				new DmoField("dmoCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("dmoCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("dmoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("dmoDMRClaimID,dmoDMRClaimLineID,dmoDMRClaimComponentID", unique: true),
				new DmoIndex("dmoUniqueID", unique: true),
				new DmoIndex("dmoShippedComplete", unique: false)
			});
		}
	}
}
