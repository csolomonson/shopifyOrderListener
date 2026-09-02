using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRMAReceiptModel : ERPBaseModel, IERPRMAReceiptModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRMAReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
		using (iERPRMAReceiptRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRMAReceiptRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRMAReceiptRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRMAReceiptRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRMAReceiptRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRMAReceipt(Guid rMAReceiptId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
		using (iERPRMAReceiptRepository)
		{
			if (!(await base.ERPRMAReceiptRepository.DoesRMAReceiptExist(rMAReceiptId)))
			{
				errorsList.Add($"RMAReceipt [{rMAReceiptId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRMAReceipt(ERPRMAReceiptDto rMAReceipt)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
		using (iERPRMAReceiptRepository)
		{
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpPlantDepartmentID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { rMAReceipt.rrpPlantID, rMAReceipt.rrpPlantDepartmentID })))
			{
				errorsList.Add("rrpPlantDepartmentID [" + rMAReceipt.rrpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpPlantID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { rMAReceipt.rrpPlantID })))
			{
				errorsList.Add("rrpPlantID [" + rMAReceipt.rrpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpCustomerOrganizationID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAReceipt.rrpCustomerOrganizationID })))
			{
				errorsList.Add("rrpCustomerOrganizationID [" + rMAReceipt.rrpCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpArInvoiceLocationID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAReceipt.rrpCustomerOrganizationID, rMAReceipt.rrpArInvoiceLocationID })))
			{
				errorsList.Add("rrpArInvoiceLocationID [" + rMAReceipt.rrpArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpArInvoiceContactID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rMAReceipt.rrpCustomerOrganizationID, rMAReceipt.rrpArInvoiceLocationID, rMAReceipt.rrpArInvoiceContactID })))
			{
				errorsList.Add("rrpArInvoiceContactID [" + rMAReceipt.rrpArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpShipOrganizationID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rMAReceipt.rrpShipOrganizationID })))
			{
				errorsList.Add("rrpShipOrganizationID [" + rMAReceipt.rrpShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpShipLocationID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rMAReceipt.rrpShipOrganizationID, rMAReceipt.rrpShipLocationID })))
			{
				errorsList.Add("rrpShipLocationID [" + rMAReceipt.rrpShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpShipContactID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rMAReceipt.rrpShipOrganizationID, rMAReceipt.rrpShipLocationID, rMAReceipt.rrpShipContactID })))
			{
				errorsList.Add("rrpShipContactID [" + rMAReceipt.rrpShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpShippingMethodID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { rMAReceipt.rrpShippingMethodID })))
			{
				errorsList.Add("rrpShippingMethodID [" + rMAReceipt.rrpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpProjectID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { rMAReceipt.rrpProjectID })))
			{
				errorsList.Add("rrpProjectID [" + rMAReceipt.rrpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rMAReceipt.rrpCurrencyRateID) && !(await base.ERPRMAReceiptRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { rMAReceipt.rrpCurrencyRateID })))
			{
				errorsList.Add("rrpCurrencyRateID [" + rMAReceipt.rrpCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRMAReceiptDto>>> Process_GetAllRMAReceipts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRMAReceiptDto> allRMAReceiptsDto = new List<ERPRMAReceiptDto>();
		ERPResponseMessageDto<IList<ERPRMAReceiptDto>> result;
		try
		{
			IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
			using (iERPRMAReceiptRepository)
			{
				foreach (ERPRMAReceiptInformationDto item2 in await base.ERPRMAReceiptRepository.GetAllRMAReceipts(pageSize, pageNumber, filter, orderBy))
				{
					ERPRMAReceiptDto item = new ERPRMAReceiptDto
					{
						rrpArInvoiceContactID = item2.rrpArInvoiceContactID,
						rrpArInvoiceLocationID = item2.rrpArInvoiceLocationID,
						rrpClosedDate = item2.rrpClosedDate,
						rrpRmaReceiptID = item2.rrpRmaReceiptID,
						rrpCreatedBy = item2.rrpCreatedBy,
						rrpCreatedDate = item2.rrpCreatedDate,
						rrpCurrencyRateID = item2.rrpCurrencyRateID,
						rrpCustomerOrganizationID = item2.rrpCustomerOrganizationID,
						rrpDeliveryDocket = item2.rrpDeliveryDocket,
						rrpUniqueID = item2.rrpUniqueID,
						rrpExchangeRate = item2.rrpExchangeRate,
						rrpFreightCharge = item2.rrpFreightCharge,
						rrpFreightChargeForeign = item2.rrpFreightChargeForeign,
						rrpClosed = item2.rrpClosed,
						rrpCustomRate = item2.rrpCustomRate,
						rrpPosted = item2.rrpPosted,
						rrpReversalEntry = item2.rrpReversalEntry,
						rrpReversed = item2.rrpReversed,
						rrpPlantDepartmentID = item2.rrpPlantDepartmentID,
						rrpPlantID = item2.rrpPlantID,
						rrpPostedDate = item2.rrpPostedDate,
						rrpProjectID = item2.rrpProjectID,
						rrpReceiptDate = item2.rrpReceiptDate,
						rrpRowVersion = item2.rrpRowVersion,
						rrpShipContactID = item2.rrpShipContactID,
						rrpShipLocationID = item2.rrpShipLocationID,
						rrpShipOrganizationID = item2.rrpShipOrganizationID,
						rrpShippingMethodID = item2.rrpShippingMethodID,
						CustomFields = item2.CustomFields
					};
					allRMAReceiptsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RMAReceipts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRMAReceiptDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRMAReceiptsDto,
				RecordCount = allRMAReceiptsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_GetRMAReceipt(Guid rMAReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRMAReceiptDto rMAReceiptDto = null;
		ERPResponseMessageDto<ERPRMAReceiptDto> result;
		try
		{
			IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
			using (iERPRMAReceiptRepository)
			{
				ERPRMAReceiptInformationDto eRPRMAReceiptInformationDto = await base.ERPRMAReceiptRepository.GetRMAReceipt(rMAReceiptId);
				rMAReceiptDto = new ERPRMAReceiptDto
				{
					rrpArInvoiceContactID = eRPRMAReceiptInformationDto.rrpArInvoiceContactID,
					rrpArInvoiceLocationID = eRPRMAReceiptInformationDto.rrpArInvoiceLocationID,
					rrpClosedDate = eRPRMAReceiptInformationDto.rrpClosedDate,
					rrpRmaReceiptID = eRPRMAReceiptInformationDto.rrpRmaReceiptID,
					rrpCreatedBy = eRPRMAReceiptInformationDto.rrpCreatedBy,
					rrpCreatedDate = eRPRMAReceiptInformationDto.rrpCreatedDate,
					rrpCurrencyRateID = eRPRMAReceiptInformationDto.rrpCurrencyRateID,
					rrpCustomerOrganizationID = eRPRMAReceiptInformationDto.rrpCustomerOrganizationID,
					rrpDeliveryDocket = eRPRMAReceiptInformationDto.rrpDeliveryDocket,
					rrpUniqueID = eRPRMAReceiptInformationDto.rrpUniqueID,
					rrpExchangeRate = eRPRMAReceiptInformationDto.rrpExchangeRate,
					rrpFreightCharge = eRPRMAReceiptInformationDto.rrpFreightCharge,
					rrpFreightChargeForeign = eRPRMAReceiptInformationDto.rrpFreightChargeForeign,
					rrpClosed = eRPRMAReceiptInformationDto.rrpClosed,
					rrpCustomRate = eRPRMAReceiptInformationDto.rrpCustomRate,
					rrpPosted = eRPRMAReceiptInformationDto.rrpPosted,
					rrpReversalEntry = eRPRMAReceiptInformationDto.rrpReversalEntry,
					rrpReversed = eRPRMAReceiptInformationDto.rrpReversed,
					rrpPlantDepartmentID = eRPRMAReceiptInformationDto.rrpPlantDepartmentID,
					rrpPlantID = eRPRMAReceiptInformationDto.rrpPlantID,
					rrpPostedDate = eRPRMAReceiptInformationDto.rrpPostedDate,
					rrpProjectID = eRPRMAReceiptInformationDto.rrpProjectID,
					rrpReceiptDate = eRPRMAReceiptInformationDto.rrpReceiptDate,
					rrpRowVersion = eRPRMAReceiptInformationDto.rrpRowVersion,
					rrpShipContactID = eRPRMAReceiptInformationDto.rrpShipContactID,
					rrpShipLocationID = eRPRMAReceiptInformationDto.rrpShipLocationID,
					rrpShipOrganizationID = eRPRMAReceiptInformationDto.rrpShipOrganizationID,
					rrpShippingMethodID = eRPRMAReceiptInformationDto.rrpShippingMethodID,
					CustomFields = eRPRMAReceiptInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RMAReceipts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rMAReceiptDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_PutRMAReceipt(ERPRMAReceiptDto rMAReceipt)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRMAReceiptDto createdObject = null;
		ERPResponseMessageDto<ERPRMAReceiptDto> result;
		try
		{
			IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
			using (iERPRMAReceiptRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRMAReceiptRepository.SaveRMAReceipt(rMAReceipt);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRMAReceiptInformationDto eRPRMAReceiptInformationDto = await base.ERPRMAReceiptRepository.GetRMAReceipt(rMAReceipt.rrpUniqueID);
					createdObject = new ERPRMAReceiptDto
					{
						rrpArInvoiceContactID = eRPRMAReceiptInformationDto.rrpArInvoiceContactID,
						rrpArInvoiceLocationID = eRPRMAReceiptInformationDto.rrpArInvoiceLocationID,
						rrpClosedDate = eRPRMAReceiptInformationDto.rrpClosedDate,
						rrpRmaReceiptID = eRPRMAReceiptInformationDto.rrpRmaReceiptID,
						rrpCreatedBy = eRPRMAReceiptInformationDto.rrpCreatedBy,
						rrpCreatedDate = eRPRMAReceiptInformationDto.rrpCreatedDate,
						rrpCurrencyRateID = eRPRMAReceiptInformationDto.rrpCurrencyRateID,
						rrpCustomerOrganizationID = eRPRMAReceiptInformationDto.rrpCustomerOrganizationID,
						rrpDeliveryDocket = eRPRMAReceiptInformationDto.rrpDeliveryDocket,
						rrpUniqueID = eRPRMAReceiptInformationDto.rrpUniqueID,
						rrpExchangeRate = eRPRMAReceiptInformationDto.rrpExchangeRate,
						rrpFreightCharge = eRPRMAReceiptInformationDto.rrpFreightCharge,
						rrpFreightChargeForeign = eRPRMAReceiptInformationDto.rrpFreightChargeForeign,
						rrpClosed = eRPRMAReceiptInformationDto.rrpClosed,
						rrpCustomRate = eRPRMAReceiptInformationDto.rrpCustomRate,
						rrpPosted = eRPRMAReceiptInformationDto.rrpPosted,
						rrpReversalEntry = eRPRMAReceiptInformationDto.rrpReversalEntry,
						rrpReversed = eRPRMAReceiptInformationDto.rrpReversed,
						rrpPlantDepartmentID = eRPRMAReceiptInformationDto.rrpPlantDepartmentID,
						rrpPlantID = eRPRMAReceiptInformationDto.rrpPlantID,
						rrpPostedDate = eRPRMAReceiptInformationDto.rrpPostedDate,
						rrpProjectID = eRPRMAReceiptInformationDto.rrpProjectID,
						rrpReceiptDate = eRPRMAReceiptInformationDto.rrpReceiptDate,
						rrpRowVersion = eRPRMAReceiptInformationDto.rrpRowVersion,
						rrpShipContactID = eRPRMAReceiptInformationDto.rrpShipContactID,
						rrpShipLocationID = eRPRMAReceiptInformationDto.rrpShipLocationID,
						rrpShipOrganizationID = eRPRMAReceiptInformationDto.rrpShipOrganizationID,
						rrpShippingMethodID = eRPRMAReceiptInformationDto.rrpShippingMethodID,
						CustomFields = eRPRMAReceiptInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RMAReceipt [{rMAReceipt.rrpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRMAReceipt(Guid rMAReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
		using (iERPRMAReceiptRepository)
		{
			if (!(await base.ERPRMAReceiptRepository.DoesRMAReceiptExist(rMAReceiptId)))
			{
				base.ErrorsList.Add($"RMAReceipt [{rMAReceiptId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRMAReceiptInformationDto eRPRMAReceiptInformationDto = await base.ERPRMAReceiptRepository.GetRMAReceipt(rMAReceiptId);
				string text = await base.ERPRMAReceiptRepository.WhereUsed("RMAReceipts", new object[1] { eRPRMAReceiptInformationDto.rrpRmaReceiptID }, new object[1] { "rrpRmaReceiptID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RMAReceipt cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRMAReceiptDto>> Process_DeleteRMAReceipt(Guid rMAReceiptId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRMAReceiptDto> result;
		try
		{
			IERPRMAReceiptRepository iERPRMAReceiptRepository = (base.ERPRMAReceiptRepository = new ERPRMAReceiptRepository(base.ApiClientContext));
			using (iERPRMAReceiptRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRMAReceiptRepository.DeleteRowFromTable("RMAReceipts", "rrp", rMAReceiptId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RMAReceipt [{rMAReceiptId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRMAReceiptDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRMAReceiptDto()
			};
		}
		return result;
	}
}
