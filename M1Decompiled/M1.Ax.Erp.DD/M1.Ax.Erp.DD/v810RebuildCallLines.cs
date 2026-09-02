using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert CallLines to support unicode", "2013-10-17")]
public class v810RebuildCallLines
{
	public v810RebuildCallLines(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "CallLines", new DmoField[17]
		{
			new DmoField("kblCallID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kblCallLineID", "smallint", 4, 0, nullable: false),
			new DmoField("kblShortDescription", "nvarchar", 70, 0, nullable: false),
			new DmoField("kblLongDescriptionRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kblLongDescriptionText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("kblContactMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("kblBillable", "bit", 1, 0, nullable: false),
			new DmoField("kblInbound", "bit", 1, 0, nullable: false),
			new DmoField("kblTimeSpent", "numeric", 7, 2, nullable: false),
			new DmoField("kblInternalOnly", "bit", 1, 0, nullable: false),
			new DmoField("kblExtraTime", "numeric", 7, 2, nullable: false),
			new DmoField("kblAddedByEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("kblAddedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kblCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("kblCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("kblCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("kblUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("KBLCALLID,KBLCALLLINEID", unique: true),
			new DmoIndex("KBLUNIQUEID", unique: true),
			new DmoIndex("kblCallID", unique: false),
			new DmoIndex("kblCallLineID", unique: false),
			new DmoIndex("kblContactMethodID", unique: false),
			new DmoIndex("kblInternalOnly", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
