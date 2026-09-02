using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert RFQLines to support unicode", "2013-10-17")]
public class v810RebuildRFQLines
{
	public v810RebuildRFQLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RFQLines", new DmoField[28]
		{
			new DmoField("rqlRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqlRFQLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rqlPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("rqlPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rqlPurchaseUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("rqlInventoryUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("rqlPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rqlPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rqlPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rqlAlternatePart", "bit", 1, 0, nullable: false),
			new DmoField("rqlRFQType", "tinyint", 1, 0, nullable: false),
			new DmoField("rqlQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqlQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("rqlQuoteAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("rqlQuoteMaterialID", "int", 5, 0, nullable: false),
			new DmoField("rqlQuoteOperationID", "int", 5, 0, nullable: false),
			new DmoField("rqlJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqlJobAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("rqlJobMaterialID", "int", 5, 0, nullable: false),
			new DmoField("rqlJobOperationID", "int", 5, 0, nullable: false),
			new DmoField("rqlClosed", "bit", 1, 0, nullable: false),
			new DmoField("rqlProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("rqlProjectAreaID", "nvarchar", 15, 0, nullable: false),
			new DmoField("rqlDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("rqlJobEstimatedQty", "numeric", 10, 2, nullable: false),
			new DmoField("rqlCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rqlCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rqlUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("RQLRFQID,RQLRFQLINEID", unique: true),
			new DmoIndex("RQLUNIQUEID", unique: true),
			new DmoIndex("rqlRFQID", unique: false),
			new DmoIndex("rqlRFQLineID", unique: false),
			new DmoIndex("rqlPartID", unique: false),
			new DmoIndex("rqlPartRevisionID", unique: false),
			new DmoIndex("rqlQuoteID", unique: false),
			new DmoIndex("rqlQuoteLineID", unique: false),
			new DmoIndex("rqlJobID", unique: false),
			new DmoIndex("rqlProjectID", unique: false),
			new DmoIndex("rqlProjectAreaID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
