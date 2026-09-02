using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPEmployeePersonalDatumModel : ERPBaseModel, IERPEmployeePersonalDatumModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllEmployeePersonalData(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
		using (iERPEmployeePersonalDatumRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPEmployeePersonalDatumRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPEmployeePersonalDatumRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPEmployeePersonalDatumRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPEmployeePersonalDatumRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetEmployeePersonalDatum(Guid employeePersonalDatumId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
		using (iERPEmployeePersonalDatumRepository)
		{
			if (!(await base.ERPEmployeePersonalDatumRepository.DoesEmployeePersonalDatumExist(employeePersonalDatumId)))
			{
				errorsList.Add($"EmployeePersonalDatum [{employeePersonalDatumId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
		using (iERPEmployeePersonalDatumRepository)
		{
			if (!string.IsNullOrWhiteSpace(employeePersonalDatum.lmdEmployeeID) && !(await base.ERPEmployeePersonalDatumRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { employeePersonalDatum.lmdEmployeeID })))
			{
				errorsList.Add("lmdEmployeeID [" + employeePersonalDatum.lmdEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(employeePersonalDatum.lmdPayrollDefinitionID) && !(await base.ERPEmployeePersonalDatumRepository.DoesRecordExistInTableUsingKeys("PayrollDefinitions", new object[1] { "LMRPAYROLLDEFINITIONID" }, new object[1] { employeePersonalDatum.lmdPayrollDefinitionID })))
			{
				errorsList.Add("lmdPayrollDefinitionID [" + employeePersonalDatum.lmdPayrollDefinitionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPEmployeePersonalDatumDto>>> Process_GetAllEmployeePersonalData(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPEmployeePersonalDatumDto> allEmployeePersonalDataDto = new List<ERPEmployeePersonalDatumDto>();
		ERPResponseMessageDto<IList<ERPEmployeePersonalDatumDto>> result;
		try
		{
			IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
			using (iERPEmployeePersonalDatumRepository)
			{
				foreach (ERPEmployeePersonalDatumInformationDto item2 in await base.ERPEmployeePersonalDatumRepository.GetAllEmployeePersonalData(pageSize, pageNumber, filter, orderBy))
				{
					ERPEmployeePersonalDatumDto item = new ERPEmployeePersonalDatumDto
					{
						lmdAddressLine1 = item2.lmdAddressLine1,
						lmdAddressLine2 = item2.lmdAddressLine2,
						lmdAddressLine3 = item2.lmdAddressLine3,
						lmdBasisOfPayment = item2.lmdBasisOfPayment,
						lmdBirthDate = item2.lmdBirthDate,
						lmdCity = item2.lmdCity,
						lmdContact1HomePhoneNumber = item2.lmdContact1HomePhoneNumber,
						lmdContact1MobilePhoneNumber = item2.lmdContact1MobilePhoneNumber,
						lmdContact1Name = item2.lmdContact1Name,
						lmdContact1Relationship = item2.lmdContact1Relationship,
						lmdContact1WorkPhoneNumber = item2.lmdContact1WorkPhoneNumber,
						lmdContact2HomePhoneNumber = item2.lmdContact2HomePhoneNumber,
						lmdContact2MobilePhoneNumber = item2.lmdContact2MobilePhoneNumber,
						lmdContact2Name = item2.lmdContact2Name,
						lmdContact2Relationship = item2.lmdContact2Relationship,
						lmdContact2WorkPhoneNumber = item2.lmdContact2WorkPhoneNumber,
						lmdCountry = item2.lmdCountry,
						lmdCreatedBy = item2.lmdCreatedBy,
						lmdCreatedDate = item2.lmdCreatedDate,
						lmdEmployeeFirstName = item2.lmdEmployeeFirstName,
						lmdEmployeeID = item2.lmdEmployeeID,
						lmdEmployeeLastName = item2.lmdEmployeeLastName,
						lmdEmployeeMiddleName = item2.lmdEmployeeMiddleName,
						lmdEmploymentDeclarationDate = item2.lmdEmploymentDeclarationDate,
						lmdEmploymentStatus = item2.lmdEmploymentStatus,
						lmdUniqueID = item2.lmdUniqueID,
						lmdFaxNumber = item2.lmdFaxNumber,
						lmdGender = item2.lmdGender,
						lmdHomeCountry = item2.lmdHomeCountry,
						lmdEmploymentDeclarationOnFile = item2.lmdEmploymentDeclarationOnFile,
						lmdPayrollEmployee = item2.lmdPayrollEmployee,
						lmdStdntFinSupplSchemeLoan = item2.lmdStdntFinSupplSchemeLoan,
						lmdStudyTrainLoanRepayment = item2.lmdStudyTrainLoanRepayment,
						lmdTaxFreeThresholdClaimed = item2.lmdTaxFreeThresholdClaimed,
						lmdWorkingHolidayMaker = item2.lmdWorkingHolidayMaker,
						lmdLaborRate = item2.lmdLaborRate,
						lmdMaritalStatus = item2.lmdMaritalStatus,
						lmdMobileNumber = item2.lmdMobileNumber,
						lmdNZTaxCode = item2.lmdNZTaxCode,
						lmdPAYGSummaryType = item2.lmdPAYGSummaryType,
						lmdPayrollDefinitionID = item2.lmdPayrollDefinitionID,
						lmdPayrollExportEmployeeID = item2.lmdPayrollExportEmployeeID,
						lmdPersonalEmailAddress = item2.lmdPersonalEmailAddress,
						lmdPhoneNumber = item2.lmdPhoneNumber,
						lmdPostCode = item2.lmdPostCode,
						lmdResidencyStatus = item2.lmdResidencyStatus,
						lmdRowVersion = item2.lmdRowVersion,
						lmdState = item2.lmdState,
						lmdStateAus = item2.lmdStateAus,
						lmdTaxFileNumber = item2.lmdTaxFileNumber,
						CustomFields = item2.CustomFields
					};
					allEmployeePersonalDataDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all EmployeePersonalData]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPEmployeePersonalDatumDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allEmployeePersonalDataDto,
				RecordCount = allEmployeePersonalDataDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_GetEmployeePersonalDatum(Guid employeePersonalDatumId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPEmployeePersonalDatumDto employeePersonalDatumDto = null;
		ERPResponseMessageDto<ERPEmployeePersonalDatumDto> result;
		try
		{
			IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
			using (iERPEmployeePersonalDatumRepository)
			{
				ERPEmployeePersonalDatumInformationDto eRPEmployeePersonalDatumInformationDto = await base.ERPEmployeePersonalDatumRepository.GetEmployeePersonalDatum(employeePersonalDatumId);
				employeePersonalDatumDto = new ERPEmployeePersonalDatumDto
				{
					lmdAddressLine1 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine1,
					lmdAddressLine2 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine2,
					lmdAddressLine3 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine3,
					lmdBasisOfPayment = eRPEmployeePersonalDatumInformationDto.lmdBasisOfPayment,
					lmdBirthDate = eRPEmployeePersonalDatumInformationDto.lmdBirthDate,
					lmdCity = eRPEmployeePersonalDatumInformationDto.lmdCity,
					lmdContact1HomePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1HomePhoneNumber,
					lmdContact1MobilePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1MobilePhoneNumber,
					lmdContact1Name = eRPEmployeePersonalDatumInformationDto.lmdContact1Name,
					lmdContact1Relationship = eRPEmployeePersonalDatumInformationDto.lmdContact1Relationship,
					lmdContact1WorkPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1WorkPhoneNumber,
					lmdContact2HomePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2HomePhoneNumber,
					lmdContact2MobilePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2MobilePhoneNumber,
					lmdContact2Name = eRPEmployeePersonalDatumInformationDto.lmdContact2Name,
					lmdContact2Relationship = eRPEmployeePersonalDatumInformationDto.lmdContact2Relationship,
					lmdContact2WorkPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2WorkPhoneNumber,
					lmdCountry = eRPEmployeePersonalDatumInformationDto.lmdCountry,
					lmdCreatedBy = eRPEmployeePersonalDatumInformationDto.lmdCreatedBy,
					lmdCreatedDate = eRPEmployeePersonalDatumInformationDto.lmdCreatedDate,
					lmdEmployeeFirstName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeFirstName,
					lmdEmployeeID = eRPEmployeePersonalDatumInformationDto.lmdEmployeeID,
					lmdEmployeeLastName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeLastName,
					lmdEmployeeMiddleName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeMiddleName,
					lmdEmploymentDeclarationDate = eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationDate,
					lmdEmploymentStatus = eRPEmployeePersonalDatumInformationDto.lmdEmploymentStatus,
					lmdUniqueID = eRPEmployeePersonalDatumInformationDto.lmdUniqueID,
					lmdFaxNumber = eRPEmployeePersonalDatumInformationDto.lmdFaxNumber,
					lmdGender = eRPEmployeePersonalDatumInformationDto.lmdGender,
					lmdHomeCountry = eRPEmployeePersonalDatumInformationDto.lmdHomeCountry,
					lmdEmploymentDeclarationOnFile = eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationOnFile,
					lmdPayrollEmployee = eRPEmployeePersonalDatumInformationDto.lmdPayrollEmployee,
					lmdStdntFinSupplSchemeLoan = eRPEmployeePersonalDatumInformationDto.lmdStdntFinSupplSchemeLoan,
					lmdStudyTrainLoanRepayment = eRPEmployeePersonalDatumInformationDto.lmdStudyTrainLoanRepayment,
					lmdTaxFreeThresholdClaimed = eRPEmployeePersonalDatumInformationDto.lmdTaxFreeThresholdClaimed,
					lmdWorkingHolidayMaker = eRPEmployeePersonalDatumInformationDto.lmdWorkingHolidayMaker,
					lmdLaborRate = eRPEmployeePersonalDatumInformationDto.lmdLaborRate,
					lmdMaritalStatus = eRPEmployeePersonalDatumInformationDto.lmdMaritalStatus,
					lmdMobileNumber = eRPEmployeePersonalDatumInformationDto.lmdMobileNumber,
					lmdNZTaxCode = eRPEmployeePersonalDatumInformationDto.lmdNZTaxCode,
					lmdPAYGSummaryType = eRPEmployeePersonalDatumInformationDto.lmdPAYGSummaryType,
					lmdPayrollDefinitionID = eRPEmployeePersonalDatumInformationDto.lmdPayrollDefinitionID,
					lmdPayrollExportEmployeeID = eRPEmployeePersonalDatumInformationDto.lmdPayrollExportEmployeeID,
					lmdPersonalEmailAddress = eRPEmployeePersonalDatumInformationDto.lmdPersonalEmailAddress,
					lmdPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdPhoneNumber,
					lmdPostCode = eRPEmployeePersonalDatumInformationDto.lmdPostCode,
					lmdResidencyStatus = eRPEmployeePersonalDatumInformationDto.lmdResidencyStatus,
					lmdRowVersion = eRPEmployeePersonalDatumInformationDto.lmdRowVersion,
					lmdState = eRPEmployeePersonalDatumInformationDto.lmdState,
					lmdStateAus = eRPEmployeePersonalDatumInformationDto.lmdStateAus,
					lmdTaxFileNumber = eRPEmployeePersonalDatumInformationDto.lmdTaxFileNumber,
					CustomFields = eRPEmployeePersonalDatumInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the EmployeePersonalData []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePersonalDatumDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = employeePersonalDatumDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_PutEmployeePersonalDatum(ERPEmployeePersonalDatumDto employeePersonalDatum)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPEmployeePersonalDatumDto createdObject = null;
		ERPResponseMessageDto<ERPEmployeePersonalDatumDto> result;
		try
		{
			IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
			using (iERPEmployeePersonalDatumRepository)
			{
				APIValidationInfoDto postResult = await base.ERPEmployeePersonalDatumRepository.SaveEmployeePersonalDatum(employeePersonalDatum);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPEmployeePersonalDatumInformationDto eRPEmployeePersonalDatumInformationDto = await base.ERPEmployeePersonalDatumRepository.GetEmployeePersonalDatum(employeePersonalDatum.lmdUniqueID);
					createdObject = new ERPEmployeePersonalDatumDto
					{
						lmdAddressLine1 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine1,
						lmdAddressLine2 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine2,
						lmdAddressLine3 = eRPEmployeePersonalDatumInformationDto.lmdAddressLine3,
						lmdBasisOfPayment = eRPEmployeePersonalDatumInformationDto.lmdBasisOfPayment,
						lmdBirthDate = eRPEmployeePersonalDatumInformationDto.lmdBirthDate,
						lmdCity = eRPEmployeePersonalDatumInformationDto.lmdCity,
						lmdContact1HomePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1HomePhoneNumber,
						lmdContact1MobilePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1MobilePhoneNumber,
						lmdContact1Name = eRPEmployeePersonalDatumInformationDto.lmdContact1Name,
						lmdContact1Relationship = eRPEmployeePersonalDatumInformationDto.lmdContact1Relationship,
						lmdContact1WorkPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact1WorkPhoneNumber,
						lmdContact2HomePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2HomePhoneNumber,
						lmdContact2MobilePhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2MobilePhoneNumber,
						lmdContact2Name = eRPEmployeePersonalDatumInformationDto.lmdContact2Name,
						lmdContact2Relationship = eRPEmployeePersonalDatumInformationDto.lmdContact2Relationship,
						lmdContact2WorkPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdContact2WorkPhoneNumber,
						lmdCountry = eRPEmployeePersonalDatumInformationDto.lmdCountry,
						lmdCreatedBy = eRPEmployeePersonalDatumInformationDto.lmdCreatedBy,
						lmdCreatedDate = eRPEmployeePersonalDatumInformationDto.lmdCreatedDate,
						lmdEmployeeFirstName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeFirstName,
						lmdEmployeeID = eRPEmployeePersonalDatumInformationDto.lmdEmployeeID,
						lmdEmployeeLastName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeLastName,
						lmdEmployeeMiddleName = eRPEmployeePersonalDatumInformationDto.lmdEmployeeMiddleName,
						lmdEmploymentDeclarationDate = eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationDate,
						lmdEmploymentStatus = eRPEmployeePersonalDatumInformationDto.lmdEmploymentStatus,
						lmdUniqueID = eRPEmployeePersonalDatumInformationDto.lmdUniqueID,
						lmdFaxNumber = eRPEmployeePersonalDatumInformationDto.lmdFaxNumber,
						lmdGender = eRPEmployeePersonalDatumInformationDto.lmdGender,
						lmdHomeCountry = eRPEmployeePersonalDatumInformationDto.lmdHomeCountry,
						lmdEmploymentDeclarationOnFile = eRPEmployeePersonalDatumInformationDto.lmdEmploymentDeclarationOnFile,
						lmdPayrollEmployee = eRPEmployeePersonalDatumInformationDto.lmdPayrollEmployee,
						lmdStdntFinSupplSchemeLoan = eRPEmployeePersonalDatumInformationDto.lmdStdntFinSupplSchemeLoan,
						lmdStudyTrainLoanRepayment = eRPEmployeePersonalDatumInformationDto.lmdStudyTrainLoanRepayment,
						lmdTaxFreeThresholdClaimed = eRPEmployeePersonalDatumInformationDto.lmdTaxFreeThresholdClaimed,
						lmdWorkingHolidayMaker = eRPEmployeePersonalDatumInformationDto.lmdWorkingHolidayMaker,
						lmdLaborRate = eRPEmployeePersonalDatumInformationDto.lmdLaborRate,
						lmdMaritalStatus = eRPEmployeePersonalDatumInformationDto.lmdMaritalStatus,
						lmdMobileNumber = eRPEmployeePersonalDatumInformationDto.lmdMobileNumber,
						lmdNZTaxCode = eRPEmployeePersonalDatumInformationDto.lmdNZTaxCode,
						lmdPAYGSummaryType = eRPEmployeePersonalDatumInformationDto.lmdPAYGSummaryType,
						lmdPayrollDefinitionID = eRPEmployeePersonalDatumInformationDto.lmdPayrollDefinitionID,
						lmdPayrollExportEmployeeID = eRPEmployeePersonalDatumInformationDto.lmdPayrollExportEmployeeID,
						lmdPersonalEmailAddress = eRPEmployeePersonalDatumInformationDto.lmdPersonalEmailAddress,
						lmdPhoneNumber = eRPEmployeePersonalDatumInformationDto.lmdPhoneNumber,
						lmdPostCode = eRPEmployeePersonalDatumInformationDto.lmdPostCode,
						lmdResidencyStatus = eRPEmployeePersonalDatumInformationDto.lmdResidencyStatus,
						lmdRowVersion = eRPEmployeePersonalDatumInformationDto.lmdRowVersion,
						lmdState = eRPEmployeePersonalDatumInformationDto.lmdState,
						lmdStateAus = eRPEmployeePersonalDatumInformationDto.lmdStateAus,
						lmdTaxFileNumber = eRPEmployeePersonalDatumInformationDto.lmdTaxFileNumber,
						CustomFields = eRPEmployeePersonalDatumInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing EmployeePersonalDatum [{employeePersonalDatum.lmdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePersonalDatumDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteEmployeePersonalDatum(Guid employeePersonalDatumId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
		using (iERPEmployeePersonalDatumRepository)
		{
			if (!(await base.ERPEmployeePersonalDatumRepository.DoesEmployeePersonalDatumExist(employeePersonalDatumId)))
			{
				base.ErrorsList.Add($"EmployeePersonalDatum [{employeePersonalDatumId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPEmployeePersonalDatumInformationDto eRPEmployeePersonalDatumInformationDto = await base.ERPEmployeePersonalDatumRepository.GetEmployeePersonalDatum(employeePersonalDatumId);
				string text = await base.ERPEmployeePersonalDatumRepository.WhereUsed("EmployeePersonalData", new object[1] { eRPEmployeePersonalDatumInformationDto.lmdEmployeeID }, new object[1] { "lmdEmployeeID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("EmployeePersonalDatum cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPEmployeePersonalDatumDto>> Process_DeleteEmployeePersonalDatum(Guid employeePersonalDatumId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPEmployeePersonalDatumDto> result;
		try
		{
			IERPEmployeePersonalDatumRepository iERPEmployeePersonalDatumRepository = (base.ERPEmployeePersonalDatumRepository = new ERPEmployeePersonalDatumRepository(base.ApiClientContext));
			using (iERPEmployeePersonalDatumRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPEmployeePersonalDatumRepository.DeleteRowFromTable("EmployeePersonalData", "lmd", employeePersonalDatumId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of EmployeePersonalDatum [{employeePersonalDatumId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPEmployeePersonalDatumDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPEmployeePersonalDatumDto()
			};
		}
		return result;
	}
}
