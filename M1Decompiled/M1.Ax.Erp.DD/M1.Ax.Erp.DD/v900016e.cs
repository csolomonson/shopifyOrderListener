using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.016", "Add fields to ScheduleBranches table", "2015-02-08")]
public class v900016e
{
	public v900016e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ScheduleBranches"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ScheduleBranches", new DmoField[12]
			{
				new DmoField("sxbScheduleTreeID", "int", 4, 0, nullable: false),
				new DmoField("sxbScheduleBranchID", "int", 4, 0, nullable: false),
				new DmoField("sxbParentScheduleBranchID", "int", 4, 0, nullable: false),
				new DmoField("sxbSiblingBranchLink", "tinyint", 1, 0, nullable: false),
				new DmoField("sxbParentLinkedTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxbCurrentLinkedTaskID", "int", 4, 0, nullable: false),
				new DmoField("sxbCurrentLinkedTaskDateType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxbParentLinkedTaskDateType", "tinyint", 1, 0, nullable: false),
				new DmoField("sxbOffsetTime", "numeric", 5, 2, nullable: false),
				new DmoField("sxbCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("sxbCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("sxbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("sxbScheduleTreeID,sxbScheduleBranchID", unique: true),
				new DmoIndex("sxbUniqueID", unique: true)
			});
		}
	}
}
