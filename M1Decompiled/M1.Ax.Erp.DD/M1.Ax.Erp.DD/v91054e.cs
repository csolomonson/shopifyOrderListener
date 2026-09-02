using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.054", "Add fields to WorkCenterSkills table", "2016-05-12")]
public class v91054e
{
	public v91054e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WorkCenterSkills"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WorkCenterSkills", new DmoField[9]
			{
				new DmoField("xbaWorkCenterID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xbaWorkCenterSkillID", "smallint", 4, 0, nullable: false),
				new DmoField("xbaSkillID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xbaNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xbaNotesText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xbaDocuments", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xbaCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xbaCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xbaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[5]
			{
				new DmoIndex("xbaWorkCenterID,xbaWorkCenterSkillID", unique: true),
				new DmoIndex("xbaUniqueID", unique: true),
				new DmoIndex("xbaWorkCenterID", unique: false),
				new DmoIndex("xbaWorkCenterSkillID", unique: false),
				new DmoIndex("xbaSkillID", unique: false)
			});
		}
	}
}
