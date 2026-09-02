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

public class BOMMaterialIssueLineModel : BOMBaseModel, IBOMMaterialIssueLineModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	private readonly IBOMMaterialIssueModel _bomMaterialIssueModel;

	public IDictionary<string, object> MaterialIssueKeyDictionary { get; set; }

	public BOMMaterialIssueLineModel(IBOMMaterialIssueModel bomMaterialIssueModel)
	{
		_bomMaterialIssueModel = bomMaterialIssueModel;
	}

	public BOMMaterialIssueLineModel()
	{
		MaterialIssueKeyDictionary = new Dictionary<string, object>();
	}

	public async Task<BOMResponseMessageDto<IList<BOMMaterialIssueLineDto>>> Process_GetAllMaterialIssueLines(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BOMMaterialIssueLineDto> list = new List<BOMMaterialIssueLineDto>();
		BOMResponseMessageDto<IList<BOMMaterialIssueLineDto>> result;
		try
		{
			using MaterialIssueLineRepository materialIssueLineRepository = new MaterialIssueLineRepository(base.ApiClientContext);
			foreach (MaterialIssueLineInformationDto item2 in materialIssueLineRepository.GetAllMaterialIssueLines(pageSize, pageNumber).Result)
			{
				BOMMaterialIssueLineDto item = new BOMMaterialIssueLineDto
				{
					CreatedBy = item2.CreatedBy,
					CreatedDate = item2.CreatedDate,
					UniqueID = item2.UniqueID,
					EstimatedQuantity = item2.EstimatedQuantity,
					HeatLot = item2.HeatLot,
					InvIssueQuantity = item2.InvIssueQuantity,
					InvScrapQuantity = item2.InvScrapQuantity,
					CreateJobSeq = item2.CreateJobSeq,
					IssueComplete = item2.IssueComplete,
					KitPart = item2.KitPart,
					Posted = item2.Posted,
					Reversed = item2.Reversed,
					IssueType = item2.IssueType,
					JobAsmIssueQuantity = item2.JobAsmIssueQuantity,
					JobAsmScrapQuantity = item2.JobAsmScrapQuantity,
					JobAssemblyID = item2.JobAssemblyID,
					JobID = item2.JobID,
					JobMaterialID = item2.JobMaterialID,
					JobMatIssueQuantity = item2.JobMatIssueQuantity,
					JobMatReturnIssueQuantity = item2.JobMatReturnIssueQuantity,
					JobMatReturnScrapQuantity = item2.JobMatReturnScrapQuantity,
					JobMatScrapQuantity = item2.JobMatScrapQuantity,
					JobOpenQuantity = item2.JobOpenQuantity,
					JobType = item2.JobType,
					LongDescriptionText = item2.LongDescriptionText,
					MaterialIssueID = item2.MaterialIssueID,
					MiscIssueReasonID = item2.MiscIssueReasonID,
					PartBinID = item2.PartBinID,
					PartID = item2.PartID,
					PartRevisionID = item2.PartRevisionID,
					PartWarehouseLocationID = item2.PartWarehouseLocationID,
					PlantID = item2.PlantID,
					ProjectAreaID = item2.ProjectAreaID,
					ProjectID = item2.ProjectID,
					QuantityAllocated = item2.QuantityAllocated,
					QuantityOnHand = item2.QuantityOnHand,
					Reference = item2.Reference,
					ReverseMaterialIssueID = item2.ReverseMaterialIssueID,
					ReverseMaterialIssueLineID = item2.ReverseMaterialIssueLineID,
					MaterialIssueLineID = item2.MaterialIssueLineID,
					RowVersion = item2.RowVersion
				};
				list.Add(item);
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MaterialIssueLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BOMMaterialIssueLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<CTMBOMMaterialIssueLineDto>> Process_GetMaterialIssueLines(string materialIssueId)
	{
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		CTMBOMMaterialIssueLineDto cTMBOMMaterialIssueLineDto = new CTMBOMMaterialIssueLineDto();
		new BOMResponseMessageDto<CTMBOMMaterialIssueLineDto>();
		BOMResponseMessageDto<CTMBOMMaterialIssueLineDto> result3;
		try
		{
			using (MaterialIssueLineRepository materialIssueLineRepository = new MaterialIssueLineRepository(base.ApiClientContext))
			{
				MaterialIssueDto result = materialIssueLineRepository.GetMaterialIssueInfo(materialIssueId).Result;
				IList<MaterialIssueLineInformationDto> result2 = materialIssueLineRepository.GetMaterialIssueLineInfo(materialIssueId).Result;
				cTMBOMMaterialIssueLineDto.MaterialIssue = new BOMMaterialIssueDto
				{
					MaterialIssueID = result.MaterialIssueID,
					MaterialIssueDate = result.MaterialIssueDate,
					Posted = result.Posted,
					PostedDate = result.PostedDate,
					ReversalEntry = result.ReversalEntry,
					Reversed = result.Reversed,
					UniqueID = result.UniqueID,
					RowVersion = result.RowVersion
				};
				foreach (MaterialIssueLineInformationDto item in result2)
				{
					cTMBOMMaterialIssueLineDto.MaterialIssueLines.Add(new BOMMaterialIssueLineDto
					{
						CreatedBy = item.CreatedBy,
						CreatedDate = item.CreatedDate,
						UniqueID = item.UniqueID,
						EstimatedQuantity = item.EstimatedQuantity,
						HeatLot = item.HeatLot,
						InvIssueQuantity = item.InvIssueQuantity,
						InvScrapQuantity = item.InvScrapQuantity,
						CreateJobSeq = item.CreateJobSeq,
						IssueComplete = item.IssueComplete,
						KitPart = item.KitPart,
						Posted = item.Posted,
						Reversed = item.Reversed,
						IssueType = item.IssueType,
						JobAsmIssueQuantity = item.JobAsmIssueQuantity,
						JobAsmScrapQuantity = item.JobAsmScrapQuantity,
						JobAssemblyID = item.JobAssemblyID,
						JobID = item.JobID,
						JobMaterialID = item.JobMaterialID,
						JobMatIssueQuantity = item.JobMatIssueQuantity,
						JobMatReturnIssueQuantity = item.JobMatReturnIssueQuantity,
						JobMatReturnScrapQuantity = item.JobMatReturnScrapQuantity,
						JobMatScrapQuantity = item.JobMatScrapQuantity,
						JobOpenQuantity = item.JobOpenQuantity,
						JobType = item.JobType,
						LongDescriptionText = item.LongDescriptionText,
						MaterialIssueID = item.MaterialIssueID,
						MiscIssueReasonID = item.MiscIssueReasonID,
						PartBinID = item.PartBinID,
						PartID = item.PartID,
						PartRevisionID = item.PartRevisionID,
						PartWarehouseLocationID = item.PartWarehouseLocationID,
						PlantID = item.PlantID,
						ProjectAreaID = item.ProjectAreaID,
						ProjectID = item.ProjectID,
						QuantityAllocated = item.QuantityAllocated,
						QuantityOnHand = item.QuantityOnHand,
						Reference = item.Reference,
						ReverseMaterialIssueID = item.ReverseMaterialIssueID,
						ReverseMaterialIssueLineID = item.ReverseMaterialIssueLineID,
						MaterialIssueLineID = item.MaterialIssueLineID,
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
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Material Issue [" + materialIssueId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result3 = new BOMResponseMessageDto<CTMBOMMaterialIssueLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = cTMBOMMaterialIssueLineDto
			};
		}
		return result3;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMaterialIssue(string materialIssueId)
	{
		return await _bomMaterialIssueModel.ValidateRequest_GetMaterialIssue(materialIssueId, base.ApiClientContext);
	}
}
