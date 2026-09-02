using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert POSTransactions to support unicode", "2013-10-17")]
public class v810RebuildPOSTransactions
{
	public v810RebuildPOSTransactions(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "POSTransactions", new DmoField[36]
		{
			new DmoField("pspPOSTransactionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pspPOSSessionID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pspSaleDate", "date", 14, 0, nullable: true),
			new DmoField("pspGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("pspGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("pspCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pspARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pspShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspFreightTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspFreightTaxAmount", "money", 14, 4, nullable: false),
			new DmoField("pspShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspSecondFreightTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("pspSecondFreightTaxAmt", "money", 14, 4, nullable: false),
			new DmoField("pspFreightAmount", "money", 12, 2, nullable: false),
			new DmoField("pspFreightSubtotal", "money", 12, 2, nullable: false),
			new DmoField("pspFreightTotal", "money", 12, 2, nullable: false),
			new DmoField("pspSubtotal", "money", 12, 2, nullable: false),
			new DmoField("pspDiscountTotal", "money", 12, 2, nullable: false),
			new DmoField("pspTaxAmount", "money", 12, 2, nullable: false),
			new DmoField("pspCashRoundAmt", "money", 12, 2, nullable: false),
			new DmoField("pspTotal", "money", 12, 2, nullable: false),
			new DmoField("pspClosed", "bit", 1, 0, nullable: false),
			new DmoField("pspClosedDate", "date", 14, 0, nullable: true),
			new DmoField("pspPosted", "bit", 1, 0, nullable: false),
			new DmoField("pspPostedDate", "date", 14, 0, nullable: true),
			new DmoField("pspVoided", "bit", 1, 0, nullable: false),
			new DmoField("pspVoidedDate", "date", 14, 0, nullable: true),
			new DmoField("pspReturnFromTransactionsIDs", "nvarchar", 130, 0, nullable: false),
			new DmoField("pspSalesEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("pspCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("pspCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("pspUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[12]
		{
			new DmoIndex("PSPPOSTRANSACTIONID", unique: true),
			new DmoIndex("PSPUNIQUEID", unique: true),
			new DmoIndex("pspPOSSessionID", unique: false),
			new DmoIndex("pspGLFiscalYearID", unique: false),
			new DmoIndex("pspGLFiscalYearPeriodID", unique: false),
			new DmoIndex("pspCustomerOrganizationID", unique: false),
			new DmoIndex("pspARInvoiceLocationID", unique: false),
			new DmoIndex("pspShipOrganizationID", unique: false),
			new DmoIndex("pspClosed", unique: false),
			new DmoIndex("pspPosted", unique: false),
			new DmoIndex("pspVoided", unique: false),
			new DmoIndex("pspSalesEmployeeID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
