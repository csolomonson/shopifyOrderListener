using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShippingMethodInformationDto
{
	public string xasAvalaraTaxCodeID { get; set; }

	public string xasCarrier { get; set; }

	public string xasCarrierAccountNumber { get; set; }

	public string xasShippingMethodID { get; set; }

	public string xasCreatedBy { get; set; }

	public DateTime? xasCreatedDate { get; set; }

	public string xasDescription { get; set; }

	public byte xasDistributeCostsOption { get; set; }

	public Guid xasUniqueID { get; set; }

	public string xasFdxAccessibility { get; set; }

	public string xasFdxCodCollectionType { get; set; }

	public string xasFdxDropOffType { get; set; }

	public string xasFdxHomeDeliveryType { get; set; }

	public string xasFdxPackageType { get; set; }

	public string xasFdxRateElementBasis { get; set; }

	public string xasFdxRateRequestType { get; set; }

	public string xasFdxRateTypeBasis { get; set; }

	public string xasFdxReturnShipIndicator { get; set; }

	public string xasFdxService { get; set; }

	public string xasFdxSignatureOption { get; set; }

	public decimal xasFdxVHCAmountOrPercentage { get; set; }

	public string xasFdxVHCLevel { get; set; }

	public string xasFdxVHCType { get; set; }

	public string xasFedExBillingOption { get; set; }

	public DateTime? xasInactiveDate { get; set; }

	public bool xasInactive { get; set; }

	public bool xasFdxCertificateOfOrigin { get; set; }

	public bool xasFdxCod { get; set; }

	public bool xasFdxCommercialInvoice { get; set; }

	public bool xasFdxExportDeclaration { get; set; }

	public bool xasFdxHoldAtLocation { get; set; }

	public bool xasFdxInsideDelivery { get; set; }

	public bool xasFdxInsidePickup { get; set; }

	public bool xasFdxNAFTACO { get; set; }

	public bool xasFdxNonStandardContainer { get; set; }

	public bool xasFdxReturnInstructions { get; set; }

	public bool xasFdxSaturdayDelivery { get; set; }

	public bool xasFdxSaturdayPickup { get; set; }

	public bool xasUpsCertificateOfOrigin { get; set; }

	public bool xasUpsCod { get; set; }

	public bool xasUpsCommercialInvoice { get; set; }

	public bool xasUpsNAFTACO { get; set; }

	public bool xasUpsPackingList { get; set; }

	public bool xasUpsPartialInvoice { get; set; }

	public bool xasUpsSaturdayDelivery { get; set; }

	public bool xasUpsUseInterface { get; set; }

	public string xasReferenceTrackingLink { get; set; }

	public byte[] xasRowVersion { get; set; }

	public string xasSecondTaxCodeID { get; set; }

	public decimal xasShipChargeWeb { get; set; }

	public string xasShippingPaymentTypeID { get; set; }

	public string xasTaxCodeID { get; set; }

	public byte xasTaxStatus { get; set; }

	public string xasTrackingLink { get; set; }

	public string xasUpsBillingOptionDefault { get; set; }

	public string xasUpsCodFundsCode { get; set; }

	public string xasUpsCostCenter { get; set; }

	public string xasUpsPackageType { get; set; }

	public string xasUpsServiceType { get; set; }

	public string xasUpsWsBillingOption { get; set; }

	public string xasUpsWSPackageType { get; set; }

	public string xasUpsWSServiceType { get; set; }

	public string xasUSPSEndorsement { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
