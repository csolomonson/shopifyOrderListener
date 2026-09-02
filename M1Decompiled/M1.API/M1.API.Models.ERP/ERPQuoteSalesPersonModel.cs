using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteSalesPersonModel : ERPBaseModel, IERPQuoteSalesPersonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
		using (iERPQuoteSalesPersonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteSalesPersonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteSalesPersonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteSalesPersonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteSalesPersonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteSalesPerson(Guid quoteSalesPersonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
		using (iERPQuoteSalesPersonRepository)
		{
			if (!(await base.ERPQuoteSalesPersonRepository.DoesQuoteSalesPersonExist(quoteSalesPersonId)))
			{
				errorsList.Add($"QuoteSalesPerson [{quoteSalesPersonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteSalesPerson(ERPQuoteSalesPersonDto quoteSalesPerson)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
		using (iERPQuoteSalesPersonRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteSalesPerson.qmjQuoteID) && !(await base.ERPQuoteSalesPersonRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteSalesPerson.qmjQuoteID })))
			{
				errorsList.Add("qmjQuoteID [" + quoteSalesPerson.qmjQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteSalesPerson.qmjSalesEmployeeID) && !(await base.ERPQuoteSalesPersonRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { quoteSalesPerson.qmjSalesEmployeeID })))
			{
				errorsList.Add("qmjSalesEmployeeID [" + quoteSalesPerson.qmjSalesEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteSalesPersonDto>>> Process_GetAllQuoteSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteSalesPersonDto> allQuoteSalesPeopleDto = new List<ERPQuoteSalesPersonDto>();
		ERPResponseMessageDto<IList<ERPQuoteSalesPersonDto>> result;
		try
		{
			IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
			using (iERPQuoteSalesPersonRepository)
			{
				foreach (ERPQuoteSalesPersonInformationDto item2 in await base.ERPQuoteSalesPersonRepository.GetAllQuoteSalesPeople(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteSalesPersonDto item = new ERPQuoteSalesPersonDto
					{
						qmjCreatedBy = item2.qmjCreatedBy,
						qmjCreatedDate = item2.qmjCreatedDate,
						qmjUniqueID = item2.qmjUniqueID,
						qmjClosed = item2.qmjClosed,
						qmjCreatedFromMobile = item2.qmjCreatedFromMobile,
						qmjPercent = item2.qmjPercent,
						qmjQuoteID = item2.qmjQuoteID,
						qmjRowVersion = item2.qmjRowVersion,
						qmjSalesEmployeeID = item2.qmjSalesEmployeeID,
						qmjSequenceID = item2.qmjSequenceID,
						CustomFields = item2.CustomFields
					};
					allQuoteSalesPeopleDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteSalesPeople]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteSalesPersonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteSalesPeopleDto,
				RecordCount = allQuoteSalesPeopleDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_GetQuoteSalesPerson(Guid quoteSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteSalesPersonDto quoteSalesPersonDto = null;
		ERPResponseMessageDto<ERPQuoteSalesPersonDto> result;
		try
		{
			IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
			using (iERPQuoteSalesPersonRepository)
			{
				ERPQuoteSalesPersonInformationDto eRPQuoteSalesPersonInformationDto = await base.ERPQuoteSalesPersonRepository.GetQuoteSalesPerson(quoteSalesPersonId);
				quoteSalesPersonDto = new ERPQuoteSalesPersonDto
				{
					qmjCreatedBy = eRPQuoteSalesPersonInformationDto.qmjCreatedBy,
					qmjCreatedDate = eRPQuoteSalesPersonInformationDto.qmjCreatedDate,
					qmjUniqueID = eRPQuoteSalesPersonInformationDto.qmjUniqueID,
					qmjClosed = eRPQuoteSalesPersonInformationDto.qmjClosed,
					qmjCreatedFromMobile = eRPQuoteSalesPersonInformationDto.qmjCreatedFromMobile,
					qmjPercent = eRPQuoteSalesPersonInformationDto.qmjPercent,
					qmjQuoteID = eRPQuoteSalesPersonInformationDto.qmjQuoteID,
					qmjRowVersion = eRPQuoteSalesPersonInformationDto.qmjRowVersion,
					qmjSalesEmployeeID = eRPQuoteSalesPersonInformationDto.qmjSalesEmployeeID,
					qmjSequenceID = eRPQuoteSalesPersonInformationDto.qmjSequenceID,
					CustomFields = eRPQuoteSalesPersonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteSalesPeople []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteSalesPersonDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_PutQuoteSalesPerson(ERPQuoteSalesPersonDto quoteSalesPerson)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteSalesPersonDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteSalesPersonDto> result;
		try
		{
			IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
			using (iERPQuoteSalesPersonRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteSalesPersonRepository.SaveQuoteSalesPerson(quoteSalesPerson);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteSalesPersonInformationDto eRPQuoteSalesPersonInformationDto = await base.ERPQuoteSalesPersonRepository.GetQuoteSalesPerson(quoteSalesPerson.qmjUniqueID);
					createdObject = new ERPQuoteSalesPersonDto
					{
						qmjCreatedBy = eRPQuoteSalesPersonInformationDto.qmjCreatedBy,
						qmjCreatedDate = eRPQuoteSalesPersonInformationDto.qmjCreatedDate,
						qmjUniqueID = eRPQuoteSalesPersonInformationDto.qmjUniqueID,
						qmjClosed = eRPQuoteSalesPersonInformationDto.qmjClosed,
						qmjCreatedFromMobile = eRPQuoteSalesPersonInformationDto.qmjCreatedFromMobile,
						qmjPercent = eRPQuoteSalesPersonInformationDto.qmjPercent,
						qmjQuoteID = eRPQuoteSalesPersonInformationDto.qmjQuoteID,
						qmjRowVersion = eRPQuoteSalesPersonInformationDto.qmjRowVersion,
						qmjSalesEmployeeID = eRPQuoteSalesPersonInformationDto.qmjSalesEmployeeID,
						qmjSequenceID = eRPQuoteSalesPersonInformationDto.qmjSequenceID,
						CustomFields = eRPQuoteSalesPersonInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteSalesPerson [{quoteSalesPerson.qmjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteSalesPerson(Guid quoteSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
		using (iERPQuoteSalesPersonRepository)
		{
			if (!(await base.ERPQuoteSalesPersonRepository.DoesQuoteSalesPersonExist(quoteSalesPersonId)))
			{
				base.ErrorsList.Add($"QuoteSalesPerson [{quoteSalesPersonId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteSalesPersonInformationDto eRPQuoteSalesPersonInformationDto = await base.ERPQuoteSalesPersonRepository.GetQuoteSalesPerson(quoteSalesPersonId);
				string text = await base.ERPQuoteSalesPersonRepository.WhereUsed("QuoteSalesPeople", new object[2] { eRPQuoteSalesPersonInformationDto.qmjQuoteID, eRPQuoteSalesPersonInformationDto.qmjSequenceID }, new object[2] { "qmjQuoteID", "qmjSequenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteSalesPerson cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteSalesPersonDto>> Process_DeleteQuoteSalesPerson(Guid quoteSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteSalesPersonDto> result;
		try
		{
			IERPQuoteSalesPersonRepository iERPQuoteSalesPersonRepository = (base.ERPQuoteSalesPersonRepository = new ERPQuoteSalesPersonRepository(base.ApiClientContext));
			using (iERPQuoteSalesPersonRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteSalesPersonRepository.DeleteRowFromTable("QuoteSalesPeople", "qmj", quoteSalesPersonId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteSalesPerson [{quoteSalesPersonId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteSalesPersonDto()
			};
		}
		return result;
	}
}
