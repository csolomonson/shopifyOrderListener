using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARInvoiceSalesPersonModel : ERPBaseModel, IERPARInvoiceSalesPersonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
		using (iERPARInvoiceSalesPersonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARInvoiceSalesPersonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARInvoiceSalesPersonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARInvoiceSalesPersonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARInvoiceSalesPersonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
		using (iERPARInvoiceSalesPersonRepository)
		{
			if (!(await base.ERPARInvoiceSalesPersonRepository.DoesARInvoiceSalesPersonExist(aRInvoiceSalesPersonId)))
			{
				errorsList.Add($"ARInvoiceSalesPerson [{aRInvoiceSalesPersonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
		using (iERPARInvoiceSalesPersonRepository)
		{
			if (!string.IsNullOrWhiteSpace(aRInvoiceSalesPerson.arjArInvoiceID) && !(await base.ERPARInvoiceSalesPersonRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoiceSalesPerson.arjArInvoiceID })))
			{
				errorsList.Add("arjArInvoiceID [" + aRInvoiceSalesPerson.arjArInvoiceID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(aRInvoiceSalesPerson.arjSalesEmployeeID) && !(await base.ERPARInvoiceSalesPersonRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { aRInvoiceSalesPerson.arjSalesEmployeeID })))
			{
				errorsList.Add("arjSalesEmployeeID [" + aRInvoiceSalesPerson.arjSalesEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARInvoiceSalesPersonDto>>> Process_GetAllARInvoiceSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARInvoiceSalesPersonDto> allARInvoiceSalesPeopleDto = new List<ERPARInvoiceSalesPersonDto>();
		ERPResponseMessageDto<IList<ERPARInvoiceSalesPersonDto>> result;
		try
		{
			IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
			using (iERPARInvoiceSalesPersonRepository)
			{
				foreach (ERPARInvoiceSalesPersonInformationDto item2 in await base.ERPARInvoiceSalesPersonRepository.GetAllARInvoiceSalesPeople(pageSize, pageNumber, filter, orderBy))
				{
					ERPARInvoiceSalesPersonDto item = new ERPARInvoiceSalesPersonDto
					{
						arjAmount = item2.arjAmount,
						arjArInvoiceID = item2.arjArInvoiceID,
						arjCreatedBy = item2.arjCreatedBy,
						arjCreatedDate = item2.arjCreatedDate,
						arjUniqueID = item2.arjUniqueID,
						arjPostedToGl = item2.arjPostedToGl,
						arjPercent = item2.arjPercent,
						arjRate = item2.arjRate,
						arjRowVersion = item2.arjRowVersion,
						arjSalesEmployeeID = item2.arjSalesEmployeeID,
						arjSequenceID = item2.arjSequenceID,
						CustomFields = item2.CustomFields
					};
					allARInvoiceSalesPeopleDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARInvoiceSalesPeople]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARInvoiceSalesPersonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARInvoiceSalesPeopleDto,
				RecordCount = allARInvoiceSalesPeopleDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_GetARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARInvoiceSalesPersonDto aRInvoiceSalesPersonDto = null;
		ERPResponseMessageDto<ERPARInvoiceSalesPersonDto> result;
		try
		{
			IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
			using (iERPARInvoiceSalesPersonRepository)
			{
				ERPARInvoiceSalesPersonInformationDto eRPARInvoiceSalesPersonInformationDto = await base.ERPARInvoiceSalesPersonRepository.GetARInvoiceSalesPerson(aRInvoiceSalesPersonId);
				aRInvoiceSalesPersonDto = new ERPARInvoiceSalesPersonDto
				{
					arjAmount = eRPARInvoiceSalesPersonInformationDto.arjAmount,
					arjArInvoiceID = eRPARInvoiceSalesPersonInformationDto.arjArInvoiceID,
					arjCreatedBy = eRPARInvoiceSalesPersonInformationDto.arjCreatedBy,
					arjCreatedDate = eRPARInvoiceSalesPersonInformationDto.arjCreatedDate,
					arjUniqueID = eRPARInvoiceSalesPersonInformationDto.arjUniqueID,
					arjPostedToGl = eRPARInvoiceSalesPersonInformationDto.arjPostedToGl,
					arjPercent = eRPARInvoiceSalesPersonInformationDto.arjPercent,
					arjRate = eRPARInvoiceSalesPersonInformationDto.arjRate,
					arjRowVersion = eRPARInvoiceSalesPersonInformationDto.arjRowVersion,
					arjSalesEmployeeID = eRPARInvoiceSalesPersonInformationDto.arjSalesEmployeeID,
					arjSequenceID = eRPARInvoiceSalesPersonInformationDto.arjSequenceID,
					CustomFields = eRPARInvoiceSalesPersonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARInvoiceSalesPeople []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRInvoiceSalesPersonDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_PutARInvoiceSalesPerson(ERPARInvoiceSalesPersonDto aRInvoiceSalesPerson)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARInvoiceSalesPersonDto createdObject = null;
		ERPResponseMessageDto<ERPARInvoiceSalesPersonDto> result;
		try
		{
			IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
			using (iERPARInvoiceSalesPersonRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARInvoiceSalesPersonRepository.SaveARInvoiceSalesPerson(aRInvoiceSalesPerson);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARInvoiceSalesPersonInformationDto eRPARInvoiceSalesPersonInformationDto = await base.ERPARInvoiceSalesPersonRepository.GetARInvoiceSalesPerson(aRInvoiceSalesPerson.arjUniqueID);
					createdObject = new ERPARInvoiceSalesPersonDto
					{
						arjAmount = eRPARInvoiceSalesPersonInformationDto.arjAmount,
						arjArInvoiceID = eRPARInvoiceSalesPersonInformationDto.arjArInvoiceID,
						arjCreatedBy = eRPARInvoiceSalesPersonInformationDto.arjCreatedBy,
						arjCreatedDate = eRPARInvoiceSalesPersonInformationDto.arjCreatedDate,
						arjUniqueID = eRPARInvoiceSalesPersonInformationDto.arjUniqueID,
						arjPostedToGl = eRPARInvoiceSalesPersonInformationDto.arjPostedToGl,
						arjPercent = eRPARInvoiceSalesPersonInformationDto.arjPercent,
						arjRate = eRPARInvoiceSalesPersonInformationDto.arjRate,
						arjRowVersion = eRPARInvoiceSalesPersonInformationDto.arjRowVersion,
						arjSalesEmployeeID = eRPARInvoiceSalesPersonInformationDto.arjSalesEmployeeID,
						arjSequenceID = eRPARInvoiceSalesPersonInformationDto.arjSequenceID,
						CustomFields = eRPARInvoiceSalesPersonInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARInvoiceSalesPerson [{aRInvoiceSalesPerson.arjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
		using (iERPARInvoiceSalesPersonRepository)
		{
			if (!(await base.ERPARInvoiceSalesPersonRepository.DoesARInvoiceSalesPersonExist(aRInvoiceSalesPersonId)))
			{
				base.ErrorsList.Add($"ARInvoiceSalesPerson [{aRInvoiceSalesPersonId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARInvoiceSalesPersonInformationDto eRPARInvoiceSalesPersonInformationDto = await base.ERPARInvoiceSalesPersonRepository.GetARInvoiceSalesPerson(aRInvoiceSalesPersonId);
				string text = await base.ERPARInvoiceSalesPersonRepository.WhereUsed("ARInvoiceSalesPeople", new object[2] { eRPARInvoiceSalesPersonInformationDto.arjArInvoiceID, eRPARInvoiceSalesPersonInformationDto.arjSequenceID }, new object[2] { "arjArInvoiceID", "arjSequenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARInvoiceSalesPerson cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>> Process_DeleteARInvoiceSalesPerson(Guid aRInvoiceSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARInvoiceSalesPersonDto> result;
		try
		{
			IERPARInvoiceSalesPersonRepository iERPARInvoiceSalesPersonRepository = (base.ERPARInvoiceSalesPersonRepository = new ERPARInvoiceSalesPersonRepository(base.ApiClientContext));
			using (iERPARInvoiceSalesPersonRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARInvoiceSalesPersonRepository.DeleteRowFromTable("ARInvoiceSalesPeople", "arj", aRInvoiceSalesPersonId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARInvoiceSalesPerson [{aRInvoiceSalesPersonId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARInvoiceSalesPersonDto()
			};
		}
		return result;
	}
}
