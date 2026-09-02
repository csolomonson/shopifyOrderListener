using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchasePlannerLineModel : ERPBaseModel, IERPPurchasePlannerLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
		using (iERPPurchasePlannerLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchasePlannerLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchasePlannerLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchasePlannerLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchasePlannerLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerLine(Guid purchasePlannerLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
		using (iERPPurchasePlannerLineRepository)
		{
			if (!(await base.ERPPurchasePlannerLineRepository.DoesPurchasePlannerLineExist(purchasePlannerLineId)))
			{
				errorsList.Add($"PurchasePlannerLine [{purchasePlannerLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
		using (iERPPurchasePlannerLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchasePlannerLine.pplSessionID) && !(await base.ERPPurchasePlannerLineRepository.DoesRecordExistInTableUsingKeys("PurchasePlannerSessions", new object[1] { "ppsSessionID" }, new object[1] { purchasePlannerLine.pplSessionID })))
			{
				errorsList.Add("pplSessionID [" + purchasePlannerLine.pplSessionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerLine.pplPartID) && !(await base.ERPPurchasePlannerLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { purchasePlannerLine.pplPartID })))
			{
				errorsList.Add("pplPartID [" + purchasePlannerLine.pplPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerLine.pplPartRevisionID) && !(await base.ERPPurchasePlannerLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { purchasePlannerLine.pplPartID, purchasePlannerLine.pplPartRevisionID })))
			{
				errorsList.Add("pplPartRevisionID [" + purchasePlannerLine.pplPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerLine.pplWarehouseID) && !(await base.ERPPurchasePlannerLineRepository.DoesRecordExistInTableUsingKeys("Warehouses", new object[1] { "IMWWAREHOUSEID" }, new object[1] { purchasePlannerLine.pplWarehouseID })))
			{
				errorsList.Add("pplWarehouseID [" + purchasePlannerLine.pplWarehouseID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerLine.pplPlantID) && !(await base.ERPPurchasePlannerLineRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { purchasePlannerLine.pplPlantID })))
			{
				errorsList.Add("pplPlantID [" + purchasePlannerLine.pplPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchasePlannerLineDto>>> Process_GetAllPurchasePlannerLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchasePlannerLineDto> allPurchasePlannerLinesDto = new List<ERPPurchasePlannerLineDto>();
		ERPResponseMessageDto<IList<ERPPurchasePlannerLineDto>> result;
		try
		{
			IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
			using (iERPPurchasePlannerLineRepository)
			{
				foreach (ERPPurchasePlannerLineInformationDto item2 in await base.ERPPurchasePlannerLineRepository.GetAllPurchasePlannerLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchasePlannerLineDto item = new ERPPurchasePlannerLineDto
					{
						pplCreatedBy = item2.pplCreatedBy,
						pplCreatedDate = item2.pplCreatedDate,
						pplDataMissing = item2.pplDataMissing,
						pplUniqueID = item2.pplUniqueID,
						pplExtendedCostBase = item2.pplExtendedCostBase,
						pplCompleted = item2.pplCompleted,
						pplNonStockedItem = item2.pplNonStockedItem,
						pplPhantomOrKitPart = item2.pplPhantomOrKitPart,
						pplLastRunDate = item2.pplLastRunDate,
						pplLineID = item2.pplLineID,
						pplLotSize = item2.pplLotSize,
						pplMaximumQuantity = item2.pplMaximumQuantity,
						pplMinimumQuantity = item2.pplMinimumQuantity,
						pplPartID = item2.pplPartID,
						pplPartRevisionID = item2.pplPartRevisionID,
						pplPartShortDescription = item2.pplPartShortDescription,
						pplPlantID = item2.pplPlantID,
						pplQuantityOnHand = item2.pplQuantityOnHand,
						pplReorderMethod = item2.pplReorderMethod,
						pplRowVersion = item2.pplRowVersion,
						pplSessionID = item2.pplSessionID,
						pplWarehouseID = item2.pplWarehouseID,
						CustomFields = item2.CustomFields
					};
					allPurchasePlannerLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchasePlannerLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchasePlannerLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchasePlannerLinesDto,
				RecordCount = allPurchasePlannerLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_GetPurchasePlannerLine(Guid purchasePlannerLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchasePlannerLineDto purchasePlannerLineDto = null;
		ERPResponseMessageDto<ERPPurchasePlannerLineDto> result;
		try
		{
			IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
			using (iERPPurchasePlannerLineRepository)
			{
				ERPPurchasePlannerLineInformationDto eRPPurchasePlannerLineInformationDto = await base.ERPPurchasePlannerLineRepository.GetPurchasePlannerLine(purchasePlannerLineId);
				purchasePlannerLineDto = new ERPPurchasePlannerLineDto
				{
					pplCreatedBy = eRPPurchasePlannerLineInformationDto.pplCreatedBy,
					pplCreatedDate = eRPPurchasePlannerLineInformationDto.pplCreatedDate,
					pplDataMissing = eRPPurchasePlannerLineInformationDto.pplDataMissing,
					pplUniqueID = eRPPurchasePlannerLineInformationDto.pplUniqueID,
					pplExtendedCostBase = eRPPurchasePlannerLineInformationDto.pplExtendedCostBase,
					pplCompleted = eRPPurchasePlannerLineInformationDto.pplCompleted,
					pplNonStockedItem = eRPPurchasePlannerLineInformationDto.pplNonStockedItem,
					pplPhantomOrKitPart = eRPPurchasePlannerLineInformationDto.pplPhantomOrKitPart,
					pplLastRunDate = eRPPurchasePlannerLineInformationDto.pplLastRunDate,
					pplLineID = eRPPurchasePlannerLineInformationDto.pplLineID,
					pplLotSize = eRPPurchasePlannerLineInformationDto.pplLotSize,
					pplMaximumQuantity = eRPPurchasePlannerLineInformationDto.pplMaximumQuantity,
					pplMinimumQuantity = eRPPurchasePlannerLineInformationDto.pplMinimumQuantity,
					pplPartID = eRPPurchasePlannerLineInformationDto.pplPartID,
					pplPartRevisionID = eRPPurchasePlannerLineInformationDto.pplPartRevisionID,
					pplPartShortDescription = eRPPurchasePlannerLineInformationDto.pplPartShortDescription,
					pplPlantID = eRPPurchasePlannerLineInformationDto.pplPlantID,
					pplQuantityOnHand = eRPPurchasePlannerLineInformationDto.pplQuantityOnHand,
					pplReorderMethod = eRPPurchasePlannerLineInformationDto.pplReorderMethod,
					pplRowVersion = eRPPurchasePlannerLineInformationDto.pplRowVersion,
					pplSessionID = eRPPurchasePlannerLineInformationDto.pplSessionID,
					pplWarehouseID = eRPPurchasePlannerLineInformationDto.pplWarehouseID,
					CustomFields = eRPPurchasePlannerLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchasePlannerLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchasePlannerLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_PutPurchasePlannerLine(ERPPurchasePlannerLineDto purchasePlannerLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchasePlannerLineDto createdObject = null;
		ERPResponseMessageDto<ERPPurchasePlannerLineDto> result;
		try
		{
			IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
			using (iERPPurchasePlannerLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchasePlannerLineRepository.SavePurchasePlannerLine(purchasePlannerLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchasePlannerLineInformationDto eRPPurchasePlannerLineInformationDto = await base.ERPPurchasePlannerLineRepository.GetPurchasePlannerLine(purchasePlannerLine.pplUniqueID);
					createdObject = new ERPPurchasePlannerLineDto
					{
						pplCreatedBy = eRPPurchasePlannerLineInformationDto.pplCreatedBy,
						pplCreatedDate = eRPPurchasePlannerLineInformationDto.pplCreatedDate,
						pplDataMissing = eRPPurchasePlannerLineInformationDto.pplDataMissing,
						pplUniqueID = eRPPurchasePlannerLineInformationDto.pplUniqueID,
						pplExtendedCostBase = eRPPurchasePlannerLineInformationDto.pplExtendedCostBase,
						pplCompleted = eRPPurchasePlannerLineInformationDto.pplCompleted,
						pplNonStockedItem = eRPPurchasePlannerLineInformationDto.pplNonStockedItem,
						pplPhantomOrKitPart = eRPPurchasePlannerLineInformationDto.pplPhantomOrKitPart,
						pplLastRunDate = eRPPurchasePlannerLineInformationDto.pplLastRunDate,
						pplLineID = eRPPurchasePlannerLineInformationDto.pplLineID,
						pplLotSize = eRPPurchasePlannerLineInformationDto.pplLotSize,
						pplMaximumQuantity = eRPPurchasePlannerLineInformationDto.pplMaximumQuantity,
						pplMinimumQuantity = eRPPurchasePlannerLineInformationDto.pplMinimumQuantity,
						pplPartID = eRPPurchasePlannerLineInformationDto.pplPartID,
						pplPartRevisionID = eRPPurchasePlannerLineInformationDto.pplPartRevisionID,
						pplPartShortDescription = eRPPurchasePlannerLineInformationDto.pplPartShortDescription,
						pplPlantID = eRPPurchasePlannerLineInformationDto.pplPlantID,
						pplQuantityOnHand = eRPPurchasePlannerLineInformationDto.pplQuantityOnHand,
						pplReorderMethod = eRPPurchasePlannerLineInformationDto.pplReorderMethod,
						pplRowVersion = eRPPurchasePlannerLineInformationDto.pplRowVersion,
						pplSessionID = eRPPurchasePlannerLineInformationDto.pplSessionID,
						pplWarehouseID = eRPPurchasePlannerLineInformationDto.pplWarehouseID,
						CustomFields = eRPPurchasePlannerLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchasePlannerLine [{purchasePlannerLine.pplUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerLine(Guid purchasePlannerLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
		using (iERPPurchasePlannerLineRepository)
		{
			if (!(await base.ERPPurchasePlannerLineRepository.DoesPurchasePlannerLineExist(purchasePlannerLineId)))
			{
				base.ErrorsList.Add($"PurchasePlannerLine [{purchasePlannerLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchasePlannerLineInformationDto eRPPurchasePlannerLineInformationDto = await base.ERPPurchasePlannerLineRepository.GetPurchasePlannerLine(purchasePlannerLineId);
				string text = await base.ERPPurchasePlannerLineRepository.WhereUsed("PurchasePlannerLines", new object[2] { eRPPurchasePlannerLineInformationDto.pplSessionID, eRPPurchasePlannerLineInformationDto.pplLineID }, new object[2] { "pplSessionID", "pplLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchasePlannerLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerLineDto>> Process_DeletePurchasePlannerLine(Guid purchasePlannerLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchasePlannerLineDto> result;
		try
		{
			IERPPurchasePlannerLineRepository iERPPurchasePlannerLineRepository = (base.ERPPurchasePlannerLineRepository = new ERPPurchasePlannerLineRepository(base.ApiClientContext));
			using (iERPPurchasePlannerLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchasePlannerLineRepository.DeleteRowFromTable("PurchasePlannerLines", "ppl", purchasePlannerLineId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchasePlannerLine [{purchasePlannerLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchasePlannerLineDto()
			};
		}
		return result;
	}
}
