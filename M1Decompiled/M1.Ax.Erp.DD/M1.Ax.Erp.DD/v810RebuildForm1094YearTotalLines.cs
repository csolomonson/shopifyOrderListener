using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form1094YearTotalLines to support unicode", "2013-10-17")]
public class v810RebuildForm1094YearTotalLines
{
	public v810RebuildForm1094YearTotalLines(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1094YearTotalLines"))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearTotalLines", new DmoField[26]
			{
				new DmoField("hclForm1094YearID", "smallint", 4, 0, nullable: false),
				new DmoField("hclPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("hclForm1094YearTotalID", "smallint", 4, 0, nullable: false),
				new DmoField("hclForm1094YearTotalLineID", "tinyint", 2, 0, nullable: false),
				new DmoField("hclFirstName", "nvarchar", 40, 0, nullable: false),
				new DmoField("hclMiddleName", "nvarchar", 20, 0, nullable: false),
				new DmoField("hclLastName", "nvarchar", 20, 0, nullable: false),
				new DmoField("hclSSN", "nvarchar", 11, 0, nullable: false),
				new DmoField("hclDateOfBirth", "date", 14, 0, nullable: true),
				new DmoField("hclAnnualCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclJanCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclFebCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclMarCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclAprCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclMayCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclJunCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclJulCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclAugCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclSeptCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclOctCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclNovCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclDecCovered", "bit", 1, 0, nullable: false),
				new DmoField("hclClosed", "bit", 1, 0, nullable: false),
				new DmoField("hclCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("hclCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("hclUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[6]
			{
				new DmoIndex("HCLFORM1094YEARID,HCLPLANTID,HCLFORM1094YEARTOTALID,HCLFORM1094YEARTOTALLINEID", unique: true),
				new DmoIndex("HCLUNIQUEID", unique: true),
				new DmoIndex("hclForm1094YearID", unique: false),
				new DmoIndex("hclPlantID", unique: false),
				new DmoIndex("hclForm1094YearTotalID", unique: false),
				new DmoIndex("hclForm1094YearTotalLineID", unique: false)
			}, mergeCustomFields: true);
		}
	}
}
