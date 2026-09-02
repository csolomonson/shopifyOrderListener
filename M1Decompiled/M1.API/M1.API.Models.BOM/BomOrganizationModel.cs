using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.Repositories.Core;

namespace M1.API.Models.BOM;

public class BomOrganizationModel : BOMBaseModel, IBomOrganizationModel, IBOMBaseModel, IAPIBaseModel, IDisposable
{
	public Task<APIValidationInfoDto> ValidateRequest_GetOrganization(string organizationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IOrganizationRepository organizationRepository = (base.OrganizationRepository = new OrganizationRepository(base.ApiClientContext));
		using (organizationRepository)
		{
			if (!base.OrganizationRepository.DoesOrganizationExists(organizationId).Result)
			{
				base.ErrorsList.Add("Organization [" + organizationId + "] is invalid");
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return Task.FromResult(new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode));
	}

	public async Task<BOMResponseMessageDto<BomOrganizationDto>> Process_GetOrganization(string organizationId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		BomOrganizationDto returnObject = new BomOrganizationDto();
		BOMResponseMessageDto<BomOrganizationDto> result;
		try
		{
			IOrganizationRepository organizationRepository = (base.OrganizationRepository = new OrganizationRepository(base.ApiClientContext));
			using (organizationRepository)
			{
				returnObject = base.OrganizationRepository.GetOrganizationInfo(organizationId).Result;
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the organization [" + organizationId + "]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<BomOrganizationDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = returnObject
			};
		}
		return result;
	}

	public async Task<BOMResponseMessageDto<IList<BomOrganizationDto>>> Process_GetAllOrganizations(int pageSize, int pageNumber)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IList<BomOrganizationDto> list = new List<BomOrganizationDto>();
		BOMResponseMessageDto<IList<BomOrganizationDto>> result;
		try
		{
			IOrganizationRepository organizationRepository = (base.OrganizationRepository = new OrganizationRepository(base.ApiClientContext));
			using (organizationRepository)
			{
				foreach (OrganizationDto item2 in base.OrganizationRepository.GetAllOrganizationsInfo(pageSize, pageNumber).Result)
				{
					BomOrganizationDto item = new BomOrganizationDto
					{
						OrganizationID = item2.OrganizationID,
						Name = item2.Name
					};
					list.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpValidationStatusCode = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Organizations]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpValidationStatusCode);
			result = new BOMResponseMessageDto<IList<BomOrganizationDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = list
			};
		}
		return result;
	}
}
