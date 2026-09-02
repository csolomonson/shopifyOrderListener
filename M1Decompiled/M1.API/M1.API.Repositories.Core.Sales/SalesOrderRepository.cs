using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Sales;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Sales;

public class SalesOrderRepository : APIBaseRepository, ISalesOrderRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] salesOrderFields = new string[37]
	{
		"ompSalesOrderID", "ompCustomerOrganizationID", "ompShipOrganizationID", "ompCustomerPO", "ompPlantID", "ompPlantDepartmentID", "ompShipLocationID", "ompShipContactID", "ompARInvoiceContactID", "ompARInvoiceLocationID",
		"ompPaymentTermID", "ompCurrencyRateID", "ompExchangeRate", "ompFullOrderSubtotalBase", "ompRequestedShipDate", "ompOrderDate", "ompStatus", "ompOrderTotalForeign", "ompOrderTotalBase", "ompCustomRate",
		"ompClosed", "ompClosedDate", "ompFreightAmountBase", "ompFreightAmountForeign", "ompFreightTaxAmountBase", "ompFreightTaxAmountForeign", "ompFreightTaxCodeID", "ompFreightTotalBase", "ompFreightTotalForeign", "ompOrderSubtotalBase",
		"ompFullOrderSubtotalForeign", "ompOrderTaxAmountBase", "ompOrderTaxAmountForeign", "ompShippingPaymentTypeID", "ompShippingMethodID", "ompTotalOrderWeight", "ompOrderSubTotalForeign"
	};

	public SalesOrderRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesSalesOrderExists(string salesOrderId)
	{
		InitializeParameterLists();
		base.filterList.Add("ompSalesOrderID|C", salesOrderId);
		base.selectList.Add("ompSalesOrderID");
		return Task.FromResult(GetAsObject("SalesOrders", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMSalesOrderDto>> GetAllSalesOrders(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMSalesOrderDto> collection = new List<BOMSalesOrderDto>();
		InitializeParameterLists();
		base.selectList.AddRange(salesOrderFields);
		List<string> orderbyList = new List<string> { "ompSalesOrderID" };
		using (DataTable dataTable = GetAsDataTable("SalesOrders", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMSalesOrderDto bOMSalesOrderDto = new BOMSalesOrderDto();
				bOMSalesOrderDto.SalesOrderID = dataTable.Rows[i].Field<string>("ompSalesOrderID");
				bOMSalesOrderDto.CustomerOrganizationID = dataTable.Rows[i].Field<string>("ompCustomerOrganizationID");
				bOMSalesOrderDto.ShipOrganizationID = dataTable.Rows[i].Field<string>("ompShipOrganizationID");
				bOMSalesOrderDto.CustomerPo = dataTable.Rows[i].Field<string>("ompCustomerPo");
				bOMSalesOrderDto.PlantDepartmentID = dataTable.Rows[i].Field<string>("ompPlantDepartmentID");
				bOMSalesOrderDto.PlantID = dataTable.Rows[i].Field<string>("ompPlantID");
				bOMSalesOrderDto.ShipLocationID = dataTable.Rows[i].Field<string>("ompShipLocationID");
				bOMSalesOrderDto.ShipContactID = dataTable.Rows[i].Field<string>("ompShipContactID");
				bOMSalesOrderDto.ArInvoiceLocationID = dataTable.Rows[i].Field<string>("ompArInvoiceLocationID");
				bOMSalesOrderDto.ArInvoiceContactID = dataTable.Rows[i].Field<string>("ompArInvoiceContactID");
				bOMSalesOrderDto.PaymentTermID = dataTable.Rows[i].Field<string>("ompPaymentTermID");
				bOMSalesOrderDto.CurrencyRateID = dataTable.Rows[i].Field<string>("ompCurrencyRateID");
				bOMSalesOrderDto.ExchangeRate = dataTable.Rows[i].Field<decimal>("ompExchangeRate");
				bOMSalesOrderDto.FullOrderSubtotalBase = dataTable.Rows[i].Field<decimal>("ompFullOrderSubtotalBase");
				bOMSalesOrderDto.RequestedShipDate = dataTable.Rows[i].Field<DateTime?>("ompRequestedShipDate");
				bOMSalesOrderDto.OrderDate = dataTable.Rows[i].Field<DateTime?>("ompOrderDate");
				bOMSalesOrderDto.OrderTotalBase = dataTable.Rows[i].Field<decimal>("ompOrderTotalBase");
				bOMSalesOrderDto.OrderTotalForeign = dataTable.Rows[i].Field<decimal>("ompOrderTotalForeign");
				bOMSalesOrderDto.Status = dataTable.Rows[i].Field<byte>("ompStatus");
				bOMSalesOrderDto.CustomRate = dataTable.Rows[i].Field<bool>("ompCustomRate");
				bOMSalesOrderDto.Closed = dataTable.Rows[i].Field<bool>("ompClosed");
				bOMSalesOrderDto.ClosedDate = dataTable.Rows[i].Field<DateTime?>("ompClosedDate");
				bOMSalesOrderDto.FreightAmountBase = dataTable.Rows[i].Field<decimal>("ompFreightAmountBase");
				bOMSalesOrderDto.FreightAmountForeign = dataTable.Rows[i].Field<decimal>("ompFreightAmountForeign");
				bOMSalesOrderDto.FreightTaxAmountBase = dataTable.Rows[i].Field<decimal>("ompFreightTaxAmountBase");
				bOMSalesOrderDto.FreightTaxAmountForeign = dataTable.Rows[i].Field<decimal>("ompFreightTaxAmountForeign");
				bOMSalesOrderDto.FreightTaxCodeID = dataTable.Rows[i].Field<string>("ompFreightTaxCodeID");
				bOMSalesOrderDto.FreightTotalBase = dataTable.Rows[i].Field<decimal>("ompFreightTotalBase");
				bOMSalesOrderDto.FreightTotalForeign = dataTable.Rows[i].Field<decimal>("ompFreightTotalForeign");
				bOMSalesOrderDto.OrderSubtotalBase = dataTable.Rows[i].Field<decimal>("ompOrderSubtotalBase");
				bOMSalesOrderDto.OrderSubTotalForeign = dataTable.Rows[i].Field<decimal>("ompOrderSubTotalForeign");
				bOMSalesOrderDto.OrderTaxAmountBase = dataTable.Rows[i].Field<decimal>("ompOrderTaxAmountBase");
				bOMSalesOrderDto.OrderTaxAmountForeign = dataTable.Rows[i].Field<decimal>("ompOrderTaxAmountForeign");
				bOMSalesOrderDto.ShippingMethodID = dataTable.Rows[i].Field<string>("ompShippingMethodID");
				bOMSalesOrderDto.ShippingPaymentTypeID = dataTable.Rows[i].Field<string>("ompShippingPaymentTypeID");
				bOMSalesOrderDto.TotalOrderWeight = dataTable.Rows[i].Field<decimal>("ompTotalOrderWeight");
				collection.Add(bOMSalesOrderDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMSalesOrderDto> GetSalesOrder(string salesOrderId)
	{
		BOMSalesOrderDto bOMSalesOrderDto = new BOMSalesOrderDto();
		InitializeParameterLists();
		base.selectList.AddRange(salesOrderFields);
		base.filterList.Add(Guid.TryParse(salesOrderId, out var _) ? "ompUniqueID|C" : "ompSalesOrderID|C", salesOrderId);
		using (DataTable dataTable = GetAsDataTable("SalesOrders", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMSalesOrderDto);
			}
			bOMSalesOrderDto.SalesOrderID = dataTable.Rows[0].Field<string>("ompSalesOrderID");
			bOMSalesOrderDto.CustomerOrganizationID = dataTable.Rows[0].Field<string>("ompCustomerOrganizationID");
			bOMSalesOrderDto.ShipOrganizationID = dataTable.Rows[0].Field<string>("ompShipOrganizationID");
			bOMSalesOrderDto.CustomerPo = dataTable.Rows[0].Field<string>("ompCustomerPo");
			bOMSalesOrderDto.PlantDepartmentID = dataTable.Rows[0].Field<string>("ompPlantDepartmentID");
			bOMSalesOrderDto.PlantID = dataTable.Rows[0].Field<string>("ompPlantID");
			bOMSalesOrderDto.ShipLocationID = dataTable.Rows[0].Field<string>("ompShipLocationID");
			bOMSalesOrderDto.ShipContactID = dataTable.Rows[0].Field<string>("ompShipContactID");
			bOMSalesOrderDto.ArInvoiceLocationID = dataTable.Rows[0].Field<string>("ompArInvoiceLocationID");
			bOMSalesOrderDto.ArInvoiceContactID = dataTable.Rows[0].Field<string>("ompArInvoiceContactID");
			bOMSalesOrderDto.PaymentTermID = dataTable.Rows[0].Field<string>("ompPaymentTermID");
			bOMSalesOrderDto.CurrencyRateID = dataTable.Rows[0].Field<string>("ompCurrencyRateID");
			bOMSalesOrderDto.ExchangeRate = dataTable.Rows[0].Field<decimal>("ompExchangeRate");
			bOMSalesOrderDto.FullOrderSubtotalBase = dataTable.Rows[0].Field<decimal>("ompFullOrderSubtotalBase");
			bOMSalesOrderDto.RequestedShipDate = dataTable.Rows[0].Field<DateTime?>("ompRequestedShipDate");
			bOMSalesOrderDto.OrderDate = dataTable.Rows[0].Field<DateTime?>("ompOrderDate");
			bOMSalesOrderDto.OrderTotalBase = dataTable.Rows[0].Field<decimal>("ompOrderTotalBase");
			bOMSalesOrderDto.OrderTotalForeign = dataTable.Rows[0].Field<decimal>("ompOrderTotalForeign");
			bOMSalesOrderDto.Status = dataTable.Rows[0].Field<byte>("ompStatus");
			bOMSalesOrderDto.CustomRate = dataTable.Rows[0].Field<bool>("ompCustomRate");
			bOMSalesOrderDto.Closed = dataTable.Rows[0].Field<bool>("ompClosed");
			bOMSalesOrderDto.ClosedDate = dataTable.Rows[0].Field<DateTime?>("ompClosedDate");
			bOMSalesOrderDto.FreightAmountBase = dataTable.Rows[0].Field<decimal>("ompFreightAmountBase");
			bOMSalesOrderDto.FreightAmountForeign = dataTable.Rows[0].Field<decimal>("ompFreightAmountForeign");
			bOMSalesOrderDto.FreightTaxAmountBase = dataTable.Rows[0].Field<decimal>("ompFreightTaxAmountBase");
			bOMSalesOrderDto.FreightTaxAmountForeign = dataTable.Rows[0].Field<decimal>("ompFreightTaxAmountForeign");
			bOMSalesOrderDto.FreightTaxCodeID = dataTable.Rows[0].Field<string>("ompFreightTaxCodeID");
			bOMSalesOrderDto.FreightTotalBase = dataTable.Rows[0].Field<decimal>("ompFreightTotalBase");
			bOMSalesOrderDto.FreightTotalForeign = dataTable.Rows[0].Field<decimal>("ompFreightTotalForeign");
			bOMSalesOrderDto.OrderSubtotalBase = dataTable.Rows[0].Field<decimal>("ompOrderSubtotalBase");
			bOMSalesOrderDto.OrderSubTotalForeign = dataTable.Rows[0].Field<decimal>("ompOrderSubTotalForeign");
			bOMSalesOrderDto.OrderTaxAmountBase = dataTable.Rows[0].Field<decimal>("ompOrderTaxAmountBase");
			bOMSalesOrderDto.OrderTaxAmountForeign = dataTable.Rows[0].Field<decimal>("ompOrderTaxAmountForeign");
			bOMSalesOrderDto.ShippingMethodID = dataTable.Rows[0].Field<string>("ompShippingMethodID");
			bOMSalesOrderDto.ShippingPaymentTypeID = dataTable.Rows[0].Field<string>("ompShippingPaymentTypeID");
			bOMSalesOrderDto.TotalOrderWeight = dataTable.Rows[0].Field<decimal>("ompTotalOrderWeight");
		}
		return Task.FromResult(bOMSalesOrderDto);
	}
}
