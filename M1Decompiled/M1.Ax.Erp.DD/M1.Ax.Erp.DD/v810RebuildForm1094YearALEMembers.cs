using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form1094YearALEMembers to support unicode", "2013-10-17")]
public class v810RebuildForm1094YearALEMembers
{
	public v810RebuildForm1094YearALEMembers(DBConversionParms parms)
	{
		if (parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "Form1094YearALEMembers"))
		{
			parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1094YearALEMembers", new DmoField[9]
			{
				new DmoField("hcaForm1094YearID", "smallint", 4, 0, nullable: false),
				new DmoField("hcaPlantID", "nvarchar", 5, 0, nullable: false),
				new DmoField("hcaForm1094YearALEMemberID", "smallint", 4, 0, nullable: false),
				new DmoField("hcaBusinessName", "nvarchar", 75, 0, nullable: false),
				new DmoField("hcaBusinessEIN", "nvarchar", 9, 0, nullable: false),
				new DmoField("hcaClosed", "bit", 1, 0, nullable: false),
				new DmoField("hcaCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("hcaCreatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("hcaUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[5]
			{
				new DmoIndex("HCAFORM1094YEARID,HCAPLANTID,HCAFORM1094YEARALEMEMBERID", unique: true),
				new DmoIndex("HCAUNIQUEID", unique: true),
				new DmoIndex("hcaForm1094YearID", unique: false),
				new DmoIndex("hcaPlantID", unique: false),
				new DmoIndex("hcaForm1094YearALEMemberID", unique: false)
			}, mergeCustomFields: true);
		}
	}
}
