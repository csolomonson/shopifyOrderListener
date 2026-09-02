using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartUnitSalePriceModel : ERPBaseModel, IERPPartUnitSalePriceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartUnitSalePrices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
		using (iERPPartUnitSalePriceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartUnitSalePriceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartUnitSalePriceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartUnitSalePriceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartUnitSalePriceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartUnitSalePrice(Guid partUnitSalePriceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
		using (iERPPartUnitSalePriceRepository)
		{
			if (!(await base.ERPPartUnitSalePriceRepository.DoesPartUnitSalePriceExist(partUnitSalePriceId)))
			{
				errorsList.Add($"PartUnitSalePrice [{partUnitSalePriceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
		using (iERPPartUnitSalePriceRepository)
		{
			if (!string.IsNullOrWhiteSpace(partUnitSalePrice.imhPartID) && !(await base.ERPPartUnitSalePriceRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partUnitSalePrice.imhPartID })))
			{
				errorsList.Add("imhPartID [" + partUnitSalePrice.imhPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partUnitSalePrice.imhPartRevisionID) && !(await base.ERPPartUnitSalePriceRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partUnitSalePrice.imhPartID, partUnitSalePrice.imhPartRevisionID })))
			{
				errorsList.Add("imhPartRevisionID [" + partUnitSalePrice.imhPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partUnitSalePrice.imhCurrencyRateID) && !(await base.ERPPartUnitSalePriceRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { partUnitSalePrice.imhCurrencyRateID })))
			{
				errorsList.Add("imhCurrencyRateID [" + partUnitSalePrice.imhCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartUnitSalePriceDto>>> Process_GetAllPartUnitSalePrices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartUnitSalePriceDto> allPartUnitSalePricesDto = new List<ERPPartUnitSalePriceDto>();
		ERPResponseMessageDto<IList<ERPPartUnitSalePriceDto>> result;
		try
		{
			IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
			using (iERPPartUnitSalePriceRepository)
			{
				foreach (ERPPartUnitSalePriceInformationDto item2 in await base.ERPPartUnitSalePriceRepository.GetAllPartUnitSalePrices(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartUnitSalePriceDto item = new ERPPartUnitSalePriceDto
					{
						imhCreatedBy = item2.imhCreatedBy,
						imhCreatedDate = item2.imhCreatedDate,
						imhCurrencyRateID = item2.imhCurrencyRateID,
						imhEndDate = item2.imhEndDate,
						imhUniqueID = item2.imhUniqueID,
						imhPartID = item2.imhPartID,
						imhPartRevisionID = item2.imhPartRevisionID,
						imhRowVersion = item2.imhRowVersion,
						imhPartUnitSalePriceID = item2.imhPartUnitSalePriceID,
						imhStartDate = item2.imhStartDate,
						imhUnitSalePrice = item2.imhUnitSalePrice,
						CustomFields = item2.CustomFields
					};
					allPartUnitSalePricesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartUnitSalePrices]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartUnitSalePriceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartUnitSalePricesDto,
				RecordCount = allPartUnitSalePricesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_GetPartUnitSalePrice(Guid partUnitSalePriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartUnitSalePriceDto partUnitSalePriceDto = null;
		ERPResponseMessageDto<ERPPartUnitSalePriceDto> result;
		try
		{
			IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
			using (iERPPartUnitSalePriceRepository)
			{
				ERPPartUnitSalePriceInformationDto eRPPartUnitSalePriceInformationDto = await base.ERPPartUnitSalePriceRepository.GetPartUnitSalePrice(partUnitSalePriceId);
				partUnitSalePriceDto = new ERPPartUnitSalePriceDto
				{
					imhCreatedBy = eRPPartUnitSalePriceInformationDto.imhCreatedBy,
					imhCreatedDate = eRPPartUnitSalePriceInformationDto.imhCreatedDate,
					imhCurrencyRateID = eRPPartUnitSalePriceInformationDto.imhCurrencyRateID,
					imhEndDate = eRPPartUnitSalePriceInformationDto.imhEndDate,
					imhUniqueID = eRPPartUnitSalePriceInformationDto.imhUniqueID,
					imhPartID = eRPPartUnitSalePriceInformationDto.imhPartID,
					imhPartRevisionID = eRPPartUnitSalePriceInformationDto.imhPartRevisionID,
					imhRowVersion = eRPPartUnitSalePriceInformationDto.imhRowVersion,
					imhPartUnitSalePriceID = eRPPartUnitSalePriceInformationDto.imhPartUnitSalePriceID,
					imhStartDate = eRPPartUnitSalePriceInformationDto.imhStartDate,
					imhUnitSalePrice = eRPPartUnitSalePriceInformationDto.imhUnitSalePrice,
					CustomFields = eRPPartUnitSalePriceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartUnitSalePrices []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartUnitSalePriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partUnitSalePriceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_PutPartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartUnitSalePriceDto createdObject = null;
		ERPResponseMessageDto<ERPPartUnitSalePriceDto> result;
		try
		{
			IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
			using (iERPPartUnitSalePriceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartUnitSalePriceRepository.SavePartUnitSalePrice(partUnitSalePrice);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartUnitSalePriceInformationDto eRPPartUnitSalePriceInformationDto = await base.ERPPartUnitSalePriceRepository.GetPartUnitSalePrice(partUnitSalePrice.imhUniqueID);
					createdObject = new ERPPartUnitSalePriceDto
					{
						imhCreatedBy = eRPPartUnitSalePriceInformationDto.imhCreatedBy,
						imhCreatedDate = eRPPartUnitSalePriceInformationDto.imhCreatedDate,
						imhCurrencyRateID = eRPPartUnitSalePriceInformationDto.imhCurrencyRateID,
						imhEndDate = eRPPartUnitSalePriceInformationDto.imhEndDate,
						imhUniqueID = eRPPartUnitSalePriceInformationDto.imhUniqueID,
						imhPartID = eRPPartUnitSalePriceInformationDto.imhPartID,
						imhPartRevisionID = eRPPartUnitSalePriceInformationDto.imhPartRevisionID,
						imhRowVersion = eRPPartUnitSalePriceInformationDto.imhRowVersion,
						imhPartUnitSalePriceID = eRPPartUnitSalePriceInformationDto.imhPartUnitSalePriceID,
						imhStartDate = eRPPartUnitSalePriceInformationDto.imhStartDate,
						imhUnitSalePrice = eRPPartUnitSalePriceInformationDto.imhUnitSalePrice,
						CustomFields = eRPPartUnitSalePriceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartUnitSalePrice [{partUnitSalePrice.imhUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartUnitSalePriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartUnitSalePrice(Guid partUnitSalePriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
		using (iERPPartUnitSalePriceRepository)
		{
			if (!(await base.ERPPartUnitSalePriceRepository.DoesPartUnitSalePriceExist(partUnitSalePriceId)))
			{
				base.ErrorsList.Add($"PartUnitSalePrice [{partUnitSalePriceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartUnitSalePriceInformationDto eRPPartUnitSalePriceInformationDto = await base.ERPPartUnitSalePriceRepository.GetPartUnitSalePrice(partUnitSalePriceId);
				string text = await base.ERPPartUnitSalePriceRepository.WhereUsed("PartUnitSalePrices", new object[3] { eRPPartUnitSalePriceInformationDto.imhPartID, eRPPartUnitSalePriceInformationDto.imhPartRevisionID, eRPPartUnitSalePriceInformationDto.imhPartUnitSalePriceID }, new object[3] { "imhPartID", "imhPartRevisionID", "imhPartUnitSalePriceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartUnitSalePrice cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartUnitSalePriceDto>> Process_DeletePartUnitSalePrice(Guid partUnitSalePriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartUnitSalePriceDto> result;
		try
		{
			IERPPartUnitSalePriceRepository iERPPartUnitSalePriceRepository = (base.ERPPartUnitSalePriceRepository = new ERPPartUnitSalePriceRepository(base.ApiClientContext));
			using (iERPPartUnitSalePriceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartUnitSalePriceRepository.DeleteRowFromTable("PartUnitSalePrices", "imh", partUnitSalePriceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartUnitSalePrice [{partUnitSalePriceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartUnitSalePriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartUnitSalePriceDto()
			};
		}
		return result;
	}
}
