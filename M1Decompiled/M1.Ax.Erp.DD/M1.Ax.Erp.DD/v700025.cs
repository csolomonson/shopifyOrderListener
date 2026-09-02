using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("7.00.025", "Set Currency Rate field on various tables", "2008-02-26")]
public class v700025
{
	public v700025(DBConversionParms parms)
	{
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PURCHASEORDERS Set pmpCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), pmpExchangeRate  = 1, pmpCustomRate= 0 Where pmpCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> ''");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SALESORDERS Set ompCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), ompExchangeRate  = 1, ompCustomRate= 0 Where ompCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update QUOTES Set qmpCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), qmpExchangeRate  = 1, qmpCustomRate= 0 Where qmpCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update RFQSUPPLIERS Set rqsCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), rqsExchangeRate  = 1, rqsCustomRate= 0 Where rqsCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ORGANIZATIONS Set cmoCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) Where cmoCurrencyRateId = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ORGANIZATIONLOCATIONS Set cmlCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) Where cmlCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PARTPRICES Set imiCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) Where imiCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update PARTUNITSALEPRICES Set imhCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) Where imhCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update SHIPMENTS Set smpCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), smpExchangeRate  = 1, smpCustomRate= 0 Where smpCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update APINVOICES Set appCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), appExchangeRate  = 1, appCustomRate= 0 Where appCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARINVOICES Set arpCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), arpExchangeRate  = 1, arpCustomRate= 0 Where arpCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
		parms.ServerManager.ExecuteCommand(null, parms.User, parms.DatabaseName, "Update ARRECURRINGINVOICES Set arrCurrencyRateID =  (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES), arrExchangeRate  = 1, arrCustomRate= 0 Where arrCurrencyRateID = '' AND (SELECT XADCURRENCYRATEID FROM DATASETPROPERTIES) <> '' ");
	}
}
