using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchasePlannerOrderDetailModel : ERPBaseModel, IERPPurchasePlannerOrderDetailModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchasePlannerOrderDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
		using (iERPPurchasePlannerOrderDetailRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchasePlannerOrderDetailRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchasePlannerOrderDetailRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchasePlannerOrderDetailRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchasePlannerOrderDetailRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
		using (iERPPurchasePlannerOrderDetailRepository)
		{
			if (!(await base.ERPPurchasePlannerOrderDetailRepository.DoesPurchasePlannerOrderDetailExist(purchasePlannerOrderDetailId)))
			{
				errorsList.Add($"PurchasePlannerOrderDetail [{purchasePlannerOrderDetailId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
		using (iERPPurchasePlannerOrderDetailRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoSessionID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("PurchasePlannerSessions", new object[1] { "ppsSessionID" }, new object[1] { purchasePlannerOrderDetail.ppoSessionID })))
			{
				errorsList.Add("ppoSessionID [" + purchasePlannerOrderDetail.ppoSessionID + "] not found.");
			}
			if (purchasePlannerOrderDetail.ppoLineID > 0 && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("PurchasePlannerLines", new object[2] { "pplSessionID", "pplLineID" }, new object[2] { purchasePlannerOrderDetail.ppoSessionID, purchasePlannerOrderDetail.ppoLineID })))
			{
				errorsList.Add($"ppoLineID [{purchasePlannerOrderDetail.ppoLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoJobID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { purchasePlannerOrderDetail.ppoJobID })))
			{
				errorsList.Add("ppoJobID [" + purchasePlannerOrderDetail.ppoJobID + "] not found.");
			}
			if (purchasePlannerOrderDetail.ppoJobAssemblyID > 0 && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { purchasePlannerOrderDetail.ppoJobID, purchasePlannerOrderDetail.ppoJobAssemblyID })))
			{
				errorsList.Add($"ppoJobAssemblyID [{purchasePlannerOrderDetail.ppoJobAssemblyID}] not found.");
			}
			if (purchasePlannerOrderDetail.ppoJobMaterialID > 0 && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { purchasePlannerOrderDetail.ppoJobID, purchasePlannerOrderDetail.ppoJobAssemblyID, purchasePlannerOrderDetail.ppoJobMaterialID })))
			{
				errorsList.Add($"ppoJobMaterialID [{purchasePlannerOrderDetail.ppoJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoSupplierOrganizationID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { purchasePlannerOrderDetail.ppoSupplierOrganizationID })))
			{
				errorsList.Add("ppoSupplierOrganizationID [" + purchasePlannerOrderDetail.ppoSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoPurchaseLocationID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { purchasePlannerOrderDetail.ppoSupplierOrganizationID, purchasePlannerOrderDetail.ppoPurchaseLocationID })))
			{
				errorsList.Add("ppoPurchaseLocationID [" + purchasePlannerOrderDetail.ppoPurchaseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoCurrencyRateID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("CurrencyRates", new object[1] { "MCPCURRENCYRATEID" }, new object[1] { purchasePlannerOrderDetail.ppoCurrencyRateID })))
			{
				errorsList.Add("ppoCurrencyRateID [" + purchasePlannerOrderDetail.ppoCurrencyRateID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoSalesOrderID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { purchasePlannerOrderDetail.ppoSalesOrderID })))
			{
				errorsList.Add("ppoSalesOrderID [" + purchasePlannerOrderDetail.ppoSalesOrderID + "] not found.");
			}
			if (purchasePlannerOrderDetail.ppoSalesOrderLineID > 0 && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { purchasePlannerOrderDetail.ppoSalesOrderID, purchasePlannerOrderDetail.ppoSalesOrderLineID })))
			{
				errorsList.Add($"ppoSalesOrderLineID [{purchasePlannerOrderDetail.ppoSalesOrderLineID}] not found.");
			}
			if (purchasePlannerOrderDetail.ppoSalesOrderDeliveryID > 0 && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { purchasePlannerOrderDetail.ppoSalesOrderID, purchasePlannerOrderDetail.ppoSalesOrderLineID, purchasePlannerOrderDetail.ppoSalesOrderDeliveryID })))
			{
				errorsList.Add($"ppoSalesOrderDeliveryID [{purchasePlannerOrderDetail.ppoSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoPartID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { purchasePlannerOrderDetail.ppoPartID })))
			{
				errorsList.Add("ppoPartID [" + purchasePlannerOrderDetail.ppoPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoPartRevisionID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { purchasePlannerOrderDetail.ppoPartID, purchasePlannerOrderDetail.ppoPartRevisionID })))
			{
				errorsList.Add("ppoPartRevisionID [" + purchasePlannerOrderDetail.ppoPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoProjectID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { purchasePlannerOrderDetail.ppoProjectID })))
			{
				errorsList.Add("ppoProjectID [" + purchasePlannerOrderDetail.ppoProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoProjectAreaID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { purchasePlannerOrderDetail.ppoProjectID, purchasePlannerOrderDetail.ppoProjectAreaID })))
			{
				errorsList.Add("ppoProjectAreaID [" + purchasePlannerOrderDetail.ppoProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoPartBinID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { purchasePlannerOrderDetail.ppoPartID, purchasePlannerOrderDetail.ppoPartRevisionID, purchasePlannerOrderDetail.ppoPartWarehouseLocationID, purchasePlannerOrderDetail.ppoPartBinID })))
			{
				errorsList.Add("ppoPartBinID [" + purchasePlannerOrderDetail.ppoPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchasePlannerOrderDetail.ppoPartWarehouseLocationID) && !(await base.ERPPurchasePlannerOrderDetailRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { purchasePlannerOrderDetail.ppoPartID, purchasePlannerOrderDetail.ppoPartRevisionID, purchasePlannerOrderDetail.ppoPartWarehouseLocationID })))
			{
				errorsList.Add("ppoPartWarehouseLocationID [" + purchasePlannerOrderDetail.ppoPartWarehouseLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchasePlannerOrderDetailDto>>> Process_GetAllPurchasePlannerOrderDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchasePlannerOrderDetailDto> allPurchasePlannerOrderDetailsDto = new List<ERPPurchasePlannerOrderDetailDto>();
		ERPResponseMessageDto<IList<ERPPurchasePlannerOrderDetailDto>> result;
		try
		{
			IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
			using (iERPPurchasePlannerOrderDetailRepository)
			{
				foreach (ERPPurchasePlannerOrderDetailInformationDto item2 in await base.ERPPurchasePlannerOrderDetailRepository.GetAllPurchasePlannerOrderDetails(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchasePlannerOrderDetailDto item = new ERPPurchasePlannerOrderDetailDto
					{
						ppoConversionFactor = item2.ppoConversionFactor,
						ppoCreatedBy = item2.ppoCreatedBy,
						ppoCreatedDate = item2.ppoCreatedDate,
						ppoCurrencyRateID = item2.ppoCurrencyRateID,
						ppoDataMissing = item2.ppoDataMissing,
						ppoDueDate = item2.ppoDueDate,
						ppoUniqueID = item2.ppoUniqueID,
						ppoExtendedCostBase = item2.ppoExtendedCostBase,
						ppoInventoryQuantity = item2.ppoInventoryQuantity,
						ppoInventoryUnitOfMeasure = item2.ppoInventoryUnitOfMeasure,
						ppoCompleted = item2.ppoCompleted,
						ppoSupplierRequirement = item2.ppoSupplierRequirement,
						ppoJobAssemblyID = item2.ppoJobAssemblyID,
						ppoJobID = item2.ppoJobID,
						ppoJobMaterialID = item2.ppoJobMaterialID,
						ppoLeadTime = item2.ppoLeadTime,
						ppoLineID = item2.ppoLineID,
						ppoOrderDetailID = item2.ppoOrderDetailID,
						ppoPartBinID = item2.ppoPartBinID,
						ppoPartID = item2.ppoPartID,
						ppoPartRevisionID = item2.ppoPartRevisionID,
						ppoPartWarehouseLocationID = item2.ppoPartWarehouseLocationID,
						ppoProjectAreaID = item2.ppoProjectAreaID,
						ppoProjectID = item2.ppoProjectID,
						ppoPurchaseLocationID = item2.ppoPurchaseLocationID,
						ppoPurchaseQuantity = item2.ppoPurchaseQuantity,
						ppoPurchaseType = item2.ppoPurchaseType,
						ppoPurchaseUnitOfMeasure = item2.ppoPurchaseUnitOfMeasure,
						ppoRowVersion = item2.ppoRowVersion,
						ppoSalesOrderDeliveryID = item2.ppoSalesOrderDeliveryID,
						ppoSalesOrderID = item2.ppoSalesOrderID,
						ppoSalesOrderLineID = item2.ppoSalesOrderLineID,
						ppoSessionID = item2.ppoSessionID,
						ppoSupplierOrganizationID = item2.ppoSupplierOrganizationID,
						ppoUnitCostBase = item2.ppoUnitCostBase,
						ppoUnitCostForeign = item2.ppoUnitCostForeign,
						CustomFields = item2.CustomFields
					};
					allPurchasePlannerOrderDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchasePlannerOrderDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchasePlannerOrderDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchasePlannerOrderDetailsDto,
				RecordCount = allPurchasePlannerOrderDetailsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_GetPurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetailDto = null;
		ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto> result;
		try
		{
			IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
			using (iERPPurchasePlannerOrderDetailRepository)
			{
				ERPPurchasePlannerOrderDetailInformationDto eRPPurchasePlannerOrderDetailInformationDto = await base.ERPPurchasePlannerOrderDetailRepository.GetPurchasePlannerOrderDetail(purchasePlannerOrderDetailId);
				purchasePlannerOrderDetailDto = new ERPPurchasePlannerOrderDetailDto
				{
					ppoConversionFactor = eRPPurchasePlannerOrderDetailInformationDto.ppoConversionFactor,
					ppoCreatedBy = eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedBy,
					ppoCreatedDate = eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedDate,
					ppoCurrencyRateID = eRPPurchasePlannerOrderDetailInformationDto.ppoCurrencyRateID,
					ppoDataMissing = eRPPurchasePlannerOrderDetailInformationDto.ppoDataMissing,
					ppoDueDate = eRPPurchasePlannerOrderDetailInformationDto.ppoDueDate,
					ppoUniqueID = eRPPurchasePlannerOrderDetailInformationDto.ppoUniqueID,
					ppoExtendedCostBase = eRPPurchasePlannerOrderDetailInformationDto.ppoExtendedCostBase,
					ppoInventoryQuantity = eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryQuantity,
					ppoInventoryUnitOfMeasure = eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryUnitOfMeasure,
					ppoCompleted = eRPPurchasePlannerOrderDetailInformationDto.ppoCompleted,
					ppoSupplierRequirement = eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierRequirement,
					ppoJobAssemblyID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobAssemblyID,
					ppoJobID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobID,
					ppoJobMaterialID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobMaterialID,
					ppoLeadTime = eRPPurchasePlannerOrderDetailInformationDto.ppoLeadTime,
					ppoLineID = eRPPurchasePlannerOrderDetailInformationDto.ppoLineID,
					ppoOrderDetailID = eRPPurchasePlannerOrderDetailInformationDto.ppoOrderDetailID,
					ppoPartBinID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartBinID,
					ppoPartID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartID,
					ppoPartRevisionID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartRevisionID,
					ppoPartWarehouseLocationID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartWarehouseLocationID,
					ppoProjectAreaID = eRPPurchasePlannerOrderDetailInformationDto.ppoProjectAreaID,
					ppoProjectID = eRPPurchasePlannerOrderDetailInformationDto.ppoProjectID,
					ppoPurchaseLocationID = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseLocationID,
					ppoPurchaseQuantity = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseQuantity,
					ppoPurchaseType = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseType,
					ppoPurchaseUnitOfMeasure = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseUnitOfMeasure,
					ppoRowVersion = eRPPurchasePlannerOrderDetailInformationDto.ppoRowVersion,
					ppoSalesOrderDeliveryID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderDeliveryID,
					ppoSalesOrderID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderID,
					ppoSalesOrderLineID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderLineID,
					ppoSessionID = eRPPurchasePlannerOrderDetailInformationDto.ppoSessionID,
					ppoSupplierOrganizationID = eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierOrganizationID,
					ppoUnitCostBase = eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostBase,
					ppoUnitCostForeign = eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostForeign,
					CustomFields = eRPPurchasePlannerOrderDetailInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchasePlannerOrderDetails []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchasePlannerOrderDetailDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_PutPurchasePlannerOrderDetail(ERPPurchasePlannerOrderDetailDto purchasePlannerOrderDetail)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchasePlannerOrderDetailDto createdObject = null;
		ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto> result;
		try
		{
			IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
			using (iERPPurchasePlannerOrderDetailRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchasePlannerOrderDetailRepository.SavePurchasePlannerOrderDetail(purchasePlannerOrderDetail);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchasePlannerOrderDetailInformationDto eRPPurchasePlannerOrderDetailInformationDto = await base.ERPPurchasePlannerOrderDetailRepository.GetPurchasePlannerOrderDetail(purchasePlannerOrderDetail.ppoUniqueID);
					createdObject = new ERPPurchasePlannerOrderDetailDto
					{
						ppoConversionFactor = eRPPurchasePlannerOrderDetailInformationDto.ppoConversionFactor,
						ppoCreatedBy = eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedBy,
						ppoCreatedDate = eRPPurchasePlannerOrderDetailInformationDto.ppoCreatedDate,
						ppoCurrencyRateID = eRPPurchasePlannerOrderDetailInformationDto.ppoCurrencyRateID,
						ppoDataMissing = eRPPurchasePlannerOrderDetailInformationDto.ppoDataMissing,
						ppoDueDate = eRPPurchasePlannerOrderDetailInformationDto.ppoDueDate,
						ppoUniqueID = eRPPurchasePlannerOrderDetailInformationDto.ppoUniqueID,
						ppoExtendedCostBase = eRPPurchasePlannerOrderDetailInformationDto.ppoExtendedCostBase,
						ppoInventoryQuantity = eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryQuantity,
						ppoInventoryUnitOfMeasure = eRPPurchasePlannerOrderDetailInformationDto.ppoInventoryUnitOfMeasure,
						ppoCompleted = eRPPurchasePlannerOrderDetailInformationDto.ppoCompleted,
						ppoSupplierRequirement = eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierRequirement,
						ppoJobAssemblyID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobAssemblyID,
						ppoJobID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobID,
						ppoJobMaterialID = eRPPurchasePlannerOrderDetailInformationDto.ppoJobMaterialID,
						ppoLeadTime = eRPPurchasePlannerOrderDetailInformationDto.ppoLeadTime,
						ppoLineID = eRPPurchasePlannerOrderDetailInformationDto.ppoLineID,
						ppoOrderDetailID = eRPPurchasePlannerOrderDetailInformationDto.ppoOrderDetailID,
						ppoPartBinID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartBinID,
						ppoPartID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartID,
						ppoPartRevisionID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartRevisionID,
						ppoPartWarehouseLocationID = eRPPurchasePlannerOrderDetailInformationDto.ppoPartWarehouseLocationID,
						ppoProjectAreaID = eRPPurchasePlannerOrderDetailInformationDto.ppoProjectAreaID,
						ppoProjectID = eRPPurchasePlannerOrderDetailInformationDto.ppoProjectID,
						ppoPurchaseLocationID = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseLocationID,
						ppoPurchaseQuantity = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseQuantity,
						ppoPurchaseType = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseType,
						ppoPurchaseUnitOfMeasure = eRPPurchasePlannerOrderDetailInformationDto.ppoPurchaseUnitOfMeasure,
						ppoRowVersion = eRPPurchasePlannerOrderDetailInformationDto.ppoRowVersion,
						ppoSalesOrderDeliveryID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderDeliveryID,
						ppoSalesOrderID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderID,
						ppoSalesOrderLineID = eRPPurchasePlannerOrderDetailInformationDto.ppoSalesOrderLineID,
						ppoSessionID = eRPPurchasePlannerOrderDetailInformationDto.ppoSessionID,
						ppoSupplierOrganizationID = eRPPurchasePlannerOrderDetailInformationDto.ppoSupplierOrganizationID,
						ppoUnitCostBase = eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostBase,
						ppoUnitCostForeign = eRPPurchasePlannerOrderDetailInformationDto.ppoUnitCostForeign,
						CustomFields = eRPPurchasePlannerOrderDetailInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchasePlannerOrderDetail [{purchasePlannerOrderDetail.ppoUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
		using (iERPPurchasePlannerOrderDetailRepository)
		{
			if (!(await base.ERPPurchasePlannerOrderDetailRepository.DoesPurchasePlannerOrderDetailExist(purchasePlannerOrderDetailId)))
			{
				base.ErrorsList.Add($"PurchasePlannerOrderDetail [{purchasePlannerOrderDetailId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchasePlannerOrderDetailInformationDto eRPPurchasePlannerOrderDetailInformationDto = await base.ERPPurchasePlannerOrderDetailRepository.GetPurchasePlannerOrderDetail(purchasePlannerOrderDetailId);
				string text = await base.ERPPurchasePlannerOrderDetailRepository.WhereUsed("PurchasePlannerOrderDetails", new object[3] { eRPPurchasePlannerOrderDetailInformationDto.ppoSessionID, eRPPurchasePlannerOrderDetailInformationDto.ppoLineID, eRPPurchasePlannerOrderDetailInformationDto.ppoOrderDetailID }, new object[3] { "ppoSessionID", "ppoLineID", "ppoOrderDetailID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchasePlannerOrderDetail cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>> Process_DeletePurchasePlannerOrderDetail(Guid purchasePlannerOrderDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto> result;
		try
		{
			IERPPurchasePlannerOrderDetailRepository iERPPurchasePlannerOrderDetailRepository = (base.ERPPurchasePlannerOrderDetailRepository = new ERPPurchasePlannerOrderDetailRepository(base.ApiClientContext));
			using (iERPPurchasePlannerOrderDetailRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchasePlannerOrderDetailRepository.DeleteRowFromTable("PurchasePlannerOrderDetails", "ppo", purchasePlannerOrderDetailId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchasePlannerOrderDetail [{purchasePlannerOrderDetailId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchasePlannerOrderDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchasePlannerOrderDetailDto()
			};
		}
		return result;
	}
}
