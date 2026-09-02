using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPLeadSalesPersonModel : ERPBaseModel, IERPLeadSalesPersonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllLeadSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
		using (iERPLeadSalesPersonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPLeadSalesPersonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPLeadSalesPersonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPLeadSalesPersonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPLeadSalesPersonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetLeadSalesPerson(Guid leadSalesPersonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
		using (iERPLeadSalesPersonRepository)
		{
			if (!(await base.ERPLeadSalesPersonRepository.DoesLeadSalesPersonExist(leadSalesPersonId)))
			{
				errorsList.Add($"LeadSalesPerson [{leadSalesPersonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutLeadSalesPerson(ERPLeadSalesPersonDto leadSalesPerson)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
		using (iERPLeadSalesPersonRepository)
		{
			if (!string.IsNullOrWhiteSpace(leadSalesPerson.lojLeadID) && !(await base.ERPLeadSalesPersonRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { leadSalesPerson.lojLeadID })))
			{
				errorsList.Add("lojLeadID [" + leadSalesPerson.lojLeadID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(leadSalesPerson.lojSalesEmployeeID) && !(await base.ERPLeadSalesPersonRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { leadSalesPerson.lojSalesEmployeeID })))
			{
				errorsList.Add("lojSalesEmployeeID [" + leadSalesPerson.lojSalesEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPLeadSalesPersonDto>>> Process_GetAllLeadSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPLeadSalesPersonDto> allLeadSalesPeopleDto = new List<ERPLeadSalesPersonDto>();
		ERPResponseMessageDto<IList<ERPLeadSalesPersonDto>> result;
		try
		{
			IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
			using (iERPLeadSalesPersonRepository)
			{
				foreach (ERPLeadSalesPersonInformationDto item2 in await base.ERPLeadSalesPersonRepository.GetAllLeadSalesPeople(pageSize, pageNumber, filter, orderBy))
				{
					ERPLeadSalesPersonDto item = new ERPLeadSalesPersonDto
					{
						lojCreatedBy = item2.lojCreatedBy,
						lojCreatedDate = item2.lojCreatedDate,
						lojUniqueID = item2.lojUniqueID,
						lojLeadID = item2.lojLeadID,
						lojPercent = item2.lojPercent,
						lojRowVersion = item2.lojRowVersion,
						lojSalesEmployeeID = item2.lojSalesEmployeeID,
						lojSequenceID = item2.lojSequenceID,
						CustomFields = item2.CustomFields
					};
					allLeadSalesPeopleDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all LeadSalesPeople]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPLeadSalesPersonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allLeadSalesPeopleDto,
				RecordCount = allLeadSalesPeopleDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadSalesPersonDto>> Process_GetLeadSalesPerson(Guid leadSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPLeadSalesPersonDto leadSalesPersonDto = null;
		ERPResponseMessageDto<ERPLeadSalesPersonDto> result;
		try
		{
			IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
			using (iERPLeadSalesPersonRepository)
			{
				ERPLeadSalesPersonInformationDto eRPLeadSalesPersonInformationDto = await base.ERPLeadSalesPersonRepository.GetLeadSalesPerson(leadSalesPersonId);
				leadSalesPersonDto = new ERPLeadSalesPersonDto
				{
					lojCreatedBy = eRPLeadSalesPersonInformationDto.lojCreatedBy,
					lojCreatedDate = eRPLeadSalesPersonInformationDto.lojCreatedDate,
					lojUniqueID = eRPLeadSalesPersonInformationDto.lojUniqueID,
					lojLeadID = eRPLeadSalesPersonInformationDto.lojLeadID,
					lojPercent = eRPLeadSalesPersonInformationDto.lojPercent,
					lojRowVersion = eRPLeadSalesPersonInformationDto.lojRowVersion,
					lojSalesEmployeeID = eRPLeadSalesPersonInformationDto.lojSalesEmployeeID,
					lojSequenceID = eRPLeadSalesPersonInformationDto.lojSequenceID,
					CustomFields = eRPLeadSalesPersonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the LeadSalesPeople []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = leadSalesPersonDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPLeadSalesPersonDto>> Process_PutLeadSalesPerson(ERPLeadSalesPersonDto leadSalesPerson)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPLeadSalesPersonDto createdObject = null;
		ERPResponseMessageDto<ERPLeadSalesPersonDto> result;
		try
		{
			IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
			using (iERPLeadSalesPersonRepository)
			{
				APIValidationInfoDto postResult = await base.ERPLeadSalesPersonRepository.SaveLeadSalesPerson(leadSalesPerson);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPLeadSalesPersonInformationDto eRPLeadSalesPersonInformationDto = await base.ERPLeadSalesPersonRepository.GetLeadSalesPerson(leadSalesPerson.lojUniqueID);
					createdObject = new ERPLeadSalesPersonDto
					{
						lojCreatedBy = eRPLeadSalesPersonInformationDto.lojCreatedBy,
						lojCreatedDate = eRPLeadSalesPersonInformationDto.lojCreatedDate,
						lojUniqueID = eRPLeadSalesPersonInformationDto.lojUniqueID,
						lojLeadID = eRPLeadSalesPersonInformationDto.lojLeadID,
						lojPercent = eRPLeadSalesPersonInformationDto.lojPercent,
						lojRowVersion = eRPLeadSalesPersonInformationDto.lojRowVersion,
						lojSalesEmployeeID = eRPLeadSalesPersonInformationDto.lojSalesEmployeeID,
						lojSequenceID = eRPLeadSalesPersonInformationDto.lojSequenceID,
						CustomFields = eRPLeadSalesPersonInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing LeadSalesPerson [{leadSalesPerson.lojUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteLeadSalesPerson(Guid leadSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
		using (iERPLeadSalesPersonRepository)
		{
			if (!(await base.ERPLeadSalesPersonRepository.DoesLeadSalesPersonExist(leadSalesPersonId)))
			{
				base.ErrorsList.Add($"LeadSalesPerson [{leadSalesPersonId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPLeadSalesPersonInformationDto eRPLeadSalesPersonInformationDto = await base.ERPLeadSalesPersonRepository.GetLeadSalesPerson(leadSalesPersonId);
				string text = await base.ERPLeadSalesPersonRepository.WhereUsed("LeadSalesPeople", new object[2] { eRPLeadSalesPersonInformationDto.lojLeadID, eRPLeadSalesPersonInformationDto.lojSequenceID }, new object[2] { "lojLeadID", "lojSequenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("LeadSalesPerson cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPLeadSalesPersonDto>> Process_DeleteLeadSalesPerson(Guid leadSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPLeadSalesPersonDto> result;
		try
		{
			IERPLeadSalesPersonRepository iERPLeadSalesPersonRepository = (base.ERPLeadSalesPersonRepository = new ERPLeadSalesPersonRepository(base.ApiClientContext));
			using (iERPLeadSalesPersonRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPLeadSalesPersonRepository.DeleteRowFromTable("LeadSalesPeople", "loj", leadSalesPersonId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of LeadSalesPerson [{leadSalesPersonId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPLeadSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPLeadSalesPersonDto()
			};
		}
		return result;
	}
}
