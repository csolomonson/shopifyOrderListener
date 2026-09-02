using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.050", "Convert Quotes to support unicode", "2013-10-17")]
public class v810RebuildQuotes
{
	public v810RebuildQuotes(DBConversionParms parms)
	{
		parms.Dmo.RebuildTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Quotes", new DmoField[35]
		{
			new DmoField("qmpQuoteID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpCustomerOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpPlantDepartmentID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpPlantID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpARInvoiceLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpARInvoiceContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpQuoteLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpQuoteContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpShipOrganizationID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpShipLocationID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpShipContactID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpStandardMessageID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpShippingPaymentTypeID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpQuoterEmployeeID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpDueDate", "date", 14, 0, nullable: true),
			new DmoField("qmpQuoteDate", "date", 14, 0, nullable: true),
			new DmoField("qmpExpirationDate", "date", 14, 0, nullable: true),
			new DmoField("qmpQuoteHeaderMessageRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmpQuoteHeaderMessageText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmpQuoteFooterMessageRTF", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmpQuoteFooterMessageText", "nvarchar(max)", 50, 0, nullable: true),
			new DmoField("qmpCurrencyRateID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpCustomRate", "bit", 1, 0, nullable: false),
			new DmoField("qmpExchangeRate", "numeric", 13, 6, nullable: false),
			new DmoField("qmpProjectID", "nvarchar", 10, 0, nullable: false),
			new DmoField("qmpClosed", "bit", 1, 0, nullable: false),
			new DmoField("qmpClosedDate", "date", 14, 0, nullable: true),
			new DmoField("qmpPaymentTermID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpShippingMethodID", "nvarchar", 5, 0, nullable: false),
			new DmoField("qmpFreeOnBoardDescription", "nvarchar", 15, 0, nullable: false),
			new DmoField("qmpAvalaraTaxCalculated", "bit", 1, 0, nullable: false),
			new DmoField("qmpCreatedFromMobile", "bit", 1, 0, nullable: false),
			new DmoField("qmpCreatedBy", "nvarchar", 20, 0, nullable: false),
			new DmoField("qmpCreatedDate", "datetime", 14, 0, nullable: true),
			new DmoField("qmpUniqueID", "uniqueidentifier", 16, 0, nullable: false)
		}, new DmoIndex[10]
		{
			new DmoIndex("QMPQUOTEID", unique: true),
			new DmoIndex("QMPUNIQUEID", unique: true),
			new DmoIndex("qmpCustomerOrganizationID", unique: false),
			new DmoIndex("qmpPlantDepartmentID", unique: false),
			new DmoIndex("qmpPlantID", unique: false),
			new DmoIndex("qmpARInvoiceLocationID", unique: false),
			new DmoIndex("qmpShipOrganizationID", unique: false),
			new DmoIndex("qmpProjectID", unique: false),
			new DmoIndex("qmpClosed", unique: false),
			new DmoIndex("qmpShippingMethodID", unique: false)
		}, mergeCustomFields: true, disableTriggers: true);
	}
}
