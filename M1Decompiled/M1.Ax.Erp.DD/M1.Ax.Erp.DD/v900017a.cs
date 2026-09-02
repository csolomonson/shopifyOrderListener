using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.017", "Add fields to SalesOrderComponents and SalesOrderLines tables", "2015-02-10")]
public class v900017a
{
	public v900017a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlDeliveryQuantityTotal"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlDeliveryQuantityTotal", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlUnitPriceForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderLines Set omlUnitPriceForeign = omlFullUnitPriceForeign-omlUnitDiscountForeign");
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlUnitDiscountForeign"))
		{
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SalesOrderLines Set omlUnitDiscountForeign = omlFullUnitPriceForeign * (omlDiscountPercent / 100)");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderComponents", "omoParentQuantity"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderComponents", "omoParentQuantity", "numeric", 15, 5, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update SalesOrderComponents Set omoParentQuantity = omdDeliveryQuantity From SalesOrderDeliveries Inner Join SalesOrderComponents On OMDSALESORDERID = OMOSALESORDERID And OMDSALESORDERLINEID = OMOSALESORDERLINEID And OMDSALESORDERDELIVERYID = OMOSALESORDERDELIVERYID; Update SalesOrderLines Set omlDeliveryQuantityTotal = DetailAmount From SalesOrderLines Inner Join (Select OMDSALESORDERID,OMDSALESORDERLINEID,Sum(omdDeliveryQuantity) As DetailAmount From SalesOrderDeliveries Group By OMDSALESORDERID,OMDSALESORDERLINEID) As DetailTable On OMLSALESORDERID = OMDSALESORDERID And OMLSALESORDERLINEID = OMDSALESORDERLINEID; Update SalesOrderComponents Set omoDeliveryQuantity = Round(omoParentQuantity * omoQuantityPerParent, xadSellQuantityDecimals) + omoAdditionalQuantity From SalesOrderComponents, DatasetProperties Where omoDeliveryQuantity <> Round(omoParentQuantity * omoQuantityPerParent, xadSellQuantityDecimals) + omoAdditionalQuantity; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
