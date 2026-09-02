using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteMaterials to support unicode", "2013-10-17")]
public class v810RebuildQuoteMaterials
{
	public v810RebuildQuoteMaterials(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMaterials", new DmoField[57]
		{
			new DmoField("qmmQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmmQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmmQuoteAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("qmmQuoteMaterialID", "int", 5, 0, nullable: false),
			new DmoField("qmmPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmmPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmmPartWarehouseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmmPartBinID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmmUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("qmmPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmmPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmmPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmmSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmmPurchaseLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmmLeadTime", "smallint", 3, 0, nullable: false),
			new DmoField("qmmQuantityPerAssembly", "numeric", 13, 6, nullable: false),
			new DmoField("qmmEstimatedUnitCost", "numeric", 15, 5, nullable: false),
			new DmoField("qmmScrapQuantity", "numeric", 15, 5, nullable: false),
			new DmoField("qmmScrapPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmmMinimumCharge", "numeric", 8, 2, nullable: false),
			new DmoField("qmmRelatedQuoteOperationID", "int", 5, 0, nullable: false),
			new DmoField("qmmBackflush", "bit", 1, 0, nullable: false),
			new DmoField("qmmQuantityBreak1", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost1", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak2", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost2", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak3", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost3", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak4", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost4", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak5", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost5", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak6", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost6", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak7", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost7", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak8", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost8", "numeric", 15, 5, nullable: false),
			new DmoField("qmmQuantityBreak9", "numeric", 15, 5, nullable: false),
			new DmoField("qmmUnitCost9", "numeric", 15, 5, nullable: false),
			new DmoField("qmmClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmmSourcePriceID", "int", 9, 0, nullable: false),
			new DmoField("qmmSourceRFQID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmmLeadTime1", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime2", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime3", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime4", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime5", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime6", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime7", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime8", "smallint", 3, 0, nullable: false),
			new DmoField("qmmLeadTime9", "smallint", 3, 0, nullable: false),
			new DmoField("qmmCostOverride", "bit", 1, 0, nullable: false),
			new DmoField("qmmDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[14]
		{
			new DmoIndex("QMMQUOTEID,QMMQUOTELINEID,QMMQUOTEASSEMBLYID,QMMQUOTEMATERIALID", unique: true),
			new DmoIndex("QMMUNIQUEID", unique: true),
			new DmoIndex("qmmQuoteID", unique: false),
			new DmoIndex("qmmQuoteLineID", unique: false),
			new DmoIndex("qmmQuoteAssemblyID", unique: false),
			new DmoIndex("qmmQuoteMaterialID", unique: false),
			new DmoIndex("qmmPartID", unique: false),
			new DmoIndex("qmmPartRevisionID", unique: false),
			new DmoIndex("qmmPartWarehouseLocationID", unique: false),
			new DmoIndex("qmmPartBinID", unique: false),
			new DmoIndex("qmmClosed", unique: false),
			new DmoIndex("qmmSourcePriceID", unique: false),
			new DmoIndex("qmmSourceRFQID", unique: false),
			new DmoIndex("qmmCostOverride", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
