using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPServiceContractModel : ERPBaseModel, IERPServiceContractModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContracts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
		using (iERPServiceContractRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPServiceContractRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPServiceContractRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPServiceContractRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPServiceContractRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetServiceContract(Guid serviceContractId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
		using (iERPServiceContractRepository)
		{
			if (!(await base.ERPServiceContractRepository.DoesServiceContractExist(serviceContractId)))
			{
				errorsList.Add($"ServiceContract [{serviceContractId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutServiceContract(ERPServiceContractDto serviceContract)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
		using (iERPServiceContractRepository)
		{
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsOrganizationID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { serviceContract.kbsOrganizationID })))
			{
				errorsList.Add("kbsOrganizationID [" + serviceContract.kbsOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsServiceContractTypeID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("ServiceContractTypes", new object[1] { "KBYSERVICECONTRACTTYPEID" }, new object[1] { serviceContract.kbsServiceContractTypeID })))
			{
				errorsList.Add("kbsServiceContractTypeID [" + serviceContract.kbsServiceContractTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsProjectID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { serviceContract.kbsProjectID })))
			{
				errorsList.Add("kbsProjectID [" + serviceContract.kbsProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsSerialNumberID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { serviceContract.kbsPartID, serviceContract.kbsPartRevisionID, serviceContract.kbsSerialNumberID })))
			{
				errorsList.Add("kbsSerialNumberID [" + serviceContract.kbsSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsPartID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { serviceContract.kbsPartID })))
			{
				errorsList.Add("kbsPartID [" + serviceContract.kbsPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsPartRevisionID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { serviceContract.kbsPartID, serviceContract.kbsPartRevisionID })))
			{
				errorsList.Add("kbsPartRevisionID [" + serviceContract.kbsPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsResellerOrganizationID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { serviceContract.kbsResellerOrganizationID })))
			{
				errorsList.Add("kbsResellerOrganizationID [" + serviceContract.kbsResellerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContract.kbsProjectAreaID) && !(await base.ERPServiceContractRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { serviceContract.kbsProjectID, serviceContract.kbsProjectAreaID })))
			{
				errorsList.Add("kbsProjectAreaID [" + serviceContract.kbsProjectAreaID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPServiceContractDto>>> Process_GetAllServiceContracts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPServiceContractDto> allServiceContractsDto = new List<ERPServiceContractDto>();
		ERPResponseMessageDto<IList<ERPServiceContractDto>> result;
		try
		{
			IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
			using (iERPServiceContractRepository)
			{
				foreach (ERPServiceContractInformationDto item2 in await base.ERPServiceContractRepository.GetAllServiceContracts(pageSize, pageNumber, filter, orderBy))
				{
					ERPServiceContractDto item = new ERPServiceContractDto
					{
						kbsServiceContractID = item2.kbsServiceContractID,
						kbsContractAmount = item2.kbsContractAmount,
						kbsContractLength = item2.kbsContractLength,
						kbsContractLengthType = item2.kbsContractLengthType,
						kbsCreatedBy = item2.kbsCreatedBy,
						kbsCreatedDate = item2.kbsCreatedDate,
						kbsDescription = item2.kbsDescription,
						kbsEndDate = item2.kbsEndDate,
						kbsUniqueID = item2.kbsUniqueID,
						kbsLongDescriptionRtf = item2.kbsLongDescriptionRtf,
						kbsLongDescriptionText = item2.kbsLongDescriptionText,
						kbsOrganizationID = item2.kbsOrganizationID,
						kbsPartID = item2.kbsPartID,
						kbsPartRevisionID = item2.kbsPartRevisionID,
						kbsPartShortDescription = item2.kbsPartShortDescription,
						kbsProjectAreaID = item2.kbsProjectAreaID,
						kbsProjectID = item2.kbsProjectID,
						kbsResellerOrganizationID = item2.kbsResellerOrganizationID,
						kbsRowVersion = item2.kbsRowVersion,
						kbsSerialNumberID = item2.kbsSerialNumberID,
						kbsServiceContractTypeID = item2.kbsServiceContractTypeID,
						kbsStartDate = item2.kbsStartDate,
						CustomFields = item2.CustomFields
					};
					allServiceContractsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ServiceContracts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPServiceContractDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allServiceContractsDto,
				RecordCount = allServiceContractsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_GetServiceContract(Guid serviceContractId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPServiceContractDto serviceContractDto = null;
		ERPResponseMessageDto<ERPServiceContractDto> result;
		try
		{
			IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
			using (iERPServiceContractRepository)
			{
				ERPServiceContractInformationDto eRPServiceContractInformationDto = await base.ERPServiceContractRepository.GetServiceContract(serviceContractId);
				serviceContractDto = new ERPServiceContractDto
				{
					kbsServiceContractID = eRPServiceContractInformationDto.kbsServiceContractID,
					kbsContractAmount = eRPServiceContractInformationDto.kbsContractAmount,
					kbsContractLength = eRPServiceContractInformationDto.kbsContractLength,
					kbsContractLengthType = eRPServiceContractInformationDto.kbsContractLengthType,
					kbsCreatedBy = eRPServiceContractInformationDto.kbsCreatedBy,
					kbsCreatedDate = eRPServiceContractInformationDto.kbsCreatedDate,
					kbsDescription = eRPServiceContractInformationDto.kbsDescription,
					kbsEndDate = eRPServiceContractInformationDto.kbsEndDate,
					kbsUniqueID = eRPServiceContractInformationDto.kbsUniqueID,
					kbsLongDescriptionRtf = eRPServiceContractInformationDto.kbsLongDescriptionRtf,
					kbsLongDescriptionText = eRPServiceContractInformationDto.kbsLongDescriptionText,
					kbsOrganizationID = eRPServiceContractInformationDto.kbsOrganizationID,
					kbsPartID = eRPServiceContractInformationDto.kbsPartID,
					kbsPartRevisionID = eRPServiceContractInformationDto.kbsPartRevisionID,
					kbsPartShortDescription = eRPServiceContractInformationDto.kbsPartShortDescription,
					kbsProjectAreaID = eRPServiceContractInformationDto.kbsProjectAreaID,
					kbsProjectID = eRPServiceContractInformationDto.kbsProjectID,
					kbsResellerOrganizationID = eRPServiceContractInformationDto.kbsResellerOrganizationID,
					kbsRowVersion = eRPServiceContractInformationDto.kbsRowVersion,
					kbsSerialNumberID = eRPServiceContractInformationDto.kbsSerialNumberID,
					kbsServiceContractTypeID = eRPServiceContractInformationDto.kbsServiceContractTypeID,
					kbsStartDate = eRPServiceContractInformationDto.kbsStartDate,
					CustomFields = eRPServiceContractInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ServiceContracts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serviceContractDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_PutServiceContract(ERPServiceContractDto serviceContract)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPServiceContractDto createdObject = null;
		ERPResponseMessageDto<ERPServiceContractDto> result;
		try
		{
			IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
			using (iERPServiceContractRepository)
			{
				APIValidationInfoDto postResult = await base.ERPServiceContractRepository.SaveServiceContract(serviceContract);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPServiceContractInformationDto eRPServiceContractInformationDto = await base.ERPServiceContractRepository.GetServiceContract(serviceContract.kbsUniqueID);
					createdObject = new ERPServiceContractDto
					{
						kbsServiceContractID = eRPServiceContractInformationDto.kbsServiceContractID,
						kbsContractAmount = eRPServiceContractInformationDto.kbsContractAmount,
						kbsContractLength = eRPServiceContractInformationDto.kbsContractLength,
						kbsContractLengthType = eRPServiceContractInformationDto.kbsContractLengthType,
						kbsCreatedBy = eRPServiceContractInformationDto.kbsCreatedBy,
						kbsCreatedDate = eRPServiceContractInformationDto.kbsCreatedDate,
						kbsDescription = eRPServiceContractInformationDto.kbsDescription,
						kbsEndDate = eRPServiceContractInformationDto.kbsEndDate,
						kbsUniqueID = eRPServiceContractInformationDto.kbsUniqueID,
						kbsLongDescriptionRtf = eRPServiceContractInformationDto.kbsLongDescriptionRtf,
						kbsLongDescriptionText = eRPServiceContractInformationDto.kbsLongDescriptionText,
						kbsOrganizationID = eRPServiceContractInformationDto.kbsOrganizationID,
						kbsPartID = eRPServiceContractInformationDto.kbsPartID,
						kbsPartRevisionID = eRPServiceContractInformationDto.kbsPartRevisionID,
						kbsPartShortDescription = eRPServiceContractInformationDto.kbsPartShortDescription,
						kbsProjectAreaID = eRPServiceContractInformationDto.kbsProjectAreaID,
						kbsProjectID = eRPServiceContractInformationDto.kbsProjectID,
						kbsResellerOrganizationID = eRPServiceContractInformationDto.kbsResellerOrganizationID,
						kbsRowVersion = eRPServiceContractInformationDto.kbsRowVersion,
						kbsSerialNumberID = eRPServiceContractInformationDto.kbsSerialNumberID,
						kbsServiceContractTypeID = eRPServiceContractInformationDto.kbsServiceContractTypeID,
						kbsStartDate = eRPServiceContractInformationDto.kbsStartDate,
						CustomFields = eRPServiceContractInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ServiceContract [{serviceContract.kbsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContract(Guid serviceContractId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
		using (iERPServiceContractRepository)
		{
			if (!(await base.ERPServiceContractRepository.DoesServiceContractExist(serviceContractId)))
			{
				base.ErrorsList.Add($"ServiceContract [{serviceContractId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPServiceContractInformationDto eRPServiceContractInformationDto = await base.ERPServiceContractRepository.GetServiceContract(serviceContractId);
				string text = await base.ERPServiceContractRepository.WhereUsed("ServiceContracts", new object[1] { eRPServiceContractInformationDto.kbsServiceContractID }, new object[1] { "kbsServiceContractID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ServiceContract cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractDto>> Process_DeleteServiceContract(Guid serviceContractId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPServiceContractDto> result;
		try
		{
			IERPServiceContractRepository iERPServiceContractRepository = (base.ERPServiceContractRepository = new ERPServiceContractRepository(base.ApiClientContext));
			using (iERPServiceContractRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPServiceContractRepository.DeleteRowFromTable("ServiceContracts", "kbs", serviceContractId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ServiceContract [{serviceContractId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPServiceContractDto()
			};
		}
		return result;
	}
}
