using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPFreightReferenceModel : ERPBaseModel, IERPFreightReferenceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
		using (iERPFreightReferenceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPFreightReferenceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPFreightReferenceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPFreightReferenceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPFreightReferenceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetFreightReference(Guid freightReferenceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
		using (iERPFreightReferenceRepository)
		{
			if (!(await base.ERPFreightReferenceRepository.DoesFreightReferenceExist(freightReferenceId)))
			{
				errorsList.Add($"FreightReference [{freightReferenceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutFreightReference(ERPFreightReferenceDto freightReference)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
		using (iERPFreightReferenceRepository)
		{
			if (!string.IsNullOrWhiteSpace(freightReference.frcQuoteID) && !(await base.ERPFreightReferenceRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { freightReference.frcQuoteID })))
			{
				errorsList.Add("frcQuoteID [" + freightReference.frcQuoteID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPFreightReferenceDto>>> Process_GetAllFreightReferences(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPFreightReferenceDto> allFreightReferencesDto = new List<ERPFreightReferenceDto>();
		ERPResponseMessageDto<IList<ERPFreightReferenceDto>> result;
		try
		{
			IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
			using (iERPFreightReferenceRepository)
			{
				foreach (ERPFreightReferenceInformationDto item2 in await base.ERPFreightReferenceRepository.GetAllFreightReferences(pageSize, pageNumber, filter, orderBy))
				{
					ERPFreightReferenceDto item = new ERPFreightReferenceDto
					{
						frcFreightReferenceID = item2.frcFreightReferenceID,
						frcUniqueID = item2.frcUniqueID,
						frcFreightShipmentID = item2.frcFreightShipmentID,
						frcQuoteID = item2.frcQuoteID,
						frcRowVersion = item2.frcRowVersion,
						frcShipmentID = item2.frcShipmentID,
						CustomFields = item2.CustomFields
					};
					allFreightReferencesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all FreightReferences]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPFreightReferenceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allFreightReferencesDto,
				RecordCount = allFreightReferencesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_GetFreightReference(Guid freightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPFreightReferenceDto freightReferenceDto = null;
		ERPResponseMessageDto<ERPFreightReferenceDto> result;
		try
		{
			IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
			using (iERPFreightReferenceRepository)
			{
				ERPFreightReferenceInformationDto eRPFreightReferenceInformationDto = await base.ERPFreightReferenceRepository.GetFreightReference(freightReferenceId);
				freightReferenceDto = new ERPFreightReferenceDto
				{
					frcFreightReferenceID = eRPFreightReferenceInformationDto.frcFreightReferenceID,
					frcUniqueID = eRPFreightReferenceInformationDto.frcUniqueID,
					frcFreightShipmentID = eRPFreightReferenceInformationDto.frcFreightShipmentID,
					frcQuoteID = eRPFreightReferenceInformationDto.frcQuoteID,
					frcRowVersion = eRPFreightReferenceInformationDto.frcRowVersion,
					frcShipmentID = eRPFreightReferenceInformationDto.frcShipmentID,
					CustomFields = eRPFreightReferenceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the FreightReferences []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = freightReferenceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_PutFreightReference(ERPFreightReferenceDto freightReference)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPFreightReferenceDto createdObject = null;
		ERPResponseMessageDto<ERPFreightReferenceDto> result;
		try
		{
			IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
			using (iERPFreightReferenceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPFreightReferenceRepository.SaveFreightReference(freightReference);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPFreightReferenceInformationDto eRPFreightReferenceInformationDto = await base.ERPFreightReferenceRepository.GetFreightReference(freightReference.frcUniqueID);
					createdObject = new ERPFreightReferenceDto
					{
						frcFreightReferenceID = eRPFreightReferenceInformationDto.frcFreightReferenceID,
						frcUniqueID = eRPFreightReferenceInformationDto.frcUniqueID,
						frcFreightShipmentID = eRPFreightReferenceInformationDto.frcFreightShipmentID,
						frcQuoteID = eRPFreightReferenceInformationDto.frcQuoteID,
						frcRowVersion = eRPFreightReferenceInformationDto.frcRowVersion,
						frcShipmentID = eRPFreightReferenceInformationDto.frcShipmentID,
						CustomFields = eRPFreightReferenceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing FreightReference [{freightReference.frcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteFreightReference(Guid freightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
		using (iERPFreightReferenceRepository)
		{
			if (!(await base.ERPFreightReferenceRepository.DoesFreightReferenceExist(freightReferenceId)))
			{
				base.ErrorsList.Add($"FreightReference [{freightReferenceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPFreightReferenceInformationDto eRPFreightReferenceInformationDto = await base.ERPFreightReferenceRepository.GetFreightReference(freightReferenceId);
				string text = await base.ERPFreightReferenceRepository.WhereUsed("FreightReferences", new object[1] { eRPFreightReferenceInformationDto.frcFreightReferenceID }, new object[1] { "frcFreightReferenceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("FreightReference cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPFreightReferenceDto>> Process_DeleteFreightReference(Guid freightReferenceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPFreightReferenceDto> result;
		try
		{
			IERPFreightReferenceRepository iERPFreightReferenceRepository = (base.ERPFreightReferenceRepository = new ERPFreightReferenceRepository(base.ApiClientContext));
			using (iERPFreightReferenceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPFreightReferenceRepository.DeleteRowFromTable("FreightReferences", "frc", freightReferenceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of FreightReference [{freightReferenceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPFreightReferenceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPFreightReferenceDto()
			};
		}
		return result;
	}
}
