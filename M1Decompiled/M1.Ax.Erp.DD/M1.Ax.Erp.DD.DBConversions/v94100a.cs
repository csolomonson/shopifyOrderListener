using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.100", "Add ProcessedMessages table", "2021-04-27")]
public class v94100a
{
	public v94100a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ProcessedMessages"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ProcessedMessages", new DmoField[2]
			{
				new DmoField("pmsMessageID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("pmsProcessedDateTime", "datetime", 14, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("pmsMessageID", unique: true),
				new DmoIndex("pmsProcessedDateTime", unique: false)
			});
		}
	}
}
