using System;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Inventory;
using M1.API.Repositories.Core.Job;

namespace M1.API.Models.BOM;

public interface IBOMBaseModel : IAPIBaseModel, IDisposable
{
	IPartRepository PartRepository { get; set; }

	IOrganizationRepository OrganizationRepository { get; set; }

	IJobRepository JobRepository { get; set; }

	IPartBinDetailRepository PartBinDetailRepository { get; set; }

	APIValidationInfoDto APIValidationIsTrueFunction();
}
