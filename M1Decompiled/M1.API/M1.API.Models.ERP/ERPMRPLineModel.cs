using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMRPLineModel : ERPBaseModel, IERPMRPLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMRPLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
		using (iERPMRPLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMRPLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMRPLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMRPLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMRPLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMRPLine(Guid mRPLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
		using (iERPMRPLineRepository)
		{
			if (!(await base.ERPMRPLineRepository.DoesMRPLineExist(mRPLineId)))
			{
				errorsList.Add($"MRPLine [{mRPLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMRPLine(ERPMRPLineDto mRPLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
		using (iERPMRPLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(mRPLine.mrlSessionID) && !(await base.ERPMRPLineRepository.DoesRecordExistInTableUsingKeys("MRPSessions", new object[1] { "mrpSessionID" }, new object[1] { mRPLine.mrlSessionID })))
			{
				errorsList.Add("mrlSessionID [" + mRPLine.mrlSessionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPLine.mrlPartID) && !(await base.ERPMRPLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mRPLine.mrlPartID })))
			{
				errorsList.Add("mrlPartID [" + mRPLine.mrlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPLine.mrlPartRevisionID) && !(await base.ERPMRPLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mRPLine.mrlPartID, mRPLine.mrlPartRevisionID })))
			{
				errorsList.Add("mrlPartRevisionID [" + mRPLine.mrlPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMRPLineDto>>> Process_GetAllMRPLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMRPLineDto> allMRPLinesDto = new List<ERPMRPLineDto>();
		ERPResponseMessageDto<IList<ERPMRPLineDto>> result;
		try
		{
			IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
			using (iERPMRPLineRepository)
			{
				foreach (ERPMRPLineInformationDto item2 in await base.ERPMRPLineRepository.GetAllMRPLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPMRPLineDto item = new ERPMRPLineDto
					{
						mrlCreatedBy = item2.mrlCreatedBy,
						mrlCreatedDate = item2.mrlCreatedDate,
						mrlUniqueID = item2.mrlUniqueID,
						mrlForecastDemand = item2.mrlForecastDemand,
						mrlInvQtyInProduction = item2.mrlInvQtyInProduction,
						mrlCompleted = item2.mrlCompleted,
						mrlDataMissing = item2.mrlDataMissing,
						mrlLineID = item2.mrlLineID,
						mrlMaximumQuantity = item2.mrlMaximumQuantity,
						mrlMfgLotSize = item2.mrlMfgLotSize,
						mrlMinimumQuantity = item2.mrlMinimumQuantity,
						mrlPartID = item2.mrlPartID,
						mrlPartRevisionID = item2.mrlPartRevisionID,
						mrlPartShortDescription = item2.mrlPartShortDescription,
						mrlPlantIDs = item2.mrlPlantIDs,
						mrlQuantityAllocated = item2.mrlQuantityAllocated,
						mrlQuantityOnHand = item2.mrlQuantityOnHand,
						mrlQuantityToInspect = item2.mrlQuantityToInspect,
						mrlRowVersion = item2.mrlRowVersion,
						mrlSessionID = item2.mrlSessionID,
						mrlWarehouseIDs = item2.mrlWarehouseIDs,
						CustomFields = item2.CustomFields
					};
					allMRPLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MRPLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMRPLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMRPLinesDto,
				RecordCount = allMRPLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_GetMRPLine(Guid mRPLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMRPLineDto mRPLineDto = null;
		ERPResponseMessageDto<ERPMRPLineDto> result;
		try
		{
			IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
			using (iERPMRPLineRepository)
			{
				ERPMRPLineInformationDto eRPMRPLineInformationDto = await base.ERPMRPLineRepository.GetMRPLine(mRPLineId);
				mRPLineDto = new ERPMRPLineDto
				{
					mrlCreatedBy = eRPMRPLineInformationDto.mrlCreatedBy,
					mrlCreatedDate = eRPMRPLineInformationDto.mrlCreatedDate,
					mrlUniqueID = eRPMRPLineInformationDto.mrlUniqueID,
					mrlForecastDemand = eRPMRPLineInformationDto.mrlForecastDemand,
					mrlInvQtyInProduction = eRPMRPLineInformationDto.mrlInvQtyInProduction,
					mrlCompleted = eRPMRPLineInformationDto.mrlCompleted,
					mrlDataMissing = eRPMRPLineInformationDto.mrlDataMissing,
					mrlLineID = eRPMRPLineInformationDto.mrlLineID,
					mrlMaximumQuantity = eRPMRPLineInformationDto.mrlMaximumQuantity,
					mrlMfgLotSize = eRPMRPLineInformationDto.mrlMfgLotSize,
					mrlMinimumQuantity = eRPMRPLineInformationDto.mrlMinimumQuantity,
					mrlPartID = eRPMRPLineInformationDto.mrlPartID,
					mrlPartRevisionID = eRPMRPLineInformationDto.mrlPartRevisionID,
					mrlPartShortDescription = eRPMRPLineInformationDto.mrlPartShortDescription,
					mrlPlantIDs = eRPMRPLineInformationDto.mrlPlantIDs,
					mrlQuantityAllocated = eRPMRPLineInformationDto.mrlQuantityAllocated,
					mrlQuantityOnHand = eRPMRPLineInformationDto.mrlQuantityOnHand,
					mrlQuantityToInspect = eRPMRPLineInformationDto.mrlQuantityToInspect,
					mrlRowVersion = eRPMRPLineInformationDto.mrlRowVersion,
					mrlSessionID = eRPMRPLineInformationDto.mrlSessionID,
					mrlWarehouseIDs = eRPMRPLineInformationDto.mrlWarehouseIDs,
					CustomFields = eRPMRPLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MRPLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mRPLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_PutMRPLine(ERPMRPLineDto mRPLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMRPLineDto createdObject = null;
		ERPResponseMessageDto<ERPMRPLineDto> result;
		try
		{
			IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
			using (iERPMRPLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMRPLineRepository.SaveMRPLine(mRPLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMRPLineInformationDto eRPMRPLineInformationDto = await base.ERPMRPLineRepository.GetMRPLine(mRPLine.mrlUniqueID);
					createdObject = new ERPMRPLineDto
					{
						mrlCreatedBy = eRPMRPLineInformationDto.mrlCreatedBy,
						mrlCreatedDate = eRPMRPLineInformationDto.mrlCreatedDate,
						mrlUniqueID = eRPMRPLineInformationDto.mrlUniqueID,
						mrlForecastDemand = eRPMRPLineInformationDto.mrlForecastDemand,
						mrlInvQtyInProduction = eRPMRPLineInformationDto.mrlInvQtyInProduction,
						mrlCompleted = eRPMRPLineInformationDto.mrlCompleted,
						mrlDataMissing = eRPMRPLineInformationDto.mrlDataMissing,
						mrlLineID = eRPMRPLineInformationDto.mrlLineID,
						mrlMaximumQuantity = eRPMRPLineInformationDto.mrlMaximumQuantity,
						mrlMfgLotSize = eRPMRPLineInformationDto.mrlMfgLotSize,
						mrlMinimumQuantity = eRPMRPLineInformationDto.mrlMinimumQuantity,
						mrlPartID = eRPMRPLineInformationDto.mrlPartID,
						mrlPartRevisionID = eRPMRPLineInformationDto.mrlPartRevisionID,
						mrlPartShortDescription = eRPMRPLineInformationDto.mrlPartShortDescription,
						mrlPlantIDs = eRPMRPLineInformationDto.mrlPlantIDs,
						mrlQuantityAllocated = eRPMRPLineInformationDto.mrlQuantityAllocated,
						mrlQuantityOnHand = eRPMRPLineInformationDto.mrlQuantityOnHand,
						mrlQuantityToInspect = eRPMRPLineInformationDto.mrlQuantityToInspect,
						mrlRowVersion = eRPMRPLineInformationDto.mrlRowVersion,
						mrlSessionID = eRPMRPLineInformationDto.mrlSessionID,
						mrlWarehouseIDs = eRPMRPLineInformationDto.mrlWarehouseIDs,
						CustomFields = eRPMRPLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MRPLine [{mRPLine.mrlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMRPLine(Guid mRPLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
		using (iERPMRPLineRepository)
		{
			if (!(await base.ERPMRPLineRepository.DoesMRPLineExist(mRPLineId)))
			{
				base.ErrorsList.Add($"MRPLine [{mRPLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMRPLineInformationDto eRPMRPLineInformationDto = await base.ERPMRPLineRepository.GetMRPLine(mRPLineId);
				string text = await base.ERPMRPLineRepository.WhereUsed("MRPLines", new object[2] { eRPMRPLineInformationDto.mrlSessionID, eRPMRPLineInformationDto.mrlLineID }, new object[2] { "mrlSessionID", "mrlLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MRPLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMRPLineDto>> Process_DeleteMRPLine(Guid mRPLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMRPLineDto> result;
		try
		{
			IERPMRPLineRepository iERPMRPLineRepository = (base.ERPMRPLineRepository = new ERPMRPLineRepository(base.ApiClientContext));
			using (iERPMRPLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMRPLineRepository.DeleteRowFromTable("MRPLines", "mrl", mRPLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MRPLine [{mRPLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMRPLineDto()
			};
		}
		return result;
	}
}
