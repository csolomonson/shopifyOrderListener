using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Add fields to Form940YearTotalStates table", "2015-05-19")]
public class v900036i
{
	public v900036i(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form940YearTotalStates"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form940YearTotalStates", new DmoField[12]
			{
				new DmoField("pfsForm940YearID", "smallint", 4, 0, nullable: false),
				new DmoField("pfsPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("pfsForm940YearTotalID", "smallint", 4, 0, nullable: false),
				new DmoField("pfsForm940YearTotalStateID", "tinyint", 2, 0, nullable: false),
				new DmoField("pfsState", "nvarchar", 2, 0, nullable: false),
				new DmoField("pfsFUTATaxableWages", "money", 12, 2, nullable: false),
				new DmoField("pfsReductionRate", "numeric", 5, 3, nullable: false),
				new DmoField("pfsCreditReduction", "money", 12, 2, nullable: false),
				new DmoField("pfsClosed", "bit", 1, 0, nullable: false),
				new DmoField("pfsCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("pfsCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("pfsUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("PFSFORM940YEARID,PFSPLANTID,PFSFORM940YEARTOTALID,PFSFORM940YEARTOTALSTATEID", unique: true),
				new DmoIndex("PFSUNIQUEID", unique: true),
				new DmoIndex("pfsForm940YearID", unique: false),
				new DmoIndex("pfsPlantID", unique: false),
				new DmoIndex("pfsForm940YearTotalID", unique: false),
				new DmoIndex("pfsForm940YearTotalStateID", unique: false)
			});
		}
	}
}
