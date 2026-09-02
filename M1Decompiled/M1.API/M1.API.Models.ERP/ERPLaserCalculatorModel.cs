using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLaserCalculatorModel : ERPBaseModel, IERPLaserCalculatorModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLaserCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
		using (iERPLaserCalculatorRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLaserCalculatorRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLaserCalculatorRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLaserCalculatorRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLaserCalculatorRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLaserCalculator(Guid laserCalculatorId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
		using (iERPLaserCalculatorRepository)
		{
			if (!(await base.ERPLaserCalculatorRepository.DoesLaserCalculatorExist(laserCalculatorId)))
			{
				errorsList.Add($"LaserCalculator [{laserCalculatorId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLaserCalculator(ERPLaserCalculatorDto laserCalculator)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
		using (iERPLaserCalculatorRepository)
		{
			if (!string.IsNullOrWhiteSpace(laserCalculator.ccpLaserMaterialTypeID) && !(await base.ERPLaserCalculatorRepository.DoesRecordExistInTableUsingKeys("LaserMaterialTypes", new object[1] { "CCMLASERMATERIALTYPEID" }, new object[1] { laserCalculator.ccpLaserMaterialTypeID })))
			{
				errorsList.Add("ccpLaserMaterialTypeID [" + laserCalculator.ccpLaserMaterialTypeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLaserCalculatorDto>>> Process_GetAllLaserCalculators(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLaserCalculatorDto> allLaserCalculatorsDto = new List<ERPLaserCalculatorDto>();
		ERPResponseMessageDto<IList<ERPLaserCalculatorDto>> result;
		try
		{
			IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
			using (iERPLaserCalculatorRepository)
			{
				foreach (ERPLaserCalculatorInformationDto item2 in await base.ERPLaserCalculatorRepository.GetAllLaserCalculators(pageSize, pageNumber, filter, orderBy))
				{
					ERPLaserCalculatorDto item = new ERPLaserCalculatorDto
					{
						ccpLaserCalculatorID = item2.ccpLaserCalculatorID,
						ccpCreatedBy = item2.ccpCreatedBy,
						ccpCreatedDate = item2.ccpCreatedDate,
						ccpdescription = item2.ccpdescription,
						ccpUniqueID = item2.ccpUniqueID,
						ccpExternalFeed = item2.ccpExternalFeed,
						ccpHoleCutTime = item2.ccpHoleCutTime,
						ccpObround = item2.ccpObround,
						ccpOther = item2.ccpOther,
						ccpRectangle = item2.ccpRectangle,
						ccpRound = item2.ccpRound,
						ccpSquare = item2.ccpSquare,
						ccpLaserMaterialTypeID = item2.ccpLaserMaterialTypeID,
						ccpLeadInOut = item2.ccpLeadInOut,
						ccpLeadInOutFeed = item2.ccpLeadInOutFeed,
						ccpLeadInOutTime = item2.ccpLeadInOutTime,
						ccplength = item2.ccplength,
						ccpMeasurementType = item2.ccpMeasurementType,
						ccpNumberOfHoles = item2.ccpNumberOfHoles,
						ccpPartPerimeter = item2.ccpPartPerimeter,
						ccpPerimeterCutTime = item2.ccpPerimeterCutTime,
						ccpPiercedHoles = item2.ccpPiercedHoles,
						ccpPierceTime = item2.ccpPierceTime,
						ccpQuantity = item2.ccpQuantity,
						ccpRate = item2.ccpRate,
						ccpRowVersion = item2.ccpRowVersion,
						ccpThickness = item2.ccpThickness,
						ccpTotalCutTime = item2.ccpTotalCutTime,
						ccpTotalLeadInOutTime = item2.ccpTotalLeadInOutTime,
						ccpTotalPierceTime = item2.ccpTotalPierceTime,
						ccpWidth = item2.ccpWidth,
						CustomFields = item2.CustomFields
					};
					allLaserCalculatorsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LaserCalculators]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLaserCalculatorDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLaserCalculatorsDto,
				RecordCount = allLaserCalculatorsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_GetLaserCalculator(Guid laserCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLaserCalculatorDto laserCalculatorDto = null;
		ERPResponseMessageDto<ERPLaserCalculatorDto> result;
		try
		{
			IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
			using (iERPLaserCalculatorRepository)
			{
				ERPLaserCalculatorInformationDto eRPLaserCalculatorInformationDto = await base.ERPLaserCalculatorRepository.GetLaserCalculator(laserCalculatorId);
				laserCalculatorDto = new ERPLaserCalculatorDto
				{
					ccpLaserCalculatorID = eRPLaserCalculatorInformationDto.ccpLaserCalculatorID,
					ccpCreatedBy = eRPLaserCalculatorInformationDto.ccpCreatedBy,
					ccpCreatedDate = eRPLaserCalculatorInformationDto.ccpCreatedDate,
					ccpdescription = eRPLaserCalculatorInformationDto.ccpdescription,
					ccpUniqueID = eRPLaserCalculatorInformationDto.ccpUniqueID,
					ccpExternalFeed = eRPLaserCalculatorInformationDto.ccpExternalFeed,
					ccpHoleCutTime = eRPLaserCalculatorInformationDto.ccpHoleCutTime,
					ccpObround = eRPLaserCalculatorInformationDto.ccpObround,
					ccpOther = eRPLaserCalculatorInformationDto.ccpOther,
					ccpRectangle = eRPLaserCalculatorInformationDto.ccpRectangle,
					ccpRound = eRPLaserCalculatorInformationDto.ccpRound,
					ccpSquare = eRPLaserCalculatorInformationDto.ccpSquare,
					ccpLaserMaterialTypeID = eRPLaserCalculatorInformationDto.ccpLaserMaterialTypeID,
					ccpLeadInOut = eRPLaserCalculatorInformationDto.ccpLeadInOut,
					ccpLeadInOutFeed = eRPLaserCalculatorInformationDto.ccpLeadInOutFeed,
					ccpLeadInOutTime = eRPLaserCalculatorInformationDto.ccpLeadInOutTime,
					ccplength = eRPLaserCalculatorInformationDto.ccplength,
					ccpMeasurementType = eRPLaserCalculatorInformationDto.ccpMeasurementType,
					ccpNumberOfHoles = eRPLaserCalculatorInformationDto.ccpNumberOfHoles,
					ccpPartPerimeter = eRPLaserCalculatorInformationDto.ccpPartPerimeter,
					ccpPerimeterCutTime = eRPLaserCalculatorInformationDto.ccpPerimeterCutTime,
					ccpPiercedHoles = eRPLaserCalculatorInformationDto.ccpPiercedHoles,
					ccpPierceTime = eRPLaserCalculatorInformationDto.ccpPierceTime,
					ccpQuantity = eRPLaserCalculatorInformationDto.ccpQuantity,
					ccpRate = eRPLaserCalculatorInformationDto.ccpRate,
					ccpRowVersion = eRPLaserCalculatorInformationDto.ccpRowVersion,
					ccpThickness = eRPLaserCalculatorInformationDto.ccpThickness,
					ccpTotalCutTime = eRPLaserCalculatorInformationDto.ccpTotalCutTime,
					ccpTotalLeadInOutTime = eRPLaserCalculatorInformationDto.ccpTotalLeadInOutTime,
					ccpTotalPierceTime = eRPLaserCalculatorInformationDto.ccpTotalPierceTime,
					ccpWidth = eRPLaserCalculatorInformationDto.ccpWidth,
					CustomFields = eRPLaserCalculatorInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LaserCalculators []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = laserCalculatorDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_PutLaserCalculator(ERPLaserCalculatorDto laserCalculator)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLaserCalculatorDto createdObject = null;
		ERPResponseMessageDto<ERPLaserCalculatorDto> result;
		try
		{
			IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
			using (iERPLaserCalculatorRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLaserCalculatorRepository.SaveLaserCalculator(laserCalculator);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLaserCalculatorInformationDto eRPLaserCalculatorInformationDto = await base.ERPLaserCalculatorRepository.GetLaserCalculator(laserCalculator.ccpUniqueID);
					createdObject = new ERPLaserCalculatorDto
					{
						ccpLaserCalculatorID = eRPLaserCalculatorInformationDto.ccpLaserCalculatorID,
						ccpCreatedBy = eRPLaserCalculatorInformationDto.ccpCreatedBy,
						ccpCreatedDate = eRPLaserCalculatorInformationDto.ccpCreatedDate,
						ccpdescription = eRPLaserCalculatorInformationDto.ccpdescription,
						ccpUniqueID = eRPLaserCalculatorInformationDto.ccpUniqueID,
						ccpExternalFeed = eRPLaserCalculatorInformationDto.ccpExternalFeed,
						ccpHoleCutTime = eRPLaserCalculatorInformationDto.ccpHoleCutTime,
						ccpObround = eRPLaserCalculatorInformationDto.ccpObround,
						ccpOther = eRPLaserCalculatorInformationDto.ccpOther,
						ccpRectangle = eRPLaserCalculatorInformationDto.ccpRectangle,
						ccpRound = eRPLaserCalculatorInformationDto.ccpRound,
						ccpSquare = eRPLaserCalculatorInformationDto.ccpSquare,
						ccpLaserMaterialTypeID = eRPLaserCalculatorInformationDto.ccpLaserMaterialTypeID,
						ccpLeadInOut = eRPLaserCalculatorInformationDto.ccpLeadInOut,
						ccpLeadInOutFeed = eRPLaserCalculatorInformationDto.ccpLeadInOutFeed,
						ccpLeadInOutTime = eRPLaserCalculatorInformationDto.ccpLeadInOutTime,
						ccplength = eRPLaserCalculatorInformationDto.ccplength,
						ccpMeasurementType = eRPLaserCalculatorInformationDto.ccpMeasurementType,
						ccpNumberOfHoles = eRPLaserCalculatorInformationDto.ccpNumberOfHoles,
						ccpPartPerimeter = eRPLaserCalculatorInformationDto.ccpPartPerimeter,
						ccpPerimeterCutTime = eRPLaserCalculatorInformationDto.ccpPerimeterCutTime,
						ccpPiercedHoles = eRPLaserCalculatorInformationDto.ccpPiercedHoles,
						ccpPierceTime = eRPLaserCalculatorInformationDto.ccpPierceTime,
						ccpQuantity = eRPLaserCalculatorInformationDto.ccpQuantity,
						ccpRate = eRPLaserCalculatorInformationDto.ccpRate,
						ccpRowVersion = eRPLaserCalculatorInformationDto.ccpRowVersion,
						ccpThickness = eRPLaserCalculatorInformationDto.ccpThickness,
						ccpTotalCutTime = eRPLaserCalculatorInformationDto.ccpTotalCutTime,
						ccpTotalLeadInOutTime = eRPLaserCalculatorInformationDto.ccpTotalLeadInOutTime,
						ccpTotalPierceTime = eRPLaserCalculatorInformationDto.ccpTotalPierceTime,
						ccpWidth = eRPLaserCalculatorInformationDto.ccpWidth,
						CustomFields = eRPLaserCalculatorInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LaserCalculator [{laserCalculator.ccpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLaserCalculator(Guid laserCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
		using (iERPLaserCalculatorRepository)
		{
			if (!(await base.ERPLaserCalculatorRepository.DoesLaserCalculatorExist(laserCalculatorId)))
			{
				base.ErrorsList.Add($"LaserCalculator [{laserCalculatorId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLaserCalculatorInformationDto eRPLaserCalculatorInformationDto = await base.ERPLaserCalculatorRepository.GetLaserCalculator(laserCalculatorId);
				string text = await base.ERPLaserCalculatorRepository.WhereUsed("LaserCalculators", new object[1] { eRPLaserCalculatorInformationDto.ccpLaserCalculatorID }, new object[1] { "ccpLaserCalculatorID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LaserCalculator cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLaserCalculatorDto>> Process_DeleteLaserCalculator(Guid laserCalculatorId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLaserCalculatorDto> result;
		try
		{
			IERPLaserCalculatorRepository iERPLaserCalculatorRepository = (base.ERPLaserCalculatorRepository = new ERPLaserCalculatorRepository(base.ApiClientContext));
			using (iERPLaserCalculatorRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLaserCalculatorRepository.DeleteRowFromTable("LaserCalculators", "ccp", laserCalculatorId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LaserCalculator [{laserCalculatorId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLaserCalculatorDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLaserCalculatorDto()
			};
		}
		return result;
	}
}
