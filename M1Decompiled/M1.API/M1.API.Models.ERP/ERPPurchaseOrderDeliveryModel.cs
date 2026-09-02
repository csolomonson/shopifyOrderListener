using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPurchaseOrderDeliveryModel : ERPBaseModel, IERPPurchaseOrderDeliveryModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPurchaseOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
		using (iERPPurchaseOrderDeliveryRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPurchaseOrderDeliveryRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPurchaseOrderDeliveryRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPurchaseOrderDeliveryRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPurchaseOrderDeliveryRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
		using (iERPPurchaseOrderDeliveryRepository)
		{
			if (!(await base.ERPPurchaseOrderDeliveryRepository.DoesPurchaseOrderDeliveryExist(purchaseOrderDeliveryId)))
			{
				errorsList.Add($"PurchaseOrderDelivery [{purchaseOrderDeliveryId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
		using (iERPPurchaseOrderDeliveryRepository)
		{
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdPurchaseOrderID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("PurchaseOrders", new object[1] { "PMPPURCHASEORDERID" }, new object[1] { purchaseOrderDelivery.pmdPurchaseOrderID })))
			{
				errorsList.Add("pmdPurchaseOrderID [" + purchaseOrderDelivery.pmdPurchaseOrderID + "] not found.");
			}
			if (purchaseOrderDelivery.pmdPurchaseOrderLineID > 0 && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("PurchaseOrderLines", new object[2] { "PMLPURCHASEORDERID", "PMLPURCHASEORDERLINEID" }, new object[2] { purchaseOrderDelivery.pmdPurchaseOrderID, purchaseOrderDelivery.pmdPurchaseOrderLineID })))
			{
				errorsList.Add($"pmdPurchaseOrderLineID [{purchaseOrderDelivery.pmdPurchaseOrderLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdJobID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { purchaseOrderDelivery.pmdJobID })))
			{
				errorsList.Add("pmdJobID [" + purchaseOrderDelivery.pmdJobID + "] not found.");
			}
			if (purchaseOrderDelivery.pmdJobAssemblyID > 0 && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { purchaseOrderDelivery.pmdJobID, purchaseOrderDelivery.pmdJobAssemblyID })))
			{
				errorsList.Add($"pmdJobAssemblyID [{purchaseOrderDelivery.pmdJobAssemblyID}] not found.");
			}
			if (purchaseOrderDelivery.pmdJobMaterialID > 0 && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { purchaseOrderDelivery.pmdJobID, purchaseOrderDelivery.pmdJobAssemblyID, purchaseOrderDelivery.pmdJobMaterialID })))
			{
				errorsList.Add($"pmdJobMaterialID [{purchaseOrderDelivery.pmdJobMaterialID}] not found.");
			}
			if (purchaseOrderDelivery.pmdJobOperationID > 0 && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { purchaseOrderDelivery.pmdJobID, purchaseOrderDelivery.pmdJobAssemblyID, purchaseOrderDelivery.pmdJobOperationID })))
			{
				errorsList.Add($"pmdJobOperationID [{purchaseOrderDelivery.pmdJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdOrganizationID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { purchaseOrderDelivery.pmdOrganizationID })))
			{
				errorsList.Add("pmdOrganizationID [" + purchaseOrderDelivery.pmdOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdLocationID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { purchaseOrderDelivery.pmdOrganizationID, purchaseOrderDelivery.pmdLocationID })))
			{
				errorsList.Add("pmdLocationID [" + purchaseOrderDelivery.pmdLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdContactID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("OrganizationContacts", new object[3] { "CMCORGANIZATIONID", "CMCLOCATIONID", "CMCCONTACTID" }, new object[3] { purchaseOrderDelivery.pmdOrganizationID, purchaseOrderDelivery.pmdLocationID, purchaseOrderDelivery.pmdContactID })))
			{
				errorsList.Add("pmdContactID [" + purchaseOrderDelivery.pmdContactID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(purchaseOrderDelivery.pmdShippingMethodID) && !(await base.ERPPurchaseOrderDeliveryRepository.DoesRecordExistInTableUsingKeys("ShippingMethods", new object[1] { "XASSHIPPINGMETHODID" }, new object[1] { purchaseOrderDelivery.pmdShippingMethodID })))
			{
				errorsList.Add("pmdShippingMethodID [" + purchaseOrderDelivery.pmdShippingMethodID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPurchaseOrderDeliveryDto>>> Process_GetAllPurchaseOrderDeliveries(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPurchaseOrderDeliveryDto> allPurchaseOrderDeliveriesDto = new List<ERPPurchaseOrderDeliveryDto>();
		ERPResponseMessageDto<IList<ERPPurchaseOrderDeliveryDto>> result;
		try
		{
			IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
			using (iERPPurchaseOrderDeliveryRepository)
			{
				foreach (ERPPurchaseOrderDeliveryInformationDto item2 in await base.ERPPurchaseOrderDeliveryRepository.GetAllPurchaseOrderDeliveries(pageSize, pageNumber, filter, orderBy))
				{
					ERPPurchaseOrderDeliveryDto item = new ERPPurchaseOrderDeliveryDto
					{
						pmdContactID = item2.pmdContactID,
						pmdCreatedBy = item2.pmdCreatedBy,
						pmdCreatedDate = item2.pmdCreatedDate,
						pmdDeliveryDate = item2.pmdDeliveryDate,
						pmdDeliveryQuantity = item2.pmdDeliveryQuantity,
						pmdDeliveryType = item2.pmdDeliveryType,
						pmdUniqueID = item2.pmdUniqueID,
						pmdClosed = item2.pmdClosed,
						pmdInTransit = item2.pmdInTransit,
						pmdInvoicedComplete = item2.pmdInvoicedComplete,
						pmdReceivedComplete = item2.pmdReceivedComplete,
						pmdJobAssemblyID = item2.pmdJobAssemblyID,
						pmdJobID = item2.pmdJobID,
						pmdJobMaterialID = item2.pmdJobMaterialID,
						pmdJobOperationID = item2.pmdJobOperationID,
						pmdJobType = item2.pmdJobType,
						pmdLocationID = item2.pmdLocationID,
						pmdOrganizationID = item2.pmdOrganizationID,
						pmdPurchaseOrderID = item2.pmdPurchaseOrderID,
						pmdPurchaseOrderLineID = item2.pmdPurchaseOrderLineID,
						pmdQuantityInvoiced = item2.pmdQuantityInvoiced,
						pmdQuantityReceived = item2.pmdQuantityReceived,
						pmdRowVersion = item2.pmdRowVersion,
						pmdPurchaseOrderDeliveryID = item2.pmdPurchaseOrderDeliveryID,
						pmdShippingMethodID = item2.pmdShippingMethodID,
						pmdTrackingNumber = item2.pmdTrackingNumber,
						CustomFields = item2.CustomFields
					};
					allPurchaseOrderDeliveriesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PurchaseOrderDeliveries]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPurchaseOrderDeliveryDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPurchaseOrderDeliveriesDto,
				RecordCount = allPurchaseOrderDeliveriesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_GetPurchaseOrderDelivery(Guid purchaseOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPurchaseOrderDeliveryDto purchaseOrderDeliveryDto = null;
		ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto> result;
		try
		{
			IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
			using (iERPPurchaseOrderDeliveryRepository)
			{
				ERPPurchaseOrderDeliveryInformationDto eRPPurchaseOrderDeliveryInformationDto = await base.ERPPurchaseOrderDeliveryRepository.GetPurchaseOrderDelivery(purchaseOrderDeliveryId);
				purchaseOrderDeliveryDto = new ERPPurchaseOrderDeliveryDto
				{
					pmdContactID = eRPPurchaseOrderDeliveryInformationDto.pmdContactID,
					pmdCreatedBy = eRPPurchaseOrderDeliveryInformationDto.pmdCreatedBy,
					pmdCreatedDate = eRPPurchaseOrderDeliveryInformationDto.pmdCreatedDate,
					pmdDeliveryDate = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryDate,
					pmdDeliveryQuantity = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryQuantity,
					pmdDeliveryType = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryType,
					pmdUniqueID = eRPPurchaseOrderDeliveryInformationDto.pmdUniqueID,
					pmdClosed = eRPPurchaseOrderDeliveryInformationDto.pmdClosed,
					pmdInTransit = eRPPurchaseOrderDeliveryInformationDto.pmdInTransit,
					pmdInvoicedComplete = eRPPurchaseOrderDeliveryInformationDto.pmdInvoicedComplete,
					pmdReceivedComplete = eRPPurchaseOrderDeliveryInformationDto.pmdReceivedComplete,
					pmdJobAssemblyID = eRPPurchaseOrderDeliveryInformationDto.pmdJobAssemblyID,
					pmdJobID = eRPPurchaseOrderDeliveryInformationDto.pmdJobID,
					pmdJobMaterialID = eRPPurchaseOrderDeliveryInformationDto.pmdJobMaterialID,
					pmdJobOperationID = eRPPurchaseOrderDeliveryInformationDto.pmdJobOperationID,
					pmdJobType = eRPPurchaseOrderDeliveryInformationDto.pmdJobType,
					pmdLocationID = eRPPurchaseOrderDeliveryInformationDto.pmdLocationID,
					pmdOrganizationID = eRPPurchaseOrderDeliveryInformationDto.pmdOrganizationID,
					pmdPurchaseOrderID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderID,
					pmdPurchaseOrderLineID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderLineID,
					pmdQuantityInvoiced = eRPPurchaseOrderDeliveryInformationDto.pmdQuantityInvoiced,
					pmdQuantityReceived = eRPPurchaseOrderDeliveryInformationDto.pmdQuantityReceived,
					pmdRowVersion = eRPPurchaseOrderDeliveryInformationDto.pmdRowVersion,
					pmdPurchaseOrderDeliveryID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderDeliveryID,
					pmdShippingMethodID = eRPPurchaseOrderDeliveryInformationDto.pmdShippingMethodID,
					pmdTrackingNumber = eRPPurchaseOrderDeliveryInformationDto.pmdTrackingNumber,
					CustomFields = eRPPurchaseOrderDeliveryInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PurchaseOrderDeliveries []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = purchaseOrderDeliveryDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_PutPurchaseOrderDelivery(ERPPurchaseOrderDeliveryDto purchaseOrderDelivery)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPurchaseOrderDeliveryDto createdObject = null;
		ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto> result;
		try
		{
			IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
			using (iERPPurchaseOrderDeliveryRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPurchaseOrderDeliveryRepository.SavePurchaseOrderDelivery(purchaseOrderDelivery);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPurchaseOrderDeliveryInformationDto eRPPurchaseOrderDeliveryInformationDto = await base.ERPPurchaseOrderDeliveryRepository.GetPurchaseOrderDelivery(purchaseOrderDelivery.pmdUniqueID);
					createdObject = new ERPPurchaseOrderDeliveryDto
					{
						pmdContactID = eRPPurchaseOrderDeliveryInformationDto.pmdContactID,
						pmdCreatedBy = eRPPurchaseOrderDeliveryInformationDto.pmdCreatedBy,
						pmdCreatedDate = eRPPurchaseOrderDeliveryInformationDto.pmdCreatedDate,
						pmdDeliveryDate = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryDate,
						pmdDeliveryQuantity = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryQuantity,
						pmdDeliveryType = eRPPurchaseOrderDeliveryInformationDto.pmdDeliveryType,
						pmdUniqueID = eRPPurchaseOrderDeliveryInformationDto.pmdUniqueID,
						pmdClosed = eRPPurchaseOrderDeliveryInformationDto.pmdClosed,
						pmdInTransit = eRPPurchaseOrderDeliveryInformationDto.pmdInTransit,
						pmdInvoicedComplete = eRPPurchaseOrderDeliveryInformationDto.pmdInvoicedComplete,
						pmdReceivedComplete = eRPPurchaseOrderDeliveryInformationDto.pmdReceivedComplete,
						pmdJobAssemblyID = eRPPurchaseOrderDeliveryInformationDto.pmdJobAssemblyID,
						pmdJobID = eRPPurchaseOrderDeliveryInformationDto.pmdJobID,
						pmdJobMaterialID = eRPPurchaseOrderDeliveryInformationDto.pmdJobMaterialID,
						pmdJobOperationID = eRPPurchaseOrderDeliveryInformationDto.pmdJobOperationID,
						pmdJobType = eRPPurchaseOrderDeliveryInformationDto.pmdJobType,
						pmdLocationID = eRPPurchaseOrderDeliveryInformationDto.pmdLocationID,
						pmdOrganizationID = eRPPurchaseOrderDeliveryInformationDto.pmdOrganizationID,
						pmdPurchaseOrderID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderID,
						pmdPurchaseOrderLineID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderLineID,
						pmdQuantityInvoiced = eRPPurchaseOrderDeliveryInformationDto.pmdQuantityInvoiced,
						pmdQuantityReceived = eRPPurchaseOrderDeliveryInformationDto.pmdQuantityReceived,
						pmdRowVersion = eRPPurchaseOrderDeliveryInformationDto.pmdRowVersion,
						pmdPurchaseOrderDeliveryID = eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderDeliveryID,
						pmdShippingMethodID = eRPPurchaseOrderDeliveryInformationDto.pmdShippingMethodID,
						pmdTrackingNumber = eRPPurchaseOrderDeliveryInformationDto.pmdTrackingNumber,
						CustomFields = eRPPurchaseOrderDeliveryInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PurchaseOrderDelivery [{purchaseOrderDelivery.pmdUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePurchaseOrderDelivery(Guid purchaseOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
		using (iERPPurchaseOrderDeliveryRepository)
		{
			if (!(await base.ERPPurchaseOrderDeliveryRepository.DoesPurchaseOrderDeliveryExist(purchaseOrderDeliveryId)))
			{
				base.ErrorsList.Add($"PurchaseOrderDelivery [{purchaseOrderDeliveryId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPurchaseOrderDeliveryInformationDto eRPPurchaseOrderDeliveryInformationDto = await base.ERPPurchaseOrderDeliveryRepository.GetPurchaseOrderDelivery(purchaseOrderDeliveryId);
				string text = await base.ERPPurchaseOrderDeliveryRepository.WhereUsed("PurchaseOrderDeliveries", new object[3] { eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderID, eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderLineID, eRPPurchaseOrderDeliveryInformationDto.pmdPurchaseOrderDeliveryID }, new object[3] { "pmdPurchaseOrderID", "pmdPurchaseOrderLineID", "pmdPurchaseOrderDeliveryID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PurchaseOrderDelivery cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>> Process_DeletePurchaseOrderDelivery(Guid purchaseOrderDeliveryId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto> result;
		try
		{
			IERPPurchaseOrderDeliveryRepository iERPPurchaseOrderDeliveryRepository = (base.ERPPurchaseOrderDeliveryRepository = new ERPPurchaseOrderDeliveryRepository(base.ApiClientContext));
			using (iERPPurchaseOrderDeliveryRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPurchaseOrderDeliveryRepository.DeleteRowFromTable("PurchaseOrderDeliveries", "pmd", purchaseOrderDeliveryId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PurchaseOrderDelivery [{purchaseOrderDeliveryId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPurchaseOrderDeliveryDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPurchaseOrderDeliveryDto()
			};
		}
		return result;
	}
}
