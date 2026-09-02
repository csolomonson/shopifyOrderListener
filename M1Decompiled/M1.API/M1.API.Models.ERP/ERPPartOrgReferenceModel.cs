using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartOrgReferenceModel : ERPBaseModel, IERPPartOrgReferenceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartOrgReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
		using (iERPPartOrgReferenceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartOrgReferenceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartOrgReferenceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartOrgReferenceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartOrgReferenceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartOrgReference(Guid partOrgReferenceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
		using (iERPPartOrgReferenceRepository)
		{
			if (!(await base.ERPPartOrgReferenceRepository.DoesPartOrgReferenceExist(partOrgReferenceId)))
			{
				errorsList.Add($"PartOrgReference [{partOrgReferenceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartOrgReference(ERPPartOrgReferenceDto partOrgReference)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
		using (iERPPartOrgReferenceRepository)
		{
			if (!string.IsNullOrWhiteSpace(partOrgReference.imzPartID) && !(await base.ERPPartOrgReferenceRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partOrgReference.imzPartID })))
			{
				errorsList.Add("imzPartID [" + partOrgReference.imzPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOrgReference.imzPartRevisionID) && !(await base.ERPPartOrgReferenceRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partOrgReference.imzPartID, partOrgReference.imzPartRevisionID })))
			{
				errorsList.Add("imzPartRevisionID [" + partOrgReference.imzPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partOrgReference.imzOrganizationID) && !(await base.ERPPartOrgReferenceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partOrgReference.imzOrganizationID })))
			{
				errorsList.Add("imzOrganizationID [" + partOrgReference.imzOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartOrgReferenceDto>>> Process_GetAllPartOrgReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartOrgReferenceDto> allPartOrgReferencesDto = new List<ERPPartOrgReferenceDto>();
		ERPResponseMessageDto<IList<ERPPartOrgReferenceDto>> result;
		try
		{
			IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
			using (iERPPartOrgReferenceRepository)
			{
				foreach (ERPPartOrgReferenceInformationDto item2 in await base.ERPPartOrgReferenceRepository.GetAllPartOrgReferences(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartOrgReferenceDto item = new ERPPartOrgReferenceDto
					{
						imzConversionFactor = item2.imzConversionFactor,
						imzCreatedBy = item2.imzCreatedBy,
						imzCreatedDate = item2.imzCreatedDate,
						imzUniqueID = item2.imzUniqueID,
						imzInactive = item2.imzInactive,
						imzPurchased = item2.imzPurchased,
						imzSold = item2.imzSold,
						imzLeadTime = item2.imzLeadTime,
						imzLotSize = item2.imzLotSize,
						imzMinimumPurchaseQuantity = item2.imzMinimumPurchaseQuantity,
						imzOrganizationID = item2.imzOrganizationID,
						imzOrgPartID = item2.imzOrgPartID,
						imzOrgPartShortDescription = item2.imzOrgPartShortDescription,
						imzPartID = item2.imzPartID,
						imzPartRevisionID = item2.imzPartRevisionID,
						imzPurchaseUnitOfMeasure = item2.imzPurchaseUnitOfMeasure,
						imzRowVersion = item2.imzRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartOrgReferencesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartOrgReferences]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartOrgReferenceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartOrgReferencesDto,
				RecordCount = allPartOrgReferencesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_GetPartOrgReference(Guid partOrgReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartOrgReferenceDto partOrgReferenceDto = null;
		ERPResponseMessageDto<ERPPartOrgReferenceDto> result;
		try
		{
			IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
			using (iERPPartOrgReferenceRepository)
			{
				ERPPartOrgReferenceInformationDto eRPPartOrgReferenceInformationDto = await base.ERPPartOrgReferenceRepository.GetPartOrgReference(partOrgReferenceId);
				partOrgReferenceDto = new ERPPartOrgReferenceDto
				{
					imzConversionFactor = eRPPartOrgReferenceInformationDto.imzConversionFactor,
					imzCreatedBy = eRPPartOrgReferenceInformationDto.imzCreatedBy,
					imzCreatedDate = eRPPartOrgReferenceInformationDto.imzCreatedDate,
					imzUniqueID = eRPPartOrgReferenceInformationDto.imzUniqueID,
					imzInactive = eRPPartOrgReferenceInformationDto.imzInactive,
					imzPurchased = eRPPartOrgReferenceInformationDto.imzPurchased,
					imzSold = eRPPartOrgReferenceInformationDto.imzSold,
					imzLeadTime = eRPPartOrgReferenceInformationDto.imzLeadTime,
					imzLotSize = eRPPartOrgReferenceInformationDto.imzLotSize,
					imzMinimumPurchaseQuantity = eRPPartOrgReferenceInformationDto.imzMinimumPurchaseQuantity,
					imzOrganizationID = eRPPartOrgReferenceInformationDto.imzOrganizationID,
					imzOrgPartID = eRPPartOrgReferenceInformationDto.imzOrgPartID,
					imzOrgPartShortDescription = eRPPartOrgReferenceInformationDto.imzOrgPartShortDescription,
					imzPartID = eRPPartOrgReferenceInformationDto.imzPartID,
					imzPartRevisionID = eRPPartOrgReferenceInformationDto.imzPartRevisionID,
					imzPurchaseUnitOfMeasure = eRPPartOrgReferenceInformationDto.imzPurchaseUnitOfMeasure,
					imzRowVersion = eRPPartOrgReferenceInformationDto.imzRowVersion,
					CustomFields = eRPPartOrgReferenceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartOrgReferences []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOrgReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partOrgReferenceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_PutPartOrgReference(ERPPartOrgReferenceDto partOrgReference)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartOrgReferenceDto createdObject = null;
		ERPResponseMessageDto<ERPPartOrgReferenceDto> result;
		try
		{
			IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
			using (iERPPartOrgReferenceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartOrgReferenceRepository.SavePartOrgReference(partOrgReference);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartOrgReferenceInformationDto eRPPartOrgReferenceInformationDto = await base.ERPPartOrgReferenceRepository.GetPartOrgReference(partOrgReference.imzUniqueID);
					createdObject = new ERPPartOrgReferenceDto
					{
						imzConversionFactor = eRPPartOrgReferenceInformationDto.imzConversionFactor,
						imzCreatedBy = eRPPartOrgReferenceInformationDto.imzCreatedBy,
						imzCreatedDate = eRPPartOrgReferenceInformationDto.imzCreatedDate,
						imzUniqueID = eRPPartOrgReferenceInformationDto.imzUniqueID,
						imzInactive = eRPPartOrgReferenceInformationDto.imzInactive,
						imzPurchased = eRPPartOrgReferenceInformationDto.imzPurchased,
						imzSold = eRPPartOrgReferenceInformationDto.imzSold,
						imzLeadTime = eRPPartOrgReferenceInformationDto.imzLeadTime,
						imzLotSize = eRPPartOrgReferenceInformationDto.imzLotSize,
						imzMinimumPurchaseQuantity = eRPPartOrgReferenceInformationDto.imzMinimumPurchaseQuantity,
						imzOrganizationID = eRPPartOrgReferenceInformationDto.imzOrganizationID,
						imzOrgPartID = eRPPartOrgReferenceInformationDto.imzOrgPartID,
						imzOrgPartShortDescription = eRPPartOrgReferenceInformationDto.imzOrgPartShortDescription,
						imzPartID = eRPPartOrgReferenceInformationDto.imzPartID,
						imzPartRevisionID = eRPPartOrgReferenceInformationDto.imzPartRevisionID,
						imzPurchaseUnitOfMeasure = eRPPartOrgReferenceInformationDto.imzPurchaseUnitOfMeasure,
						imzRowVersion = eRPPartOrgReferenceInformationDto.imzRowVersion,
						CustomFields = eRPPartOrgReferenceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartOrgReference [{partOrgReference.imzUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOrgReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartOrgReference(Guid partOrgReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
		using (iERPPartOrgReferenceRepository)
		{
			if (!(await base.ERPPartOrgReferenceRepository.DoesPartOrgReferenceExist(partOrgReferenceId)))
			{
				base.ErrorsList.Add($"PartOrgReference [{partOrgReferenceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartOrgReferenceInformationDto eRPPartOrgReferenceInformationDto = await base.ERPPartOrgReferenceRepository.GetPartOrgReference(partOrgReferenceId);
				string text = await base.ERPPartOrgReferenceRepository.WhereUsed("PartOrgReferences", new object[3] { eRPPartOrgReferenceInformationDto.imzPartID, eRPPartOrgReferenceInformationDto.imzPartRevisionID, eRPPartOrgReferenceInformationDto.imzOrganizationID }, new object[3] { "imzPartID", "imzPartRevisionID", "imzOrganizationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartOrgReference cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartOrgReferenceDto>> Process_DeletePartOrgReference(Guid partOrgReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartOrgReferenceDto> result;
		try
		{
			IERPPartOrgReferenceRepository iERPPartOrgReferenceRepository = (base.ERPPartOrgReferenceRepository = new ERPPartOrgReferenceRepository(base.ApiClientContext));
			using (iERPPartOrgReferenceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartOrgReferenceRepository.DeleteRowFromTable("PartOrgReferences", "imz", partOrgReferenceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartOrgReference [{partOrgReferenceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartOrgReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartOrgReferenceDto()
			};
		}
		return result;
	}
}
