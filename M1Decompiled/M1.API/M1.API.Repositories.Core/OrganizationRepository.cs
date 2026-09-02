using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class OrganizationRepository : APIBaseRepository, IOrganizationRepository, IAPIBaseRepository, IDisposable
{
	private readonly string GET_CHECK_ACTIVE_SUPPLIER = "SELECT cmoOrganizationID FROM Organizations WHERE cmoOrganizationID=@OrganizationID AND (cmoSupplierStatus=1 OR cmoSupplierStatus=2)";

	public OrganizationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public OrganizationRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<bool> DoesPlantExists(string plantId)
	{
		InitializeParameterLists();
		base.filterList.Add("xauPlantID|C", plantId);
		base.filterList.Add("xauInactive|C", false);
		base.selectList.Add("xauPlantID");
		return Task.FromResult(GetAsObject("Plants", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesOrganizationExists(string organizationId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmoOrganizationID|C", organizationId);
		base.filterList.Add("cmoSupplierStatus|<>", 3);
		base.selectList.Add("cmoOrganizationID");
		return Task.FromResult(GetAsObject("Organizations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesSupplierOrganizationExists(string organizationId)
	{
		InitializeParameterLists();
		base.filterList.Add("@OrganizationID", organizationId);
		using DataTable dataTable = GetAsDataTable(GET_CHECK_ACTIVE_SUPPLIER, base.filterList, null);
		return Task.FromResult(dataTable.Rows.Count > 0);
	}

	public Task<bool> DoesSupplierPurchaseLocationExists(string supplierOrganizationID, string purchaseLocationID)
	{
		InitializeParameterLists();
		base.filterList.Add("cmlOrganizationID|C", supplierOrganizationID);
		base.filterList.Add("cmlLocationID|C", purchaseLocationID);
		base.filterList.Add("cmlPurchaseLocation", 1);
		base.filterList.Add("cmlInactive", 0);
		base.selectList.Add("cmlOrganizationID");
		return Task.FromResult(GetAsObject("OrganizationLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<OrganizationDto> GetOrganizationInforAll(string organizationId)
	{
		OrganizationDto organizationDto = null;
		IList<OrganizationLocationDto> list = new List<OrganizationLocationDto>();
		organizationDto = GetOrganizationHeaderInfor(organizationId).Result;
		if (organizationDto != null && !string.IsNullOrEmpty(organizationDto.OrganizationID))
		{
			list = GetOrganizationLocationsInfor(organizationId).Result;
			foreach (OrganizationLocationDto item in list)
			{
				((List<OrganizationContactDto>)item.OrganizationContacts).AddRange(GetOrganizationContactsInfor(item.OrganizationID, item.LocationID).Result);
			}
			((List<OrganizationLocationDto>)organizationDto.OrganizationLocations).AddRange(new List<OrganizationLocationDto>(list));
		}
		return Task.FromResult(organizationDto ?? new OrganizationDto());
	}

	public Task<OrganizationDto> GetOrganizationHeaderInfor(string organizationId)
	{
		DataTable dataTable = null;
		new OrganizationDto();
		string item = "cmoOrganizationID, cmoName, cmoAddressLine1,cmoAddressLine2,cmoAddressLine3,cmoPhoneNumber,cmoCity, cmoState, cmoPostCode, cmoCountry, cmoShipContactID,cmoCountryCode,cmoEMailAddress,cmoARInvoiceContactID, cmoCustomerPaymentTermsID, cmoCustomerTaxCodeID, cmoCurrencyRateID,cmoCustomerShippingMethodID,cmoEDIIntegrated,cmoUPSAcctNumber,cmoUPSValidated";
		InitializeParameterLists();
		base.filterList.Add("cmoOrganizationID|C", organizationId);
		base.selectList.Add(item);
		dataTable = GetAsDataTable("Organizations", base.filterList, base.selectList, null, null);
		OrganizationDto organizationDto = (from locLine in dataTable.AsEnumerable()
			select new OrganizationDto
			{
				OrganizationID = locLine.Field<string>("cmoOrganizationID").Trim(),
				Name = locLine.Field<string>("cmoName").Trim(),
				ARInvoiceContactID = GetString(locLine.Field<string>("cmoARInvoiceContactID")),
				ShipContactID = GetString(locLine.Field<string>("cmoShipContactID")),
				AddressLine1 = GetString(locLine.Field<string>("cmoAddressLine1")),
				AddressLine2 = GetString(locLine.Field<string>("cmoAddressLine2")),
				AddressLine3 = GetString(locLine.Field<string>("cmoAddressLine3")),
				CountryCode = GetString(locLine.Field<string>("cmoCountryCode")),
				Country = GetString(locLine.Field<string>("cmoCountry")),
				State = GetString(locLine.Field<string>("cmoState")),
				City = GetString(locLine.Field<string>("cmoCity")),
				PostCode = GetString(locLine.Field<string>("cmoPostCode")),
				EMailAddress = GetString(locLine.Field<string>("cmoEMailAddress")),
				PhoneNumber = GetString(locLine.Field<string>("cmoPhoneNumber")),
				UPSAcctNumber = GetString(locLine.Field<string>("cmoUPSAcctNumber")),
				UPSValidated = locLine.Field<bool>("cmoUPSValidated"),
				CustomerShippingMethodID = GetString(locLine.Field<string>("cmoCustomerShippingMethodID")),
				CustomerPaymentTermsID = GetString(locLine.Field<string>("cmoCustomerPaymentTermsID")),
				CustomerTaxCodeID = GetString(locLine.Field<string>("cmoCustomerTaxCodeID")),
				CurrencyRateID = GetString(locLine.Field<string>("cmoCurrencyRateID")),
				EDIIntegrated = locLine.Field<bool>("cmoEDIIntegrated")
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationDto == null)
		{
			organizationDto = new OrganizationDto();
		}
		return Task.FromResult(organizationDto);
	}

	public Task<OrganizationLocationDto> GetOrganizationLocationInfor(string organizationId, string locationId)
	{
		DataTable dataTable = null;
		InitializeParameterLists();
		base.filterList.Add("cmlOrganizationID|C", organizationId);
		base.filterList.Add("cmlLocationID|C", locationId);
		dataTable = GetAsDataTable("OrganizationLocations", base.filterList, null, null, null);
		OrganizationLocationDto organizationLocationDto = (from locLine in dataTable.AsEnumerable()
			select new OrganizationLocationDto
			{
				OrganizationID = locLine.Field<string>("cmlOrganizationID").Trim(),
				LocationID = locLine.Field<string>("cmlLocationID").Trim(),
				Name = GetString(locLine.Field<string>("cmlName")),
				ARInvoiceLocation = locLine.Field<bool>("cmlARInvoiceLocation"),
				ARInvoiceContactID = GetString(locLine.Field<string>("cmlARInvoiceContactID")),
				ShipLocation = locLine.Field<bool>("cmlShipLocation"),
				ShipContactID = GetString(locLine.Field<string>("cmlShipContactID")),
				AddressLine1 = GetString(locLine.Field<string>("cmlAddressLine1")),
				AddressLine2 = GetString(locLine.Field<string>("cmlAddressLine2")),
				AddressLine3 = GetString(locLine.Field<string>("cmlAddressLine3")),
				CountryCode = GetString(locLine.Field<string>("cmlCountryCode")),
				Country = GetString(locLine.Field<string>("cmlCountry")),
				State = GetString(locLine.Field<string>("cmlState")),
				City = GetString(locLine.Field<string>("cmlCity")),
				PostCode = GetString(locLine.Field<string>("cmlPostCode")),
				EMailAddress = GetString(locLine.Field<string>("cmlEMailAddress")),
				Inactive = locLine.Field<bool>("cmlInactive"),
				PhoneNumber = GetString(locLine.Field<string>("cmlPhoneNumber")),
				UPSAcctNumber = GetString(locLine.Field<string>("cmlUPSAcctNumber")),
				UPSValidated = locLine.Field<bool>("cmlUPSValidated"),
				CustomerShippingMethodID = GetString(locLine.Field<string>("cmlCustomerShippingMethodID")),
				CustomerPaymentTermID = GetString(locLine.Field<string>("cmlCustomerPaymentTermID")),
				CustomerTaxCodeID = GetString(locLine.Field<string>("cmlCustomerTaxCodeID")),
				CustomerSecondTaxCodeID = GetString(locLine.Field<string>("cmlCustomerSecondTaxCodeID")),
				EDILocationID = GetString(locLine.Field<string>("cmlEDILocationID"))
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationLocationDto == null)
		{
			organizationLocationDto = new OrganizationLocationDto();
		}
		return Task.FromResult(organizationLocationDto);
	}

	public Task<IList<OrganizationLocationDto>> GetOrganizationLocationsInfor(string organizationId)
	{
		IList<OrganizationLocationDto> list = new List<OrganizationLocationDto>();
		InitializeParameterLists();
		base.filterList.Add("cmlOrganizationID|C", organizationId);
		using (DataTable source = GetAsDataTable("OrganizationLocations", base.filterList, null, null, null))
		{
			list = (from locLine in source.AsEnumerable()
				select new OrganizationLocationDto
				{
					OrganizationID = locLine.Field<string>("cmlOrganizationID").Trim(),
					LocationID = locLine.Field<string>("cmlLocationID").Trim(),
					Name = GetString(locLine.Field<string>("cmlName")),
					ARInvoiceLocation = locLine.Field<bool>("cmlARInvoiceLocation"),
					ARInvoiceContactID = GetString(locLine.Field<string>("cmlARInvoiceContactID")),
					ShipLocation = locLine.Field<bool>("cmlShipLocation"),
					ShipContactID = GetString(locLine.Field<string>("cmlShipContactID")),
					AddressLine1 = GetString(locLine.Field<string>("cmlAddressLine1")),
					AddressLine2 = GetString(locLine.Field<string>("cmlAddressLine2")),
					AddressLine3 = GetString(locLine.Field<string>("cmlAddressLine3")),
					CountryCode = GetString(locLine.Field<string>("cmlCountryCode")),
					Country = GetString(locLine.Field<string>("cmlCountry")),
					State = GetString(locLine.Field<string>("cmlState")),
					City = GetString(locLine.Field<string>("cmlCity")),
					PostCode = GetString(locLine.Field<string>("cmlPostCode")),
					EMailAddress = GetString(locLine.Field<string>("cmlEMailAddress")),
					Inactive = locLine.Field<bool>("cmlInactive"),
					PhoneNumber = GetString(locLine.Field<string>("cmlPhoneNumber")),
					UPSAcctNumber = GetString(locLine.Field<string>("cmlUPSAcctNumber")),
					UPSValidated = locLine.Field<bool>("cmlUPSValidated"),
					CustomerShippingMethodID = GetString(locLine.Field<string>("cmlCustomerShippingMethodID")),
					CustomerPaymentTermID = GetString(locLine.Field<string>("cmlCustomerPaymentTermID")),
					CustomerTaxCodeID = GetString(locLine.Field<string>("cmlCustomerTaxCodeID")),
					CustomerSecondTaxCodeID = GetString(locLine.Field<string>("cmlCustomerSecondTaxCodeID")),
					EDILocationID = GetString(locLine.Field<string>("cmlEDILocationID"))
				}).ToList();
		}
		return Task.FromResult(list ?? new List<OrganizationLocationDto>());
	}

	public Task<OrganizationContactDto> GetOrganizationContactInfor(string organizationId, string orgLocationId, string orgContactId)
	{
		DataTable dataTable = null;
		InitializeParameterLists();
		base.filterList.Add("cmcOrganizationID|C", organizationId);
		base.filterList.Add("cmcLocationID|C", orgLocationId);
		base.filterList.Add("cmcContactID|C", orgContactId);
		dataTable = GetAsDataTable("OrganizationContacts", base.filterList, null, null, null);
		OrganizationContactDto organizationContactDto = (from locLine in dataTable.AsEnumerable()
			select new OrganizationContactDto
			{
				OrganizationID = locLine.Field<string>("cmcOrganizationID").Trim(),
				LocationID = locLine.Field<string>("cmcLocationID").Trim(),
				ContactID = locLine.Field<string>("cmcContactID").Trim(),
				Name = GetString(locLine.Field<string>("cmcName")),
				EMailAddress = GetString(locLine.Field<string>("cmcEMailAddress")),
				Inactive = locLine.Field<bool>("cmcInactive"),
				MobileNumber = GetString(locLine.Field<string>("cmcMobileNumber")),
				PhoneNumber = GetString(locLine.Field<string>("cmcPhoneNumber"))
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationContactDto == null)
		{
			organizationContactDto = new OrganizationContactDto();
		}
		return Task.FromResult(organizationContactDto);
	}

	public Task<IList<OrganizationContactDto>> GetOrganizationContactsInfor(string organizationId, string orgLocationId)
	{
		IList<OrganizationContactDto> list = new List<OrganizationContactDto>();
		InitializeParameterLists();
		base.filterList.Add("cmcOrganizationID|C", organizationId);
		base.filterList.Add("cmcLocationID|C", orgLocationId);
		using (DataTable source = GetAsDataTable("OrganizationContacts", base.filterList, null, null, null))
		{
			list = (from locLine in source.AsEnumerable()
				select new OrganizationContactDto
				{
					OrganizationID = locLine.Field<string>("cmcOrganizationID").Trim(),
					LocationID = locLine.Field<string>("cmcLocationID").Trim(),
					ContactID = locLine.Field<string>("cmcContactID").Trim(),
					Name = GetString(locLine.Field<string>("cmcName")),
					EMailAddress = GetString(locLine.Field<string>("cmcEMailAddress")),
					Inactive = locLine.Field<bool>("cmcInactive"),
					MobileNumber = GetString(locLine.Field<string>("cmcMobileNumber")),
					PhoneNumber = GetString(locLine.Field<string>("cmcPhoneNumber"))
				}).ToList();
		}
		return Task.FromResult(list ?? new List<OrganizationContactDto>());
	}

	public Task<IList<OrganizationLocationSalespeopleDto>> GetOrganizationLocationSalesPeopleInfo_ForLocationId(string organizationId, string organizationLocationId)
	{
		IList<OrganizationLocationSalespeopleDto> list = new List<OrganizationLocationSalespeopleDto>();
		if (!string.IsNullOrEmpty(organizationId))
		{
			InitializeParameterLists();
			base.filterList.Add("cmkOrganizationID|C", organizationId);
			base.filterList.Add("cmkLocationID|C", organizationLocationId);
			base.selectList.AddRange(new string[3] { "cmkSequenceID", "cmkSalesEmployeeID", "cmkPercent" });
			using DataTable source = GetAsDataTable("OrganizationLocSalesPeople", base.filterList, base.selectList, null, null);
			list = (from locLine in source.AsEnumerable()
				select new OrganizationLocationSalespeopleDto
				{
					OrganizationID = organizationId.Trim(),
					LocationID = organizationLocationId.Trim(),
					SequenceID = locLine.Field<short>("cmkSequenceID"),
					SalesEmployeeID = locLine.Field<string>("cmkSalesEmployeeID").Trim(),
					Percent = locLine.Field<decimal>("cmkPercent")
				}).ToList();
		}
		return Task.FromResult(list ?? new List<OrganizationLocationSalespeopleDto>());
	}

	public Task<IDictionary<byte, string>> GetTaxCodes_ForLocationId(string organizationId, string organizationLocationId)
	{
		IDictionary<byte, string> dictionary = new Dictionary<byte, string>();
		InitializeParameterLists();
		base.filterList.Add("cmlOrganizationID|C", organizationId);
		base.filterList.Add("cmlLocationID|C", organizationLocationId);
		base.selectList.AddRange(new string[2] { "cmlCustomerTaxCodeID", "cmlCustomerSecondTaxCodeID" });
		using (DataTable dataTable = GetAsDataTable("OrganizationLocations", base.filterList, base.selectList, null, null))
		{
			if (dataTable.Rows.Count > 0)
			{
				dictionary.Add(1, dataTable.Rows[0].Field<string>("cmlCustomerTaxCodeID").Trim());
				dictionary.Add(2, dataTable.Rows[0].Field<string>("cmlCustomerSecondTaxCodeID").Trim());
			}
		}
		return Task.FromResult(dictionary);
	}

	public Task<OrganizationLocationAddressDto> GetM1CompanyAddressFromPlant(string plantId)
	{
		OrganizationLocationAddressDto organizationLocationAddressDto = null;
		InitializeParameterLists();
		base.filterList.Add("xauPlantID|C", plantId);
		base.selectList.AddRange(new string[7] { "xauName", "xauAddressLine1", "xauCity", "xauState", "xauPostCode", "xauCountry", "xauPhoneNumber" });
		using (DataTable dataTable = GetAsDataTable("Plants", base.filterList, base.selectList, null, null))
		{
			organizationLocationAddressDto = new OrganizationLocationAddressDto();
			DataRow row = dataTable.Rows[0];
			organizationLocationAddressDto.LocationName = row.Field<string>("xauName").Trim();
			organizationLocationAddressDto.AddressLine = row.Field<string>("xauAddressLine1").Trim();
			organizationLocationAddressDto.City = row.Field<string>("xauCity").Trim();
			organizationLocationAddressDto.State = row.Field<string>("xauState").Trim();
			organizationLocationAddressDto.PostCode = row.Field<string>("xauPostCode").Trim();
			organizationLocationAddressDto.Country = row.Field<string>("xauCountry").Trim();
			organizationLocationAddressDto.PhoneNumber = row.Field<string>("xauPhoneNumber").Trim();
		}
		return Task.FromResult(organizationLocationAddressDto);
	}

	public Task<BomOrganizationDto> GetOrganizationInfo(string organizationId)
	{
		BomOrganizationDto bomOrganizationDto = new BomOrganizationDto();
		InitializeParameterLists();
		base.selectList.AddRange(new string[2] { "cmoOrganizationID", "cmoName" });
		base.filterList.Add("cmoOrganizationId|C", organizationId);
		using (DataTable dataTable = GetAsDataTable("Organizations", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				bomOrganizationDto.OrganizationID = dataTable.Rows[0]["cmoOrganizationID"].ToString().Trim();
				bomOrganizationDto.Name = dataTable.Rows[0]["cmoName"].ToString();
			}
		}
		return Task.FromResult(bomOrganizationDto);
	}

	public Task<ICollection<OrganizationDto>> GetAllOrganizationsInfo(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<OrganizationDto> collection = new List<OrganizationDto>();
		InitializeParameterLists();
		string[] array = new string[2] { "cmoOrganizationID", "cmoName" };
		base.selectList.AddRange(array);
		List<string> orderbyList = new List<string> { "cmoOrganizationID" };
		using (DataTable dataTable = GetAsDataTable("Organizations", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				OrganizationDto item = new OrganizationDto
				{
					OrganizationID = dataTable.Rows[i][array[0]].ToString().Trim(),
					Name = dataTable.Rows[i][array[1]].ToString()
				};
				collection.Add(item);
			}
		}
		return Task.FromResult(collection);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
