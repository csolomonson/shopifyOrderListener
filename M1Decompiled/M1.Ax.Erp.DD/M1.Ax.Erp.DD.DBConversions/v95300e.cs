using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.300", "Add Recent Transaction Log table", "2022-05-16")]
public class v95300e
{
	public v95300e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RecentTransactionsLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RecentTransactionsLog", new DmoField[10]
			{
				new DmoField("rtlRecentTransactionLogID", "identity", 4, 0, nullable: false),
				new DmoField("rtlExplorerType", "nvarchar", 30, 0, nullable: false),
				new DmoField("rtlParentKey", "nvarchar", 50, 0, nullable: false),
				new DmoField("rtlObjectID", "nvarchar", 50, 0, nullable: false),
				new DmoField("rtlObjectDataRun", "nvarchar", 200, 0, nullable: false),
				new DmoField("rtlObjectName", "nvarchar", 100, 0, nullable: false),
				new DmoField("rtlUserID", "nvarchar", 50, 0, nullable: false),
				new DmoField("rtlCount", "int", 5, 0, nullable: false),
				new DmoField("rtlLastOpenedDateTime", "datetime", 14, 0, nullable: false),
				new DmoField("rtlRowVersion", "timestamp", 0, 0, nullable: true)
			}, new DmoIndex[5]
			{
				new DmoIndex("RTLRECENTTRANSACTIONLOGID", unique: true),
				new DmoIndex("rtlObjectID", unique: false),
				new DmoIndex("rtlParentKey", unique: false),
				new DmoIndex("rtlUserID", unique: false),
				new DmoIndex("rtlLastOpenedDateTime", unique: false)
			});
		}
	}
}
