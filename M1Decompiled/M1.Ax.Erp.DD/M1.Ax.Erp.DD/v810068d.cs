using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.068", "Add fields to Skills table", "2014-03-03")]
public class v810068d
{
	public v810068d(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Skills"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Skills", new DmoField[9]
			{
				new DmoField("lesSkillID", "nvarchar", 10, 0, nullable: false),
				new DmoField("lesDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("lesLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lesLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("lesInactive", "bit", 1, 0, nullable: false),
				new DmoField("lesInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("lesCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("lesCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("lesUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("lesSkillID", unique: true),
				new DmoIndex("lesUniqueID", unique: true)
			});
		}
	}
}
