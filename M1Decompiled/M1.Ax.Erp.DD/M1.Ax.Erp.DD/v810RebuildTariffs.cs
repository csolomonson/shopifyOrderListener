using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Tariffs to support unicode", "2013-10-17")]
public class v810RebuildTariffs
{
	public v810RebuildTariffs(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Tariffs", new DmoField[8]
		{
			new DmoField("rmtTariffID", "nvarchar", 5, 0, nullable: false),
			new DmoField("rmtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("rmtPercentage", "numeric", 6, 2, nullable: false),
			new DmoField("rmtInactive", "bit", 1, 0, nullable: false),
			new DmoField("rmtInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("rmtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("rmtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("rmtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[3]
		{
			new DmoIndex("RMTTARIFFID", unique: true),
			new DmoIndex("RMTUNIQUEID", unique: true),
			new DmoIndex("rmtInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
