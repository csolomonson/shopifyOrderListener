using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPOrganizationMemoModel : ERPBaseModel, IERPOrganizationMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllOrganizationMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
		using (iERPOrganizationMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPOrganizationMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPOrganizationMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPOrganizationMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPOrganizationMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetOrganizationMemo(Guid organizationMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
		using (iERPOrganizationMemoRepository)
		{
			if (!(await base.ERPOrganizationMemoRepository.DoesOrganizationMemoExist(organizationMemoId)))
			{
				errorsList.Add($"OrganizationMemo [{organizationMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutOrganizationMemo(ERPOrganizationMemoDto organizationMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
		using (iERPOrganizationMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(organizationMemo.cmmOrganizationID) && !(await base.ERPOrganizationMemoRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { organizationMemo.cmmOrganizationID })))
			{
				errorsList.Add("cmmOrganizationID [" + organizationMemo.cmmOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationMemo.cmmLocationID) && !(await base.ERPOrganizationMemoRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { organizationMemo.cmmOrganizationID, organizationMemo.cmmLocationID })))
			{
				errorsList.Add("cmmLocationID [" + organizationMemo.cmmLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(organizationMemo.cmmContactID) && !(await base.ERPOrganizationMemoRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { organizationMemo.cmmOrganizationID, organizationMemo.cmmLocationID, organizationMemo.cmmContactID })))
			{
				errorsList.Add("cmmContactID [" + organizationMemo.cmmContactID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPOrganizationMemoDto>>> Process_GetAllOrganizationMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPOrganizationMemoDto> allOrganizationMemosDto = new List<ERPOrganizationMemoDto>();
		ERPResponseMessageDto<IList<ERPOrganizationMemoDto>> result;
		try
		{
			IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
			using (iERPOrganizationMemoRepository)
			{
				foreach (ERPOrganizationMemoInformationDto item2 in await base.ERPOrganizationMemoRepository.GetAllOrganizationMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPOrganizationMemoDto item = new ERPOrganizationMemoDto
					{
						cmmContactID = item2.cmmContactID,
						cmmCreatedBy = item2.cmmCreatedBy,
						cmmCreatedDate = item2.cmmCreatedDate,
						cmmUniqueID = item2.cmmUniqueID,
						cmmLocationID = item2.cmmLocationID,
						cmmLongDescriptionRtf = item2.cmmLongDescriptionRtf,
						cmmLongDescriptionText = item2.cmmLongDescriptionText,
						cmmMemoDate = item2.cmmMemoDate,
						cmmOrganizationID = item2.cmmOrganizationID,
						cmmRowVersion = item2.cmmRowVersion,
						cmmOrganizationMemoID = item2.cmmOrganizationMemoID,
						cmmShortDescription = item2.cmmShortDescription,
						cmmShowInApInvoices = item2.cmmShowInApInvoices,
						cmmShowInApPayments = item2.cmmShowInApPayments,
						cmmShowInArInvoices = item2.cmmShowInArInvoices,
						cmmShowInArPayments = item2.cmmShowInArPayments,
						cmmShowInCalls = item2.cmmShowInCalls,
						cmmShowInDmrClaims = item2.cmmShowInDmrClaims,
						cmmShowInDmrShipments = item2.cmmShowInDmrShipments,
						cmmShowInLeads = item2.cmmShowInLeads,
						cmmShowInOrganizations = item2.cmmShowInOrganizations,
						cmmShowInPriceAndAvailability = item2.cmmShowInPriceAndAvailability,
						cmmShowInPurchaseOrders = item2.cmmShowInPurchaseOrders,
						cmmShowInQuotes = item2.cmmShowInQuotes,
						cmmShowInReceipts = item2.cmmShowInReceipts,
						cmmShowInRfqs = item2.cmmShowInRfqs,
						cmmShowInRmaClaims = item2.cmmShowInRmaClaims,
						cmmShowInRmaReceipts = item2.cmmShowInRmaReceipts,
						cmmShowInSalesOrders = item2.cmmShowInSalesOrders,
						cmmShowInShipments = item2.cmmShowInShipments,
						CustomFields = item2.CustomFields
					};
					allOrganizationMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all OrganizationMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPOrganizationMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allOrganizationMemosDto,
				RecordCount = allOrganizationMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_GetOrganizationMemo(Guid organizationMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPOrganizationMemoDto organizationMemoDto = null;
		ERPResponseMessageDto<ERPOrganizationMemoDto> result;
		try
		{
			IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
			using (iERPOrganizationMemoRepository)
			{
				ERPOrganizationMemoInformationDto eRPOrganizationMemoInformationDto = await base.ERPOrganizationMemoRepository.GetOrganizationMemo(organizationMemoId);
				organizationMemoDto = new ERPOrganizationMemoDto
				{
					cmmContactID = eRPOrganizationMemoInformationDto.cmmContactID,
					cmmCreatedBy = eRPOrganizationMemoInformationDto.cmmCreatedBy,
					cmmCreatedDate = eRPOrganizationMemoInformationDto.cmmCreatedDate,
					cmmUniqueID = eRPOrganizationMemoInformationDto.cmmUniqueID,
					cmmLocationID = eRPOrganizationMemoInformationDto.cmmLocationID,
					cmmLongDescriptionRtf = eRPOrganizationMemoInformationDto.cmmLongDescriptionRtf,
					cmmLongDescriptionText = eRPOrganizationMemoInformationDto.cmmLongDescriptionText,
					cmmMemoDate = eRPOrganizationMemoInformationDto.cmmMemoDate,
					cmmOrganizationID = eRPOrganizationMemoInformationDto.cmmOrganizationID,
					cmmRowVersion = eRPOrganizationMemoInformationDto.cmmRowVersion,
					cmmOrganizationMemoID = eRPOrganizationMemoInformationDto.cmmOrganizationMemoID,
					cmmShortDescription = eRPOrganizationMemoInformationDto.cmmShortDescription,
					cmmShowInApInvoices = eRPOrganizationMemoInformationDto.cmmShowInApInvoices,
					cmmShowInApPayments = eRPOrganizationMemoInformationDto.cmmShowInApPayments,
					cmmShowInArInvoices = eRPOrganizationMemoInformationDto.cmmShowInArInvoices,
					cmmShowInArPayments = eRPOrganizationMemoInformationDto.cmmShowInArPayments,
					cmmShowInCalls = eRPOrganizationMemoInformationDto.cmmShowInCalls,
					cmmShowInDmrClaims = eRPOrganizationMemoInformationDto.cmmShowInDmrClaims,
					cmmShowInDmrShipments = eRPOrganizationMemoInformationDto.cmmShowInDmrShipments,
					cmmShowInLeads = eRPOrganizationMemoInformationDto.cmmShowInLeads,
					cmmShowInOrganizations = eRPOrganizationMemoInformationDto.cmmShowInOrganizations,
					cmmShowInPriceAndAvailability = eRPOrganizationMemoInformationDto.cmmShowInPriceAndAvailability,
					cmmShowInPurchaseOrders = eRPOrganizationMemoInformationDto.cmmShowInPurchaseOrders,
					cmmShowInQuotes = eRPOrganizationMemoInformationDto.cmmShowInQuotes,
					cmmShowInReceipts = eRPOrganizationMemoInformationDto.cmmShowInReceipts,
					cmmShowInRfqs = eRPOrganizationMemoInformationDto.cmmShowInRfqs,
					cmmShowInRmaClaims = eRPOrganizationMemoInformationDto.cmmShowInRmaClaims,
					cmmShowInRmaReceipts = eRPOrganizationMemoInformationDto.cmmShowInRmaReceipts,
					cmmShowInSalesOrders = eRPOrganizationMemoInformationDto.cmmShowInSalesOrders,
					cmmShowInShipments = eRPOrganizationMemoInformationDto.cmmShowInShipments,
					CustomFields = eRPOrganizationMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the OrganizationMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = organizationMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_PutOrganizationMemo(ERPOrganizationMemoDto organizationMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPOrganizationMemoDto createdObject = null;
		ERPResponseMessageDto<ERPOrganizationMemoDto> result;
		try
		{
			IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
			using (iERPOrganizationMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPOrganizationMemoRepository.SaveOrganizationMemo(organizationMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPOrganizationMemoInformationDto eRPOrganizationMemoInformationDto = await base.ERPOrganizationMemoRepository.GetOrganizationMemo(organizationMemo.cmmUniqueID);
					createdObject = new ERPOrganizationMemoDto
					{
						cmmContactID = eRPOrganizationMemoInformationDto.cmmContactID,
						cmmCreatedBy = eRPOrganizationMemoInformationDto.cmmCreatedBy,
						cmmCreatedDate = eRPOrganizationMemoInformationDto.cmmCreatedDate,
						cmmUniqueID = eRPOrganizationMemoInformationDto.cmmUniqueID,
						cmmLocationID = eRPOrganizationMemoInformationDto.cmmLocationID,
						cmmLongDescriptionRtf = eRPOrganizationMemoInformationDto.cmmLongDescriptionRtf,
						cmmLongDescriptionText = eRPOrganizationMemoInformationDto.cmmLongDescriptionText,
						cmmMemoDate = eRPOrganizationMemoInformationDto.cmmMemoDate,
						cmmOrganizationID = eRPOrganizationMemoInformationDto.cmmOrganizationID,
						cmmRowVersion = eRPOrganizationMemoInformationDto.cmmRowVersion,
						cmmOrganizationMemoID = eRPOrganizationMemoInformationDto.cmmOrganizationMemoID,
						cmmShortDescription = eRPOrganizationMemoInformationDto.cmmShortDescription,
						cmmShowInApInvoices = eRPOrganizationMemoInformationDto.cmmShowInApInvoices,
						cmmShowInApPayments = eRPOrganizationMemoInformationDto.cmmShowInApPayments,
						cmmShowInArInvoices = eRPOrganizationMemoInformationDto.cmmShowInArInvoices,
						cmmShowInArPayments = eRPOrganizationMemoInformationDto.cmmShowInArPayments,
						cmmShowInCalls = eRPOrganizationMemoInformationDto.cmmShowInCalls,
						cmmShowInDmrClaims = eRPOrganizationMemoInformationDto.cmmShowInDmrClaims,
						cmmShowInDmrShipments = eRPOrganizationMemoInformationDto.cmmShowInDmrShipments,
						cmmShowInLeads = eRPOrganizationMemoInformationDto.cmmShowInLeads,
						cmmShowInOrganizations = eRPOrganizationMemoInformationDto.cmmShowInOrganizations,
						cmmShowInPriceAndAvailability = eRPOrganizationMemoInformationDto.cmmShowInPriceAndAvailability,
						cmmShowInPurchaseOrders = eRPOrganizationMemoInformationDto.cmmShowInPurchaseOrders,
						cmmShowInQuotes = eRPOrganizationMemoInformationDto.cmmShowInQuotes,
						cmmShowInReceipts = eRPOrganizationMemoInformationDto.cmmShowInReceipts,
						cmmShowInRfqs = eRPOrganizationMemoInformationDto.cmmShowInRfqs,
						cmmShowInRmaClaims = eRPOrganizationMemoInformationDto.cmmShowInRmaClaims,
						cmmShowInRmaReceipts = eRPOrganizationMemoInformationDto.cmmShowInRmaReceipts,
						cmmShowInSalesOrders = eRPOrganizationMemoInformationDto.cmmShowInSalesOrders,
						cmmShowInShipments = eRPOrganizationMemoInformationDto.cmmShowInShipments,
						CustomFields = eRPOrganizationMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing OrganizationMemo [{organizationMemo.cmmUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteOrganizationMemo(Guid organizationMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
		using (iERPOrganizationMemoRepository)
		{
			if (!(await base.ERPOrganizationMemoRepository.DoesOrganizationMemoExist(organizationMemoId)))
			{
				base.ErrorsList.Add($"OrganizationMemo [{organizationMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPOrganizationMemoInformationDto eRPOrganizationMemoInformationDto = await base.ERPOrganizationMemoRepository.GetOrganizationMemo(organizationMemoId);
				string text = await base.ERPOrganizationMemoRepository.WhereUsed("OrganizationMemos", new object[4] { eRPOrganizationMemoInformationDto.cmmOrganizationID, eRPOrganizationMemoInformationDto.cmmLocationID, eRPOrganizationMemoInformationDto.cmmContactID, eRPOrganizationMemoInformationDto.cmmOrganizationMemoID }, new object[4] { "cmmOrganizationID", "cmmLocationID", "cmmContactID", "cmmOrganizationMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("OrganizationMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPOrganizationMemoDto>> Process_DeleteOrganizationMemo(Guid organizationMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPOrganizationMemoDto> result;
		try
		{
			IERPOrganizationMemoRepository iERPOrganizationMemoRepository = (base.ERPOrganizationMemoRepository = new ERPOrganizationMemoRepository(base.ApiClientContext));
			using (iERPOrganizationMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPOrganizationMemoRepository.DeleteRowFromTable("OrganizationMemos", "cmm", organizationMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of OrganizationMemo [{organizationMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPOrganizationMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPOrganizationMemoDto()
			};
		}
		return result;
	}
}
