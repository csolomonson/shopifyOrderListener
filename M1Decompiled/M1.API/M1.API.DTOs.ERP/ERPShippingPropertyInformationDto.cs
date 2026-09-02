using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShippingPropertyInformationDto
{
	public string xsmCreatedBy { get; set; }

	public DateTime? xsmCreatedDate { get; set; }

	public Guid xsmUniqueID { get; set; }

	public string xsmFdxAccessibility { get; set; }

	public string xsmFdxAccountNumber { get; set; }

	public string xsmFdxAccountNumberOAuth { get; set; }

	public string xsmFdxAddressLine1 { get; set; }

	public string xsmFdxAddressLine2 { get; set; }

	public string xsmFdxAddrValAccuracyIndicator { get; set; }

	public string xsmFdxCity { get; set; }

	public string xsmFdxClientID { get; set; }

	public string xsmFdxClientIDTrack { get; set; }

	public string xsmFdxClientSecret { get; set; }

	public string xsmFdxClientSecretTrack { get; set; }

	public decimal xsmFdxCodCollectionAmount { get; set; }

	public string xsmFdxCodCollectionType { get; set; }

	public string xsmFdxCountry { get; set; }

	public string xsmFdxCurrencyType { get; set; }

	public string xsmFdxDeclaredValueCurrency { get; set; }

	public string xsmFdxDepartment { get; set; }

	public string xsmFdxDimensionsUnitOfMeasure { get; set; }

	public string xsmFdxDropOffType { get; set; }

	public string xsmFdxEmailAddress { get; set; }

	public string xsmFdxFaxNumber { get; set; }

	public decimal xsmFdxHandlingCost { get; set; }

	public DateTime? xsmFdxHomeDeliveryDate { get; set; }

	public string xsmFdxHomeDeliveryType { get; set; }

	public string xsmFdxHostAddress { get; set; }

	public int xsmFdxHostPort { get; set; }

	public string xsmFdxHostService { get; set; }

	public string xsmFdxLabelFormatType { get; set; }

	public string xsmFdxLabelImageType { get; set; }

	public string xsmFdxLabelStockType { get; set; }

	public string xsmFdxLabelStoreLocation { get; set; }

	public string xsmFdxLabelType { get; set; }

	public string xsmFdxLblPrintOrientType { get; set; }

	public decimal xsmFdxMeterNumber { get; set; }

	public string xsmFdxName { get; set; }

	public int xsmFdxPackageHeight { get; set; }

	public int xsmFdxPackageLength { get; set; }

	public int xsmFdxPackageWidth { get; set; }

	public string xsmFdxPackaging { get; set; }

	public decimal xsmFdxPackagingCost { get; set; }

	public string xsmFdxPagerNumber { get; set; }

	public string xsmFdxPayorType { get; set; }

	public string xsmFdxPersonName { get; set; }

	public string xsmFdxPhoneNumber { get; set; }

	public string xsmFdxPostCode { get; set; }

	public string xsmFdxRateElementBasis { get; set; }

	public string xsmFdxRateRequestType { get; set; }

	public string xsmFdxRateTypeBasis { get; set; }

	public string xsmFdxReturnShipIndicator { get; set; }

	public decimal xsmFdxShipCostMarkupPct { get; set; }

	public string xsmFdxShipDocImageType { get; set; }

	public string xsmFdxSignatureOption { get; set; }

	public string xsmFdxState { get; set; }

	public string xsmFdxSubscribedServices { get; set; }

	public decimal xsmFdxVHCAmountOrPercentage { get; set; }

	public string xsmFdxVHCLevel { get; set; }

	public string xsmFdxVHCType { get; set; }

	public string xsmFdxWeightUnitOfMeasure { get; set; }

	public string xsmFedExAccessKey { get; set; }

	public string xsmFedExAccessToken { get; set; }

	public string xsmFedExAccessTokenTrack { get; set; }

	public string xsmFedExAuthenticationMethod { get; set; }

	public string xsmFedExPassword { get; set; }

	public DateTime? xsmFedExTokenExpiresIn { get; set; }

	public DateTime? xsmFedExTokenExpiresInTrack { get; set; }

	public string xsmFedExUserName { get; set; }

	public bool xsmFdxBareCostOfDuty { get; set; }

	public bool xsmFdxBareTrasportationCost { get; set; }

	public bool xsmFdxCod { get; set; }

	public bool xsmFdxHoldAtLocation { get; set; }

	public bool xsmFdxInsideDelivery { get; set; }

	public bool xsmFdxInsidePickup { get; set; }

	public bool xsmFdxNonstandardContainer { get; set; }

	public bool xsmFdxOneItemPerShipment { get; set; }

	public bool xsmFdxResidentialAddress { get; set; }

	public bool xsmFdxSaturdayDelivery { get; set; }

	public bool xsmFdxSaturdayPickup { get; set; }

	public bool xsmFedExIsProduction { get; set; }

	public bool xsmUpsIsProduction { get; set; }

	public byte[] xsmRowVersion { get; set; }

	public string xsmUpsAccessKey { get; set; }

	public string xsmUpsAccessToken { get; set; }

	public string xsmUpsAccountNo { get; set; }

	public string xsmUpsAccountNoOAuth { get; set; }

	public string xsmUpsAuthenticationMethod { get; set; }

	public string xsmUpsLabelStockSize { get; set; }

	public string xsmUpsLabelStoreLocation { get; set; }

	public string xsmUpsLabelType { get; set; }

	public string xsmUpsLocIDPref { get; set; }

	public string xsmUpsLocPostCodePref { get; set; }

	public string xsmUpsPassword { get; set; }

	public string xsmUpsRefreshToken { get; set; }

	public string xsmUpsUsername { get; set; }

	public string xsmUSDcurrencyCode { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
