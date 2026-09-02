using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPDMRShipmentModel : ERPBaseModel, IERPDMRShipmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllDMRShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
		using (iERPDMRShipmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPDMRShipmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPDMRShipmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPDMRShipmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPDMRShipmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetDMRShipment(Guid dMRShipmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
		using (iERPDMRShipmentRepository)
		{
			if (!(await base.ERPDMRShipmentRepository.DoesDMRShipmentExist(dMRShipmentId)))
			{
				errorsList.Add($"DMRShipment [{dMRShipmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutDMRShipment(ERPDMRShipmentDto dMRShipment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
		using (iERPDMRShipmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspPlantDepartmentID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { dMRShipment.dspPlantID, dMRShipment.dspPlantDepartmentID })))
			{
				errorsList.Add("dspPlantDepartmentID [" + dMRShipment.dspPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspPlantID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { dMRShipment.dspPlantID })))
			{
				errorsList.Add("dspPlantID [" + dMRShipment.dspPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspSupplierOrganizationID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { dMRShipment.dspSupplierOrganizationID })))
			{
				errorsList.Add("dspSupplierOrganizationID [" + dMRShipment.dspSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspShipLocationID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { dMRShipment.dspSupplierOrganizationID, dMRShipment.dspShipLocationID })))
			{
				errorsList.Add("dspShipLocationID [" + dMRShipment.dspShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspShipContactID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { dMRShipment.dspSupplierOrganizationID, dMRShipment.dspShipLocationID, dMRShipment.dspShipContactID })))
			{
				errorsList.Add("dspShipContactID [" + dMRShipment.dspShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspShippingMethodID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { dMRShipment.dspShippingMethodID })))
			{
				errorsList.Add("dspShippingMethodID [" + dMRShipment.dspShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspShippingPaymentTypeID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { dMRShipment.dspShippingPaymentTypeID })))
			{
				errorsList.Add("dspShippingPaymentTypeID [" + dMRShipment.dspShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspStandardMessageID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { dMRShipment.dspStandardMessageID })))
			{
				errorsList.Add("dspStandardMessageID [" + dMRShipment.dspStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspProjectID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { dMRShipment.dspProjectID })))
			{
				errorsList.Add("dspProjectID [" + dMRShipment.dspProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspApInvoiceLocationID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { dMRShipment.dspSupplierOrganizationID, dMRShipment.dspApInvoiceLocationID })))
			{
				errorsList.Add("dspApInvoiceLocationID [" + dMRShipment.dspApInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(dMRShipment.dspCurrencyRateID) && !(await base.ERPDMRShipmentRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { dMRShipment.dspCurrencyRateID })))
			{
				errorsList.Add("dspCurrencyRateID [" + dMRShipment.dspCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPDMRShipmentDto>>> Process_GetAllDMRShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPDMRShipmentDto> allDMRShipmentsDto = new List<ERPDMRShipmentDto>();
		ERPResponseMessageDto<IList<ERPDMRShipmentDto>> result;
		try
		{
			IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
			using (iERPDMRShipmentRepository)
			{
				foreach (ERPDMRShipmentInformationDto item2 in await base.ERPDMRShipmentRepository.GetAllDMRShipments(pageSize, pageNumber, filter, orderBy))
				{
					ERPDMRShipmentDto item = new ERPDMRShipmentDto
					{
						dspApInvoiceLocationID = item2.dspApInvoiceLocationID,
						dspClosedDate = item2.dspClosedDate,
						dspDmrShipmentID = item2.dspDmrShipmentID,
						dspCreatedBy = item2.dspCreatedBy,
						dspCreatedDate = item2.dspCreatedDate,
						dspCurrencyRateID = item2.dspCurrencyRateID,
						dspUniqueID = item2.dspUniqueID,
						dspExchangeRate = item2.dspExchangeRate,
						dspFreightCharge = item2.dspFreightCharge,
						dspFreightChargeForeign = item2.dspFreightChargeForeign,
						dspFreightSubtotal = item2.dspFreightSubtotal,
						dspFreightTotal = item2.dspFreightTotal,
						dspClosed = item2.dspClosed,
						dspCustomRate = item2.dspCustomRate,
						dspPosted = item2.dspPosted,
						dspPrintDmrPackingSlip = item2.dspPrintDmrPackingSlip,
						dspPrintLabels = item2.dspPrintLabels,
						dspReversalEntry = item2.dspReversalEntry,
						dspReversed = item2.dspReversed,
						dspNumberOfLabels = item2.dspNumberOfLabels,
						dspPlantDepartmentID = item2.dspPlantDepartmentID,
						dspPlantID = item2.dspPlantID,
						dspPostedDate = item2.dspPostedDate,
						dspProjectID = item2.dspProjectID,
						dspRowVersion = item2.dspRowVersion,
						dspShipContactID = item2.dspShipContactID,
						dspShipDate = item2.dspShipDate,
						dspShipLocationID = item2.dspShipLocationID,
						dspShippingCommentsRTF = item2.dspShippingCommentsRTF,
						dspShippingCommentsText = item2.dspShippingCommentsText,
						dspShippingMethodID = item2.dspShippingMethodID,
						dspShippingPaymentTypeID = item2.dspShippingPaymentTypeID,
						dspStandardMessageID = item2.dspStandardMessageID,
						dspSupplierOrganizationID = item2.dspSupplierOrganizationID,
						dspTrackingNumber = item2.dspTrackingNumber,
						CustomFields = item2.CustomFields
					};
					allDMRShipmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all DMRShipments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPDMRShipmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allDMRShipmentsDto,
				RecordCount = allDMRShipmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_GetDMRShipment(Guid dMRShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPDMRShipmentDto dMRShipmentDto = null;
		ERPResponseMessageDto<ERPDMRShipmentDto> result;
		try
		{
			IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
			using (iERPDMRShipmentRepository)
			{
				ERPDMRShipmentInformationDto eRPDMRShipmentInformationDto = await base.ERPDMRShipmentRepository.GetDMRShipment(dMRShipmentId);
				dMRShipmentDto = new ERPDMRShipmentDto
				{
					dspApInvoiceLocationID = eRPDMRShipmentInformationDto.dspApInvoiceLocationID,
					dspClosedDate = eRPDMRShipmentInformationDto.dspClosedDate,
					dspDmrShipmentID = eRPDMRShipmentInformationDto.dspDmrShipmentID,
					dspCreatedBy = eRPDMRShipmentInformationDto.dspCreatedBy,
					dspCreatedDate = eRPDMRShipmentInformationDto.dspCreatedDate,
					dspCurrencyRateID = eRPDMRShipmentInformationDto.dspCurrencyRateID,
					dspUniqueID = eRPDMRShipmentInformationDto.dspUniqueID,
					dspExchangeRate = eRPDMRShipmentInformationDto.dspExchangeRate,
					dspFreightCharge = eRPDMRShipmentInformationDto.dspFreightCharge,
					dspFreightChargeForeign = eRPDMRShipmentInformationDto.dspFreightChargeForeign,
					dspFreightSubtotal = eRPDMRShipmentInformationDto.dspFreightSubtotal,
					dspFreightTotal = eRPDMRShipmentInformationDto.dspFreightTotal,
					dspClosed = eRPDMRShipmentInformationDto.dspClosed,
					dspCustomRate = eRPDMRShipmentInformationDto.dspCustomRate,
					dspPosted = eRPDMRShipmentInformationDto.dspPosted,
					dspPrintDmrPackingSlip = eRPDMRShipmentInformationDto.dspPrintDmrPackingSlip,
					dspPrintLabels = eRPDMRShipmentInformationDto.dspPrintLabels,
					dspReversalEntry = eRPDMRShipmentInformationDto.dspReversalEntry,
					dspReversed = eRPDMRShipmentInformationDto.dspReversed,
					dspNumberOfLabels = eRPDMRShipmentInformationDto.dspNumberOfLabels,
					dspPlantDepartmentID = eRPDMRShipmentInformationDto.dspPlantDepartmentID,
					dspPlantID = eRPDMRShipmentInformationDto.dspPlantID,
					dspPostedDate = eRPDMRShipmentInformationDto.dspPostedDate,
					dspProjectID = eRPDMRShipmentInformationDto.dspProjectID,
					dspRowVersion = eRPDMRShipmentInformationDto.dspRowVersion,
					dspShipContactID = eRPDMRShipmentInformationDto.dspShipContactID,
					dspShipDate = eRPDMRShipmentInformationDto.dspShipDate,
					dspShipLocationID = eRPDMRShipmentInformationDto.dspShipLocationID,
					dspShippingCommentsRTF = eRPDMRShipmentInformationDto.dspShippingCommentsRTF,
					dspShippingCommentsText = eRPDMRShipmentInformationDto.dspShippingCommentsText,
					dspShippingMethodID = eRPDMRShipmentInformationDto.dspShippingMethodID,
					dspShippingPaymentTypeID = eRPDMRShipmentInformationDto.dspShippingPaymentTypeID,
					dspStandardMessageID = eRPDMRShipmentInformationDto.dspStandardMessageID,
					dspSupplierOrganizationID = eRPDMRShipmentInformationDto.dspSupplierOrganizationID,
					dspTrackingNumber = eRPDMRShipmentInformationDto.dspTrackingNumber,
					CustomFields = eRPDMRShipmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the DMRShipments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = dMRShipmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_PutDMRShipment(ERPDMRShipmentDto dMRShipment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPDMRShipmentDto createdObject = null;
		ERPResponseMessageDto<ERPDMRShipmentDto> result;
		try
		{
			IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
			using (iERPDMRShipmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPDMRShipmentRepository.SaveDMRShipment(dMRShipment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPDMRShipmentInformationDto eRPDMRShipmentInformationDto = await base.ERPDMRShipmentRepository.GetDMRShipment(dMRShipment.dspUniqueID);
					createdObject = new ERPDMRShipmentDto
					{
						dspApInvoiceLocationID = eRPDMRShipmentInformationDto.dspApInvoiceLocationID,
						dspClosedDate = eRPDMRShipmentInformationDto.dspClosedDate,
						dspDmrShipmentID = eRPDMRShipmentInformationDto.dspDmrShipmentID,
						dspCreatedBy = eRPDMRShipmentInformationDto.dspCreatedBy,
						dspCreatedDate = eRPDMRShipmentInformationDto.dspCreatedDate,
						dspCurrencyRateID = eRPDMRShipmentInformationDto.dspCurrencyRateID,
						dspUniqueID = eRPDMRShipmentInformationDto.dspUniqueID,
						dspExchangeRate = eRPDMRShipmentInformationDto.dspExchangeRate,
						dspFreightCharge = eRPDMRShipmentInformationDto.dspFreightCharge,
						dspFreightChargeForeign = eRPDMRShipmentInformationDto.dspFreightChargeForeign,
						dspFreightSubtotal = eRPDMRShipmentInformationDto.dspFreightSubtotal,
						dspFreightTotal = eRPDMRShipmentInformationDto.dspFreightTotal,
						dspClosed = eRPDMRShipmentInformationDto.dspClosed,
						dspCustomRate = eRPDMRShipmentInformationDto.dspCustomRate,
						dspPosted = eRPDMRShipmentInformationDto.dspPosted,
						dspPrintDmrPackingSlip = eRPDMRShipmentInformationDto.dspPrintDmrPackingSlip,
						dspPrintLabels = eRPDMRShipmentInformationDto.dspPrintLabels,
						dspReversalEntry = eRPDMRShipmentInformationDto.dspReversalEntry,
						dspReversed = eRPDMRShipmentInformationDto.dspReversed,
						dspNumberOfLabels = eRPDMRShipmentInformationDto.dspNumberOfLabels,
						dspPlantDepartmentID = eRPDMRShipmentInformationDto.dspPlantDepartmentID,
						dspPlantID = eRPDMRShipmentInformationDto.dspPlantID,
						dspPostedDate = eRPDMRShipmentInformationDto.dspPostedDate,
						dspProjectID = eRPDMRShipmentInformationDto.dspProjectID,
						dspRowVersion = eRPDMRShipmentInformationDto.dspRowVersion,
						dspShipContactID = eRPDMRShipmentInformationDto.dspShipContactID,
						dspShipDate = eRPDMRShipmentInformationDto.dspShipDate,
						dspShipLocationID = eRPDMRShipmentInformationDto.dspShipLocationID,
						dspShippingCommentsRTF = eRPDMRShipmentInformationDto.dspShippingCommentsRTF,
						dspShippingCommentsText = eRPDMRShipmentInformationDto.dspShippingCommentsText,
						dspShippingMethodID = eRPDMRShipmentInformationDto.dspShippingMethodID,
						dspShippingPaymentTypeID = eRPDMRShipmentInformationDto.dspShippingPaymentTypeID,
						dspStandardMessageID = eRPDMRShipmentInformationDto.dspStandardMessageID,
						dspSupplierOrganizationID = eRPDMRShipmentInformationDto.dspSupplierOrganizationID,
						dspTrackingNumber = eRPDMRShipmentInformationDto.dspTrackingNumber,
						CustomFields = eRPDMRShipmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing DMRShipment [{dMRShipment.dspUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteDMRShipment(Guid dMRShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
		using (iERPDMRShipmentRepository)
		{
			if (!(await base.ERPDMRShipmentRepository.DoesDMRShipmentExist(dMRShipmentId)))
			{
				base.ErrorsList.Add($"DMRShipment [{dMRShipmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPDMRShipmentInformationDto eRPDMRShipmentInformationDto = await base.ERPDMRShipmentRepository.GetDMRShipment(dMRShipmentId);
				string text = await base.ERPDMRShipmentRepository.WhereUsed("DMRShipments", new object[1] { eRPDMRShipmentInformationDto.dspDmrShipmentID }, new object[1] { "dspDmrShipmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("DMRShipment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPDMRShipmentDto>> Process_DeleteDMRShipment(Guid dMRShipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPDMRShipmentDto> result;
		try
		{
			IERPDMRShipmentRepository iERPDMRShipmentRepository = (base.ERPDMRShipmentRepository = new ERPDMRShipmentRepository(base.ApiClientContext));
			using (iERPDMRShipmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPDMRShipmentRepository.DeleteRowFromTable("DMRShipments", "dsp", dMRShipmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of DMRShipment [{dMRShipmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPDMRShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPDMRShipmentDto()
			};
		}
		return result;
	}
}
