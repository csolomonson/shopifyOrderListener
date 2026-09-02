using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.081", "Add fields to ToolCategories table", "2014-09-22")]
public class v810081a
{
	public v810081a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ToolCategories"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ToolCategories", new DmoField[7]
			{
				new DmoField("xtcToolCategoryID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtcDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("xtcInactive", "bit", 1, 0, nullable: false),
				new DmoField("xtcInactiveDate", "date", 14, 0, nullable: true),
				new DmoField("xtcCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xtcCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xtcUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("xtcToolCategoryID", unique: true),
				new DmoIndex("xtcUniqueID", unique: true)
			});
		}
	}
}
