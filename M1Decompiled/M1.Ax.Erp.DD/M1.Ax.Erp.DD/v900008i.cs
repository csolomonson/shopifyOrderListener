using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.008", "Add fields to ToolMemos table", "2014-10-30")]
public class v900008i
{
	public v900008i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "ToolMemos"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ToolMemos", new DmoField[9]
			{
				new DmoField("xtmToolID", "nvarchar", 10, 0, nullable: false),
				new DmoField("xtmToolMemoID", "smallint", 4, 0, nullable: false),
				new DmoField("xtmMemoDate", "date", 14, 0, nullable: true),
				new DmoField("xtmShortDescription", "nvarchar", 50, 0, nullable: false),
				new DmoField("xtmLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xtmLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("xtmCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xtmCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xtmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("xtmToolID,xtmToolMemoID", unique: true),
				new DmoIndex("xtmUniqueID", unique: true)
			});
		}
	}
}
