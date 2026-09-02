using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSheetCalculatorModel : ERPBaseModel, IERPSheetCalculatorModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSheetCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
		using (iERPSheetCalculatorRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSheetCalculatorRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSheetCalculatorRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSheetCalculatorRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSheetCalculatorRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSheetCalculator(Guid sheetCalculatorId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
		using (iERPSheetCalculatorRepository)
		{
			if (!(await base.ERPSheetCalculatorRepository.DoesSheetCalculatorExist(sheetCalculatorId)))
			{
				errorsList.Add($"SheetCalculator [{sheetCalculatorId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSheetCalculator(ERPSheetCalculatorDto sheetCalculator)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
		using (iERPSheetCalculatorRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSheetCalculatorDto>>> Process_GetAllSheetCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSheetCalculatorDto> allSheetCalculatorsDto = new List<ERPSheetCalculatorDto>();
		ERPResponseMessageDto<IList<ERPSheetCalculatorDto>> result;
		try
		{
			IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
			using (iERPSheetCalculatorRepository)
			{
				foreach (ERPSheetCalculatorInformationDto item2 in await base.ERPSheetCalculatorRepository.GetAllSheetCalculators(pageSize, pageNumber, filter, orderBy))
				{
					ERPSheetCalculatorDto item = new ERPSheetCalculatorDto
					{
						ccs0Rotation = item2.ccs0Rotation,
						ccs90Rotation = item2.ccs90Rotation,
						ccsSheetCalculatorID = item2.ccsSheetCalculatorID,
						ccsCreatedBy = item2.ccsCreatedBy,
						ccsCreatedDate = item2.ccsCreatedDate,
						ccsUniqueID = item2.ccsUniqueID,
						ccsGrain = item2.ccsGrain,
						ccsMeasurementType = item2.ccsMeasurementType,
						ccsPartSizeX = item2.ccsPartSizeX,
						ccsPartSizeY = item2.ccsPartSizeY,
						ccsPartSpacingX = item2.ccsPartSpacingX,
						ccsPartSpacingY = item2.ccsPartSpacingY,
						ccsRowVersion = item2.ccsRowVersion,
						ccsSheetSizeX = item2.ccsSheetSizeX,
						ccsSheetSizeY = item2.ccsSheetSizeY,
						ccsTotalTrimX = item2.ccsTotalTrimX,
						ccsTotalTrimY = item2.ccsTotalTrimY,
						CustomFields = item2.CustomFields
					};
					allSheetCalculatorsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SheetCalculators]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSheetCalculatorDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSheetCalculatorsDto,
				RecordCount = allSheetCalculatorsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_GetSheetCalculator(Guid sheetCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSheetCalculatorDto sheetCalculatorDto = null;
		ERPResponseMessageDto<ERPSheetCalculatorDto> result;
		try
		{
			IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
			using (iERPSheetCalculatorRepository)
			{
				ERPSheetCalculatorInformationDto eRPSheetCalculatorInformationDto = await base.ERPSheetCalculatorRepository.GetSheetCalculator(sheetCalculatorId);
				sheetCalculatorDto = new ERPSheetCalculatorDto
				{
					ccs0Rotation = eRPSheetCalculatorInformationDto.ccs0Rotation,
					ccs90Rotation = eRPSheetCalculatorInformationDto.ccs90Rotation,
					ccsSheetCalculatorID = eRPSheetCalculatorInformationDto.ccsSheetCalculatorID,
					ccsCreatedBy = eRPSheetCalculatorInformationDto.ccsCreatedBy,
					ccsCreatedDate = eRPSheetCalculatorInformationDto.ccsCreatedDate,
					ccsUniqueID = eRPSheetCalculatorInformationDto.ccsUniqueID,
					ccsGrain = eRPSheetCalculatorInformationDto.ccsGrain,
					ccsMeasurementType = eRPSheetCalculatorInformationDto.ccsMeasurementType,
					ccsPartSizeX = eRPSheetCalculatorInformationDto.ccsPartSizeX,
					ccsPartSizeY = eRPSheetCalculatorInformationDto.ccsPartSizeY,
					ccsPartSpacingX = eRPSheetCalculatorInformationDto.ccsPartSpacingX,
					ccsPartSpacingY = eRPSheetCalculatorInformationDto.ccsPartSpacingY,
					ccsRowVersion = eRPSheetCalculatorInformationDto.ccsRowVersion,
					ccsSheetSizeX = eRPSheetCalculatorInformationDto.ccsSheetSizeX,
					ccsSheetSizeY = eRPSheetCalculatorInformationDto.ccsSheetSizeY,
					ccsTotalTrimX = eRPSheetCalculatorInformationDto.ccsTotalTrimX,
					ccsTotalTrimY = eRPSheetCalculatorInformationDto.ccsTotalTrimY,
					CustomFields = eRPSheetCalculatorInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SheetCalculators []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSheetCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = sheetCalculatorDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_PutSheetCalculator(ERPSheetCalculatorDto sheetCalculator)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSheetCalculatorDto createdObject = null;
		ERPResponseMessageDto<ERPSheetCalculatorDto> result;
		try
		{
			IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
			using (iERPSheetCalculatorRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSheetCalculatorRepository.SaveSheetCalculator(sheetCalculator);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSheetCalculatorInformationDto eRPSheetCalculatorInformationDto = await base.ERPSheetCalculatorRepository.GetSheetCalculator(sheetCalculator.ccsUniqueID);
					createdObject = new ERPSheetCalculatorDto
					{
						ccs0Rotation = eRPSheetCalculatorInformationDto.ccs0Rotation,
						ccs90Rotation = eRPSheetCalculatorInformationDto.ccs90Rotation,
						ccsSheetCalculatorID = eRPSheetCalculatorInformationDto.ccsSheetCalculatorID,
						ccsCreatedBy = eRPSheetCalculatorInformationDto.ccsCreatedBy,
						ccsCreatedDate = eRPSheetCalculatorInformationDto.ccsCreatedDate,
						ccsUniqueID = eRPSheetCalculatorInformationDto.ccsUniqueID,
						ccsGrain = eRPSheetCalculatorInformationDto.ccsGrain,
						ccsMeasurementType = eRPSheetCalculatorInformationDto.ccsMeasurementType,
						ccsPartSizeX = eRPSheetCalculatorInformationDto.ccsPartSizeX,
						ccsPartSizeY = eRPSheetCalculatorInformationDto.ccsPartSizeY,
						ccsPartSpacingX = eRPSheetCalculatorInformationDto.ccsPartSpacingX,
						ccsPartSpacingY = eRPSheetCalculatorInformationDto.ccsPartSpacingY,
						ccsRowVersion = eRPSheetCalculatorInformationDto.ccsRowVersion,
						ccsSheetSizeX = eRPSheetCalculatorInformationDto.ccsSheetSizeX,
						ccsSheetSizeY = eRPSheetCalculatorInformationDto.ccsSheetSizeY,
						ccsTotalTrimX = eRPSheetCalculatorInformationDto.ccsTotalTrimX,
						ccsTotalTrimY = eRPSheetCalculatorInformationDto.ccsTotalTrimY,
						CustomFields = eRPSheetCalculatorInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SheetCalculator [{sheetCalculator.ccsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSheetCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSheetCalculator(Guid sheetCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
		using (iERPSheetCalculatorRepository)
		{
			if (!(await base.ERPSheetCalculatorRepository.DoesSheetCalculatorExist(sheetCalculatorId)))
			{
				base.ErrorsList.Add($"SheetCalculator [{sheetCalculatorId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSheetCalculatorInformationDto eRPSheetCalculatorInformationDto = await base.ERPSheetCalculatorRepository.GetSheetCalculator(sheetCalculatorId);
				string text = await base.ERPSheetCalculatorRepository.WhereUsed("SheetCalculators", new object[1] { eRPSheetCalculatorInformationDto.ccsSheetCalculatorID }, new object[1] { "ccsSheetCalculatorID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SheetCalculator cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSheetCalculatorDto>> Process_DeleteSheetCalculator(Guid sheetCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSheetCalculatorDto> result;
		try
		{
			IERPSheetCalculatorRepository iERPSheetCalculatorRepository = (base.ERPSheetCalculatorRepository = new ERPSheetCalculatorRepository(base.ApiClientContext));
			using (iERPSheetCalculatorRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSheetCalculatorRepository.DeleteRowFromTable("SheetCalculators", "ccs", sheetCalculatorId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SheetCalculator [{sheetCalculatorId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSheetCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSheetCalculatorDto()
			};
		}
		return result;
	}
}
