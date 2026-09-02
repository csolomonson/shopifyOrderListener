using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxTableSurtaxes to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxTableSurtaxes
{
	public v810RebuildIncomeTaxTableSurtaxes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTableSurtaxes", new DmoField[12]
		{
			new DmoField("pacIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pacIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pacIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pacIncomeTaxTableRevisionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pacIncomeTaxTableSurtaxID", "smallint", 4, 0, nullable: false),
			new DmoField("pacTaxOver", "money", 11, 2, nullable: false),
			new DmoField("pacTaxNotOver", "money", 11, 2, nullable: false),
			new DmoField("pacTaxAmount", "money", 13, 4, nullable: false),
			new DmoField("pacTaxPercent", "numeric", 8, 4, nullable: false),
			new DmoField("pacCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pacCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pacUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("PACINCOMETAXID,PACINCOMETAXTYPEID,PACINCOMETAXTABLEID,PACINCOMETAXTABLEREVISIONID,PACINCOMETAXTABLESURTAXID", unique: true),
			new DmoIndex("PACUNIQUEID", unique: true),
			new DmoIndex("pacIncomeTaxID", unique: false),
			new DmoIndex("pacIncomeTaxTypeID", unique: false),
			new DmoIndex("pacIncomeTaxTableID", unique: false),
			new DmoIndex("pacIncomeTaxTableRevisionID", unique: false),
			new DmoIndex("pacIncomeTaxTableSurtaxID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
