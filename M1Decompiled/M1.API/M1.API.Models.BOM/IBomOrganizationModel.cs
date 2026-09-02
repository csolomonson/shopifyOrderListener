using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;

namespace M1.API.Models.BOM;

public interface IBomOrganizationModel : IBOMBaseModel, IAPIBaseModel, IDisposable
{
	Task<APIValidationInfoDto> ValidateRequest_GetOrganization(string organizationId);

	Task<BOMResponseMessageDto<BomOrganizationDto>> Process_GetOrganization(string organizationId);

	Task<BOMResponseMessageDto<IList<BomOrganizationDto>>> Process_GetAllOrganizations(int pageSize, int pageNumber);
}
