using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFreightPackageLinkModel : ERPBaseModel, IERPFreightPackageLinkModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFreightPackageLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
		using (iERPFreightPackageLinkRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFreightPackageLinkRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFreightPackageLinkRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFreightPackageLinkRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFreightPackageLinkRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFreightPackageLink(Guid freightPackageLinkId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
		using (iERPFreightPackageLinkRepository)
		{
			if (!(await base.ERPFreightPackageLinkRepository.DoesFreightPackageLinkExist(freightPackageLinkId)))
			{
				errorsList.Add($"FreightPackageLink [{freightPackageLinkId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
		using (iERPFreightPackageLinkRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFreightPackageLinkDto>>> Process_GetAllFreightPackageLinks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFreightPackageLinkDto> allFreightPackageLinksDto = new List<ERPFreightPackageLinkDto>();
		ERPResponseMessageDto<IList<ERPFreightPackageLinkDto>> result;
		try
		{
			IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
			using (iERPFreightPackageLinkRepository)
			{
				foreach (ERPFreightPackageLinkInformationDto item2 in await base.ERPFreightPackageLinkRepository.GetAllFreightPackageLinks(pageSize, pageNumber, filter, orderBy))
				{
					ERPFreightPackageLinkDto item = new ERPFreightPackageLinkDto
					{
						fplCreatedBy = item2.fplCreatedBy,
						fplCreatedDate = item2.fplCreatedDate,
						fplUniqueID = item2.fplUniqueID,
						fplFreightPackageID = item2.fplFreightPackageID,
						fplFreightPackageLineID = item2.fplFreightPackageLineID,
						fplFreightShipmentID = item2.fplFreightShipmentID,
						fplRowVersion = item2.fplRowVersion,
						CustomFields = item2.CustomFields
					};
					allFreightPackageLinksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FreightPackageLinks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFreightPackageLinkDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFreightPackageLinksDto,
				RecordCount = allFreightPackageLinksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_GetFreightPackageLink(Guid freightPackageLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFreightPackageLinkDto freightPackageLinkDto = null;
		ERPResponseMessageDto<ERPFreightPackageLinkDto> result;
		try
		{
			IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
			using (iERPFreightPackageLinkRepository)
			{
				ERPFreightPackageLinkInformationDto eRPFreightPackageLinkInformationDto = await base.ERPFreightPackageLinkRepository.GetFreightPackageLink(freightPackageLinkId);
				freightPackageLinkDto = new ERPFreightPackageLinkDto
				{
					fplCreatedBy = eRPFreightPackageLinkInformationDto.fplCreatedBy,
					fplCreatedDate = eRPFreightPackageLinkInformationDto.fplCreatedDate,
					fplUniqueID = eRPFreightPackageLinkInformationDto.fplUniqueID,
					fplFreightPackageID = eRPFreightPackageLinkInformationDto.fplFreightPackageID,
					fplFreightPackageLineID = eRPFreightPackageLinkInformationDto.fplFreightPackageLineID,
					fplFreightShipmentID = eRPFreightPackageLinkInformationDto.fplFreightShipmentID,
					fplRowVersion = eRPFreightPackageLinkInformationDto.fplRowVersion,
					CustomFields = eRPFreightPackageLinkInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FreightPackageLinks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = freightPackageLinkDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_PutFreightPackageLink(ERPFreightPackageLinkDto freightPackageLink)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFreightPackageLinkDto createdObject = null;
		ERPResponseMessageDto<ERPFreightPackageLinkDto> result;
		try
		{
			IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
			using (iERPFreightPackageLinkRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFreightPackageLinkRepository.SaveFreightPackageLink(freightPackageLink);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFreightPackageLinkInformationDto eRPFreightPackageLinkInformationDto = await base.ERPFreightPackageLinkRepository.GetFreightPackageLink(freightPackageLink.fplUniqueID);
					createdObject = new ERPFreightPackageLinkDto
					{
						fplCreatedBy = eRPFreightPackageLinkInformationDto.fplCreatedBy,
						fplCreatedDate = eRPFreightPackageLinkInformationDto.fplCreatedDate,
						fplUniqueID = eRPFreightPackageLinkInformationDto.fplUniqueID,
						fplFreightPackageID = eRPFreightPackageLinkInformationDto.fplFreightPackageID,
						fplFreightPackageLineID = eRPFreightPackageLinkInformationDto.fplFreightPackageLineID,
						fplFreightShipmentID = eRPFreightPackageLinkInformationDto.fplFreightShipmentID,
						fplRowVersion = eRPFreightPackageLinkInformationDto.fplRowVersion,
						CustomFields = eRPFreightPackageLinkInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing FreightPackageLink [{freightPackageLink.fplUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFreightPackageLink(Guid freightPackageLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
		using (iERPFreightPackageLinkRepository)
		{
			if (!(await base.ERPFreightPackageLinkRepository.DoesFreightPackageLinkExist(freightPackageLinkId)))
			{
				base.ErrorsList.Add($"FreightPackageLink [{freightPackageLinkId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFreightPackageLinkInformationDto eRPFreightPackageLinkInformationDto = await base.ERPFreightPackageLinkRepository.GetFreightPackageLink(freightPackageLinkId);
				string text = await base.ERPFreightPackageLinkRepository.WhereUsed("FreightPackageLinks", new object[3] { eRPFreightPackageLinkInformationDto.fplFreightShipmentID, eRPFreightPackageLinkInformationDto.fplFreightPackageID, eRPFreightPackageLinkInformationDto.fplFreightPackageLineID }, new object[3] { "fplFreightShipmentID", "fplFreightPackageID", "fplFreightPackageLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("FreightPackageLink cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFreightPackageLinkDto>> Process_DeleteFreightPackageLink(Guid freightPackageLinkId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFreightPackageLinkDto> result;
		try
		{
			IERPFreightPackageLinkRepository iERPFreightPackageLinkRepository = (base.ERPFreightPackageLinkRepository = new ERPFreightPackageLinkRepository(base.ApiClientContext));
			using (iERPFreightPackageLinkRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFreightPackageLinkRepository.DeleteRowFromTable("FreightPackageLinks", "fpl", freightPackageLinkId);
				((List<string>)base.ErrorsList).AddRange(new List<string>(aPIValidationInfoDto.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(aPIValidationInfoDto.WarningsList));
				IList<string> errorsList = base.ErrorsList;
				if (errorsList != null && errorsList.Count > 0)
				{
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of FreightPackageLink [{freightPackageLinkId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightPackageLinkDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFreightPackageLinkDto()
			};
		}
		return result;
	}
}
