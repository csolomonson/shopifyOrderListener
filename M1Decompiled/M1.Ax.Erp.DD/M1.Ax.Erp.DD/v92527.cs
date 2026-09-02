using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.527", "WebECommerce Table for PunchOut EasyOrder", "2017-09-27")]
public class v92527
{
	public v92527(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "WebECommerce"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "WebECommerce", new DmoField[19]
			{
				new DmoField("wecECommerceID", "int", 9, 0, nullable: false),
				new DmoField("wecOperation", "nvarchar", 10, 0, nullable: false),
				new DmoField("wecPayloadID", "nvarchar", 100, 0, nullable: false),
				new DmoField("wecRelayingIdentity", "nvarchar", 20, 0, nullable: false),
				new DmoField("wecDestinationIdentity", "nvarchar", 20, 0, nullable: false),
				new DmoField("wecOriginatorIdentity", "nvarchar", 20, 0, nullable: false),
				new DmoField("wecBuyerCookie", "nvarchar", 120, 0, nullable: false),
				new DmoField("wecSignature", "text", 50, 0, nullable: false),
				new DmoField("wecHashURL", "text", 50, 0, nullable: false),
				new DmoField("wecEasyOrderID", "nvarchar", 50, 0, nullable: false),
				new DmoField("wecSalesOrderLineID", "numeric", 4, 0, nullable: false),
				new DmoField("wecPartID", "nvarchar", 30, 0, nullable: false),
				new DmoField("wecPartRevisionID", "nvarchar", 15, 0, nullable: false),
				new DmoField("wecOrderQuantity", "numeric", 15, 5, nullable: false),
				new DmoField("wecOrderMessageURL", "text", 50, 0, nullable: false),
				new DmoField("wecReturnURL", "text", 50, 0, nullable: false),
				new DmoField("wecCreatedBy", "nvarchar", 20, 0, nullable: false),
				new DmoField("wecCreatedDate", "datetime", 14, 0, nullable: false),
				new DmoField("wecUniqueID", "uniqueidentifier", 16, 0, nullable: false)
			}, new DmoIndex[1]
			{
				new DmoIndex("wecUniqueID", unique: true)
			});
		}
	}
}
