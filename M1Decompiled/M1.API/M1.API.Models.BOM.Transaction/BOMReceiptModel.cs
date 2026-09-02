using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core.Transaction;
using M1.API.Utilities;

namespace M1.API.Models.BOM.Transaction;

/// <summary>
/// Implementation of the BOM receipt model.
/// </summary>
public class BOMReceiptModel : BOMBaseModel, IBOMReceiptModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IReceiptRepository receiptRepository { get; set; }

	public Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId, APIClientContext context)
	{
		if (base.ApiClientContext == null)
		{
			base.ApiClientContext = context;
		}
		return ValidateRequest_GetReceipt(receiptId);
	}

	public Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (ReceiptRepository receiptRepository = new ReceiptRepository(base.ApiClientContext))
		{
			if (!receiptRepository.DoesReceiptExists(receiptId).Result)
			{
				list.Add("Receipt [" + receiptId + "] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMReceiptDto>>> Process_GetAllReceipts(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<BOMReceiptDto> allReceiptsDto = new List<BOMReceiptDto>();
		BOMResponseMessageDto<IList<BOMReceiptDto>> result;
		try
		{
			IReceiptRepository receiptRepository = (this.receiptRepository = new ReceiptRepository(base.ApiClientContext));
			using (receiptRepository)
			{
				foreach (ReceiptInformationDto item2 in await this.receiptRepository.GetAllReceipts(pageSize, pageNumber))
				{
					BOMReceiptDto item = new BOMReceiptDto
					{
						ApInvoiceContactID = item2.ApInvoiceContactID,
						ApInvoiceLocationID = item2.ApInvoiceLocationID,
						ClosedDate = item2.ClosedDate,
						ReceiptID = item2.ReceiptID,
						CreatedBy = item2.CreatedBy,
						CreatedDate = item2.CreatedDate,
						CurrencyRateID = item2.CurrencyRateID,
						DeliveryDocket = item2.DeliveryDocket,
						UniqueID = item2.UniqueID,
						ExchangeRate = item2.ExchangeRate,
						FreightCharge = item2.FreightCharge,
						Closed = item2.Closed,
						CustomRate = item2.CustomRate,
						PostedToGl = item2.PostedToGl,
						ReversalEntry = item2.ReversalEntry,
						Reversed = item2.Reversed,
						PlantDepartmentID = item2.PlantDepartmentID,
						PlantID = item2.PlantID,
						PostedDate = item2.PostedDate,
						ProjectID = item2.ProjectID,
						PurchaseLocationID = item2.PurchaseLocationID,
						ReceiptDate = item2.ReceiptDate,
						ReceiptSubtotal = item2.ReceiptSubtotal,
						ReceiptTotal = item2.ReceiptTotal,
						ShippingMethodID = item2.ShippingMethodID,
						SupplierOrganizationID = item2.SupplierOrganizationID,
						RowVersion = item2.RowVersion,
						NestlinkProcessed = item2.NestlinkProcessed
					};
					allReceiptsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Receipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<IList<BOMReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allReceiptsDto
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMReceiptDto>> Process_GetReceipt(string receiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		BOMReceiptDto receiptDto = null;
		BOMResponseMessageDto<BOMReceiptDto> result;
		try
		{
			IReceiptRepository receiptRepository = (this.receiptRepository = new ReceiptRepository(base.ApiClientContext));
			using (receiptRepository)
			{
				ReceiptInformationDto receiptInformationDto = await this.receiptRepository.GetReceiptInfo(receiptId);
				receiptDto = new BOMReceiptDto
				{
					ApInvoiceContactID = receiptInformationDto.ApInvoiceContactID,
					ApInvoiceLocationID = receiptInformationDto.ApInvoiceLocationID,
					ClosedDate = receiptInformationDto.ClosedDate,
					ReceiptID = receiptInformationDto.ReceiptID,
					CreatedBy = receiptInformationDto.CreatedBy,
					CreatedDate = receiptInformationDto.CreatedDate,
					CurrencyRateID = receiptInformationDto.CurrencyRateID,
					DeliveryDocket = receiptInformationDto.DeliveryDocket,
					UniqueID = receiptInformationDto.UniqueID,
					ExchangeRate = receiptInformationDto.ExchangeRate,
					FreightCharge = receiptInformationDto.FreightCharge,
					Closed = receiptInformationDto.Closed,
					CustomRate = receiptInformationDto.CustomRate,
					PostedToGl = receiptInformationDto.PostedToGl,
					ReversalEntry = receiptInformationDto.ReversalEntry,
					Reversed = receiptInformationDto.Reversed,
					PlantDepartmentID = receiptInformationDto.PlantDepartmentID,
					PlantID = receiptInformationDto.PlantID,
					PostedDate = receiptInformationDto.PostedDate,
					ProjectID = receiptInformationDto.ProjectID,
					PurchaseLocationID = receiptInformationDto.PurchaseLocationID,
					ReceiptDate = receiptInformationDto.ReceiptDate,
					ReceiptSubtotal = receiptInformationDto.ReceiptSubtotal,
					ReceiptTotal = receiptInformationDto.ReceiptTotal,
					ShippingMethodID = receiptInformationDto.ShippingMethodID,
					SupplierOrganizationID = receiptInformationDto.SupplierOrganizationID,
					RowVersion = receiptInformationDto.RowVersion,
					NestlinkProcessed = receiptInformationDto.NestlinkProcessed
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Receipt [" + receiptId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new BOMResponseMessageDto<BOMReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = receiptDto
			};
		}
		return result;
	}
}
