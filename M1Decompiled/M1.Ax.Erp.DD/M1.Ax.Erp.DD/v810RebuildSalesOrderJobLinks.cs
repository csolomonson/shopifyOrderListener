using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert SalesOrderJobLinks to support unicode", "2013-10-17")]
public class v810RebuildSalesOrderJobLinks
{
	public v810RebuildSalesOrderJobLinks(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderJobLinks", new DmoField[10]
		{
			new DmoField("omjSalesOrderID", "nvarchar", 10, 0, nullable: false),
			new DmoField("omjSalesOrderLineID", "smallint", 4, 0, nullable: false),
			new DmoField("omjSalesOrderJobLinkID", "int", 5, 0, nullable: false),
			new DmoField("omjLinkType", "tinyint", 1, 0, nullable: false),
			new DmoField("omjSalesOrderDeliveryID", "smallint", 4, 0, nullable: false),
			new DmoField("omjJobID", "nvarchar", 20, 0, nullable: false),
			new DmoField("omjClosed", "bit", 1, 0, nullable: false),
			new DmoField("omjCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("omjCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("omjUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[7]
		{
			new DmoIndex("OMJSALESORDERID,OMJSALESORDERLINEID,OMJSALESORDERJOBLINKID", unique: true),
			new DmoIndex("OMJUNIQUEID", unique: true),
			new DmoIndex("omjSalesOrderID", unique: false),
			new DmoIndex("omjSalesOrderLineID", unique: false),
			new DmoIndex("omjSalesOrderJobLinkID", unique: false),
			new DmoIndex("omjJobID", unique: false),
			new DmoIndex("omjClosed", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
