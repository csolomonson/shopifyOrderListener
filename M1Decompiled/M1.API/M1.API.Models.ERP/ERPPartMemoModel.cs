using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartMemoModel : ERPBaseModel, IERPPartMemoModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
		using (iERPPartMemoRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartMemoRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartMemoRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartMemoRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartMemoRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartMemo(Guid partMemoId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
		using (iERPPartMemoRepository)
		{
			if (!(await base.ERPPartMemoRepository.DoesPartMemoExist(partMemoId)))
			{
				errorsList.Add($"PartMemo [{partMemoId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartMemo(ERPPartMemoDto partMemo)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
		using (iERPPartMemoRepository)
		{
			if (!string.IsNullOrWhiteSpace(partMemo.imkPartID) && !(await base.ERPPartMemoRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partMemo.imkPartID })))
			{
				errorsList.Add("imkPartID [" + partMemo.imkPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partMemo.imkPartRevisionID) && !(await base.ERPPartMemoRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partMemo.imkPartID, partMemo.imkPartRevisionID })))
			{
				errorsList.Add("imkPartRevisionID [" + partMemo.imkPartRevisionID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartMemoDto>>> Process_GetAllPartMemos(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartMemoDto> allPartMemosDto = new List<ERPPartMemoDto>();
		ERPResponseMessageDto<IList<ERPPartMemoDto>> result;
		try
		{
			IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
			using (iERPPartMemoRepository)
			{
				foreach (ERPPartMemoInformationDto item2 in await base.ERPPartMemoRepository.GetAllPartMemos(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartMemoDto item = new ERPPartMemoDto
					{
						imkCreatedBy = item2.imkCreatedBy,
						imkCreatedDate = item2.imkCreatedDate,
						imkUniqueID = item2.imkUniqueID,
						imkLongDescriptionRtf = item2.imkLongDescriptionRtf,
						imkLongDescriptionText = item2.imkLongDescriptionText,
						imkMemoDate = item2.imkMemoDate,
						imkPartID = item2.imkPartID,
						imkPartRevisionID = item2.imkPartRevisionID,
						imkRowVersion = item2.imkRowVersion,
						imkPartMemoID = item2.imkPartMemoID,
						imkShortDescription = item2.imkShortDescription,
						imkShowInApInvoices = item2.imkShowInApInvoices,
						imkShowInArInvoices = item2.imkShowInArInvoices,
						imkShowInCalls = item2.imkShowInCalls,
						imkShowInChangeRequests = item2.imkShowInChangeRequests,
						imkShowInDmrClaims = item2.imkShowInDmrClaims,
						imkShowInDmrShipments = item2.imkShowInDmrShipments,
						imkShowInInspections = item2.imkShowInInspections,
						imkShowInJobAssemblies = item2.imkShowInJobAssemblies,
						imkShowInJobMaterials = item2.imkShowInJobMaterials,
						imkShowInJobOperations = item2.imkShowInJobOperations,
						imkShowInJobs = item2.imkShowInJobs,
						imkShowInKnowledgebasePages = item2.imkShowInKnowledgebasePages,
						imkShowInLeads = item2.imkShowInLeads,
						imkShowInNonconformances = item2.imkShowInNonconformances,
						imkShowInPartAssemblies = item2.imkShowInPartAssemblies,
						imkShowInPartMaterials = item2.imkShowInPartMaterials,
						imkShowInPartOperations = item2.imkShowInPartOperations,
						imkShowInPartRevisions = item2.imkShowInPartRevisions,
						imkShowInPriceAndAvailability = item2.imkShowInPriceAndAvailability,
						imkShowInPurchaseOrders = item2.imkShowInPurchaseOrders,
						imkShowInQuoteAssemblies = item2.imkShowInQuoteAssemblies,
						imkShowInQuoteLines = item2.imkShowInQuoteLines,
						imkShowInQuoteMaterials = item2.imkShowInQuoteMaterials,
						imkShowInQuoteOperations = item2.imkShowInQuoteOperations,
						imkShowInReceipts = item2.imkShowInReceipts,
						imkShowInRfqs = item2.imkShowInRfqs,
						imkShowInRmaClaims = item2.imkShowInRmaClaims,
						imkShowInRmaReceipts = item2.imkShowInRmaReceipts,
						imkShowInSalesOrders = item2.imkShowInSalesOrders,
						imkShowInServiceContracts = item2.imkShowInServiceContracts,
						imkShowInShipments = item2.imkShowInShipments,
						imkShowInWarehouseReceipts = item2.imkShowInWarehouseReceipts,
						imkShowInWarehouseRequisitions = item2.imkShowInWarehouseRequisitions,
						imkShowInWarehouseTransfers = item2.imkShowInWarehouseTransfers,
						CustomFields = item2.CustomFields
					};
					allPartMemosDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartMemos]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartMemoDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartMemosDto,
				RecordCount = allPartMemosDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_GetPartMemo(Guid partMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartMemoDto partMemoDto = null;
		ERPResponseMessageDto<ERPPartMemoDto> result;
		try
		{
			IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
			using (iERPPartMemoRepository)
			{
				ERPPartMemoInformationDto eRPPartMemoInformationDto = await base.ERPPartMemoRepository.GetPartMemo(partMemoId);
				partMemoDto = new ERPPartMemoDto
				{
					imkCreatedBy = eRPPartMemoInformationDto.imkCreatedBy,
					imkCreatedDate = eRPPartMemoInformationDto.imkCreatedDate,
					imkUniqueID = eRPPartMemoInformationDto.imkUniqueID,
					imkLongDescriptionRtf = eRPPartMemoInformationDto.imkLongDescriptionRtf,
					imkLongDescriptionText = eRPPartMemoInformationDto.imkLongDescriptionText,
					imkMemoDate = eRPPartMemoInformationDto.imkMemoDate,
					imkPartID = eRPPartMemoInformationDto.imkPartID,
					imkPartRevisionID = eRPPartMemoInformationDto.imkPartRevisionID,
					imkRowVersion = eRPPartMemoInformationDto.imkRowVersion,
					imkPartMemoID = eRPPartMemoInformationDto.imkPartMemoID,
					imkShortDescription = eRPPartMemoInformationDto.imkShortDescription,
					imkShowInApInvoices = eRPPartMemoInformationDto.imkShowInApInvoices,
					imkShowInArInvoices = eRPPartMemoInformationDto.imkShowInArInvoices,
					imkShowInCalls = eRPPartMemoInformationDto.imkShowInCalls,
					imkShowInChangeRequests = eRPPartMemoInformationDto.imkShowInChangeRequests,
					imkShowInDmrClaims = eRPPartMemoInformationDto.imkShowInDmrClaims,
					imkShowInDmrShipments = eRPPartMemoInformationDto.imkShowInDmrShipments,
					imkShowInInspections = eRPPartMemoInformationDto.imkShowInInspections,
					imkShowInJobAssemblies = eRPPartMemoInformationDto.imkShowInJobAssemblies,
					imkShowInJobMaterials = eRPPartMemoInformationDto.imkShowInJobMaterials,
					imkShowInJobOperations = eRPPartMemoInformationDto.imkShowInJobOperations,
					imkShowInJobs = eRPPartMemoInformationDto.imkShowInJobs,
					imkShowInKnowledgebasePages = eRPPartMemoInformationDto.imkShowInKnowledgebasePages,
					imkShowInLeads = eRPPartMemoInformationDto.imkShowInLeads,
					imkShowInNonconformances = eRPPartMemoInformationDto.imkShowInNonconformances,
					imkShowInPartAssemblies = eRPPartMemoInformationDto.imkShowInPartAssemblies,
					imkShowInPartMaterials = eRPPartMemoInformationDto.imkShowInPartMaterials,
					imkShowInPartOperations = eRPPartMemoInformationDto.imkShowInPartOperations,
					imkShowInPartRevisions = eRPPartMemoInformationDto.imkShowInPartRevisions,
					imkShowInPriceAndAvailability = eRPPartMemoInformationDto.imkShowInPriceAndAvailability,
					imkShowInPurchaseOrders = eRPPartMemoInformationDto.imkShowInPurchaseOrders,
					imkShowInQuoteAssemblies = eRPPartMemoInformationDto.imkShowInQuoteAssemblies,
					imkShowInQuoteLines = eRPPartMemoInformationDto.imkShowInQuoteLines,
					imkShowInQuoteMaterials = eRPPartMemoInformationDto.imkShowInQuoteMaterials,
					imkShowInQuoteOperations = eRPPartMemoInformationDto.imkShowInQuoteOperations,
					imkShowInReceipts = eRPPartMemoInformationDto.imkShowInReceipts,
					imkShowInRfqs = eRPPartMemoInformationDto.imkShowInRfqs,
					imkShowInRmaClaims = eRPPartMemoInformationDto.imkShowInRmaClaims,
					imkShowInRmaReceipts = eRPPartMemoInformationDto.imkShowInRmaReceipts,
					imkShowInSalesOrders = eRPPartMemoInformationDto.imkShowInSalesOrders,
					imkShowInServiceContracts = eRPPartMemoInformationDto.imkShowInServiceContracts,
					imkShowInShipments = eRPPartMemoInformationDto.imkShowInShipments,
					imkShowInWarehouseReceipts = eRPPartMemoInformationDto.imkShowInWarehouseReceipts,
					imkShowInWarehouseRequisitions = eRPPartMemoInformationDto.imkShowInWarehouseRequisitions,
					imkShowInWarehouseTransfers = eRPPartMemoInformationDto.imkShowInWarehouseTransfers,
					CustomFields = eRPPartMemoInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartMemos []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partMemoDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_PutPartMemo(ERPPartMemoDto partMemo)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartMemoDto createdObject = null;
		ERPResponseMessageDto<ERPPartMemoDto> result;
		try
		{
			IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
			using (iERPPartMemoRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartMemoRepository.SavePartMemo(partMemo);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartMemoInformationDto eRPPartMemoInformationDto = await base.ERPPartMemoRepository.GetPartMemo(partMemo.imkUniqueID);
					createdObject = new ERPPartMemoDto
					{
						imkCreatedBy = eRPPartMemoInformationDto.imkCreatedBy,
						imkCreatedDate = eRPPartMemoInformationDto.imkCreatedDate,
						imkUniqueID = eRPPartMemoInformationDto.imkUniqueID,
						imkLongDescriptionRtf = eRPPartMemoInformationDto.imkLongDescriptionRtf,
						imkLongDescriptionText = eRPPartMemoInformationDto.imkLongDescriptionText,
						imkMemoDate = eRPPartMemoInformationDto.imkMemoDate,
						imkPartID = eRPPartMemoInformationDto.imkPartID,
						imkPartRevisionID = eRPPartMemoInformationDto.imkPartRevisionID,
						imkRowVersion = eRPPartMemoInformationDto.imkRowVersion,
						imkPartMemoID = eRPPartMemoInformationDto.imkPartMemoID,
						imkShortDescription = eRPPartMemoInformationDto.imkShortDescription,
						imkShowInApInvoices = eRPPartMemoInformationDto.imkShowInApInvoices,
						imkShowInArInvoices = eRPPartMemoInformationDto.imkShowInArInvoices,
						imkShowInCalls = eRPPartMemoInformationDto.imkShowInCalls,
						imkShowInChangeRequests = eRPPartMemoInformationDto.imkShowInChangeRequests,
						imkShowInDmrClaims = eRPPartMemoInformationDto.imkShowInDmrClaims,
						imkShowInDmrShipments = eRPPartMemoInformationDto.imkShowInDmrShipments,
						imkShowInInspections = eRPPartMemoInformationDto.imkShowInInspections,
						imkShowInJobAssemblies = eRPPartMemoInformationDto.imkShowInJobAssemblies,
						imkShowInJobMaterials = eRPPartMemoInformationDto.imkShowInJobMaterials,
						imkShowInJobOperations = eRPPartMemoInformationDto.imkShowInJobOperations,
						imkShowInJobs = eRPPartMemoInformationDto.imkShowInJobs,
						imkShowInKnowledgebasePages = eRPPartMemoInformationDto.imkShowInKnowledgebasePages,
						imkShowInLeads = eRPPartMemoInformationDto.imkShowInLeads,
						imkShowInNonconformances = eRPPartMemoInformationDto.imkShowInNonconformances,
						imkShowInPartAssemblies = eRPPartMemoInformationDto.imkShowInPartAssemblies,
						imkShowInPartMaterials = eRPPartMemoInformationDto.imkShowInPartMaterials,
						imkShowInPartOperations = eRPPartMemoInformationDto.imkShowInPartOperations,
						imkShowInPartRevisions = eRPPartMemoInformationDto.imkShowInPartRevisions,
						imkShowInPriceAndAvailability = eRPPartMemoInformationDto.imkShowInPriceAndAvailability,
						imkShowInPurchaseOrders = eRPPartMemoInformationDto.imkShowInPurchaseOrders,
						imkShowInQuoteAssemblies = eRPPartMemoInformationDto.imkShowInQuoteAssemblies,
						imkShowInQuoteLines = eRPPartMemoInformationDto.imkShowInQuoteLines,
						imkShowInQuoteMaterials = eRPPartMemoInformationDto.imkShowInQuoteMaterials,
						imkShowInQuoteOperations = eRPPartMemoInformationDto.imkShowInQuoteOperations,
						imkShowInReceipts = eRPPartMemoInformationDto.imkShowInReceipts,
						imkShowInRfqs = eRPPartMemoInformationDto.imkShowInRfqs,
						imkShowInRmaClaims = eRPPartMemoInformationDto.imkShowInRmaClaims,
						imkShowInRmaReceipts = eRPPartMemoInformationDto.imkShowInRmaReceipts,
						imkShowInSalesOrders = eRPPartMemoInformationDto.imkShowInSalesOrders,
						imkShowInServiceContracts = eRPPartMemoInformationDto.imkShowInServiceContracts,
						imkShowInShipments = eRPPartMemoInformationDto.imkShowInShipments,
						imkShowInWarehouseReceipts = eRPPartMemoInformationDto.imkShowInWarehouseReceipts,
						imkShowInWarehouseRequisitions = eRPPartMemoInformationDto.imkShowInWarehouseRequisitions,
						imkShowInWarehouseTransfers = eRPPartMemoInformationDto.imkShowInWarehouseTransfers,
						CustomFields = eRPPartMemoInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartMemo [{partMemo.imkUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartMemo(Guid partMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
		using (iERPPartMemoRepository)
		{
			if (!(await base.ERPPartMemoRepository.DoesPartMemoExist(partMemoId)))
			{
				base.ErrorsList.Add($"PartMemo [{partMemoId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartMemoInformationDto eRPPartMemoInformationDto = await base.ERPPartMemoRepository.GetPartMemo(partMemoId);
				string text = await base.ERPPartMemoRepository.WhereUsed("PartMemos", new object[3] { eRPPartMemoInformationDto.imkPartID, eRPPartMemoInformationDto.imkPartRevisionID, eRPPartMemoInformationDto.imkPartMemoID }, new object[3] { "imkPartID", "imkPartRevisionID", "imkPartMemoID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartMemo cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartMemoDto>> Process_DeletePartMemo(Guid partMemoId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartMemoDto> result;
		try
		{
			IERPPartMemoRepository iERPPartMemoRepository = (base.ERPPartMemoRepository = new ERPPartMemoRepository(base.ApiClientContext));
			using (iERPPartMemoRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartMemoRepository.DeleteRowFromTable("PartMemos", "imk", partMemoId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartMemo [{partMemoId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartMemoDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartMemoDto()
			};
		}
		return result;
	}
}
