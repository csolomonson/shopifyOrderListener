using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.Custom.Transaction;
using M1.API.Repositories.Core.Transaction;

namespace M1.API.Models.BOM.Transaction;

public class BOMReceiptLineModel : BOMBaseModel, IBOMReceiptLineModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	private readonly IBOMReceiptModel _bomReceiptModel;

	private IReceiptRepository receiptRepository { get; set; }

	private IReceiptLineRepository receiptLineRepository { get; set; }

	public BOMReceiptLineModel()
	{
	}

	public BOMReceiptLineModel(IBOMReceiptModel bomReceiptModel)
	{
		_bomReceiptModel = bomReceiptModel;
	}

	public async Task<BOMResponseMessageDto<CTMBOMReceiptLineDto>> Process_GetReceiptLine(string receiptId)
	{
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMBOMReceiptLineDto cTMBOMReceiptLineDto = new CTMBOMReceiptLineDto();
		new BOMResponseMessageDto<CTMBOMReceiptLineDto>();
		BOMResponseMessageDto<CTMBOMReceiptLineDto> result3;
		try
		{
			IReceiptRepository receiptRepository = (this.receiptRepository = new ReceiptRepository(base.ApiClientContext));
			using (receiptRepository)
			{
				ReceiptInformationDto result = this.receiptRepository.GetReceiptInfo(receiptId).Result;
				IList<ReceiptLineInformationDto> result2 = this.receiptRepository.GetReceiptLineInfo(receiptId).Result;
				cTMBOMReceiptLineDto.Receipt = new BOMReceiptDto
				{
					ApInvoiceContactID = result.ApInvoiceContactID,
					ApInvoiceLocationID = result.ApInvoiceLocationID,
					ClosedDate = result.ClosedDate,
					ReceiptID = result.ReceiptID,
					CreatedBy = result.CreatedBy,
					CreatedDate = result.CreatedDate,
					CurrencyRateID = result.CurrencyRateID,
					DeliveryDocket = result.DeliveryDocket,
					UniqueID = result.UniqueID,
					ExchangeRate = result.ExchangeRate,
					FreightCharge = result.FreightCharge,
					Closed = result.Closed,
					CustomRate = result.CustomRate,
					PostedToGl = result.PostedToGl,
					ReversalEntry = result.ReversalEntry,
					Reversed = result.Reversed,
					PlantDepartmentID = result.PlantDepartmentID,
					PlantID = result.PlantID,
					PostedDate = result.PostedDate,
					ProjectID = result.ProjectID,
					PurchaseLocationID = result.PurchaseLocationID,
					ReceiptDate = result.ReceiptDate,
					ReceiptSubtotal = result.ReceiptSubtotal,
					ReceiptTotal = result.ReceiptTotal,
					ShippingMethodID = result.ShippingMethodID,
					SupplierOrganizationID = result.SupplierOrganizationID,
					RowVersion = result.RowVersion,
					NestlinkProcessed = result.NestlinkProcessed
				};
				foreach (ReceiptLineInformationDto item in result2)
				{
					cTMBOMReceiptLineDto.ReceiptLines.Add(new BOMReceiptLineDto
					{
						ConversionFactor = item.ConversionFactor,
						CreatedBy = item.CreatedBy,
						CreatedDate = item.CreatedDate,
						Description = item.Description,
						UniqueID = item.UniqueID,
						HeatLot = item.HeatLot,
						InventoryUnitOfMeasure = item.InventoryUnitOfMeasure,
						Closed = item.Closed,
						JobReceivedComplete = item.JobReceivedComplete,
						PoReceivedComplete = item.PoReceivedComplete,
						PostedToGl = item.PostedToGl,
						RequiresInspection = item.RequiresInspection,
						Reversed = item.Reversed,
						JobAssemblyID = item.JobAssemblyID,
						JobID = item.JobID,
						JobMaterialID = item.JobMaterialID,
						JobMatQuantityReceived = item.JobMatQuantityReceived,
						JobOperationID = item.JobOperationID,
						JobOprQuantityReceived = item.JobOprQuantityReceived,
						JobType = item.JobType,
						OrgPartID = item.OrgPartID,
						OrgPartShortDescription = item.OrgPartShortDescription,
						PartBinID = item.PartBinID,
						PartID = item.PartID,
						PartRevisionID = item.PartRevisionID,
						PartWarehouseLocationID = item.PartWarehouseLocationID,
						ProjectAreaID = item.ProjectAreaID,
						ProjectID = item.ProjectID,
						PurchaseOrderID = item.PurchaseOrderID,
						PurchaseOrderLineID = item.PurchaseOrderLineID,
						PurchaseQuantityReceived = item.PurchaseQuantityReceived,
						PurchaseUnitCost = item.PurchaseUnitCost,
						PurchaseUnitOfMeasure = item.PurchaseUnitOfMeasure,
						ReceiptID = item.ReceiptID,
						Reference = item.Reference,
						ReverseReceiptID = item.ReverseReceiptID,
						ReverseReceiptLineID = item.ReverseReceiptLineID,
						ReceiptLineID = item.ReceiptLineID,
						SetupCharge = item.SetupCharge,
						RowVersion = item.RowVersion
					});
				}
			}
			IList<string> errorsList = base.ErrorsList;
			if (errorsList != null && errorsList.Count > 0)
			{
				httpValidationStatusCode = HttpStatusCode.BadRequest;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Receipt [" + receiptId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result3 = new BOMResponseMessageDto<CTMBOMReceiptLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cTMBOMReceiptLineDto
			};
		}
		return result3;
	}

	public async Task<BOMResponseMessageDto<IList<BOMReceiptLineDto>>> Process_GetAllReceiptLines(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMReceiptLineDto> list = new List<BOMReceiptLineDto>();
		BOMResponseMessageDto<IList<BOMReceiptLineDto>> result;
		try
		{
			IReceiptLineRepository receiptLineRepository = (this.receiptLineRepository = new ReceiptLineRepository(base.ApiClientContext));
			using (receiptLineRepository)
			{
				foreach (ReceiptLineDto item2 in this.receiptLineRepository.GetAllReceiptLines(pageSize, pageNumber).Result)
				{
					BOMReceiptLineDto item = new BOMReceiptLineDto
					{
						ConversionFactor = item2.ConversionFactor,
						CreatedBy = item2.CreatedBy,
						CreatedDate = item2.CreatedDate,
						Description = item2.Description,
						UniqueID = item2.UniqueID,
						HeatLot = item2.HeatLot,
						InventoryUnitOfMeasure = item2.InventoryUnitOfMeasure,
						Closed = item2.Closed,
						JobReceivedComplete = item2.JobReceivedComplete,
						PoReceivedComplete = item2.PoReceivedComplete,
						PostedToGl = item2.PostedToGl,
						RequiresInspection = item2.RequiresInspection,
						Reversed = item2.Reversed,
						JobAssemblyID = item2.JobAssemblyID,
						JobID = item2.JobID,
						JobMaterialID = item2.JobMaterialID,
						JobMatQuantityReceived = item2.JobMatQuantityReceived,
						JobOperationID = item2.JobOperationID,
						JobOprQuantityReceived = item2.JobOprQuantityReceived,
						JobType = item2.JobType,
						OrgPartID = item2.OrgPartID,
						OrgPartShortDescription = item2.OrgPartShortDescription,
						PartBinID = item2.PartBinID,
						PartID = item2.PartID,
						PartRevisionID = item2.PartRevisionID,
						PartWarehouseLocationID = item2.PartWarehouseLocationID,
						ProjectAreaID = item2.ProjectAreaID,
						ProjectID = item2.ProjectID,
						PurchaseOrderID = item2.PurchaseOrderID,
						PurchaseOrderLineID = item2.PurchaseOrderLineID,
						PurchaseQuantityReceived = item2.PurchaseQuantityReceived,
						PurchaseUnitCost = item2.PurchaseUnitCost,
						PurchaseUnitOfMeasure = item2.PurchaseUnitOfMeasure,
						ReceiptID = item2.ReceiptID,
						Reference = item2.Reference,
						ReverseReceiptID = item2.ReverseReceiptID,
						ReverseReceiptLineID = item2.ReverseReceiptLineID,
						ReceiptLineID = item2.ReceiptLineID,
						SetupCharge = item2.SetupCharge,
						RowVersion = item2.RowVersion
					};
					list.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ReceiptLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMReceiptLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetReceipt(string receiptId)
	{
		return await _bomReceiptModel.ValidateRequest_GetReceipt(receiptId, base.ApiClientContext);
	}
}
