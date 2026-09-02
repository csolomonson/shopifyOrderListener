using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFreightShipmentInformationDto
{
	public string fspCarrier { get; set; }

	public string fspFreightShipmentID { get; set; }

	public string fspCreatedBy { get; set; }

	public DateTime? fspCreatedDate { get; set; }

	public decimal fspDeclaredValue { get; set; }

	public byte fspDistributeCostsOption { get; set; }

	public Guid fspUniqueID { get; set; }

	public string fspFdxAccessibility { get; set; }

	public decimal fspFdxCodCollectionAmount { get; set; }

	public string fspFdxCodCollectionType { get; set; }

	public string fspFdxDropOffType { get; set; }

	public decimal fspFdxHandlingCost { get; set; }

	public string fspFdxHomeDeliveryType { get; set; }

	public int fspFdxLastLogID { get; set; }

	public string fspFdxLastReplyErrorCode { get; set; }

	public string fspFdxLastReplyErrorMessage { get; set; }

	public string fspFdxLastReplySoftErrorCode { get; set; }

	public string fspFdxLastReplySoftErrorMsg { get; set; }

	public string fspFdxLastReplySoftErrorType { get; set; }

	public DateTime? fspFdxLastRequestDate { get; set; }

	public string fspFdxLastUTI { get; set; }

	public decimal fspFdxPackagingCost { get; set; }

	public string fspFdxPayorAccountNumber { get; set; }

	public string fspFdxPayorCountryCode { get; set; }

	public string fspFdxPayorType { get; set; }

	public string fspFdxRateRequestType { get; set; }

	public string fspFdxReturnShipIndicator { get; set; }

	public string fspFdxService { get; set; }

	public decimal fspFdxShipCostMarkupPct { get; set; }

	public string fspFdxSignatureOption { get; set; }

	public string fspFdxSignatureReleaseAuthNum { get; set; }

	public byte fspFdxStatus { get; set; }

	public string fspFdxStatusText { get; set; }

	public decimal fspFdxVHCAmountOrPercentage { get; set; }

	public string fspFdxVHCLevel { get; set; }

	public string fspFdxVHCType { get; set; }

	public DateTime? fspFreightShipmentDate { get; set; }

	public bool fspFdxCod { get; set; }

	public bool fspFdxHoldAtLocation { get; set; }

	public bool fspFdxInsideDelivery { get; set; }

	public bool fspFdxInsidePickup { get; set; }

	public bool fspFdxOneItemPerShipment { get; set; }

	public bool fspFdxSaturdayDelivery { get; set; }

	public bool fspFdxSaturdayPickup { get; set; }

	public bool fspUpsSaturdayDelivery { get; set; }

	public bool fspVoidOnUps { get; set; }

	public string fspNotesRTF { get; set; }

	public string fspNotesText { get; set; }

	public byte[] fspRowVersion { get; set; }

	public string fspShipFromOrganizationID { get; set; }

	public string fspShipLocationID { get; set; }

	public string fspShipOrganizationID { get; set; }

	public string fspShipperAcctNumber { get; set; }

	public string fspShippingMethodID { get; set; }

	public decimal fspTotalCharges { get; set; }

	public decimal fspTotalPublishedCharges { get; set; }

	public string fspUps3rdPartyLocationID { get; set; }

	public string fspUps3rdPartyOrganizationID { get; set; }

	public string fspUpsBillAcctNumber { get; set; }

	public string fspUpsBillingOption { get; set; }

	public byte fspUpsInterfaceStatus { get; set; }

	public string fspUpsServiceType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
