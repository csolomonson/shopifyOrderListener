using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartPriceBreakModel : ERPBaseModel, IERPPartPriceBreakModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartPriceBreaks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
		using (iERPPartPriceBreakRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartPriceBreakRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartPriceBreakRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartPriceBreakRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartPriceBreakRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartPriceBreak(Guid partPriceBreakId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
		using (iERPPartPriceBreakRepository)
		{
			if (!(await base.ERPPartPriceBreakRepository.DoesPartPriceBreakExist(partPriceBreakId)))
			{
				errorsList.Add($"PartPriceBreak [{partPriceBreakId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartPriceBreak(ERPPartPriceBreakDto partPriceBreak)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
		using (iERPPartPriceBreakRepository)
		{
			if (partPriceBreak.imjPartPriceID > 0 && !(await base.ERPPartPriceBreakRepository.DoesRecordExistInTableUsingKeys("PartPrices", new object[1] { "IMIPARTPRICEID" }, new object[1] { partPriceBreak.imjPartPriceID })))
			{
				errorsList.Add($"imjPartPriceID [{partPriceBreak.imjPartPriceID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartPriceBreakDto>>> Process_GetAllPartPriceBreaks(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartPriceBreakDto> allPartPriceBreaksDto = new List<ERPPartPriceBreakDto>();
		ERPResponseMessageDto<IList<ERPPartPriceBreakDto>> result;
		try
		{
			IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
			using (iERPPartPriceBreakRepository)
			{
				foreach (ERPPartPriceBreakInformationDto item2 in await base.ERPPartPriceBreakRepository.GetAllPartPriceBreaks(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartPriceBreakDto item = new ERPPartPriceBreakDto
					{
						imjCreatedBy = item2.imjCreatedBy,
						imjCreatedDate = item2.imjCreatedDate,
						imjDiscount = item2.imjDiscount,
						imjUniqueID = item2.imjUniqueID,
						imjLeadTime = item2.imjLeadTime,
						imjPartPriceID = item2.imjPartPriceID,
						imjProposedNewPrice = item2.imjProposedNewPrice,
						imjQuantity = item2.imjQuantity,
						imjRowVersion = item2.imjRowVersion,
						imjPartPriceBreakID = item2.imjPartPriceBreakID,
						imjUnitPrice = item2.imjUnitPrice,
						CustomFields = item2.CustomFields
					};
					allPartPriceBreaksDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartPriceBreaks]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartPriceBreakDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartPriceBreaksDto,
				RecordCount = allPartPriceBreaksDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_GetPartPriceBreak(Guid partPriceBreakId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartPriceBreakDto partPriceBreakDto = null;
		ERPResponseMessageDto<ERPPartPriceBreakDto> result;
		try
		{
			IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
			using (iERPPartPriceBreakRepository)
			{
				ERPPartPriceBreakInformationDto eRPPartPriceBreakInformationDto = await base.ERPPartPriceBreakRepository.GetPartPriceBreak(partPriceBreakId);
				partPriceBreakDto = new ERPPartPriceBreakDto
				{
					imjCreatedBy = eRPPartPriceBreakInformationDto.imjCreatedBy,
					imjCreatedDate = eRPPartPriceBreakInformationDto.imjCreatedDate,
					imjDiscount = eRPPartPriceBreakInformationDto.imjDiscount,
					imjUniqueID = eRPPartPriceBreakInformationDto.imjUniqueID,
					imjLeadTime = eRPPartPriceBreakInformationDto.imjLeadTime,
					imjPartPriceID = eRPPartPriceBreakInformationDto.imjPartPriceID,
					imjProposedNewPrice = eRPPartPriceBreakInformationDto.imjProposedNewPrice,
					imjQuantity = eRPPartPriceBreakInformationDto.imjQuantity,
					imjRowVersion = eRPPartPriceBreakInformationDto.imjRowVersion,
					imjPartPriceBreakID = eRPPartPriceBreakInformationDto.imjPartPriceBreakID,
					imjUnitPrice = eRPPartPriceBreakInformationDto.imjUnitPrice,
					CustomFields = eRPPartPriceBreakInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartPriceBreaks []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceBreakDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partPriceBreakDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_PutPartPriceBreak(ERPPartPriceBreakDto partPriceBreak)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartPriceBreakDto createdObject = null;
		ERPResponseMessageDto<ERPPartPriceBreakDto> result;
		try
		{
			IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
			using (iERPPartPriceBreakRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartPriceBreakRepository.SavePartPriceBreak(partPriceBreak);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartPriceBreakInformationDto eRPPartPriceBreakInformationDto = await base.ERPPartPriceBreakRepository.GetPartPriceBreak(partPriceBreak.imjUniqueID);
					createdObject = new ERPPartPriceBreakDto
					{
						imjCreatedBy = eRPPartPriceBreakInformationDto.imjCreatedBy,
						imjCreatedDate = eRPPartPriceBreakInformationDto.imjCreatedDate,
						imjDiscount = eRPPartPriceBreakInformationDto.imjDiscount,
						imjUniqueID = eRPPartPriceBreakInformationDto.imjUniqueID,
						imjLeadTime = eRPPartPriceBreakInformationDto.imjLeadTime,
						imjPartPriceID = eRPPartPriceBreakInformationDto.imjPartPriceID,
						imjProposedNewPrice = eRPPartPriceBreakInformationDto.imjProposedNewPrice,
						imjQuantity = eRPPartPriceBreakInformationDto.imjQuantity,
						imjRowVersion = eRPPartPriceBreakInformationDto.imjRowVersion,
						imjPartPriceBreakID = eRPPartPriceBreakInformationDto.imjPartPriceBreakID,
						imjUnitPrice = eRPPartPriceBreakInformationDto.imjUnitPrice,
						CustomFields = eRPPartPriceBreakInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartPriceBreak [{partPriceBreak.imjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceBreakDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartPriceBreak(Guid partPriceBreakId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
		using (iERPPartPriceBreakRepository)
		{
			if (!(await base.ERPPartPriceBreakRepository.DoesPartPriceBreakExist(partPriceBreakId)))
			{
				base.ErrorsList.Add($"PartPriceBreak [{partPriceBreakId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartPriceBreakInformationDto eRPPartPriceBreakInformationDto = await base.ERPPartPriceBreakRepository.GetPartPriceBreak(partPriceBreakId);
				string text = await base.ERPPartPriceBreakRepository.WhereUsed("PartPriceBreaks", new object[2] { eRPPartPriceBreakInformationDto.imjPartPriceID, eRPPartPriceBreakInformationDto.imjPartPriceBreakID }, new object[2] { "imjPartPriceID", "imjPartPriceBreakID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartPriceBreak cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceBreakDto>> Process_DeletePartPriceBreak(Guid partPriceBreakId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartPriceBreakDto> result;
		try
		{
			IERPPartPriceBreakRepository iERPPartPriceBreakRepository = (base.ERPPartPriceBreakRepository = new ERPPartPriceBreakRepository(base.ApiClientContext));
			using (iERPPartPriceBreakRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartPriceBreakRepository.DeleteRowFromTable("PartPriceBreaks", "imj", partPriceBreakId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartPriceBreak [{partPriceBreakId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceBreakDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartPriceBreakDto()
			};
		}
		return result;
	}
}
