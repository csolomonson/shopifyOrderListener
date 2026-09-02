using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPServiceContractMemoModel : ERPBaseModel, IERPServiceContractMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
		using (iERPServiceContractMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPServiceContractMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPServiceContractMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPServiceContractMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPServiceContractMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetServiceContractMemo(Guid serviceContractMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
		using (iERPServiceContractMemoRepository)
		{
			if (!(await base.ERPServiceContractMemoRepository.DoesServiceContractMemoExist(serviceContractMemoId)))
			{
				errorsList.Add($"ServiceContractMemo [{serviceContractMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutServiceContractMemo(ERPServiceContractMemoDto serviceContractMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
		using (iERPServiceContractMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(serviceContractMemo.kbmServiceContractID) && !(await base.ERPServiceContractMemoRepository.DoesRecordExistInTableUsingKeys("ServiceContracts", new object[1] { "KBSSERVICECONTRACTID" }, new object[1] { serviceContractMemo.kbmServiceContractID })))
			{
				errorsList.Add("kbmServiceContractID [" + serviceContractMemo.kbmServiceContractID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPServiceContractMemoDto>>> Process_GetAllServiceContractMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPServiceContractMemoDto> allServiceContractMemosDto = new List<ERPServiceContractMemoDto>();
		ERPResponseMessageDto<IList<ERPServiceContractMemoDto>> result;
		try
		{
			IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
			using (iERPServiceContractMemoRepository)
			{
				foreach (ERPServiceContractMemoInformationDto item2 in await base.ERPServiceContractMemoRepository.GetAllServiceContractMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPServiceContractMemoDto item = new ERPServiceContractMemoDto
					{
						kbmCreatedBy = item2.kbmCreatedBy,
						kbmCreatedDate = item2.kbmCreatedDate,
						kbmUniqueID = item2.kbmUniqueID,
						kbmLongDescriptionRtf = item2.kbmLongDescriptionRtf,
						kbmLongDescriptionText = item2.kbmLongDescriptionText,
						kbmMemoDate = item2.kbmMemoDate,
						kbmRowVersion = item2.kbmRowVersion,
						kbmServiceContractMemoID = item2.kbmServiceContractMemoID,
						kbmServiceContractID = item2.kbmServiceContractID,
						kbmShortDescription = item2.kbmShortDescription,
						CustomFields = item2.CustomFields
					};
					allServiceContractMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ServiceContractMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPServiceContractMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allServiceContractMemosDto,
				RecordCount = allServiceContractMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_GetServiceContractMemo(Guid serviceContractMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPServiceContractMemoDto serviceContractMemoDto = null;
		ERPResponseMessageDto<ERPServiceContractMemoDto> result;
		try
		{
			IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
			using (iERPServiceContractMemoRepository)
			{
				ERPServiceContractMemoInformationDto eRPServiceContractMemoInformationDto = await base.ERPServiceContractMemoRepository.GetServiceContractMemo(serviceContractMemoId);
				serviceContractMemoDto = new ERPServiceContractMemoDto
				{
					kbmCreatedBy = eRPServiceContractMemoInformationDto.kbmCreatedBy,
					kbmCreatedDate = eRPServiceContractMemoInformationDto.kbmCreatedDate,
					kbmUniqueID = eRPServiceContractMemoInformationDto.kbmUniqueID,
					kbmLongDescriptionRtf = eRPServiceContractMemoInformationDto.kbmLongDescriptionRtf,
					kbmLongDescriptionText = eRPServiceContractMemoInformationDto.kbmLongDescriptionText,
					kbmMemoDate = eRPServiceContractMemoInformationDto.kbmMemoDate,
					kbmRowVersion = eRPServiceContractMemoInformationDto.kbmRowVersion,
					kbmServiceContractMemoID = eRPServiceContractMemoInformationDto.kbmServiceContractMemoID,
					kbmServiceContractID = eRPServiceContractMemoInformationDto.kbmServiceContractID,
					kbmShortDescription = eRPServiceContractMemoInformationDto.kbmShortDescription,
					CustomFields = eRPServiceContractMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ServiceContractMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serviceContractMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_PutServiceContractMemo(ERPServiceContractMemoDto serviceContractMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPServiceContractMemoDto createdObject = null;
		ERPResponseMessageDto<ERPServiceContractMemoDto> result;
		try
		{
			IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
			using (iERPServiceContractMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPServiceContractMemoRepository.SaveServiceContractMemo(serviceContractMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPServiceContractMemoInformationDto eRPServiceContractMemoInformationDto = await base.ERPServiceContractMemoRepository.GetServiceContractMemo(serviceContractMemo.kbmUniqueID);
					createdObject = new ERPServiceContractMemoDto
					{
						kbmCreatedBy = eRPServiceContractMemoInformationDto.kbmCreatedBy,
						kbmCreatedDate = eRPServiceContractMemoInformationDto.kbmCreatedDate,
						kbmUniqueID = eRPServiceContractMemoInformationDto.kbmUniqueID,
						kbmLongDescriptionRtf = eRPServiceContractMemoInformationDto.kbmLongDescriptionRtf,
						kbmLongDescriptionText = eRPServiceContractMemoInformationDto.kbmLongDescriptionText,
						kbmMemoDate = eRPServiceContractMemoInformationDto.kbmMemoDate,
						kbmRowVersion = eRPServiceContractMemoInformationDto.kbmRowVersion,
						kbmServiceContractMemoID = eRPServiceContractMemoInformationDto.kbmServiceContractMemoID,
						kbmServiceContractID = eRPServiceContractMemoInformationDto.kbmServiceContractID,
						kbmShortDescription = eRPServiceContractMemoInformationDto.kbmShortDescription,
						CustomFields = eRPServiceContractMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ServiceContractMemo [{serviceContractMemo.kbmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractMemo(Guid serviceContractMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
		using (iERPServiceContractMemoRepository)
		{
			if (!(await base.ERPServiceContractMemoRepository.DoesServiceContractMemoExist(serviceContractMemoId)))
			{
				base.ErrorsList.Add($"ServiceContractMemo [{serviceContractMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPServiceContractMemoInformationDto eRPServiceContractMemoInformationDto = await base.ERPServiceContractMemoRepository.GetServiceContractMemo(serviceContractMemoId);
				string text = await base.ERPServiceContractMemoRepository.WhereUsed("ServiceContractMemos", new object[2] { eRPServiceContractMemoInformationDto.kbmServiceContractID, eRPServiceContractMemoInformationDto.kbmServiceContractMemoID }, new object[2] { "kbmServiceContractID", "kbmServiceContractMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ServiceContractMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractMemoDto>> Process_DeleteServiceContractMemo(Guid serviceContractMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPServiceContractMemoDto> result;
		try
		{
			IERPServiceContractMemoRepository iERPServiceContractMemoRepository = (base.ERPServiceContractMemoRepository = new ERPServiceContractMemoRepository(base.ApiClientContext));
			using (iERPServiceContractMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPServiceContractMemoRepository.DeleteRowFromTable("ServiceContractMemos", "kbm", serviceContractMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ServiceContractMemo [{serviceContractMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPServiceContractMemoDto()
			};
		}
		return result;
	}
}
