using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.BOM.Transaction;
using M1.API.DTOs.Core;
using M1.API.DTOs.Core.Transaction;
using M1.API.DTOs.Custom;
using M1.API.Repositories.Core.Transaction;

namespace M1.API.Models.BOM.Transaction;

public class BOMMfgReceiptModel : BOMBaseModel, IBOMMfgReceiptModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public Task<APIValidationInfoDto> ValidateRequest_GetMfgReceipt(string mfgReceiptId)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		using (MfgReceiptRepository mfgReceiptRepository = new MfgReceiptRepository(base.ApiClientContext))
		{
			if (!mfgReceiptRepository.DoesMfgReceiptExists(mfgReceiptId).Result)
			{
				list.Add("MfgReceipt [" + mfgReceiptId + "] is invalid");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<IList<BOMMfgReceiptDto>>> Process_GetAllMfgReceipts(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMMfgReceiptDto> list = new List<BOMMfgReceiptDto>();
		BOMResponseMessageDto<IList<BOMMfgReceiptDto>> result;
		try
		{
			using MfgReceiptRepository mfgReceiptRepository = new MfgReceiptRepository(base.ApiClientContext);
			foreach (MfgReceiptInformationDto item2 in mfgReceiptRepository.GetAllMfgReceipts(pageSize, pageNumber).Result)
			{
				BOMMfgReceiptDto item = new BOMMfgReceiptDto
				{
					MfgReceiptID = item2.MfgReceiptID,
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					HeatLot = item2.HeatLot,
					Posted = item2.Posted,
					MiscInvQuantityReceived = item2.MiscInvQuantityReceived,
					InventoryQuantityReceived = item2.InventoryQuantityReceived,
					JobAsmQuantityReceived = item2.JobAsmQuantityReceived,
					JobMatQuantityReceived = item2.JobMatQuantityReceived,
					JobOprQuantityReceived = item2.JobOprQuantityReceived,
					PartBinID = item2.PartBinID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PostedDate = item2.PostedDate,
					ProjectAreaID = item2.ProjectAreaID,
					ProjectID = item2.ProjectID,
					ReceiptDate = item2.ReceiptDate,
					ReceiptType = item2.ReceiptType,
					Reference = item2.Reference,
					RowVersion = item2.RowVersion
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MfgReceipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMMfgReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<BOMMfgReceiptDto>> Process_GetMfgReceipt(string mfgReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BOMMfgReceiptDto returnObject = null;
		BOMResponseMessageDto<BOMMfgReceiptDto> result2;
		try
		{
			using MfgReceiptRepository mfgReceiptRepository = new MfgReceiptRepository(base.ApiClientContext);
			MfgReceiptDto result = mfgReceiptRepository.GetMfgReceipt(mfgReceiptId).Result;
			returnObject = new BOMMfgReceiptDto
			{
				MfgReceiptID = result.MfgReceiptID,
				CreatedBy = result.CreatedBy,
				CreatedDate = result.CreatedDate,
				UniqueID = result.UniqueID,
				HeatLot = result.HeatLot,
				Posted = result.Posted,
				MiscInvQuantityReceived = result.MiscInvQuantityReceived,
				InventoryQuantityReceived = result.InventoryQuantityReceived,
				JobAsmQuantityReceived = result.JobAsmQuantityReceived,
				JobOprQuantityReceived = result.JobOprQuantityReceived,
				JobMatQuantityReceived = result.JobMatQuantityReceived,
				PartBinID = result.PartBinID,
				PartID = result.PartID,
				PartRevisionID = result.PartRevisionID,
				PartWarehouseLocationID = result.PartWarehouseLocationID,
				PostedDate = result.PostedDate,
				ProjectAreaID = result.ProjectAreaID,
				ProjectID = result.ProjectID,
				ReceiptDate = result.ReceiptDate,
				ReceiptType = result.ReceiptType,
				Reference = result.Reference,
				RowVersion = result.RowVersion
			};
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MfgReceipts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result2 = new BOMResponseMessageDto<BOMMfgReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result2;
	}
}
