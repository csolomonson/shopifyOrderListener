using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRFQModel : ERPBaseModel, IERPRFQModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRFQs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
		using (iERPRFQRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRFQRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRFQRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRFQRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRFQRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRFQ(Guid rFQId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
		using (iERPRFQRepository)
		{
			if (!(await base.ERPRFQRepository.DoesRFQExist(rFQId)))
			{
				errorsList.Add($"RFQ [{rFQId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRFQ(ERPRFQDto rFQ)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
		using (iERPRFQRepository)
		{
			if (!string.IsNullOrWhiteSpace(rFQ.rqpPlantDepartmentID) && !(await base.ERPRFQRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { rFQ.rqpPlantID, rFQ.rqpPlantDepartmentID })))
			{
				errorsList.Add("rqpPlantDepartmentID [" + rFQ.rqpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQ.rqpPlantID) && !(await base.ERPRFQRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { rFQ.rqpPlantID })))
			{
				errorsList.Add("rqpPlantID [" + rFQ.rqpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQ.rqpBuyerEmployeeID) && !(await base.ERPRFQRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { rFQ.rqpBuyerEmployeeID })))
			{
				errorsList.Add("rqpBuyerEmployeeID [" + rFQ.rqpBuyerEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQ.rqpStandardMessageID) && !(await base.ERPRFQRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { rFQ.rqpStandardMessageID })))
			{
				errorsList.Add("rqpStandardMessageID [" + rFQ.rqpStandardMessageID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRFQDto>>> Process_GetAllRFQs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRFQDto> allRFQsDto = new List<ERPRFQDto>();
		ERPResponseMessageDto<IList<ERPRFQDto>> result;
		try
		{
			IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
			using (iERPRFQRepository)
			{
				foreach (ERPRFQInformationDto item2 in await base.ERPRFQRepository.GetAllRFQs(pageSize, pageNumber, filter, orderBy))
				{
					ERPRFQDto item = new ERPRFQDto
					{
						rqpBuyerEmployeeID = item2.rqpBuyerEmployeeID,
						rqpClosedDate = item2.rqpClosedDate,
						rqpRfqID = item2.rqpRfqID,
						rqpCreatedBy = item2.rqpCreatedBy,
						rqpCreatedDate = item2.rqpCreatedDate,
						rqpDueDate = item2.rqpDueDate,
						rqpUniqueID = item2.rqpUniqueID,
						rqpClosed = item2.rqpClosed,
						rqpReadyToPrint = item2.rqpReadyToPrint,
						rqpPlantDepartmentID = item2.rqpPlantDepartmentID,
						rqpPlantID = item2.rqpPlantID,
						rqpRfqDate = item2.rqpRfqDate,
						rqpRowVersion = item2.rqpRowVersion,
						rqpStandardMessageID = item2.rqpStandardMessageID,
						CustomFields = item2.CustomFields
					};
					allRFQsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RFQs]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRFQDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRFQsDto,
				RecordCount = allRFQsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQDto>> Process_GetRFQ(Guid rFQId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRFQDto rFQDto = null;
		ERPResponseMessageDto<ERPRFQDto> result;
		try
		{
			IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
			using (iERPRFQRepository)
			{
				ERPRFQInformationDto eRPRFQInformationDto = await base.ERPRFQRepository.GetRFQ(rFQId);
				rFQDto = new ERPRFQDto
				{
					rqpBuyerEmployeeID = eRPRFQInformationDto.rqpBuyerEmployeeID,
					rqpClosedDate = eRPRFQInformationDto.rqpClosedDate,
					rqpRfqID = eRPRFQInformationDto.rqpRfqID,
					rqpCreatedBy = eRPRFQInformationDto.rqpCreatedBy,
					rqpCreatedDate = eRPRFQInformationDto.rqpCreatedDate,
					rqpDueDate = eRPRFQInformationDto.rqpDueDate,
					rqpUniqueID = eRPRFQInformationDto.rqpUniqueID,
					rqpClosed = eRPRFQInformationDto.rqpClosed,
					rqpReadyToPrint = eRPRFQInformationDto.rqpReadyToPrint,
					rqpPlantDepartmentID = eRPRFQInformationDto.rqpPlantDepartmentID,
					rqpPlantID = eRPRFQInformationDto.rqpPlantID,
					rqpRfqDate = eRPRFQInformationDto.rqpRfqDate,
					rqpRowVersion = eRPRFQInformationDto.rqpRowVersion,
					rqpStandardMessageID = eRPRFQInformationDto.rqpStandardMessageID,
					CustomFields = eRPRFQInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RFQs []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rFQDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQDto>> Process_PutRFQ(ERPRFQDto rFQ)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRFQDto createdObject = null;
		ERPResponseMessageDto<ERPRFQDto> result;
		try
		{
			IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
			using (iERPRFQRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRFQRepository.SaveRFQ(rFQ);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRFQInformationDto eRPRFQInformationDto = await base.ERPRFQRepository.GetRFQ(rFQ.rqpUniqueID);
					createdObject = new ERPRFQDto
					{
						rqpBuyerEmployeeID = eRPRFQInformationDto.rqpBuyerEmployeeID,
						rqpClosedDate = eRPRFQInformationDto.rqpClosedDate,
						rqpRfqID = eRPRFQInformationDto.rqpRfqID,
						rqpCreatedBy = eRPRFQInformationDto.rqpCreatedBy,
						rqpCreatedDate = eRPRFQInformationDto.rqpCreatedDate,
						rqpDueDate = eRPRFQInformationDto.rqpDueDate,
						rqpUniqueID = eRPRFQInformationDto.rqpUniqueID,
						rqpClosed = eRPRFQInformationDto.rqpClosed,
						rqpReadyToPrint = eRPRFQInformationDto.rqpReadyToPrint,
						rqpPlantDepartmentID = eRPRFQInformationDto.rqpPlantDepartmentID,
						rqpPlantID = eRPRFQInformationDto.rqpPlantID,
						rqpRfqDate = eRPRFQInformationDto.rqpRfqDate,
						rqpRowVersion = eRPRFQInformationDto.rqpRowVersion,
						rqpStandardMessageID = eRPRFQInformationDto.rqpStandardMessageID,
						CustomFields = eRPRFQInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RFQ [{rFQ.rqpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRFQ(Guid rFQId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
		using (iERPRFQRepository)
		{
			if (!(await base.ERPRFQRepository.DoesRFQExist(rFQId)))
			{
				base.ErrorsList.Add($"RFQ [{rFQId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRFQInformationDto eRPRFQInformationDto = await base.ERPRFQRepository.GetRFQ(rFQId);
				string text = await base.ERPRFQRepository.WhereUsed("RFQs", new object[1] { eRPRFQInformationDto.rqpRfqID }, new object[1] { "rqpRfqID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RFQ cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRFQDto>> Process_DeleteRFQ(Guid rFQId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRFQDto> result;
		try
		{
			IERPRFQRepository iERPRFQRepository = (base.ERPRFQRepository = new ERPRFQRepository(base.ApiClientContext));
			using (iERPRFQRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRFQRepository.DeleteRowFromTable("RFQs", "rqp", rFQId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RFQ [{rFQId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRFQDto()
			};
		}
		return result;
	}
}
