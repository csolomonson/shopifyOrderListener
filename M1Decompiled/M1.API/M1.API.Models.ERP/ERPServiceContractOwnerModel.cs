using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPServiceContractOwnerModel : ERPBaseModel, IERPServiceContractOwnerModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractOwners(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
		using (iERPServiceContractOwnerRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPServiceContractOwnerRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPServiceContractOwnerRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPServiceContractOwnerRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPServiceContractOwnerRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetServiceContractOwner(Guid serviceContractOwnerId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
		using (iERPServiceContractOwnerRepository)
		{
			if (!(await base.ERPServiceContractOwnerRepository.DoesServiceContractOwnerExist(serviceContractOwnerId)))
			{
				errorsList.Add($"ServiceContractOwner [{serviceContractOwnerId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
		using (iERPServiceContractOwnerRepository)
		{
			if (!string.IsNullOrWhiteSpace(serviceContractOwner.kboServiceContractID) && !(await base.ERPServiceContractOwnerRepository.DoesRecordExistInTableUsingKeys("ServiceContracts", new object[1] { "KBSSERVICECONTRACTID" }, new object[1] { serviceContractOwner.kboServiceContractID })))
			{
				errorsList.Add("kboServiceContractID [" + serviceContractOwner.kboServiceContractID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContractOwner.kboOrganizationID) && !(await base.ERPServiceContractOwnerRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { serviceContractOwner.kboOrganizationID })))
			{
				errorsList.Add("kboOrganizationID [" + serviceContractOwner.kboOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPServiceContractOwnerDto>>> Process_GetAllServiceContractOwners(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPServiceContractOwnerDto> allServiceContractOwnersDto = new List<ERPServiceContractOwnerDto>();
		ERPResponseMessageDto<IList<ERPServiceContractOwnerDto>> result;
		try
		{
			IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
			using (iERPServiceContractOwnerRepository)
			{
				foreach (ERPServiceContractOwnerInformationDto item2 in await base.ERPServiceContractOwnerRepository.GetAllServiceContractOwners(pageSize, pageNumber, filter, orderBy))
				{
					ERPServiceContractOwnerDto item = new ERPServiceContractOwnerDto
					{
						kboAddressLine1 = item2.kboAddressLine1,
						kboAddressLine2 = item2.kboAddressLine2,
						kboAddressLine3 = item2.kboAddressLine3,
						kboCity = item2.kboCity,
						kboCountry = item2.kboCountry,
						kboCreatedBy = item2.kboCreatedBy,
						kboCreatedDate = item2.kboCreatedDate,
						kboDeliveryDate = item2.kboDeliveryDate,
						kboEmailAddress = item2.kboEmailAddress,
						kboUniqueID = item2.kboUniqueID,
						kboFaxNumber = item2.kboFaxNumber,
						kboFirstName = item2.kboFirstName,
						kboHomePhoneNumber = item2.kboHomePhoneNumber,
						kboCurrentOwner = item2.kboCurrentOwner,
						kboSameAsAbove = item2.kboSameAsAbove,
						kboTermsAccepted = item2.kboTermsAccepted,
						kboLastName = item2.kboLastName,
						kboMiddleName = item2.kboMiddleName,
						kboMobileNumber = item2.kboMobileNumber,
						kboOrganizationID = item2.kboOrganizationID,
						kboPhysicalAddressLine1 = item2.kboPhysicalAddressLine1,
						kboPhysicalAddressLine2 = item2.kboPhysicalAddressLine2,
						kboPhysicalAddressLine3 = item2.kboPhysicalAddressLine3,
						kboPhysicalCity = item2.kboPhysicalCity,
						kboPhysicalCountry = item2.kboPhysicalCountry,
						kboPhysicalLocationCity = item2.kboPhysicalLocationCity,
						kboPhysicalLocationState = item2.kboPhysicalLocationState,
						kboPhysicalPostCode = item2.kboPhysicalPostCode,
						kboPhysicalState = item2.kboPhysicalState,
						kboPostCode = item2.kboPostCode,
						kboRegisteredDate = item2.kboRegisteredDate,
						kboRowVersion = item2.kboRowVersion,
						kboServiceContractOwnerID = item2.kboServiceContractOwnerID,
						kboServiceContractID = item2.kboServiceContractID,
						kboStartDate = item2.kboStartDate,
						kboState = item2.kboState,
						kboWorkPhoneNumber = item2.kboWorkPhoneNumber,
						CustomFields = item2.CustomFields
					};
					allServiceContractOwnersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ServiceContractOwners]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPServiceContractOwnerDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allServiceContractOwnersDto,
				RecordCount = allServiceContractOwnersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_GetServiceContractOwner(Guid serviceContractOwnerId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPServiceContractOwnerDto serviceContractOwnerDto = null;
		ERPResponseMessageDto<ERPServiceContractOwnerDto> result;
		try
		{
			IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
			using (iERPServiceContractOwnerRepository)
			{
				ERPServiceContractOwnerInformationDto eRPServiceContractOwnerInformationDto = await base.ERPServiceContractOwnerRepository.GetServiceContractOwner(serviceContractOwnerId);
				serviceContractOwnerDto = new ERPServiceContractOwnerDto
				{
					kboAddressLine1 = eRPServiceContractOwnerInformationDto.kboAddressLine1,
					kboAddressLine2 = eRPServiceContractOwnerInformationDto.kboAddressLine2,
					kboAddressLine3 = eRPServiceContractOwnerInformationDto.kboAddressLine3,
					kboCity = eRPServiceContractOwnerInformationDto.kboCity,
					kboCountry = eRPServiceContractOwnerInformationDto.kboCountry,
					kboCreatedBy = eRPServiceContractOwnerInformationDto.kboCreatedBy,
					kboCreatedDate = eRPServiceContractOwnerInformationDto.kboCreatedDate,
					kboDeliveryDate = eRPServiceContractOwnerInformationDto.kboDeliveryDate,
					kboEmailAddress = eRPServiceContractOwnerInformationDto.kboEmailAddress,
					kboUniqueID = eRPServiceContractOwnerInformationDto.kboUniqueID,
					kboFaxNumber = eRPServiceContractOwnerInformationDto.kboFaxNumber,
					kboFirstName = eRPServiceContractOwnerInformationDto.kboFirstName,
					kboHomePhoneNumber = eRPServiceContractOwnerInformationDto.kboHomePhoneNumber,
					kboCurrentOwner = eRPServiceContractOwnerInformationDto.kboCurrentOwner,
					kboSameAsAbove = eRPServiceContractOwnerInformationDto.kboSameAsAbove,
					kboTermsAccepted = eRPServiceContractOwnerInformationDto.kboTermsAccepted,
					kboLastName = eRPServiceContractOwnerInformationDto.kboLastName,
					kboMiddleName = eRPServiceContractOwnerInformationDto.kboMiddleName,
					kboMobileNumber = eRPServiceContractOwnerInformationDto.kboMobileNumber,
					kboOrganizationID = eRPServiceContractOwnerInformationDto.kboOrganizationID,
					kboPhysicalAddressLine1 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine1,
					kboPhysicalAddressLine2 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine2,
					kboPhysicalAddressLine3 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine3,
					kboPhysicalCity = eRPServiceContractOwnerInformationDto.kboPhysicalCity,
					kboPhysicalCountry = eRPServiceContractOwnerInformationDto.kboPhysicalCountry,
					kboPhysicalLocationCity = eRPServiceContractOwnerInformationDto.kboPhysicalLocationCity,
					kboPhysicalLocationState = eRPServiceContractOwnerInformationDto.kboPhysicalLocationState,
					kboPhysicalPostCode = eRPServiceContractOwnerInformationDto.kboPhysicalPostCode,
					kboPhysicalState = eRPServiceContractOwnerInformationDto.kboPhysicalState,
					kboPostCode = eRPServiceContractOwnerInformationDto.kboPostCode,
					kboRegisteredDate = eRPServiceContractOwnerInformationDto.kboRegisteredDate,
					kboRowVersion = eRPServiceContractOwnerInformationDto.kboRowVersion,
					kboServiceContractOwnerID = eRPServiceContractOwnerInformationDto.kboServiceContractOwnerID,
					kboServiceContractID = eRPServiceContractOwnerInformationDto.kboServiceContractID,
					kboStartDate = eRPServiceContractOwnerInformationDto.kboStartDate,
					kboState = eRPServiceContractOwnerInformationDto.kboState,
					kboWorkPhoneNumber = eRPServiceContractOwnerInformationDto.kboWorkPhoneNumber,
					CustomFields = eRPServiceContractOwnerInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ServiceContractOwners []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractOwnerDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serviceContractOwnerDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_PutServiceContractOwner(ERPServiceContractOwnerDto serviceContractOwner)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPServiceContractOwnerDto createdObject = null;
		ERPResponseMessageDto<ERPServiceContractOwnerDto> result;
		try
		{
			IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
			using (iERPServiceContractOwnerRepository)
			{
				APIValidationInfoDto postResult = await base.ERPServiceContractOwnerRepository.SaveServiceContractOwner(serviceContractOwner);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPServiceContractOwnerInformationDto eRPServiceContractOwnerInformationDto = await base.ERPServiceContractOwnerRepository.GetServiceContractOwner(serviceContractOwner.kboUniqueID);
					createdObject = new ERPServiceContractOwnerDto
					{
						kboAddressLine1 = eRPServiceContractOwnerInformationDto.kboAddressLine1,
						kboAddressLine2 = eRPServiceContractOwnerInformationDto.kboAddressLine2,
						kboAddressLine3 = eRPServiceContractOwnerInformationDto.kboAddressLine3,
						kboCity = eRPServiceContractOwnerInformationDto.kboCity,
						kboCountry = eRPServiceContractOwnerInformationDto.kboCountry,
						kboCreatedBy = eRPServiceContractOwnerInformationDto.kboCreatedBy,
						kboCreatedDate = eRPServiceContractOwnerInformationDto.kboCreatedDate,
						kboDeliveryDate = eRPServiceContractOwnerInformationDto.kboDeliveryDate,
						kboEmailAddress = eRPServiceContractOwnerInformationDto.kboEmailAddress,
						kboUniqueID = eRPServiceContractOwnerInformationDto.kboUniqueID,
						kboFaxNumber = eRPServiceContractOwnerInformationDto.kboFaxNumber,
						kboFirstName = eRPServiceContractOwnerInformationDto.kboFirstName,
						kboHomePhoneNumber = eRPServiceContractOwnerInformationDto.kboHomePhoneNumber,
						kboCurrentOwner = eRPServiceContractOwnerInformationDto.kboCurrentOwner,
						kboSameAsAbove = eRPServiceContractOwnerInformationDto.kboSameAsAbove,
						kboTermsAccepted = eRPServiceContractOwnerInformationDto.kboTermsAccepted,
						kboLastName = eRPServiceContractOwnerInformationDto.kboLastName,
						kboMiddleName = eRPServiceContractOwnerInformationDto.kboMiddleName,
						kboMobileNumber = eRPServiceContractOwnerInformationDto.kboMobileNumber,
						kboOrganizationID = eRPServiceContractOwnerInformationDto.kboOrganizationID,
						kboPhysicalAddressLine1 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine1,
						kboPhysicalAddressLine2 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine2,
						kboPhysicalAddressLine3 = eRPServiceContractOwnerInformationDto.kboPhysicalAddressLine3,
						kboPhysicalCity = eRPServiceContractOwnerInformationDto.kboPhysicalCity,
						kboPhysicalCountry = eRPServiceContractOwnerInformationDto.kboPhysicalCountry,
						kboPhysicalLocationCity = eRPServiceContractOwnerInformationDto.kboPhysicalLocationCity,
						kboPhysicalLocationState = eRPServiceContractOwnerInformationDto.kboPhysicalLocationState,
						kboPhysicalPostCode = eRPServiceContractOwnerInformationDto.kboPhysicalPostCode,
						kboPhysicalState = eRPServiceContractOwnerInformationDto.kboPhysicalState,
						kboPostCode = eRPServiceContractOwnerInformationDto.kboPostCode,
						kboRegisteredDate = eRPServiceContractOwnerInformationDto.kboRegisteredDate,
						kboRowVersion = eRPServiceContractOwnerInformationDto.kboRowVersion,
						kboServiceContractOwnerID = eRPServiceContractOwnerInformationDto.kboServiceContractOwnerID,
						kboServiceContractID = eRPServiceContractOwnerInformationDto.kboServiceContractID,
						kboStartDate = eRPServiceContractOwnerInformationDto.kboStartDate,
						kboState = eRPServiceContractOwnerInformationDto.kboState,
						kboWorkPhoneNumber = eRPServiceContractOwnerInformationDto.kboWorkPhoneNumber,
						CustomFields = eRPServiceContractOwnerInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ServiceContractOwner [{serviceContractOwner.kboUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractOwnerDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractOwner(Guid serviceContractOwnerId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
		using (iERPServiceContractOwnerRepository)
		{
			if (!(await base.ERPServiceContractOwnerRepository.DoesServiceContractOwnerExist(serviceContractOwnerId)))
			{
				base.ErrorsList.Add($"ServiceContractOwner [{serviceContractOwnerId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPServiceContractOwnerInformationDto eRPServiceContractOwnerInformationDto = await base.ERPServiceContractOwnerRepository.GetServiceContractOwner(serviceContractOwnerId);
				string text = await base.ERPServiceContractOwnerRepository.WhereUsed("ServiceContractOwners", new object[2] { eRPServiceContractOwnerInformationDto.kboServiceContractID, eRPServiceContractOwnerInformationDto.kboServiceContractOwnerID }, new object[2] { "kboServiceContractID", "kboServiceContractOwnerID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ServiceContractOwner cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractOwnerDto>> Process_DeleteServiceContractOwner(Guid serviceContractOwnerId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPServiceContractOwnerDto> result;
		try
		{
			IERPServiceContractOwnerRepository iERPServiceContractOwnerRepository = (base.ERPServiceContractOwnerRepository = new ERPServiceContractOwnerRepository(base.ApiClientContext));
			using (iERPServiceContractOwnerRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPServiceContractOwnerRepository.DeleteRowFromTable("ServiceContractOwners", "kbo", serviceContractOwnerId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ServiceContractOwner [{serviceContractOwnerId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractOwnerDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPServiceContractOwnerDto()
			};
		}
		return result;
	}
}
