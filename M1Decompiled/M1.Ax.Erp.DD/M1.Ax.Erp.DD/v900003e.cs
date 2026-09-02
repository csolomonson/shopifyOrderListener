using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.003", "Add fields to MaterialIssues table", "2014-09-25")]
public class v900003e
{
	public v900003e(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "MaterialIssues"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "MaterialIssues", new DmoField[6]
			{
				new DmoField("iniMaterialIssueID", "nvarchar", 10, 0, nullable: false),
				new DmoField("iniMaterialIssueDate", "datetime", 14, 0, nullable: true),
				new DmoField("iniClosed", "bit", 1, 0, nullable: false),
				new DmoField("iniCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("iniCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("iniUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[3]
			{
				new DmoIndex("iniMaterialIssueID", unique: true),
				new DmoIndex("iniUniqueID", unique: true),
				new DmoIndex("iniMaterialIssueDate", unique: false)
			});
		}
	}
}
