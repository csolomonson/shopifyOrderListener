using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartPriceModel : ERPBaseModel, IERPPartPriceModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartPrices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
		using (iERPPartPriceRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartPriceRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartPriceRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartPriceRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartPriceRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartPrice(Guid partPriceId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
		using (iERPPartPriceRepository)
		{
			if (!(await base.ERPPartPriceRepository.DoesPartPriceExist(partPriceId)))
			{
				errorsList.Add($"PartPrice [{partPriceId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartPrice(ERPPartPriceDto partPrice)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
		using (iERPPartPriceRepository)
		{
			if (!string.IsNullOrWhiteSpace(partPrice.imiPartGroupID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { partPrice.imiPartGroupID })))
			{
				errorsList.Add("imiPartGroupID [" + partPrice.imiPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiPartID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partPrice.imiPartID })))
			{
				errorsList.Add("imiPartID [" + partPrice.imiPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiPartRevisionID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partPrice.imiPartID, partPrice.imiPartRevisionID })))
			{
				errorsList.Add("imiPartRevisionID [" + partPrice.imiPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiCustomerGroupID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("CustomerGroups", new object[1] { "CMUCUSTOMERGROUPID" }, new object[1] { partPrice.imiCustomerGroupID })))
			{
				errorsList.Add("imiCustomerGroupID [" + partPrice.imiCustomerGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiOrganizationID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { partPrice.imiOrganizationID })))
			{
				errorsList.Add("imiOrganizationID [" + partPrice.imiOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiCurrencyRateID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { partPrice.imiCurrencyRateID })))
			{
				errorsList.Add("imiCurrencyRateID [" + partPrice.imiCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiQuoteID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { partPrice.imiQuoteID })))
			{
				errorsList.Add("imiQuoteID [" + partPrice.imiQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiRfqID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { partPrice.imiRfqID })))
			{
				errorsList.Add("imiRfqID [" + partPrice.imiRfqID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partPrice.imiLocationID) && !(await base.ERPPartPriceRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { partPrice.imiOrganizationID, partPrice.imiLocationID })))
			{
				errorsList.Add("imiLocationID [" + partPrice.imiLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartPriceDto>>> Process_GetAllPartPrices(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartPriceDto> allPartPricesDto = new List<ERPPartPriceDto>();
		ERPResponseMessageDto<IList<ERPPartPriceDto>> result;
		try
		{
			IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
			using (iERPPartPriceRepository)
			{
				foreach (ERPPartPriceInformationDto item2 in await base.ERPPartPriceRepository.GetAllPartPrices(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartPriceDto item = new ERPPartPriceDto
					{
						imiCreatedBy = item2.imiCreatedBy,
						imiCreatedDate = item2.imiCreatedDate,
						imiCurrencyRateID = item2.imiCurrencyRateID,
						imiCustomerGroupID = item2.imiCustomerGroupID,
						imiEndDate = item2.imiEndDate,
						imiUniqueID = item2.imiUniqueID,
						imiInventoryPrice = item2.imiInventoryPrice,
						imiLocationID = item2.imiLocationID,
						imiOrganizationID = item2.imiOrganizationID,
						imiPartGroupID = item2.imiPartGroupID,
						imiPartID = item2.imiPartID,
						imiPartRevisionID = item2.imiPartRevisionID,
						imiPriceType = item2.imiPriceType,
						imiQuoteID = item2.imiQuoteID,
						imiRfqID = item2.imiRfqID,
						imiRowVersion = item2.imiRowVersion,
						imiPartPriceID = item2.imiPartPriceID,
						imiStartDate = item2.imiStartDate,
						CustomFields = item2.CustomFields
					};
					allPartPricesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartPrices]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartPriceDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartPricesDto,
				RecordCount = allPartPricesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_GetPartPrice(Guid partPriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartPriceDto partPriceDto = null;
		ERPResponseMessageDto<ERPPartPriceDto> result;
		try
		{
			IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
			using (iERPPartPriceRepository)
			{
				ERPPartPriceInformationDto eRPPartPriceInformationDto = await base.ERPPartPriceRepository.GetPartPrice(partPriceId);
				partPriceDto = new ERPPartPriceDto
				{
					imiCreatedBy = eRPPartPriceInformationDto.imiCreatedBy,
					imiCreatedDate = eRPPartPriceInformationDto.imiCreatedDate,
					imiCurrencyRateID = eRPPartPriceInformationDto.imiCurrencyRateID,
					imiCustomerGroupID = eRPPartPriceInformationDto.imiCustomerGroupID,
					imiEndDate = eRPPartPriceInformationDto.imiEndDate,
					imiUniqueID = eRPPartPriceInformationDto.imiUniqueID,
					imiInventoryPrice = eRPPartPriceInformationDto.imiInventoryPrice,
					imiLocationID = eRPPartPriceInformationDto.imiLocationID,
					imiOrganizationID = eRPPartPriceInformationDto.imiOrganizationID,
					imiPartGroupID = eRPPartPriceInformationDto.imiPartGroupID,
					imiPartID = eRPPartPriceInformationDto.imiPartID,
					imiPartRevisionID = eRPPartPriceInformationDto.imiPartRevisionID,
					imiPriceType = eRPPartPriceInformationDto.imiPriceType,
					imiQuoteID = eRPPartPriceInformationDto.imiQuoteID,
					imiRfqID = eRPPartPriceInformationDto.imiRfqID,
					imiRowVersion = eRPPartPriceInformationDto.imiRowVersion,
					imiPartPriceID = eRPPartPriceInformationDto.imiPartPriceID,
					imiStartDate = eRPPartPriceInformationDto.imiStartDate,
					CustomFields = eRPPartPriceInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartPrices []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partPriceDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_PutPartPrice(ERPPartPriceDto partPrice)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartPriceDto createdObject = null;
		ERPResponseMessageDto<ERPPartPriceDto> result;
		try
		{
			IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
			using (iERPPartPriceRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartPriceRepository.SavePartPrice(partPrice);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartPriceInformationDto eRPPartPriceInformationDto = await base.ERPPartPriceRepository.GetPartPrice(partPrice.imiUniqueID);
					createdObject = new ERPPartPriceDto
					{
						imiCreatedBy = eRPPartPriceInformationDto.imiCreatedBy,
						imiCreatedDate = eRPPartPriceInformationDto.imiCreatedDate,
						imiCurrencyRateID = eRPPartPriceInformationDto.imiCurrencyRateID,
						imiCustomerGroupID = eRPPartPriceInformationDto.imiCustomerGroupID,
						imiEndDate = eRPPartPriceInformationDto.imiEndDate,
						imiUniqueID = eRPPartPriceInformationDto.imiUniqueID,
						imiInventoryPrice = eRPPartPriceInformationDto.imiInventoryPrice,
						imiLocationID = eRPPartPriceInformationDto.imiLocationID,
						imiOrganizationID = eRPPartPriceInformationDto.imiOrganizationID,
						imiPartGroupID = eRPPartPriceInformationDto.imiPartGroupID,
						imiPartID = eRPPartPriceInformationDto.imiPartID,
						imiPartRevisionID = eRPPartPriceInformationDto.imiPartRevisionID,
						imiPriceType = eRPPartPriceInformationDto.imiPriceType,
						imiQuoteID = eRPPartPriceInformationDto.imiQuoteID,
						imiRfqID = eRPPartPriceInformationDto.imiRfqID,
						imiRowVersion = eRPPartPriceInformationDto.imiRowVersion,
						imiPartPriceID = eRPPartPriceInformationDto.imiPartPriceID,
						imiStartDate = eRPPartPriceInformationDto.imiStartDate,
						CustomFields = eRPPartPriceInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartPrice [{partPrice.imiUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartPrice(Guid partPriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
		using (iERPPartPriceRepository)
		{
			if (!(await base.ERPPartPriceRepository.DoesPartPriceExist(partPriceId)))
			{
				base.ErrorsList.Add($"PartPrice [{partPriceId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartPriceInformationDto eRPPartPriceInformationDto = await base.ERPPartPriceRepository.GetPartPrice(partPriceId);
				string text = await base.ERPPartPriceRepository.WhereUsed("PartPrices", new object[1] { eRPPartPriceInformationDto.imiPartPriceID }, new object[1] { "imiPartPriceID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartPrice cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartPriceDto>> Process_DeletePartPrice(Guid partPriceId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartPriceDto> result;
		try
		{
			IERPPartPriceRepository iERPPartPriceRepository = (base.ERPPartPriceRepository = new ERPPartPriceRepository(base.ApiClientContext));
			using (iERPPartPriceRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartPriceRepository.DeleteRowFromTable("PartPrices", "imi", partPriceId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartPrice [{partPriceId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartPriceDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartPriceDto()
			};
		}
		return result;
	}
}
