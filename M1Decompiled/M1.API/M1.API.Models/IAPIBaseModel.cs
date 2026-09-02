using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.Custom;
using M1.API.DTOs.EDI;
using M1.API.Repositories;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Models;

public interface IAPIBaseModel : IDisposable
{
	APIClientContext ApiClientContext { get; set; }

	IList<string> ErrorsList { get; set; }

	IList<string> WarningsList { get; set; }

	Task<PartInformationDto> GetPartInfo(IAPIBaseRepository apiRepository, string currentSalesOrder, short currentSalesOrderLine, string orgPartId, string orgPartShortDescription, string partRevisionId, string customerOrganizationID);

	Task<OrganizationLocationAddressDto> GetM1CompanyAddressFromDP(M1Database database);
}
