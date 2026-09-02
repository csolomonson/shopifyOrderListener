using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert PartFindAndReplaceLogLines to support unicode", "2013-10-17")]
public class v810RebuildPartFindAndReplaceLogLines
{
	public v810RebuildPartFindAndReplaceLogLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartFindAndReplaceLogLines", new DmoField[21]
		{
			new DmoField("abiPartFindAndReplaceLogID", "int", 9, 0, nullable: false),
			new DmoField("abiPartFindAndReplaceLogLineID", "int", 6, 0, nullable: false),
			new DmoField("abiSource", "nvarchar", 2, 0, nullable: false),
			new DmoField("abiQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("abiQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("abiQuoteAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("abiQuoteMaterialID", "int", 5, 0, nullable: false),
			new DmoField("abiQuoteOperationID", "int", 5, 0, nullable: false),
			new DmoField("abiJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("abiJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("abiJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("abiJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("abiPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("abiPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("abiPartAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("abiPartMaterialID", "int", 5, 0, nullable: false),
			new DmoField("abiPartOperationID", "int", 5, 0, nullable: false),
			new DmoField("abiSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("abiSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("abiShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("abiShipmentLineID", "smallint", 4, 0, nullable: false)
		}, new DmoIndex[9]
		{
			new DmoIndex("ABIPARTFINDANDREPLACELOGID,ABIPARTFINDANDREPLACELOGLINEID", unique: true),
			new DmoIndex("abiPartFindAndReplaceLogID", unique: false),
			new DmoIndex("abiPartFindAndReplaceLogLineID", unique: false),
			new DmoIndex("abiSource", unique: false),
			new DmoIndex("abiQuoteID", unique: false),
			new DmoIndex("abiJobID", unique: false),
			new DmoIndex("abiPartID", unique: false),
			new DmoIndex("abiSalesOrderID", unique: false),
			new DmoIndex("abiShipmentID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
