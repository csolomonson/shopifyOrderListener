using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.573", "Add fields to MRPSessions table", "2017-11-13")]
public class v92573a
{
	public v92573a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MRPSessions"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MRPSessions", new DmoField[14]
			{
				new DmoField("mrpSessionID", "nvarchar", 10, 0, nullable: false),
				new DmoField("mrpPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("mrpWarehouseID", "nvarchar", 5, 0, nullable: false),
				new DmoField("mrpPartClassIDs", "nvarchar(max)", 4, 0, nullable: true),
				new DmoField("mrpPartGroupIDs", "nvarchar(max)", 4, 0, nullable: true),
				new DmoField("mrpPartIDs", "nvarchar(max)", 4, 0, nullable: true),
				new DmoField("mrpCustomerIDs", "nvarchar(max)", 4, 0, nullable: true),
				new DmoField("mrpCutoffDate", "date", 14, 0, nullable: true),
				new DmoField("mrpGenerated", "bit", 1, 0, nullable: false),
				new DmoField("mrpCompleted", "bit", 1, 0, nullable: false),
				new DmoField("mrpCompletedDate", "date", 14, 0, nullable: true),
				new DmoField("mrpCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("mrpCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("mrpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[4]
			{
				new DmoIndex("mrpSessionID", unique: true),
				new DmoIndex("mrpUniqueID", unique: true),
				new DmoIndex("mrpGenerated", unique: false),
				new DmoIndex("mrpCompleted", unique: false)
			});
		}
	}
}
