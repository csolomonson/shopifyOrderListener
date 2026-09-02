using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Add fields to CountyCodes table", "2015-05-19")]
public class v900036c
{
	public v900036c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "CountyCodes"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CountyCodes", new DmoField[7]
			{
				new DmoField("xccCountyCodeID", "nvarchar", 5, 0, nullable: false),
				new DmoField("xccStateCode", "nvarchar", 2, 0, nullable: false),
				new DmoField("xccCounty", "nvarchar", 30, 0, nullable: false),
				new DmoField("xccCountyCode", "nvarchar", 3, 0, nullable: false),
				new DmoField("xccCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("xccCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("xccUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("XCCCOUNTYCODEID", unique: true),
				new DmoIndex("XCCUNIQUEID", unique: true)
			});
		}
	}
}
