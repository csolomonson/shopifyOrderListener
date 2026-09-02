using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.068", "Add fields to EmployeeSkills table", "2014-03-03")]
public class v810068a
{
	public v810068a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EmployeeSkills"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeSkills", new DmoField[9]
			{
				new DmoField("lnkEmployeeID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnkEmployeeSkillID", "smallint", 4, 0, nullable: false),
				new DmoField("lnkSkillID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lnkNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lnkNotesText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lnkDocuments", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lnkCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lnkCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lnkUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[5]
			{
				new DmoIndex("lnkEmployeeID,lnkEmployeeSkillID", unique: true),
				new DmoIndex("lnkUniqueID", unique: true),
				new DmoIndex("lnkEmployeeID", unique: false),
				new DmoIndex("lnkEmployeeSkillID", unique: false),
				new DmoIndex("lnkSkillID", unique: false)
			});
		}
	}
}
