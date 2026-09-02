using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.068", "Add fields to SkillCompetencies table", "2014-03-03")]
public class v810068c
{
	public v810068c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "SkillCompetencies"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SkillCompetencies", new DmoField[11]
			{
				new DmoField("lecCompetencyID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lecLevel", "tinyint", 2, 0, nullable: false),
				new DmoField("lecDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("lecColor", "int", 8, 0, nullable: false),
				new DmoField("lecLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lecLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lecInactive", "bit", 1, 0, nullable: false),
				new DmoField("lecInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("lecCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lecCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lecUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("lecCompetencyID", unique: true),
				new DmoIndex("lecUniqueID", unique: true)
			});
		}
	}
}
