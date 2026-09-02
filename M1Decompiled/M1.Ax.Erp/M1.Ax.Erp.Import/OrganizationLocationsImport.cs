using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("OrganizationLocations")]
public class OrganizationLocationsImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into Organizations (cmoOrganizationID) Select cmlOrganizationID From OrganizationLocations Where cmlOrganizationID Not In (Select cmoOrganizationID From Organizations) And cmlOrganizationID In (Select cmlOrganizationID From " + parm.TempTable + ") Group By cmlOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into OrganizationLocations (cmlOrganizationID,cmlLocationID) Select cmoOrganizationID,'' As cmlLocationID From Organizations Where cmoOrganizationID Not In (Select cmlOrganizationID From OrganizationLocations Where cmlLocationID = '') And cmoOrganizationID In (Select cmlOrganizationID From " + parm.TempTable + ") Group By cmoOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update Organizations Set cmoName = dest.cmlName, cmoAddressLine1 = dest.cmlAddressLine1, cmoAddressLine2 = dest.cmlAddressLine2, cmoAddressLine3 = dest.cmlAddressLine3, cmoCity = dest.cmlCity, cmoState = dest.cmlState, cmoCountry = dest.cmlCountry, cmoPostCode = dest.cmlPostCode, cmoPhoneNumber = dest.cmlPhoneNumber, cmoAlternatePhoneNumber = dest.cmlAlternatePhoneNumber, cmoFaxNumber = dest.cmlFaxNumber, cmoQuoteContactID = dest.cmlQuoteContactID, cmoShipContactID = dest.cmlShipContactID, cmoARInvoiceContactID = dest.cmlARInvoiceContactID,cmoPurchaseContactID = dest.cmlPurchaseContactID, cmoAPInvoiceContactID = dest.cmlAPInvoiceContactID,cmoCustomerTaxable = dest.cmlCustomerTaxable, cmoCustomerTaxCodeID = dest.cmlCustomerTaxCodeID, cmoCustomerShippingMethodID = dest.cmlCustomerShippingMethodID, cmoCustomerShipPaymentTypeID = dest.cmlCustomerShipPaymentTypeID,cmoCustomerPaymentTermsID = dest.cmlCustomerPaymentTermID, cmoARInvoicePerShipmentLine = dest.cmlARInvoicePerShipmentLine, cmoSupplierShippingMethodID = dest.cmlSupplierShippingMethodID, cmoSupplierPaymentTermID = dest.cmlSupplierPaymentTermID, cmoCurrencyRateID = dest.cmlCurrencyRateID, cmoCreditHold = dest.cmlCreditHold, cmoCustomerCreditLimit = dest.cmlCustomerCreditLimit, cmoTaxExemptNumber = dest.cmlTaxExemptNumber, cmoNonTaxReasonID = dest.cmlNonTaxReasonID, cmoEMailAddress = dest.cmlEMailAddress From Organizations Inner Join OrganizationLocations dest On cmlOrganizationID = cmoOrganizationID And cmlLocationID = '' Inner Join " + parm.TempTable + " On dest.cmlOrganizationID = " + parm.TempTable + ".cmlOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update OrganizationLocations Set cmlName = dest.cmoName,cmlAddressLine1 = dest.cmoAddressLine1,cmlAddressLine2 = dest.cmoAddressLine2,cmlAddressLine3 = dest.cmoAddressLine3,cmlCity = dest.cmoCity,cmlCounty = dest.cmoCounty,cmlState = dest.cmoState,cmlPostCode = dest.cmoPostCode,cmlCountry = dest.cmoCountry,cmlPhoneNumber = dest.cmoPhoneNumber,cmlAlternatePhoneNumber = dest.cmoAlternatePhoneNumber,cmlFaxNumber = dest.cmoFaxNumber,cmlEmailAddress = dest.cmoEmailAddress From OrganizationLocations Inner Join Organizations dest On cmlFinanceOrganizationID = dest.cmoOrganizationID Inner Join " + parm.TempTable + " On cmlFinanceOrganizationID = " + parm.TempTable + ".cmlOrganizationID Where " + parm.TempTable + ".cmlLocationID = ''"));
	}
}
