using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartCrossReferenceModel : ERPBaseModel, IERPPartCrossReferenceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartCrossReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
		using (iERPPartCrossReferenceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartCrossReferenceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartCrossReferenceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartCrossReferenceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartCrossReferenceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartCrossReference(Guid partCrossReferenceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
		using (iERPPartCrossReferenceRepository)
		{
			if (!(await base.ERPPartCrossReferenceRepository.DoesPartCrossReferenceExist(partCrossReferenceId)))
			{
				errorsList.Add($"PartCrossReference [{partCrossReferenceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartCrossReference(ERPPartCrossReferenceDto partCrossReference)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
		using (iERPPartCrossReferenceRepository)
		{
			if (!string.IsNullOrWhiteSpace(partCrossReference.imxPartID) && !(await base.ERPPartCrossReferenceRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partCrossReference.imxPartID })))
			{
				errorsList.Add("imxPartID [" + partCrossReference.imxPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partCrossReference.imxPartRevisionID) && !(await base.ERPPartCrossReferenceRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partCrossReference.imxPartID, partCrossReference.imxPartRevisionID })))
			{
				errorsList.Add("imxPartRevisionID [" + partCrossReference.imxPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partCrossReference.imxOrganizationID) && !(await base.ERPPartCrossReferenceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partCrossReference.imxOrganizationID })))
			{
				errorsList.Add("imxOrganizationID [" + partCrossReference.imxOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partCrossReference.imxLocationID) && !(await base.ERPPartCrossReferenceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { partCrossReference.imxOrganizationID, partCrossReference.imxLocationID })))
			{
				errorsList.Add("imxLocationID [" + partCrossReference.imxLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartCrossReferenceDto>>> Process_GetAllPartCrossReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartCrossReferenceDto> allPartCrossReferencesDto = new List<ERPPartCrossReferenceDto>();
		ERPResponseMessageDto<IList<ERPPartCrossReferenceDto>> result;
		try
		{
			IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
			using (iERPPartCrossReferenceRepository)
			{
				foreach (ERPPartCrossReferenceInformationDto item2 in await base.ERPPartCrossReferenceRepository.GetAllPartCrossReferences(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartCrossReferenceDto item = new ERPPartCrossReferenceDto
					{
						imxConversionFactor = item2.imxConversionFactor,
						imxCreatedBy = item2.imxCreatedBy,
						imxCreatedDate = item2.imxCreatedDate,
						imxUniqueID = item2.imxUniqueID,
						imxInactive = item2.imxInactive,
						imxPurchased = item2.imxPurchased,
						imxSold = item2.imxSold,
						imxLeadTime = item2.imxLeadTime,
						imxLocationID = item2.imxLocationID,
						imxLotSize = item2.imxLotSize,
						imxMinimumPurchaseQuantity = item2.imxMinimumPurchaseQuantity,
						imxOrganizationID = item2.imxOrganizationID,
						imxOrgPartID = item2.imxOrgPartID,
						imxOrgPartShortDescription = item2.imxOrgPartShortDescription,
						imxPartID = item2.imxPartID,
						imxPartRevisionID = item2.imxPartRevisionID,
						imxPurchaseUnitOfMeasure = item2.imxPurchaseUnitOfMeasure,
						imxRowVersion = item2.imxRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartCrossReferencesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartCrossReferences]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartCrossReferenceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartCrossReferencesDto,
				RecordCount = allPartCrossReferencesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_GetPartCrossReference(Guid partCrossReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartCrossReferenceDto partCrossReferenceDto = null;
		ERPResponseMessageDto<ERPPartCrossReferenceDto> result;
		try
		{
			IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
			using (iERPPartCrossReferenceRepository)
			{
				ERPPartCrossReferenceInformationDto eRPPartCrossReferenceInformationDto = await base.ERPPartCrossReferenceRepository.GetPartCrossReference(partCrossReferenceId);
				partCrossReferenceDto = new ERPPartCrossReferenceDto
				{
					imxConversionFactor = eRPPartCrossReferenceInformationDto.imxConversionFactor,
					imxCreatedBy = eRPPartCrossReferenceInformationDto.imxCreatedBy,
					imxCreatedDate = eRPPartCrossReferenceInformationDto.imxCreatedDate,
					imxUniqueID = eRPPartCrossReferenceInformationDto.imxUniqueID,
					imxInactive = eRPPartCrossReferenceInformationDto.imxInactive,
					imxPurchased = eRPPartCrossReferenceInformationDto.imxPurchased,
					imxSold = eRPPartCrossReferenceInformationDto.imxSold,
					imxLeadTime = eRPPartCrossReferenceInformationDto.imxLeadTime,
					imxLocationID = eRPPartCrossReferenceInformationDto.imxLocationID,
					imxLotSize = eRPPartCrossReferenceInformationDto.imxLotSize,
					imxMinimumPurchaseQuantity = eRPPartCrossReferenceInformationDto.imxMinimumPurchaseQuantity,
					imxOrganizationID = eRPPartCrossReferenceInformationDto.imxOrganizationID,
					imxOrgPartID = eRPPartCrossReferenceInformationDto.imxOrgPartID,
					imxOrgPartShortDescription = eRPPartCrossReferenceInformationDto.imxOrgPartShortDescription,
					imxPartID = eRPPartCrossReferenceInformationDto.imxPartID,
					imxPartRevisionID = eRPPartCrossReferenceInformationDto.imxPartRevisionID,
					imxPurchaseUnitOfMeasure = eRPPartCrossReferenceInformationDto.imxPurchaseUnitOfMeasure,
					imxRowVersion = eRPPartCrossReferenceInformationDto.imxRowVersion,
					CustomFields = eRPPartCrossReferenceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartCrossReferences []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartCrossReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partCrossReferenceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_PutPartCrossReference(ERPPartCrossReferenceDto partCrossReference)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartCrossReferenceDto createdObject = null;
		ERPResponseMessageDto<ERPPartCrossReferenceDto> result;
		try
		{
			IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
			using (iERPPartCrossReferenceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartCrossReferenceRepository.SavePartCrossReference(partCrossReference);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartCrossReferenceInformationDto eRPPartCrossReferenceInformationDto = await base.ERPPartCrossReferenceRepository.GetPartCrossReference(partCrossReference.imxUniqueID);
					createdObject = new ERPPartCrossReferenceDto
					{
						imxConversionFactor = eRPPartCrossReferenceInformationDto.imxConversionFactor,
						imxCreatedBy = eRPPartCrossReferenceInformationDto.imxCreatedBy,
						imxCreatedDate = eRPPartCrossReferenceInformationDto.imxCreatedDate,
						imxUniqueID = eRPPartCrossReferenceInformationDto.imxUniqueID,
						imxInactive = eRPPartCrossReferenceInformationDto.imxInactive,
						imxPurchased = eRPPartCrossReferenceInformationDto.imxPurchased,
						imxSold = eRPPartCrossReferenceInformationDto.imxSold,
						imxLeadTime = eRPPartCrossReferenceInformationDto.imxLeadTime,
						imxLocationID = eRPPartCrossReferenceInformationDto.imxLocationID,
						imxLotSize = eRPPartCrossReferenceInformationDto.imxLotSize,
						imxMinimumPurchaseQuantity = eRPPartCrossReferenceInformationDto.imxMinimumPurchaseQuantity,
						imxOrganizationID = eRPPartCrossReferenceInformationDto.imxOrganizationID,
						imxOrgPartID = eRPPartCrossReferenceInformationDto.imxOrgPartID,
						imxOrgPartShortDescription = eRPPartCrossReferenceInformationDto.imxOrgPartShortDescription,
						imxPartID = eRPPartCrossReferenceInformationDto.imxPartID,
						imxPartRevisionID = eRPPartCrossReferenceInformationDto.imxPartRevisionID,
						imxPurchaseUnitOfMeasure = eRPPartCrossReferenceInformationDto.imxPurchaseUnitOfMeasure,
						imxRowVersion = eRPPartCrossReferenceInformationDto.imxRowVersion,
						CustomFields = eRPPartCrossReferenceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartCrossReference [{partCrossReference.imxUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartCrossReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartCrossReference(Guid partCrossReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
		using (iERPPartCrossReferenceRepository)
		{
			if (!(await base.ERPPartCrossReferenceRepository.DoesPartCrossReferenceExist(partCrossReferenceId)))
			{
				base.ErrorsList.Add($"PartCrossReference [{partCrossReferenceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartCrossReferenceInformationDto eRPPartCrossReferenceInformationDto = await base.ERPPartCrossReferenceRepository.GetPartCrossReference(partCrossReferenceId);
				string text = await base.ERPPartCrossReferenceRepository.WhereUsed("PartCrossReferences", new object[4] { eRPPartCrossReferenceInformationDto.imxPartID, eRPPartCrossReferenceInformationDto.imxPartRevisionID, eRPPartCrossReferenceInformationDto.imxOrganizationID, eRPPartCrossReferenceInformationDto.imxLocationID }, new object[4] { "imxPartID", "imxPartRevisionID", "imxOrganizationID", "imxLocationID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartCrossReference cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartCrossReferenceDto>> Process_DeletePartCrossReference(Guid partCrossReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartCrossReferenceDto> result;
		try
		{
			IERPPartCrossReferenceRepository iERPPartCrossReferenceRepository = (base.ERPPartCrossReferenceRepository = new ERPPartCrossReferenceRepository(base.ApiClientContext));
			using (iERPPartCrossReferenceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartCrossReferenceRepository.DeleteRowFromTable("PartCrossReferences", "imx", partCrossReferenceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartCrossReference [{partCrossReferenceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartCrossReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartCrossReferenceDto()
			};
		}
		return result;
	}
}
