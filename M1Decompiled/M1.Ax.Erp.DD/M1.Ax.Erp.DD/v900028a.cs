using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.028", "Create PartTransactionCosts table", "2015-04-08")]
public class v900028a
{
	public v900028a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "PartTransactionCosts"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartTransactionCosts", new DmoField[30]
			{
				new DmoField("intPartTransactionID", "int", 9, 0, nullable: false),
				new DmoField("intPartTransactionCostID", "int", 4, 0, nullable: false),
				new DmoField("intCostType", "tinyint", 1, 0, nullable: false),
				new DmoField("intQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitLaborCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitOverheadCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitMaterialCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitSubcontractCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitDutyCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitFreightCost", "numeric", 15, 5, nullable: false),
				new DmoField("intUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("intPrevUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("intActualUnitMiscCost", "numeric", 15, 5, nullable: false),
				new DmoField("intSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("intSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("intCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("intCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("intUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("intPartTransactionID,intPartTransactionCostID", unique: true),
				new DmoIndex("intUniqueID", unique: true),
				new DmoIndex("intCostType", unique: false),
				new DmoIndex("intQuantity", unique: false),
				new DmoIndex("intSourceTableName", unique: false),
				new DmoIndex("intSourceTableUniqueID", unique: false)
			});
		}
	}
}
