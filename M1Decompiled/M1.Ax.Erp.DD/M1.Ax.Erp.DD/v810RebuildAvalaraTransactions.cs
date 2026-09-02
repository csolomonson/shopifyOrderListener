using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert AvalaraTransactions to support unicode", "2013-10-17")]
public class v810RebuildAvalaraTransactions
{
	public v810RebuildAvalaraTransactions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AvalaraTransactions", new DmoField[13]
		{
			new DmoField("avtAvalaraTransactionID", "numeric", 10, 0, nullable: false),
			new DmoField("avtSourceTable", "nvarchar", 30, 0, nullable: false),
			new DmoField("avtSourceTableKeyFields", "nvarchar", 50, 0, nullable: false),
			new DmoField("avtTransactionDate", "datetime", 14, 0, nullable: true),
			new DmoField("avtTransactionType", "tinyint", 1, 0, nullable: false),
			new DmoField("avtTransactionID", "nvarchar", 30, 0, nullable: false),
			new DmoField("avtResultCode", "tinyint", 1, 0, nullable: false),
			new DmoField("avtMessageSummary", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("avtMessageDetail", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("avtReferenceCode", "nvarchar", 50, 0, nullable: true),
			new DmoField("avtCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("avtCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("avtUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[2]
		{
			new DmoIndex("AVTAVALARATRANSACTIONID", unique: true),
			new DmoIndex("AVTUNIQUEID", unique: true)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
