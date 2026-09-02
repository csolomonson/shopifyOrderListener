using System.Data.SqlClient;
using M1.Core;

namespace M1.Ax.Erp.Import;

[ImportProcessing("Organizations")]
public class OrganizationsImport : IImportProcessing
{
	public void BeforeUpdate(ImportProcessingParms parm)
	{
	}

	public void AfterUpdate(ImportProcessingParms parm)
	{
		parm.Database.ExecuteCommand(new SqlCommand("Insert Into OrganizationLocations (cmlOrganizationID,cmlLocationID) Select cmoOrganizationID,'' As cmlLocationID From Organizations Where cmoOrganizationID Not In (Select cmlOrganizationID From OrganizationLocations Where cmlLocationID = '') And cmoOrganizationID In (Select cmoOrganizationID From " + parm.TempTable + ") Group By cmoOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update OrganizationLocations Set cmlName = dest.cmoName, cmlAddressLine1 = dest.cmoAddressLine1, cmlAddressLine2 = dest.cmoAddressLine2, cmlAddressLine3 = dest.cmoAddressLine3, cmlCity = dest.cmoCity, cmlState = dest.cmoState, cmlCountry = dest.cmoCountry, cmlPostCode = dest.cmoPostCode, cmlPhoneNumber = dest.cmoPhoneNumber, cmlAlternatePhoneNumber = dest.cmoAlternatePhoneNumber, cmlFaxNumber = dest.cmoFaxNumber, cmlQuoteContactID = dest.cmoQuoteContactID, cmlQuoteLocation = Case When dest.cmoCustomerStatus = 1 Or dest.cmoCustomerStatus = 2 Then -1 Else 0 End, cmlShipContactID = dest.cmoShipContactID, cmlShipLocation = Case When dest.cmoCustomerStatus = 2 Then -1 Else 0 End, cmlARInvoiceContactID = dest.cmoARInvoiceContactID, cmlARInvoiceLocation = Case When dest.cmoCustomerStatus = 2 Then -1 Else 0 End,cmlPurchaseContactID = dest.cmoPurchaseContactID, cmlPurchaseLocation = Case When dest.cmoSupplierStatus = 2 Then -1 Else 0 End, cmlAPInvoiceContactID = dest.cmoAPInvoiceContactID, cmlAPInvoiceLocation = Case When dest.cmoSupplierStatus = 2 Then -1 Else 0 End,cmlCustomerTaxable = dest.cmoCustomerTaxable, cmlCustomerTaxCodeID = dest.cmoCustomerTaxCodeID, cmlCustomerShippingMethodID = dest.cmoCustomerShippingMethodID, cmlCustomerShipPaymentTypeID = dest.cmoCustomerShipPaymentTypeID,cmlCustomerPaymentTermID = dest.cmoCustomerPaymentTermsID, cmlARInvoicePerShipmentLine = dest.cmoARInvoicePerShipmentLine, cmlSupplierShippingMethodID = dest.cmoSupplierShippingMethodID, cmlSupplierPaymentTermID = dest.cmoSupplierPaymentTermID, cmlCurrencyRateID = dest.cmoCurrencyRateID, cmlCreditHold = dest.cmoCreditHold, cmlCustomerCreditLimit = dest.cmoCustomerCreditLimit, cmlTaxExemptNumber = dest.cmoTaxExemptNumber, cmlNonTaxReasonID = dest.cmoNonTaxReasonID, cmlEMailAddress = dest.cmoEMailAddress From OrganizationLocations Inner Join Organizations dest On cmlOrganizationID = cmoOrganizationID And cmlLocationID = '' Inner Join " + parm.TempTable + " On dest.cmoOrganizationID = " + parm.TempTable + ".cmoOrganizationID"));
		parm.Database.ExecuteCommand(new SqlCommand("Update OrganizationLocations Set cmlName = dest.cmoName,cmlAddressLine1 = dest.cmoAddressLine1,cmlAddressLine2 = dest.cmoAddressLine2,cmlAddressLine3 = dest.cmoAddressLine3,cmlCity = dest.cmoCity,cmlCounty = dest.cmoCounty,cmlState = dest.cmoState,cmlPostCode = dest.cmoPostCode,cmlCountry = dest.cmoCountry,cmlPhoneNumber = dest.cmoPhoneNumber,cmlAlternatePhoneNumber = dest.cmoAlternatePhoneNumber,cmlFaxNumber = dest.cmoFaxNumber,cmlEmailAddress = dest.cmoEmailAddress From OrganizationLocations Inner Join Organizations dest On cmlFinanceOrganizationID = dest.cmoOrganizationID Inner Join " + parm.TempTable + " On cmlFinanceOrganizationID = " + parm.TempTable + ".cmoOrganizationID"));
	}
}
