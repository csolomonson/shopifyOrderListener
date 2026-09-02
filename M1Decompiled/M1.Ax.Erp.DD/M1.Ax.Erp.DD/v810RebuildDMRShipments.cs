using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert DMRShipments to support unicode", "2013-10-17")]
public class v810RebuildDMRShipments
{
	public v810RebuildDMRShipments(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "DMRShipments", new DmoField[30]
		{
			new DmoField("dspDMRShipmentID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dspPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dspShipDate", "datetime", 14, 0, nullable: true),
			new DmoField("dspShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspNumberOfLabels", "smallint", 3, 0, nullable: false),
			new DmoField("dspFreightSubtotal", "money", 12, 2, nullable: false),
			new DmoField("dspFreightCharge", "money", 12, 2, nullable: false),
			new DmoField("dspFreightTotal", "money", 12, 2, nullable: false),
			new DmoField("dspStandardMessageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dspShippingCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dspShippingCommentsText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("dspPrintDMRPackingSlip", "bit", 1, 0, nullable: false),
			new DmoField("dspPrintLabels", "bit", 1, 0, nullable: false),
			new DmoField("dspTrackingNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("dspProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("dspClosed", "bit", 1, 0, nullable: false),
			new DmoField("dspClosedDate", "date", 14, 0, nullable: true),
			new DmoField("dspAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspFreightChargeForeign", "money", 12, 2, nullable: false),
			new DmoField("dspCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("dspExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("dspCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("dspCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("dspCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("dspUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[11]
		{
			new DmoIndex("DSPDMRSHIPMENTID", unique: true),
			new DmoIndex("DSPUNIQUEID", unique: true),
			new DmoIndex("dspPlantDepartmentID", unique: false),
			new DmoIndex("dspPlantID", unique: false),
			new DmoIndex("dspSupplierOrganizationID", unique: false),
			new DmoIndex("dspShipDate", unique: false),
			new DmoIndex("dspPrintDMRPackingSlip", unique: false),
			new DmoIndex("dspPrintLabels", unique: false),
			new DmoIndex("dspProjectID", unique: false),
			new DmoIndex("dspClosed", unique: false),
			new DmoIndex("dspAPInvoiceLocationID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
