using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMarketingProgramModel : ERPBaseModel, IERPMarketingProgramModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMarketingPrograms(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
		using (iERPMarketingProgramRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMarketingProgramRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMarketingProgramRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMarketingProgramRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMarketingProgramRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMarketingProgram(Guid marketingProgramId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
		using (iERPMarketingProgramRepository)
		{
			if (!(await base.ERPMarketingProgramRepository.DoesMarketingProgramExist(marketingProgramId)))
			{
				errorsList.Add($"MarketingProgram [{marketingProgramId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMarketingProgram(ERPMarketingProgramDto marketingProgram)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
		using (iERPMarketingProgramRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMarketingProgramDto>>> Process_GetAllMarketingPrograms(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMarketingProgramDto> allMarketingProgramsDto = new List<ERPMarketingProgramDto>();
		ERPResponseMessageDto<IList<ERPMarketingProgramDto>> result;
		try
		{
			IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
			using (iERPMarketingProgramRepository)
			{
				foreach (ERPMarketingProgramInformationDto item2 in await base.ERPMarketingProgramRepository.GetAllMarketingPrograms(pageSize, pageNumber, filter, orderBy))
				{
					ERPMarketingProgramDto item = new ERPMarketingProgramDto
					{
						looActivityType = item2.looActivityType,
						looMarketingProgramID = item2.looMarketingProgramID,
						looCreatedBy = item2.looCreatedBy,
						looCreatedDate = item2.looCreatedDate,
						looEndDate = item2.looEndDate,
						looUniqueID = item2.looUniqueID,
						looExpectedRevenue = item2.looExpectedRevenue,
						looInactiveDate = item2.looInactiveDate,
						looInactive = item2.looInactive,
						looLongDescriptionRtf = item2.looLongDescriptionRtf,
						looLongDescriptionText = item2.looLongDescriptionText,
						looMarketingCost = item2.looMarketingCost,
						looRowVersion = item2.looRowVersion,
						looShortDescription = item2.looShortDescription,
						looStartDate = item2.looStartDate,
						CustomFields = item2.CustomFields
					};
					allMarketingProgramsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MarketingPrograms]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMarketingProgramDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMarketingProgramsDto,
				RecordCount = allMarketingProgramsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_GetMarketingProgram(Guid marketingProgramId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMarketingProgramDto marketingProgramDto = null;
		ERPResponseMessageDto<ERPMarketingProgramDto> result;
		try
		{
			IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
			using (iERPMarketingProgramRepository)
			{
				ERPMarketingProgramInformationDto eRPMarketingProgramInformationDto = await base.ERPMarketingProgramRepository.GetMarketingProgram(marketingProgramId);
				marketingProgramDto = new ERPMarketingProgramDto
				{
					looActivityType = eRPMarketingProgramInformationDto.looActivityType,
					looMarketingProgramID = eRPMarketingProgramInformationDto.looMarketingProgramID,
					looCreatedBy = eRPMarketingProgramInformationDto.looCreatedBy,
					looCreatedDate = eRPMarketingProgramInformationDto.looCreatedDate,
					looEndDate = eRPMarketingProgramInformationDto.looEndDate,
					looUniqueID = eRPMarketingProgramInformationDto.looUniqueID,
					looExpectedRevenue = eRPMarketingProgramInformationDto.looExpectedRevenue,
					looInactiveDate = eRPMarketingProgramInformationDto.looInactiveDate,
					looInactive = eRPMarketingProgramInformationDto.looInactive,
					looLongDescriptionRtf = eRPMarketingProgramInformationDto.looLongDescriptionRtf,
					looLongDescriptionText = eRPMarketingProgramInformationDto.looLongDescriptionText,
					looMarketingCost = eRPMarketingProgramInformationDto.looMarketingCost,
					looRowVersion = eRPMarketingProgramInformationDto.looRowVersion,
					looShortDescription = eRPMarketingProgramInformationDto.looShortDescription,
					looStartDate = eRPMarketingProgramInformationDto.looStartDate,
					CustomFields = eRPMarketingProgramInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MarketingPrograms []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMarketingProgramDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = marketingProgramDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_PutMarketingProgram(ERPMarketingProgramDto marketingProgram)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMarketingProgramDto createdObject = null;
		ERPResponseMessageDto<ERPMarketingProgramDto> result;
		try
		{
			IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
			using (iERPMarketingProgramRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMarketingProgramRepository.SaveMarketingProgram(marketingProgram);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMarketingProgramInformationDto eRPMarketingProgramInformationDto = await base.ERPMarketingProgramRepository.GetMarketingProgram(marketingProgram.looUniqueID);
					createdObject = new ERPMarketingProgramDto
					{
						looActivityType = eRPMarketingProgramInformationDto.looActivityType,
						looMarketingProgramID = eRPMarketingProgramInformationDto.looMarketingProgramID,
						looCreatedBy = eRPMarketingProgramInformationDto.looCreatedBy,
						looCreatedDate = eRPMarketingProgramInformationDto.looCreatedDate,
						looEndDate = eRPMarketingProgramInformationDto.looEndDate,
						looUniqueID = eRPMarketingProgramInformationDto.looUniqueID,
						looExpectedRevenue = eRPMarketingProgramInformationDto.looExpectedRevenue,
						looInactiveDate = eRPMarketingProgramInformationDto.looInactiveDate,
						looInactive = eRPMarketingProgramInformationDto.looInactive,
						looLongDescriptionRtf = eRPMarketingProgramInformationDto.looLongDescriptionRtf,
						looLongDescriptionText = eRPMarketingProgramInformationDto.looLongDescriptionText,
						looMarketingCost = eRPMarketingProgramInformationDto.looMarketingCost,
						looRowVersion = eRPMarketingProgramInformationDto.looRowVersion,
						looShortDescription = eRPMarketingProgramInformationDto.looShortDescription,
						looStartDate = eRPMarketingProgramInformationDto.looStartDate,
						CustomFields = eRPMarketingProgramInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MarketingProgram [{marketingProgram.looUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMarketingProgramDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMarketingProgram(Guid marketingProgramId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
		using (iERPMarketingProgramRepository)
		{
			if (!(await base.ERPMarketingProgramRepository.DoesMarketingProgramExist(marketingProgramId)))
			{
				base.ErrorsList.Add($"MarketingProgram [{marketingProgramId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMarketingProgramInformationDto eRPMarketingProgramInformationDto = await base.ERPMarketingProgramRepository.GetMarketingProgram(marketingProgramId);
				string text = await base.ERPMarketingProgramRepository.WhereUsed("MarketingPrograms", new object[1] { eRPMarketingProgramInformationDto.looMarketingProgramID }, new object[1] { "looMarketingProgramID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MarketingProgram cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMarketingProgramDto>> Process_DeleteMarketingProgram(Guid marketingProgramId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMarketingProgramDto> result;
		try
		{
			IERPMarketingProgramRepository iERPMarketingProgramRepository = (base.ERPMarketingProgramRepository = new ERPMarketingProgramRepository(base.ApiClientContext));
			using (iERPMarketingProgramRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMarketingProgramRepository.DeleteRowFromTable("MarketingPrograms", "loo", marketingProgramId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MarketingProgram [{marketingProgramId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMarketingProgramDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMarketingProgramDto()
			};
		}
		return result;
	}
}
