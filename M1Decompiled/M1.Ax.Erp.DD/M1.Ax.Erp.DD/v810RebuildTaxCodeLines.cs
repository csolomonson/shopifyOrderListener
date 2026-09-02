using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert TaxCodeLines to support unicode", "2013-10-17")]
public class v810RebuildTaxCodeLines
{
	public v810RebuildTaxCodeLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TaxCodeLines", new DmoField[9]
		{
			new DmoField("xabTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xabTaxCodeLineID", "int", 7, 0, nullable: false),
			new DmoField("xabEffectiveDate", "date", 14, 0, nullable: true),
			new DmoField("xabTaxRate", "numeric", 7, 4, nullable: false),
			new DmoField("xabTaxRateNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xabTaxRateNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xabCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xabCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xabUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("XABTAXCODEID,XABTAXCODELINEID", unique: true),
			new DmoIndex("XABUNIQUEID", unique: true),
			new DmoIndex("xabTaxCodeID", unique: false),
			new DmoIndex("xabTaxCodeLineID", unique: false),
			new DmoIndex("xabEffectiveDate", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
