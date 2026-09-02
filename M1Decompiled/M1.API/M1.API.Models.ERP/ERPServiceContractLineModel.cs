using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPServiceContractLineModel : ERPBaseModel, IERPServiceContractLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllServiceContractLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
		using (iERPServiceContractLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPServiceContractLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPServiceContractLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPServiceContractLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPServiceContractLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetServiceContractLine(Guid serviceContractLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
		using (iERPServiceContractLineRepository)
		{
			if (!(await base.ERPServiceContractLineRepository.DoesServiceContractLineExist(serviceContractLineId)))
			{
				errorsList.Add($"ServiceContractLine [{serviceContractLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutServiceContractLine(ERPServiceContractLineDto serviceContractLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
		using (iERPServiceContractLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(serviceContractLine.kbnServiceContractID) && !(await base.ERPServiceContractLineRepository.DoesRecordExistInTableUsingKeys("ServiceContracts", new object[1] { "KBSSERVICECONTRACTID" }, new object[1] { serviceContractLine.kbnServiceContractID })))
			{
				errorsList.Add("kbnServiceContractID [" + serviceContractLine.kbnServiceContractID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContractLine.kbnSerialNumberID) && !(await base.ERPServiceContractLineRepository.DoesRecordExistInTableUsingKeys("SerialNumbers", new object[3] { "IMSPARTID", "IMSPARTREVISIONID", "IMSSERIALNUMBERID" }, new object[3] { serviceContractLine.kbnPartID, serviceContractLine.kbnPartRevisionID, serviceContractLine.kbnSerialNumberID })))
			{
				errorsList.Add("kbnSerialNumberID [" + serviceContractLine.kbnSerialNumberID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContractLine.kbnPartID) && !(await base.ERPServiceContractLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { serviceContractLine.kbnPartID })))
			{
				errorsList.Add("kbnPartID [" + serviceContractLine.kbnPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(serviceContractLine.kbnPartRevisionID) && !(await base.ERPServiceContractLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { serviceContractLine.kbnPartID, serviceContractLine.kbnPartRevisionID })))
			{
				errorsList.Add("kbnPartRevisionID [" + serviceContractLine.kbnPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPServiceContractLineDto>>> Process_GetAllServiceContractLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPServiceContractLineDto> allServiceContractLinesDto = new List<ERPServiceContractLineDto>();
		ERPResponseMessageDto<IList<ERPServiceContractLineDto>> result;
		try
		{
			IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
			using (iERPServiceContractLineRepository)
			{
				foreach (ERPServiceContractLineInformationDto item2 in await base.ERPServiceContractLineRepository.GetAllServiceContractLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPServiceContractLineDto item = new ERPServiceContractLineDto
					{
						kbnContractLength = item2.kbnContractLength,
						kbnContractLengthType = item2.kbnContractLengthType,
						kbnCreatedBy = item2.kbnCreatedBy,
						kbnCreatedDate = item2.kbnCreatedDate,
						kbnEndDate = item2.kbnEndDate,
						kbnUniqueID = item2.kbnUniqueID,
						kbnPartID = item2.kbnPartID,
						kbnPartRevisionID = item2.kbnPartRevisionID,
						kbnPartShortDescription = item2.kbnPartShortDescription,
						kbnRowVersion = item2.kbnRowVersion,
						kbnServiceContractLineID = item2.kbnServiceContractLineID,
						kbnSerialNumberID = item2.kbnSerialNumberID,
						kbnServiceContractID = item2.kbnServiceContractID,
						kbnStartDate = item2.kbnStartDate,
						CustomFields = item2.CustomFields
					};
					allServiceContractLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ServiceContractLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPServiceContractLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allServiceContractLinesDto,
				RecordCount = allServiceContractLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_GetServiceContractLine(Guid serviceContractLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPServiceContractLineDto serviceContractLineDto = null;
		ERPResponseMessageDto<ERPServiceContractLineDto> result;
		try
		{
			IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
			using (iERPServiceContractLineRepository)
			{
				ERPServiceContractLineInformationDto eRPServiceContractLineInformationDto = await base.ERPServiceContractLineRepository.GetServiceContractLine(serviceContractLineId);
				serviceContractLineDto = new ERPServiceContractLineDto
				{
					kbnContractLength = eRPServiceContractLineInformationDto.kbnContractLength,
					kbnContractLengthType = eRPServiceContractLineInformationDto.kbnContractLengthType,
					kbnCreatedBy = eRPServiceContractLineInformationDto.kbnCreatedBy,
					kbnCreatedDate = eRPServiceContractLineInformationDto.kbnCreatedDate,
					kbnEndDate = eRPServiceContractLineInformationDto.kbnEndDate,
					kbnUniqueID = eRPServiceContractLineInformationDto.kbnUniqueID,
					kbnPartID = eRPServiceContractLineInformationDto.kbnPartID,
					kbnPartRevisionID = eRPServiceContractLineInformationDto.kbnPartRevisionID,
					kbnPartShortDescription = eRPServiceContractLineInformationDto.kbnPartShortDescription,
					kbnRowVersion = eRPServiceContractLineInformationDto.kbnRowVersion,
					kbnServiceContractLineID = eRPServiceContractLineInformationDto.kbnServiceContractLineID,
					kbnSerialNumberID = eRPServiceContractLineInformationDto.kbnSerialNumberID,
					kbnServiceContractID = eRPServiceContractLineInformationDto.kbnServiceContractID,
					kbnStartDate = eRPServiceContractLineInformationDto.kbnStartDate,
					CustomFields = eRPServiceContractLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ServiceContractLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = serviceContractLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_PutServiceContractLine(ERPServiceContractLineDto serviceContractLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPServiceContractLineDto createdObject = null;
		ERPResponseMessageDto<ERPServiceContractLineDto> result;
		try
		{
			IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
			using (iERPServiceContractLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPServiceContractLineRepository.SaveServiceContractLine(serviceContractLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPServiceContractLineInformationDto eRPServiceContractLineInformationDto = await base.ERPServiceContractLineRepository.GetServiceContractLine(serviceContractLine.kbnUniqueID);
					createdObject = new ERPServiceContractLineDto
					{
						kbnContractLength = eRPServiceContractLineInformationDto.kbnContractLength,
						kbnContractLengthType = eRPServiceContractLineInformationDto.kbnContractLengthType,
						kbnCreatedBy = eRPServiceContractLineInformationDto.kbnCreatedBy,
						kbnCreatedDate = eRPServiceContractLineInformationDto.kbnCreatedDate,
						kbnEndDate = eRPServiceContractLineInformationDto.kbnEndDate,
						kbnUniqueID = eRPServiceContractLineInformationDto.kbnUniqueID,
						kbnPartID = eRPServiceContractLineInformationDto.kbnPartID,
						kbnPartRevisionID = eRPServiceContractLineInformationDto.kbnPartRevisionID,
						kbnPartShortDescription = eRPServiceContractLineInformationDto.kbnPartShortDescription,
						kbnRowVersion = eRPServiceContractLineInformationDto.kbnRowVersion,
						kbnServiceContractLineID = eRPServiceContractLineInformationDto.kbnServiceContractLineID,
						kbnSerialNumberID = eRPServiceContractLineInformationDto.kbnSerialNumberID,
						kbnServiceContractID = eRPServiceContractLineInformationDto.kbnServiceContractID,
						kbnStartDate = eRPServiceContractLineInformationDto.kbnStartDate,
						CustomFields = eRPServiceContractLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ServiceContractLine [{serviceContractLine.kbnUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteServiceContractLine(Guid serviceContractLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
		using (iERPServiceContractLineRepository)
		{
			if (!(await base.ERPServiceContractLineRepository.DoesServiceContractLineExist(serviceContractLineId)))
			{
				base.ErrorsList.Add($"ServiceContractLine [{serviceContractLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPServiceContractLineInformationDto eRPServiceContractLineInformationDto = await base.ERPServiceContractLineRepository.GetServiceContractLine(serviceContractLineId);
				string text = await base.ERPServiceContractLineRepository.WhereUsed("ServiceContractLines", new object[2] { eRPServiceContractLineInformationDto.kbnServiceContractID, eRPServiceContractLineInformationDto.kbnServiceContractLineID }, new object[2] { "kbnServiceContractID", "kbnServiceContractLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ServiceContractLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPServiceContractLineDto>> Process_DeleteServiceContractLine(Guid serviceContractLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPServiceContractLineDto> result;
		try
		{
			IERPServiceContractLineRepository iERPServiceContractLineRepository = (base.ERPServiceContractLineRepository = new ERPServiceContractLineRepository(base.ApiClientContext));
			using (iERPServiceContractLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPServiceContractLineRepository.DeleteRowFromTable("ServiceContractLines", "kbn", serviceContractLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ServiceContractLine [{serviceContractLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPServiceContractLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPServiceContractLineDto()
			};
		}
		return result;
	}
}
