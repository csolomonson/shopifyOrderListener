using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLaserCalculatorLineModel : ERPBaseModel, IERPLaserCalculatorLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLaserCalculatorLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
		using (iERPLaserCalculatorLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLaserCalculatorLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLaserCalculatorLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLaserCalculatorLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLaserCalculatorLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLaserCalculatorLine(Guid laserCalculatorLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
		using (iERPLaserCalculatorLineRepository)
		{
			if (!(await base.ERPLaserCalculatorLineRepository.DoesLaserCalculatorLineExist(laserCalculatorLineId)))
			{
				errorsList.Add($"LaserCalculatorLine [{laserCalculatorLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
		using (iERPLaserCalculatorLineRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLaserCalculatorLineDto>>> Process_GetAllLaserCalculatorLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLaserCalculatorLineDto> allLaserCalculatorLinesDto = new List<ERPLaserCalculatorLineDto>();
		ERPResponseMessageDto<IList<ERPLaserCalculatorLineDto>> result;
		try
		{
			IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
			using (iERPLaserCalculatorLineRepository)
			{
				foreach (ERPLaserCalculatorLineInformationDto item2 in await base.ERPLaserCalculatorLineRepository.GetAllLaserCalculatorLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPLaserCalculatorLineDto item = new ERPLaserCalculatorLineDto
					{
						cclCreatedBy = item2.cclCreatedBy,
						cclCreatedDate = item2.cclCreatedDate,
						cclCutTime = item2.cclCutTime,
						cclDescription = item2.cclDescription,
						cclUniqueID = item2.cclUniqueID,
						cclLaserCalculatorID = item2.cclLaserCalculatorID,
						ccllength = item2.ccllength,
						cclQuantity = item2.cclQuantity,
						cclRate = item2.cclRate,
						cclRowVersion = item2.cclRowVersion,
						cclLaserCalculatorLineID = item2.cclLaserCalculatorLineID,
						cclWidth = item2.cclWidth,
						CustomFields = item2.CustomFields
					};
					allLaserCalculatorLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LaserCalculatorLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLaserCalculatorLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLaserCalculatorLinesDto,
				RecordCount = allLaserCalculatorLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_GetLaserCalculatorLine(Guid laserCalculatorLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLaserCalculatorLineDto laserCalculatorLineDto = null;
		ERPResponseMessageDto<ERPLaserCalculatorLineDto> result;
		try
		{
			IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
			using (iERPLaserCalculatorLineRepository)
			{
				ERPLaserCalculatorLineInformationDto eRPLaserCalculatorLineInformationDto = await base.ERPLaserCalculatorLineRepository.GetLaserCalculatorLine(laserCalculatorLineId);
				laserCalculatorLineDto = new ERPLaserCalculatorLineDto
				{
					cclCreatedBy = eRPLaserCalculatorLineInformationDto.cclCreatedBy,
					cclCreatedDate = eRPLaserCalculatorLineInformationDto.cclCreatedDate,
					cclCutTime = eRPLaserCalculatorLineInformationDto.cclCutTime,
					cclDescription = eRPLaserCalculatorLineInformationDto.cclDescription,
					cclUniqueID = eRPLaserCalculatorLineInformationDto.cclUniqueID,
					cclLaserCalculatorID = eRPLaserCalculatorLineInformationDto.cclLaserCalculatorID,
					ccllength = eRPLaserCalculatorLineInformationDto.ccllength,
					cclQuantity = eRPLaserCalculatorLineInformationDto.cclQuantity,
					cclRate = eRPLaserCalculatorLineInformationDto.cclRate,
					cclRowVersion = eRPLaserCalculatorLineInformationDto.cclRowVersion,
					cclLaserCalculatorLineID = eRPLaserCalculatorLineInformationDto.cclLaserCalculatorLineID,
					cclWidth = eRPLaserCalculatorLineInformationDto.cclWidth,
					CustomFields = eRPLaserCalculatorLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LaserCalculatorLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = laserCalculatorLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_PutLaserCalculatorLine(ERPLaserCalculatorLineDto laserCalculatorLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLaserCalculatorLineDto createdObject = null;
		ERPResponseMessageDto<ERPLaserCalculatorLineDto> result;
		try
		{
			IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
			using (iERPLaserCalculatorLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLaserCalculatorLineRepository.SaveLaserCalculatorLine(laserCalculatorLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLaserCalculatorLineInformationDto eRPLaserCalculatorLineInformationDto = await base.ERPLaserCalculatorLineRepository.GetLaserCalculatorLine(laserCalculatorLine.cclUniqueID);
					createdObject = new ERPLaserCalculatorLineDto
					{
						cclCreatedBy = eRPLaserCalculatorLineInformationDto.cclCreatedBy,
						cclCreatedDate = eRPLaserCalculatorLineInformationDto.cclCreatedDate,
						cclCutTime = eRPLaserCalculatorLineInformationDto.cclCutTime,
						cclDescription = eRPLaserCalculatorLineInformationDto.cclDescription,
						cclUniqueID = eRPLaserCalculatorLineInformationDto.cclUniqueID,
						cclLaserCalculatorID = eRPLaserCalculatorLineInformationDto.cclLaserCalculatorID,
						ccllength = eRPLaserCalculatorLineInformationDto.ccllength,
						cclQuantity = eRPLaserCalculatorLineInformationDto.cclQuantity,
						cclRate = eRPLaserCalculatorLineInformationDto.cclRate,
						cclRowVersion = eRPLaserCalculatorLineInformationDto.cclRowVersion,
						cclLaserCalculatorLineID = eRPLaserCalculatorLineInformationDto.cclLaserCalculatorLineID,
						cclWidth = eRPLaserCalculatorLineInformationDto.cclWidth,
						CustomFields = eRPLaserCalculatorLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LaserCalculatorLine [{laserCalculatorLine.cclUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLaserCalculatorLine(Guid laserCalculatorLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
		using (iERPLaserCalculatorLineRepository)
		{
			if (!(await base.ERPLaserCalculatorLineRepository.DoesLaserCalculatorLineExist(laserCalculatorLineId)))
			{
				base.ErrorsList.Add($"LaserCalculatorLine [{laserCalculatorLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLaserCalculatorLineInformationDto eRPLaserCalculatorLineInformationDto = await base.ERPLaserCalculatorLineRepository.GetLaserCalculatorLine(laserCalculatorLineId);
				string text = await base.ERPLaserCalculatorLineRepository.WhereUsed("LaserCalculatorLines", new object[2] { eRPLaserCalculatorLineInformationDto.cclLaserCalculatorID, eRPLaserCalculatorLineInformationDto.cclLaserCalculatorLineID }, new object[2] { "cclLaserCalculatorID", "cclLaserCalculatorLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LaserCalculatorLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorLineDto>> Process_DeleteLaserCalculatorLine(Guid laserCalculatorLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLaserCalculatorLineDto> result;
		try
		{
			IERPLaserCalculatorLineRepository iERPLaserCalculatorLineRepository = (base.ERPLaserCalculatorLineRepository = new ERPLaserCalculatorLineRepository(base.ApiClientContext));
			using (iERPLaserCalculatorLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLaserCalculatorLineRepository.DeleteRowFromTable("LaserCalculatorLines", "ccl", laserCalculatorLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LaserCalculatorLine [{laserCalculatorLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLaserCalculatorLineDto()
			};
		}
		return result;
	}
}
