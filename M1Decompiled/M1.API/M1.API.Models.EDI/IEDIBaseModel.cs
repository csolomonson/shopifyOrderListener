using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories;
using M1.API.Repositories.Core;
using M1.Core;

namespace M1.API.Models.EDI;

public interface IEDIBaseModel : IAPIBaseModel, IDisposable
{
	ISalesOrderRepository SalesOrderRepository { get; set; }

	IOrganizationRepository OrganizationRepository { get; set; }

	IPartRepository PartRepository { get; set; }

	IInvoiceRepository InvoiceRepository { get; set; }

	IShipmentRepository ShipmentRepository { get; set; }

	Task<EDIOrganizationLocationAddressDto> GetOrganizationNameAndAddress_ForLocationId(M1Database database, string organizationId, string locationId, bool billToLocation, bool shipToLocation);

	Task<OrganizationLocationAddressDto> GetShipFromOrganizationNameAndAddress(IAPIBaseRepository apiRepository, string plantId);

	OrganizationInformationDto GetCustomerOrganizationData(IAPIBaseRepository apiRepository, GetOrganizationDataParam parameter);

	Task<IList<SalesOrderSalespeopleDto>> GetSalesPeopleInfo(IAPIBaseRepository apiRepository, string customerOrganizationID, string shipLocationID);

	Task<EDIOrganizationLocationAddressDto> ChangeLocationContact(EDIOrganizationLocationAddressDto organizationLocation, string contactID);
}
