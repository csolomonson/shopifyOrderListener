using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.1.040", "Add fields to Form1094YearMonths table", "2016-03-06")]
public class v91040c
{
	public v91040c(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1094YearMonths"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearMonths", new DmoField[12]
			{
				new DmoField("hcmForm1094YearID", "smallint", 4, 0, nullable: false),
				new DmoField("hcmPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("hcmForm1094YearMonthID", "tinyint", 2, 0, nullable: false),
				new DmoField("hcmMinEssentialCvrOffr", "bit", 1, 0, nullable: false),
				new DmoField("hcmALEFullTimeCount", "smallint", 4, 0, nullable: false),
				new DmoField("hcmTotalEmployeesCount", "smallint", 4, 0, nullable: false),
				new DmoField("hcmAggregatedGroup", "bit", 1, 0, nullable: false),
				new DmoField("hcmSection4980HRelief", "nvarchar", 1, 0, nullable: false),
				new DmoField("hcmClosed", "bit", 1, 0, nullable: false),
				new DmoField("hcmCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("hcmCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("hcmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[5]
			{
				new DmoIndex("HCMFORM1094YEARID,HCMPLANTID,HCMFORM1094YEARMONTHID", unique: true),
				new DmoIndex("HCMUNIQUEID", unique: true),
				new DmoIndex("hcmForm1094YearID", unique: false),
				new DmoIndex("hcmPlantID", unique: false),
				new DmoIndex("hcmForm1094YearMonthID", unique: false)
			});
		}
	}
}
