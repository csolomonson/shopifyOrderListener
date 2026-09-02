using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPARInvoiceMemoModel : ERPBaseModel, IERPARInvoiceMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllARInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
		using (iERPARInvoiceMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPARInvoiceMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPARInvoiceMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPARInvoiceMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPARInvoiceMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetARInvoiceMemo(Guid aRInvoiceMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
		using (iERPARInvoiceMemoRepository)
		{
			if (!(await base.ERPARInvoiceMemoRepository.DoesARInvoiceMemoExist(aRInvoiceMemoId)))
			{
				errorsList.Add($"ARInvoiceMemo [{aRInvoiceMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
		using (iERPARInvoiceMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(aRInvoiceMemo.ariArInvoiceID) && !(await base.ERPARInvoiceMemoRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { aRInvoiceMemo.ariArInvoiceID })))
			{
				errorsList.Add("ariArInvoiceID [" + aRInvoiceMemo.ariArInvoiceID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPARInvoiceMemoDto>>> Process_GetAllARInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPARInvoiceMemoDto> allARInvoiceMemosDto = new List<ERPARInvoiceMemoDto>();
		ERPResponseMessageDto<IList<ERPARInvoiceMemoDto>> result;
		try
		{
			IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
			using (iERPARInvoiceMemoRepository)
			{
				foreach (ERPARInvoiceMemoInformationDto item2 in await base.ERPARInvoiceMemoRepository.GetAllARInvoiceMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPARInvoiceMemoDto item = new ERPARInvoiceMemoDto
					{
						ariArInvoiceID = item2.ariArInvoiceID,
						ariCreatedBy = item2.ariCreatedBy,
						ariCreatedDate = item2.ariCreatedDate,
						ariUniqueID = item2.ariUniqueID,
						ariLongDescriptionRtf = item2.ariLongDescriptionRtf,
						ariLongDescriptionText = item2.ariLongDescriptionText,
						ariMemoDate = item2.ariMemoDate,
						ariRowVersion = item2.ariRowVersion,
						ariArInvoiceMemoID = item2.ariArInvoiceMemoID,
						ariShortDescription = item2.ariShortDescription,
						ariShowInArInvoices = item2.ariShowInArInvoices,
						ariShowInArPayments = item2.ariShowInArPayments,
						CustomFields = item2.CustomFields
					};
					allARInvoiceMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all ARInvoiceMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPARInvoiceMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allARInvoiceMemosDto,
				RecordCount = allARInvoiceMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_GetARInvoiceMemo(Guid aRInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPARInvoiceMemoDto aRInvoiceMemoDto = null;
		ERPResponseMessageDto<ERPARInvoiceMemoDto> result;
		try
		{
			IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
			using (iERPARInvoiceMemoRepository)
			{
				ERPARInvoiceMemoInformationDto eRPARInvoiceMemoInformationDto = await base.ERPARInvoiceMemoRepository.GetARInvoiceMemo(aRInvoiceMemoId);
				aRInvoiceMemoDto = new ERPARInvoiceMemoDto
				{
					ariArInvoiceID = eRPARInvoiceMemoInformationDto.ariArInvoiceID,
					ariCreatedBy = eRPARInvoiceMemoInformationDto.ariCreatedBy,
					ariCreatedDate = eRPARInvoiceMemoInformationDto.ariCreatedDate,
					ariUniqueID = eRPARInvoiceMemoInformationDto.ariUniqueID,
					ariLongDescriptionRtf = eRPARInvoiceMemoInformationDto.ariLongDescriptionRtf,
					ariLongDescriptionText = eRPARInvoiceMemoInformationDto.ariLongDescriptionText,
					ariMemoDate = eRPARInvoiceMemoInformationDto.ariMemoDate,
					ariRowVersion = eRPARInvoiceMemoInformationDto.ariRowVersion,
					ariArInvoiceMemoID = eRPARInvoiceMemoInformationDto.ariArInvoiceMemoID,
					ariShortDescription = eRPARInvoiceMemoInformationDto.ariShortDescription,
					ariShowInArInvoices = eRPARInvoiceMemoInformationDto.ariShowInArInvoices,
					ariShowInArPayments = eRPARInvoiceMemoInformationDto.ariShowInArPayments,
					CustomFields = eRPARInvoiceMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the ARInvoiceMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aRInvoiceMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_PutARInvoiceMemo(ERPARInvoiceMemoDto aRInvoiceMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPARInvoiceMemoDto createdObject = null;
		ERPResponseMessageDto<ERPARInvoiceMemoDto> result;
		try
		{
			IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
			using (iERPARInvoiceMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPARInvoiceMemoRepository.SaveARInvoiceMemo(aRInvoiceMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPARInvoiceMemoInformationDto eRPARInvoiceMemoInformationDto = await base.ERPARInvoiceMemoRepository.GetARInvoiceMemo(aRInvoiceMemo.ariUniqueID);
					createdObject = new ERPARInvoiceMemoDto
					{
						ariArInvoiceID = eRPARInvoiceMemoInformationDto.ariArInvoiceID,
						ariCreatedBy = eRPARInvoiceMemoInformationDto.ariCreatedBy,
						ariCreatedDate = eRPARInvoiceMemoInformationDto.ariCreatedDate,
						ariUniqueID = eRPARInvoiceMemoInformationDto.ariUniqueID,
						ariLongDescriptionRtf = eRPARInvoiceMemoInformationDto.ariLongDescriptionRtf,
						ariLongDescriptionText = eRPARInvoiceMemoInformationDto.ariLongDescriptionText,
						ariMemoDate = eRPARInvoiceMemoInformationDto.ariMemoDate,
						ariRowVersion = eRPARInvoiceMemoInformationDto.ariRowVersion,
						ariArInvoiceMemoID = eRPARInvoiceMemoInformationDto.ariArInvoiceMemoID,
						ariShortDescription = eRPARInvoiceMemoInformationDto.ariShortDescription,
						ariShowInArInvoices = eRPARInvoiceMemoInformationDto.ariShowInArInvoices,
						ariShowInArPayments = eRPARInvoiceMemoInformationDto.ariShowInArPayments,
						CustomFields = eRPARInvoiceMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing ARInvoiceMemo [{aRInvoiceMemo.ariUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteARInvoiceMemo(Guid aRInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
		using (iERPARInvoiceMemoRepository)
		{
			if (!(await base.ERPARInvoiceMemoRepository.DoesARInvoiceMemoExist(aRInvoiceMemoId)))
			{
				base.ErrorsList.Add($"ARInvoiceMemo [{aRInvoiceMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPARInvoiceMemoInformationDto eRPARInvoiceMemoInformationDto = await base.ERPARInvoiceMemoRepository.GetARInvoiceMemo(aRInvoiceMemoId);
				string text = await base.ERPARInvoiceMemoRepository.WhereUsed("ARInvoiceMemos", new object[2] { eRPARInvoiceMemoInformationDto.ariArInvoiceID, eRPARInvoiceMemoInformationDto.ariArInvoiceMemoID }, new object[2] { "ariArInvoiceID", "ariArInvoiceMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("ARInvoiceMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPARInvoiceMemoDto>> Process_DeleteARInvoiceMemo(Guid aRInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPARInvoiceMemoDto> result;
		try
		{
			IERPARInvoiceMemoRepository iERPARInvoiceMemoRepository = (base.ERPARInvoiceMemoRepository = new ERPARInvoiceMemoRepository(base.ApiClientContext));
			using (iERPARInvoiceMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPARInvoiceMemoRepository.DeleteRowFromTable("ARInvoiceMemos", "ari", aRInvoiceMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of ARInvoiceMemo [{aRInvoiceMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPARInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPARInvoiceMemoDto()
			};
		}
		return result;
	}
}
