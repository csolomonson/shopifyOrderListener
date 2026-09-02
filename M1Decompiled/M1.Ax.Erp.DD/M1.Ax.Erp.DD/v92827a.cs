using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.827", "Create IntegrationTransactionQueue table", "2020-06-18")]
public class v92827a
{
	public v92827a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "IntegrationTransactionQueue"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationTransactionQueue", new DmoField[17]
			{
				new DmoField("itqUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("itqIntegrationType", "nvarchar", 20, 0, nullable: false),
				new DmoField("itqAPIAction", "nvarchar", 20, 0, nullable: false),
				new DmoField("itqEntityType", "nvarchar", 20, 0, nullable: false),
				new DmoField("itqStatus", "nvarchar", 20, 0, nullable: false),
				new DmoField("itqStatusUpdatedDate", "datetime", 14, 0, nullable: true),
				new DmoField("itqNextRetryTime", "datetime", 14, 0, nullable: true),
				new DmoField("itqRetryCount", "tinyint", 1, 0, nullable: false),
				new DmoField("itqErrorCode", "nvarchar", 50, 0, nullable: true),
				new DmoField("itqErrorMessage", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("itqRequest", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("itqResponse", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("itqSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("itqSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("itqParentUniqueID", "uniqueidentifier", 16, 0, nullable: true),
				new DmoField("itqCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("itqCreatedDate", "datetime", 14, 0, nullable: true)
			}, new DmoIndex[6]
			{
				new DmoIndex("itqUniqueID", unique: true),
				new DmoIndex("itqIntegrationType", unique: false),
				new DmoIndex("itqEntityType", unique: false),
				new DmoIndex("itqStatus", unique: false),
				new DmoIndex("itqSourceTableUniqueID", unique: false),
				new DmoIndex("itqParentUniqueID", unique: false)
			});
		}
	}
}
