using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert JobCosts to support unicode", "2013-10-17")]
public class v810RebuildJobCosts
{
	public v810RebuildJobCosts(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobCosts", new DmoField[28]
		{
			new DmoField("jmcJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmcJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("jmcJobType", "tinyint", 1, 0, nullable: false),
			new DmoField("jmcJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("jmcJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("jmcJobSequence", "int", 5, 0, nullable: false),
			new DmoField("jmcJobMaterialComponentID", "int", 5, 0, nullable: false),
			new DmoField("jmcCostSequence", "int", 6, 0, nullable: false),
			new DmoField("jmcReference", "nvarchar", 30, 0, nullable: false),
			new DmoField("jmcTotalCost", "numeric", 15, 5, nullable: false),
			new DmoField("jmcQuantityReceived", "numeric", 15, 5, nullable: false),
			new DmoField("jmcTransactionDate", "date", 14, 0, nullable: true),
			new DmoField("jmcPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("jmcPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("jmcReceivedUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("jmcPartDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmcSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("jmcSource", "tinyint", 1, 0, nullable: false),
			new DmoField("jmcHeatLot", "nvarchar", 50, 0, nullable: false),
			new DmoField("jmcReceiptID", "nvarchar", 10, 0, nullable: false),
			new DmoField("jmcReceiptLineID", "smallint", 4, 0, nullable: false),
			new DmoField("jmcReceiptComponentID", "smallint", 4, 0, nullable: false),
			new DmoField("jmcAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("jmcAPInvoiceLineID", "smallint", 4, 0, nullable: false),
			new DmoField("jmcTotalCOGSCost", "numeric", 15, 5, nullable: false),
			new DmoField("jmcCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("jmcCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("jmcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[15]
		{
			new DmoIndex("JMCJOBID,JMCJOBASSEMBLYID,JMCJOBTYPE,JMCJOBSEQUENCE,JMCCOSTSEQUENCE", unique: true),
			new DmoIndex("JMCUNIQUEID", unique: true),
			new DmoIndex("jmcJobID", unique: false),
			new DmoIndex("jmcJobAssemblyID", unique: false),
			new DmoIndex("jmcJobType", unique: false),
			new DmoIndex("jmcJobMaterialID", unique: false),
			new DmoIndex("jmcJobOperationID", unique: false),
			new DmoIndex("jmcJobSequence", unique: false),
			new DmoIndex("jmcJobMaterialComponentID", unique: false),
			new DmoIndex("jmcCostSequence", unique: false),
			new DmoIndex("jmcPartID", unique: false),
			new DmoIndex("jmcPartRevisionID", unique: false),
			new DmoIndex("jmcSource", unique: false),
			new DmoIndex("jmcAPInvoiceID", unique: false),
			new DmoIndex("jmcAPInvoiceLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
