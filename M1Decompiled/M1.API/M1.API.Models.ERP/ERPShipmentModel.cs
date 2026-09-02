using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPShipmentModel : ERPBaseModel, IERPShipmentModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
		using (iERPShipmentRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPShipmentRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPShipmentRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPShipmentRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPShipmentRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetShipment(Guid shipmentId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
		using (iERPShipmentRepository)
		{
			if (!(await base.ERPShipmentRepository.DoesShipmentExist(shipmentId)))
			{
				errorsList.Add($"Shipment [{shipmentId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutShipment(ERPShipmentDto shipment)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
		using (iERPShipmentRepository)
		{
			if (!string.IsNullOrWhiteSpace(shipment.smpPlantDepartmentID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { shipment.smpPlantID, shipment.smpPlantDepartmentID })))
			{
				errorsList.Add("smpPlantDepartmentID [" + shipment.smpPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpPlantID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { shipment.smpPlantID })))
			{
				errorsList.Add("smpPlantID [" + shipment.smpPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpCustomerOrganizationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { shipment.smpCustomerOrganizationID })))
			{
				errorsList.Add("smpCustomerOrganizationID [" + shipment.smpCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpArInvoiceLocationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { shipment.smpCustomerOrganizationID, shipment.smpArInvoiceLocationID })))
			{
				errorsList.Add("smpArInvoiceLocationID [" + shipment.smpArInvoiceLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpArInvoiceContactID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { shipment.smpCustomerOrganizationID, shipment.smpArInvoiceLocationID, shipment.smpArInvoiceContactID })))
			{
				errorsList.Add("smpArInvoiceContactID [" + shipment.smpArInvoiceContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpShipOrganizationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { shipment.smpShipOrganizationID })))
			{
				errorsList.Add("smpShipOrganizationID [" + shipment.smpShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpShipLocationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { shipment.smpShipOrganizationID, shipment.smpShipLocationID })))
			{
				errorsList.Add("smpShipLocationID [" + shipment.smpShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpShipContactID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { shipment.smpShipOrganizationID, shipment.smpShipLocationID, shipment.smpShipContactID })))
			{
				errorsList.Add("smpShipContactID [" + shipment.smpShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpShippingMethodID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { shipment.smpShippingMethodID })))
			{
				errorsList.Add("smpShippingMethodID [" + shipment.smpShippingMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpShippingPaymentTypeID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ShippingPaymentTypes", new object[1] { "XAYSHIPPINGPAYMENTTYPEID" }, new object[1] { shipment.smpShippingPaymentTypeID })))
			{
				errorsList.Add("smpShippingPaymentTypeID [" + shipment.smpShippingPaymentTypeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpStandardMessageID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("StandardMessages", new object[1] { "XAMSTANDARDMESSAGEID" }, new object[1] { shipment.smpStandardMessageID })))
			{
				errorsList.Add("smpStandardMessageID [" + shipment.smpStandardMessageID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpProjectID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { shipment.smpProjectID })))
			{
				errorsList.Add("smpProjectID [" + shipment.smpProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpCurrencyRateID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { shipment.smpCurrencyRateID })))
			{
				errorsList.Add("smpCurrencyRateID [" + shipment.smpCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpUps3rdPartyOrganizationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { shipment.smpUps3rdPartyOrganizationID })))
			{
				errorsList.Add("smpUps3rdPartyOrganizationID [" + shipment.smpUps3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpUps3rdPartyLocationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { shipment.smpUps3rdPartyOrganizationID, shipment.smpUps3rdPartyLocationID })))
			{
				errorsList.Add("smpUps3rdPartyLocationID [" + shipment.smpUps3rdPartyLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpBlindShipOrganizationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { shipment.smpBlindShipOrganizationID })))
			{
				errorsList.Add("smpBlindShipOrganizationID [" + shipment.smpBlindShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpBlindShipLocationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { shipment.smpBlindShipOrganizationID, shipment.smpBlindShipLocationID })))
			{
				errorsList.Add("smpBlindShipLocationID [" + shipment.smpBlindShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpBlindShipContactID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONCONTACTS", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { shipment.smpBlindShipOrganizationID, shipment.smpBlindShipLocationID, shipment.smpBlindShipContactID })))
			{
				errorsList.Add("smpBlindShipContactID [" + shipment.smpBlindShipContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpFedEx3rdPartyOrganizationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONS", new object[1] { "CMOORGANIZATIONID" }, new object[1] { shipment.smpFedEx3rdPartyOrganizationID })))
			{
				errorsList.Add("smpFedEx3rdPartyOrganizationID [" + shipment.smpFedEx3rdPartyOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(shipment.smpFedEx3rdPartyLocationID) && !(await base.ERPShipmentRepository.DoesRecordExistInTableUsingKeys("ORGANIZATIONLOCATIONS", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { shipment.smpFedEx3rdPartyOrganizationID, shipment.smpFedEx3rdPartyLocationID })))
			{
				errorsList.Add("smpFedEx3rdPartyLocationID [" + shipment.smpFedEx3rdPartyLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPShipmentDto>>> Process_GetAllShipments(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPShipmentDto> allShipmentsDto = new List<ERPShipmentDto>();
		ERPResponseMessageDto<IList<ERPShipmentDto>> result;
		try
		{
			IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
			using (iERPShipmentRepository)
			{
				foreach (ERPShipmentInformationDto item2 in await base.ERPShipmentRepository.GetAllShipments(pageSize, pageNumber, filter, orderBy))
				{
					ERPShipmentDto item = new ERPShipmentDto
					{
						smpAccBaseChargeBase = item2.smpAccBaseChargeBase,
						smpAccBaseChargeForeign = item2.smpAccBaseChargeForeign,
						smpAccCarrierFreightBase = item2.smpAccCarrierFreightBase,
						smpAccCarrierFreightForeign = item2.smpAccCarrierFreightForeign,
						smpAccDiscountBase = item2.smpAccDiscountBase,
						smpAccDiscountForeign = item2.smpAccDiscountForeign,
						smpAccSurchargeBase = item2.smpAccSurchargeBase,
						smpAccSurchargeForeign = item2.smpAccSurchargeForeign,
						smpAdditionalWeight = item2.smpAdditionalWeight,
						smpAESITN = item2.smpAESITN,
						smpArInvoiceContactID = item2.smpArInvoiceContactID,
						smpArInvoiceLocationID = item2.smpArInvoiceLocationID,
						smpBlindShipContactID = item2.smpBlindShipContactID,
						smpBlindShipLocationID = item2.smpBlindShipLocationID,
						smpBlindShipOrganizationID = item2.smpBlindShipOrganizationID,
						smpCarrierDocumentFilePath = item2.smpCarrierDocumentFilePath,
						smpClosedDate = item2.smpClosedDate,
						smpShipmentID = item2.smpShipmentID,
						smpCodLabelFilePath = item2.smpCodLabelFilePath,
						smpCreatedBy = item2.smpCreatedBy,
						smpCreatedDate = item2.smpCreatedDate,
						smpCurrencyRateID = item2.smpCurrencyRateID,
						smpCustomerOrganizationID = item2.smpCustomerOrganizationID,
						smpDocuments = item2.smpDocuments,
						smpEdiTransferredDate = item2.smpEdiTransferredDate,
						smpUniqueID = item2.smpUniqueID,
						smpExchangeRate = item2.smpExchangeRate,
						smpExportingCarrier = item2.smpExportingCarrier,
						smpFedEx3rdPartyLocationID = item2.smpFedEx3rdPartyLocationID,
						smpFedEx3rdPartyOrganizationID = item2.smpFedEx3rdPartyOrganizationID,
						smpFedExAccountNumber = item2.smpFedExAccountNumber,
						smpFedExBillingOption = item2.smpFedExBillingOption,
						smpFreightCharge = item2.smpFreightCharge,
						smpFreightChargeForeign = item2.smpFreightChargeForeign,
						smpFreightSubtotal = item2.smpFreightSubtotal,
						smpFreightSubtotalForeign = item2.smpFreightSubtotalForeign,
						smpFreightTotal = item2.smpFreightTotal,
						smpFreightTotalForeign = item2.smpFreightTotalForeign,
						smpClosed = item2.smpClosed,
						smpCustomRate = item2.smpCustomRate,
						smpEdiShipmentReady = item2.smpEdiShipmentReady,
						smpEdiTransferred = item2.smpEdiTransferred,
						smpPostedToGl = item2.smpPostedToGl,
						smpPrintLabels = item2.smpPrintLabels,
						smpPrintPackingSlip = item2.smpPrintPackingSlip,
						smpReversalEntry = item2.smpReversalEntry,
						smpReversed = item2.smpReversed,
						smpListBaseChargeBase = item2.smpListBaseChargeBase,
						smpListBaseChargeForeign = item2.smpListBaseChargeForeign,
						smpListCarrierFreightBase = item2.smpListCarrierFreightBase,
						smpListCarrierFreightForeign = item2.smpListCarrierFreightForeign,
						smpListDiscountBase = item2.smpListDiscountBase,
						smpListDiscountForeign = item2.smpListDiscountForeign,
						smpListSurchargeBase = item2.smpListSurchargeBase,
						smpListSurchargeForeign = item2.smpListSurchargeForeign,
						smpNumberOfLabels = item2.smpNumberOfLabels,
						smpPlantDepartmentID = item2.smpPlantDepartmentID,
						smpPlantID = item2.smpPlantID,
						smpPostedDate = item2.smpPostedDate,
						smpProjectID = item2.smpProjectID,
						smpReasonForExport = item2.smpReasonForExport,
						smpReturnInstructionsRTF = item2.smpReturnInstructionsRTF,
						smpReturnInstructionsText = item2.smpReturnInstructionsText,
						smpRowVersion = item2.smpRowVersion,
						smpShipContactID = item2.smpShipContactID,
						smpShipDate = item2.smpShipDate,
						smpShipLocationID = item2.smpShipLocationID,
						smpShipmentIDNumber = item2.smpShipmentIDNumber,
						smpShipmentSubtotal = item2.smpShipmentSubtotal,
						smpShipmentSubtotalForeign = item2.smpShipmentSubtotalForeign,
						smpShipmentTotal = item2.smpShipmentTotal,
						smpShipmentTotalForeign = item2.smpShipmentTotalForeign,
						smpShipOrganizationID = item2.smpShipOrganizationID,
						smpShippingCommentsRTF = item2.smpShippingCommentsRTF,
						smpShippingCommentsText = item2.smpShippingCommentsText,
						smpShippingMethodID = item2.smpShippingMethodID,
						smpShippingPaymentTypeID = item2.smpShippingPaymentTypeID,
						smpStandardMessageID = item2.smpStandardMessageID,
						smpTrackingNumber = item2.smpTrackingNumber,
						smpUps3rdPartyLocationID = item2.smpUps3rdPartyLocationID,
						smpUps3rdPartyOrganizationID = item2.smpUps3rdPartyOrganizationID,
						smpUpsAccountNumber = item2.smpUpsAccountNumber,
						smpUpsBillingOption = item2.smpUpsBillingOption,
						smpWeightSubtotal = item2.smpWeightSubtotal,
						smpWeightTotal = item2.smpWeightTotal,
						CustomFields = item2.CustomFields
					};
					allShipmentsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Shipments]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPShipmentDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allShipmentsDto,
				RecordCount = allShipmentsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentDto>> Process_GetShipment(Guid shipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPShipmentDto shipmentDto = null;
		ERPResponseMessageDto<ERPShipmentDto> result;
		try
		{
			IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
			using (iERPShipmentRepository)
			{
				ERPShipmentInformationDto eRPShipmentInformationDto = await base.ERPShipmentRepository.GetShipment(shipmentId);
				shipmentDto = new ERPShipmentDto
				{
					smpAccBaseChargeBase = eRPShipmentInformationDto.smpAccBaseChargeBase,
					smpAccBaseChargeForeign = eRPShipmentInformationDto.smpAccBaseChargeForeign,
					smpAccCarrierFreightBase = eRPShipmentInformationDto.smpAccCarrierFreightBase,
					smpAccCarrierFreightForeign = eRPShipmentInformationDto.smpAccCarrierFreightForeign,
					smpAccDiscountBase = eRPShipmentInformationDto.smpAccDiscountBase,
					smpAccDiscountForeign = eRPShipmentInformationDto.smpAccDiscountForeign,
					smpAccSurchargeBase = eRPShipmentInformationDto.smpAccSurchargeBase,
					smpAccSurchargeForeign = eRPShipmentInformationDto.smpAccSurchargeForeign,
					smpAdditionalWeight = eRPShipmentInformationDto.smpAdditionalWeight,
					smpAESITN = eRPShipmentInformationDto.smpAESITN,
					smpArInvoiceContactID = eRPShipmentInformationDto.smpArInvoiceContactID,
					smpArInvoiceLocationID = eRPShipmentInformationDto.smpArInvoiceLocationID,
					smpBlindShipContactID = eRPShipmentInformationDto.smpBlindShipContactID,
					smpBlindShipLocationID = eRPShipmentInformationDto.smpBlindShipLocationID,
					smpBlindShipOrganizationID = eRPShipmentInformationDto.smpBlindShipOrganizationID,
					smpCarrierDocumentFilePath = eRPShipmentInformationDto.smpCarrierDocumentFilePath,
					smpClosedDate = eRPShipmentInformationDto.smpClosedDate,
					smpShipmentID = eRPShipmentInformationDto.smpShipmentID,
					smpCodLabelFilePath = eRPShipmentInformationDto.smpCodLabelFilePath,
					smpCreatedBy = eRPShipmentInformationDto.smpCreatedBy,
					smpCreatedDate = eRPShipmentInformationDto.smpCreatedDate,
					smpCurrencyRateID = eRPShipmentInformationDto.smpCurrencyRateID,
					smpCustomerOrganizationID = eRPShipmentInformationDto.smpCustomerOrganizationID,
					smpDocuments = eRPShipmentInformationDto.smpDocuments,
					smpEdiTransferredDate = eRPShipmentInformationDto.smpEdiTransferredDate,
					smpUniqueID = eRPShipmentInformationDto.smpUniqueID,
					smpExchangeRate = eRPShipmentInformationDto.smpExchangeRate,
					smpExportingCarrier = eRPShipmentInformationDto.smpExportingCarrier,
					smpFedEx3rdPartyLocationID = eRPShipmentInformationDto.smpFedEx3rdPartyLocationID,
					smpFedEx3rdPartyOrganizationID = eRPShipmentInformationDto.smpFedEx3rdPartyOrganizationID,
					smpFedExAccountNumber = eRPShipmentInformationDto.smpFedExAccountNumber,
					smpFedExBillingOption = eRPShipmentInformationDto.smpFedExBillingOption,
					smpFreightCharge = eRPShipmentInformationDto.smpFreightCharge,
					smpFreightChargeForeign = eRPShipmentInformationDto.smpFreightChargeForeign,
					smpFreightSubtotal = eRPShipmentInformationDto.smpFreightSubtotal,
					smpFreightSubtotalForeign = eRPShipmentInformationDto.smpFreightSubtotalForeign,
					smpFreightTotal = eRPShipmentInformationDto.smpFreightTotal,
					smpFreightTotalForeign = eRPShipmentInformationDto.smpFreightTotalForeign,
					smpClosed = eRPShipmentInformationDto.smpClosed,
					smpCustomRate = eRPShipmentInformationDto.smpCustomRate,
					smpEdiShipmentReady = eRPShipmentInformationDto.smpEdiShipmentReady,
					smpEdiTransferred = eRPShipmentInformationDto.smpEdiTransferred,
					smpPostedToGl = eRPShipmentInformationDto.smpPostedToGl,
					smpPrintLabels = eRPShipmentInformationDto.smpPrintLabels,
					smpPrintPackingSlip = eRPShipmentInformationDto.smpPrintPackingSlip,
					smpReversalEntry = eRPShipmentInformationDto.smpReversalEntry,
					smpReversed = eRPShipmentInformationDto.smpReversed,
					smpListBaseChargeBase = eRPShipmentInformationDto.smpListBaseChargeBase,
					smpListBaseChargeForeign = eRPShipmentInformationDto.smpListBaseChargeForeign,
					smpListCarrierFreightBase = eRPShipmentInformationDto.smpListCarrierFreightBase,
					smpListCarrierFreightForeign = eRPShipmentInformationDto.smpListCarrierFreightForeign,
					smpListDiscountBase = eRPShipmentInformationDto.smpListDiscountBase,
					smpListDiscountForeign = eRPShipmentInformationDto.smpListDiscountForeign,
					smpListSurchargeBase = eRPShipmentInformationDto.smpListSurchargeBase,
					smpListSurchargeForeign = eRPShipmentInformationDto.smpListSurchargeForeign,
					smpNumberOfLabels = eRPShipmentInformationDto.smpNumberOfLabels,
					smpPlantDepartmentID = eRPShipmentInformationDto.smpPlantDepartmentID,
					smpPlantID = eRPShipmentInformationDto.smpPlantID,
					smpPostedDate = eRPShipmentInformationDto.smpPostedDate,
					smpProjectID = eRPShipmentInformationDto.smpProjectID,
					smpReasonForExport = eRPShipmentInformationDto.smpReasonForExport,
					smpReturnInstructionsRTF = eRPShipmentInformationDto.smpReturnInstructionsRTF,
					smpReturnInstructionsText = eRPShipmentInformationDto.smpReturnInstructionsText,
					smpRowVersion = eRPShipmentInformationDto.smpRowVersion,
					smpShipContactID = eRPShipmentInformationDto.smpShipContactID,
					smpShipDate = eRPShipmentInformationDto.smpShipDate,
					smpShipLocationID = eRPShipmentInformationDto.smpShipLocationID,
					smpShipmentIDNumber = eRPShipmentInformationDto.smpShipmentIDNumber,
					smpShipmentSubtotal = eRPShipmentInformationDto.smpShipmentSubtotal,
					smpShipmentSubtotalForeign = eRPShipmentInformationDto.smpShipmentSubtotalForeign,
					smpShipmentTotal = eRPShipmentInformationDto.smpShipmentTotal,
					smpShipmentTotalForeign = eRPShipmentInformationDto.smpShipmentTotalForeign,
					smpShipOrganizationID = eRPShipmentInformationDto.smpShipOrganizationID,
					smpShippingCommentsRTF = eRPShipmentInformationDto.smpShippingCommentsRTF,
					smpShippingCommentsText = eRPShipmentInformationDto.smpShippingCommentsText,
					smpShippingMethodID = eRPShipmentInformationDto.smpShippingMethodID,
					smpShippingPaymentTypeID = eRPShipmentInformationDto.smpShippingPaymentTypeID,
					smpStandardMessageID = eRPShipmentInformationDto.smpStandardMessageID,
					smpTrackingNumber = eRPShipmentInformationDto.smpTrackingNumber,
					smpUps3rdPartyLocationID = eRPShipmentInformationDto.smpUps3rdPartyLocationID,
					smpUps3rdPartyOrganizationID = eRPShipmentInformationDto.smpUps3rdPartyOrganizationID,
					smpUpsAccountNumber = eRPShipmentInformationDto.smpUpsAccountNumber,
					smpUpsBillingOption = eRPShipmentInformationDto.smpUpsBillingOption,
					smpWeightSubtotal = eRPShipmentInformationDto.smpWeightSubtotal,
					smpWeightTotal = eRPShipmentInformationDto.smpWeightTotal,
					CustomFields = eRPShipmentInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Shipments []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = shipmentDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPShipmentDto>> Process_PutShipment(ERPShipmentDto shipment)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPShipmentDto createdObject = null;
		ERPResponseMessageDto<ERPShipmentDto> result;
		try
		{
			IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
			using (iERPShipmentRepository)
			{
				APIValidationInfoDto postResult = await base.ERPShipmentRepository.SaveShipment(shipment);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPShipmentInformationDto eRPShipmentInformationDto = await base.ERPShipmentRepository.GetShipment(shipment.smpUniqueID);
					createdObject = new ERPShipmentDto
					{
						smpAccBaseChargeBase = eRPShipmentInformationDto.smpAccBaseChargeBase,
						smpAccBaseChargeForeign = eRPShipmentInformationDto.smpAccBaseChargeForeign,
						smpAccCarrierFreightBase = eRPShipmentInformationDto.smpAccCarrierFreightBase,
						smpAccCarrierFreightForeign = eRPShipmentInformationDto.smpAccCarrierFreightForeign,
						smpAccDiscountBase = eRPShipmentInformationDto.smpAccDiscountBase,
						smpAccDiscountForeign = eRPShipmentInformationDto.smpAccDiscountForeign,
						smpAccSurchargeBase = eRPShipmentInformationDto.smpAccSurchargeBase,
						smpAccSurchargeForeign = eRPShipmentInformationDto.smpAccSurchargeForeign,
						smpAdditionalWeight = eRPShipmentInformationDto.smpAdditionalWeight,
						smpAESITN = eRPShipmentInformationDto.smpAESITN,
						smpArInvoiceContactID = eRPShipmentInformationDto.smpArInvoiceContactID,
						smpArInvoiceLocationID = eRPShipmentInformationDto.smpArInvoiceLocationID,
						smpBlindShipContactID = eRPShipmentInformationDto.smpBlindShipContactID,
						smpBlindShipLocationID = eRPShipmentInformationDto.smpBlindShipLocationID,
						smpBlindShipOrganizationID = eRPShipmentInformationDto.smpBlindShipOrganizationID,
						smpCarrierDocumentFilePath = eRPShipmentInformationDto.smpCarrierDocumentFilePath,
						smpClosedDate = eRPShipmentInformationDto.smpClosedDate,
						smpShipmentID = eRPShipmentInformationDto.smpShipmentID,
						smpCodLabelFilePath = eRPShipmentInformationDto.smpCodLabelFilePath,
						smpCreatedBy = eRPShipmentInformationDto.smpCreatedBy,
						smpCreatedDate = eRPShipmentInformationDto.smpCreatedDate,
						smpCurrencyRateID = eRPShipmentInformationDto.smpCurrencyRateID,
						smpCustomerOrganizationID = eRPShipmentInformationDto.smpCustomerOrganizationID,
						smpDocuments = eRPShipmentInformationDto.smpDocuments,
						smpEdiTransferredDate = eRPShipmentInformationDto.smpEdiTransferredDate,
						smpUniqueID = eRPShipmentInformationDto.smpUniqueID,
						smpExchangeRate = eRPShipmentInformationDto.smpExchangeRate,
						smpExportingCarrier = eRPShipmentInformationDto.smpExportingCarrier,
						smpFedEx3rdPartyLocationID = eRPShipmentInformationDto.smpFedEx3rdPartyLocationID,
						smpFedEx3rdPartyOrganizationID = eRPShipmentInformationDto.smpFedEx3rdPartyOrganizationID,
						smpFedExAccountNumber = eRPShipmentInformationDto.smpFedExAccountNumber,
						smpFedExBillingOption = eRPShipmentInformationDto.smpFedExBillingOption,
						smpFreightCharge = eRPShipmentInformationDto.smpFreightCharge,
						smpFreightChargeForeign = eRPShipmentInformationDto.smpFreightChargeForeign,
						smpFreightSubtotal = eRPShipmentInformationDto.smpFreightSubtotal,
						smpFreightSubtotalForeign = eRPShipmentInformationDto.smpFreightSubtotalForeign,
						smpFreightTotal = eRPShipmentInformationDto.smpFreightTotal,
						smpFreightTotalForeign = eRPShipmentInformationDto.smpFreightTotalForeign,
						smpClosed = eRPShipmentInformationDto.smpClosed,
						smpCustomRate = eRPShipmentInformationDto.smpCustomRate,
						smpEdiShipmentReady = eRPShipmentInformationDto.smpEdiShipmentReady,
						smpEdiTransferred = eRPShipmentInformationDto.smpEdiTransferred,
						smpPostedToGl = eRPShipmentInformationDto.smpPostedToGl,
						smpPrintLabels = eRPShipmentInformationDto.smpPrintLabels,
						smpPrintPackingSlip = eRPShipmentInformationDto.smpPrintPackingSlip,
						smpReversalEntry = eRPShipmentInformationDto.smpReversalEntry,
						smpReversed = eRPShipmentInformationDto.smpReversed,
						smpListBaseChargeBase = eRPShipmentInformationDto.smpListBaseChargeBase,
						smpListBaseChargeForeign = eRPShipmentInformationDto.smpListBaseChargeForeign,
						smpListCarrierFreightBase = eRPShipmentInformationDto.smpListCarrierFreightBase,
						smpListCarrierFreightForeign = eRPShipmentInformationDto.smpListCarrierFreightForeign,
						smpListDiscountBase = eRPShipmentInformationDto.smpListDiscountBase,
						smpListDiscountForeign = eRPShipmentInformationDto.smpListDiscountForeign,
						smpListSurchargeBase = eRPShipmentInformationDto.smpListSurchargeBase,
						smpListSurchargeForeign = eRPShipmentInformationDto.smpListSurchargeForeign,
						smpNumberOfLabels = eRPShipmentInformationDto.smpNumberOfLabels,
						smpPlantDepartmentID = eRPShipmentInformationDto.smpPlantDepartmentID,
						smpPlantID = eRPShipmentInformationDto.smpPlantID,
						smpPostedDate = eRPShipmentInformationDto.smpPostedDate,
						smpProjectID = eRPShipmentInformationDto.smpProjectID,
						smpReasonForExport = eRPShipmentInformationDto.smpReasonForExport,
						smpReturnInstructionsRTF = eRPShipmentInformationDto.smpReturnInstructionsRTF,
						smpReturnInstructionsText = eRPShipmentInformationDto.smpReturnInstructionsText,
						smpRowVersion = eRPShipmentInformationDto.smpRowVersion,
						smpShipContactID = eRPShipmentInformationDto.smpShipContactID,
						smpShipDate = eRPShipmentInformationDto.smpShipDate,
						smpShipLocationID = eRPShipmentInformationDto.smpShipLocationID,
						smpShipmentIDNumber = eRPShipmentInformationDto.smpShipmentIDNumber,
						smpShipmentSubtotal = eRPShipmentInformationDto.smpShipmentSubtotal,
						smpShipmentSubtotalForeign = eRPShipmentInformationDto.smpShipmentSubtotalForeign,
						smpShipmentTotal = eRPShipmentInformationDto.smpShipmentTotal,
						smpShipmentTotalForeign = eRPShipmentInformationDto.smpShipmentTotalForeign,
						smpShipOrganizationID = eRPShipmentInformationDto.smpShipOrganizationID,
						smpShippingCommentsRTF = eRPShipmentInformationDto.smpShippingCommentsRTF,
						smpShippingCommentsText = eRPShipmentInformationDto.smpShippingCommentsText,
						smpShippingMethodID = eRPShipmentInformationDto.smpShippingMethodID,
						smpShippingPaymentTypeID = eRPShipmentInformationDto.smpShippingPaymentTypeID,
						smpStandardMessageID = eRPShipmentInformationDto.smpStandardMessageID,
						smpTrackingNumber = eRPShipmentInformationDto.smpTrackingNumber,
						smpUps3rdPartyLocationID = eRPShipmentInformationDto.smpUps3rdPartyLocationID,
						smpUps3rdPartyOrganizationID = eRPShipmentInformationDto.smpUps3rdPartyOrganizationID,
						smpUpsAccountNumber = eRPShipmentInformationDto.smpUpsAccountNumber,
						smpUpsBillingOption = eRPShipmentInformationDto.smpUpsBillingOption,
						smpWeightSubtotal = eRPShipmentInformationDto.smpWeightSubtotal,
						smpWeightTotal = eRPShipmentInformationDto.smpWeightTotal,
						CustomFields = eRPShipmentInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Shipment [{shipment.smpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteShipment(Guid shipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
		using (iERPShipmentRepository)
		{
			if (!(await base.ERPShipmentRepository.DoesShipmentExist(shipmentId)))
			{
				base.ErrorsList.Add($"Shipment [{shipmentId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPShipmentInformationDto eRPShipmentInformationDto = await base.ERPShipmentRepository.GetShipment(shipmentId);
				string text = await base.ERPShipmentRepository.WhereUsed("Shipments", new object[1] { eRPShipmentInformationDto.smpShipmentID }, new object[1] { "smpShipmentID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Shipment cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPShipmentDto>> Process_DeleteShipment(Guid shipmentId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPShipmentDto> result;
		try
		{
			IERPShipmentRepository iERPShipmentRepository = (base.ERPShipmentRepository = new ERPShipmentRepository(base.ApiClientContext));
			using (iERPShipmentRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPShipmentRepository.DeleteRowFromTable("Shipments", "smp", shipmentId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Shipment [{shipmentId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPShipmentDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPShipmentDto()
			};
		}
		return result;
	}
}
