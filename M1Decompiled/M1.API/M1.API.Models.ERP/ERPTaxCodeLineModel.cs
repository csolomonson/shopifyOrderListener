using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPTaxCodeLineModel : ERPBaseModel, IERPTaxCodeLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTaxCodeLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
		using (iERPTaxCodeLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPTaxCodeLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPTaxCodeLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPTaxCodeLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPTaxCodeLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTaxCodeLine(Guid taxCodeLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
		using (iERPTaxCodeLineRepository)
		{
			if (!(await base.ERPTaxCodeLineRepository.DoesTaxCodeLineExist(taxCodeLineId)))
			{
				errorsList.Add($"TaxCodeLine [{taxCodeLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTaxCodeLine(ERPTaxCodeLineDto taxCodeLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
		using (iERPTaxCodeLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(taxCodeLine.xabTaxCodeID) && !(await base.ERPTaxCodeLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { taxCodeLine.xabTaxCodeID })))
			{
				errorsList.Add("xabTaxCodeID [" + taxCodeLine.xabTaxCodeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPTaxCodeLineDto>>> Process_GetAllTaxCodeLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPTaxCodeLineDto> allTaxCodeLinesDto = new List<ERPTaxCodeLineDto>();
		ERPResponseMessageDto<IList<ERPTaxCodeLineDto>> result;
		try
		{
			IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
			using (iERPTaxCodeLineRepository)
			{
				foreach (ERPTaxCodeLineInformationDto item2 in await base.ERPTaxCodeLineRepository.GetAllTaxCodeLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPTaxCodeLineDto item = new ERPTaxCodeLineDto
					{
						xabCreatedBy = item2.xabCreatedBy,
						xabCreatedDate = item2.xabCreatedDate,
						xabEffectiveDate = item2.xabEffectiveDate,
						xabUniqueID = item2.xabUniqueID,
						xabRowVersion = item2.xabRowVersion,
						xabTaxCodeLineID = item2.xabTaxCodeLineID,
						xabTaxCodeID = item2.xabTaxCodeID,
						xabTaxRate = item2.xabTaxRate,
						xabTaxRateNotesRTF = item2.xabTaxRateNotesRTF,
						xabTaxRateNotesText = item2.xabTaxRateNotesText,
						CustomFields = item2.CustomFields
					};
					allTaxCodeLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all TaxCodeLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPTaxCodeLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allTaxCodeLinesDto,
				RecordCount = allTaxCodeLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_GetTaxCodeLine(Guid taxCodeLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPTaxCodeLineDto taxCodeLineDto = null;
		ERPResponseMessageDto<ERPTaxCodeLineDto> result;
		try
		{
			IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
			using (iERPTaxCodeLineRepository)
			{
				ERPTaxCodeLineInformationDto eRPTaxCodeLineInformationDto = await base.ERPTaxCodeLineRepository.GetTaxCodeLine(taxCodeLineId);
				taxCodeLineDto = new ERPTaxCodeLineDto
				{
					xabCreatedBy = eRPTaxCodeLineInformationDto.xabCreatedBy,
					xabCreatedDate = eRPTaxCodeLineInformationDto.xabCreatedDate,
					xabEffectiveDate = eRPTaxCodeLineInformationDto.xabEffectiveDate,
					xabUniqueID = eRPTaxCodeLineInformationDto.xabUniqueID,
					xabRowVersion = eRPTaxCodeLineInformationDto.xabRowVersion,
					xabTaxCodeLineID = eRPTaxCodeLineInformationDto.xabTaxCodeLineID,
					xabTaxCodeID = eRPTaxCodeLineInformationDto.xabTaxCodeID,
					xabTaxRate = eRPTaxCodeLineInformationDto.xabTaxRate,
					xabTaxRateNotesRTF = eRPTaxCodeLineInformationDto.xabTaxRateNotesRTF,
					xabTaxRateNotesText = eRPTaxCodeLineInformationDto.xabTaxRateNotesText,
					CustomFields = eRPTaxCodeLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the TaxCodeLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = taxCodeLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_PutTaxCodeLine(ERPTaxCodeLineDto taxCodeLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPTaxCodeLineDto createdObject = null;
		ERPResponseMessageDto<ERPTaxCodeLineDto> result;
		try
		{
			IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
			using (iERPTaxCodeLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPTaxCodeLineRepository.SaveTaxCodeLine(taxCodeLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPTaxCodeLineInformationDto eRPTaxCodeLineInformationDto = await base.ERPTaxCodeLineRepository.GetTaxCodeLine(taxCodeLine.xabUniqueID);
					createdObject = new ERPTaxCodeLineDto
					{
						xabCreatedBy = eRPTaxCodeLineInformationDto.xabCreatedBy,
						xabCreatedDate = eRPTaxCodeLineInformationDto.xabCreatedDate,
						xabEffectiveDate = eRPTaxCodeLineInformationDto.xabEffectiveDate,
						xabUniqueID = eRPTaxCodeLineInformationDto.xabUniqueID,
						xabRowVersion = eRPTaxCodeLineInformationDto.xabRowVersion,
						xabTaxCodeLineID = eRPTaxCodeLineInformationDto.xabTaxCodeLineID,
						xabTaxCodeID = eRPTaxCodeLineInformationDto.xabTaxCodeID,
						xabTaxRate = eRPTaxCodeLineInformationDto.xabTaxRate,
						xabTaxRateNotesRTF = eRPTaxCodeLineInformationDto.xabTaxRateNotesRTF,
						xabTaxRateNotesText = eRPTaxCodeLineInformationDto.xabTaxRateNotesText,
						CustomFields = eRPTaxCodeLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing TaxCodeLine [{taxCodeLine.xabUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTaxCodeLine(Guid taxCodeLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
		using (iERPTaxCodeLineRepository)
		{
			if (!(await base.ERPTaxCodeLineRepository.DoesTaxCodeLineExist(taxCodeLineId)))
			{
				base.ErrorsList.Add($"TaxCodeLine [{taxCodeLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPTaxCodeLineInformationDto eRPTaxCodeLineInformationDto = await base.ERPTaxCodeLineRepository.GetTaxCodeLine(taxCodeLineId);
				string text = await base.ERPTaxCodeLineRepository.WhereUsed("TaxCodeLines", new object[2] { eRPTaxCodeLineInformationDto.xabTaxCodeID, eRPTaxCodeLineInformationDto.xabTaxCodeLineID }, new object[2] { "xabTaxCodeID", "xabTaxCodeLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("TaxCodeLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPTaxCodeLineDto>> Process_DeleteTaxCodeLine(Guid taxCodeLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPTaxCodeLineDto> result;
		try
		{
			IERPTaxCodeLineRepository iERPTaxCodeLineRepository = (base.ERPTaxCodeLineRepository = new ERPTaxCodeLineRepository(base.ApiClientContext));
			using (iERPTaxCodeLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPTaxCodeLineRepository.DeleteRowFromTable("TaxCodeLines", "xab", taxCodeLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of TaxCodeLine [{taxCodeLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPTaxCodeLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPTaxCodeLineDto()
			};
		}
		return result;
	}
}
