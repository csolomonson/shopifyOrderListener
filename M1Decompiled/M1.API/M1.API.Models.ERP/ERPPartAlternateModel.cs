using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartAlternateModel : ERPBaseModel, IERPPartAlternateModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartAlternates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
		using (iERPPartAlternateRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartAlternateRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartAlternateRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartAlternateRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartAlternateRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartAlternate(Guid partAlternateId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
		using (iERPPartAlternateRepository)
		{
			if (!(await base.ERPPartAlternateRepository.DoesPartAlternateExist(partAlternateId)))
			{
				errorsList.Add($"PartAlternate [{partAlternateId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartAlternate(ERPPartAlternateDto partAlternate)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
		using (iERPPartAlternateRepository)
		{
			if (!string.IsNullOrWhiteSpace(partAlternate.imePartID) && !(await base.ERPPartAlternateRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partAlternate.imePartID })))
			{
				errorsList.Add("imePartID [" + partAlternate.imePartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAlternate.imePartRevisionID) && !(await base.ERPPartAlternateRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partAlternate.imePartID, partAlternate.imePartRevisionID })))
			{
				errorsList.Add("imePartRevisionID [" + partAlternate.imePartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAlternate.imeAlternatePartID) && !(await base.ERPPartAlternateRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partAlternate.imeAlternatePartID })))
			{
				errorsList.Add("imeAlternatePartID [" + partAlternate.imeAlternatePartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partAlternate.imeAlternatePartRevisionID) && !(await base.ERPPartAlternateRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partAlternate.imeAlternatePartID, partAlternate.imeAlternatePartRevisionID })))
			{
				errorsList.Add("imeAlternatePartRevisionID [" + partAlternate.imeAlternatePartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartAlternateDto>>> Process_GetAllPartAlternates(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartAlternateDto> allPartAlternatesDto = new List<ERPPartAlternateDto>();
		ERPResponseMessageDto<IList<ERPPartAlternateDto>> result;
		try
		{
			IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
			using (iERPPartAlternateRepository)
			{
				foreach (ERPPartAlternateInformationDto item2 in await base.ERPPartAlternateRepository.GetAllPartAlternates(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartAlternateDto item = new ERPPartAlternateDto
					{
						imeAlternatePartID = item2.imeAlternatePartID,
						imeAlternatePartRevisionID = item2.imeAlternatePartRevisionID,
						imeComment = item2.imeComment,
						imeCreatedBy = item2.imeCreatedBy,
						imeCreatedDate = item2.imeCreatedDate,
						imeUniqueID = item2.imeUniqueID,
						imePartID = item2.imePartID,
						imePartRevisionID = item2.imePartRevisionID,
						imeRowVersion = item2.imeRowVersion,
						CustomFields = item2.CustomFields
					};
					allPartAlternatesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartAlternates]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartAlternateDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartAlternatesDto,
				RecordCount = allPartAlternatesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_GetPartAlternate(Guid partAlternateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartAlternateDto partAlternateDto = null;
		ERPResponseMessageDto<ERPPartAlternateDto> result;
		try
		{
			IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
			using (iERPPartAlternateRepository)
			{
				ERPPartAlternateInformationDto eRPPartAlternateInformationDto = await base.ERPPartAlternateRepository.GetPartAlternate(partAlternateId);
				partAlternateDto = new ERPPartAlternateDto
				{
					imeAlternatePartID = eRPPartAlternateInformationDto.imeAlternatePartID,
					imeAlternatePartRevisionID = eRPPartAlternateInformationDto.imeAlternatePartRevisionID,
					imeComment = eRPPartAlternateInformationDto.imeComment,
					imeCreatedBy = eRPPartAlternateInformationDto.imeCreatedBy,
					imeCreatedDate = eRPPartAlternateInformationDto.imeCreatedDate,
					imeUniqueID = eRPPartAlternateInformationDto.imeUniqueID,
					imePartID = eRPPartAlternateInformationDto.imePartID,
					imePartRevisionID = eRPPartAlternateInformationDto.imePartRevisionID,
					imeRowVersion = eRPPartAlternateInformationDto.imeRowVersion,
					CustomFields = eRPPartAlternateInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartAlternates []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAlternateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partAlternateDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_PutPartAlternate(ERPPartAlternateDto partAlternate)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartAlternateDto createdObject = null;
		ERPResponseMessageDto<ERPPartAlternateDto> result;
		try
		{
			IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
			using (iERPPartAlternateRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartAlternateRepository.SavePartAlternate(partAlternate);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartAlternateInformationDto eRPPartAlternateInformationDto = await base.ERPPartAlternateRepository.GetPartAlternate(partAlternate.imeUniqueID);
					createdObject = new ERPPartAlternateDto
					{
						imeAlternatePartID = eRPPartAlternateInformationDto.imeAlternatePartID,
						imeAlternatePartRevisionID = eRPPartAlternateInformationDto.imeAlternatePartRevisionID,
						imeComment = eRPPartAlternateInformationDto.imeComment,
						imeCreatedBy = eRPPartAlternateInformationDto.imeCreatedBy,
						imeCreatedDate = eRPPartAlternateInformationDto.imeCreatedDate,
						imeUniqueID = eRPPartAlternateInformationDto.imeUniqueID,
						imePartID = eRPPartAlternateInformationDto.imePartID,
						imePartRevisionID = eRPPartAlternateInformationDto.imePartRevisionID,
						imeRowVersion = eRPPartAlternateInformationDto.imeRowVersion,
						CustomFields = eRPPartAlternateInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartAlternate [{partAlternate.imeUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAlternateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartAlternate(Guid partAlternateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
		using (iERPPartAlternateRepository)
		{
			if (!(await base.ERPPartAlternateRepository.DoesPartAlternateExist(partAlternateId)))
			{
				base.ErrorsList.Add($"PartAlternate [{partAlternateId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartAlternateInformationDto eRPPartAlternateInformationDto = await base.ERPPartAlternateRepository.GetPartAlternate(partAlternateId);
				string text = await base.ERPPartAlternateRepository.WhereUsed("PartAlternates", new object[4] { eRPPartAlternateInformationDto.imePartID, eRPPartAlternateInformationDto.imePartRevisionID, eRPPartAlternateInformationDto.imeAlternatePartID, eRPPartAlternateInformationDto.imeAlternatePartRevisionID }, new object[4] { "imePartID", "imePartRevisionID", "imeAlternatePartID", "imeAlternatePartRevisionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartAlternate cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartAlternateDto>> Process_DeletePartAlternate(Guid partAlternateId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartAlternateDto> result;
		try
		{
			IERPPartAlternateRepository iERPPartAlternateRepository = (base.ERPPartAlternateRepository = new ERPPartAlternateRepository(base.ApiClientContext));
			using (iERPPartAlternateRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartAlternateRepository.DeleteRowFromTable("PartAlternates", "ime", partAlternateId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartAlternate [{partAlternateId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartAlternateDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartAlternateDto()
			};
		}
		return result;
	}
}
