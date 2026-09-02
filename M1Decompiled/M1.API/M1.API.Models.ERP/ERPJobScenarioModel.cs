using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobScenarioModel : ERPBaseModel, IERPJobScenarioModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobScenarios(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
		using (iERPJobScenarioRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobScenarioRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobScenarioRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobScenarioRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobScenarioRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobScenario(Guid jobScenarioId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
		using (iERPJobScenarioRepository)
		{
			if (!(await base.ERPJobScenarioRepository.DoesJobScenarioExist(jobScenarioId)))
			{
				errorsList.Add($"JobScenario [{jobScenarioId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobScenario(ERPJobScenarioDto jobScenario)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
		using (iERPJobScenarioRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobScenarioDto>>> Process_GetAllJobScenarios(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobScenarioDto> allJobScenariosDto = new List<ERPJobScenarioDto>();
		ERPResponseMessageDto<IList<ERPJobScenarioDto>> result;
		try
		{
			IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
			using (iERPJobScenarioRepository)
			{
				foreach (ERPJobScenarioInformationDto item2 in await base.ERPJobScenarioRepository.GetAllJobScenarios(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobScenarioDto item = new ERPJobScenarioDto
					{
						jmnJobScenarioID = item2.jmnJobScenarioID,
						jmnCreatedBy = item2.jmnCreatedBy,
						jmnCreatedDate = item2.jmnCreatedDate,
						jmnDescription = item2.jmnDescription,
						jmnUniqueID = item2.jmnUniqueID,
						jmnRowVersion = item2.jmnRowVersion,
						CustomFields = item2.CustomFields
					};
					allJobScenariosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobScenarios]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobScenarioDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobScenariosDto,
				RecordCount = allJobScenariosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_GetJobScenario(Guid jobScenarioId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobScenarioDto jobScenarioDto = null;
		ERPResponseMessageDto<ERPJobScenarioDto> result;
		try
		{
			IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
			using (iERPJobScenarioRepository)
			{
				ERPJobScenarioInformationDto eRPJobScenarioInformationDto = await base.ERPJobScenarioRepository.GetJobScenario(jobScenarioId);
				jobScenarioDto = new ERPJobScenarioDto
				{
					jmnJobScenarioID = eRPJobScenarioInformationDto.jmnJobScenarioID,
					jmnCreatedBy = eRPJobScenarioInformationDto.jmnCreatedBy,
					jmnCreatedDate = eRPJobScenarioInformationDto.jmnCreatedDate,
					jmnDescription = eRPJobScenarioInformationDto.jmnDescription,
					jmnUniqueID = eRPJobScenarioInformationDto.jmnUniqueID,
					jmnRowVersion = eRPJobScenarioInformationDto.jmnRowVersion,
					CustomFields = eRPJobScenarioInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobScenarios []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobScenarioDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobScenarioDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_PutJobScenario(ERPJobScenarioDto jobScenario)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobScenarioDto createdObject = null;
		ERPResponseMessageDto<ERPJobScenarioDto> result;
		try
		{
			IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
			using (iERPJobScenarioRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobScenarioRepository.SaveJobScenario(jobScenario);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobScenarioInformationDto eRPJobScenarioInformationDto = await base.ERPJobScenarioRepository.GetJobScenario(jobScenario.jmnUniqueID);
					createdObject = new ERPJobScenarioDto
					{
						jmnJobScenarioID = eRPJobScenarioInformationDto.jmnJobScenarioID,
						jmnCreatedBy = eRPJobScenarioInformationDto.jmnCreatedBy,
						jmnCreatedDate = eRPJobScenarioInformationDto.jmnCreatedDate,
						jmnDescription = eRPJobScenarioInformationDto.jmnDescription,
						jmnUniqueID = eRPJobScenarioInformationDto.jmnUniqueID,
						jmnRowVersion = eRPJobScenarioInformationDto.jmnRowVersion,
						CustomFields = eRPJobScenarioInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobScenario [{jobScenario.jmnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobScenarioDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobScenario(Guid jobScenarioId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
		using (iERPJobScenarioRepository)
		{
			if (!(await base.ERPJobScenarioRepository.DoesJobScenarioExist(jobScenarioId)))
			{
				base.ErrorsList.Add($"JobScenario [{jobScenarioId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobScenarioInformationDto eRPJobScenarioInformationDto = await base.ERPJobScenarioRepository.GetJobScenario(jobScenarioId);
				string text = await base.ERPJobScenarioRepository.WhereUsed("JobScenarios", new object[1] { eRPJobScenarioInformationDto.jmnJobScenarioID }, new object[1] { "jmnJobScenarioID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobScenario cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobScenarioDto>> Process_DeleteJobScenario(Guid jobScenarioId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobScenarioDto> result;
		try
		{
			IERPJobScenarioRepository iERPJobScenarioRepository = (base.ERPJobScenarioRepository = new ERPJobScenarioRepository(base.ApiClientContext));
			using (iERPJobScenarioRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobScenarioRepository.DeleteRowFromTable("JobScenarios", "jmn", jobScenarioId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobScenario [{jobScenarioId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobScenarioDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobScenarioDto()
			};
		}
		return result;
	}
}
