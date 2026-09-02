using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPSalesOrderSalesPersonModel : ERPBaseModel, IERPSalesOrderSalesPersonModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllSalesOrderSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
		using (iERPSalesOrderSalesPersonRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPSalesOrderSalesPersonRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPSalesOrderSalesPersonRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPSalesOrderSalesPersonRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPSalesOrderSalesPersonRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
		using (iERPSalesOrderSalesPersonRepository)
		{
			if (!(await base.ERPSalesOrderSalesPersonRepository.DoesSalesOrderSalesPersonExist(salesOrderSalesPersonId)))
			{
				errorsList.Add($"SalesOrderSalesPerson [{salesOrderSalesPersonId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
		using (iERPSalesOrderSalesPersonRepository)
		{
			if (!string.IsNullOrWhiteSpace(salesOrderSalesPerson.omiSalesOrderID) && !(await base.ERPSalesOrderSalesPersonRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { salesOrderSalesPerson.omiSalesOrderID })))
			{
				errorsList.Add("omiSalesOrderID [" + salesOrderSalesPerson.omiSalesOrderID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(salesOrderSalesPerson.omiSalesEmployeeID) && !(await base.ERPSalesOrderSalesPersonRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { salesOrderSalesPerson.omiSalesEmployeeID })))
			{
				errorsList.Add("omiSalesEmployeeID [" + salesOrderSalesPerson.omiSalesEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPSalesOrderSalesPersonDto>>> Process_GetAllSalesOrderSalesPeople(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPSalesOrderSalesPersonDto> allSalesOrderSalesPeopleDto = new List<ERPSalesOrderSalesPersonDto>();
		ERPResponseMessageDto<IList<ERPSalesOrderSalesPersonDto>> result;
		try
		{
			IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
			using (iERPSalesOrderSalesPersonRepository)
			{
				foreach (ERPSalesOrderSalesPersonInformationDto item2 in await base.ERPSalesOrderSalesPersonRepository.GetAllSalesOrderSalesPeople(pageSize, pageNumber, filter, orderBy))
				{
					ERPSalesOrderSalesPersonDto item = new ERPSalesOrderSalesPersonDto
					{
						omiCreatedBy = item2.omiCreatedBy,
						omiCreatedDate = item2.omiCreatedDate,
						omiUniqueID = item2.omiUniqueID,
						omiClosed = item2.omiClosed,
						omiPercent = item2.omiPercent,
						omiRowVersion = item2.omiRowVersion,
						omiSalesEmployeeID = item2.omiSalesEmployeeID,
						omiSalesOrderID = item2.omiSalesOrderID,
						omiSequenceID = item2.omiSequenceID,
						CustomFields = item2.CustomFields
					};
					allSalesOrderSalesPeopleDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all SalesOrderSalesPeople]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPSalesOrderSalesPersonDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allSalesOrderSalesPeopleDto,
				RecordCount = allSalesOrderSalesPeopleDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_GetSalesOrderSalesPerson(Guid salesOrderSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPSalesOrderSalesPersonDto salesOrderSalesPersonDto = null;
		ERPResponseMessageDto<ERPSalesOrderSalesPersonDto> result;
		try
		{
			IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
			using (iERPSalesOrderSalesPersonRepository)
			{
				ERPSalesOrderSalesPersonInformationDto eRPSalesOrderSalesPersonInformationDto = await base.ERPSalesOrderSalesPersonRepository.GetSalesOrderSalesPerson(salesOrderSalesPersonId);
				salesOrderSalesPersonDto = new ERPSalesOrderSalesPersonDto
				{
					omiCreatedBy = eRPSalesOrderSalesPersonInformationDto.omiCreatedBy,
					omiCreatedDate = eRPSalesOrderSalesPersonInformationDto.omiCreatedDate,
					omiUniqueID = eRPSalesOrderSalesPersonInformationDto.omiUniqueID,
					omiClosed = eRPSalesOrderSalesPersonInformationDto.omiClosed,
					omiPercent = eRPSalesOrderSalesPersonInformationDto.omiPercent,
					omiRowVersion = eRPSalesOrderSalesPersonInformationDto.omiRowVersion,
					omiSalesEmployeeID = eRPSalesOrderSalesPersonInformationDto.omiSalesEmployeeID,
					omiSalesOrderID = eRPSalesOrderSalesPersonInformationDto.omiSalesOrderID,
					omiSequenceID = eRPSalesOrderSalesPersonInformationDto.omiSequenceID,
					CustomFields = eRPSalesOrderSalesPersonInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the SalesOrderSalesPeople []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = salesOrderSalesPersonDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_PutSalesOrderSalesPerson(ERPSalesOrderSalesPersonDto salesOrderSalesPerson)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPSalesOrderSalesPersonDto createdObject = null;
		ERPResponseMessageDto<ERPSalesOrderSalesPersonDto> result;
		try
		{
			IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
			using (iERPSalesOrderSalesPersonRepository)
			{
				APIValidationInfoDto postResult = await base.ERPSalesOrderSalesPersonRepository.SaveSalesOrderSalesPerson(salesOrderSalesPerson);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPSalesOrderSalesPersonInformationDto eRPSalesOrderSalesPersonInformationDto = await base.ERPSalesOrderSalesPersonRepository.GetSalesOrderSalesPerson(salesOrderSalesPerson.omiUniqueID);
					createdObject = new ERPSalesOrderSalesPersonDto
					{
						omiCreatedBy = eRPSalesOrderSalesPersonInformationDto.omiCreatedBy,
						omiCreatedDate = eRPSalesOrderSalesPersonInformationDto.omiCreatedDate,
						omiUniqueID = eRPSalesOrderSalesPersonInformationDto.omiUniqueID,
						omiClosed = eRPSalesOrderSalesPersonInformationDto.omiClosed,
						omiPercent = eRPSalesOrderSalesPersonInformationDto.omiPercent,
						omiRowVersion = eRPSalesOrderSalesPersonInformationDto.omiRowVersion,
						omiSalesEmployeeID = eRPSalesOrderSalesPersonInformationDto.omiSalesEmployeeID,
						omiSalesOrderID = eRPSalesOrderSalesPersonInformationDto.omiSalesOrderID,
						omiSequenceID = eRPSalesOrderSalesPersonInformationDto.omiSequenceID,
						CustomFields = eRPSalesOrderSalesPersonInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing SalesOrderSalesPerson [{salesOrderSalesPerson.omiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteSalesOrderSalesPerson(Guid salesOrderSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
		using (iERPSalesOrderSalesPersonRepository)
		{
			if (!(await base.ERPSalesOrderSalesPersonRepository.DoesSalesOrderSalesPersonExist(salesOrderSalesPersonId)))
			{
				base.ErrorsList.Add($"SalesOrderSalesPerson [{salesOrderSalesPersonId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPSalesOrderSalesPersonInformationDto eRPSalesOrderSalesPersonInformationDto = await base.ERPSalesOrderSalesPersonRepository.GetSalesOrderSalesPerson(salesOrderSalesPersonId);
				string text = await base.ERPSalesOrderSalesPersonRepository.WhereUsed("SalesOrderSalesPeople", new object[2] { eRPSalesOrderSalesPersonInformationDto.omiSalesOrderID, eRPSalesOrderSalesPersonInformationDto.omiSequenceID }, new object[2] { "omiSalesOrderID", "omiSequenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("SalesOrderSalesPerson cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>> Process_DeleteSalesOrderSalesPerson(Guid salesOrderSalesPersonId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPSalesOrderSalesPersonDto> result;
		try
		{
			IERPSalesOrderSalesPersonRepository iERPSalesOrderSalesPersonRepository = (base.ERPSalesOrderSalesPersonRepository = new ERPSalesOrderSalesPersonRepository(base.ApiClientContext));
			using (iERPSalesOrderSalesPersonRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPSalesOrderSalesPersonRepository.DeleteRowFromTable("SalesOrderSalesPeople", "omi", salesOrderSalesPersonId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of SalesOrderSalesPerson [{salesOrderSalesPersonId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPSalesOrderSalesPersonDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPSalesOrderSalesPersonDto()
			};
		}
		return result;
	}
}
