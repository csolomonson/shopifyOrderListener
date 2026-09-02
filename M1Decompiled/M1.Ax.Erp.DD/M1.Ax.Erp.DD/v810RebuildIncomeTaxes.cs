using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert IncomeTaxes to support unicode", "2013-10-17")]
public class v810RebuildIncomeTaxes
{
	public v810RebuildIncomeTaxes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IncomeTaxes", new DmoField[14]
		{
			new DmoField("paxIncomeTaxID", "nvarchar", 5, 0, nullable: false),
			new DmoField("paxDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("paxTaxAuthority", "tinyint", 1, 0, nullable: false),
			new DmoField("paxCountry", "nvarchar", 3, 0, nullable: false),
			new DmoField("paxState", "nvarchar", 3, 0, nullable: false),
			new DmoField("paxLocalityName", "nvarchar", 10, 0, nullable: false),
			new DmoField("paxExternalUnemploymentID", "nvarchar", 15, 0, nullable: false),
			new DmoField("paxExternalTaxID", "nvarchar", 15, 0, nullable: false),
			new DmoField("paxOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("paxInactive", "bit", 1, 0, nullable: false),
			new DmoField("paxInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("paxCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("paxCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("paxUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("PAXINCOMETAXID", unique: true),
			new DmoIndex("PAXUNIQUEID", unique: true),
			new DmoIndex("paxOrganizationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
