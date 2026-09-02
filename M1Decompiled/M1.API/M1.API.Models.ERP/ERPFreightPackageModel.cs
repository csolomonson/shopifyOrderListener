using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFreightPackageModel : ERPBaseModel, IERPFreightPackageModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
		using (iERPFreightPackageRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFreightPackageRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFreightPackageRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFreightPackageRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFreightPackageRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFreightPackage(Guid freightPackageId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
		using (iERPFreightPackageRepository)
		{
			if (!(await base.ERPFreightPackageRepository.DoesFreightPackageExist(freightPackageId)))
			{
				errorsList.Add($"FreightPackage [{freightPackageId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFreightPackage(ERPFreightPackageDto freightPackage)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
		using (iERPFreightPackageRepository)
		{
			if (!string.IsNullOrWhiteSpace(freightPackage.fslFreightShipmentID) && !(await base.ERPFreightPackageRepository.DoesRecordExistInTableUsingKeys("FreightShipments", new object[1] { "FSPFREIGHTSHIPMENTID" }, new object[1] { freightPackage.fslFreightShipmentID })))
			{
				errorsList.Add("fslFreightShipmentID [" + freightPackage.fslFreightShipmentID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFreightPackageDto>>> Process_GetAllFreightPackages(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFreightPackageDto> allFreightPackagesDto = new List<ERPFreightPackageDto>();
		ERPResponseMessageDto<IList<ERPFreightPackageDto>> result;
		try
		{
			IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
			using (iERPFreightPackageRepository)
			{
				foreach (ERPFreightPackageInformationDto item2 in await base.ERPFreightPackageRepository.GetAllFreightPackages(pageSize, pageNumber, filter, orderBy))
				{
					ERPFreightPackageDto item = new ERPFreightPackageDto
					{
						fslCreatedBy = item2.fslCreatedBy,
						fslCreatedDate = item2.fslCreatedDate,
						fslDimensionsUnitOfMeasure = item2.fslDimensionsUnitOfMeasure,
						fslDistributeCostsOption = item2.fslDistributeCostsOption,
						fslUniqueID = item2.fslUniqueID,
						fslFdxPackageHeight = item2.fslFdxPackageHeight,
						fslFdxPackageLength = item2.fslFdxPackageLength,
						fslFdxPackageWidth = item2.fslFdxPackageWidth,
						fslFdxPackaging = item2.fslFdxPackaging,
						fslFreightShipmentID = item2.fslFreightShipmentID,
						fslFdxNonstandardContainer = item2.fslFdxNonstandardContainer,
						fslVoidOnUps = item2.fslVoidOnUps,
						fslNotesRTF = item2.fslNotesRTF,
						fslNotesText = item2.fslNotesText,
						fslPackageCharge = item2.fslPackageCharge,
						fslPackageFullWeight = item2.fslPackageFullWeight,
						fslPackagePublishedCharge = item2.fslPackagePublishedCharge,
						fslRowVersion = item2.fslRowVersion,
						fslFreightPackageID = item2.fslFreightPackageID,
						fslTrackingNumber = item2.fslTrackingNumber,
						fslUpsPackageType = item2.fslUpsPackageType,
						fslWeightUnitOfMeasure = item2.fslWeightUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allFreightPackagesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FreightPackages]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFreightPackageDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFreightPackagesDto,
				RecordCount = allFreightPackagesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_GetFreightPackage(Guid freightPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFreightPackageDto freightPackageDto = null;
		ERPResponseMessageDto<ERPFreightPackageDto> result;
		try
		{
			IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
			using (iERPFreightPackageRepository)
			{
				ERPFreightPackageInformationDto eRPFreightPackageInformationDto = await base.ERPFreightPackageRepository.GetFreightPackage(freightPackageId);
				freightPackageDto = new ERPFreightPackageDto
				{
					fslCreatedBy = eRPFreightPackageInformationDto.fslCreatedBy,
					fslCreatedDate = eRPFreightPackageInformationDto.fslCreatedDate,
					fslDimensionsUnitOfMeasure = eRPFreightPackageInformationDto.fslDimensionsUnitOfMeasure,
					fslDistributeCostsOption = eRPFreightPackageInformationDto.fslDistributeCostsOption,
					fslUniqueID = eRPFreightPackageInformationDto.fslUniqueID,
					fslFdxPackageHeight = eRPFreightPackageInformationDto.fslFdxPackageHeight,
					fslFdxPackageLength = eRPFreightPackageInformationDto.fslFdxPackageLength,
					fslFdxPackageWidth = eRPFreightPackageInformationDto.fslFdxPackageWidth,
					fslFdxPackaging = eRPFreightPackageInformationDto.fslFdxPackaging,
					fslFreightShipmentID = eRPFreightPackageInformationDto.fslFreightShipmentID,
					fslFdxNonstandardContainer = eRPFreightPackageInformationDto.fslFdxNonstandardContainer,
					fslVoidOnUps = eRPFreightPackageInformationDto.fslVoidOnUps,
					fslNotesRTF = eRPFreightPackageInformationDto.fslNotesRTF,
					fslNotesText = eRPFreightPackageInformationDto.fslNotesText,
					fslPackageCharge = eRPFreightPackageInformationDto.fslPackageCharge,
					fslPackageFullWeight = eRPFreightPackageInformationDto.fslPackageFullWeight,
					fslPackagePublishedCharge = eRPFreightPackageInformationDto.fslPackagePublishedCharge,
					fslRowVersion = eRPFreightPackageInformationDto.fslRowVersion,
					fslFreightPackageID = eRPFreightPackageInformationDto.fslFreightPackageID,
					fslTrackingNumber = eRPFreightPackageInformationDto.fslTrackingNumber,
					fslUpsPackageType = eRPFreightPackageInformationDto.fslUpsPackageType,
					fslWeightUnitOfMeasure = eRPFreightPackageInformationDto.fslWeightUnitOfMeasure,
					CustomFields = eRPFreightPackageInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FreightPackages []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = freightPackageDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_PutFreightPackage(ERPFreightPackageDto freightPackage)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFreightPackageDto createdObject = null;
		ERPResponseMessageDto<ERPFreightPackageDto> result;
		try
		{
			IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
			using (iERPFreightPackageRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFreightPackageRepository.SaveFreightPackage(freightPackage);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFreightPackageInformationDto eRPFreightPackageInformationDto = await base.ERPFreightPackageRepository.GetFreightPackage(freightPackage.fslUniqueID);
					createdObject = new ERPFreightPackageDto
					{
						fslCreatedBy = eRPFreightPackageInformationDto.fslCreatedBy,
						fslCreatedDate = eRPFreightPackageInformationDto.fslCreatedDate,
						fslDimensionsUnitOfMeasure = eRPFreightPackageInformationDto.fslDimensionsUnitOfMeasure,
						fslDistributeCostsOption = eRPFreightPackageInformationDto.fslDistributeCostsOption,
						fslUniqueID = eRPFreightPackageInformationDto.fslUniqueID,
						fslFdxPackageHeight = eRPFreightPackageInformationDto.fslFdxPackageHeight,
						fslFdxPackageLength = eRPFreightPackageInformationDto.fslFdxPackageLength,
						fslFdxPackageWidth = eRPFreightPackageInformationDto.fslFdxPackageWidth,
						fslFdxPackaging = eRPFreightPackageInformationDto.fslFdxPackaging,
						fslFreightShipmentID = eRPFreightPackageInformationDto.fslFreightShipmentID,
						fslFdxNonstandardContainer = eRPFreightPackageInformationDto.fslFdxNonstandardContainer,
						fslVoidOnUps = eRPFreightPackageInformationDto.fslVoidOnUps,
						fslNotesRTF = eRPFreightPackageInformationDto.fslNotesRTF,
						fslNotesText = eRPFreightPackageInformationDto.fslNotesText,
						fslPackageCharge = eRPFreightPackageInformationDto.fslPackageCharge,
						fslPackageFullWeight = eRPFreightPackageInformationDto.fslPackageFullWeight,
						fslPackagePublishedCharge = eRPFreightPackageInformationDto.fslPackagePublishedCharge,
						fslRowVersion = eRPFreightPackageInformationDto.fslRowVersion,
						fslFreightPackageID = eRPFreightPackageInformationDto.fslFreightPackageID,
						fslTrackingNumber = eRPFreightPackageInformationDto.fslTrackingNumber,
						fslUpsPackageType = eRPFreightPackageInformationDto.fslUpsPackageType,
						fslWeightUnitOfMeasure = eRPFreightPackageInformationDto.fslWeightUnitOfMeasure,
						CustomFields = eRPFreightPackageInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing FreightPackage [{freightPackage.fslUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackage(Guid freightPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
		using (iERPFreightPackageRepository)
		{
			if (!(await base.ERPFreightPackageRepository.DoesFreightPackageExist(freightPackageId)))
			{
				base.ErrorsList.Add($"FreightPackage [{freightPackageId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFreightPackageInformationDto eRPFreightPackageInformationDto = await base.ERPFreightPackageRepository.GetFreightPackage(freightPackageId);
				string text = await base.ERPFreightPackageRepository.WhereUsed("FreightPackages", new object[2] { eRPFreightPackageInformationDto.fslFreightShipmentID, eRPFreightPackageInformationDto.fslFreightPackageID }, new object[2] { "fslFreightShipmentID", "fslFreightPackageID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("FreightPackage cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageDto>> Process_DeleteFreightPackage(Guid freightPackageId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFreightPackageDto> result;
		try
		{
			IERPFreightPackageRepository iERPFreightPackageRepository = (base.ERPFreightPackageRepository = new ERPFreightPackageRepository(base.ApiClientContext));
			using (iERPFreightPackageRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFreightPackageRepository.DeleteRowFromTable("FreightPackages", "fsl", freightPackageId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of FreightPackage [{freightPackageId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFreightPackageDto()
			};
		}
		return result;
	}
}
