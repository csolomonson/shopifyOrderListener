using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to MaterialIssueLines table", "2014-09-25")]
public class v900003f
{
	public v900003f(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MaterialIssueLines"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssueLines", new DmoField[31]
			{
				new DmoField("injMaterialIssueID", "nvarchar", 10, 0, nullable: false),
				new DmoField("injMaterialIssueLineID", "smallint", 4, 0, nullable: false),
				new DmoField("injMaterialIssueDate", "datetime", 14, 0, nullable: true),
				new DmoField("injIssueType", "tinyint", 1, 0, nullable: false),
				new DmoField("injJobID", "nvarchar", 20, 0, nullable: false),
				new DmoField("injJobAssemblyID", "int", 5, 0, nullable: false),
				new DmoField("injCreateJobMatSeq", "bit", 1, 0, nullable: false),
				new DmoField("injJobMaterialID", "int", 5, 0, nullable: false),
				new DmoField("injJobType", "tinyint", 1, 0, nullable: false),
				new DmoField("injEstimatedQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("injJobOpenQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("injIssueComplete", "bit", 1, 0, nullable: false),
				new DmoField("injPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("injPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("injPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
				new DmoField("injPartBinID", "nvarchar", 15, 0, nullable: false),
				new DmoField("injQuantityOnHand", "numeric", 15, 5, nullable: false),
				new DmoField("injQuantityAllocated", "numeric", 15, 5, nullable: false),
				new DmoField("injLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("injLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("injIssueQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("injScrapQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("injReference", "nvarchar", 30, 0, nullable: false),
				new DmoField("injHeatLot", "nvarchar", 50, 0, nullable: false),
				new DmoField("injMiscIssueReasonID", "nvarchar", 5, 0, nullable: false),
				new DmoField("injProjectID", "nvarchar", 10, 0, nullable: false),
				new DmoField("injProjectAreaID", "nvarchar", 15, 0, nullable: false),
				new DmoField("injPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("injCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("injCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("injUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[15]
			{
				new DmoIndex("injMaterialIssueID,injMaterialIssueLineID", unique: true),
				new DmoIndex("injUniqueID", unique: true),
				new DmoIndex("injMaterialIssueDate", unique: false),
				new DmoIndex("injIssueType", unique: false),
				new DmoIndex("injJobID", unique: false),
				new DmoIndex("injJobAssemblyID", unique: false),
				new DmoIndex("injCreateJobMatSeq", unique: false),
				new DmoIndex("injJobMaterialID", unique: false),
				new DmoIndex("injJobType", unique: false),
				new DmoIndex("injIssueComplete", unique: false),
				new DmoIndex("injPartID", unique: false),
				new DmoIndex("injPartRevisionID", unique: false),
				new DmoIndex("injPartWarehouseLocationID", unique: false),
				new DmoIndex("injPartBinID", unique: false),
				new DmoIndex("injPlantID", unique: false)
			});
		}
	}
}
