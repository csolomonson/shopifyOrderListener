using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPShipmentRepository : APIBaseRepository, IERPShipmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentExist(Guid shipmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("smpUniqueID|C", shipmentId);
		base.selectList.Add("smpUniqueID");
		return Task.FromResult(GetAsObject("Shipments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentInformationDto>> GetAllShipments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentInformationDto> collection = new List<ERPShipmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[85]
		{
			"smpAccBaseChargeBase", "smpAccBaseChargeForeign", "smpAccCarrierFreightBase", "smpAccCarrierFreightForeign", "smpAccDiscountBase", "smpAccDiscountForeign", "smpAccSurchargeBase", "smpAccSurchargeForeign", "smpAdditionalWeight", "smpAESITN",
			"smpArInvoiceContactID", "smpArInvoiceLocationID", "smpBlindShipContactID", "smpBlindShipLocationID", "smpBlindShipOrganizationID", "smpCarrierDocumentFilePath", "smpClosedDate", "smpShipmentID", "smpCodLabelFilePath", "smpCreatedBy",
			"smpCreatedDate", "smpCurrencyRateID", "smpCustomerOrganizationID", "smpDocuments", "smpEdiTransferredDate", "smpUniqueID", "smpExchangeRate", "smpExportingCarrier", "smpFedEx3rdPartyLocationID", "smpFedEx3rdPartyOrganizationID",
			"smpFedExAccountNumber", "smpFedExBillingOption", "smpFreightCharge", "smpFreightChargeForeign", "smpFreightSubtotal", "smpFreightSubtotalForeign", "smpFreightTotal", "smpFreightTotalForeign", "smpClosed", "smpCustomRate",
			"smpEdiShipmentReady", "smpEdiTransferred", "smpPostedToGl", "smpPrintLabels", "smpPrintPackingSlip", "smpReversalEntry", "smpReversed", "smpListBaseChargeBase", "smpListBaseChargeForeign", "smpListCarrierFreightBase",
			"smpListCarrierFreightForeign", "smpListDiscountBase", "smpListDiscountForeign", "smpListSurchargeBase", "smpListSurchargeForeign", "smpNumberOfLabels", "smpPlantDepartmentID", "smpPlantID", "smpPostedDate", "smpProjectID",
			"smpReasonForExport", "smpReturnInstructionsRTF", "smpReturnInstructionsText", "smpRowVersion", "smpShipContactID", "smpShipDate", "smpShipLocationID", "smpShipmentIDNumber", "smpShipmentSubtotal", "smpShipmentSubtotalForeign",
			"smpShipmentTotal", "smpShipmentTotalForeign", "smpShipOrganizationID", "smpShippingCommentsRTF", "smpShippingCommentsText", "smpShippingMethodID", "smpShippingPaymentTypeID", "smpStandardMessageID", "smpTrackingNumber", "smpUps3rdPartyLocationID",
			"smpUps3rdPartyOrganizationID", "smpUpsAccountNumber", "smpUpsBillingOption", "smpWeightSubtotal", "smpWeightTotal"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Shipments");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("Shipments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentInformationDto eRPShipmentInformationDto = new ERPShipmentInformationDto();
				eRPShipmentInformationDto.smpAccBaseChargeBase = dataTable.Rows[i].Field<decimal>("smpAccBaseChargeBase");
				eRPShipmentInformationDto.smpAccBaseChargeForeign = dataTable.Rows[i].Field<decimal>("smpAccBaseChargeForeign");
				eRPShipmentInformationDto.smpAccCarrierFreightBase = dataTable.Rows[i].Field<decimal>("smpAccCarrierFreightBase");
				eRPShipmentInformationDto.smpAccCarrierFreightForeign = dataTable.Rows[i].Field<decimal>("smpAccCarrierFreightForeign");
				eRPShipmentInformationDto.smpAccDiscountBase = dataTable.Rows[i].Field<decimal>("smpAccDiscountBase");
				eRPShipmentInformationDto.smpAccDiscountForeign = dataTable.Rows[i].Field<decimal>("smpAccDiscountForeign");
				eRPShipmentInformationDto.smpAccSurchargeBase = dataTable.Rows[i].Field<decimal>("smpAccSurchargeBase");
				eRPShipmentInformationDto.smpAccSurchargeForeign = dataTable.Rows[i].Field<decimal>("smpAccSurchargeForeign");
				eRPShipmentInformationDto.smpAdditionalWeight = dataTable.Rows[i].Field<decimal>("smpAdditionalWeight");
				eRPShipmentInformationDto.smpAESITN = dataTable.Rows[i].Field<string>("smpAESITN");
				eRPShipmentInformationDto.smpArInvoiceContactID = dataTable.Rows[i].Field<string>("smpArInvoiceContactID");
				eRPShipmentInformationDto.smpArInvoiceLocationID = dataTable.Rows[i].Field<string>("smpArInvoiceLocationID");
				eRPShipmentInformationDto.smpBlindShipContactID = dataTable.Rows[i].Field<string>("smpBlindShipContactID");
				eRPShipmentInformationDto.smpBlindShipLocationID = dataTable.Rows[i].Field<string>("smpBlindShipLocationID");
				eRPShipmentInformationDto.smpBlindShipOrganizationID = dataTable.Rows[i].Field<string>("smpBlindShipOrganizationID");
				eRPShipmentInformationDto.smpCarrierDocumentFilePath = dataTable.Rows[i].Field<string>("smpCarrierDocumentFilePath");
				eRPShipmentInformationDto.smpClosedDate = dataTable.Rows[i].Field<DateTime?>("smpClosedDate");
				eRPShipmentInformationDto.smpShipmentID = dataTable.Rows[i].Field<string>("smpShipmentID");
				eRPShipmentInformationDto.smpCodLabelFilePath = dataTable.Rows[i].Field<string>("smpCodLabelFilePath");
				eRPShipmentInformationDto.smpCreatedBy = dataTable.Rows[i].Field<string>("smpCreatedBy");
				eRPShipmentInformationDto.smpCreatedDate = dataTable.Rows[i].Field<DateTime?>("smpCreatedDate");
				eRPShipmentInformationDto.smpCurrencyRateID = dataTable.Rows[i].Field<string>("smpCurrencyRateID");
				eRPShipmentInformationDto.smpCustomerOrganizationID = dataTable.Rows[i].Field<string>("smpCustomerOrganizationID");
				eRPShipmentInformationDto.smpDocuments = dataTable.Rows[i].Field<string>("smpDocuments");
				eRPShipmentInformationDto.smpEdiTransferredDate = dataTable.Rows[i].Field<DateTime?>("smpEdiTransferredDate");
				eRPShipmentInformationDto.smpUniqueID = dataTable.Rows[i].Field<Guid>("smpUniqueID");
				eRPShipmentInformationDto.smpExchangeRate = dataTable.Rows[i].Field<decimal>("smpExchangeRate");
				eRPShipmentInformationDto.smpExportingCarrier = dataTable.Rows[i].Field<string>("smpExportingCarrier");
				eRPShipmentInformationDto.smpFedEx3rdPartyLocationID = dataTable.Rows[i].Field<string>("smpFedEx3rdPartyLocationID");
				eRPShipmentInformationDto.smpFedEx3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("smpFedEx3rdPartyOrganizationID");
				eRPShipmentInformationDto.smpFedExAccountNumber = dataTable.Rows[i].Field<string>("smpFedExAccountNumber");
				eRPShipmentInformationDto.smpFedExBillingOption = dataTable.Rows[i].Field<string>("smpFedExBillingOption");
				eRPShipmentInformationDto.smpFreightCharge = dataTable.Rows[i].Field<decimal>("smpFreightCharge");
				eRPShipmentInformationDto.smpFreightChargeForeign = dataTable.Rows[i].Field<decimal>("smpFreightChargeForeign");
				eRPShipmentInformationDto.smpFreightSubtotal = dataTable.Rows[i].Field<decimal>("smpFreightSubtotal");
				eRPShipmentInformationDto.smpFreightSubtotalForeign = dataTable.Rows[i].Field<decimal>("smpFreightSubtotalForeign");
				eRPShipmentInformationDto.smpFreightTotal = dataTable.Rows[i].Field<decimal>("smpFreightTotal");
				eRPShipmentInformationDto.smpFreightTotalForeign = dataTable.Rows[i].Field<decimal>("smpFreightTotalForeign");
				eRPShipmentInformationDto.smpClosed = dataTable.Rows[i].Field<bool>("smpClosed");
				eRPShipmentInformationDto.smpCustomRate = dataTable.Rows[i].Field<bool>("smpCustomRate");
				eRPShipmentInformationDto.smpEdiShipmentReady = dataTable.Rows[i].Field<bool>("smpEdiShipmentReady");
				eRPShipmentInformationDto.smpEdiTransferred = dataTable.Rows[i].Field<bool>("smpEdiTransferred");
				eRPShipmentInformationDto.smpPostedToGl = dataTable.Rows[i].Field<bool>("smpPostedToGl");
				eRPShipmentInformationDto.smpPrintLabels = dataTable.Rows[i].Field<bool>("smpPrintLabels");
				eRPShipmentInformationDto.smpPrintPackingSlip = dataTable.Rows[i].Field<bool>("smpPrintPackingSlip");
				eRPShipmentInformationDto.smpReversalEntry = dataTable.Rows[i].Field<bool>("smpReversalEntry");
				eRPShipmentInformationDto.smpReversed = dataTable.Rows[i].Field<bool>("smpReversed");
				eRPShipmentInformationDto.smpListBaseChargeBase = dataTable.Rows[i].Field<decimal>("smpListBaseChargeBase");
				eRPShipmentInformationDto.smpListBaseChargeForeign = dataTable.Rows[i].Field<decimal>("smpListBaseChargeForeign");
				eRPShipmentInformationDto.smpListCarrierFreightBase = dataTable.Rows[i].Field<decimal>("smpListCarrierFreightBase");
				eRPShipmentInformationDto.smpListCarrierFreightForeign = dataTable.Rows[i].Field<decimal>("smpListCarrierFreightForeign");
				eRPShipmentInformationDto.smpListDiscountBase = dataTable.Rows[i].Field<decimal>("smpListDiscountBase");
				eRPShipmentInformationDto.smpListDiscountForeign = dataTable.Rows[i].Field<decimal>("smpListDiscountForeign");
				eRPShipmentInformationDto.smpListSurchargeBase = dataTable.Rows[i].Field<decimal>("smpListSurchargeBase");
				eRPShipmentInformationDto.smpListSurchargeForeign = dataTable.Rows[i].Field<decimal>("smpListSurchargeForeign");
				eRPShipmentInformationDto.smpNumberOfLabels = dataTable.Rows[i].Field<short>("smpNumberOfLabels");
				eRPShipmentInformationDto.smpPlantDepartmentID = dataTable.Rows[i].Field<string>("smpPlantDepartmentID");
				eRPShipmentInformationDto.smpPlantID = dataTable.Rows[i].Field<string>("smpPlantID");
				eRPShipmentInformationDto.smpPostedDate = dataTable.Rows[i].Field<DateTime?>("smpPostedDate");
				eRPShipmentInformationDto.smpProjectID = dataTable.Rows[i].Field<string>("smpProjectID");
				eRPShipmentInformationDto.smpReasonForExport = dataTable.Rows[i].Field<string>("smpReasonForExport");
				eRPShipmentInformationDto.smpReturnInstructionsRTF = dataTable.Rows[i].Field<string>("smpReturnInstructionsRTF");
				eRPShipmentInformationDto.smpReturnInstructionsText = dataTable.Rows[i].Field<string>("smpReturnInstructionsText");
				eRPShipmentInformationDto.smpRowVersion = dataTable.Rows[i].Field<byte[]>("smpRowVersion");
				eRPShipmentInformationDto.smpShipContactID = dataTable.Rows[i].Field<string>("smpShipContactID");
				eRPShipmentInformationDto.smpShipDate = dataTable.Rows[i].Field<DateTime?>("smpShipDate");
				eRPShipmentInformationDto.smpShipLocationID = dataTable.Rows[i].Field<string>("smpShipLocationID");
				eRPShipmentInformationDto.smpShipmentIDNumber = dataTable.Rows[i].Field<string>("smpShipmentIDNumber");
				eRPShipmentInformationDto.smpShipmentSubtotal = dataTable.Rows[i].Field<decimal>("smpShipmentSubtotal");
				eRPShipmentInformationDto.smpShipmentSubtotalForeign = dataTable.Rows[i].Field<decimal>("smpShipmentSubtotalForeign");
				eRPShipmentInformationDto.smpShipmentTotal = dataTable.Rows[i].Field<decimal>("smpShipmentTotal");
				eRPShipmentInformationDto.smpShipmentTotalForeign = dataTable.Rows[i].Field<decimal>("smpShipmentTotalForeign");
				eRPShipmentInformationDto.smpShipOrganizationID = dataTable.Rows[i].Field<string>("smpShipOrganizationID");
				eRPShipmentInformationDto.smpShippingCommentsRTF = dataTable.Rows[i].Field<string>("smpShippingCommentsRTF");
				eRPShipmentInformationDto.smpShippingCommentsText = dataTable.Rows[i].Field<string>("smpShippingCommentsText");
				eRPShipmentInformationDto.smpShippingMethodID = dataTable.Rows[i].Field<string>("smpShippingMethodID");
				eRPShipmentInformationDto.smpShippingPaymentTypeID = dataTable.Rows[i].Field<string>("smpShippingPaymentTypeID");
				eRPShipmentInformationDto.smpStandardMessageID = dataTable.Rows[i].Field<string>("smpStandardMessageID");
				eRPShipmentInformationDto.smpTrackingNumber = dataTable.Rows[i].Field<string>("smpTrackingNumber");
				eRPShipmentInformationDto.smpUps3rdPartyLocationID = dataTable.Rows[i].Field<string>("smpUps3rdPartyLocationID");
				eRPShipmentInformationDto.smpUps3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("smpUps3rdPartyOrganizationID");
				eRPShipmentInformationDto.smpUpsAccountNumber = dataTable.Rows[i].Field<string>("smpUpsAccountNumber");
				eRPShipmentInformationDto.smpUpsBillingOption = dataTable.Rows[i].Field<string>("smpUpsBillingOption");
				eRPShipmentInformationDto.smpWeightSubtotal = dataTable.Rows[i].Field<decimal>("smpWeightSubtotal");
				eRPShipmentInformationDto.smpWeightTotal = dataTable.Rows[i].Field<decimal>("smpWeightTotal");
				eRPShipmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentInformationDto> GetShipment(Guid shipmentId)
	{
		ERPShipmentInformationDto eRPShipmentInformationDto = new ERPShipmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[85]
		{
			"smpAccBaseChargeBase", "smpAccBaseChargeForeign", "smpAccCarrierFreightBase", "smpAccCarrierFreightForeign", "smpAccDiscountBase", "smpAccDiscountForeign", "smpAccSurchargeBase", "smpAccSurchargeForeign", "smpAdditionalWeight", "smpAESITN",
			"smpArInvoiceContactID", "smpArInvoiceLocationID", "smpBlindShipContactID", "smpBlindShipLocationID", "smpBlindShipOrganizationID", "smpCarrierDocumentFilePath", "smpClosedDate", "smpShipmentID", "smpCodLabelFilePath", "smpCreatedBy",
			"smpCreatedDate", "smpCurrencyRateID", "smpCustomerOrganizationID", "smpDocuments", "smpEdiTransferredDate", "smpUniqueID", "smpExchangeRate", "smpExportingCarrier", "smpFedEx3rdPartyLocationID", "smpFedEx3rdPartyOrganizationID",
			"smpFedExAccountNumber", "smpFedExBillingOption", "smpFreightCharge", "smpFreightChargeForeign", "smpFreightSubtotal", "smpFreightSubtotalForeign", "smpFreightTotal", "smpFreightTotalForeign", "smpClosed", "smpCustomRate",
			"smpEdiShipmentReady", "smpEdiTransferred", "smpPostedToGl", "smpPrintLabels", "smpPrintPackingSlip", "smpReversalEntry", "smpReversed", "smpListBaseChargeBase", "smpListBaseChargeForeign", "smpListCarrierFreightBase",
			"smpListCarrierFreightForeign", "smpListDiscountBase", "smpListDiscountForeign", "smpListSurchargeBase", "smpListSurchargeForeign", "smpNumberOfLabels", "smpPlantDepartmentID", "smpPlantID", "smpPostedDate", "smpProjectID",
			"smpReasonForExport", "smpReturnInstructionsRTF", "smpReturnInstructionsText", "smpRowVersion", "smpShipContactID", "smpShipDate", "smpShipLocationID", "smpShipmentIDNumber", "smpShipmentSubtotal", "smpShipmentSubtotalForeign",
			"smpShipmentTotal", "smpShipmentTotalForeign", "smpShipOrganizationID", "smpShippingCommentsRTF", "smpShippingCommentsText", "smpShippingMethodID", "smpShippingPaymentTypeID", "smpStandardMessageID", "smpTrackingNumber", "smpUps3rdPartyLocationID",
			"smpUps3rdPartyOrganizationID", "smpUpsAccountNumber", "smpUpsBillingOption", "smpWeightSubtotal", "smpWeightTotal"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("smpUniqueID|C", shipmentId);
		AddCustomFieldsToSelectList("Shipments");
		using (DataTable dataTable = GetAsDataTable("Shipments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentInformationDto);
			}
			eRPShipmentInformationDto.smpAccBaseChargeBase = dataTable.Rows[0].Field<decimal>("smpAccBaseChargeBase");
			eRPShipmentInformationDto.smpAccBaseChargeForeign = dataTable.Rows[0].Field<decimal>("smpAccBaseChargeForeign");
			eRPShipmentInformationDto.smpAccCarrierFreightBase = dataTable.Rows[0].Field<decimal>("smpAccCarrierFreightBase");
			eRPShipmentInformationDto.smpAccCarrierFreightForeign = dataTable.Rows[0].Field<decimal>("smpAccCarrierFreightForeign");
			eRPShipmentInformationDto.smpAccDiscountBase = dataTable.Rows[0].Field<decimal>("smpAccDiscountBase");
			eRPShipmentInformationDto.smpAccDiscountForeign = dataTable.Rows[0].Field<decimal>("smpAccDiscountForeign");
			eRPShipmentInformationDto.smpAccSurchargeBase = dataTable.Rows[0].Field<decimal>("smpAccSurchargeBase");
			eRPShipmentInformationDto.smpAccSurchargeForeign = dataTable.Rows[0].Field<decimal>("smpAccSurchargeForeign");
			eRPShipmentInformationDto.smpAdditionalWeight = dataTable.Rows[0].Field<decimal>("smpAdditionalWeight");
			eRPShipmentInformationDto.smpAESITN = dataTable.Rows[0].Field<string>("smpAESITN");
			eRPShipmentInformationDto.smpArInvoiceContactID = dataTable.Rows[0].Field<string>("smpArInvoiceContactID");
			eRPShipmentInformationDto.smpArInvoiceLocationID = dataTable.Rows[0].Field<string>("smpArInvoiceLocationID");
			eRPShipmentInformationDto.smpBlindShipContactID = dataTable.Rows[0].Field<string>("smpBlindShipContactID");
			eRPShipmentInformationDto.smpBlindShipLocationID = dataTable.Rows[0].Field<string>("smpBlindShipLocationID");
			eRPShipmentInformationDto.smpBlindShipOrganizationID = dataTable.Rows[0].Field<string>("smpBlindShipOrganizationID");
			eRPShipmentInformationDto.smpCarrierDocumentFilePath = dataTable.Rows[0].Field<string>("smpCarrierDocumentFilePath");
			eRPShipmentInformationDto.smpClosedDate = dataTable.Rows[0].Field<DateTime?>("smpClosedDate");
			eRPShipmentInformationDto.smpShipmentID = dataTable.Rows[0].Field<string>("smpShipmentID");
			eRPShipmentInformationDto.smpCodLabelFilePath = dataTable.Rows[0].Field<string>("smpCodLabelFilePath");
			eRPShipmentInformationDto.smpCreatedBy = dataTable.Rows[0].Field<string>("smpCreatedBy");
			eRPShipmentInformationDto.smpCreatedDate = dataTable.Rows[0].Field<DateTime?>("smpCreatedDate");
			eRPShipmentInformationDto.smpCurrencyRateID = dataTable.Rows[0].Field<string>("smpCurrencyRateID");
			eRPShipmentInformationDto.smpCustomerOrganizationID = dataTable.Rows[0].Field<string>("smpCustomerOrganizationID");
			eRPShipmentInformationDto.smpDocuments = dataTable.Rows[0].Field<string>("smpDocuments");
			eRPShipmentInformationDto.smpEdiTransferredDate = dataTable.Rows[0].Field<DateTime?>("smpEdiTransferredDate");
			eRPShipmentInformationDto.smpUniqueID = dataTable.Rows[0].Field<Guid>("smpUniqueID");
			eRPShipmentInformationDto.smpExchangeRate = dataTable.Rows[0].Field<decimal>("smpExchangeRate");
			eRPShipmentInformationDto.smpExportingCarrier = dataTable.Rows[0].Field<string>("smpExportingCarrier");
			eRPShipmentInformationDto.smpFedEx3rdPartyLocationID = dataTable.Rows[0].Field<string>("smpFedEx3rdPartyLocationID");
			eRPShipmentInformationDto.smpFedEx3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("smpFedEx3rdPartyOrganizationID");
			eRPShipmentInformationDto.smpFedExAccountNumber = dataTable.Rows[0].Field<string>("smpFedExAccountNumber");
			eRPShipmentInformationDto.smpFedExBillingOption = dataTable.Rows[0].Field<string>("smpFedExBillingOption");
			eRPShipmentInformationDto.smpFreightCharge = dataTable.Rows[0].Field<decimal>("smpFreightCharge");
			eRPShipmentInformationDto.smpFreightChargeForeign = dataTable.Rows[0].Field<decimal>("smpFreightChargeForeign");
			eRPShipmentInformationDto.smpFreightSubtotal = dataTable.Rows[0].Field<decimal>("smpFreightSubtotal");
			eRPShipmentInformationDto.smpFreightSubtotalForeign = dataTable.Rows[0].Field<decimal>("smpFreightSubtotalForeign");
			eRPShipmentInformationDto.smpFreightTotal = dataTable.Rows[0].Field<decimal>("smpFreightTotal");
			eRPShipmentInformationDto.smpFreightTotalForeign = dataTable.Rows[0].Field<decimal>("smpFreightTotalForeign");
			eRPShipmentInformationDto.smpClosed = dataTable.Rows[0].Field<bool>("smpClosed");
			eRPShipmentInformationDto.smpCustomRate = dataTable.Rows[0].Field<bool>("smpCustomRate");
			eRPShipmentInformationDto.smpEdiShipmentReady = dataTable.Rows[0].Field<bool>("smpEdiShipmentReady");
			eRPShipmentInformationDto.smpEdiTransferred = dataTable.Rows[0].Field<bool>("smpEdiTransferred");
			eRPShipmentInformationDto.smpPostedToGl = dataTable.Rows[0].Field<bool>("smpPostedToGl");
			eRPShipmentInformationDto.smpPrintLabels = dataTable.Rows[0].Field<bool>("smpPrintLabels");
			eRPShipmentInformationDto.smpPrintPackingSlip = dataTable.Rows[0].Field<bool>("smpPrintPackingSlip");
			eRPShipmentInformationDto.smpReversalEntry = dataTable.Rows[0].Field<bool>("smpReversalEntry");
			eRPShipmentInformationDto.smpReversed = dataTable.Rows[0].Field<bool>("smpReversed");
			eRPShipmentInformationDto.smpListBaseChargeBase = dataTable.Rows[0].Field<decimal>("smpListBaseChargeBase");
			eRPShipmentInformationDto.smpListBaseChargeForeign = dataTable.Rows[0].Field<decimal>("smpListBaseChargeForeign");
			eRPShipmentInformationDto.smpListCarrierFreightBase = dataTable.Rows[0].Field<decimal>("smpListCarrierFreightBase");
			eRPShipmentInformationDto.smpListCarrierFreightForeign = dataTable.Rows[0].Field<decimal>("smpListCarrierFreightForeign");
			eRPShipmentInformationDto.smpListDiscountBase = dataTable.Rows[0].Field<decimal>("smpListDiscountBase");
			eRPShipmentInformationDto.smpListDiscountForeign = dataTable.Rows[0].Field<decimal>("smpListDiscountForeign");
			eRPShipmentInformationDto.smpListSurchargeBase = dataTable.Rows[0].Field<decimal>("smpListSurchargeBase");
			eRPShipmentInformationDto.smpListSurchargeForeign = dataTable.Rows[0].Field<decimal>("smpListSurchargeForeign");
			eRPShipmentInformationDto.smpNumberOfLabels = dataTable.Rows[0].Field<short>("smpNumberOfLabels");
			eRPShipmentInformationDto.smpPlantDepartmentID = dataTable.Rows[0].Field<string>("smpPlantDepartmentID");
			eRPShipmentInformationDto.smpPlantID = dataTable.Rows[0].Field<string>("smpPlantID");
			eRPShipmentInformationDto.smpPostedDate = dataTable.Rows[0].Field<DateTime?>("smpPostedDate");
			eRPShipmentInformationDto.smpProjectID = dataTable.Rows[0].Field<string>("smpProjectID");
			eRPShipmentInformationDto.smpReasonForExport = dataTable.Rows[0].Field<string>("smpReasonForExport");
			eRPShipmentInformationDto.smpReturnInstructionsRTF = dataTable.Rows[0].Field<string>("smpReturnInstructionsRTF");
			eRPShipmentInformationDto.smpReturnInstructionsText = dataTable.Rows[0].Field<string>("smpReturnInstructionsText");
			eRPShipmentInformationDto.smpRowVersion = dataTable.Rows[0].Field<byte[]>("smpRowVersion");
			eRPShipmentInformationDto.smpShipContactID = dataTable.Rows[0].Field<string>("smpShipContactID");
			eRPShipmentInformationDto.smpShipDate = dataTable.Rows[0].Field<DateTime?>("smpShipDate");
			eRPShipmentInformationDto.smpShipLocationID = dataTable.Rows[0].Field<string>("smpShipLocationID");
			eRPShipmentInformationDto.smpShipmentIDNumber = dataTable.Rows[0].Field<string>("smpShipmentIDNumber");
			eRPShipmentInformationDto.smpShipmentSubtotal = dataTable.Rows[0].Field<decimal>("smpShipmentSubtotal");
			eRPShipmentInformationDto.smpShipmentSubtotalForeign = dataTable.Rows[0].Field<decimal>("smpShipmentSubtotalForeign");
			eRPShipmentInformationDto.smpShipmentTotal = dataTable.Rows[0].Field<decimal>("smpShipmentTotal");
			eRPShipmentInformationDto.smpShipmentTotalForeign = dataTable.Rows[0].Field<decimal>("smpShipmentTotalForeign");
			eRPShipmentInformationDto.smpShipOrganizationID = dataTable.Rows[0].Field<string>("smpShipOrganizationID");
			eRPShipmentInformationDto.smpShippingCommentsRTF = dataTable.Rows[0].Field<string>("smpShippingCommentsRTF");
			eRPShipmentInformationDto.smpShippingCommentsText = dataTable.Rows[0].Field<string>("smpShippingCommentsText");
			eRPShipmentInformationDto.smpShippingMethodID = dataTable.Rows[0].Field<string>("smpShippingMethodID");
			eRPShipmentInformationDto.smpShippingPaymentTypeID = dataTable.Rows[0].Field<string>("smpShippingPaymentTypeID");
			eRPShipmentInformationDto.smpStandardMessageID = dataTable.Rows[0].Field<string>("smpStandardMessageID");
			eRPShipmentInformationDto.smpTrackingNumber = dataTable.Rows[0].Field<string>("smpTrackingNumber");
			eRPShipmentInformationDto.smpUps3rdPartyLocationID = dataTable.Rows[0].Field<string>("smpUps3rdPartyLocationID");
			eRPShipmentInformationDto.smpUps3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("smpUps3rdPartyOrganizationID");
			eRPShipmentInformationDto.smpUpsAccountNumber = dataTable.Rows[0].Field<string>("smpUpsAccountNumber");
			eRPShipmentInformationDto.smpUpsBillingOption = dataTable.Rows[0].Field<string>("smpUpsBillingOption");
			eRPShipmentInformationDto.smpWeightSubtotal = dataTable.Rows[0].Field<decimal>("smpWeightSubtotal");
			eRPShipmentInformationDto.smpWeightTotal = dataTable.Rows[0].Field<decimal>("smpWeightTotal");
			eRPShipmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipment(ERPShipmentDto shipment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Shipments WHERE smpUniqueID = " + M1Util.ConvertToLinq(shipment.smpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["smpShipmentID"] = shipment.smpShipmentID.ToUpper();
				shipment.smpUniqueID = ((shipment.smpUniqueID == Guid.Empty) ? Guid.NewGuid() : shipment.smpUniqueID);
				dataRow["smpUniqueID"] = shipment.smpUniqueID;
				dataRow["smpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["smpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Shipment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipment.smpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Shipment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["smpRowVersion"], shipment.smpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Shipment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Shipment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["smpAccBaseChargeBase"] = shipment.smpAccBaseChargeBase;
			dataRow["smpAccBaseChargeForeign"] = shipment.smpAccBaseChargeForeign;
			dataRow["smpAccCarrierFreightBase"] = shipment.smpAccCarrierFreightBase;
			dataRow["smpAccCarrierFreightForeign"] = shipment.smpAccCarrierFreightForeign;
			dataRow["smpAccDiscountBase"] = shipment.smpAccDiscountBase;
			dataRow["smpAccDiscountForeign"] = shipment.smpAccDiscountForeign;
			dataRow["smpAccSurchargeBase"] = shipment.smpAccSurchargeBase;
			dataRow["smpAccSurchargeForeign"] = shipment.smpAccSurchargeForeign;
			dataRow["smpAdditionalWeight"] = shipment.smpAdditionalWeight;
			dataRow["smpAESITN"] = shipment.smpAESITN;
			dataRow["smpArInvoiceContactID"] = shipment.smpArInvoiceContactID;
			dataRow["smpArInvoiceLocationID"] = shipment.smpArInvoiceLocationID;
			dataRow["smpBlindShipContactID"] = shipment.smpBlindShipContactID;
			dataRow["smpBlindShipLocationID"] = shipment.smpBlindShipLocationID;
			dataRow["smpBlindShipOrganizationID"] = shipment.smpBlindShipOrganizationID;
			dataRow["smpCarrierDocumentFilePath"] = shipment.smpCarrierDocumentFilePath ?? dataRow["smpCarrierDocumentFilePath"];
			DataRow dataRow2 = dataRow;
			DateTime? smpClosedDate = shipment.smpClosedDate;
			dataRow2["smpClosedDate"] = (smpClosedDate.HasValue ? ((object)smpClosedDate.GetValueOrDefault()) : dataRow["smpClosedDate"]);
			dataRow["smpCodLabelFilePath"] = shipment.smpCodLabelFilePath ?? dataRow["smpCodLabelFilePath"];
			dataRow["smpCurrencyRateID"] = shipment.smpCurrencyRateID;
			dataRow["smpCustomerOrganizationID"] = shipment.smpCustomerOrganizationID;
			dataRow["smpDocuments"] = shipment.smpDocuments ?? dataRow["smpDocuments"];
			DataRow dataRow3 = dataRow;
			smpClosedDate = shipment.smpEdiTransferredDate;
			dataRow3["smpEdiTransferredDate"] = (smpClosedDate.HasValue ? ((object)smpClosedDate.GetValueOrDefault()) : dataRow["smpEdiTransferredDate"]);
			dataRow["smpExchangeRate"] = shipment.smpExchangeRate;
			dataRow["smpExportingCarrier"] = shipment.smpExportingCarrier;
			dataRow["smpFedEx3rdPartyLocationID"] = shipment.smpFedEx3rdPartyLocationID;
			dataRow["smpFedEx3rdPartyOrganizationID"] = shipment.smpFedEx3rdPartyOrganizationID;
			dataRow["smpFedExAccountNumber"] = shipment.smpFedExAccountNumber;
			dataRow["smpFedExBillingOption"] = shipment.smpFedExBillingOption;
			dataRow["smpFreightCharge"] = shipment.smpFreightCharge;
			dataRow["smpFreightChargeForeign"] = shipment.smpFreightChargeForeign;
			dataRow["smpFreightSubtotal"] = shipment.smpFreightSubtotal;
			dataRow["smpFreightSubtotalForeign"] = shipment.smpFreightSubtotalForeign;
			dataRow["smpFreightTotal"] = shipment.smpFreightTotal;
			dataRow["smpFreightTotalForeign"] = shipment.smpFreightTotalForeign;
			dataRow["smpClosed"] = shipment.smpClosed;
			dataRow["smpCustomRate"] = shipment.smpCustomRate;
			dataRow["smpEdiShipmentReady"] = shipment.smpEdiShipmentReady;
			dataRow["smpEdiTransferred"] = shipment.smpEdiTransferred;
			dataRow["smpPostedToGl"] = shipment.smpPostedToGl;
			dataRow["smpPrintLabels"] = shipment.smpPrintLabels;
			dataRow["smpPrintPackingSlip"] = shipment.smpPrintPackingSlip;
			dataRow["smpReversalEntry"] = shipment.smpReversalEntry;
			dataRow["smpReversed"] = shipment.smpReversed;
			dataRow["smpListBaseChargeBase"] = shipment.smpListBaseChargeBase;
			dataRow["smpListBaseChargeForeign"] = shipment.smpListBaseChargeForeign;
			dataRow["smpListCarrierFreightBase"] = shipment.smpListCarrierFreightBase;
			dataRow["smpListCarrierFreightForeign"] = shipment.smpListCarrierFreightForeign;
			dataRow["smpListDiscountBase"] = shipment.smpListDiscountBase;
			dataRow["smpListDiscountForeign"] = shipment.smpListDiscountForeign;
			dataRow["smpListSurchargeBase"] = shipment.smpListSurchargeBase;
			dataRow["smpListSurchargeForeign"] = shipment.smpListSurchargeForeign;
			dataRow["smpNumberOfLabels"] = shipment.smpNumberOfLabels;
			dataRow["smpPlantDepartmentID"] = shipment.smpPlantDepartmentID;
			dataRow["smpPlantID"] = shipment.smpPlantID;
			DataRow dataRow4 = dataRow;
			smpClosedDate = shipment.smpPostedDate;
			dataRow4["smpPostedDate"] = (smpClosedDate.HasValue ? ((object)smpClosedDate.GetValueOrDefault()) : dataRow["smpPostedDate"]);
			dataRow["smpProjectID"] = shipment.smpProjectID;
			dataRow["smpReasonForExport"] = shipment.smpReasonForExport;
			dataRow["smpReturnInstructionsRTF"] = shipment.smpReturnInstructionsRTF ?? dataRow["smpReturnInstructionsRTF"];
			dataRow["smpReturnInstructionsText"] = shipment.smpReturnInstructionsText ?? dataRow["smpReturnInstructionsText"];
			dataRow["smpShipContactID"] = shipment.smpShipContactID;
			DataRow dataRow5 = dataRow;
			smpClosedDate = shipment.smpShipDate;
			dataRow5["smpShipDate"] = (smpClosedDate.HasValue ? ((object)smpClosedDate.GetValueOrDefault()) : dataRow["smpShipDate"]);
			dataRow["smpShipLocationID"] = shipment.smpShipLocationID;
			dataRow["smpShipmentIDNumber"] = shipment.smpShipmentIDNumber;
			dataRow["smpShipmentSubtotal"] = shipment.smpShipmentSubtotal;
			dataRow["smpShipmentSubtotalForeign"] = shipment.smpShipmentSubtotalForeign;
			dataRow["smpShipmentTotal"] = shipment.smpShipmentTotal;
			dataRow["smpShipmentTotalForeign"] = shipment.smpShipmentTotalForeign;
			dataRow["smpShipOrganizationID"] = shipment.smpShipOrganizationID;
			dataRow["smpShippingCommentsRTF"] = shipment.smpShippingCommentsRTF ?? dataRow["smpShippingCommentsRTF"];
			dataRow["smpShippingCommentsText"] = shipment.smpShippingCommentsText ?? dataRow["smpShippingCommentsText"];
			dataRow["smpShippingMethodID"] = shipment.smpShippingMethodID;
			dataRow["smpShippingPaymentTypeID"] = shipment.smpShippingPaymentTypeID;
			dataRow["smpStandardMessageID"] = shipment.smpStandardMessageID;
			dataRow["smpTrackingNumber"] = shipment.smpTrackingNumber;
			dataRow["smpUps3rdPartyLocationID"] = shipment.smpUps3rdPartyLocationID;
			dataRow["smpUps3rdPartyOrganizationID"] = shipment.smpUps3rdPartyOrganizationID;
			dataRow["smpUpsAccountNumber"] = shipment.smpUpsAccountNumber;
			dataRow["smpUpsBillingOption"] = shipment.smpUpsBillingOption;
			dataRow["smpWeightSubtotal"] = shipment.smpWeightSubtotal;
			dataRow["smpWeightTotal"] = shipment.smpWeightTotal;
			if (shipment.CustomFields != null && shipment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipment.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Shipment [{shipment.smpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Shipment [{shipment.smpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
