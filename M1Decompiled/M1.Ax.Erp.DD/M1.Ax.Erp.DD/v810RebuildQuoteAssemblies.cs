using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteAssemblies to support unicode", "2013-10-17")]
public class v810RebuildQuoteAssemblies
{
	public v810RebuildQuoteAssemblies(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteAssemblies", new DmoField[24]
		{
			new DmoField("qmaQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmaQuoteLineID", "smallint", 4, 0, nullable: false),
			new DmoField("qmaQuoteAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("qmaParentAssemblyID", "int", 5, 0, nullable: false),
			new DmoField("qmaLevel", "smallint", 3, 0, nullable: false),
			new DmoField("qmaSourceMethodID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmaSourceRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmaPartID", "nvarchar", 30, 0, nullable: false),
			new DmoField("qmaPartRevisionID", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmaUnitOfMeasure", "nvarchar", 2, 0, nullable: false),
			new DmoField("qmaPartShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmaPartLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmaPartLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmaQuantityPerParent", "numeric", 12, 5, nullable: false),
			new DmoField("qmaOverlapQuoteOperationID", "int", 5, 0, nullable: false),
			new DmoField("qmaOverlapType", "tinyint", 1, 0, nullable: false),
			new DmoField("qmaProductionNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmaProductionNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmaDocuments", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmaClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmaPullAllFromStock", "bit", 1, 0, nullable: false),
			new DmoField("qmaCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmaCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("QMAQUOTEID,QMAQUOTELINEID,QMAQUOTEASSEMBLYID", unique: true),
			new DmoIndex("QMAUNIQUEID", unique: true),
			new DmoIndex("qmaQuoteID", unique: false),
			new DmoIndex("qmaQuoteLineID", unique: false),
			new DmoIndex("qmaQuoteAssemblyID", unique: false),
			new DmoIndex("qmaSourceMethodID", unique: false),
			new DmoIndex("qmaSourceRevisionID", unique: false),
			new DmoIndex("qmaPartID", unique: false),
			new DmoIndex("qmaPartRevisionID", unique: false),
			new DmoIndex("qmaOverlapQuoteOperationID", unique: false),
			new DmoIndex("qmaClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
