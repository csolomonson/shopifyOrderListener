using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartForecastLineModel : ERPBaseModel, IERPPartForecastLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartForecastLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
		using (iERPPartForecastLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartForecastLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartForecastLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartForecastLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartForecastLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartForecastLine(Guid partForecastLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
		using (iERPPartForecastLineRepository)
		{
			if (!(await base.ERPPartForecastLineRepository.DoesPartForecastLineExist(partForecastLineId)))
			{
				errorsList.Add($"PartForecastLine [{partForecastLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartForecastLine(ERPPartForecastLineDto partForecastLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
		using (iERPPartForecastLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(partForecastLine.inlPartID) && !(await base.ERPPartForecastLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partForecastLine.inlPartID })))
			{
				errorsList.Add("inlPartID [" + partForecastLine.inlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partForecastLine.inlPartRevisionID) && !(await base.ERPPartForecastLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partForecastLine.inlPartID, partForecastLine.inlPartRevisionID })))
			{
				errorsList.Add("inlPartRevisionID [" + partForecastLine.inlPartRevisionID + "] not found.");
			}
			if (partForecastLine.inlPartForecastYearID > 0 && !(await base.ERPPartForecastLineRepository.DoesRecordExistInTableUsingKeys("PartForecasts", new object[3] { "INPPARTID", "INPPARTREVISIONID", "INPPARTFORECASTYEARID" }, new object[3] { partForecastLine.inlPartID, partForecastLine.inlPartRevisionID, partForecastLine.inlPartForecastYearID })))
			{
				errorsList.Add($"inlPartForecastYearID [{partForecastLine.inlPartForecastYearID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartForecastLineDto>>> Process_GetAllPartForecastLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartForecastLineDto> allPartForecastLinesDto = new List<ERPPartForecastLineDto>();
		ERPResponseMessageDto<IList<ERPPartForecastLineDto>> result;
		try
		{
			IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
			using (iERPPartForecastLineRepository)
			{
				foreach (ERPPartForecastLineInformationDto item2 in await base.ERPPartForecastLineRepository.GetAllPartForecastLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartForecastLineDto item = new ERPPartForecastLineDto
					{
						inlActualBalance = item2.inlActualBalance,
						inlActualQuantity = item2.inlActualQuantity,
						inlCreatedBy = item2.inlCreatedBy,
						inlCreatedDate = item2.inlCreatedDate,
						inlEndDate = item2.inlEndDate,
						inlUniqueID = item2.inlUniqueID,
						inlForecastBalance = item2.inlForecastBalance,
						inlForecastQuantity = item2.inlForecastQuantity,
						inlIncludeInMRP = item2.inlIncludeInMRP,
						inlPartForecastPeriodID = item2.inlPartForecastPeriodID,
						inlPartForecastYearID = item2.inlPartForecastYearID,
						inlPartID = item2.inlPartID,
						inlPartRevisionID = item2.inlPartRevisionID,
						inlRemainingQuantity = item2.inlRemainingQuantity,
						inlRemainingQuantityBalance = item2.inlRemainingQuantityBalance,
						inlRowVersion = item2.inlRowVersion,
						inlStartDate = item2.inlStartDate,
						CustomFields = item2.CustomFields
					};
					allPartForecastLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartForecastLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartForecastLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartForecastLinesDto,
				RecordCount = allPartForecastLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_GetPartForecastLine(Guid partForecastLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartForecastLineDto partForecastLineDto = null;
		ERPResponseMessageDto<ERPPartForecastLineDto> result;
		try
		{
			IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
			using (iERPPartForecastLineRepository)
			{
				ERPPartForecastLineInformationDto eRPPartForecastLineInformationDto = await base.ERPPartForecastLineRepository.GetPartForecastLine(partForecastLineId);
				partForecastLineDto = new ERPPartForecastLineDto
				{
					inlActualBalance = eRPPartForecastLineInformationDto.inlActualBalance,
					inlActualQuantity = eRPPartForecastLineInformationDto.inlActualQuantity,
					inlCreatedBy = eRPPartForecastLineInformationDto.inlCreatedBy,
					inlCreatedDate = eRPPartForecastLineInformationDto.inlCreatedDate,
					inlEndDate = eRPPartForecastLineInformationDto.inlEndDate,
					inlUniqueID = eRPPartForecastLineInformationDto.inlUniqueID,
					inlForecastBalance = eRPPartForecastLineInformationDto.inlForecastBalance,
					inlForecastQuantity = eRPPartForecastLineInformationDto.inlForecastQuantity,
					inlIncludeInMRP = eRPPartForecastLineInformationDto.inlIncludeInMRP,
					inlPartForecastPeriodID = eRPPartForecastLineInformationDto.inlPartForecastPeriodID,
					inlPartForecastYearID = eRPPartForecastLineInformationDto.inlPartForecastYearID,
					inlPartID = eRPPartForecastLineInformationDto.inlPartID,
					inlPartRevisionID = eRPPartForecastLineInformationDto.inlPartRevisionID,
					inlRemainingQuantity = eRPPartForecastLineInformationDto.inlRemainingQuantity,
					inlRemainingQuantityBalance = eRPPartForecastLineInformationDto.inlRemainingQuantityBalance,
					inlRowVersion = eRPPartForecastLineInformationDto.inlRowVersion,
					inlStartDate = eRPPartForecastLineInformationDto.inlStartDate,
					CustomFields = eRPPartForecastLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartForecastLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partForecastLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_PutPartForecastLine(ERPPartForecastLineDto partForecastLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartForecastLineDto createdObject = null;
		ERPResponseMessageDto<ERPPartForecastLineDto> result;
		try
		{
			IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
			using (iERPPartForecastLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartForecastLineRepository.SavePartForecastLine(partForecastLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartForecastLineInformationDto eRPPartForecastLineInformationDto = await base.ERPPartForecastLineRepository.GetPartForecastLine(partForecastLine.inlUniqueID);
					createdObject = new ERPPartForecastLineDto
					{
						inlActualBalance = eRPPartForecastLineInformationDto.inlActualBalance,
						inlActualQuantity = eRPPartForecastLineInformationDto.inlActualQuantity,
						inlCreatedBy = eRPPartForecastLineInformationDto.inlCreatedBy,
						inlCreatedDate = eRPPartForecastLineInformationDto.inlCreatedDate,
						inlEndDate = eRPPartForecastLineInformationDto.inlEndDate,
						inlUniqueID = eRPPartForecastLineInformationDto.inlUniqueID,
						inlForecastBalance = eRPPartForecastLineInformationDto.inlForecastBalance,
						inlForecastQuantity = eRPPartForecastLineInformationDto.inlForecastQuantity,
						inlIncludeInMRP = eRPPartForecastLineInformationDto.inlIncludeInMRP,
						inlPartForecastPeriodID = eRPPartForecastLineInformationDto.inlPartForecastPeriodID,
						inlPartForecastYearID = eRPPartForecastLineInformationDto.inlPartForecastYearID,
						inlPartID = eRPPartForecastLineInformationDto.inlPartID,
						inlPartRevisionID = eRPPartForecastLineInformationDto.inlPartRevisionID,
						inlRemainingQuantity = eRPPartForecastLineInformationDto.inlRemainingQuantity,
						inlRemainingQuantityBalance = eRPPartForecastLineInformationDto.inlRemainingQuantityBalance,
						inlRowVersion = eRPPartForecastLineInformationDto.inlRowVersion,
						inlStartDate = eRPPartForecastLineInformationDto.inlStartDate,
						CustomFields = eRPPartForecastLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartForecastLine [{partForecastLine.inlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartForecastLine(Guid partForecastLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
		using (iERPPartForecastLineRepository)
		{
			if (!(await base.ERPPartForecastLineRepository.DoesPartForecastLineExist(partForecastLineId)))
			{
				base.ErrorsList.Add($"PartForecastLine [{partForecastLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartForecastLineInformationDto eRPPartForecastLineInformationDto = await base.ERPPartForecastLineRepository.GetPartForecastLine(partForecastLineId);
				string text = await base.ERPPartForecastLineRepository.WhereUsed("PartForecastLines", new object[4] { eRPPartForecastLineInformationDto.inlPartID, eRPPartForecastLineInformationDto.inlPartRevisionID, eRPPartForecastLineInformationDto.inlPartForecastYearID, eRPPartForecastLineInformationDto.inlPartForecastPeriodID }, new object[4] { "inlPartID", "inlPartRevisionID", "inlPartForecastYearID", "inlPartForecastPeriodID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartForecastLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartForecastLineDto>> Process_DeletePartForecastLine(Guid partForecastLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartForecastLineDto> result;
		try
		{
			IERPPartForecastLineRepository iERPPartForecastLineRepository = (base.ERPPartForecastLineRepository = new ERPPartForecastLineRepository(base.ApiClientContext));
			using (iERPPartForecastLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartForecastLineRepository.DeleteRowFromTable("PartForecastLines", "inl", partForecastLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartForecastLine [{partForecastLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartForecastLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartForecastLineDto()
			};
		}
		return result;
	}
}
