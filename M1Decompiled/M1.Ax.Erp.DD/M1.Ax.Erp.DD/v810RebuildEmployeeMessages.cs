using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert EmployeeMessages to support unicode", "2013-10-17")]
public class v810RebuildEmployeeMessages
{
	public v810RebuildEmployeeMessages(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EmployeeMessages", new DmoField[11]
		{
			new DmoField("lmmEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmmEmployeeMessageID", "int", 7, 0, nullable: false),
			new DmoField("lmmSubject", "nvarchar", 50, 0, nullable: false),
			new DmoField("lmmBodyText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmmBodyRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("lmmSenderEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("lmmSentDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmmStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("lmmCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("lmmCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("lmmUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[6]
		{
			new DmoIndex("LMMEMPLOYEEID,LMMEMPLOYEEMESSAGEID", unique: true),
			new DmoIndex("LMMUNIQUEID", unique: true),
			new DmoIndex("lmmEmployeeID", unique: false),
			new DmoIndex("lmmEmployeeMessageID", unique: false),
			new DmoIndex("lmmSenderEmployeeID", unique: false),
			new DmoIndex("lmmStatus", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
