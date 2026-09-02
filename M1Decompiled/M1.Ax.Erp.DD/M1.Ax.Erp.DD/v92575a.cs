using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.575a", "Create ErrorLog table", "2017-09-11")]
public class v92575a
{
	public v92575a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ErrorLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ErrorLog", new DmoField[9]
			{
				new DmoField("errMachine", "nvarchar", 50, 0, nullable: false),
				new DmoField("errCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("errCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("errSQLServerVersion", "nvarchar", 50, 0, nullable: false),
				new DmoField("errM1Version", "nvarchar", 15, 0, nullable: false),
				new DmoField("errOS", "nvarchar", 50, 0, nullable: false),
				new DmoField("errErrorMessageText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("errUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("errErrorLogID", "identity", 4, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("errErrorLogID", unique: true),
				new DmoIndex("errUniqueID", unique: true)
			});
		}
	}
}
