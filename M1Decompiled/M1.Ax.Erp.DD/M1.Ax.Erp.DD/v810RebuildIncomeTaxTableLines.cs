using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxTableLines to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxTableLines
{
	public v810RebuildIncomeTaxTableLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableLines", new DmoField[15]
		{
			new DmoField("palIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("palIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("palIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("palIncomeTaxTableRevisionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("palIncomeTaxTableLineID", "smallint", 4, 0, nullable: false),
			new DmoField("palEarningsOver", "money", 11, 2, nullable: false),
			new DmoField("palEarningsNotOver", "money", 11, 2, nullable: false),
			new DmoField("palTaxAmount", "money", 13, 4, nullable: false),
			new DmoField("palTaxPercent", "numeric", 8, 4, nullable: false),
			new DmoField("palMultiplier", "numeric", 8, 4, nullable: false),
			new DmoField("palTaxCredit", "bit", 1, 0, nullable: false),
			new DmoField("palTaxLimit", "money", 13, 4, nullable: false),
			new DmoField("palCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("palCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("palUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("PALINCOMETAXID,PALINCOMETAXTYPEID,PALINCOMETAXTABLEID,PALINCOMETAXTABLEREVISIONID,PALINCOMETAXTABLELINEID", unique: true),
			new DmoIndex("PALUNIQUEID", unique: true),
			new DmoIndex("palIncomeTaxID", unique: false),
			new DmoIndex("palIncomeTaxTypeID", unique: false),
			new DmoIndex("palIncomeTaxTableID", unique: false),
			new DmoIndex("palIncomeTaxTableRevisionID", unique: false),
			new DmoIndex("palIncomeTaxTableLineID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
