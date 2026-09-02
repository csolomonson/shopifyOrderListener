using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories;
using M1.API.Repositories.Core;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Models.EDI;

public abstract class EDIBaseModel : APIBaseModel, IEDIBaseModel, IAPIBaseModel, IDisposable
{
	public ISalesOrderRepository SalesOrderRepository { get; set; }

	public IOrganizationRepository OrganizationRepository { get; set; }

	public IPartRepository PartRepository { get; set; }

	public IInvoiceRepository InvoiceRepository { get; set; }

	public IShipmentRepository ShipmentRepository { get; set; }

	public EDIBaseModel(APIClientContext apiClientContext)
	{
		base.ApiClientContext = apiClientContext;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}

	public Task<EDIOrganizationLocationAddressDto> GetOrganizationNameAndAddress_ForLocationId(M1Database database, string organizationId, string locationId, bool billToLocation, bool shipToLocation)
	{
		List<OrganizationLocationDto> source = new List<OrganizationLocationDto>();
		OrganizationLocationDto organizationLocationDto = new OrganizationLocationDto();
		new EDIOrganizationLocationAddressDto();
		using (OrganizationRepository organizationRepository = new OrganizationRepository(database))
		{
			source = organizationRepository.GetOrganizationLocationsInfor(organizationId.Trim()).Result.ToList();
		}
		organizationLocationDto = ((string.IsNullOrWhiteSpace(organizationId.Trim()) || !string.IsNullOrWhiteSpace(locationId.Trim())) ? source.Where((OrganizationLocationDto o) => o.LocationID.Equals(locationId.Trim(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() : ((!billToLocation) ? source.Where((OrganizationLocationDto o) => o.ShipLocation = true).FirstOrDefault() : source.Where((OrganizationLocationDto o) => o.ARInvoiceLocation = true).FirstOrDefault()));
		return Task.FromResult(new EDIOrganizationLocationAddressDto
		{
			OrganizationID = organizationLocationDto.OrganizationID,
			LocationName = organizationLocationDto.Name,
			ContactID = (shipToLocation ? organizationLocationDto.ShipContactID : organizationLocationDto.ARInvoiceContactID),
			AddressLine = organizationLocationDto.AddressLine1,
			Country = organizationLocationDto.Country,
			State = organizationLocationDto.State,
			City = organizationLocationDto.City,
			PostCode = organizationLocationDto.PostCode,
			EDILocationID = organizationLocationDto.EDILocationID,
			PhoneNumber = organizationLocationDto.PhoneNumber,
			M1LocationID = organizationLocationDto.LocationID
		});
	}

	public Task<OrganizationLocationAddressDto> GetShipFromOrganizationNameAndAddress(IAPIBaseRepository apiRepository, string plantId)
	{
		OrganizationLocationAddressDto organizationLocationAddressDto = null;
		if (!string.IsNullOrWhiteSpace(plantId))
		{
			organizationLocationAddressDto = GetM1CompanyAddressFromPlant(apiRepository, plantId).Result;
		}
		if (organizationLocationAddressDto == null)
		{
			organizationLocationAddressDto = GetM1CompanyAddressFromDP(apiRepository.M1database).Result;
		}
		if (organizationLocationAddressDto == null)
		{
			organizationLocationAddressDto = new OrganizationLocationAddressDto();
		}
		return Task.FromResult(organizationLocationAddressDto);
	}

	public Task<EDIOrganizationLocationAddressDto> ChangeLocationContact(EDIOrganizationLocationAddressDto organizationLocation, string contactID)
	{
		organizationLocation.ContactID = contactID;
		return Task.FromResult(organizationLocation);
	}

	public Task<IList<SalesOrderSalespeopleDto>> GetSalesPeopleInfo(IAPIBaseRepository apiRepository, string customerOrganizationID, string shipLocationID)
	{
		IList<SalesOrderSalespeopleDto> list = new List<SalesOrderSalespeopleDto>();
		SalesOrderSalespeopleDto salesOrderSalespeopleDto = null;
		foreach (OrganizationLocationSalespeopleDto item in ((OrganizationRepository)apiRepository).GetOrganizationLocationSalesPeopleInfo_ForLocationId(customerOrganizationID, shipLocationID).Result)
		{
			salesOrderSalespeopleDto = new SalesOrderSalespeopleDto
			{
				SalesEmployeeID = item.SalesEmployeeID,
				SequenceID = item.SequenceID,
				Percent = item.Percent
			};
			list.Add(salesOrderSalespeopleDto);
		}
		return Task.FromResult(list ?? new List<SalesOrderSalespeopleDto>());
	}

	public OrganizationInformationDto GetCustomerOrganizationData(IAPIBaseRepository apiRepository, GetOrganizationDataParam parameter)
	{
		OrganizationInformationDto organizationInformationDto = new OrganizationInformationDto();
		OrganizationLocationDto organizationLocationDto = new OrganizationLocationDto();
		OrganizationLocationDto organizationLocationDto2 = new OrganizationLocationDto();
		OrganizationDto result = ((OrganizationRepository)apiRepository).GetOrganizationInforAll(parameter.CustomerOrganizationID).Result;
		if (result != null && !string.IsNullOrEmpty(result.OrganizationID))
		{
			organizationLocationDto = ((!parameter.ARInvoiceLocationID.IsM1ID) ? result.OrganizationLocations.Where((OrganizationLocationDto x) => x.EDILocationID.Equals(parameter.ARInvoiceLocationID.Value, StringComparison.CurrentCultureIgnoreCase) && x.ARInvoiceLocation).FirstOrDefault() : result.OrganizationLocations.Where((OrganizationLocationDto x) => x.LocationID.Equals(parameter.ARInvoiceLocationID.Value, StringComparison.CurrentCultureIgnoreCase) && x.ARInvoiceLocation).FirstOrDefault());
			if (organizationLocationDto == null || string.IsNullOrWhiteSpace(organizationLocationDto.OrganizationID))
			{
				organizationInformationDto.WarningsList.Add("ARInvoiceLocationID [" + parameter.ARInvoiceLocationID.Value + "] in sales order [" + parameter.SalesOrderID + "]/customer PO [" + parameter.CustomerPO + "] is invalid.ARInvoiceLocationID value in sales order will not be updated.");
			}
			organizationLocationDto2 = ((!parameter.ShipLocationID.IsM1ID) ? result.OrganizationLocations.Where((OrganizationLocationDto x) => x.EDILocationID.Equals(parameter.ShipLocationID.Value, StringComparison.CurrentCultureIgnoreCase) && x.ShipLocation).FirstOrDefault() : result.OrganizationLocations.Where((OrganizationLocationDto x) => x.LocationID.Equals(parameter.ShipLocationID.Value, StringComparison.CurrentCultureIgnoreCase) && x.ShipLocation).FirstOrDefault());
			if (organizationLocationDto2 == null || string.IsNullOrWhiteSpace(organizationLocationDto2.OrganizationID))
			{
				organizationInformationDto.WarningsList.Add("ShipLocationID [" + parameter.ShipLocationID.Value + "] in sales order [" + parameter.SalesOrderID + "]/customer PO [" + parameter.CustomerPO + "] is invalid.ShipLocationID value in sales order will not be updated.");
			}
			organizationInformationDto.CustomerOrganizationID = parameter.CustomerOrganizationID;
			organizationInformationDto.ShipOrganizationID = parameter.CustomerOrganizationID;
			organizationInformationDto.ShipLocationID = organizationLocationDto2?.LocationID ?? string.Empty;
			organizationInformationDto.ShipContactID = organizationLocationDto2?.ShipContactID ?? string.Empty;
			organizationInformationDto.ARInvoiceLocationID = organizationLocationDto?.LocationID ?? string.Empty;
			organizationInformationDto.ARInvoiceContactID = organizationLocationDto?.ShipContactID ?? string.Empty;
			organizationInformationDto.PaymentTermsID = result.CustomerPaymentTermsID ?? string.Empty;
			if (string.IsNullOrWhiteSpace(result.CurrencyRateID))
			{
				organizationInformationDto.WarningsList.Add("CurrencyRate has not been set for the CustomerOrganizationID [" + parameter.CustomerOrganizationID + "] in sales order [" + parameter.SalesOrderID + "]/customer PO [" + parameter.CustomerPO + "].Home currency rate id [" + base.ApiClientContext?.Database.HomeCurrencyID + "] was taken.");
				organizationInformationDto.CurrencyRateID = base.ApiClientContext?.Database.HomeCurrencyID ?? string.Empty;
			}
			else
			{
				organizationInformationDto.CurrencyRateID = result.CurrencyRateID ?? string.Empty;
			}
			organizationInformationDto.TaxCodeID = result.CustomerTaxCodeID ?? string.Empty;
			organizationInformationDto.ShipLocation = result.OrganizationLocations.Where((OrganizationLocationDto x) => x.LocationID.Equals(organizationInformationDto.ShipLocationID, StringComparison.CurrentCultureIgnoreCase) && x.ShipLocation).FirstOrDefault();
			organizationInformationDto.ARInvoiceLocation = result.OrganizationLocations.Where((OrganizationLocationDto x) => x.LocationID.Equals(organizationInformationDto.ARInvoiceLocationID, StringComparison.CurrentCultureIgnoreCase) && x.ARInvoiceLocation).FirstOrDefault();
			organizationInformationDto.ShipLocationSalesPeople = GetSalesPeopleInfo((OrganizationRepository)apiRepository, organizationInformationDto.CustomerOrganizationID, organizationInformationDto.ShipLocationID).Result;
			organizationInformationDto.ShippingPaymentTypeID = result.CustomerPaymentTermsID ?? string.Empty;
		}
		else
		{
			organizationInformationDto.ErrorsList.Add("CustomerOrganizationID [" + parameter.CustomerOrganizationID + "] in sales order [" + parameter.SalesOrderID + "]/customer PO [" + parameter.CustomerPO + "] is invalid.");
		}
		return organizationInformationDto;
	}
}
