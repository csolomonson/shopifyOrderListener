using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core;

public class OrganizationLocationRepository : APIBaseRepository, IOrganizationLocationRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] orgLocationFields = new string[55]
	{
		"cmlOrganizationID", "cmlLocationID", "cmlName", "cmlAddressLine1", "cmlAddressLine2", "cmlAddressLine3", "cmlCity", "cmlCounty", "cmlState", "cmlPostCode",
		"cmlCountry", "cmlPhoneNumber", "cmlEMailAddress", "cmlQuoteLocation", "cmlQuoteContactID", "cmlShipLocation", "cmlShipContactID", "cmlARInvoiceLocation", "cmlARInvoiceContactID", "cmlPurchaseLocation",
		"cmlPurchaseContactID", "cmlAPInvoiceLocation", "cmlAPInvoiceContactID", "cmlCustomerTaxable", "cmlCustomerTaxCodeID", "cmlCustomerSecondTaxCodeID", "cmlCustomerShippingMethodID", "cmlCustomerShipPaymentTypeID", "cmlTaxExemptNumber", "cmlNonTaxReasonID",
		"cmlCustomerPaymentTermID", "cmlCurrencyRateID", "cmlSupplierPaymentTermID", "cmlSupplierShippingMethodID", "cmlInactive", "cmlInactiveDate", "cmlCustomerShippingCarrier", "cmlUPSAcctNumber", "cmlUPSWSBillingOption", "cmlUPS3rdPartyOrganizationID",
		"cmlUPS3rdPartyLocationID", "cmlResidentialAddress", "cmlFedExAccountNumber", "cmlFedEx3rdPartyOrganizationID", "cmlFedexBillingOption", "cmlCreditCheckForLocation", "cmlCustomerCreditLimit", "cmlCreditHold", "cmlCountryCode", "cmlCreatedDate",
		"cmlCreatedBy", "cmlAvalaraUseCodes", "cmlCounty", "cmlUniqueID", "cmlRowVersion"
	};

	public OrganizationLocationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesOrganizationLocationExists(string organizationId, string organizationLocationId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmlOrganizationID|C", organizationId);
		base.filterList.Add("cmlLocationID|C", organizationLocationId);
		base.selectList.Add("cmlLocationID");
		return Task.FromResult(GetAsObject("OrganizationLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<OrganizationLocationInformationDto>> GetAllOrganizationLocations(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<OrganizationLocationInformationDto> collection = new List<OrganizationLocationInformationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(orgLocationFields);
		List<string> orderbyList = new List<string> { "cmlLocationID" };
		using (DataTable dataTable = GetAsDataTable("OrganizationLocations", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				OrganizationLocationInformationDto organizationLocationInformationDto = new OrganizationLocationInformationDto();
				organizationLocationInformationDto.OrganizationID = dataTable.Rows[i].Field<string>("cmlOrganizationID");
				organizationLocationInformationDto.LocationID = dataTable.Rows[i].Field<string>("cmlLocationID");
				organizationLocationInformationDto.Name = dataTable.Rows[i].Field<string>("cmlName");
				organizationLocationInformationDto.AddressLine1 = dataTable.Rows[i].Field<string>("cmlAddressLine1");
				organizationLocationInformationDto.AddressLine2 = dataTable.Rows[i].Field<string>("cmlAddressLine2");
				organizationLocationInformationDto.AddressLine3 = dataTable.Rows[i].Field<string>("cmlAddressLine3");
				organizationLocationInformationDto.City = dataTable.Rows[i].Field<string>("cmlCity");
				organizationLocationInformationDto.County = dataTable.Rows[i].Field<string>("cmlCounty");
				organizationLocationInformationDto.State = dataTable.Rows[i].Field<string>("cmlState");
				organizationLocationInformationDto.PostCode = dataTable.Rows[i].Field<string>("cmlPostCode");
				organizationLocationInformationDto.Country = dataTable.Rows[i].Field<string>("cmlCountry");
				organizationLocationInformationDto.PhoneNumber = dataTable.Rows[i].Field<string>("cmlPhoneNumber");
				organizationLocationInformationDto.EmailAddress = dataTable.Rows[i].Field<string>("cmlEmailAddress");
				organizationLocationInformationDto.QuoteLocation = dataTable.Rows[i].Field<bool>("cmlQuoteLocation");
				organizationLocationInformationDto.QuoteContactID = dataTable.Rows[i].Field<string>("cmlQuoteContactID");
				organizationLocationInformationDto.ShipLocation = dataTable.Rows[i].Field<bool>("cmlShipLocation");
				organizationLocationInformationDto.ShipContactID = dataTable.Rows[i].Field<string>("cmlShipContactID");
				organizationLocationInformationDto.ArInvoiceLocation = dataTable.Rows[i].Field<bool>("cmlArInvoiceLocation");
				organizationLocationInformationDto.ArInvoiceContactID = dataTable.Rows[i].Field<string>("cmlArInvoiceContactID");
				organizationLocationInformationDto.PurchaseLocation = dataTable.Rows[i].Field<bool>("cmlPurchaseLocation");
				organizationLocationInformationDto.PurchaseContactID = dataTable.Rows[i].Field<string>("cmlPurchaseContactID");
				organizationLocationInformationDto.ApInvoiceLocation = dataTable.Rows[i].Field<bool>("cmlApInvoiceLocation");
				organizationLocationInformationDto.ApInvoiceContactID = dataTable.Rows[i].Field<string>("cmlApInvoiceContactID");
				organizationLocationInformationDto.CustomerTaxable = dataTable.Rows[i].Field<bool>("cmlCustomerTaxable");
				organizationLocationInformationDto.CustomerTaxCodeID = dataTable.Rows[i].Field<string>("cmlCustomerTaxCodeID");
				organizationLocationInformationDto.CustomerSecondTaxCodeID = dataTable.Rows[i].Field<string>("cmlCustomerSecondTaxCodeID");
				organizationLocationInformationDto.CustomerShippingMethodID = dataTable.Rows[i].Field<string>("cmlCustomerShippingMethodID");
				organizationLocationInformationDto.CustomerShipPaymentTypeID = dataTable.Rows[i].Field<string>("cmlCustomerShipPaymentTypeID");
				organizationLocationInformationDto.TaxExemptNumber = dataTable.Rows[i].Field<string>("cmlTaxExemptNumber");
				organizationLocationInformationDto.NonTaxReasonID = dataTable.Rows[i].Field<string>("cmlNonTaxReasonID");
				organizationLocationInformationDto.CustomerPaymentTermID = dataTable.Rows[i].Field<string>("cmlCustomerPaymentTermID");
				organizationLocationInformationDto.CurrencyRateID = dataTable.Rows[i].Field<string>("cmlCurrencyRateID");
				organizationLocationInformationDto.SupplierPaymentTermID = dataTable.Rows[i].Field<string>("cmlSupplierPaymentTermID");
				organizationLocationInformationDto.SupplierShippingMethodID = dataTable.Rows[i].Field<string>("cmlSupplierShippingMethodID");
				organizationLocationInformationDto.Inactive = dataTable.Rows[i].Field<bool>("cmlInactive");
				organizationLocationInformationDto.InactiveDate = dataTable.Rows[i].Field<DateTime?>("cmlInactiveDate");
				organizationLocationInformationDto.CustomerShippingCarrier = dataTable.Rows[i].Field<string>("cmlCustomerShippingCarrier");
				organizationLocationInformationDto.UpsAcctNumber = dataTable.Rows[i].Field<string>("cmlUpsAcctNumber");
				organizationLocationInformationDto.UpsWsBillingOption = dataTable.Rows[i].Field<string>("cmlUpsWsBillingOption");
				organizationLocationInformationDto.Ups3rdPartyLocationID = dataTable.Rows[i].Field<string>("cmlUps3rdPartyLocationID");
				organizationLocationInformationDto.ResidentialAddress = dataTable.Rows[i].Field<bool>("cmlResidentialAddress");
				organizationLocationInformationDto.FedExAccountNumber = dataTable.Rows[i].Field<string>("cmlFedExAccountNumber");
				organizationLocationInformationDto.FedEx3rdPartyOrganizationID = dataTable.Rows[i].Field<string>("cmlFedEx3rdPartyOrganizationID");
				organizationLocationInformationDto.FedExBillingOption = dataTable.Rows[i].Field<string>("cmlFedExBillingOption");
				organizationLocationInformationDto.CreditCheckForLocation = dataTable.Rows[i].Field<bool>("cmlCreditCheckForLocation");
				organizationLocationInformationDto.CustomerCreditLimit = dataTable.Rows[i].Field<decimal>("cmlCustomerCreditLimit");
				organizationLocationInformationDto.CreditHold = dataTable.Rows[i].Field<bool>("cmlCreditHold");
				organizationLocationInformationDto.CountryCode = dataTable.Rows[i].Field<string>("cmlCountryCode");
				organizationLocationInformationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("cmlCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("cmlCreatedDate"));
				organizationLocationInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("cmlCreatedBy");
				organizationLocationInformationDto.AvalaraUseCodes = dataTable.Rows[i].Field<string>("cmlAvalaraUseCodes");
				organizationLocationInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("cmlUniqueID");
				organizationLocationInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("cmlRowVersion");
				collection.Add(organizationLocationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<OrganizationLocationInformationDto> GetOrganizationLocation(string organizationId, string organizationLocationId)
	{
		OrganizationLocationInformationDto organizationLocationInformationDto = new OrganizationLocationInformationDto();
		InitializeParameterLists();
		base.selectList.AddRange(orgLocationFields);
		base.filterList.Add(Guid.TryParse(organizationLocationId, out var _) ? "cmlUniqueID|C" : "cmlLocationID|C", organizationLocationId);
		base.filterList.Add("cmlOrganizationID|C", organizationId);
		using (DataTable dataTable = GetAsDataTable("OrganizationLocations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(organizationLocationInformationDto);
			}
			organizationLocationInformationDto.OrganizationID = dataTable.Rows[0].Field<string>("cmlOrganizationID");
			organizationLocationInformationDto.LocationID = dataTable.Rows[0].Field<string>("cmlLocationID");
			organizationLocationInformationDto.Name = dataTable.Rows[0].Field<string>("cmlName");
			organizationLocationInformationDto.AddressLine1 = dataTable.Rows[0].Field<string>("cmlAddressLine1");
			organizationLocationInformationDto.AddressLine2 = dataTable.Rows[0].Field<string>("cmlAddressLine2");
			organizationLocationInformationDto.AddressLine3 = dataTable.Rows[0].Field<string>("cmlAddressLine3");
			organizationLocationInformationDto.City = dataTable.Rows[0].Field<string>("cmlCity");
			organizationLocationInformationDto.County = dataTable.Rows[0].Field<string>("cmlCounty");
			organizationLocationInformationDto.State = dataTable.Rows[0].Field<string>("cmlState");
			organizationLocationInformationDto.PostCode = dataTable.Rows[0].Field<string>("cmlPostCode");
			organizationLocationInformationDto.Country = dataTable.Rows[0].Field<string>("cmlCountry");
			organizationLocationInformationDto.PhoneNumber = dataTable.Rows[0].Field<string>("cmlPhoneNumber");
			organizationLocationInformationDto.EmailAddress = dataTable.Rows[0].Field<string>("cmlEmailAddress");
			organizationLocationInformationDto.QuoteLocation = dataTable.Rows[0].Field<bool>("cmlQuoteLocation");
			organizationLocationInformationDto.QuoteContactID = dataTable.Rows[0].Field<string>("cmlQuoteContactID");
			organizationLocationInformationDto.ShipLocation = dataTable.Rows[0].Field<bool>("cmlShipLocation");
			organizationLocationInformationDto.ShipContactID = dataTable.Rows[0].Field<string>("cmlShipContactID");
			organizationLocationInformationDto.ArInvoiceLocation = dataTable.Rows[0].Field<bool>("cmlArInvoiceLocation");
			organizationLocationInformationDto.ArInvoiceContactID = dataTable.Rows[0].Field<string>("cmlArInvoiceContactID");
			organizationLocationInformationDto.PurchaseLocation = dataTable.Rows[0].Field<bool>("cmlPurchaseLocation");
			organizationLocationInformationDto.PurchaseContactID = dataTable.Rows[0].Field<string>("cmlPurchaseContactID");
			organizationLocationInformationDto.ApInvoiceLocation = dataTable.Rows[0].Field<bool>("cmlApInvoiceLocation");
			organizationLocationInformationDto.ApInvoiceContactID = dataTable.Rows[0].Field<string>("cmlApInvoiceContactID");
			organizationLocationInformationDto.CustomerTaxable = dataTable.Rows[0].Field<bool>("cmlCustomerTaxable");
			organizationLocationInformationDto.CustomerTaxCodeID = dataTable.Rows[0].Field<string>("cmlCustomerTaxCodeID");
			organizationLocationInformationDto.CustomerSecondTaxCodeID = dataTable.Rows[0].Field<string>("cmlCustomerSecondTaxCodeID");
			organizationLocationInformationDto.CustomerShippingMethodID = dataTable.Rows[0].Field<string>("cmlCustomerShippingMethodID");
			organizationLocationInformationDto.CustomerShipPaymentTypeID = dataTable.Rows[0].Field<string>("cmlCustomerShipPaymentTypeID");
			organizationLocationInformationDto.TaxExemptNumber = dataTable.Rows[0].Field<string>("cmlTaxExemptNumber");
			organizationLocationInformationDto.NonTaxReasonID = dataTable.Rows[0].Field<string>("cmlNonTaxReasonID");
			organizationLocationInformationDto.CustomerPaymentTermID = dataTable.Rows[0].Field<string>("cmlCustomerPaymentTermID");
			organizationLocationInformationDto.CurrencyRateID = dataTable.Rows[0].Field<string>("cmlCurrencyRateID");
			organizationLocationInformationDto.SupplierPaymentTermID = dataTable.Rows[0].Field<string>("cmlSupplierPaymentTermID");
			organizationLocationInformationDto.SupplierShippingMethodID = dataTable.Rows[0].Field<string>("cmlSupplierShippingMethodID");
			organizationLocationInformationDto.Inactive = dataTable.Rows[0].Field<bool>("cmlInactive");
			organizationLocationInformationDto.InactiveDate = dataTable.Rows[0].Field<DateTime?>("cmlInactiveDate");
			organizationLocationInformationDto.CustomerShippingCarrier = dataTable.Rows[0].Field<string>("cmlCustomerShippingCarrier");
			organizationLocationInformationDto.UpsAcctNumber = dataTable.Rows[0].Field<string>("cmlUpsAcctNumber");
			organizationLocationInformationDto.UpsWsBillingOption = dataTable.Rows[0].Field<string>("cmlUpsWsBillingOption");
			organizationLocationInformationDto.Ups3rdPartyLocationID = dataTable.Rows[0].Field<string>("cmlUps3rdPartyLocationID");
			organizationLocationInformationDto.ResidentialAddress = dataTable.Rows[0].Field<bool>("cmlResidentialAddress");
			organizationLocationInformationDto.FedExAccountNumber = dataTable.Rows[0].Field<string>("cmlFedExAccountNumber");
			organizationLocationInformationDto.FedEx3rdPartyOrganizationID = dataTable.Rows[0].Field<string>("cmlFedEx3rdPartyOrganizationID");
			organizationLocationInformationDto.FedExBillingOption = dataTable.Rows[0].Field<string>("cmlFedExBillingOption");
			organizationLocationInformationDto.CreditCheckForLocation = dataTable.Rows[0].Field<bool>("cmlCreditCheckForLocation");
			organizationLocationInformationDto.CustomerCreditLimit = dataTable.Rows[0].Field<decimal>("cmlCustomerCreditLimit");
			organizationLocationInformationDto.CreditHold = dataTable.Rows[0].Field<bool>("cmlCreditHold");
			organizationLocationInformationDto.CountryCode = dataTable.Rows[0].Field<string>("cmlCountryCode");
			organizationLocationInformationDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("cmlCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("cmlCreatedDate"));
			organizationLocationInformationDto.CreatedBy = dataTable.Rows[0].Field<string>("cmlCreatedBy");
			organizationLocationInformationDto.AvalaraUseCodes = dataTable.Rows[0].Field<string>("cmlAvalaraUseCodes");
			organizationLocationInformationDto.UniqueID = dataTable.Rows[0].Field<Guid>("cmlUniqueID");
			organizationLocationInformationDto.RowVersion = dataTable.Rows[0].Field<byte[]>("cmlRowVersion");
		}
		return Task.FromResult(organizationLocationInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationLocation(BOMOrganizationLocationDto organizationLocation)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("cmlOrganizationID = " + M1Util.ConvertToLinq(organizationLocation.OrganizationID) + "And cmlLocationID = " + M1Util.ConvertToLinq(organizationLocation.LocationID));
			m1BindingSource.DataSourceTable = "OrganizationLocations";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["cmlOrganizationID"] = organizationLocation.OrganizationID;
				dataRow["cmlLocationID"] = organizationLocation.LocationID;
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["cmlName"] = organizationLocation.Name ?? dataRow["cmlName"];
			dataRow["cmlAddressLine1"] = organizationLocation.AddressLine1 ?? dataRow["cmlAddressLine1"];
			dataRow["cmlAddressLine2"] = organizationLocation.AddressLine2 ?? dataRow["cmlAddressLine2"];
			dataRow["cmlAddressLine3"] = organizationLocation.AddressLine3 ?? dataRow["cmlAddressLine3"];
			dataRow["cmlCity"] = organizationLocation.City ?? dataRow["cmlCity"];
			dataRow["cmlCounty"] = organizationLocation.County ?? dataRow["cmlCounty"];
			dataRow["cmlState"] = organizationLocation.State ?? dataRow["cmlState"];
			dataRow["cmlCountry"] = organizationLocation.Country ?? dataRow["cmlCountry"];
			dataRow["cmlPostCode"] = organizationLocation.PostCode ?? dataRow["cmlPostCode"];
			dataRow["cmlPhoneNumber"] = organizationLocation.PhoneNumber ?? dataRow["cmlPhoneNumber"];
			dataRow["cmlEmailAddress"] = organizationLocation.EmailAddress ?? dataRow["cmlEmailAddress"];
			if (!organizationLocation.QuoteLocation)
			{
				dataRow["cmlQuoteLocation"] = organizationLocation.QuoteLocation;
			}
			if (!organizationLocation.ShipLocation)
			{
				dataRow["cmlShipLocation"] = organizationLocation.ShipLocation;
			}
			if (!organizationLocation.ArInvoiceLocation)
			{
				dataRow["cmlArInvoiceLocation"] = organizationLocation.ArInvoiceLocation;
			}
			if (!organizationLocation.CustomerTaxable)
			{
				dataRow["cmlCustomerTaxable"] = organizationLocation.CustomerTaxable;
			}
			dataRow["cmlCustomerTaxCodeID"] = organizationLocation.CustomerTaxCodeID ?? dataRow["cmlCustomerTaxCodeID"];
			dataRow["cmlCustomerSecondTaxCodeID"] = organizationLocation.CustomerSecondTaxCodeID ?? dataRow["cmlCustomerSecondTaxCodeID"];
			dataRow["cmlCustomerShippingMethodID"] = organizationLocation.CustomerShippingMethodID ?? dataRow["cmlCustomerShippingMethodID"];
			dataRow["cmlCustomerShipPaymentTypeID"] = organizationLocation.CustomerShipPaymentTypeID ?? dataRow["cmlCustomerShipPaymentTypeID"];
			dataRow["cmlCustomerShippingCarrier"] = organizationLocation.CustomerShippingCarrier ?? dataRow["cmlCustomerShippingCarrier"];
			dataRow["cmlUpsAcctNumber"] = organizationLocation.UpsAcctNumber ?? dataRow["cmlUpsAcctNumber"];
			dataRow["cmlFedExAccountNumber"] = organizationLocation.FedExAccountNumber ?? dataRow["cmlFedExAccountNumber"];
			dataRow["cmlCountryCode"] = organizationLocation.CountryCode ?? dataRow["cmlCountryCode"];
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the OrganizationLocation [" + organizationLocation.LocationID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}
}
