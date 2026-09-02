using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchasePlannerRequirementModel : ERPBaseModel, IERPPurchasePlannerRequirementModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerRequirements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
		using (iERPPurchasePlannerRequirementRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchasePlannerRequirementRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchasePlannerRequirementRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchasePlannerRequirementRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchasePlannerRequirementRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerRequirement(Guid purchasePlannerRequirementId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
		using (iERPPurchasePlannerRequirementRepository)
		{
			if (!(await base.ERPPurchasePlannerRequirementRepository.DoesPurchasePlannerRequirementExist(purchasePlannerRequirementId)))
			{
				errorsList.Add($"PurchasePlannerRequirement [{purchasePlannerRequirementId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerRequirement(ERPPurchasePlannerRequirementDto purchasePlannerRequirement)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
		using (iERPPurchasePlannerRequirementRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchasePlannerRequirement.pprSessionID) && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("PurchasePlannerSessions", new object[1] { "ppsSessionID" }, new object[1] { purchasePlannerRequirement.pprSessionID })))
			{
				errorsList.Add("pprSessionID [" + purchasePlannerRequirement.pprSessionID + "] not found.");
			}
			if (purchasePlannerRequirement.pprLineID > 0 && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("PurchasePlannerLines", new object[2] { "pplSessionID", "pplLineID" }, new object[2] { purchasePlannerRequirement.pprSessionID, purchasePlannerRequirement.pprLineID })))
			{
				errorsList.Add($"pprLineID [{purchasePlannerRequirement.pprLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerRequirement.pprJobID) && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { purchasePlannerRequirement.pprJobID })))
			{
				errorsList.Add("pprJobID [" + purchasePlannerRequirement.pprJobID + "] not found.");
			}
			if (purchasePlannerRequirement.pprJobAssemblyID > 0 && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { purchasePlannerRequirement.pprJobID, purchasePlannerRequirement.pprJobAssemblyID })))
			{
				errorsList.Add($"pprJobAssemblyID [{purchasePlannerRequirement.pprJobAssemblyID}] not found.");
			}
			if (purchasePlannerRequirement.pprJobMaterialID > 0 && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { purchasePlannerRequirement.pprJobID, purchasePlannerRequirement.pprJobAssemblyID, purchasePlannerRequirement.pprJobMaterialID })))
			{
				errorsList.Add($"pprJobMaterialID [{purchasePlannerRequirement.pprJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerRequirement.pprSalesOrderID) && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { purchasePlannerRequirement.pprSalesOrderID })))
			{
				errorsList.Add("pprSalesOrderID [" + purchasePlannerRequirement.pprSalesOrderID + "] not found.");
			}
			if (purchasePlannerRequirement.pprSalesOrderLineID > 0 && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { purchasePlannerRequirement.pprSalesOrderID, purchasePlannerRequirement.pprSalesOrderLineID })))
			{
				errorsList.Add($"pprSalesOrderLineID [{purchasePlannerRequirement.pprSalesOrderLineID}] not found.");
			}
			if (purchasePlannerRequirement.pprSalesOrderDeliveryID > 0 && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { purchasePlannerRequirement.pprSalesOrderID, purchasePlannerRequirement.pprSalesOrderLineID, purchasePlannerRequirement.pprSalesOrderDeliveryID })))
			{
				errorsList.Add($"pprSalesOrderDeliveryID [{purchasePlannerRequirement.pprSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerRequirement.pprPurchaseOrderID) && !(await base.ERPPurchasePlannerRequirementRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchasePlannerRequirement.pprPurchaseOrderID })))
			{
				errorsList.Add("pprPurchaseOrderID [" + purchasePlannerRequirement.pprPurchaseOrderID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchasePlannerRequirementDto>>> Process_GetAllPurchasePlannerRequirements(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchasePlannerRequirementDto> allPurchasePlannerRequirementsDto = new List<ERPPurchasePlannerRequirementDto>();
		ERPResponseMessageDto<IList<ERPPurchasePlannerRequirementDto>> result;
		try
		{
			IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
			using (iERPPurchasePlannerRequirementRepository)
			{
				foreach (ERPPurchasePlannerRequirementInformationDto item2 in await base.ERPPurchasePlannerRequirementRepository.GetAllPurchasePlannerRequirements(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchasePlannerRequirementDto item = new ERPPurchasePlannerRequirementDto
					{
						pprCreatedBy = item2.pprCreatedBy,
						pprCreatedDate = item2.pprCreatedDate,
						pprDueDate = item2.pprDueDate,
						pprUniqueID = item2.pprUniqueID,
						pprJobAssemblyID = item2.pprJobAssemblyID,
						pprJobID = item2.pprJobID,
						pprJobMaterialID = item2.pprJobMaterialID,
						pprLineID = item2.pprLineID,
						pprPlannedReceiptQuantity = item2.pprPlannedReceiptQuantity,
						pprPlannedRequirementQuantity = item2.pprPlannedRequirementQuantity,
						pprProjectedBalance = item2.pprProjectedBalance,
						pprPullFromStockQuantity = item2.pprPullFromStockQuantity,
						pprPurchaseOrderDate = item2.pprPurchaseOrderDate,
						pprPurchaseOrderID = item2.pprPurchaseOrderID,
						pprPurchaseToJobQuantity = item2.pprPurchaseToJobQuantity,
						pprPurchaseType = item2.pprPurchaseType,
						pprRequirementID = item2.pprRequirementID,
						pprRowVersion = item2.pprRowVersion,
						pprSalesOrderDeliveryID = item2.pprSalesOrderDeliveryID,
						pprSalesOrderID = item2.pprSalesOrderID,
						pprSalesOrderLineID = item2.pprSalesOrderLineID,
						pprSessionID = item2.pprSessionID,
						pprSource = item2.pprSource,
						CustomFields = item2.CustomFields
					};
					allPurchasePlannerRequirementsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchasePlannerRequirements]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchasePlannerRequirementDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchasePlannerRequirementsDto,
				RecordCount = allPurchasePlannerRequirementsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_GetPurchasePlannerRequirement(Guid purchasePlannerRequirementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchasePlannerRequirementDto purchasePlannerRequirementDto = null;
		ERPResponseMessageDto<ERPPurchasePlannerRequirementDto> result;
		try
		{
			IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
			using (iERPPurchasePlannerRequirementRepository)
			{
				ERPPurchasePlannerRequirementInformationDto eRPPurchasePlannerRequirementInformationDto = await base.ERPPurchasePlannerRequirementRepository.GetPurchasePlannerRequirement(purchasePlannerRequirementId);
				purchasePlannerRequirementDto = new ERPPurchasePlannerRequirementDto
				{
					pprCreatedBy = eRPPurchasePlannerRequirementInformationDto.pprCreatedBy,
					pprCreatedDate = eRPPurchasePlannerRequirementInformationDto.pprCreatedDate,
					pprDueDate = eRPPurchasePlannerRequirementInformationDto.pprDueDate,
					pprUniqueID = eRPPurchasePlannerRequirementInformationDto.pprUniqueID,
					pprJobAssemblyID = eRPPurchasePlannerRequirementInformationDto.pprJobAssemblyID,
					pprJobID = eRPPurchasePlannerRequirementInformationDto.pprJobID,
					pprJobMaterialID = eRPPurchasePlannerRequirementInformationDto.pprJobMaterialID,
					pprLineID = eRPPurchasePlannerRequirementInformationDto.pprLineID,
					pprPlannedReceiptQuantity = eRPPurchasePlannerRequirementInformationDto.pprPlannedReceiptQuantity,
					pprPlannedRequirementQuantity = eRPPurchasePlannerRequirementInformationDto.pprPlannedRequirementQuantity,
					pprProjectedBalance = eRPPurchasePlannerRequirementInformationDto.pprProjectedBalance,
					pprPullFromStockQuantity = eRPPurchasePlannerRequirementInformationDto.pprPullFromStockQuantity,
					pprPurchaseOrderDate = eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderDate,
					pprPurchaseOrderID = eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderID,
					pprPurchaseToJobQuantity = eRPPurchasePlannerRequirementInformationDto.pprPurchaseToJobQuantity,
					pprPurchaseType = eRPPurchasePlannerRequirementInformationDto.pprPurchaseType,
					pprRequirementID = eRPPurchasePlannerRequirementInformationDto.pprRequirementID,
					pprRowVersion = eRPPurchasePlannerRequirementInformationDto.pprRowVersion,
					pprSalesOrderDeliveryID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderDeliveryID,
					pprSalesOrderID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderID,
					pprSalesOrderLineID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderLineID,
					pprSessionID = eRPPurchasePlannerRequirementInformationDto.pprSessionID,
					pprSource = eRPPurchasePlannerRequirementInformationDto.pprSource,
					CustomFields = eRPPurchasePlannerRequirementInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchasePlannerRequirements []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchasePlannerRequirementDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_PutPurchasePlannerRequirement(ERPPurchasePlannerRequirementDto purchasePlannerRequirement)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchasePlannerRequirementDto createdObject = null;
		ERPResponseMessageDto<ERPPurchasePlannerRequirementDto> result;
		try
		{
			IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
			using (iERPPurchasePlannerRequirementRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchasePlannerRequirementRepository.SavePurchasePlannerRequirement(purchasePlannerRequirement);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchasePlannerRequirementInformationDto eRPPurchasePlannerRequirementInformationDto = await base.ERPPurchasePlannerRequirementRepository.GetPurchasePlannerRequirement(purchasePlannerRequirement.pprUniqueID);
					createdObject = new ERPPurchasePlannerRequirementDto
					{
						pprCreatedBy = eRPPurchasePlannerRequirementInformationDto.pprCreatedBy,
						pprCreatedDate = eRPPurchasePlannerRequirementInformationDto.pprCreatedDate,
						pprDueDate = eRPPurchasePlannerRequirementInformationDto.pprDueDate,
						pprUniqueID = eRPPurchasePlannerRequirementInformationDto.pprUniqueID,
						pprJobAssemblyID = eRPPurchasePlannerRequirementInformationDto.pprJobAssemblyID,
						pprJobID = eRPPurchasePlannerRequirementInformationDto.pprJobID,
						pprJobMaterialID = eRPPurchasePlannerRequirementInformationDto.pprJobMaterialID,
						pprLineID = eRPPurchasePlannerRequirementInformationDto.pprLineID,
						pprPlannedReceiptQuantity = eRPPurchasePlannerRequirementInformationDto.pprPlannedReceiptQuantity,
						pprPlannedRequirementQuantity = eRPPurchasePlannerRequirementInformationDto.pprPlannedRequirementQuantity,
						pprProjectedBalance = eRPPurchasePlannerRequirementInformationDto.pprProjectedBalance,
						pprPullFromStockQuantity = eRPPurchasePlannerRequirementInformationDto.pprPullFromStockQuantity,
						pprPurchaseOrderDate = eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderDate,
						pprPurchaseOrderID = eRPPurchasePlannerRequirementInformationDto.pprPurchaseOrderID,
						pprPurchaseToJobQuantity = eRPPurchasePlannerRequirementInformationDto.pprPurchaseToJobQuantity,
						pprPurchaseType = eRPPurchasePlannerRequirementInformationDto.pprPurchaseType,
						pprRequirementID = eRPPurchasePlannerRequirementInformationDto.pprRequirementID,
						pprRowVersion = eRPPurchasePlannerRequirementInformationDto.pprRowVersion,
						pprSalesOrderDeliveryID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderDeliveryID,
						pprSalesOrderID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderID,
						pprSalesOrderLineID = eRPPurchasePlannerRequirementInformationDto.pprSalesOrderLineID,
						pprSessionID = eRPPurchasePlannerRequirementInformationDto.pprSessionID,
						pprSource = eRPPurchasePlannerRequirementInformationDto.pprSource,
						CustomFields = eRPPurchasePlannerRequirementInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchasePlannerRequirement [{purchasePlannerRequirement.pprUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerRequirement(Guid purchasePlannerRequirementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
		using (iERPPurchasePlannerRequirementRepository)
		{
			if (!(await base.ERPPurchasePlannerRequirementRepository.DoesPurchasePlannerRequirementExist(purchasePlannerRequirementId)))
			{
				base.ErrorsList.Add($"PurchasePlannerRequirement [{purchasePlannerRequirementId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchasePlannerRequirementInformationDto eRPPurchasePlannerRequirementInformationDto = await base.ERPPurchasePlannerRequirementRepository.GetPurchasePlannerRequirement(purchasePlannerRequirementId);
				string text = await base.ERPPurchasePlannerRequirementRepository.WhereUsed("PurchasePlannerRequirements", new object[3] { eRPPurchasePlannerRequirementInformationDto.pprSessionID, eRPPurchasePlannerRequirementInformationDto.pprLineID, eRPPurchasePlannerRequirementInformationDto.pprRequirementID }, new object[3] { "pprSessionID", "pprLineID", "pprRequirementID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchasePlannerRequirement cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>> Process_DeletePurchasePlannerRequirement(Guid purchasePlannerRequirementId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchasePlannerRequirementDto> result;
		try
		{
			IERPPurchasePlannerRequirementRepository iERPPurchasePlannerRequirementRepository = (base.ERPPurchasePlannerRequirementRepository = new ERPPurchasePlannerRequirementRepository(base.ApiClientContext));
			using (iERPPurchasePlannerRequirementRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchasePlannerRequirementRepository.DeleteRowFromTable("PurchasePlannerRequirements", "ppr", purchasePlannerRequirementId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchasePlannerRequirement [{purchasePlannerRequirementId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerRequirementDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchasePlannerRequirementDto()
			};
		}
		return result;
	}
}
