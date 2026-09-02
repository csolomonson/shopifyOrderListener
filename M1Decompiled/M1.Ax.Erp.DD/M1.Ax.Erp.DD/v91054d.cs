using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.054", "Add fields to WorkCenterSkillCompetencies table", "2016-05-12")]
public class v91054d
{
	public v91054d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WorkCenterSkillCompetencies"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenterSkillCompetencies", new DmoField[12]
			{
				new DmoField("xbbWorkCenterID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xbbWorkCenterSkillID", "smallint", 4, 0, nullable: false),
				new DmoField("xbbWorkCenterSkillCompetencyID", "smallint", 4, 0, nullable: false),
				new DmoField("xbbSkillID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xbbDateAchieved", "date", 14, 0, nullable: true),
				new DmoField("xbbCompetencyID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xbbDateExpires", "date", 14, 0, nullable: true),
				new DmoField("xbbCommentsText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xbbCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xbbCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xbbCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xbbUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("xbbWorkCenterID,xbbWorkCenterSkillID,xbbWorkCenterSkillCompetencyID", unique: true),
				new DmoIndex("xbbUniqueID", unique: true),
				new DmoIndex("xbbDateAchieved", unique: false)
			});
		}
	}
}
