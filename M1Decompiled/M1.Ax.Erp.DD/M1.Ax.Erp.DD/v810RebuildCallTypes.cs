using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CallTypes to support unicode", "2013-10-17")]
public class v810RebuildCallTypes
{
	public v810RebuildCallTypes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallTypes", new DmoField[12]
		{
			new DmoField("kbtCallTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("kbtDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("kbtCallStatus", "nvarchar", 1, 0, nullable: false),
			new DmoField("kbtInboundCall", "bit", 1, 0, nullable: false),
			new DmoField("kbtBillableCall", "bit", 1, 0, nullable: false),
			new DmoField("kbtInternalOnlyCall", "bit", 1, 0, nullable: false),
			new DmoField("kbtInactive", "bit", 1, 0, nullable: false),
			new DmoField("kbtInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("kbtFieldServiceCall", "bit", 1, 0, nullable: false),
			new DmoField("kbtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kbtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kbtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("KBTCALLTYPEID", unique: true),
			new DmoIndex("KBTUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
