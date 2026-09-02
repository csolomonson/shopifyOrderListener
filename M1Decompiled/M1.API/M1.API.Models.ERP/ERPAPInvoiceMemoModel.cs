using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPAPInvoiceMemoModel : ERPBaseModel, IERPAPInvoiceMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllAPInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
		using (iERPAPInvoiceMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPAPInvoiceMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPAPInvoiceMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPAPInvoiceMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPAPInvoiceMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetAPInvoiceMemo(Guid aPInvoiceMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
		using (iERPAPInvoiceMemoRepository)
		{
			if (!(await base.ERPAPInvoiceMemoRepository.DoesAPInvoiceMemoExist(aPInvoiceMemoId)))
			{
				errorsList.Add($"APInvoiceMemo [{aPInvoiceMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
		using (iERPAPInvoiceMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(aPInvoiceMemo.apiApInvoiceID) && !(await base.ERPAPInvoiceMemoRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { aPInvoiceMemo.apiApInvoiceID })))
			{
				errorsList.Add("apiApInvoiceID [" + aPInvoiceMemo.apiApInvoiceID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPAPInvoiceMemoDto>>> Process_GetAllAPInvoiceMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPAPInvoiceMemoDto> allAPInvoiceMemosDto = new List<ERPAPInvoiceMemoDto>();
		ERPResponseMessageDto<IList<ERPAPInvoiceMemoDto>> result;
		try
		{
			IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
			using (iERPAPInvoiceMemoRepository)
			{
				foreach (ERPAPInvoiceMemoInformationDto item2 in await base.ERPAPInvoiceMemoRepository.GetAllAPInvoiceMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPAPInvoiceMemoDto item = new ERPAPInvoiceMemoDto
					{
						apiApInvoiceID = item2.apiApInvoiceID,
						apiCreatedBy = item2.apiCreatedBy,
						apiCreatedDate = item2.apiCreatedDate,
						apiUniqueID = item2.apiUniqueID,
						apiLongDescriptionRtf = item2.apiLongDescriptionRtf,
						apiLongDescriptionText = item2.apiLongDescriptionText,
						apiMemoDate = item2.apiMemoDate,
						apiRowVersion = item2.apiRowVersion,
						apiApInvoiceMemoID = item2.apiApInvoiceMemoID,
						apiShortDescription = item2.apiShortDescription,
						apiShowInApInvoices = item2.apiShowInApInvoices,
						apiShowInApPayments = item2.apiShowInApPayments,
						CustomFields = item2.CustomFields
					};
					allAPInvoiceMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all APInvoiceMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPAPInvoiceMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allAPInvoiceMemosDto,
				RecordCount = allAPInvoiceMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_GetAPInvoiceMemo(Guid aPInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPAPInvoiceMemoDto aPInvoiceMemoDto = null;
		ERPResponseMessageDto<ERPAPInvoiceMemoDto> result;
		try
		{
			IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
			using (iERPAPInvoiceMemoRepository)
			{
				ERPAPInvoiceMemoInformationDto eRPAPInvoiceMemoInformationDto = await base.ERPAPInvoiceMemoRepository.GetAPInvoiceMemo(aPInvoiceMemoId);
				aPInvoiceMemoDto = new ERPAPInvoiceMemoDto
				{
					apiApInvoiceID = eRPAPInvoiceMemoInformationDto.apiApInvoiceID,
					apiCreatedBy = eRPAPInvoiceMemoInformationDto.apiCreatedBy,
					apiCreatedDate = eRPAPInvoiceMemoInformationDto.apiCreatedDate,
					apiUniqueID = eRPAPInvoiceMemoInformationDto.apiUniqueID,
					apiLongDescriptionRtf = eRPAPInvoiceMemoInformationDto.apiLongDescriptionRtf,
					apiLongDescriptionText = eRPAPInvoiceMemoInformationDto.apiLongDescriptionText,
					apiMemoDate = eRPAPInvoiceMemoInformationDto.apiMemoDate,
					apiRowVersion = eRPAPInvoiceMemoInformationDto.apiRowVersion,
					apiApInvoiceMemoID = eRPAPInvoiceMemoInformationDto.apiApInvoiceMemoID,
					apiShortDescription = eRPAPInvoiceMemoInformationDto.apiShortDescription,
					apiShowInApInvoices = eRPAPInvoiceMemoInformationDto.apiShowInApInvoices,
					apiShowInApPayments = eRPAPInvoiceMemoInformationDto.apiShowInApPayments,
					CustomFields = eRPAPInvoiceMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the APInvoiceMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = aPInvoiceMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_PutAPInvoiceMemo(ERPAPInvoiceMemoDto aPInvoiceMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPAPInvoiceMemoDto createdObject = null;
		ERPResponseMessageDto<ERPAPInvoiceMemoDto> result;
		try
		{
			IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
			using (iERPAPInvoiceMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPAPInvoiceMemoRepository.SaveAPInvoiceMemo(aPInvoiceMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPAPInvoiceMemoInformationDto eRPAPInvoiceMemoInformationDto = await base.ERPAPInvoiceMemoRepository.GetAPInvoiceMemo(aPInvoiceMemo.apiUniqueID);
					createdObject = new ERPAPInvoiceMemoDto
					{
						apiApInvoiceID = eRPAPInvoiceMemoInformationDto.apiApInvoiceID,
						apiCreatedBy = eRPAPInvoiceMemoInformationDto.apiCreatedBy,
						apiCreatedDate = eRPAPInvoiceMemoInformationDto.apiCreatedDate,
						apiUniqueID = eRPAPInvoiceMemoInformationDto.apiUniqueID,
						apiLongDescriptionRtf = eRPAPInvoiceMemoInformationDto.apiLongDescriptionRtf,
						apiLongDescriptionText = eRPAPInvoiceMemoInformationDto.apiLongDescriptionText,
						apiMemoDate = eRPAPInvoiceMemoInformationDto.apiMemoDate,
						apiRowVersion = eRPAPInvoiceMemoInformationDto.apiRowVersion,
						apiApInvoiceMemoID = eRPAPInvoiceMemoInformationDto.apiApInvoiceMemoID,
						apiShortDescription = eRPAPInvoiceMemoInformationDto.apiShortDescription,
						apiShowInApInvoices = eRPAPInvoiceMemoInformationDto.apiShowInApInvoices,
						apiShowInApPayments = eRPAPInvoiceMemoInformationDto.apiShowInApPayments,
						CustomFields = eRPAPInvoiceMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing APInvoiceMemo [{aPInvoiceMemo.apiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteAPInvoiceMemo(Guid aPInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
		using (iERPAPInvoiceMemoRepository)
		{
			if (!(await base.ERPAPInvoiceMemoRepository.DoesAPInvoiceMemoExist(aPInvoiceMemoId)))
			{
				base.ErrorsList.Add($"APInvoiceMemo [{aPInvoiceMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPAPInvoiceMemoInformationDto eRPAPInvoiceMemoInformationDto = await base.ERPAPInvoiceMemoRepository.GetAPInvoiceMemo(aPInvoiceMemoId);
				string text = await base.ERPAPInvoiceMemoRepository.WhereUsed("APInvoiceMemos", new object[2] { eRPAPInvoiceMemoInformationDto.apiApInvoiceID, eRPAPInvoiceMemoInformationDto.apiApInvoiceMemoID }, new object[2] { "apiApInvoiceID", "apiApInvoiceMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("APInvoiceMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPAPInvoiceMemoDto>> Process_DeleteAPInvoiceMemo(Guid aPInvoiceMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPAPInvoiceMemoDto> result;
		try
		{
			IERPAPInvoiceMemoRepository iERPAPInvoiceMemoRepository = (base.ERPAPInvoiceMemoRepository = new ERPAPInvoiceMemoRepository(base.ApiClientContext));
			using (iERPAPInvoiceMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPAPInvoiceMemoRepository.DeleteRowFromTable("APInvoiceMemos", "api", aPInvoiceMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of APInvoiceMemo [{aPInvoiceMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPAPInvoiceMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPAPInvoiceMemoDto()
			};
		}
		return result;
	}
}
