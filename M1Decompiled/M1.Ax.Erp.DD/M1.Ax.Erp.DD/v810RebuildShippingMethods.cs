using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert ShippingMethods to support unicode", "2013-10-17")]
public class v810RebuildShippingMethods
{
	public v810RebuildShippingMethods(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingMethods", new DmoField[65]
		{
			new DmoField("xasShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xasDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("xasCarrier", "nvarchar", 5, 0, nullable: false),
			new DmoField("xasCarrierAccountNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasTrackingLink", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xasReferenceTrackingLink", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("xasShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xasCanSelectFromWeb", "bit", 1, 0, nullable: false),
			new DmoField("xasShipChargeWeb", "money", 12, 2, nullable: false),
			new DmoField("xasInactive", "bit", 1, 0, nullable: false),
			new DmoField("xasInactiveDate", "date", 14, 0, nullable: true),
			new DmoField("xasUPSUseInterface", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSSaturdayDelivery", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSServiceType", "nvarchar", 22, 0, nullable: false),
			new DmoField("xasUPSPackageType", "nvarchar", 35, 0, nullable: false),
			new DmoField("xasUPSBillingOptionDefault", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasDistributeCostsOption", "tinyint", 2, 0, nullable: false),
			new DmoField("xasFdxService", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasFdxDropOffType", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasFdxReturnShipIndicator", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasFdxSignatureOption", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasFdxSaturdayPickup", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxInsidePickup", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxSaturdayDelivery", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxInsideDelivery", "bit", 1, 0, nullable: false),
			new DmoField("xasTaxStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("xasTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xasSecondTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("xasAvalaraTaxCodeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("xasCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("xasUniqueID", "uniqueidentifier", 16, 0, nullable: false),
			new DmoField("xasFdxCOD", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxNonStandardContainer", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxHoldAtLocation", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxCODCollectionType", "nvarchar", 16, 0, nullable: false),
			new DmoField("xasFdxVHCLevel", "nvarchar", 8, 0, nullable: false),
			new DmoField("xasFdxVHCType", "nvarchar", 40, 0, nullable: false),
			new DmoField("xasFdxVHCAmountOrPercentage", "numeric", 10, 2, nullable: false),
			new DmoField("xasFdxRateRequestType", "nvarchar", 7, 0, nullable: false),
			new DmoField("xasFdxHomeDeliveryType", "nvarchar", 11, 0, nullable: false),
			new DmoField("xasFdxAccessibility", "nvarchar", 12, 0, nullable: false),
			new DmoField("xasFdxRateTypeBasis", "nvarchar", 10, 0, nullable: false),
			new DmoField("xasFdxRateElementBasis", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasFdxPackageType", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasFdxWebRateRequestType", "nvarchar", 10, 0, nullable: false),
			new DmoField("xasUseReceiverDefaults", "bit", 1, 0, nullable: false),
			new DmoField("xasUSPSEndorsement", "nvarchar", 1, 0, nullable: false),
			new DmoField("xasUPSCostCenter", "nvarchar", 30, 0, nullable: false),
			new DmoField("xasUPSCOD", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSCODFundsCode", "nvarchar", 1, 0, nullable: false),
			new DmoField("xasUPSCommercialInvoice", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSCertificateOfOrigin", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSNAFTACO", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSPartialInvoice", "bit", 1, 0, nullable: false),
			new DmoField("xasUPSPackingList", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxCertificateOfOrigin", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxCommercialInvoice", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxExportDeclaration", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxNAFTACO", "bit", 1, 0, nullable: false),
			new DmoField("xasFdxReturnInstructions", "bit", 1, 0, nullable: false),
			new DmoField("xasFedExBillingOption", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasUPSWSBillingOption", "nvarchar", 20, 0, nullable: false),
			new DmoField("xasUPSWSServiceType", "nvarchar", 22, 0, nullable: false),
			new DmoField("xasUPSWSPackageType", "nvarchar", 35, 0, nullable: false)
		}, new DmoIndex[4]
		{
			new DmoIndex("XASSHIPPINGMETHODID", unique: true),
			new DmoIndex("XASUNIQUEID", unique: true),
			new DmoIndex("xasShippingPaymentTypeID", unique: false),
			new DmoIndex("xasInactive", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
