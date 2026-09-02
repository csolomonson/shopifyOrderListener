using System;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.EDI;

namespace M1.API.Models.EDI;

public interface IEDIShipmentModel : IEDIBaseModel, IAPIBaseModel, IDisposable
{
	Task<EDI856ASNCollectionDto> Process_AllUnmapped(int page, int pagesize);

	Task<APIValidationInfoDto> ValidateRequest_SetEDIFlag(EDI856ASNsIN ediShipments);

	Task<APIValidationInfoDto> Process_SetEDIFlag(EDI856ASNsIN ediShipments);
}
