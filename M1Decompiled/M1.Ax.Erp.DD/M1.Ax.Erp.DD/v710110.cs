using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.10.110", "Add indexes for project related queries", "2008-08-08")]
public class v710110
{
	public v710110(DBConversionParms parms)
	{
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "PurchaseOrderLines", "pmlPurchaseType");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PurchaseOrderLines", "pmlPurchaseType", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdDeliveryType");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdDeliveryType", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlFirm");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlPurchaseToOrder");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "QuoteLines", "qmlProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlFirm", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlPurchaseToOrder", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "QuoteLines", "qmlProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlWorkType");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlCompletionType");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "TimecardLines", "lmlProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlWorkType", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlCompletionType", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "TimecardLines", "lmlProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "Jobs", "jmpProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Jobs", "jmpProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "JobOperations", "jmoOperationType");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobOperations", "jmoOperationType", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "JobCosts", "jmcSource");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "JobCosts", "jmcTotalCost");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobCosts", "jmcSource", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "JobCosts", "jmcTotalCost", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ShipmentLines", "smlProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", "smlProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShipmentLines", "smlProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "APInvoiceLines", "aplProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "APInvoiceLines", "aplProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceLines", "aplProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "APInvoiceLines", "aplProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlProjectAreaID", parms.Messages);
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqProjectID");
		parms.Dmo.DropRelatedIndexes(null, parms.User, parms.DatabaseName, "ARRecurringInvoiceLines", "arqProjectAreaID");
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqProjectID", parms.Messages);
		parms.Dmo.VerifyIndexesOnField(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARRecurringInvoiceLines", "arqProjectAreaID", parms.Messages);
	}
}
