using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.683", "Add fields to EDISalesOrderChangeLog table", "2018-04-16")]
public class v92683a
{
	public v92683a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "EDISalesOrderChangeLog"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "EDISalesOrderChangeLog", new DmoField[11]
			{
				new DmoField("omeChangeLogID", "identity", 4, 0, nullable: false),
				new DmoField("omeCustomerPO", "nvarchar", 40, 0, nullable: false),
				new DmoField("omeCustomerPOLineID", "smallint", 10, 0, nullable: false),
				new DmoField("omeChangeType", "nchar", 1, 0, nullable: false),
				new DmoField("omeChangeRequestDate", "datetime", 14, 0, nullable: true),
				new DmoField("omeTableNewValues", "nvarchar(max)", 50, 0, nullable: true),
				new DmoField("omeSalesOrderIDsText", "nvarchar", 200, 0, nullable: false),
				new DmoField("omeVerifyStatus", "bit", 1, 0, nullable: false),
				new DmoField("omeVerifiedBy", "nvarchar", 20, 0, nullable: true),
				new DmoField("omeVerifiedDate", "datetime", 14, 0, nullable: true),
				new DmoField("omeUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[2]
			{
				new DmoIndex("omeChangeLogID", unique: true),
				new DmoIndex("omeUniqueID", unique: true)
			});
		}
	}
}
