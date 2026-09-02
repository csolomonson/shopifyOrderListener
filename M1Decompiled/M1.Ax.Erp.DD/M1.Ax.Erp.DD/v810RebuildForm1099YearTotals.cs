using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Form1099YearTotals to support unicode", "2013-10-17")]
public class v810RebuildForm1099YearTotals
{
	public v810RebuildForm1099YearTotals(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099YearTotals", new DmoField[35]
		{
			new DmoField("apfForm1099YearID", "smallint", 4, 0, nullable: false),
			new DmoField("apfForm1099YearLineID", "smallint", 4, 0, nullable: false),
			new DmoField("apfSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apfPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apfSupplierName", "nvarchar", 50, 0, nullable: false),
			new DmoField("apfSupplierAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("apfSupplierAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("apfSupplierCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("apfSupplierState", "nvarchar", 3, 0, nullable: false),
			new DmoField("apfOrganizationAccountID", "nvarchar", 20, 0, nullable: false),
			new DmoField("apfBox1", "money", 12, 2, nullable: false),
			new DmoField("apfBox2", "money", 12, 2, nullable: false),
			new DmoField("apfBox3", "money", 12, 2, nullable: false),
			new DmoField("apfBox4", "money", 12, 2, nullable: false),
			new DmoField("apfBox5", "money", 12, 2, nullable: false),
			new DmoField("apfBox6", "money", 12, 2, nullable: false),
			new DmoField("apfBox7", "bit", 1, 0, nullable: false),
			new DmoField("apfBox8", "money", 12, 2, nullable: false),
			new DmoField("apfBox9", "money", 12, 2, nullable: false),
			new DmoField("apfBox10", "money", 12, 2, nullable: false),
			new DmoField("apfBox11", "money", 12, 2, nullable: false),
			new DmoField("apfBox12", "money", 12, 2, nullable: false),
			new DmoField("apfBox13", "money", 12, 2, nullable: false),
			new DmoField("apfBox14", "money", 12, 2, nullable: false),
			new DmoField("apfBox15", "money", 12, 2, nullable: false),
			new DmoField("apfBox16", "money", 12, 0, nullable: false),
			new DmoField("apfBox17", "money", 12, 2, nullable: false),
			new DmoField("apfFederalID", "nvarchar", 20, 0, nullable: false),
			new DmoField("apfFormType", "tinyint", 1, 0, nullable: false),
			new DmoField("apfClosed", "bit", 1, 0, nullable: false),
			new DmoField("apfSupplierPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("apfType", "tinyint", 1, 0, nullable: false),
			new DmoField("apfCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apfCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("apfUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("APFFORM1099YEARID,APFPLANTID,APFFORM1099YEARLINEID", unique: true),
			new DmoIndex("APFUNIQUEID", unique: true),
			new DmoIndex("apfPlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
