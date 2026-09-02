using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert QuoteSalesPeople to support unicode", "2013-10-17")]
public class v810RebuildQuoteSalesPeople
{
	public v810RebuildQuoteSalesPeople(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteSalesPeople", new DmoField[9]
		{
			new DmoField("qmjQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmjSequenceID", "smallint", 4, 0, nullable: false),
			new DmoField("qmjSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmjPercent", "numeric", 6, 2, nullable: false),
			new DmoField("qmjClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmjCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("qmjCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmjCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("QMJQUOTEID,QMJSEQUENCEID", unique: true),
			new DmoIndex("QMJUNIQUEID", unique: true),
			new DmoIndex("qmjQuoteID", unique: false),
			new DmoIndex("qmjSequenceID", unique: false),
			new DmoIndex("qmjClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
