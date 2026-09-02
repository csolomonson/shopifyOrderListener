using M1.Core;

namespace M1.Ax.Erp.DD.DBConversions;

[DBConversion("9.4.200", "Convert Form1099NECYearTotals table to support unicode", "2021-08-11")]
public class v94200c
{
	public v94200c(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Form1099NECYearTotals", new DmoField[25]
		{
			new DmoField("apeForm1099YearID", "smallint", 4, 0, nullable: false),
			new DmoField("apeForm1099YearLineID", "smallint", 4, 0, nullable: false),
			new DmoField("apeSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("apePlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("apeSupplierName", "nvarchar", 50, 0, nullable: false),
			new DmoField("apeSupplierAddressLine1", "nvarchar", 50, 0, nullable: false),
			new DmoField("apeSupplierAddressLine2", "nvarchar", 50, 0, nullable: false),
			new DmoField("apeSupplierCity", "nvarchar", 30, 0, nullable: false),
			new DmoField("apeSupplierState", "nvarchar", 3, 0, nullable: false),
			new DmoField("apeOrganizationAccountID", "nvarchar", 20, 0, nullable: false),
			new DmoField("apeBox1", "money", 12, 2, nullable: false),
			new DmoField("apeBox2", "bit", 1, 0, nullable: false),
			new DmoField("apeBox4", "money", 12, 2, nullable: false),
			new DmoField("apeBox5", "money", 12, 2, nullable: false),
			new DmoField("apeBox6", "money", 12, 0, nullable: false),
			new DmoField("apeBox7", "money", 12, 2, nullable: false),
			new DmoField("apeFederalID", "nvarchar", 20, 0, nullable: false),
			new DmoField("apeClosed", "bit", 1, 0, nullable: false),
			new DmoField("apeSupplierPostCode", "nvarchar", 10, 0, nullable: false),
			new DmoField("apeType", "tinyint", 1, 0, nullable: false),
			new DmoField("apeFormType", "tinyint", 1, 0, nullable: false),
			new DmoField("apeCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("apeCreatedDate", "date", 14, 0, nullable: true),
			new DmoField("apeUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("apeRowVersion", "timestamp", 0, 0, nullable: true)
		}, new DmoIndex[3]
		{
			new DmoIndex("apeForm1099YearID,apePlantID,apeForm1099YearLineID", unique: true),
			new DmoIndex("apeUniqueID", unique: true),
			new DmoIndex("apePlantID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
