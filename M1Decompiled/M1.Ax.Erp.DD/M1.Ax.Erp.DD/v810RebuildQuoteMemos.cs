using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteMemos to support unicode", "2013-10-17")]
public class v810RebuildQuoteMemos
{
	public v810RebuildQuoteMemos(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteMemos", new DmoField[12]
		{
			new DmoField("qmkQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmkQuoteMemoID", "smallint", 4, 0, nullable: false),
			new DmoField("qmkMemoDate", "date", 14, 0, nullable: true),
			new DmoField("qmkShortDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("qmkLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmkLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmkClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmkShowInQuotes", "bit", 1, 0, nullable: false),
			new DmoField("qmkShowInSalesOrders", "bit", 1, 0, nullable: false),
			new DmoField("qmkCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmkCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("QMKQUOTEID,QMKQUOTEMEMOID", unique: true),
			new DmoIndex("QMKUNIQUEID", unique: true),
			new DmoIndex("qmkQuoteID", unique: false),
			new DmoIndex("qmkQuoteMemoID", unique: false),
			new DmoIndex("qmkMemoDate", unique: false),
			new DmoIndex("qmkShowInQuotes", unique: false),
			new DmoIndex("qmkShowInSalesOrders", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
