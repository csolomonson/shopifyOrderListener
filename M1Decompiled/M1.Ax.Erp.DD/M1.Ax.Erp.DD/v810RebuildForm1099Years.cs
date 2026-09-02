using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form1099Years to support unicode", "2013-10-17")]
public class v810RebuildForm1099Years
{
	public v810RebuildForm1099Years(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099Years", new DmoField[22]
		{
			new DmoField("apyForm1099YearID", "smallint", 4, 0, nullable: false),
			new DmoField("apyPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apyPayersName", "nvarchar", 50, 0, nullable: false),
			new DmoField("apyAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("apyAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("apyPayersCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("apyPayersState", "nvarchar", 3, 0, nullable: false),
			new DmoField("apyPayersPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("apyTotalsCalculatedDate", "date", 14, 0, nullable: true),
			new DmoField("apyPrintedDate", "date", 14, 0, nullable: true),
			new DmoField("apyClosedDate", "date", 14, 0, nullable: true),
			new DmoField("apyClosed", "bit", 1, 0, nullable: false),
			new DmoField("apyFederalID", "nvarchar", 20, 0, nullable: false),
			new DmoField("apyContactPhoneNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("apyStateNo", "nvarchar", 20, 0, nullable: false),
			new DmoField("apyIncludeNoneInOther", "bit", 1, 0, nullable: false),
			new DmoField("apyFaxNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("apyContactName", "nvarchar", 50, 0, nullable: false),
			new DmoField("apyEmailAddress", "nvarchar", 50, 0, nullable: false),
			new DmoField("apyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apyCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("apyUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("APYFORM1099YEARID,APYPLANTID", unique: true),
			new DmoIndex("APYUNIQUEID", unique: true),
			new DmoIndex("apyPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
