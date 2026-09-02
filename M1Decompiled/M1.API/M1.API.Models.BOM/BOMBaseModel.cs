using System;
using System.Collections.Generic;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;
using M1.API.Repositories.Core.Inventory;
using M1.API.Repositories.Core.Job;
using M1.API.Utilities;

namespace M1.API.Models.BOM;

public abstract class BOMBaseModel : APIBaseModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public IPartRepository PartRepository { get; set; }

	public IOrganizationRepository OrganizationRepository { get; set; }

	public IJobRepository JobRepository { get; set; }

	public IPartBinDetailRepository PartBinDetailRepository { get; set; }

	public APIValidationInfoDto APIValidationIsTrueFunction()
	{
		return new APIValidationInfoDto();
	}

	public BOMBaseModel(APIClientContext clientContext)
	{
		base.ApiClientContext = clientContext;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}

	public BOMBaseModel()
	{
		base.ApiClientContext = null;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
	}
}
