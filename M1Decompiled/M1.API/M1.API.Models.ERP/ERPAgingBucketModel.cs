using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAgingBucketModel : ERPBaseModel, IERPAgingBucketModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAgingBuckets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAgingBucketRepository iERPAgingBucketRepository = (base.ERPAgingBucketRepository = new ERPAgingBucketRepository(base.ApiClientContext));
		using (iERPAgingBucketRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAgingBucketRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAgingBucketRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAgingBucketRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAgingBucketRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAgingBucket(Guid agingBucketId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAgingBucketRepository iERPAgingBucketRepository = (base.ERPAgingBucketRepository = new ERPAgingBucketRepository(base.ApiClientContext));
		using (iERPAgingBucketRepository)
		{
			if (!(await base.ERPAgingBucketRepository.DoesAgingBucketExist(agingBucketId)))
			{
				errorsList.Add($"AgingBucket [{agingBucketId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAgingBucketDto>>> Process_GetAllAgingBuckets(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAgingBucketDto> allAgingBucketsDto = new List<ERPAgingBucketDto>();
		ERPResponseMessageDto<IList<ERPAgingBucketDto>> result;
		try
		{
			IERPAgingBucketRepository iERPAgingBucketRepository = (base.ERPAgingBucketRepository = new ERPAgingBucketRepository(base.ApiClientContext));
			using (iERPAgingBucketRepository)
			{
				foreach (ERPAgingBucketInformationDto item2 in await base.ERPAgingBucketRepository.GetAllAgingBuckets(pageSize, pageNumber, filter, orderBy))
				{
					ERPAgingBucketDto item = new ERPAgingBucketDto
					{
						xaaBucket1DaysOver = item2.xaaBucket1DaysOver,
						xaaBucket1Description = item2.xaaBucket1Description,
						xaaBucket2DaysOver = item2.xaaBucket2DaysOver,
						xaaBucket2Description = item2.xaaBucket2Description,
						xaaBucket3DaysOver = item2.xaaBucket3DaysOver,
						xaaBucket3Description = item2.xaaBucket3Description,
						xaaBucket4DaysOver = item2.xaaBucket4DaysOver,
						xaaBucket4Description = item2.xaaBucket4Description,
						xaaBucket5DaysOver = item2.xaaBucket5DaysOver,
						xaaBucket5Description = item2.xaaBucket5Description,
						xaaCalculationType = item2.xaaCalculationType,
						xaaAgingBucketID = item2.xaaAgingBucketID,
						xaaCreatedBy = item2.xaaCreatedBy,
						xaaCreatedDate = item2.xaaCreatedDate,
						xaaDescription = item2.xaaDescription,
						xaaUniqueID = item2.xaaUniqueID,
						xaaRowVersion = item2.xaaRowVersion,
						CustomFields = item2.CustomFields
					};
					allAgingBucketsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all AgingBuckets]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAgingBucketDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAgingBucketsDto,
				RecordCount = allAgingBucketsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAgingBucketDto>> Process_GetAgingBucket(Guid agingBucketId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAgingBucketDto agingBucketDto = null;
		ERPResponseMessageDto<ERPAgingBucketDto> result;
		try
		{
			IERPAgingBucketRepository iERPAgingBucketRepository = (base.ERPAgingBucketRepository = new ERPAgingBucketRepository(base.ApiClientContext));
			using (iERPAgingBucketRepository)
			{
				ERPAgingBucketInformationDto eRPAgingBucketInformationDto = await base.ERPAgingBucketRepository.GetAgingBucket(agingBucketId);
				agingBucketDto = new ERPAgingBucketDto
				{
					xaaBucket1DaysOver = eRPAgingBucketInformationDto.xaaBucket1DaysOver,
					xaaBucket1Description = eRPAgingBucketInformationDto.xaaBucket1Description,
					xaaBucket2DaysOver = eRPAgingBucketInformationDto.xaaBucket2DaysOver,
					xaaBucket2Description = eRPAgingBucketInformationDto.xaaBucket2Description,
					xaaBucket3DaysOver = eRPAgingBucketInformationDto.xaaBucket3DaysOver,
					xaaBucket3Description = eRPAgingBucketInformationDto.xaaBucket3Description,
					xaaBucket4DaysOver = eRPAgingBucketInformationDto.xaaBucket4DaysOver,
					xaaBucket4Description = eRPAgingBucketInformationDto.xaaBucket4Description,
					xaaBucket5DaysOver = eRPAgingBucketInformationDto.xaaBucket5DaysOver,
					xaaBucket5Description = eRPAgingBucketInformationDto.xaaBucket5Description,
					xaaCalculationType = eRPAgingBucketInformationDto.xaaCalculationType,
					xaaAgingBucketID = eRPAgingBucketInformationDto.xaaAgingBucketID,
					xaaCreatedBy = eRPAgingBucketInformationDto.xaaCreatedBy,
					xaaCreatedDate = eRPAgingBucketInformationDto.xaaCreatedDate,
					xaaDescription = eRPAgingBucketInformationDto.xaaDescription,
					xaaUniqueID = eRPAgingBucketInformationDto.xaaUniqueID,
					xaaRowVersion = eRPAgingBucketInformationDto.xaaRowVersion,
					CustomFields = eRPAgingBucketInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the AgingBuckets []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAgingBucketDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = agingBucketDto
			};
		}
		return result;
	}
}
