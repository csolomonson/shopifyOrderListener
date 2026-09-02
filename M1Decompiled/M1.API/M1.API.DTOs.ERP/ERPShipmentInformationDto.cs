using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShipmentInformationDto
{
	public decimal smpAccBaseChargeBase { get; set; }

	public decimal smpAccBaseChargeForeign { get; set; }

	public decimal smpAccCarrierFreightBase { get; set; }

	public decimal smpAccCarrierFreightForeign { get; set; }

	public decimal smpAccDiscountBase { get; set; }

	public decimal smpAccDiscountForeign { get; set; }

	public decimal smpAccSurchargeBase { get; set; }

	public decimal smpAccSurchargeForeign { get; set; }

	public decimal smpAdditionalWeight { get; set; }

	public string smpAESITN { get; set; }

	public string smpArInvoiceContactID { get; set; }

	public string smpArInvoiceLocationID { get; set; }

	public string smpBlindShipContactID { get; set; }

	public string smpBlindShipLocationID { get; set; }

	public string smpBlindShipOrganizationID { get; set; }

	public string smpCarrierDocumentFilePath { get; set; }

	public DateTime? smpClosedDate { get; set; }

	public string smpShipmentID { get; set; }

	public string smpCodLabelFilePath { get; set; }

	public string smpCreatedBy { get; set; }

	public DateTime? smpCreatedDate { get; set; }

	public string smpCurrencyRateID { get; set; }

	public string smpCustomerOrganizationID { get; set; }

	public string smpDocuments { get; set; }

	public DateTime? smpEdiTransferredDate { get; set; }

	public Guid smpUniqueID { get; set; }

	public decimal smpExchangeRate { get; set; }

	public string smpExportingCarrier { get; set; }

	public string smpFedEx3rdPartyLocationID { get; set; }

	public string smpFedEx3rdPartyOrganizationID { get; set; }

	public string smpFedExAccountNumber { get; set; }

	public string smpFedExBillingOption { get; set; }

	public decimal smpFreightCharge { get; set; }

	public decimal smpFreightChargeForeign { get; set; }

	public decimal smpFreightSubtotal { get; set; }

	public decimal smpFreightSubtotalForeign { get; set; }

	public decimal smpFreightTotal { get; set; }

	public decimal smpFreightTotalForeign { get; set; }

	public bool smpClosed { get; set; }

	public bool smpCustomRate { get; set; }

	public bool smpEdiShipmentReady { get; set; }

	public bool smpEdiTransferred { get; set; }

	public bool smpPostedToGl { get; set; }

	public bool smpPrintLabels { get; set; }

	public bool smpPrintPackingSlip { get; set; }

	public bool smpReversalEntry { get; set; }

	public bool smpReversed { get; set; }

	public decimal smpListBaseChargeBase { get; set; }

	public decimal smpListBaseChargeForeign { get; set; }

	public decimal smpListCarrierFreightBase { get; set; }

	public decimal smpListCarrierFreightForeign { get; set; }

	public decimal smpListDiscountBase { get; set; }

	public decimal smpListDiscountForeign { get; set; }

	public decimal smpListSurchargeBase { get; set; }

	public decimal smpListSurchargeForeign { get; set; }

	public short smpNumberOfLabels { get; set; }

	public string smpPlantDepartmentID { get; set; }

	public string smpPlantID { get; set; }

	public DateTime? smpPostedDate { get; set; }

	public string smpProjectID { get; set; }

	public string smpReasonForExport { get; set; }

	public string smpReturnInstructionsRTF { get; set; }

	public string smpReturnInstructionsText { get; set; }

	public byte[] smpRowVersion { get; set; }

	public string smpShipContactID { get; set; }

	public DateTime? smpShipDate { get; set; }

	public string smpShipLocationID { get; set; }

	public string smpShipmentIDNumber { get; set; }

	public decimal smpShipmentSubtotal { get; set; }

	public decimal smpShipmentSubtotalForeign { get; set; }

	public decimal smpShipmentTotal { get; set; }

	public decimal smpShipmentTotalForeign { get; set; }

	public string smpShipOrganizationID { get; set; }

	public string smpShippingCommentsRTF { get; set; }

	public string smpShippingCommentsText { get; set; }

	public string smpShippingMethodID { get; set; }

	public string smpShippingPaymentTypeID { get; set; }

	public string smpStandardMessageID { get; set; }

	public string smpTrackingNumber { get; set; }

	public string smpUps3rdPartyLocationID { get; set; }

	public string smpUps3rdPartyOrganizationID { get; set; }

	public string smpUpsAccountNumber { get; set; }

	public string smpUpsBillingOption { get; set; }

	public decimal smpWeightSubtotal { get; set; }

	public decimal smpWeightTotal { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
