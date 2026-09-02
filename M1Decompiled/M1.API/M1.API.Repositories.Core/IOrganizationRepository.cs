using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;

namespace M1.API.Repositories.Core;

public interface IOrganizationRepository : IAPIBaseRepository, IDisposable
{
	Task<bool> DoesPlantExists(string plantId);

	Task<bool> DoesOrganizationExists(string organizationId);

	Task<bool> DoesSupplierOrganizationExists(string organizationId);

	Task<bool> DoesSupplierPurchaseLocationExists(string supplierOrganizationID, string purchaseLocationID);

	Task<OrganizationDto> GetOrganizationInforAll(string organizationId);

	Task<OrganizationDto> GetOrganizationHeaderInfor(string organizationId);

	Task<IList<OrganizationLocationDto>> GetOrganizationLocationsInfor(string organizationId);

	Task<OrganizationLocationDto> GetOrganizationLocationInfor(string organizationId, string locationId);

	Task<IList<OrganizationContactDto>> GetOrganizationContactsInfor(string organizationId, string orgLocationId);

	Task<OrganizationContactDto> GetOrganizationContactInfor(string organizationId, string orgLocationId, string orgContactId);

	Task<IList<OrganizationLocationSalespeopleDto>> GetOrganizationLocationSalesPeopleInfo_ForLocationId(string organizationId, string locationId);

	Task<IDictionary<byte, string>> GetTaxCodes_ForLocationId(string organizationId, string organizationLocationId);

	Task<OrganizationLocationAddressDto> GetM1CompanyAddressFromPlant(string plantId);

	Task<BomOrganizationDto> GetOrganizationInfo(string organizationId);

	Task<ICollection<OrganizationDto>> GetAllOrganizationsInfo(int? pageSize = null, int? pageNumber = null);
}
