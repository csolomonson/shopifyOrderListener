using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert APInvoices to support unicode", "2013-10-17")]
public class v810RebuildAPInvoices
{
	public v810RebuildAPInvoices(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoices", new DmoField[62]
		{
			new DmoField("appAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("appPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appSupplierOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("appAPInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appAPInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appSupplierInvoiceNumber", "nvarchar", 30, 0, nullable: false),
			new DmoField("appInvoiceType", "tinyint", 1, 0, nullable: false),
			new DmoField("appCreditReasonID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appInvoiceDescription", "nvarchar", 50, 0, nullable: false),
			new DmoField("appInvoiceCommentsRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("appInvoiceCommentsText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("appCreditAPInvoiceID", "nvarchar", 10, 0, nullable: false),
			new DmoField("appPaymentTermID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appInvoiceDate", "date", 14, 0, nullable: true),
			new DmoField("appDueDate", "date", 14, 0, nullable: true),
			new DmoField("appDiscountDueDate", "date", 14, 0, nullable: true),
			new DmoField("appGLFiscalYearID", "smallint", 4, 0, nullable: false),
			new DmoField("appGLFiscalYearPeriodID", "tinyint", 2, 0, nullable: false),
			new DmoField("appCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("appExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("appInvoiceSubtotalBase", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceSubtotalForeign", "money", 12, 2, nullable: false),
			new DmoField("appFreightAmountBase", "money", 12, 2, nullable: false),
			new DmoField("appFreightAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("appFreightTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appFreightTaxAmountBase", "money", 14, 4, nullable: false),
			new DmoField("appFreightTaxAmountForeign", "money", 14, 4, nullable: false),
			new DmoField("appSecondFreightTaxCodeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("appSecondFreightTaxAmtBase", "money", 14, 4, nullable: false),
			new DmoField("appSecondFreightTaxAmtForeign", "money", 14, 4, nullable: false),
			new DmoField("appInvoiceTaxAmountBase", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceTaxAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceTotalBase", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("appDiscountAmountBase", "money", 12, 2, nullable: false),
			new DmoField("appDiscountAmountForeign", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceBalanceBase", "money", 12, 2, nullable: false),
			new DmoField("appInvoiceBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("appPaidComplete", "bit", 1, 0, nullable: false),
			new DmoField("appPaidDate", "date", 14, 0, nullable: true),
			new DmoField("appPostedToGL", "bit", 1, 0, nullable: false),
			new DmoField("appPostedDate", "date", 14, 0, nullable: true),
			new DmoField("appAPGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("appFreightGLAccountID", "nvarchar", 11, 0, nullable: false),
			new DmoField("appOnHold", "bit", 1, 0, nullable: false),
			new DmoField("appProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("appOpenInvoiceLoad", "bit", 1, 0, nullable: false),
			new DmoField("appRetentionTotalBase", "money", 12, 2, nullable: false),
			new DmoField("appRetentionTotalForeign", "money", 12, 2, nullable: false),
			new DmoField("appOverpayment", "bit", 1, 0, nullable: false),
			new DmoField("appOverPaymentSessionID", "int", 9, 0, nullable: false),
			new DmoField("appOverPaymentHeaderID", "int", 7, 0, nullable: false),
			new DmoField("appOriginalExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("appRetentionBalanceBase", "money", 12, 2, nullable: false),
			new DmoField("appRetentionBalanceForeign", "money", 12, 2, nullable: false),
			new DmoField("appCreditDate", "date", 14, 0, nullable: true),
			new DmoField("appTaxReportable", "bit", 1, 0, nullable: false),
			new DmoField("appCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("appCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("appUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[14]
		{
			new DmoIndex("APPAPINVOICEID", unique: true),
			new DmoIndex("APPUNIQUEID", unique: true),
			new DmoIndex("appPlantDepartmentID", unique: false),
			new DmoIndex("appPlantID", unique: false),
			new DmoIndex("appSupplierOrganizationID", unique: false),
			new DmoIndex("appAPInvoiceLocationID", unique: false),
			new DmoIndex("appSupplierInvoiceNumber", unique: false),
			new DmoIndex("appGLFiscalYearID", unique: false),
			new DmoIndex("appGLFiscalYearPeriodID", unique: false),
			new DmoIndex("appPaidComplete", unique: false),
			new DmoIndex("appPaidDate", unique: false),
			new DmoIndex("appPostedToGL", unique: false),
			new DmoIndex("appOnHold", unique: false),
			new DmoIndex("appProjectID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
