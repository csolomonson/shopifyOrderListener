using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.Core;

namespace M1.API.Repositories.Core;

public interface ISalesOrderRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesSalesOrderExists(string orderId);

	Task<bool> DoesSalesOrderExists(string orderId, SqlTransaction sqlTransaction);

	Task<bool> DoesEOSalesOrderExists(string orderId);

	Task<bool> DoesEOSalesOrderExists(string orderId, SqlTransaction sqlTransaction);

	Task<bool> DoesEOSalesOrderIDExists(string easyOrderID, out string salesOrderID);

	Task<bool> DoesEOSalesOrderIDExists(string easyOrderID, SqlTransaction sqlTransaction, out string salesOrderID);

	Task<bool> DoesEDISalesOrderExists(string orderId);

	Task<bool> DoesEDISalesOrderExists(string orderId, SqlTransaction sqlTransaction);

	Task<SalesOrderDto> GetSalesOrderInfor(string orderId, bool headerOnly = false);

	Task<SalesOrderDto> GetSalesOrderInfor(string orderId, SqlTransaction sqlTransaction, bool headerOnly = false);

	Task<string> GetSalesOrderList_ForCustomerPO(string customerPO, string orgId, SqlTransaction sqlTransaction);

	Task<byte> GetDefaultSalesOrderDeliveryType();

	Task<SalesOrderLineDto> GetSalesOrderLineInfor(string orderId, short orderLineId);

	Task<bool> SaveSalesOrderHeader(SalesOrderDto salesOrder);

	Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder);

	Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder, SqlTransaction sqlTransaction);

	Task<SaveResponseDto> SaveSalesOrder(SalesOrderDto salesOrder, M1BindingSource salesOrderBs);

	Task<string> CreateEDISalesOrderLog(CTMSalesOrderDto ctmOrder, SqlTransaction sqlTransaction);

	Task<bool> CreateSalesOrderMemo(CTMSalesOrderDto ctmOrder, SqlTransaction sqlTransaction);
}
