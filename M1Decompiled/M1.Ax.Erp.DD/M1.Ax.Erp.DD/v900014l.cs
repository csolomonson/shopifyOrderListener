using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.014", "Add fields to InspectionComponents table", "2014-12-15")]
public class v900014l
{
	public v900014l(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "InspectionComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "InspectionComponents", new DmoField[35]
			{
				new DmoField("qamInspectionID", "nvarchar", 10, 0, nullable: false),
				new DmoField("qamInspectionLineID", "smallint", 4, 0, nullable: false),
				new DmoField("qamInspectionComponentID", "int", 4, 0, nullable: false),
				new DmoField("qamPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("qamPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qamPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("qamPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("qamParentQtyToInspect", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvParentQtyAccepted", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatParentQtyAccepted", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvParentQtyToScrap", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatParentQtyToScrap", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvParentQtyToReturn", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatParentQtyToReturn", "numeric", 15, 5, nullable: false),
				new DmoField("qamQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("qamAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvQuantityAccepted", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatQuantityAccepted", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvQuantityToScrap", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatQuantityToScrap", "numeric", 15, 5, nullable: false),
				new DmoField("qamInvQuantityToReturn", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobMatQuantityToReturn", "numeric", 15, 5, nullable: false),
				new DmoField("qamInspectionComplete", "bit", 1, 0, nullable: false),
				new DmoField("qamUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("qamDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("qamWeight", "numeric", 15, 5, nullable: false),
				new DmoField("qamJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("qamJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("qamJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("qamJobMaterialComponentID", "int", 5, 0, nullable: false),
				new DmoField("qamCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("qamCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("qamUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("qamSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("qamSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("qamInspectionID,qamInspectionLineID,qamInspectionComponentID", unique: true),
				new DmoIndex("qamUniqueID", unique: true),
				new DmoIndex("qamInspectionComplete", unique: false)
			});
		}
	}
}
