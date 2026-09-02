using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Sales;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core.Sales;

namespace M1.API.Models.BOM.Sales;

public class BOMSalesOrderModel : BOMBaseModel, IBOMSalesOrderModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrder(string salesOrderId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (SalesOrderRepository salesOrderRepository = new SalesOrderRepository(base.ApiClientContext))
		{
			if (!salesOrderRepository.DoesSalesOrderExists(salesOrderId).Result)
			{
				list.Add("SalesOrder [salesOrderId] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PostSalesOrder(BOMSalesOrderDto salesOrder)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMSalesOrderDto>>> Process_GetAllSalesOrders(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMSalesOrderDto> allSalesOrdersDto = new List<BOMSalesOrderDto>();
		BOMResponseMessageDto<IList<BOMSalesOrderDto>> result;
		try
		{
			using SalesOrderRepository salesOrderRepository = new SalesOrderRepository(base.ApiClientContext);
			foreach (BOMSalesOrderDto item2 in await salesOrderRepository.GetAllSalesOrders(pageSize, pageNumber))
			{
				BOMSalesOrderDto item = new BOMSalesOrderDto
				{
					SalesOrderID = item2.SalesOrderID,
					CustomerOrganizationID = item2.CustomerOrganizationID,
					ShipOrganizationID = item2.ShipOrganizationID,
					CustomerPo = item2.CustomerPo,
					PlantID = item2.PlantID,
					PlantDepartmentID = item2.PlantDepartmentID,
					ShipLocationID = item2.ShipLocationID,
					ShipContactID = item2.ShipContactID,
					ArInvoiceContactID = item2.ArInvoiceContactID,
					ArInvoiceLocationID = item2.ArInvoiceLocationID,
					PaymentTermID = item2.PaymentTermID,
					CurrencyRateID = item2.CurrencyRateID,
					ExchangeRate = item2.ExchangeRate,
					FullOrderSubtotalBase = item2.FullOrderSubtotalBase,
					RequestedShipDate = item2.RequestedShipDate,
					OrderDate = item2.OrderDate,
					OrderTotalBase = item2.OrderTotalBase,
					OrderTotalForeign = item2.OrderTotalForeign,
					Status = item2.Status,
					CustomRate = item2.CustomRate,
					Closed = item2.Closed,
					ClosedDate = item2.ClosedDate,
					FreightAmountBase = item2.FreightAmountBase,
					FreightAmountForeign = item2.FreightAmountForeign,
					FreightTaxAmountBase = item2.FreightTaxAmountBase,
					FreightTaxAmountForeign = item2.FreightTaxAmountForeign,
					FreightTaxCodeID = item2.FreightTaxCodeID,
					FreightTotalBase = item2.FreightTotalBase,
					FreightTotalForeign = item2.FreightTotalForeign,
					OrderSubtotalBase = item2.OrderSubtotalBase,
					OrderSubTotalForeign = item2.OrderSubTotalForeign,
					OrderTaxAmountBase = item2.OrderTaxAmountBase,
					OrderTaxAmountForeign = item2.OrderTaxAmountForeign,
					ShippingMethodID = item2.ShippingMethodID,
					ShippingPaymentTypeID = item2.ShippingPaymentTypeID,
					TotalOrderWeight = item2.TotalOrderWeight
				};
				allSalesOrdersDto.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrders]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMSalesOrderDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrdersDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMSalesOrderDto>> Process_GetSalesOrder(string salesOrderId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMSalesOrderDto salesOrderDto = null;
		BOMResponseMessageDto<BOMSalesOrderDto> result;
		try
		{
			using SalesOrderRepository salesOrderRepository = new SalesOrderRepository(base.ApiClientContext);
			BOMSalesOrderDto bOMSalesOrderDto = await salesOrderRepository.GetSalesOrder(salesOrderId);
			salesOrderDto = new BOMSalesOrderDto
			{
				SalesOrderID = bOMSalesOrderDto.SalesOrderID,
				CustomerOrganizationID = bOMSalesOrderDto.CustomerOrganizationID,
				ShipOrganizationID = bOMSalesOrderDto.ShipOrganizationID,
				CustomerPo = bOMSalesOrderDto.CustomerPo,
				PlantID = bOMSalesOrderDto.PlantID,
				PlantDepartmentID = bOMSalesOrderDto.PlantDepartmentID,
				ShipLocationID = bOMSalesOrderDto.ShipLocationID,
				ShipContactID = bOMSalesOrderDto.ShipContactID,
				ArInvoiceContactID = bOMSalesOrderDto.ArInvoiceContactID,
				ArInvoiceLocationID = bOMSalesOrderDto.ArInvoiceLocationID,
				PaymentTermID = bOMSalesOrderDto.PaymentTermID,
				CurrencyRateID = bOMSalesOrderDto.CurrencyRateID,
				ExchangeRate = bOMSalesOrderDto.ExchangeRate,
				FullOrderSubtotalBase = bOMSalesOrderDto.FullOrderSubtotalBase,
				RequestedShipDate = bOMSalesOrderDto.RequestedShipDate,
				OrderDate = bOMSalesOrderDto.OrderDate,
				OrderTotalBase = bOMSalesOrderDto.OrderTotalBase,
				OrderTotalForeign = bOMSalesOrderDto.OrderTotalForeign,
				Status = bOMSalesOrderDto.Status,
				CustomRate = bOMSalesOrderDto.CustomRate,
				Closed = bOMSalesOrderDto.Closed,
				ClosedDate = bOMSalesOrderDto.ClosedDate,
				FreightAmountBase = bOMSalesOrderDto.FreightAmountBase,
				FreightAmountForeign = bOMSalesOrderDto.FreightAmountForeign,
				FreightTaxAmountBase = bOMSalesOrderDto.FreightTaxAmountBase,
				FreightTaxAmountForeign = bOMSalesOrderDto.FreightTaxAmountForeign,
				FreightTaxCodeID = bOMSalesOrderDto.FreightTaxCodeID,
				FreightTotalBase = bOMSalesOrderDto.FreightTotalBase,
				FreightTotalForeign = bOMSalesOrderDto.FreightTotalForeign,
				OrderSubtotalBase = bOMSalesOrderDto.OrderSubtotalBase,
				OrderSubTotalForeign = bOMSalesOrderDto.OrderSubTotalForeign,
				OrderTaxAmountBase = bOMSalesOrderDto.OrderTaxAmountBase,
				OrderTaxAmountForeign = bOMSalesOrderDto.OrderTaxAmountForeign,
				ShippingMethodID = bOMSalesOrderDto.ShippingMethodID,
				ShippingPaymentTypeID = bOMSalesOrderDto.ShippingPaymentTypeID,
				TotalOrderWeight = bOMSalesOrderDto.TotalOrderWeight
			};
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrders []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMSalesOrderDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderDto
			};
		}
		return result;
	}
}
