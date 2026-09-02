using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert LeadSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildLeadSalesPeople
{
	public v810RebuildLeadSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "LeadSalesPeople", new DmoField[7]
		{
			new DmoField("lojLeadID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lojSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("lojSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lojPercent", "numeric", 6, 2, nullable: false),
			new DmoField("lojCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lojCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lojUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("LOJLEADID,LOJSEQUENCEID", unique: true),
			new DmoIndex("LOJUNIQUEID", unique: true),
			new DmoIndex("lojLeadID", unique: false),
			new DmoIndex("lojSequenceID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
