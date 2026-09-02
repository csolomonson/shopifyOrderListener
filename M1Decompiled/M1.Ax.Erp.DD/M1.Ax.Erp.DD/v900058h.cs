using System.Text;
using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.058", "Update field bindings", "2015-07-09")]
public class v900058h
{
	public v900058h(DBConversionParms parms)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("begin SET NOCOUNT ON DECLARE @SQL3 varchar(8000) set @sql3 = ' Update WarehouseRequisitionComponents Set wqoParentQuantity = wqlRequestedQuantity From WarehouseRequisitionLines Inner Join WarehouseRequisitionComponents On WQLWAREHOUSEREQUISITIONID = WQOWAREHOUSEREQUISITIONID And WQLWAREHOUSEREQUISITIONLINEID = WQOWAREHOUSEREQUISITIONLINEID; Update WarehouseTransferComponents Set mwoDestinationWarehouseID = mwlDestinationWarehouseID From WarehouseTransferLines Inner Join WarehouseTransferComponents On MWLWAREHOUSETRANSFERID = MWOWAREHOUSETRANSFERID And MWLWAREHOUSETRANSFERLINEID = MWOWAREHOUSETRANSFERLINEID; Update WarehouseTransferComponents Set mwoParentQuantity = mwlShipQuantity From WarehouseTransferLines Inner Join WarehouseTransferComponents On MWLWAREHOUSETRANSFERID = MWOWAREHOUSETRANSFERID And MWLWAREHOUSETRANSFERLINEID = MWOWAREHOUSETRANSFERLINEID; Update WarehouseTransferLines Set mwlDestinationWarehouseID = mwpDestinationWarehouseID From WarehouseTransfers Inner Join WarehouseTransferLines On MWPWAREHOUSETRANSFERID = MWLWAREHOUSETRANSFERID; Update WarehouseReceiptComponents Set wroParentQuantity = wrlQuantityReceived From WarehouseReceiptLines Inner Join WarehouseReceiptComponents On WRLWAREHOUSERECEIPTID = WROWAREHOUSERECEIPTID And WRLWAREHOUSERECEIPTLINEID = WROWAREHOUSERECEIPTLINEID; ' exec(@sql3) SET NOCOUNT OFF end;");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, stringBuilder.ToString());
	}
}
