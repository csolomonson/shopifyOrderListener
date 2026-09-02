using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPOrganizationLocationRepository : APIBaseRepository, IERPOrganizationLocationRepository, IAPIBaseRepository, IDisposable
{
	public ERPOrganizationLocationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesOrganizationLocationExist(Guid organizationLocationId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmlUniqueID|C", organizationLocationId);
		base.selectList.Add("cmlUniqueID");
		return Task.FromResult(GetAsObject("OrganizationLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPOrganizationLocationInformationDto>> GetAllOrganizationLocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPOrganizationLocationInformationDto> collection = new List<ERPOrganizationLocationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[89]
		{
			"cmlAddressLine1", "cmlAddressLine2", "cmlAddressLine3", "cmlAddressValidationResult", "cmlAlternatePhoneNumber", "cmlApInvoiceContactID", "cmlArInvoiceContactID", "cmlAvalaraUseCodes", "cmlBankAccountName", "cmlBankAccountNumber",
			"cmlBankAccountType", "cmlBankInitials", "cmlBic", "cmlBsbNumber", "cmlCity", "cmlCountry", "cmlCountryCode", "cmlCounty", "cmlCreatedBy", "cmlCreatedDate",
			"cmlCurrencyRateID", "cmlCustomerCreditLimit", "cmlCustomerPaymentTermID", "cmlCustomerSecondTaxCodeID", "cmlCustomerShipPaymentTypeID", "cmlCustomerShippingCarrier", "cmlCustomerShippingMethodID", "cmlCustomerTaxCodeID", "cmlEdiLocationID", "cmlEftCode",
			"cmlEftDescription", "cmlEftParticulars", "cmlEmailAddress", "cmlUniqueID", "cmlFaxNumber", "cmlFedEx3rdPartyLocationID", "cmlFedEx3rdPartyOrganizationID", "cmlFedExAccountNumber", "cmlFedExBillingOption", "cmlFinanceOrganizationID",
			"cmlFirstGivenName", "cmlFreeOnBoardDescription", "cmlHdAttachmentFilePath", "cmlIban", "cmlInactiveDate", "cmlInactive", "cmlApInvoiceLocation", "cmlArInvoiceLocation", "cmlArInvoicePerShipmentLine", "cmlAvalaraAddressValidated",
			"cmlBareCostOfDuty", "cmlBareTransportationCost", "cmlContractor", "cmlCreatedFromMobile", "cmlCreditCheckForLocation", "cmlCreditHold", "cmlCustomerTaxable", "cmlDirectPayment", "cmlEdiIntegrated", "cmlIgnoreAvalara",
			"cmlPurchaseLocation", "cmlQuoteLocation", "cmlResidentialAddress", "cmlShipLocation", "cmlTaxReportable", "cmlUpsValidated", "cmlLastName", "cmlLocationID", "cmlName", "cmlNonTaxReasonID",
			"cmlOrganizationID", "cmlPhoneNumber", "cmlPostCode", "cmlPurchaseContactID", "cmlQuoteContactID", "cmlRowVersion", "cmlSecondGivenName", "cmlShipContactID", "cmlSplitPercentTotal", "cmlState",
			"cmlSupplierPaymentTermID", "cmlSupplierShippingMethodID", "cmlTaxExemptNumber", "cmlTradingName", "cmlUps3rdPartyLocationID", "cmlUps3rdPartyOrganizationID", "cmlUpsAcctNumber", "cmlUpsBillingOption", "cmlUpsWsBillingOption"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("OrganizationLocations");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("OrganizationLocations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPOrganizationLocationInformationDto eRPOrganizationLocationInformationDto = new ERPOrganizationLocationInformationDto();
				eRPOrganizationLocationInformationDto.cmlAddressLine1 = dataTable.Rows[i].Field<string>("cmlAddressLine1");
				eRPOrganizationLocationInformationDto.cmlAddressLine2 = dataTable.Rows[i].Field<string>("cmlAddressLine2");
				eRPOrganizationLocationInformationDto.cmlAddressLine3 = dataTable.Rows[i].Field<string>("cmlAddressLine3");
				eRPOrganizationLocationInformationDto.cmlAddressValidationResult = dataTable.Rows[i].Field<string>("cmlAddressValidationResult");
				eRPOrganizationLocationInformationDto.cmlAlternatePhoneNumber = dataTable.Rows[i].Field<string>("cmlAlternatePhoneNumber");
				eRPOrganizationLocationInformationDto.cmlApInvoiceContactID = dataTable.Rows[i].Field<string>("cmlApInvoiceContactID");
				eRPOrganizationLocationInformationDto.cmlArInvoiceContactID = dataTable.Rows[i].Field<string>("cmlArInvoiceContactID");
				eRPOrganizationLocationInformationDto.cmlAvalaraUseCodes = dataTable.Rows[i].Field<string>("cmlAvalaraUseCodes");
				eRPOrganizationLocationInformationDto.cmlBankAccountName = dataTable.Rows[i].Field<string>("cmlBankAccountName");
				eRPOrganizationLocationInformationDto.cmlBankAccountNumber = dataTable.Rows[i].Field<string>("cmlBankAccountNumber");
				eRPOrganizationLocationInformationDto.cmlBankAccountType = dataTable.Rows[i].Field<string>("cmlBankAccountType");
				eRPOrganizationLocationInformationDto.cmlBankInitials = dataTable.Rows[i].Field<string>("cmlBankInitials");
				eRPOrganizationLocationInformationDto.cmlBic = dataTable.Rows[i].Field<string>("cmlBic");
				eRPOrganizationLocationInformationDto.cmlBsbNumber = dataTable.Rows[i].Field<string>("cmlBsbNumber");
				eRPOrganizationLocationInformationDto.cmlCity = dataTable.Rows[i].Field<string>("cmlCity");
				eRPOrganizationLocationInformationDto.cmlCountry = dataTable.Rows[i].Field<string>("cmlCountry");
				eRPOrganizationLocationInformationDto.cmlCountryCode = dataTable.Rows[i].Field<string>("cmlCountryCode");
				eRPOrganizationLocationInformationDto.cmlCounty = dataTable.Rows[i].Field<string>("cmlCounty");
				eRPOrganizationLocationInformationDto.cmlCreatedBy = dataTable.Rows[i].Field<string>("cmlCreatedBy");
				eRPOrganizationLocationInformationDto.cmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("cmlCreatedDate");
				eRPOrganizationLocationInformationDto.cmlCurrencyRateID = dataTable.Rows[i].Field<string>("cmlCurrencyRateID");
				eRPOrganizationLocationInformationDto.cmlCustomerCreditLimit = dataTable.Rows[i].Field<decimal>("cmlCustomerCreditLimit");
				eRPOrganizationLocationInformationDto.cmlCustomerPaymentTermID = dataTable.Rows[i].Field<string>("cmlCustomerPaymentTermID");
				eRPOrganizationLocationInformationDto.cmlCustomerSecondTaxCodeID = dataTable.Rows[i].Field<string>("cmlCustomerSecondTaxCodeID");
				eRPOrganizationLocationInformationDto.cmlCustomerShipPaymentTypeID = dataTable.Rows[i].Field<string>("cmlCustomerShipPaymentTypeID");
				eRPOrganizationLocationInformationDto.cmlCustomerShippingCarrier = dataTable.Rows[i].Field<string>("cmlCustomerShippingCarrier");
				eRPOrganizationLocationInformationDto.cmlCustomerShippingMethodID = dataTable.Rows[i].Field<string>("cmlCustomerShippingMethodID");
				eRPOrganizationLocationInformationDto.cmlCustomerTaxCodeID = dataTable.Rows[i].Field<string>("cmlCustomerTaxCodeID");
				eRPOrganizationLocationInformationDto.cmlEdiLocationID = dataTable.Rows[i].Field<string>("cmlEdiLocationID");
				eRPOrganizationLocationInformationDto.cmlEftCode = dataTable.Rows[i].Field<string>("cmlEftCode");
				eRPOrganizationLocationInformationDto.cmlEftDescription = dataTable.Rows[i].Field<string>("cmlEftDescription");
				eRPOrganizationLocationInformationDto.cmlEftParticulars = dataTable.Rows[i].Field<string>("cmlEftParticulars");
				eRPOrganizationLocationInformationDto.cmlEmailAddress = dataTable.Rows[i].Field<string>("cmlEmailAddress");
				eRPOrganizationLocationInformationDto.cmlUniqueID = dataTable.Rows[i].Field<Guid>("cmlUniqueID");
				eRPOrganizationLocationInformationDto.cmlFaxNumber = dataTable.Rows[i].Field<string>("cmlFaxNumber");
				eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyLocationID = dataTable.Rows[i].Field<string>("cmlFedEx3rdPartyLocationID");
				eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("cmlFedEx3rdPartyOrganizationID");
				eRPOrganizationLocationInformationDto.cmlFedExAccountNumber = dataTable.Rows[i].Field<string>("cmlFedExAccountNumber");
				eRPOrganizationLocationInformationDto.cmlFedExBillingOption = dataTable.Rows[i].Field<string>("cmlFedExBillingOption");
				eRPOrganizationLocationInformationDto.cmlFinanceOrganizationID = dataTable.Rows[i].Field<string>("cmlFinanceOrganizationID");
				eRPOrganizationLocationInformationDto.cmlFirstGivenName = dataTable.Rows[i].Field<string>("cmlFirstGivenName");
				eRPOrganizationLocationInformationDto.cmlFreeOnBoardDescription = dataTable.Rows[i].Field<string>("cmlFreeOnBoardDescription");
				eRPOrganizationLocationInformationDto.cmlHdAttachmentFilePath = dataTable.Rows[i].Field<string>("cmlHdAttachmentFilePath");
				eRPOrganizationLocationInformationDto.cmlIban = dataTable.Rows[i].Field<string>("cmlIban");
				eRPOrganizationLocationInformationDto.cmlInactiveDate = dataTable.Rows[i].Field<DateTime?>("cmlInactiveDate");
				eRPOrganizationLocationInformationDto.cmlInactive = dataTable.Rows[i].Field<bool>("cmlInactive");
				eRPOrganizationLocationInformationDto.cmlApInvoiceLocation = dataTable.Rows[i].Field<bool>("cmlApInvoiceLocation");
				eRPOrganizationLocationInformationDto.cmlArInvoiceLocation = dataTable.Rows[i].Field<bool>("cmlArInvoiceLocation");
				eRPOrganizationLocationInformationDto.cmlArInvoicePerShipmentLine = dataTable.Rows[i].Field<bool>("cmlArInvoicePerShipmentLine");
				eRPOrganizationLocationInformationDto.cmlAvalaraAddressValidated = dataTable.Rows[i].Field<bool>("cmlAvalaraAddressValidated");
				eRPOrganizationLocationInformationDto.cmlBareCostOfDuty = dataTable.Rows[i].Field<bool>("cmlBareCostOfDuty");
				eRPOrganizationLocationInformationDto.cmlBareTransportationCost = dataTable.Rows[i].Field<bool>("cmlBareTransportationCost");
				eRPOrganizationLocationInformationDto.cmlContractor = dataTable.Rows[i].Field<bool>("cmlContractor");
				eRPOrganizationLocationInformationDto.cmlCreatedFromMobile = dataTable.Rows[i].Field<bool>("cmlCreatedFromMobile");
				eRPOrganizationLocationInformationDto.cmlCreditCheckForLocation = dataTable.Rows[i].Field<bool>("cmlCreditCheckForLocation");
				eRPOrganizationLocationInformationDto.cmlCreditHold = dataTable.Rows[i].Field<bool>("cmlCreditHold");
				eRPOrganizationLocationInformationDto.cmlCustomerTaxable = dataTable.Rows[i].Field<bool>("cmlCustomerTaxable");
				eRPOrganizationLocationInformationDto.cmlDirectPayment = dataTable.Rows[i].Field<bool>("cmlDirectPayment");
				eRPOrganizationLocationInformationDto.cmlEdiIntegrated = dataTable.Rows[i].Field<bool>("cmlEdiIntegrated");
				eRPOrganizationLocationInformationDto.cmlIgnoreAvalara = dataTable.Rows[i].Field<bool>("cmlIgnoreAvalara");
				eRPOrganizationLocationInformationDto.cmlPurchaseLocation = dataTable.Rows[i].Field<bool>("cmlPurchaseLocation");
				eRPOrganizationLocationInformationDto.cmlQuoteLocation = dataTable.Rows[i].Field<bool>("cmlQuoteLocation");
				eRPOrganizationLocationInformationDto.cmlResidentialAddress = dataTable.Rows[i].Field<bool>("cmlResidentialAddress");
				eRPOrganizationLocationInformationDto.cmlShipLocation = dataTable.Rows[i].Field<bool>("cmlShipLocation");
				eRPOrganizationLocationInformationDto.cmlTaxReportable = dataTable.Rows[i].Field<bool>("cmlTaxReportable");
				eRPOrganizationLocationInformationDto.cmlUpsValidated = dataTable.Rows[i].Field<bool>("cmlUpsValidated");
				eRPOrganizationLocationInformationDto.cmlLastName = dataTable.Rows[i].Field<string>("cmlLastName");
				eRPOrganizationLocationInformationDto.cmlLocationID = dataTable.Rows[i].Field<string>("cmlLocationID");
				eRPOrganizationLocationInformationDto.cmlName = dataTable.Rows[i].Field<string>("cmlName");
				eRPOrganizationLocationInformationDto.cmlNonTaxReasonID = dataTable.Rows[i].Field<string>("cmlNonTaxReasonID");
				eRPOrganizationLocationInformationDto.cmlOrganizationID = dataTable.Rows[i].Field<string>("cmlOrganizationID");
				eRPOrganizationLocationInformationDto.cmlPhoneNumber = dataTable.Rows[i].Field<string>("cmlPhoneNumber");
				eRPOrganizationLocationInformationDto.cmlPostCode = dataTable.Rows[i].Field<string>("cmlPostCode");
				eRPOrganizationLocationInformationDto.cmlPurchaseContactID = dataTable.Rows[i].Field<string>("cmlPurchaseContactID");
				eRPOrganizationLocationInformationDto.cmlQuoteContactID = dataTable.Rows[i].Field<string>("cmlQuoteContactID");
				eRPOrganizationLocationInformationDto.cmlRowVersion = dataTable.Rows[i].Field<byte[]>("cmlRowVersion");
				eRPOrganizationLocationInformationDto.cmlSecondGivenName = dataTable.Rows[i].Field<string>("cmlSecondGivenName");
				eRPOrganizationLocationInformationDto.cmlShipContactID = dataTable.Rows[i].Field<string>("cmlShipContactID");
				eRPOrganizationLocationInformationDto.cmlSplitPercentTotal = dataTable.Rows[i].Field<decimal>("cmlSplitPercentTotal");
				eRPOrganizationLocationInformationDto.cmlState = dataTable.Rows[i].Field<string>("cmlState");
				eRPOrganizationLocationInformationDto.cmlSupplierPaymentTermID = dataTable.Rows[i].Field<string>("cmlSupplierPaymentTermID");
				eRPOrganizationLocationInformationDto.cmlSupplierShippingMethodID = dataTable.Rows[i].Field<string>("cmlSupplierShippingMethodID");
				eRPOrganizationLocationInformationDto.cmlTaxExemptNumber = dataTable.Rows[i].Field<string>("cmlTaxExemptNumber");
				eRPOrganizationLocationInformationDto.cmlTradingName = dataTable.Rows[i].Field<string>("cmlTradingName");
				eRPOrganizationLocationInformationDto.cmlUps3rdPartyLocationID = dataTable.Rows[i].Field<string>("cmlUps3rdPartyLocationID");
				eRPOrganizationLocationInformationDto.cmlUps3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("cmlUps3rdPartyOrganizationID");
				eRPOrganizationLocationInformationDto.cmlUpsAcctNumber = dataTable.Rows[i].Field<string>("cmlUpsAcctNumber");
				eRPOrganizationLocationInformationDto.cmlUpsBillingOption = dataTable.Rows[i].Field<string>("cmlUpsBillingOption");
				eRPOrganizationLocationInformationDto.cmlUpsWsBillingOption = dataTable.Rows[i].Field<string>("cmlUpsWsBillingOption");
				eRPOrganizationLocationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPOrganizationLocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPOrganizationLocationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPOrganizationLocationInformationDto> GetOrganizationLocation(Guid organizationLocationId)
	{
		ERPOrganizationLocationInformationDto eRPOrganizationLocationInformationDto = new ERPOrganizationLocationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[89]
		{
			"cmlAddressLine1", "cmlAddressLine2", "cmlAddressLine3", "cmlAddressValidationResult", "cmlAlternatePhoneNumber", "cmlApInvoiceContactID", "cmlArInvoiceContactID", "cmlAvalaraUseCodes", "cmlBankAccountName", "cmlBankAccountNumber",
			"cmlBankAccountType", "cmlBankInitials", "cmlBic", "cmlBsbNumber", "cmlCity", "cmlCountry", "cmlCountryCode", "cmlCounty", "cmlCreatedBy", "cmlCreatedDate",
			"cmlCurrencyRateID", "cmlCustomerCreditLimit", "cmlCustomerPaymentTermID", "cmlCustomerSecondTaxCodeID", "cmlCustomerShipPaymentTypeID", "cmlCustomerShippingCarrier", "cmlCustomerShippingMethodID", "cmlCustomerTaxCodeID", "cmlEdiLocationID", "cmlEftCode",
			"cmlEftDescription", "cmlEftParticulars", "cmlEmailAddress", "cmlUniqueID", "cmlFaxNumber", "cmlFedEx3rdPartyLocationID", "cmlFedEx3rdPartyOrganizationID", "cmlFedExAccountNumber", "cmlFedExBillingOption", "cmlFinanceOrganizationID",
			"cmlFirstGivenName", "cmlFreeOnBoardDescription", "cmlHdAttachmentFilePath", "cmlIban", "cmlInactiveDate", "cmlInactive", "cmlApInvoiceLocation", "cmlArInvoiceLocation", "cmlArInvoicePerShipmentLine", "cmlAvalaraAddressValidated",
			"cmlBareCostOfDuty", "cmlBareTransportationCost", "cmlContractor", "cmlCreatedFromMobile", "cmlCreditCheckForLocation", "cmlCreditHold", "cmlCustomerTaxable", "cmlDirectPayment", "cmlEdiIntegrated", "cmlIgnoreAvalara",
			"cmlPurchaseLocation", "cmlQuoteLocation", "cmlResidentialAddress", "cmlShipLocation", "cmlTaxReportable", "cmlUpsValidated", "cmlLastName", "cmlLocationID", "cmlName", "cmlNonTaxReasonID",
			"cmlOrganizationID", "cmlPhoneNumber", "cmlPostCode", "cmlPurchaseContactID", "cmlQuoteContactID", "cmlRowVersion", "cmlSecondGivenName", "cmlShipContactID", "cmlSplitPercentTotal", "cmlState",
			"cmlSupplierPaymentTermID", "cmlSupplierShippingMethodID", "cmlTaxExemptNumber", "cmlTradingName", "cmlUps3rdPartyLocationID", "cmlUps3rdPartyOrganizationID", "cmlUpsAcctNumber", "cmlUpsBillingOption", "cmlUpsWsBillingOption"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("cmlUniqueID|C", organizationLocationId);
		AddCustomFieldsToSelectList("OrganizationLocations");
		using (DataTable dataTable = GetAsDataTable("OrganizationLocations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPOrganizationLocationInformationDto);
			}
			eRPOrganizationLocationInformationDto.cmlAddressLine1 = dataTable.Rows[0].Field<string>("cmlAddressLine1");
			eRPOrganizationLocationInformationDto.cmlAddressLine2 = dataTable.Rows[0].Field<string>("cmlAddressLine2");
			eRPOrganizationLocationInformationDto.cmlAddressLine3 = dataTable.Rows[0].Field<string>("cmlAddressLine3");
			eRPOrganizationLocationInformationDto.cmlAddressValidationResult = dataTable.Rows[0].Field<string>("cmlAddressValidationResult");
			eRPOrganizationLocationInformationDto.cmlAlternatePhoneNumber = dataTable.Rows[0].Field<string>("cmlAlternatePhoneNumber");
			eRPOrganizationLocationInformationDto.cmlApInvoiceContactID = dataTable.Rows[0].Field<string>("cmlApInvoiceContactID");
			eRPOrganizationLocationInformationDto.cmlArInvoiceContactID = dataTable.Rows[0].Field<string>("cmlArInvoiceContactID");
			eRPOrganizationLocationInformationDto.cmlAvalaraUseCodes = dataTable.Rows[0].Field<string>("cmlAvalaraUseCodes");
			eRPOrganizationLocationInformationDto.cmlBankAccountName = dataTable.Rows[0].Field<string>("cmlBankAccountName");
			eRPOrganizationLocationInformationDto.cmlBankAccountNumber = dataTable.Rows[0].Field<string>("cmlBankAccountNumber");
			eRPOrganizationLocationInformationDto.cmlBankAccountType = dataTable.Rows[0].Field<string>("cmlBankAccountType");
			eRPOrganizationLocationInformationDto.cmlBankInitials = dataTable.Rows[0].Field<string>("cmlBankInitials");
			eRPOrganizationLocationInformationDto.cmlBic = dataTable.Rows[0].Field<string>("cmlBic");
			eRPOrganizationLocationInformationDto.cmlBsbNumber = dataTable.Rows[0].Field<string>("cmlBsbNumber");
			eRPOrganizationLocationInformationDto.cmlCity = dataTable.Rows[0].Field<string>("cmlCity");
			eRPOrganizationLocationInformationDto.cmlCountry = dataTable.Rows[0].Field<string>("cmlCountry");
			eRPOrganizationLocationInformationDto.cmlCountryCode = dataTable.Rows[0].Field<string>("cmlCountryCode");
			eRPOrganizationLocationInformationDto.cmlCounty = dataTable.Rows[0].Field<string>("cmlCounty");
			eRPOrganizationLocationInformationDto.cmlCreatedBy = dataTable.Rows[0].Field<string>("cmlCreatedBy");
			eRPOrganizationLocationInformationDto.cmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("cmlCreatedDate");
			eRPOrganizationLocationInformationDto.cmlCurrencyRateID = dataTable.Rows[0].Field<string>("cmlCurrencyRateID");
			eRPOrganizationLocationInformationDto.cmlCustomerCreditLimit = dataTable.Rows[0].Field<decimal>("cmlCustomerCreditLimit");
			eRPOrganizationLocationInformationDto.cmlCustomerPaymentTermID = dataTable.Rows[0].Field<string>("cmlCustomerPaymentTermID");
			eRPOrganizationLocationInformationDto.cmlCustomerSecondTaxCodeID = dataTable.Rows[0].Field<string>("cmlCustomerSecondTaxCodeID");
			eRPOrganizationLocationInformationDto.cmlCustomerShipPaymentTypeID = dataTable.Rows[0].Field<string>("cmlCustomerShipPaymentTypeID");
			eRPOrganizationLocationInformationDto.cmlCustomerShippingCarrier = dataTable.Rows[0].Field<string>("cmlCustomerShippingCarrier");
			eRPOrganizationLocationInformationDto.cmlCustomerShippingMethodID = dataTable.Rows[0].Field<string>("cmlCustomerShippingMethodID");
			eRPOrganizationLocationInformationDto.cmlCustomerTaxCodeID = dataTable.Rows[0].Field<string>("cmlCustomerTaxCodeID");
			eRPOrganizationLocationInformationDto.cmlEdiLocationID = dataTable.Rows[0].Field<string>("cmlEdiLocationID");
			eRPOrganizationLocationInformationDto.cmlEftCode = dataTable.Rows[0].Field<string>("cmlEftCode");
			eRPOrganizationLocationInformationDto.cmlEftDescription = dataTable.Rows[0].Field<string>("cmlEftDescription");
			eRPOrganizationLocationInformationDto.cmlEftParticulars = dataTable.Rows[0].Field<string>("cmlEftParticulars");
			eRPOrganizationLocationInformationDto.cmlEmailAddress = dataTable.Rows[0].Field<string>("cmlEmailAddress");
			eRPOrganizationLocationInformationDto.cmlUniqueID = dataTable.Rows[0].Field<Guid>("cmlUniqueID");
			eRPOrganizationLocationInformationDto.cmlFaxNumber = dataTable.Rows[0].Field<string>("cmlFaxNumber");
			eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyLocationID = dataTable.Rows[0].Field<string>("cmlFedEx3rdPartyLocationID");
			eRPOrganizationLocationInformationDto.cmlFedEx3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("cmlFedEx3rdPartyOrganizationID");
			eRPOrganizationLocationInformationDto.cmlFedExAccountNumber = dataTable.Rows[0].Field<string>("cmlFedExAccountNumber");
			eRPOrganizationLocationInformationDto.cmlFedExBillingOption = dataTable.Rows[0].Field<string>("cmlFedExBillingOption");
			eRPOrganizationLocationInformationDto.cmlFinanceOrganizationID = dataTable.Rows[0].Field<string>("cmlFinanceOrganizationID");
			eRPOrganizationLocationInformationDto.cmlFirstGivenName = dataTable.Rows[0].Field<string>("cmlFirstGivenName");
			eRPOrganizationLocationInformationDto.cmlFreeOnBoardDescription = dataTable.Rows[0].Field<string>("cmlFreeOnBoardDescription");
			eRPOrganizationLocationInformationDto.cmlHdAttachmentFilePath = dataTable.Rows[0].Field<string>("cmlHdAttachmentFilePath");
			eRPOrganizationLocationInformationDto.cmlIban = dataTable.Rows[0].Field<string>("cmlIban");
			eRPOrganizationLocationInformationDto.cmlInactiveDate = dataTable.Rows[0].Field<DateTime?>("cmlInactiveDate");
			eRPOrganizationLocationInformationDto.cmlInactive = dataTable.Rows[0].Field<bool>("cmlInactive");
			eRPOrganizationLocationInformationDto.cmlApInvoiceLocation = dataTable.Rows[0].Field<bool>("cmlApInvoiceLocation");
			eRPOrganizationLocationInformationDto.cmlArInvoiceLocation = dataTable.Rows[0].Field<bool>("cmlArInvoiceLocation");
			eRPOrganizationLocationInformationDto.cmlArInvoicePerShipmentLine = dataTable.Rows[0].Field<bool>("cmlArInvoicePerShipmentLine");
			eRPOrganizationLocationInformationDto.cmlAvalaraAddressValidated = dataTable.Rows[0].Field<bool>("cmlAvalaraAddressValidated");
			eRPOrganizationLocationInformationDto.cmlBareCostOfDuty = dataTable.Rows[0].Field<bool>("cmlBareCostOfDuty");
			eRPOrganizationLocationInformationDto.cmlBareTransportationCost = dataTable.Rows[0].Field<bool>("cmlBareTransportationCost");
			eRPOrganizationLocationInformationDto.cmlContractor = dataTable.Rows[0].Field<bool>("cmlContractor");
			eRPOrganizationLocationInformationDto.cmlCreatedFromMobile = dataTable.Rows[0].Field<bool>("cmlCreatedFromMobile");
			eRPOrganizationLocationInformationDto.cmlCreditCheckForLocation = dataTable.Rows[0].Field<bool>("cmlCreditCheckForLocation");
			eRPOrganizationLocationInformationDto.cmlCreditHold = dataTable.Rows[0].Field<bool>("cmlCreditHold");
			eRPOrganizationLocationInformationDto.cmlCustomerTaxable = dataTable.Rows[0].Field<bool>("cmlCustomerTaxable");
			eRPOrganizationLocationInformationDto.cmlDirectPayment = dataTable.Rows[0].Field<bool>("cmlDirectPayment");
			eRPOrganizationLocationInformationDto.cmlEdiIntegrated = dataTable.Rows[0].Field<bool>("cmlEdiIntegrated");
			eRPOrganizationLocationInformationDto.cmlIgnoreAvalara = dataTable.Rows[0].Field<bool>("cmlIgnoreAvalara");
			eRPOrganizationLocationInformationDto.cmlPurchaseLocation = dataTable.Rows[0].Field<bool>("cmlPurchaseLocation");
			eRPOrganizationLocationInformationDto.cmlQuoteLocation = dataTable.Rows[0].Field<bool>("cmlQuoteLocation");
			eRPOrganizationLocationInformationDto.cmlResidentialAddress = dataTable.Rows[0].Field<bool>("cmlResidentialAddress");
			eRPOrganizationLocationInformationDto.cmlShipLocation = dataTable.Rows[0].Field<bool>("cmlShipLocation");
			eRPOrganizationLocationInformationDto.cmlTaxReportable = dataTable.Rows[0].Field<bool>("cmlTaxReportable");
			eRPOrganizationLocationInformationDto.cmlUpsValidated = dataTable.Rows[0].Field<bool>("cmlUpsValidated");
			eRPOrganizationLocationInformationDto.cmlLastName = dataTable.Rows[0].Field<string>("cmlLastName");
			eRPOrganizationLocationInformationDto.cmlLocationID = dataTable.Rows[0].Field<string>("cmlLocationID");
			eRPOrganizationLocationInformationDto.cmlName = dataTable.Rows[0].Field<string>("cmlName");
			eRPOrganizationLocationInformationDto.cmlNonTaxReasonID = dataTable.Rows[0].Field<string>("cmlNonTaxReasonID");
			eRPOrganizationLocationInformationDto.cmlOrganizationID = dataTable.Rows[0].Field<string>("cmlOrganizationID");
			eRPOrganizationLocationInformationDto.cmlPhoneNumber = dataTable.Rows[0].Field<string>("cmlPhoneNumber");
			eRPOrganizationLocationInformationDto.cmlPostCode = dataTable.Rows[0].Field<string>("cmlPostCode");
			eRPOrganizationLocationInformationDto.cmlPurchaseContactID = dataTable.Rows[0].Field<string>("cmlPurchaseContactID");
			eRPOrganizationLocationInformationDto.cmlQuoteContactID = dataTable.Rows[0].Field<string>("cmlQuoteContactID");
			eRPOrganizationLocationInformationDto.cmlRowVersion = dataTable.Rows[0].Field<byte[]>("cmlRowVersion");
			eRPOrganizationLocationInformationDto.cmlSecondGivenName = dataTable.Rows[0].Field<string>("cmlSecondGivenName");
			eRPOrganizationLocationInformationDto.cmlShipContactID = dataTable.Rows[0].Field<string>("cmlShipContactID");
			eRPOrganizationLocationInformationDto.cmlSplitPercentTotal = dataTable.Rows[0].Field<decimal>("cmlSplitPercentTotal");
			eRPOrganizationLocationInformationDto.cmlState = dataTable.Rows[0].Field<string>("cmlState");
			eRPOrganizationLocationInformationDto.cmlSupplierPaymentTermID = dataTable.Rows[0].Field<string>("cmlSupplierPaymentTermID");
			eRPOrganizationLocationInformationDto.cmlSupplierShippingMethodID = dataTable.Rows[0].Field<string>("cmlSupplierShippingMethodID");
			eRPOrganizationLocationInformationDto.cmlTaxExemptNumber = dataTable.Rows[0].Field<string>("cmlTaxExemptNumber");
			eRPOrganizationLocationInformationDto.cmlTradingName = dataTable.Rows[0].Field<string>("cmlTradingName");
			eRPOrganizationLocationInformationDto.cmlUps3rdPartyLocationID = dataTable.Rows[0].Field<string>("cmlUps3rdPartyLocationID");
			eRPOrganizationLocationInformationDto.cmlUps3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("cmlUps3rdPartyOrganizationID");
			eRPOrganizationLocationInformationDto.cmlUpsAcctNumber = dataTable.Rows[0].Field<string>("cmlUpsAcctNumber");
			eRPOrganizationLocationInformationDto.cmlUpsBillingOption = dataTable.Rows[0].Field<string>("cmlUpsBillingOption");
			eRPOrganizationLocationInformationDto.cmlUpsWsBillingOption = dataTable.Rows[0].Field<string>("cmlUpsWsBillingOption");
			eRPOrganizationLocationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPOrganizationLocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPOrganizationLocationInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationLocation(ERPOrganizationLocationDto organizationLocation)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM OrganizationLocations WHERE cmlUniqueID = " + M1Util.ConvertToLinq(organizationLocation.cmlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["cmlOrganizationID"] = organizationLocation.cmlOrganizationID.ToUpper();
				dataRow["cmlLocationID"] = organizationLocation.cmlLocationID.ToUpper();
				organizationLocation.cmlUniqueID = ((organizationLocation.cmlUniqueID == Guid.Empty) ? Guid.NewGuid() : organizationLocation.cmlUniqueID);
				dataRow["cmlUniqueID"] = organizationLocation.cmlUniqueID;
				dataRow["cmlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["cmlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The OrganizationLocation could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (organizationLocation.cmlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the OrganizationLocation is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["cmlRowVersion"], organizationLocation.cmlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the OrganizationLocation has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the OrganizationLocation again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["cmlAddressLine1"] = organizationLocation.cmlAddressLine1;
			dataRow["cmlAddressLine2"] = organizationLocation.cmlAddressLine2;
			dataRow["cmlAddressLine3"] = organizationLocation.cmlAddressLine3;
			dataRow["cmlAddressValidationResult"] = organizationLocation.cmlAddressValidationResult ?? dataRow["cmlAddressValidationResult"];
			dataRow["cmlAlternatePhoneNumber"] = organizationLocation.cmlAlternatePhoneNumber;
			dataRow["cmlApInvoiceContactID"] = organizationLocation.cmlApInvoiceContactID;
			dataRow["cmlArInvoiceContactID"] = organizationLocation.cmlArInvoiceContactID;
			dataRow["cmlAvalaraUseCodes"] = organizationLocation.cmlAvalaraUseCodes;
			dataRow["cmlBankAccountName"] = organizationLocation.cmlBankAccountName;
			dataRow["cmlBankAccountNumber"] = organizationLocation.cmlBankAccountNumber;
			dataRow["cmlBankAccountType"] = organizationLocation.cmlBankAccountType;
			dataRow["cmlBankInitials"] = organizationLocation.cmlBankInitials;
			dataRow["cmlBic"] = organizationLocation.cmlBic;
			dataRow["cmlBsbNumber"] = organizationLocation.cmlBsbNumber;
			dataRow["cmlCity"] = organizationLocation.cmlCity;
			dataRow["cmlCountry"] = organizationLocation.cmlCountry;
			dataRow["cmlCountryCode"] = organizationLocation.cmlCountryCode;
			dataRow["cmlCounty"] = organizationLocation.cmlCounty;
			dataRow["cmlCurrencyRateID"] = organizationLocation.cmlCurrencyRateID;
			dataRow["cmlCustomerCreditLimit"] = organizationLocation.cmlCustomerCreditLimit;
			dataRow["cmlCustomerPaymentTermID"] = organizationLocation.cmlCustomerPaymentTermID;
			dataRow["cmlCustomerSecondTaxCodeID"] = organizationLocation.cmlCustomerSecondTaxCodeID;
			dataRow["cmlCustomerShipPaymentTypeID"] = organizationLocation.cmlCustomerShipPaymentTypeID;
			dataRow["cmlCustomerShippingCarrier"] = organizationLocation.cmlCustomerShippingCarrier;
			dataRow["cmlCustomerShippingMethodID"] = organizationLocation.cmlCustomerShippingMethodID;
			dataRow["cmlCustomerTaxCodeID"] = organizationLocation.cmlCustomerTaxCodeID;
			dataRow["cmlEdiLocationID"] = organizationLocation.cmlEdiLocationID;
			dataRow["cmlEftCode"] = organizationLocation.cmlEftCode;
			dataRow["cmlEftDescription"] = organizationLocation.cmlEftDescription;
			dataRow["cmlEftParticulars"] = organizationLocation.cmlEftParticulars;
			dataRow["cmlEmailAddress"] = organizationLocation.cmlEmailAddress ?? dataRow["cmlEmailAddress"];
			dataRow["cmlFaxNumber"] = organizationLocation.cmlFaxNumber;
			dataRow["cmlFedEx3rdPartyLocationID"] = organizationLocation.cmlFedEx3rdPartyLocationID;
			dataRow["cmlFedEx3rdPartyOrganizationID"] = organizationLocation.cmlFedEx3rdPartyOrganizationID;
			dataRow["cmlFedExAccountNumber"] = organizationLocation.cmlFedExAccountNumber;
			dataRow["cmlFedExBillingOption"] = organizationLocation.cmlFedExBillingOption;
			dataRow["cmlFinanceOrganizationID"] = organizationLocation.cmlFinanceOrganizationID;
			dataRow["cmlFirstGivenName"] = organizationLocation.cmlFirstGivenName;
			dataRow["cmlFreeOnBoardDescription"] = organizationLocation.cmlFreeOnBoardDescription;
			dataRow["cmlHdAttachmentFilePath"] = organizationLocation.cmlHdAttachmentFilePath ?? dataRow["cmlHdAttachmentFilePath"];
			dataRow["cmlIban"] = organizationLocation.cmlIban;
			DataRow dataRow2 = dataRow;
			DateTime? cmlInactiveDate = organizationLocation.cmlInactiveDate;
			dataRow2["cmlInactiveDate"] = (cmlInactiveDate.HasValue ? ((object)cmlInactiveDate.GetValueOrDefault()) : dataRow["cmlInactiveDate"]);
			dataRow["cmlInactive"] = organizationLocation.cmlInactive;
			dataRow["cmlApInvoiceLocation"] = organizationLocation.cmlApInvoiceLocation;
			dataRow["cmlArInvoiceLocation"] = organizationLocation.cmlArInvoiceLocation;
			dataRow["cmlArInvoicePerShipmentLine"] = organizationLocation.cmlArInvoicePerShipmentLine;
			dataRow["cmlAvalaraAddressValidated"] = organizationLocation.cmlAvalaraAddressValidated;
			dataRow["cmlBareCostOfDuty"] = organizationLocation.cmlBareCostOfDuty;
			dataRow["cmlBareTransportationCost"] = organizationLocation.cmlBareTransportationCost;
			dataRow["cmlContractor"] = organizationLocation.cmlContractor;
			dataRow["cmlCreatedFromMobile"] = organizationLocation.cmlCreatedFromMobile;
			dataRow["cmlCreditCheckForLocation"] = organizationLocation.cmlCreditCheckForLocation;
			dataRow["cmlCreditHold"] = organizationLocation.cmlCreditHold;
			dataRow["cmlCustomerTaxable"] = organizationLocation.cmlCustomerTaxable;
			dataRow["cmlDirectPayment"] = organizationLocation.cmlDirectPayment;
			dataRow["cmlEdiIntegrated"] = organizationLocation.cmlEdiIntegrated;
			dataRow["cmlIgnoreAvalara"] = organizationLocation.cmlIgnoreAvalara;
			dataRow["cmlPurchaseLocation"] = organizationLocation.cmlPurchaseLocation;
			dataRow["cmlQuoteLocation"] = organizationLocation.cmlQuoteLocation;
			dataRow["cmlResidentialAddress"] = organizationLocation.cmlResidentialAddress;
			dataRow["cmlShipLocation"] = organizationLocation.cmlShipLocation;
			dataRow["cmlTaxReportable"] = organizationLocation.cmlTaxReportable;
			dataRow["cmlUpsValidated"] = organizationLocation.cmlUpsValidated;
			dataRow["cmlLastName"] = organizationLocation.cmlLastName;
			dataRow["cmlName"] = organizationLocation.cmlName;
			dataRow["cmlNonTaxReasonID"] = organizationLocation.cmlNonTaxReasonID;
			dataRow["cmlPhoneNumber"] = organizationLocation.cmlPhoneNumber;
			dataRow["cmlPostCode"] = organizationLocation.cmlPostCode;
			dataRow["cmlPurchaseContactID"] = organizationLocation.cmlPurchaseContactID;
			dataRow["cmlQuoteContactID"] = organizationLocation.cmlQuoteContactID;
			dataRow["cmlSecondGivenName"] = organizationLocation.cmlSecondGivenName;
			dataRow["cmlShipContactID"] = organizationLocation.cmlShipContactID;
			dataRow["cmlSplitPercentTotal"] = organizationLocation.cmlSplitPercentTotal;
			dataRow["cmlState"] = organizationLocation.cmlState;
			dataRow["cmlSupplierPaymentTermID"] = organizationLocation.cmlSupplierPaymentTermID;
			dataRow["cmlSupplierShippingMethodID"] = organizationLocation.cmlSupplierShippingMethodID;
			dataRow["cmlTaxExemptNumber"] = organizationLocation.cmlTaxExemptNumber;
			dataRow["cmlTradingName"] = organizationLocation.cmlTradingName ?? dataRow["cmlTradingName"];
			dataRow["cmlUps3rdPartyLocationID"] = organizationLocation.cmlUps3rdPartyLocationID;
			dataRow["cmlUps3rdPartyOrganizationID"] = organizationLocation.cmlUps3rdPartyOrganizationID;
			dataRow["cmlUpsAcctNumber"] = organizationLocation.cmlUpsAcctNumber;
			dataRow["cmlUpsBillingOption"] = organizationLocation.cmlUpsBillingOption;
			dataRow["cmlUpsWsBillingOption"] = organizationLocation.cmlUpsWsBillingOption;
			if (organizationLocation.CustomFields != null && organizationLocation.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in organizationLocation.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the OrganizationLocation [{organizationLocation.cmlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the OrganizationLocation [{organizationLocation.cmlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
