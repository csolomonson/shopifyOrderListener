using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPNextIDModel : ERPBaseModel, IERPNextIDModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllNextIDs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
		using (iERPNextIDRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPNextIDRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPNextIDRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPNextIDRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPNextIDRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetNextID(Guid nextIDId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
		using (iERPNextIDRepository)
		{
			if (!(await base.ERPNextIDRepository.DoesNextIDExist(nextIDId)))
			{
				errorsList.Add($"NextID [{nextIDId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutNextID(ERPNextIDDto nextID)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
		using (iERPNextIDRepository)
		{
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPNextIDDto>>> Process_GetAllNextIDs(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPNextIDDto> allNextIDsDto = new List<ERPNextIDDto>();
		ERPResponseMessageDto<IList<ERPNextIDDto>> result;
		try
		{
			IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
			using (iERPNextIDRepository)
			{
				foreach (ERPNextIDInformationDto item2 in await base.ERPNextIDRepository.GetAllNextIDs(pageSize, pageNumber, filter, orderBy))
				{
					ERPNextIDDto item = new ERPNextIDDto
					{
						xanAutoIncrement = item2.xanAutoIncrement,
						xanCreatedBy = item2.xanCreatedBy,
						xanCreatedDate = item2.xanCreatedDate,
						xanDatasets = item2.xanDatasets,
						xanUniqueID = item2.xanUniqueID,
						xanIncrementAmount = item2.xanIncrementAmount,
						xanLogChanges = item2.xanLogChanges,
						xanNextID = item2.xanNextID,
						xanNumericOnly = item2.xanNumericOnly,
						xanRowVersion = item2.xanRowVersion,
						xanTable = item2.xanTable,
						CustomFields = item2.CustomFields
					};
					allNextIDsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all NextIDs]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPNextIDDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allNextIDsDto,
				RecordCount = allNextIDsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNextIDDto>> Process_GetNextID(Guid nextIDId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPNextIDDto nextIDDto = null;
		ERPResponseMessageDto<ERPNextIDDto> result;
		try
		{
			IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
			using (iERPNextIDRepository)
			{
				ERPNextIDInformationDto eRPNextIDInformationDto = await base.ERPNextIDRepository.GetNextID(nextIDId);
				nextIDDto = new ERPNextIDDto
				{
					xanAutoIncrement = eRPNextIDInformationDto.xanAutoIncrement,
					xanCreatedBy = eRPNextIDInformationDto.xanCreatedBy,
					xanCreatedDate = eRPNextIDInformationDto.xanCreatedDate,
					xanDatasets = eRPNextIDInformationDto.xanDatasets,
					xanUniqueID = eRPNextIDInformationDto.xanUniqueID,
					xanIncrementAmount = eRPNextIDInformationDto.xanIncrementAmount,
					xanLogChanges = eRPNextIDInformationDto.xanLogChanges,
					xanNextID = eRPNextIDInformationDto.xanNextID,
					xanNumericOnly = eRPNextIDInformationDto.xanNumericOnly,
					xanRowVersion = eRPNextIDInformationDto.xanRowVersion,
					xanTable = eRPNextIDInformationDto.xanTable,
					CustomFields = eRPNextIDInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the NextIDs []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNextIDDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = nextIDDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPNextIDDto>> Process_PutNextID(ERPNextIDDto nextID)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPNextIDDto createdObject = null;
		ERPResponseMessageDto<ERPNextIDDto> result;
		try
		{
			IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
			using (iERPNextIDRepository)
			{
				APIValidationInfoDto postResult = await base.ERPNextIDRepository.SaveNextID(nextID);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPNextIDInformationDto eRPNextIDInformationDto = await base.ERPNextIDRepository.GetNextID(nextID.xanUniqueID);
					createdObject = new ERPNextIDDto
					{
						xanAutoIncrement = eRPNextIDInformationDto.xanAutoIncrement,
						xanCreatedBy = eRPNextIDInformationDto.xanCreatedBy,
						xanCreatedDate = eRPNextIDInformationDto.xanCreatedDate,
						xanDatasets = eRPNextIDInformationDto.xanDatasets,
						xanUniqueID = eRPNextIDInformationDto.xanUniqueID,
						xanIncrementAmount = eRPNextIDInformationDto.xanIncrementAmount,
						xanLogChanges = eRPNextIDInformationDto.xanLogChanges,
						xanNextID = eRPNextIDInformationDto.xanNextID,
						xanNumericOnly = eRPNextIDInformationDto.xanNumericOnly,
						xanRowVersion = eRPNextIDInformationDto.xanRowVersion,
						xanTable = eRPNextIDInformationDto.xanTable,
						CustomFields = eRPNextIDInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing NextID [{nextID.xanUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNextIDDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteNextID(Guid nextIDId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
		using (iERPNextIDRepository)
		{
			if (!(await base.ERPNextIDRepository.DoesNextIDExist(nextIDId)))
			{
				base.ErrorsList.Add($"NextID [{nextIDId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPNextIDInformationDto eRPNextIDInformationDto = await base.ERPNextIDRepository.GetNextID(nextIDId);
				string text = await base.ERPNextIDRepository.WhereUsed("NextIDs", new object[1] { eRPNextIDInformationDto.xanTable }, new object[1] { "xanTable" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("NextID cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPNextIDDto>> Process_DeleteNextID(Guid nextIDId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPNextIDDto> result;
		try
		{
			IERPNextIDRepository iERPNextIDRepository = (base.ERPNextIDRepository = new ERPNextIDRepository(base.ApiClientContext));
			using (iERPNextIDRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPNextIDRepository.DeleteRowFromTable("NextIDs", "xan", nextIDId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of NextID [{nextIDId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPNextIDDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPNextIDDto()
			};
		}
		return result;
	}
}
