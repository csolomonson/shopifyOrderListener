using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.054", "Add QuantityAdjustments table", "2015-07-03")]
public class v900054b
{
	public v900054b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "QuantityAdjustments"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuantityAdjustments", new DmoField[26]
			{
				new DmoField("inqQuantityAdjustmentID", "nvarchar", 10, 0, nullable: false),
				new DmoField("inqAdjustmentType", "tinyint", 1, 0, nullable: false),
				new DmoField("inqPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("inqPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
				new DmoField("inqPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("inqPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inqPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("inqPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inqPartShortDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("inqUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
				new DmoField("inqCurrentQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inqCountedQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inqAdjustmentDate", "datetime", 14, 0, nullable: true),
				new DmoField("inqAdjustmentDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("inqQuantitySince", "numeric", 15, 5, nullable: false),
				new DmoField("inqTransactionsSince", "smallint", 4, 0, nullable: false),
				new DmoField("inqNewQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inqChangeQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("inqDestinationPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("inqBinQuantityTransferred", "numeric", 15, 5, nullable: false),
				new DmoField("inqBinQuantityReceipted", "numeric", 15, 5, nullable: false),
				new DmoField("inqPosted", "bit", 1, 0, nullable: false),
				new DmoField("inqPostedDate", "date", 14, 0, nullable: true),
				new DmoField("inqCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("inqCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("inqUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("inqQuantityAdjustmentID", unique: true),
				new DmoIndex("inqUniqueID", unique: true),
				new DmoIndex("inqAdjustmentType", unique: false)
			});
		}
	}
}
