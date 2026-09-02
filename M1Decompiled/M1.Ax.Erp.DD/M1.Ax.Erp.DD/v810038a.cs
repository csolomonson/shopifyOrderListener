using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.10.038", "Add Avalara related fields", "2013-09-19")]
public class v810038a
{
	public v810038a(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraAccountID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraAccountID", "varchar", 50, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraURL"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraURL", "varchar", 120, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraCompanyCode"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraCompanyCode", "varchar", 50, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraLicenseKey"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraLicenseKey", "varchar", 50, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraTimeoutSeconds"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraTimeoutSeconds", "numeric", 4, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraTaxCodeID", "char", 5, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "PartGroups", "imuAvalaraTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "PartGroups", "imuAvalaraTaxCodeID", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ShippingMethods", "xasAvalaraTaxCodeID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ShippingMethods", "xasAvalaraTaxCodeID", "char", 10, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderDeliveries", "omdAvalaraNonTaxReasonID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderDeliveries", "omdAvalaraNonTaxReasonID", "char", 5, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrders", "ompAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrders", "ompAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Quotes", "qmpAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Quotes", "qmpAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoAvalaraUseCodes"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoAvalaraUseCodes", "char", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlAvalaraUseCodes"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlAvalaraUseCodes", "char", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesTableExist(null, parms.User, parms.DatabaseName, "AvalaraTransactions"))
		{
			parms.Dmo.CreateTable(null, parms.User, parms.DataDictionary, parms.DatabaseName, "AvalaraTransactions");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentSessions", "arsAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentSessions", "arsAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentHeaders", "artAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentHeaders", "artAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARPaymentLines", "arnAvalaraTaxCalculated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARPaymentLines", "arnAvalaraTaxCalculated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraForceAddressValidate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraForceAddressValidate", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoAvalaraAddressValidated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoAvalaraAddressValidated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlAvalaraAddressValidated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlAvalaraAddressValidated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Plants", "xauAvalaraAddressValidated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Plants", "xauAvalaraAddressValidated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Warehouses", "imwAvalaraAddressValidated"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Warehouses", "imwAvalaraAddressValidated", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoices", "arpAvalaraOverrideTax"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoices", "arpAvalaraOverrideTax", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraDisableAddrValidate"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraDisableAddrValidate", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraFilterCountry"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraFilterCountry", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraARInvoicePostOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraARInvoicePostOption", "numeric", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FinancialProperties SET xafAvalaraARInvoicePostOption = 2");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "FinancialProperties", "xafAvalaraDisableIgnoreLine"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "FinancialProperties", "xafAvalaraDisableIgnoreLine", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
			parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "UPDATE FinancialProperties SET xafAvalaraDisableIgnoreLine = 1");
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SalesOrderLines", "omlAvalaraIgnoreLine"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SalesOrderLines", "omlAvalaraIgnoreLine", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "ARInvoiceLines", "arlAvalaraIgnoreLine"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "ARInvoiceLines", "arlAvalaraIgnoreLine", "bit", 1, 0, verifyIndexes: false, dropTriggers: true, parms.Messages);
		}
	}
}
