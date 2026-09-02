using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form941Years to support unicode", "2013-10-17")]
public class v810RebuildForm941Years
{
	public v810RebuildForm941Years(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form941Years", new DmoField[25]
		{
			new DmoField("ptyYearID", "smallint", 4, 0, nullable: false),
			new DmoField("ptyPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("ptyEIN", "nvarchar", 20, 0, nullable: false),
			new DmoField("ptyName", "nvarchar", 50, 0, nullable: false),
			new DmoField("ptyTradeName", "nvarchar", 50, 0, nullable: false),
			new DmoField("ptyAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("ptyAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("ptyCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("ptyState", "nvarchar", 3, 0, nullable: false),
			new DmoField("ptyPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("ptyQuarter1CompleteDate", "date", 14, 0, nullable: true),
			new DmoField("ptyQuarter2CompleteDate", "date", 14, 0, nullable: true),
			new DmoField("ptyQuarter3CompleteDate", "date", 14, 0, nullable: true),
			new DmoField("ptyQuarter4CompleteDate", "date", 14, 0, nullable: true),
			new DmoField("ptyQuarter1Complete", "bit", 1, 0, nullable: false),
			new DmoField("ptyQuarter2Complete", "bit", 1, 0, nullable: false),
			new DmoField("ptyQuarter3Complete", "bit", 1, 0, nullable: false),
			new DmoField("ptyQuarter4Complete", "bit", 1, 0, nullable: false),
			new DmoField("ptyClosed", "bit", 1, 0, nullable: false),
			new DmoField("ptyCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("ptyCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("ptyUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("ptyForeignCountry", "nvarchar", 20, 0, nullable: false),
			new DmoField("ptyForeignPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("ptyForeignState", "nvarchar", 30, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PTYYEARID,PTYPLANTID", unique: true),
			new DmoIndex("PTYUNIQUEID", unique: true),
			new DmoIndex("ptyPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
