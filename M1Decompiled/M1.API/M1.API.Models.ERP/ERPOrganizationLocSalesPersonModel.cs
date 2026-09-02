using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationLocSalesPersonModel : ERPBaseModel, IERPOrganizationLocSalesPersonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationLocSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
		using (iERPOrganizationLocSalesPersonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationLocSalesPersonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationLocSalesPersonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationLocSalesPersonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationLocSalesPersonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
		using (iERPOrganizationLocSalesPersonRepository)
		{
			if (!(await base.ERPOrganizationLocSalesPersonRepository.DoesOrganizationLocSalesPersonExist(organizationLocSalesPersonId)))
			{
				errorsList.Add($"OrganizationLocSalesPerson [{organizationLocSalesPersonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
		using (iERPOrganizationLocSalesPersonRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationLocSalesPerson.cmkOrganizationID) && !(await base.ERPOrganizationLocSalesPersonRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationLocSalesPerson.cmkOrganizationID })))
			{
				errorsList.Add("cmkOrganizationID [" + organizationLocSalesPerson.cmkOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocSalesPerson.cmkLocationID) && !(await base.ERPOrganizationLocSalesPersonRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationLocSalesPerson.cmkOrganizationID, organizationLocSalesPerson.cmkLocationID })))
			{
				errorsList.Add("cmkLocationID [" + organizationLocSalesPerson.cmkLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationLocSalesPerson.cmkSalesEmployeeID) && !(await base.ERPOrganizationLocSalesPersonRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { organizationLocSalesPerson.cmkSalesEmployeeID })))
			{
				errorsList.Add("cmkSalesEmployeeID [" + organizationLocSalesPerson.cmkSalesEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationLocSalesPersonDto>>> Process_GetAllOrganizationLocSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationLocSalesPersonDto> allOrganizationLocSalesPeopleDto = new List<ERPOrganizationLocSalesPersonDto>();
		ERPResponseMessageDto<IList<ERPOrganizationLocSalesPersonDto>> result;
		try
		{
			IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
			using (iERPOrganizationLocSalesPersonRepository)
			{
				foreach (ERPOrganizationLocSalesPersonInformationDto item2 in await base.ERPOrganizationLocSalesPersonRepository.GetAllOrganizationLocSalesPeople(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationLocSalesPersonDto item = new ERPOrganizationLocSalesPersonDto
					{
						cmkCreatedBy = item2.cmkCreatedBy,
						cmkCreatedDate = item2.cmkCreatedDate,
						cmkUniqueID = item2.cmkUniqueID,
						cmkLocationID = item2.cmkLocationID,
						cmkOrganizationID = item2.cmkOrganizationID,
						cmkPercent = item2.cmkPercent,
						cmkRowVersion = item2.cmkRowVersion,
						cmkSalesEmployeeID = item2.cmkSalesEmployeeID,
						cmkSequenceID = item2.cmkSequenceID,
						CustomFields = item2.CustomFields
					};
					allOrganizationLocSalesPeopleDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationLocSalesPeople]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationLocSalesPersonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationLocSalesPeopleDto,
				RecordCount = allOrganizationLocSalesPeopleDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_GetOrganizationLocSalesPerson(Guid organizationLocSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationLocSalesPersonDto organizationLocSalesPersonDto = null;
		ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto> result;
		try
		{
			IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
			using (iERPOrganizationLocSalesPersonRepository)
			{
				ERPOrganizationLocSalesPersonInformationDto eRPOrganizationLocSalesPersonInformationDto = await base.ERPOrganizationLocSalesPersonRepository.GetOrganizationLocSalesPerson(organizationLocSalesPersonId);
				organizationLocSalesPersonDto = new ERPOrganizationLocSalesPersonDto
				{
					cmkCreatedBy = eRPOrganizationLocSalesPersonInformationDto.cmkCreatedBy,
					cmkCreatedDate = eRPOrganizationLocSalesPersonInformationDto.cmkCreatedDate,
					cmkUniqueID = eRPOrganizationLocSalesPersonInformationDto.cmkUniqueID,
					cmkLocationID = eRPOrganizationLocSalesPersonInformationDto.cmkLocationID,
					cmkOrganizationID = eRPOrganizationLocSalesPersonInformationDto.cmkOrganizationID,
					cmkPercent = eRPOrganizationLocSalesPersonInformationDto.cmkPercent,
					cmkRowVersion = eRPOrganizationLocSalesPersonInformationDto.cmkRowVersion,
					cmkSalesEmployeeID = eRPOrganizationLocSalesPersonInformationDto.cmkSalesEmployeeID,
					cmkSequenceID = eRPOrganizationLocSalesPersonInformationDto.cmkSequenceID,
					CustomFields = eRPOrganizationLocSalesPersonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationLocSalesPeople []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationLocSalesPersonDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_PutOrganizationLocSalesPerson(ERPOrganizationLocSalesPersonDto organizationLocSalesPerson)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationLocSalesPersonDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto> result;
		try
		{
			IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
			using (iERPOrganizationLocSalesPersonRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationLocSalesPersonRepository.SaveOrganizationLocSalesPerson(organizationLocSalesPerson);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationLocSalesPersonInformationDto eRPOrganizationLocSalesPersonInformationDto = await base.ERPOrganizationLocSalesPersonRepository.GetOrganizationLocSalesPerson(organizationLocSalesPerson.cmkUniqueID);
					createdObject = new ERPOrganizationLocSalesPersonDto
					{
						cmkCreatedBy = eRPOrganizationLocSalesPersonInformationDto.cmkCreatedBy,
						cmkCreatedDate = eRPOrganizationLocSalesPersonInformationDto.cmkCreatedDate,
						cmkUniqueID = eRPOrganizationLocSalesPersonInformationDto.cmkUniqueID,
						cmkLocationID = eRPOrganizationLocSalesPersonInformationDto.cmkLocationID,
						cmkOrganizationID = eRPOrganizationLocSalesPersonInformationDto.cmkOrganizationID,
						cmkPercent = eRPOrganizationLocSalesPersonInformationDto.cmkPercent,
						cmkRowVersion = eRPOrganizationLocSalesPersonInformationDto.cmkRowVersion,
						cmkSalesEmployeeID = eRPOrganizationLocSalesPersonInformationDto.cmkSalesEmployeeID,
						cmkSequenceID = eRPOrganizationLocSalesPersonInformationDto.cmkSequenceID,
						CustomFields = eRPOrganizationLocSalesPersonInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationLocSalesPerson [{organizationLocSalesPerson.cmkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationLocSalesPerson(Guid organizationLocSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
		using (iERPOrganizationLocSalesPersonRepository)
		{
			if (!(await base.ERPOrganizationLocSalesPersonRepository.DoesOrganizationLocSalesPersonExist(organizationLocSalesPersonId)))
			{
				base.ErrorsList.Add($"OrganizationLocSalesPerson [{organizationLocSalesPersonId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationLocSalesPersonInformationDto eRPOrganizationLocSalesPersonInformationDto = await base.ERPOrganizationLocSalesPersonRepository.GetOrganizationLocSalesPerson(organizationLocSalesPersonId);
				string text = await base.ERPOrganizationLocSalesPersonRepository.WhereUsed("OrganizationLocSalesPeople", new object[3] { eRPOrganizationLocSalesPersonInformationDto.cmkOrganizationID, eRPOrganizationLocSalesPersonInformationDto.cmkLocationID, eRPOrganizationLocSalesPersonInformationDto.cmkSequenceID }, new object[3] { "cmkOrganizationID", "cmkLocationID", "cmkSequenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationLocSalesPerson cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>> Process_DeleteOrganizationLocSalesPerson(Guid organizationLocSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto> result;
		try
		{
			IERPOrganizationLocSalesPersonRepository iERPOrganizationLocSalesPersonRepository = (base.ERPOrganizationLocSalesPersonRepository = new ERPOrganizationLocSalesPersonRepository(base.ApiClientContext));
			using (iERPOrganizationLocSalesPersonRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationLocSalesPersonRepository.DeleteRowFromTable("OrganizationLocSalesPeople", "cmk", organizationLocSalesPersonId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationLocSalesPerson [{organizationLocSalesPersonId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationLocSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationLocSalesPersonDto()
			};
		}
		return result;
	}
}
