using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartAssemblyModel : ERPBaseModel, IERPPartAssemblyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
		using (iERPPartAssemblyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartAssemblyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartAssemblyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartAssemblyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartAssemblyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartAssembly(Guid partAssemblyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
		using (iERPPartAssemblyRepository)
		{
			if (!(await base.ERPPartAssemblyRepository.DoesPartAssemblyExist(partAssemblyId)))
			{
				errorsList.Add($"PartAssembly [{partAssemblyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartAssembly(ERPPartAssemblyDto partAssembly)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
		using (iERPPartAssemblyRepository)
		{
			if (!string.IsNullOrWhiteSpace(partAssembly.imaMethodID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partAssembly.imaMethodID })))
			{
				errorsList.Add("imaMethodID [" + partAssembly.imaMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAssembly.imaMethodRevisionID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partAssembly.imaMethodID, partAssembly.imaMethodRevisionID })))
			{
				errorsList.Add("imaMethodRevisionID [" + partAssembly.imaMethodRevisionID + "] not found.");
			}
			if (partAssembly.imaParentAssemblyID > 0 && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartAssemblies", new object[3] { "IMAMETHODID", "IMAMETHODREVISIONID", "IMAMETHODASSEMBLYID" }, new object[3] { partAssembly.imaMethodID, partAssembly.imaMethodRevisionID, partAssembly.imaParentAssemblyID })))
			{
				errorsList.Add($"imaParentAssemblyID [{partAssembly.imaParentAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAssembly.imaPartID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partAssembly.imaPartID })))
			{
				errorsList.Add("imaPartID [" + partAssembly.imaPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAssembly.imaPartRevisionID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partAssembly.imaPartID, partAssembly.imaPartRevisionID })))
			{
				errorsList.Add("imaPartRevisionID [" + partAssembly.imaPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAssembly.imaSourceMethodID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partAssembly.imaSourceMethodID })))
			{
				errorsList.Add("imaSourceMethodID [" + partAssembly.imaSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAssembly.imaSourceRevisionID) && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partAssembly.imaSourceMethodID, partAssembly.imaSourceRevisionID })))
			{
				errorsList.Add("imaSourceRevisionID [" + partAssembly.imaSourceRevisionID + "] not found.");
			}
			if (partAssembly.imaOverlapOperationID > 0 && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartOperations", new object[4] { "IMOMETHODID", "IMOMETHODREVISIONID", "IMOMETHODASSEMBLYID", "IMOMETHODOPERATIONID" }, new object[4] { partAssembly.imaMethodID, partAssembly.imaMethodRevisionID, partAssembly.imaParentAssemblyID, partAssembly.imaOverlapOperationID })))
			{
				errorsList.Add($"imaOverlapOperationID [{partAssembly.imaOverlapOperationID}] not found.");
			}
			if (partAssembly.imaOverlapSourceOperationID > 0 && !(await base.ERPPartAssemblyRepository.DoesRecordExistInTableUsingKeys("PartOperations", new object[4] { "IMOMETHODID", "IMOMETHODREVISIONID", "IMOMETHODASSEMBLYID", "IMOMETHODOPERATIONID" }, new object[4] { partAssembly.imaMethodID, partAssembly.imaMethodRevisionID, partAssembly.imaMethodAssemblyID, partAssembly.imaOverlapSourceOperationID })))
			{
				errorsList.Add($"imaOverlapSourceOperationID [{partAssembly.imaOverlapSourceOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartAssemblyDto>>> Process_GetAllPartAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartAssemblyDto> allPartAssembliesDto = new List<ERPPartAssemblyDto>();
		ERPResponseMessageDto<IList<ERPPartAssemblyDto>> result;
		try
		{
			IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
			using (iERPPartAssemblyRepository)
			{
				foreach (ERPPartAssemblyInformationDto item2 in await base.ERPPartAssemblyRepository.GetAllPartAssemblies(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartAssemblyDto item = new ERPPartAssemblyDto
					{
						imaAssemblyOverlap = item2.imaAssemblyOverlap,
						imaCreatedBy = item2.imaCreatedBy,
						imaCreatedDate = item2.imaCreatedDate,
						imaDocuments = item2.imaDocuments,
						imaUniqueID = item2.imaUniqueID,
						imaPullAllFromStock = item2.imaPullAllFromStock,
						imaUseMethod = item2.imaUseMethod,
						imaLevel = item2.imaLevel,
						imaMethodAssemblyID = item2.imaMethodAssemblyID,
						imaMethodID = item2.imaMethodID,
						imaMethodRevisionID = item2.imaMethodRevisionID,
						imaOverlapDestinationLink = item2.imaOverlapDestinationLink,
						imaOverlapOffsetTime = item2.imaOverlapOffsetTime,
						imaOverlapOperationID = item2.imaOverlapOperationID,
						imaOverlapSourceLink = item2.imaOverlapSourceLink,
						imaOverlapSourceOperationID = item2.imaOverlapSourceOperationID,
						imaOverlapType = item2.imaOverlapType,
						imaParentAssemblyID = item2.imaParentAssemblyID,
						imaPartID = item2.imaPartID,
						imaPartLongDescriptionRtf = item2.imaPartLongDescriptionRtf,
						imaPartLongDescriptionText = item2.imaPartLongDescriptionText,
						imaPartRevisionID = item2.imaPartRevisionID,
						imaPartShortDescription = item2.imaPartShortDescription,
						imaProductionNotesRTF = item2.imaProductionNotesRTF,
						imaProductionNotesText = item2.imaProductionNotesText,
						imaQuantityPerParent = item2.imaQuantityPerParent,
						imaRowVersion = item2.imaRowVersion,
						imaSourceMethodID = item2.imaSourceMethodID,
						imaSourceRevisionID = item2.imaSourceRevisionID,
						imaUnitOfMeasure = item2.imaUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allPartAssembliesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartAssemblies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartAssembliesDto,
				RecordCount = allPartAssembliesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_GetPartAssembly(Guid partAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartAssemblyDto partAssemblyDto = null;
		ERPResponseMessageDto<ERPPartAssemblyDto> result;
		try
		{
			IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
			using (iERPPartAssemblyRepository)
			{
				ERPPartAssemblyInformationDto eRPPartAssemblyInformationDto = await base.ERPPartAssemblyRepository.GetPartAssembly(partAssemblyId);
				partAssemblyDto = new ERPPartAssemblyDto
				{
					imaAssemblyOverlap = eRPPartAssemblyInformationDto.imaAssemblyOverlap,
					imaCreatedBy = eRPPartAssemblyInformationDto.imaCreatedBy,
					imaCreatedDate = eRPPartAssemblyInformationDto.imaCreatedDate,
					imaDocuments = eRPPartAssemblyInformationDto.imaDocuments,
					imaUniqueID = eRPPartAssemblyInformationDto.imaUniqueID,
					imaPullAllFromStock = eRPPartAssemblyInformationDto.imaPullAllFromStock,
					imaUseMethod = eRPPartAssemblyInformationDto.imaUseMethod,
					imaLevel = eRPPartAssemblyInformationDto.imaLevel,
					imaMethodAssemblyID = eRPPartAssemblyInformationDto.imaMethodAssemblyID,
					imaMethodID = eRPPartAssemblyInformationDto.imaMethodID,
					imaMethodRevisionID = eRPPartAssemblyInformationDto.imaMethodRevisionID,
					imaOverlapDestinationLink = eRPPartAssemblyInformationDto.imaOverlapDestinationLink,
					imaOverlapOffsetTime = eRPPartAssemblyInformationDto.imaOverlapOffsetTime,
					imaOverlapOperationID = eRPPartAssemblyInformationDto.imaOverlapOperationID,
					imaOverlapSourceLink = eRPPartAssemblyInformationDto.imaOverlapSourceLink,
					imaOverlapSourceOperationID = eRPPartAssemblyInformationDto.imaOverlapSourceOperationID,
					imaOverlapType = eRPPartAssemblyInformationDto.imaOverlapType,
					imaParentAssemblyID = eRPPartAssemblyInformationDto.imaParentAssemblyID,
					imaPartID = eRPPartAssemblyInformationDto.imaPartID,
					imaPartLongDescriptionRtf = eRPPartAssemblyInformationDto.imaPartLongDescriptionRtf,
					imaPartLongDescriptionText = eRPPartAssemblyInformationDto.imaPartLongDescriptionText,
					imaPartRevisionID = eRPPartAssemblyInformationDto.imaPartRevisionID,
					imaPartShortDescription = eRPPartAssemblyInformationDto.imaPartShortDescription,
					imaProductionNotesRTF = eRPPartAssemblyInformationDto.imaProductionNotesRTF,
					imaProductionNotesText = eRPPartAssemblyInformationDto.imaProductionNotesText,
					imaQuantityPerParent = eRPPartAssemblyInformationDto.imaQuantityPerParent,
					imaRowVersion = eRPPartAssemblyInformationDto.imaRowVersion,
					imaSourceMethodID = eRPPartAssemblyInformationDto.imaSourceMethodID,
					imaSourceRevisionID = eRPPartAssemblyInformationDto.imaSourceRevisionID,
					imaUnitOfMeasure = eRPPartAssemblyInformationDto.imaUnitOfMeasure,
					CustomFields = eRPPartAssemblyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartAssemblies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partAssemblyDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_PutPartAssembly(ERPPartAssemblyDto partAssembly)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartAssemblyDto createdObject = null;
		ERPResponseMessageDto<ERPPartAssemblyDto> result;
		try
		{
			IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
			using (iERPPartAssemblyRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartAssemblyRepository.SavePartAssembly(partAssembly);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartAssemblyInformationDto eRPPartAssemblyInformationDto = await base.ERPPartAssemblyRepository.GetPartAssembly(partAssembly.imaUniqueID);
					createdObject = new ERPPartAssemblyDto
					{
						imaAssemblyOverlap = eRPPartAssemblyInformationDto.imaAssemblyOverlap,
						imaCreatedBy = eRPPartAssemblyInformationDto.imaCreatedBy,
						imaCreatedDate = eRPPartAssemblyInformationDto.imaCreatedDate,
						imaDocuments = eRPPartAssemblyInformationDto.imaDocuments,
						imaUniqueID = eRPPartAssemblyInformationDto.imaUniqueID,
						imaPullAllFromStock = eRPPartAssemblyInformationDto.imaPullAllFromStock,
						imaUseMethod = eRPPartAssemblyInformationDto.imaUseMethod,
						imaLevel = eRPPartAssemblyInformationDto.imaLevel,
						imaMethodAssemblyID = eRPPartAssemblyInformationDto.imaMethodAssemblyID,
						imaMethodID = eRPPartAssemblyInformationDto.imaMethodID,
						imaMethodRevisionID = eRPPartAssemblyInformationDto.imaMethodRevisionID,
						imaOverlapDestinationLink = eRPPartAssemblyInformationDto.imaOverlapDestinationLink,
						imaOverlapOffsetTime = eRPPartAssemblyInformationDto.imaOverlapOffsetTime,
						imaOverlapOperationID = eRPPartAssemblyInformationDto.imaOverlapOperationID,
						imaOverlapSourceLink = eRPPartAssemblyInformationDto.imaOverlapSourceLink,
						imaOverlapSourceOperationID = eRPPartAssemblyInformationDto.imaOverlapSourceOperationID,
						imaOverlapType = eRPPartAssemblyInformationDto.imaOverlapType,
						imaParentAssemblyID = eRPPartAssemblyInformationDto.imaParentAssemblyID,
						imaPartID = eRPPartAssemblyInformationDto.imaPartID,
						imaPartLongDescriptionRtf = eRPPartAssemblyInformationDto.imaPartLongDescriptionRtf,
						imaPartLongDescriptionText = eRPPartAssemblyInformationDto.imaPartLongDescriptionText,
						imaPartRevisionID = eRPPartAssemblyInformationDto.imaPartRevisionID,
						imaPartShortDescription = eRPPartAssemblyInformationDto.imaPartShortDescription,
						imaProductionNotesRTF = eRPPartAssemblyInformationDto.imaProductionNotesRTF,
						imaProductionNotesText = eRPPartAssemblyInformationDto.imaProductionNotesText,
						imaQuantityPerParent = eRPPartAssemblyInformationDto.imaQuantityPerParent,
						imaRowVersion = eRPPartAssemblyInformationDto.imaRowVersion,
						imaSourceMethodID = eRPPartAssemblyInformationDto.imaSourceMethodID,
						imaSourceRevisionID = eRPPartAssemblyInformationDto.imaSourceRevisionID,
						imaUnitOfMeasure = eRPPartAssemblyInformationDto.imaUnitOfMeasure,
						CustomFields = eRPPartAssemblyInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartAssembly [{partAssembly.imaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartAssembly(Guid partAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
		using (iERPPartAssemblyRepository)
		{
			if (!(await base.ERPPartAssemblyRepository.DoesPartAssemblyExist(partAssemblyId)))
			{
				base.ErrorsList.Add($"PartAssembly [{partAssemblyId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartAssemblyInformationDto eRPPartAssemblyInformationDto = await base.ERPPartAssemblyRepository.GetPartAssembly(partAssemblyId);
				string text = await base.ERPPartAssemblyRepository.WhereUsed("PartAssemblies", new object[3] { eRPPartAssemblyInformationDto.imaMethodID, eRPPartAssemblyInformationDto.imaMethodRevisionID, eRPPartAssemblyInformationDto.imaMethodAssemblyID }, new object[3] { "imaMethodID", "imaMethodRevisionID", "imaMethodAssemblyID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartAssembly cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartAssemblyDto>> Process_DeletePartAssembly(Guid partAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartAssemblyDto> result;
		try
		{
			IERPPartAssemblyRepository iERPPartAssemblyRepository = (base.ERPPartAssemblyRepository = new ERPPartAssemblyRepository(base.ApiClientContext));
			using (iERPPartAssemblyRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartAssemblyRepository.DeleteRowFromTable("PartAssemblies", "ima", partAssemblyId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartAssembly [{partAssemblyId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartAssemblyDto()
			};
		}
		return result;
	}
}
