using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.068", "Add fields to EmployeeSkillCompetencies table", "2014-03-03")]
public class v810068b
{
	public v810068b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeeSkillCompetencies"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSkillCompetencies", new DmoField[12]
			{
				new DmoField("lnpSkillID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnpCompetencyID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnpCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lnpCommentsText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lnpEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnpEmployeeSkillID", "smallint", 4, 0, nullable: false),
				new DmoField("lnpEmployeeSkillCompetencyID", "smallint", 4, 0, nullable: false),
				new DmoField("lnpDateAchieved", "date", 14, 0, nullable: true),
				new DmoField("lnpDateExpires", "date", 14, 0, nullable: true),
				new DmoField("lnpCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lnpCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lnpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[7]
			{
				new DmoIndex("lnpEmployeeID,lnpEmployeeSkillID,lnpEmployeeSkillCompetencyID", unique: true),
				new DmoIndex("lnpUniqueID", unique: true),
				new DmoIndex("lnpEmployeeID", unique: false),
				new DmoIndex("lnpEmployeeSkillID", unique: false),
				new DmoIndex("lnpSkillID", unique: false),
				new DmoIndex("lnpCompetencyID", unique: false),
				new DmoIndex("lnpDateAchieved", unique: false)
			});
		}
	}
}
