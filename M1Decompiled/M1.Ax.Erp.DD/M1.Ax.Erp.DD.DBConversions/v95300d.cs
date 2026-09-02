using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.5.300", "Add Recent Activity Log table", "2022-05-16")]
public class v95300d
{
	public v95300d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "RecentActivityLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "RecentActivityLog", new DmoField[11]
			{
				new DmoField("rxlRecentActivityLogID", "identity", 4, 0, nullable: false),
				new DmoField("rxlExplorerType", "nvarchar", 30, 0, nullable: false),
				new DmoField("rxlObjectDataRun", "nvarchar", 100, 0, nullable: false),
				new DmoField("rxlObjectName", "nvarchar", 100, 0, nullable: false),
				new DmoField("rxlGridID", "nvarchar", 100, 0, nullable: false),
				new DmoField("rxlVisualizerID", "nvarchar", 100, 0, nullable: false),
				new DmoField("rxlVisualizerType", "nvarchar", 30, 0, nullable: false),
				new DmoField("rxlProcessedDateTime", "datetime", 14, 0, nullable: false),
				new DmoField("rxlCount", "int", 4, 0, nullable: false),
				new DmoField("rxlUserID", "nvarchar", 50, 0, nullable: false),
				new DmoField("rxlRowVersion", "timestamp", 0, 0, nullable: true)
			}, new DmoIndex[3]
			{
				new DmoIndex("RXLRECENTACTIVITYLOGID", unique: true),
				new DmoIndex("rxlProcessedDateTime", unique: false),
				new DmoIndex("rxlUserID", unique: false)
			});
		}
	}
}
