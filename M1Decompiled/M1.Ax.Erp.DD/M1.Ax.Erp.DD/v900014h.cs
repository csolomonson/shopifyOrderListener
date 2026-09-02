using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to DMRShipmentComponents table", "2014-12-15")]
public class v900014h
{
	public v900014h(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "DMRShipmentComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipmentComponents", new DmoField[28]
			{
				new DmoField("dsoDMRShipmentID", "nvarchar", 10, 0, nullable: false),
				new DmoField("dsoDMRShipmentLineID", "smallint", 4, 0, nullable: false),
				new DmoField("dsoDMRShipmentComponentID", "int", 4, 0, nullable: false),
				new DmoField("dsoPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("dsoPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("dsoPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("dsoPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("dsoInvParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dsoJobMatParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dsoQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("dsoAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("dsoInvQuantityShipped", "numeric", 15, 5, nullable: false),
				new DmoField("dsoJobMatQuantityShipped", "numeric", 15, 5, nullable: false),
				new DmoField("dsoUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("dsoDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("dsoWeight", "numeric", 15, 5, nullable: false),
				new DmoField("dsoShippedComplete", "bit", 1, 0, nullable: false),
				new DmoField("dsoClosed", "bit", 1, 0, nullable: false),
				new DmoField("dsoDMRClaimID", "nvarchar", 10, 0, nullable: false),
				new DmoField("dsoDMRClaimLineID", "smallint", 4, 0, nullable: false),
				new DmoField("dsoDMRClaimComponentID", "int", 4, 0, nullable: false),
				new DmoField("dsoJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("dsoJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("dsoJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("dsoJobMaterialComponentID", "int", 5, 0, nullable: false),
				new DmoField("dsoCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("dsoCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("dsoUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("dsoDMRShipmentID,dsoDMRShipmentLineID,dsoDMRShipmentComponentID", unique: true),
				new DmoIndex("dsoUniqueID", unique: true),
				new DmoIndex("dsoShippedComplete", unique: false)
			});
		}
	}
}
