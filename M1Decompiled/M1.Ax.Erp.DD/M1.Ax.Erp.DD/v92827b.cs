using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.827", "Create IntegrationCrossReferences table", "2020-06-18")]
public class v92827b
{
	public v92827b(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "IntegrationCrossReferences"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "IntegrationCrossReferences", new DmoField[10]
			{
				new DmoField("icrUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("icrIntegrationType", "nvarchar", 20, 0, nullable: false),
				new DmoField("icrSourceTableName", "nvarchar", 30, 0, nullable: false),
				new DmoField("icrSourceTableUniqueID", "uniqueidentifier", 16, 0, nullable: false),
				new DmoField("icrAPIType", "nvarchar", 20, 0, nullable: false),
				new DmoField("icrIntegrationID", "nvarchar", 20, 0, nullable: false),
				new DmoField("icrIntegrationSyncToken", "smallint", 4, 0, nullable: false),
				new DmoField("icrTransactionSourceUniqueID", "uniqueidentifier", 16, 0, nullable: true),
				new DmoField("icrCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("icrCreatedDate", "datetime", 14, 0, nullable: true)
			}, new DmoIndex[5]
			{
				new DmoIndex("icrUniqueID", unique: true),
				new DmoIndex("icrIntegrationType", unique: false),
				new DmoIndex("icrSourceTableUniqueID", unique: false),
				new DmoIndex("icrIntegrationID", unique: false),
				new DmoIndex("icrTransactionSourceUniqueID", unique: false)
			});
		}
	}
}
