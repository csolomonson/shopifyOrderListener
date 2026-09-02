using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRFQQuantityModel : ERPBaseModel, IERPRFQQuantityModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRFQQuantities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
		using (iERPRFQQuantityRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRFQQuantityRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRFQQuantityRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRFQQuantityRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRFQQuantityRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRFQQuantity(Guid rFQQuantityId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
		using (iERPRFQQuantityRepository)
		{
			if (!(await base.ERPRFQQuantityRepository.DoesRFQQuantityExist(rFQQuantityId)))
			{
				errorsList.Add($"RFQQuantity [{rFQQuantityId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRFQQuantity(ERPRFQQuantityDto rFQQuantity)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
		using (iERPRFQQuantityRepository)
		{
			if (!string.IsNullOrWhiteSpace(rFQQuantity.rqqRfqID) && !(await base.ERPRFQQuantityRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { rFQQuantity.rqqRfqID })))
			{
				errorsList.Add("rqqRfqID [" + rFQQuantity.rqqRfqID + "] not found.");
			}
			if (rFQQuantity.rqqRfqLineID > 0 && !(await base.ERPRFQQuantityRepository.DoesRecordExistInTableUsingKeys("RFQLines", new object[2] { "RQLRFQID", "RQLRFQLINEID" }, new object[2] { rFQQuantity.rqqRfqID, rFQQuantity.rqqRfqLineID })))
			{
				errorsList.Add($"rqqRfqLineID [{rFQQuantity.rqqRfqLineID}] not found.");
			}
			if (rFQQuantity.rqqRfqSupplierID > 0 && !(await base.ERPRFQQuantityRepository.DoesRecordExistInTableUsingKeys("RFQSuppliers", new object[3] { "RQSRFQID", "RQSRFQLINEID", "RQSRFQSUPPLIERID" }, new object[3] { rFQQuantity.rqqRfqID, rFQQuantity.rqqRfqLineID, rFQQuantity.rqqRfqSupplierID })))
			{
				errorsList.Add($"rqqRfqSupplierID [{rFQQuantity.rqqRfqSupplierID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRFQQuantityDto>>> Process_GetAllRFQQuantities(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRFQQuantityDto> allRFQQuantitiesDto = new List<ERPRFQQuantityDto>();
		ERPResponseMessageDto<IList<ERPRFQQuantityDto>> result;
		try
		{
			IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
			using (iERPRFQQuantityRepository)
			{
				foreach (ERPRFQQuantityInformationDto item2 in await base.ERPRFQQuantityRepository.GetAllRFQQuantities(pageSize, pageNumber, filter, orderBy))
				{
					ERPRFQQuantityDto item = new ERPRFQQuantityDto
					{
						rqqCreatedBy = item2.rqqCreatedBy,
						rqqCreatedDate = item2.rqqCreatedDate,
						rqqUniqueID = item2.rqqUniqueID,
						rqqClosed = item2.rqqClosed,
						rqqLeadTime = item2.rqqLeadTime,
						rqqPriceBase = item2.rqqPriceBase,
						rqqPriceForeign = item2.rqqPriceForeign,
						rqqQuantity = item2.rqqQuantity,
						rqqRfqID = item2.rqqRfqID,
						rqqRfqLineID = item2.rqqRfqLineID,
						rqqRfqSupplierID = item2.rqqRfqSupplierID,
						rqqRowVersion = item2.rqqRowVersion,
						rqqRfqQuantityID = item2.rqqRfqQuantityID,
						CustomFields = item2.CustomFields
					};
					allRFQQuantitiesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RFQQuantities]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRFQQuantityDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRFQQuantitiesDto,
				RecordCount = allRFQQuantitiesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_GetRFQQuantity(Guid rFQQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRFQQuantityDto rFQQuantityDto = null;
		ERPResponseMessageDto<ERPRFQQuantityDto> result;
		try
		{
			IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
			using (iERPRFQQuantityRepository)
			{
				ERPRFQQuantityInformationDto eRPRFQQuantityInformationDto = await base.ERPRFQQuantityRepository.GetRFQQuantity(rFQQuantityId);
				rFQQuantityDto = new ERPRFQQuantityDto
				{
					rqqCreatedBy = eRPRFQQuantityInformationDto.rqqCreatedBy,
					rqqCreatedDate = eRPRFQQuantityInformationDto.rqqCreatedDate,
					rqqUniqueID = eRPRFQQuantityInformationDto.rqqUniqueID,
					rqqClosed = eRPRFQQuantityInformationDto.rqqClosed,
					rqqLeadTime = eRPRFQQuantityInformationDto.rqqLeadTime,
					rqqPriceBase = eRPRFQQuantityInformationDto.rqqPriceBase,
					rqqPriceForeign = eRPRFQQuantityInformationDto.rqqPriceForeign,
					rqqQuantity = eRPRFQQuantityInformationDto.rqqQuantity,
					rqqRfqID = eRPRFQQuantityInformationDto.rqqRfqID,
					rqqRfqLineID = eRPRFQQuantityInformationDto.rqqRfqLineID,
					rqqRfqSupplierID = eRPRFQQuantityInformationDto.rqqRfqSupplierID,
					rqqRowVersion = eRPRFQQuantityInformationDto.rqqRowVersion,
					rqqRfqQuantityID = eRPRFQQuantityInformationDto.rqqRfqQuantityID,
					CustomFields = eRPRFQQuantityInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RFQQuantities []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rFQQuantityDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_PutRFQQuantity(ERPRFQQuantityDto rFQQuantity)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRFQQuantityDto createdObject = null;
		ERPResponseMessageDto<ERPRFQQuantityDto> result;
		try
		{
			IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
			using (iERPRFQQuantityRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRFQQuantityRepository.SaveRFQQuantity(rFQQuantity);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRFQQuantityInformationDto eRPRFQQuantityInformationDto = await base.ERPRFQQuantityRepository.GetRFQQuantity(rFQQuantity.rqqUniqueID);
					createdObject = new ERPRFQQuantityDto
					{
						rqqCreatedBy = eRPRFQQuantityInformationDto.rqqCreatedBy,
						rqqCreatedDate = eRPRFQQuantityInformationDto.rqqCreatedDate,
						rqqUniqueID = eRPRFQQuantityInformationDto.rqqUniqueID,
						rqqClosed = eRPRFQQuantityInformationDto.rqqClosed,
						rqqLeadTime = eRPRFQQuantityInformationDto.rqqLeadTime,
						rqqPriceBase = eRPRFQQuantityInformationDto.rqqPriceBase,
						rqqPriceForeign = eRPRFQQuantityInformationDto.rqqPriceForeign,
						rqqQuantity = eRPRFQQuantityInformationDto.rqqQuantity,
						rqqRfqID = eRPRFQQuantityInformationDto.rqqRfqID,
						rqqRfqLineID = eRPRFQQuantityInformationDto.rqqRfqLineID,
						rqqRfqSupplierID = eRPRFQQuantityInformationDto.rqqRfqSupplierID,
						rqqRowVersion = eRPRFQQuantityInformationDto.rqqRowVersion,
						rqqRfqQuantityID = eRPRFQQuantityInformationDto.rqqRfqQuantityID,
						CustomFields = eRPRFQQuantityInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RFQQuantity [{rFQQuantity.rqqUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRFQQuantity(Guid rFQQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
		using (iERPRFQQuantityRepository)
		{
			if (!(await base.ERPRFQQuantityRepository.DoesRFQQuantityExist(rFQQuantityId)))
			{
				base.ErrorsList.Add($"RFQQuantity [{rFQQuantityId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRFQQuantityInformationDto eRPRFQQuantityInformationDto = await base.ERPRFQQuantityRepository.GetRFQQuantity(rFQQuantityId);
				string text = await base.ERPRFQQuantityRepository.WhereUsed("RFQQuantities", new object[4] { eRPRFQQuantityInformationDto.rqqRfqID, eRPRFQQuantityInformationDto.rqqRfqLineID, eRPRFQQuantityInformationDto.rqqRfqSupplierID, eRPRFQQuantityInformationDto.rqqRfqQuantityID }, new object[4] { "rqqRfqID", "rqqRfqLineID", "rqqRfqSupplierID", "rqqRfqQuantityID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RFQQuantity cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRFQQuantityDto>> Process_DeleteRFQQuantity(Guid rFQQuantityId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRFQQuantityDto> result;
		try
		{
			IERPRFQQuantityRepository iERPRFQQuantityRepository = (base.ERPRFQQuantityRepository = new ERPRFQQuantityRepository(base.ApiClientContext));
			using (iERPRFQQuantityRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRFQQuantityRepository.DeleteRowFromTable("RFQQuantities", "rqq", rFQQuantityId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RFQQuantity [{rFQQuantityId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQQuantityDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRFQQuantityDto()
			};
		}
		return result;
	}
}
