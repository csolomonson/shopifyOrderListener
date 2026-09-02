using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert FreightShipments to support unicode", "2013-10-17")]
public class v810RebuildFreightShipments
{
	public v810RebuildFreightShipments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FreightShipments", new DmoField[61]
		{
			new DmoField("fspFreightShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspUPSServiceType", "nvarchar", 22, 0, nullable: false),
			new DmoField("fspUPSSaturdayDelivery", "bit", 1, 0, nullable: false),
			new DmoField("fspUPSBillingOption", "nvarchar", 20, 0, nullable: false),
			new DmoField("fspUPSBillAcctNumber", "nvarchar", 6, 0, nullable: false),
			new DmoField("fspTotalCharges", "numeric", 9, 2, nullable: false),
			new DmoField("fspTotalPublishedCharges", "numeric", 9, 2, nullable: false),
			new DmoField("fspUPSInterfaceStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("fspNotesRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fspNotesText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fspCarrier", "nvarchar", 5, 0, nullable: false),
			new DmoField("fspShipperAcctNumber", "nvarchar", 20, 0, nullable: false),
			new DmoField("fspShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fspShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fspFreightShipmentDate", "datetime", 14, 0, nullable: true),
			new DmoField("fspVoidOnUPS", "bit", 1, 0, nullable: false),
			new DmoField("fspDistributeCostsOption", "tinyint", 2, 0, nullable: false),
			new DmoField("fspFdxService", "nvarchar", 30, 0, nullable: false),
			new DmoField("fspFdxStatus", "tinyint", 1, 0, nullable: false),
			new DmoField("fspFdxStatusText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fspFdxPayorAccountNumber", "nvarchar", 12, 0, nullable: false),
			new DmoField("fspFdxPayorCountryCode", "nvarchar", 2, 0, nullable: false),
			new DmoField("fspFdxReturnShipIndicator", "nvarchar", 30, 0, nullable: false),
			new DmoField("fspFdxDropOffType", "nvarchar", 30, 0, nullable: false),
			new DmoField("fspFdxSignatureOption", "nvarchar", 30, 0, nullable: false),
			new DmoField("fspFdxSignatureReleaseAuthNum", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspDeclaredValue", "numeric", 10, 2, nullable: false),
			new DmoField("fspFdxHandlingCost", "numeric", 7, 2, nullable: false),
			new DmoField("fspFdxPackagingCost", "numeric", 7, 2, nullable: false),
			new DmoField("fspFdxShipCostMarkupPct", "numeric", 5, 2, nullable: false),
			new DmoField("fspFdxOneItemPerShipment", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxInsidePickup", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxInsideDelivery", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxSaturdayPickup", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxSaturdayDelivery", "bit", 1, 0, nullable: false),
			new DmoField("fspShipFromOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspFdxCODCollectionType", "nvarchar", 16, 0, nullable: false),
			new DmoField("fspFdxCOD", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxHoldAtLocation", "bit", 1, 0, nullable: false),
			new DmoField("fspFdxVHCLevel", "nvarchar", 8, 0, nullable: false),
			new DmoField("fspFdxVHCType", "nvarchar", 40, 0, nullable: false),
			new DmoField("fspFdxVHCAmountOrPercentage", "numeric", 9, 2, nullable: false),
			new DmoField("fspFdxHomeDeliveryType", "nvarchar", 11, 0, nullable: false),
			new DmoField("fspFdxRateRequestType", "nvarchar", 7, 0, nullable: false),
			new DmoField("fspFdxAccessibility", "nvarchar", 12, 0, nullable: false),
			new DmoField("fspFdxCODCollectionAmount", "numeric", 12, 2, nullable: false),
			new DmoField("fspUPS3rdPartyOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspUPS3rdPartyLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("fspFdxPayorType", "nvarchar", 10, 0, nullable: false),
			new DmoField("fspFdxLastLogID", "int", 4, 0, nullable: false),
			new DmoField("fspFdxLastRequestDate", "datetime", 14, 0, nullable: true),
			new DmoField("fspFdxLastReplyErrorCode", "nvarchar", 8, 0, nullable: false),
			new DmoField("fspFdxLastReplyErrorMessage", "nvarchar", 120, 0, nullable: false),
			new DmoField("fspFdxLastReplySoftErrorCode", "nvarchar", 8, 0, nullable: false),
			new DmoField("fspFdxLastReplySoftErrorType", "nvarchar", 25, 0, nullable: false),
			new DmoField("fspFdxLastReplySoftErrorMsg", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("fspCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("fspCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("fspFdxLastUTI", "nvarchar", 4, 0, nullable: false),
			new DmoField("fspUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[8]
		{
			new DmoIndex("FSPFREIGHTSHIPMENTID", unique: true),
			new DmoIndex("FSPUNIQUEID", unique: true),
			new DmoIndex("fspUPSInterfaceStatus", unique: false),
			new DmoIndex("fspShipOrganizationID", unique: false),
			new DmoIndex("fspShipLocationID", unique: false),
			new DmoIndex("fspShippingMethodID", unique: false),
			new DmoIndex("fspFreightShipmentDate", unique: false),
			new DmoIndex("fspFdxLastLogID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
