using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to MaterialIssueComponents table", "2014-10-23")]
public class v900008e
{
	public v900008e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MaterialIssueComponents"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueComponents", new DmoField[24]
			{
				new DmoField("inkMaterialIssueID", "nvarchar", 10, 0, nullable: false),
				new DmoField("inkMaterialIssueLineID", "smallint", 4, 0, nullable: false),
				new DmoField("inkPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("inkMaterialIssueComponentID", "int", 4, 0, nullable: false),
				new DmoField("inkPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inkPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("inkPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inkJobParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inkInvParentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inkQuantityPerParent", "numeric", 12, 5, nullable: false),
				new DmoField("inkAdditionalQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inkInvQuantityIssued", "numeric", 15, 5, nullable: false),
				new DmoField("inkJobQuantityIssued", "numeric", 15, 5, nullable: false),
				new DmoField("inkReceivedComplete", "bit", 1, 0, nullable: false),
				new DmoField("inkUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("inkDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("inkJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("inkJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("inkJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("inkJobMaterialComponentID", "int", 5, 0, nullable: false),
				new DmoField("inkWeight", "numeric", 15, 5, nullable: false),
				new DmoField("inkCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("inkCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("inkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[12]
			{
				new DmoIndex("inkMaterialIssueID,inkMaterialIssueLineID,inkMaterialIssueComponentID", unique: true),
				new DmoIndex("inkUniqueID", unique: true),
				new DmoIndex("inkMaterialIssueID", unique: false),
				new DmoIndex("inkMaterialIssueLineID", unique: false),
				new DmoIndex("inkPartID", unique: false),
				new DmoIndex("inkMaterialIssueComponentID", unique: false),
				new DmoIndex("inkPartRevisionID", unique: false),
				new DmoIndex("inkReceivedComplete", unique: false),
				new DmoIndex("inkJobID", unique: false),
				new DmoIndex("inkJobAssemblyID", unique: false),
				new DmoIndex("inkJobMaterialID", unique: false),
				new DmoIndex("inkJobMaterialComponentID", unique: false)
			});
		}
	}
}
