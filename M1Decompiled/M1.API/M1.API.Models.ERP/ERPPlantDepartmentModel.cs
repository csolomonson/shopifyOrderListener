using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPlantDepartmentModel : ERPBaseModel, IERPPlantDepartmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPlantDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
		using (iERPPlantDepartmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPlantDepartmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPlantDepartmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPlantDepartmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPlantDepartmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPlantDepartment(Guid plantDepartmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
		using (iERPPlantDepartmentRepository)
		{
			if (!(await base.ERPPlantDepartmentRepository.DoesPlantDepartmentExist(plantDepartmentId)))
			{
				errorsList.Add($"PlantDepartment [{plantDepartmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPlantDepartment(ERPPlantDepartmentDto plantDepartment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
		using (iERPPlantDepartmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArArGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArArGlAccountID })))
			{
				errorsList.Add("xavArArGlAccountID [" + plantDepartment.xavArArGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArCashGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArCashGlAccountID })))
			{
				errorsList.Add("xavArCashGlAccountID [" + plantDepartment.xavArCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArFreightGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArFreightGlAccountID })))
			{
				errorsList.Add("xavArFreightGlAccountID [" + plantDepartment.xavArFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArDiscountGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArDiscountGlAccountID })))
			{
				errorsList.Add("xavArDiscountGlAccountID [" + plantDepartment.xavArDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArSalesGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArSalesGlAccountID })))
			{
				errorsList.Add("xavArSalesGlAccountID [" + plantDepartment.xavArSalesGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArBankAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { plantDepartment.xavArBankAccountID })))
			{
				errorsList.Add("xavArBankAccountID [" + plantDepartment.xavArBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavApApGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavApApGlAccountID })))
			{
				errorsList.Add("xavApApGlAccountID [" + plantDepartment.xavApApGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavApCashGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavApCashGlAccountID })))
			{
				errorsList.Add("xavApCashGlAccountID [" + plantDepartment.xavApCashGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavApFreightGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavApFreightGlAccountID })))
			{
				errorsList.Add("xavApFreightGlAccountID [" + plantDepartment.xavApFreightGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavApDiscountGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavApDiscountGlAccountID })))
			{
				errorsList.Add("xavApDiscountGlAccountID [" + plantDepartment.xavApDiscountGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavApBankAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("BankAccounts", new object[1] { "GLNBANKACCOUNTID" }, new object[1] { plantDepartment.xavApBankAccountID })))
			{
				errorsList.Add("xavApBankAccountID [" + plantDepartment.xavApBankAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(plantDepartment.xavArDepositGlAccountID) && !(await base.ERPPlantDepartmentRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { plantDepartment.xavArDepositGlAccountID })))
			{
				errorsList.Add("xavArDepositGlAccountID [" + plantDepartment.xavArDepositGlAccountID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPlantDepartmentDto>>> Process_GetAllPlantDepartments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPlantDepartmentDto> allPlantDepartmentsDto = new List<ERPPlantDepartmentDto>();
		ERPResponseMessageDto<IList<ERPPlantDepartmentDto>> result;
		try
		{
			IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
			using (iERPPlantDepartmentRepository)
			{
				foreach (ERPPlantDepartmentInformationDto item2 in await base.ERPPlantDepartmentRepository.GetAllPlantDepartments(pageSize, pageNumber, filter, orderBy))
				{
					ERPPlantDepartmentDto item = new ERPPlantDepartmentDto
					{
						xavApApGlAccountID = item2.xavApApGlAccountID,
						xavApBankAccountID = item2.xavApBankAccountID,
						xavApCashGlAccountID = item2.xavApCashGlAccountID,
						xavApDiscountGlAccountID = item2.xavApDiscountGlAccountID,
						xavApFreightGlAccountID = item2.xavApFreightGlAccountID,
						xavArArGlAccountID = item2.xavArArGlAccountID,
						xavArBankAccountID = item2.xavArBankAccountID,
						xavArCashGlAccountID = item2.xavArCashGlAccountID,
						xavArDepositGlAccountID = item2.xavArDepositGlAccountID,
						xavArDiscountGlAccountID = item2.xavArDiscountGlAccountID,
						xavArFreightGlAccountID = item2.xavArFreightGlAccountID,
						xavArSalesGlAccountID = item2.xavArSalesGlAccountID,
						xavPlantDepartmentID = item2.xavPlantDepartmentID,
						xavCreatedBy = item2.xavCreatedBy,
						xavCreatedDate = item2.xavCreatedDate,
						xavUniqueID = item2.xavUniqueID,
						xavEstablishedDate = item2.xavEstablishedDate,
						xavInactiveDate = item2.xavInactiveDate,
						xavInactive = item2.xavInactive,
						xavUseProperties = item2.xavUseProperties,
						xavName = item2.xavName,
						xavPlantID = item2.xavPlantID,
						xavRowVersion = item2.xavRowVersion,
						CustomFields = item2.CustomFields
					};
					allPlantDepartmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PlantDepartments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPlantDepartmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPlantDepartmentsDto,
				RecordCount = allPlantDepartmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_GetPlantDepartment(Guid plantDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPlantDepartmentDto plantDepartmentDto = null;
		ERPResponseMessageDto<ERPPlantDepartmentDto> result;
		try
		{
			IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
			using (iERPPlantDepartmentRepository)
			{
				ERPPlantDepartmentInformationDto eRPPlantDepartmentInformationDto = await base.ERPPlantDepartmentRepository.GetPlantDepartment(plantDepartmentId);
				plantDepartmentDto = new ERPPlantDepartmentDto
				{
					xavApApGlAccountID = eRPPlantDepartmentInformationDto.xavApApGlAccountID,
					xavApBankAccountID = eRPPlantDepartmentInformationDto.xavApBankAccountID,
					xavApCashGlAccountID = eRPPlantDepartmentInformationDto.xavApCashGlAccountID,
					xavApDiscountGlAccountID = eRPPlantDepartmentInformationDto.xavApDiscountGlAccountID,
					xavApFreightGlAccountID = eRPPlantDepartmentInformationDto.xavApFreightGlAccountID,
					xavArArGlAccountID = eRPPlantDepartmentInformationDto.xavArArGlAccountID,
					xavArBankAccountID = eRPPlantDepartmentInformationDto.xavArBankAccountID,
					xavArCashGlAccountID = eRPPlantDepartmentInformationDto.xavArCashGlAccountID,
					xavArDepositGlAccountID = eRPPlantDepartmentInformationDto.xavArDepositGlAccountID,
					xavArDiscountGlAccountID = eRPPlantDepartmentInformationDto.xavArDiscountGlAccountID,
					xavArFreightGlAccountID = eRPPlantDepartmentInformationDto.xavArFreightGlAccountID,
					xavArSalesGlAccountID = eRPPlantDepartmentInformationDto.xavArSalesGlAccountID,
					xavPlantDepartmentID = eRPPlantDepartmentInformationDto.xavPlantDepartmentID,
					xavCreatedBy = eRPPlantDepartmentInformationDto.xavCreatedBy,
					xavCreatedDate = eRPPlantDepartmentInformationDto.xavCreatedDate,
					xavUniqueID = eRPPlantDepartmentInformationDto.xavUniqueID,
					xavEstablishedDate = eRPPlantDepartmentInformationDto.xavEstablishedDate,
					xavInactiveDate = eRPPlantDepartmentInformationDto.xavInactiveDate,
					xavInactive = eRPPlantDepartmentInformationDto.xavInactive,
					xavUseProperties = eRPPlantDepartmentInformationDto.xavUseProperties,
					xavName = eRPPlantDepartmentInformationDto.xavName,
					xavPlantID = eRPPlantDepartmentInformationDto.xavPlantID,
					xavRowVersion = eRPPlantDepartmentInformationDto.xavRowVersion,
					CustomFields = eRPPlantDepartmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PlantDepartments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = plantDepartmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_PutPlantDepartment(ERPPlantDepartmentDto plantDepartment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPlantDepartmentDto createdObject = null;
		ERPResponseMessageDto<ERPPlantDepartmentDto> result;
		try
		{
			IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
			using (iERPPlantDepartmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPlantDepartmentRepository.SavePlantDepartment(plantDepartment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPlantDepartmentInformationDto eRPPlantDepartmentInformationDto = await base.ERPPlantDepartmentRepository.GetPlantDepartment(plantDepartment.xavUniqueID);
					createdObject = new ERPPlantDepartmentDto
					{
						xavApApGlAccountID = eRPPlantDepartmentInformationDto.xavApApGlAccountID,
						xavApBankAccountID = eRPPlantDepartmentInformationDto.xavApBankAccountID,
						xavApCashGlAccountID = eRPPlantDepartmentInformationDto.xavApCashGlAccountID,
						xavApDiscountGlAccountID = eRPPlantDepartmentInformationDto.xavApDiscountGlAccountID,
						xavApFreightGlAccountID = eRPPlantDepartmentInformationDto.xavApFreightGlAccountID,
						xavArArGlAccountID = eRPPlantDepartmentInformationDto.xavArArGlAccountID,
						xavArBankAccountID = eRPPlantDepartmentInformationDto.xavArBankAccountID,
						xavArCashGlAccountID = eRPPlantDepartmentInformationDto.xavArCashGlAccountID,
						xavArDepositGlAccountID = eRPPlantDepartmentInformationDto.xavArDepositGlAccountID,
						xavArDiscountGlAccountID = eRPPlantDepartmentInformationDto.xavArDiscountGlAccountID,
						xavArFreightGlAccountID = eRPPlantDepartmentInformationDto.xavArFreightGlAccountID,
						xavArSalesGlAccountID = eRPPlantDepartmentInformationDto.xavArSalesGlAccountID,
						xavPlantDepartmentID = eRPPlantDepartmentInformationDto.xavPlantDepartmentID,
						xavCreatedBy = eRPPlantDepartmentInformationDto.xavCreatedBy,
						xavCreatedDate = eRPPlantDepartmentInformationDto.xavCreatedDate,
						xavUniqueID = eRPPlantDepartmentInformationDto.xavUniqueID,
						xavEstablishedDate = eRPPlantDepartmentInformationDto.xavEstablishedDate,
						xavInactiveDate = eRPPlantDepartmentInformationDto.xavInactiveDate,
						xavInactive = eRPPlantDepartmentInformationDto.xavInactive,
						xavUseProperties = eRPPlantDepartmentInformationDto.xavUseProperties,
						xavName = eRPPlantDepartmentInformationDto.xavName,
						xavPlantID = eRPPlantDepartmentInformationDto.xavPlantID,
						xavRowVersion = eRPPlantDepartmentInformationDto.xavRowVersion,
						CustomFields = eRPPlantDepartmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PlantDepartment [{plantDepartment.xavUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePlantDepartment(Guid plantDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
		using (iERPPlantDepartmentRepository)
		{
			if (!(await base.ERPPlantDepartmentRepository.DoesPlantDepartmentExist(plantDepartmentId)))
			{
				base.ErrorsList.Add($"PlantDepartment [{plantDepartmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPlantDepartmentInformationDto eRPPlantDepartmentInformationDto = await base.ERPPlantDepartmentRepository.GetPlantDepartment(plantDepartmentId);
				string text = await base.ERPPlantDepartmentRepository.WhereUsed("PlantDepartments", new object[2] { eRPPlantDepartmentInformationDto.xavPlantID, eRPPlantDepartmentInformationDto.xavPlantDepartmentID }, new object[2] { "xavPlantID", "xavPlantDepartmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PlantDepartment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPlantDepartmentDto>> Process_DeletePlantDepartment(Guid plantDepartmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPlantDepartmentDto> result;
		try
		{
			IERPPlantDepartmentRepository iERPPlantDepartmentRepository = (base.ERPPlantDepartmentRepository = new ERPPlantDepartmentRepository(base.ApiClientContext));
			using (iERPPlantDepartmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPlantDepartmentRepository.DeleteRowFromTable("PlantDepartments", "xav", plantDepartmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PlantDepartment [{plantDepartmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPlantDepartmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPlantDepartmentDto()
			};
		}
		return result;
	}
}
