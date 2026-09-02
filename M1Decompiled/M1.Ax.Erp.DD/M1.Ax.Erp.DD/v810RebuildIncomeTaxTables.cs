using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxTables to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxTables
{
	public v810RebuildIncomeTaxTables(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxTables", new DmoField[10]
		{
			new DmoField("pazIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pazIncomeTaxTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pazIncomeTaxTableID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pazDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("pazFilingStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("pazInactive", "bit", 1, 0, nullable: false),
			new DmoField("pazInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("pazCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pazCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pazUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[5]
		{
			new DmoIndex("PAZINCOMETAXID,PAZINCOMETAXTYPEID,PAZINCOMETAXTABLEID", unique: true),
			new DmoIndex("PAZUNIQUEID", unique: true),
			new DmoIndex("pazIncomeTaxID", unique: false),
			new DmoIndex("pazIncomeTaxTypeID", unique: false),
			new DmoIndex("pazIncomeTaxTableID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
