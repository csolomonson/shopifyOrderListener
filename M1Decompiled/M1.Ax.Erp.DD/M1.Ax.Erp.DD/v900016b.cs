using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.016", "Add fields to ScheduleDates table", "2015-02-08")]
public class v900016b
{
	public v900016b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleDates"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleDates", new DmoField[15]
			{
				new DmoField("sxdScheduleDateID", "identity", 4, 0, nullable: false),
				new DmoField("sxdDateType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxdDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxdHour", "numeric", 5, 2, nullable: false),
				new DmoField("sxdActualDateTime", "datetime", 14, 0, nullable: true),
				new DmoField("sxdDuration", "numeric", 8, 2, nullable: false),
				new DmoField("sxdEndDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxdEndHour", "numeric", 5, 2, nullable: false),
				new DmoField("sxdEndActualDateTime", "datetime", 14, 0, nullable: true),
				new DmoField("sxdScheduleTreeID", "int", 4, 0, nullable: false),
				new DmoField("sxdScheduleBranchID", "int", 4, 0, nullable: false),
				new DmoField("sxdScheduleTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxdCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("sxdCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxdUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("sxdScheduleDateID", unique: true),
				new DmoIndex("sxdUniqueID", unique: true)
			});
		}
	}
}
