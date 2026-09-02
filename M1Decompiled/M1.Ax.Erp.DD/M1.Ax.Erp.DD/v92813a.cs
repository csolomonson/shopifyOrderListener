using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.2.813", "Update SalesOrderComponents Delivery Quantity", "2019-02-11")]
public class v92813a
{
	public v92813a(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderComponents", "omoDeliveryQuantity"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderComponents Set omoDeliveryQuantity = Round(omoParentQuantity*omoQuantityPerParent, xadSellQuantityDecimals) + omoAdditionalQuantity From SalesOrderComponents, DatasetProperties Where omoDeliveryQuantity <> Round(omoParentQuantity*omoQuantityPerParent, xadSellQuantityDecimals) + omoAdditionalQuantity");
		}
	}
}
