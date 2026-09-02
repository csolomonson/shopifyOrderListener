using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRFQSupplierModel : ERPBaseModel, IERPRFQSupplierModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRFQSuppliers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
		using (iERPRFQSupplierRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRFQSupplierRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRFQSupplierRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRFQSupplierRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRFQSupplierRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRFQSupplier(Guid rFQSupplierId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
		using (iERPRFQSupplierRepository)
		{
			if (!(await base.ERPRFQSupplierRepository.DoesRFQSupplierExist(rFQSupplierId)))
			{
				errorsList.Add($"RFQSupplier [{rFQSupplierId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRFQSupplier(ERPRFQSupplierDto rFQSupplier)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
		using (iERPRFQSupplierRepository)
		{
			if (!string.IsNullOrWhiteSpace(rFQSupplier.rqsRfqID) && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { rFQSupplier.rqsRfqID })))
			{
				errorsList.Add("rqsRfqID [" + rFQSupplier.rqsRfqID + "] not found.");
			}
			if (rFQSupplier.rqsRfqLineID > 0 && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("RFQLines", new object[2] { "RQLRFQID", "RQLRFQLINEID" }, new object[2] { rFQSupplier.rqsRfqID, rFQSupplier.rqsRfqLineID })))
			{
				errorsList.Add($"rqsRfqLineID [{rFQSupplier.rqsRfqLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQSupplier.rqsSupplierOrganizationID) && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { rFQSupplier.rqsSupplierOrganizationID })))
			{
				errorsList.Add("rqsSupplierOrganizationID [" + rFQSupplier.rqsSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQSupplier.rqsPurchaseLocationID) && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { rFQSupplier.rqsSupplierOrganizationID, rFQSupplier.rqsPurchaseLocationID })))
			{
				errorsList.Add("rqsPurchaseLocationID [" + rFQSupplier.rqsPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQSupplier.rqsPurchaseContactID) && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { rFQSupplier.rqsSupplierOrganizationID, rFQSupplier.rqsPurchaseLocationID, rFQSupplier.rqsPurchaseContactID })))
			{
				errorsList.Add("rqsPurchaseContactID [" + rFQSupplier.rqsPurchaseContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQSupplier.rqsCurrencyRateID) && !(await base.ERPRFQSupplierRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { rFQSupplier.rqsCurrencyRateID })))
			{
				errorsList.Add("rqsCurrencyRateID [" + rFQSupplier.rqsCurrencyRateID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRFQSupplierDto>>> Process_GetAllRFQSuppliers(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRFQSupplierDto> allRFQSuppliersDto = new List<ERPRFQSupplierDto>();
		ERPResponseMessageDto<IList<ERPRFQSupplierDto>> result;
		try
		{
			IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
			using (iERPRFQSupplierRepository)
			{
				foreach (ERPRFQSupplierInformationDto item2 in await base.ERPRFQSupplierRepository.GetAllRFQSuppliers(pageSize, pageNumber, filter, orderBy))
				{
					ERPRFQSupplierDto item = new ERPRFQSupplierDto
					{
						rqsCreatedBy = item2.rqsCreatedBy,
						rqsCreatedDate = item2.rqsCreatedDate,
						rqsCurrencyRateID = item2.rqsCurrencyRateID,
						rqsDueDate = item2.rqsDueDate,
						rqsUniqueID = item2.rqsUniqueID,
						rqsExchangeRate = item2.rqsExchangeRate,
						rqsClosed = item2.rqsClosed,
						rqsComplete = item2.rqsComplete,
						rqsCustomRate = item2.rqsCustomRate,
						rqsSelectedSupplier = item2.rqsSelectedSupplier,
						rqsUpdatedPartPrices = item2.rqsUpdatedPartPrices,
						rqsOrgPartID = item2.rqsOrgPartID,
						rqsPurchaseContactID = item2.rqsPurchaseContactID,
						rqsPurchaseLocationID = item2.rqsPurchaseLocationID,
						rqsRfqID = item2.rqsRfqID,
						rqsRfqLineID = item2.rqsRfqLineID,
						rqsRowVersion = item2.rqsRowVersion,
						rqsSelectedSupplierDate = item2.rqsSelectedSupplierDate,
						rqsRfqSupplierID = item2.rqsRfqSupplierID,
						rqsSupplierOrganizationID = item2.rqsSupplierOrganizationID,
						CustomFields = item2.CustomFields
					};
					allRFQSuppliersDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RFQSuppliers]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRFQSupplierDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRFQSuppliersDto,
				RecordCount = allRFQSuppliersDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_GetRFQSupplier(Guid rFQSupplierId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRFQSupplierDto rFQSupplierDto = null;
		ERPResponseMessageDto<ERPRFQSupplierDto> result;
		try
		{
			IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
			using (iERPRFQSupplierRepository)
			{
				ERPRFQSupplierInformationDto eRPRFQSupplierInformationDto = await base.ERPRFQSupplierRepository.GetRFQSupplier(rFQSupplierId);
				rFQSupplierDto = new ERPRFQSupplierDto
				{
					rqsCreatedBy = eRPRFQSupplierInformationDto.rqsCreatedBy,
					rqsCreatedDate = eRPRFQSupplierInformationDto.rqsCreatedDate,
					rqsCurrencyRateID = eRPRFQSupplierInformationDto.rqsCurrencyRateID,
					rqsDueDate = eRPRFQSupplierInformationDto.rqsDueDate,
					rqsUniqueID = eRPRFQSupplierInformationDto.rqsUniqueID,
					rqsExchangeRate = eRPRFQSupplierInformationDto.rqsExchangeRate,
					rqsClosed = eRPRFQSupplierInformationDto.rqsClosed,
					rqsComplete = eRPRFQSupplierInformationDto.rqsComplete,
					rqsCustomRate = eRPRFQSupplierInformationDto.rqsCustomRate,
					rqsSelectedSupplier = eRPRFQSupplierInformationDto.rqsSelectedSupplier,
					rqsUpdatedPartPrices = eRPRFQSupplierInformationDto.rqsUpdatedPartPrices,
					rqsOrgPartID = eRPRFQSupplierInformationDto.rqsOrgPartID,
					rqsPurchaseContactID = eRPRFQSupplierInformationDto.rqsPurchaseContactID,
					rqsPurchaseLocationID = eRPRFQSupplierInformationDto.rqsPurchaseLocationID,
					rqsRfqID = eRPRFQSupplierInformationDto.rqsRfqID,
					rqsRfqLineID = eRPRFQSupplierInformationDto.rqsRfqLineID,
					rqsRowVersion = eRPRFQSupplierInformationDto.rqsRowVersion,
					rqsSelectedSupplierDate = eRPRFQSupplierInformationDto.rqsSelectedSupplierDate,
					rqsRfqSupplierID = eRPRFQSupplierInformationDto.rqsRfqSupplierID,
					rqsSupplierOrganizationID = eRPRFQSupplierInformationDto.rqsSupplierOrganizationID,
					CustomFields = eRPRFQSupplierInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RFQSuppliers []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQSupplierDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rFQSupplierDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_PutRFQSupplier(ERPRFQSupplierDto rFQSupplier)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRFQSupplierDto createdObject = null;
		ERPResponseMessageDto<ERPRFQSupplierDto> result;
		try
		{
			IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
			using (iERPRFQSupplierRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRFQSupplierRepository.SaveRFQSupplier(rFQSupplier);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRFQSupplierInformationDto eRPRFQSupplierInformationDto = await base.ERPRFQSupplierRepository.GetRFQSupplier(rFQSupplier.rqsUniqueID);
					createdObject = new ERPRFQSupplierDto
					{
						rqsCreatedBy = eRPRFQSupplierInformationDto.rqsCreatedBy,
						rqsCreatedDate = eRPRFQSupplierInformationDto.rqsCreatedDate,
						rqsCurrencyRateID = eRPRFQSupplierInformationDto.rqsCurrencyRateID,
						rqsDueDate = eRPRFQSupplierInformationDto.rqsDueDate,
						rqsUniqueID = eRPRFQSupplierInformationDto.rqsUniqueID,
						rqsExchangeRate = eRPRFQSupplierInformationDto.rqsExchangeRate,
						rqsClosed = eRPRFQSupplierInformationDto.rqsClosed,
						rqsComplete = eRPRFQSupplierInformationDto.rqsComplete,
						rqsCustomRate = eRPRFQSupplierInformationDto.rqsCustomRate,
						rqsSelectedSupplier = eRPRFQSupplierInformationDto.rqsSelectedSupplier,
						rqsUpdatedPartPrices = eRPRFQSupplierInformationDto.rqsUpdatedPartPrices,
						rqsOrgPartID = eRPRFQSupplierInformationDto.rqsOrgPartID,
						rqsPurchaseContactID = eRPRFQSupplierInformationDto.rqsPurchaseContactID,
						rqsPurchaseLocationID = eRPRFQSupplierInformationDto.rqsPurchaseLocationID,
						rqsRfqID = eRPRFQSupplierInformationDto.rqsRfqID,
						rqsRfqLineID = eRPRFQSupplierInformationDto.rqsRfqLineID,
						rqsRowVersion = eRPRFQSupplierInformationDto.rqsRowVersion,
						rqsSelectedSupplierDate = eRPRFQSupplierInformationDto.rqsSelectedSupplierDate,
						rqsRfqSupplierID = eRPRFQSupplierInformationDto.rqsRfqSupplierID,
						rqsSupplierOrganizationID = eRPRFQSupplierInformationDto.rqsSupplierOrganizationID,
						CustomFields = eRPRFQSupplierInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RFQSupplier [{rFQSupplier.rqsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQSupplierDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRFQSupplier(Guid rFQSupplierId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
		using (iERPRFQSupplierRepository)
		{
			if (!(await base.ERPRFQSupplierRepository.DoesRFQSupplierExist(rFQSupplierId)))
			{
				base.ErrorsList.Add($"RFQSupplier [{rFQSupplierId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRFQSupplierInformationDto eRPRFQSupplierInformationDto = await base.ERPRFQSupplierRepository.GetRFQSupplier(rFQSupplierId);
				string text = await base.ERPRFQSupplierRepository.WhereUsed("RFQSuppliers", new object[3] { eRPRFQSupplierInformationDto.rqsRfqID, eRPRFQSupplierInformationDto.rqsRfqLineID, eRPRFQSupplierInformationDto.rqsRfqSupplierID }, new object[3] { "rqsRfqID", "rqsRfqLineID", "rqsRfqSupplierID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RFQSupplier cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRFQSupplierDto>> Process_DeleteRFQSupplier(Guid rFQSupplierId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRFQSupplierDto> result;
		try
		{
			IERPRFQSupplierRepository iERPRFQSupplierRepository = (base.ERPRFQSupplierRepository = new ERPRFQSupplierRepository(base.ApiClientContext));
			using (iERPRFQSupplierRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRFQSupplierRepository.DeleteRowFromTable("RFQSuppliers", "rqs", rFQSupplierId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RFQSupplier [{rFQSupplierId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQSupplierDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRFQSupplierDto()
			};
		}
		return result;
	}
}
