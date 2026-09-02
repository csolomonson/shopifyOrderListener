using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRClaimModel : ERPBaseModel, IERPDMRClaimModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRClaims(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
		using (iERPDMRClaimRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRClaimRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRClaimRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRClaimRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRClaimRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRClaim(Guid dMRClaimId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
		using (iERPDMRClaimRepository)
		{
			if (!(await base.ERPDMRClaimRepository.DoesDMRClaimExist(dMRClaimId)))
			{
				errorsList.Add($"DMRClaim [{dMRClaimId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRClaim(ERPDMRClaimDto dMRClaim)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
		using (iERPDMRClaimRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpPlantDepartmentID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { dMRClaim.dmpPlantID, dMRClaim.dmpPlantDepartmentID })))
			{
				errorsList.Add("dmpPlantDepartmentID [" + dMRClaim.dmpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpPlantID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { dMRClaim.dmpPlantID })))
			{
				errorsList.Add("dmpPlantID [" + dMRClaim.dmpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpProjectID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { dMRClaim.dmpProjectID })))
			{
				errorsList.Add("dmpProjectID [" + dMRClaim.dmpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpSupplierOrganizationID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { dMRClaim.dmpSupplierOrganizationID })))
			{
				errorsList.Add("dmpSupplierOrganizationID [" + dMRClaim.dmpSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpApInvoiceLocationID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { dMRClaim.dmpSupplierOrganizationID, dMRClaim.dmpApInvoiceLocationID })))
			{
				errorsList.Add("dmpApInvoiceLocationID [" + dMRClaim.dmpApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpApInvoiceContactID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { dMRClaim.dmpSupplierOrganizationID, dMRClaim.dmpApInvoiceLocationID, dMRClaim.dmpApInvoiceContactID })))
			{
				errorsList.Add("dmpApInvoiceContactID [" + dMRClaim.dmpApInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpPurchaseLocationID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { dMRClaim.dmpSupplierOrganizationID, dMRClaim.dmpPurchaseLocationID })))
			{
				errorsList.Add("dmpPurchaseLocationID [" + dMRClaim.dmpPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpPurchaseContactID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { dMRClaim.dmpSupplierOrganizationID, dMRClaim.dmpPurchaseLocationID, dMRClaim.dmpPurchaseContactID })))
			{
				errorsList.Add("dmpPurchaseContactID [" + dMRClaim.dmpPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpAuthorizedByEmployeeID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { dMRClaim.dmpAuthorizedByEmployeeID })))
			{
				errorsList.Add("dmpAuthorizedByEmployeeID [" + dMRClaim.dmpAuthorizedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpClosedReasonID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { dMRClaim.dmpClosedReasonID })))
			{
				errorsList.Add("dmpClosedReasonID [" + dMRClaim.dmpClosedReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpProcessedByEmployeeID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { dMRClaim.dmpProcessedByEmployeeID })))
			{
				errorsList.Add("dmpProcessedByEmployeeID [" + dMRClaim.dmpProcessedByEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRClaim.dmpCurrencyRateID) && !(await base.ERPDMRClaimRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { dMRClaim.dmpCurrencyRateID })))
			{
				errorsList.Add("dmpCurrencyRateID [" + dMRClaim.dmpCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRClaimDto>>> Process_GetAllDMRClaims(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRClaimDto> allDMRClaimsDto = new List<ERPDMRClaimDto>();
		ERPResponseMessageDto<IList<ERPDMRClaimDto>> result;
		try
		{
			IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
			using (iERPDMRClaimRepository)
			{
				foreach (ERPDMRClaimInformationDto item2 in await base.ERPDMRClaimRepository.GetAllDMRClaims(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRClaimDto item = new ERPDMRClaimDto
					{
						dmpApInvoiceContactID = item2.dmpApInvoiceContactID,
						dmpApInvoiceLocationID = item2.dmpApInvoiceLocationID,
						dmpAuthorizationDate = item2.dmpAuthorizationDate,
						dmpAuthorizationNumber = item2.dmpAuthorizationNumber,
						dmpAuthorizedByEmployeeID = item2.dmpAuthorizedByEmployeeID,
						dmpClaimDate = item2.dmpClaimDate,
						dmpClaimTotal = item2.dmpClaimTotal,
						dmpClaimTotalForeign = item2.dmpClaimTotalForeign,
						dmpClosedDate = item2.dmpClosedDate,
						dmpClosedReasonID = item2.dmpClosedReasonID,
						dmpDmrClaimID = item2.dmpDmrClaimID,
						dmpCreatedBy = item2.dmpCreatedBy,
						dmpCreatedDate = item2.dmpCreatedDate,
						dmpCurrencyRateID = item2.dmpCurrencyRateID,
						dmpUniqueID = item2.dmpUniqueID,
						dmpExchangeRate = item2.dmpExchangeRate,
						dmpCustomRate = item2.dmpCustomRate,
						dmpPlantDepartmentID = item2.dmpPlantDepartmentID,
						dmpPlantID = item2.dmpPlantID,
						dmpProcessedByEmployeeID = item2.dmpProcessedByEmployeeID,
						dmpProjectID = item2.dmpProjectID,
						dmpPurchaseContactID = item2.dmpPurchaseContactID,
						dmpPurchaseLocationID = item2.dmpPurchaseLocationID,
						dmpReference = item2.dmpReference,
						dmpRequestedDate = item2.dmpRequestedDate,
						dmpRowVersion = item2.dmpRowVersion,
						dmpStatus = item2.dmpStatus,
						dmpSupplierOrganizationID = item2.dmpSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allDMRClaimsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRClaims]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRClaimDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRClaimsDto,
				RecordCount = allDMRClaimsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_GetDMRClaim(Guid dMRClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRClaimDto dMRClaimDto = null;
		ERPResponseMessageDto<ERPDMRClaimDto> result;
		try
		{
			IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
			using (iERPDMRClaimRepository)
			{
				ERPDMRClaimInformationDto eRPDMRClaimInformationDto = await base.ERPDMRClaimRepository.GetDMRClaim(dMRClaimId);
				dMRClaimDto = new ERPDMRClaimDto
				{
					dmpApInvoiceContactID = eRPDMRClaimInformationDto.dmpApInvoiceContactID,
					dmpApInvoiceLocationID = eRPDMRClaimInformationDto.dmpApInvoiceLocationID,
					dmpAuthorizationDate = eRPDMRClaimInformationDto.dmpAuthorizationDate,
					dmpAuthorizationNumber = eRPDMRClaimInformationDto.dmpAuthorizationNumber,
					dmpAuthorizedByEmployeeID = eRPDMRClaimInformationDto.dmpAuthorizedByEmployeeID,
					dmpClaimDate = eRPDMRClaimInformationDto.dmpClaimDate,
					dmpClaimTotal = eRPDMRClaimInformationDto.dmpClaimTotal,
					dmpClaimTotalForeign = eRPDMRClaimInformationDto.dmpClaimTotalForeign,
					dmpClosedDate = eRPDMRClaimInformationDto.dmpClosedDate,
					dmpClosedReasonID = eRPDMRClaimInformationDto.dmpClosedReasonID,
					dmpDmrClaimID = eRPDMRClaimInformationDto.dmpDmrClaimID,
					dmpCreatedBy = eRPDMRClaimInformationDto.dmpCreatedBy,
					dmpCreatedDate = eRPDMRClaimInformationDto.dmpCreatedDate,
					dmpCurrencyRateID = eRPDMRClaimInformationDto.dmpCurrencyRateID,
					dmpUniqueID = eRPDMRClaimInformationDto.dmpUniqueID,
					dmpExchangeRate = eRPDMRClaimInformationDto.dmpExchangeRate,
					dmpCustomRate = eRPDMRClaimInformationDto.dmpCustomRate,
					dmpPlantDepartmentID = eRPDMRClaimInformationDto.dmpPlantDepartmentID,
					dmpPlantID = eRPDMRClaimInformationDto.dmpPlantID,
					dmpProcessedByEmployeeID = eRPDMRClaimInformationDto.dmpProcessedByEmployeeID,
					dmpProjectID = eRPDMRClaimInformationDto.dmpProjectID,
					dmpPurchaseContactID = eRPDMRClaimInformationDto.dmpPurchaseContactID,
					dmpPurchaseLocationID = eRPDMRClaimInformationDto.dmpPurchaseLocationID,
					dmpReference = eRPDMRClaimInformationDto.dmpReference,
					dmpRequestedDate = eRPDMRClaimInformationDto.dmpRequestedDate,
					dmpRowVersion = eRPDMRClaimInformationDto.dmpRowVersion,
					dmpStatus = eRPDMRClaimInformationDto.dmpStatus,
					dmpSupplierOrganizationID = eRPDMRClaimInformationDto.dmpSupplierOrganizationID,
					CustomFields = eRPDMRClaimInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRClaims []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRClaimDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_PutDMRClaim(ERPDMRClaimDto dMRClaim)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRClaimDto createdObject = null;
		ERPResponseMessageDto<ERPDMRClaimDto> result;
		try
		{
			IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
			using (iERPDMRClaimRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRClaimRepository.SaveDMRClaim(dMRClaim);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRClaimInformationDto eRPDMRClaimInformationDto = await base.ERPDMRClaimRepository.GetDMRClaim(dMRClaim.dmpUniqueID);
					createdObject = new ERPDMRClaimDto
					{
						dmpApInvoiceContactID = eRPDMRClaimInformationDto.dmpApInvoiceContactID,
						dmpApInvoiceLocationID = eRPDMRClaimInformationDto.dmpApInvoiceLocationID,
						dmpAuthorizationDate = eRPDMRClaimInformationDto.dmpAuthorizationDate,
						dmpAuthorizationNumber = eRPDMRClaimInformationDto.dmpAuthorizationNumber,
						dmpAuthorizedByEmployeeID = eRPDMRClaimInformationDto.dmpAuthorizedByEmployeeID,
						dmpClaimDate = eRPDMRClaimInformationDto.dmpClaimDate,
						dmpClaimTotal = eRPDMRClaimInformationDto.dmpClaimTotal,
						dmpClaimTotalForeign = eRPDMRClaimInformationDto.dmpClaimTotalForeign,
						dmpClosedDate = eRPDMRClaimInformationDto.dmpClosedDate,
						dmpClosedReasonID = eRPDMRClaimInformationDto.dmpClosedReasonID,
						dmpDmrClaimID = eRPDMRClaimInformationDto.dmpDmrClaimID,
						dmpCreatedBy = eRPDMRClaimInformationDto.dmpCreatedBy,
						dmpCreatedDate = eRPDMRClaimInformationDto.dmpCreatedDate,
						dmpCurrencyRateID = eRPDMRClaimInformationDto.dmpCurrencyRateID,
						dmpUniqueID = eRPDMRClaimInformationDto.dmpUniqueID,
						dmpExchangeRate = eRPDMRClaimInformationDto.dmpExchangeRate,
						dmpCustomRate = eRPDMRClaimInformationDto.dmpCustomRate,
						dmpPlantDepartmentID = eRPDMRClaimInformationDto.dmpPlantDepartmentID,
						dmpPlantID = eRPDMRClaimInformationDto.dmpPlantID,
						dmpProcessedByEmployeeID = eRPDMRClaimInformationDto.dmpProcessedByEmployeeID,
						dmpProjectID = eRPDMRClaimInformationDto.dmpProjectID,
						dmpPurchaseContactID = eRPDMRClaimInformationDto.dmpPurchaseContactID,
						dmpPurchaseLocationID = eRPDMRClaimInformationDto.dmpPurchaseLocationID,
						dmpReference = eRPDMRClaimInformationDto.dmpReference,
						dmpRequestedDate = eRPDMRClaimInformationDto.dmpRequestedDate,
						dmpRowVersion = eRPDMRClaimInformationDto.dmpRowVersion,
						dmpStatus = eRPDMRClaimInformationDto.dmpStatus,
						dmpSupplierOrganizationID = eRPDMRClaimInformationDto.dmpSupplierOrganizationID,
						CustomFields = eRPDMRClaimInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRClaim [{dMRClaim.dmpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRClaim(Guid dMRClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
		using (iERPDMRClaimRepository)
		{
			if (!(await base.ERPDMRClaimRepository.DoesDMRClaimExist(dMRClaimId)))
			{
				base.ErrorsList.Add($"DMRClaim [{dMRClaimId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRClaimInformationDto eRPDMRClaimInformationDto = await base.ERPDMRClaimRepository.GetDMRClaim(dMRClaimId);
				string text = await base.ERPDMRClaimRepository.WhereUsed("DMRClaims", new object[1] { eRPDMRClaimInformationDto.dmpDmrClaimID }, new object[1] { "dmpDmrClaimID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRClaim cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRClaimDto>> Process_DeleteDMRClaim(Guid dMRClaimId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRClaimDto> result;
		try
		{
			IERPDMRClaimRepository iERPDMRClaimRepository = (base.ERPDMRClaimRepository = new ERPDMRClaimRepository(base.ApiClientContext));
			using (iERPDMRClaimRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRClaimRepository.DeleteRowFromTable("DMRClaims", "dmp", dMRClaimId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRClaim [{dMRClaimId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRClaimDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRClaimDto()
			};
		}
		return result;
	}
}
