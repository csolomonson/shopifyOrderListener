using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPPartTransactionModel : ERPBaseModel, IERPPartTransactionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllPartTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
		using (iERPPartTransactionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPPartTransactionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPPartTransactionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPPartTransactionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPPartTransactionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetPartTransaction(Guid partTransactionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
		using (iERPPartTransactionRepository)
		{
			if (!(await base.ERPPartTransactionRepository.DoesPartTransactionExist(partTransactionId)))
			{
				errorsList.Add($"PartTransaction [{partTransactionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutPartTransaction(ERPPartTransactionDto partTransaction)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
		using (iERPPartTransactionRepository)
		{
			if (!string.IsNullOrWhiteSpace(partTransaction.imtJobID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { partTransaction.imtJobID })))
			{
				errorsList.Add("imtJobID [" + partTransaction.imtJobID + "] not found.");
			}
			if (partTransaction.imtJobAssemblyID > 0 && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { partTransaction.imtJobID, partTransaction.imtJobAssemblyID })))
			{
				errorsList.Add($"imtJobAssemblyID [{partTransaction.imtJobAssemblyID}] not found.");
			}
			if (partTransaction.imtJobMaterialID > 0 && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { partTransaction.imtJobID, partTransaction.imtJobAssemblyID, partTransaction.imtJobMaterialID })))
			{
				errorsList.Add($"imtJobMaterialID [{partTransaction.imtJobMaterialID}] not found.");
			}
			if (partTransaction.imtJobMaterialComponentID > 0 && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { partTransaction.imtJobID, partTransaction.imtJobAssemblyID, partTransaction.imtJobMaterialID, partTransaction.imtJobMaterialComponentID })))
			{
				errorsList.Add($"imtJobMaterialComponentID [{partTransaction.imtJobMaterialComponentID}] not found.");
			}
			if (partTransaction.imtJobOperationID > 0 && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { partTransaction.imtJobID, partTransaction.imtJobAssemblyID, partTransaction.imtJobOperationID })))
			{
				errorsList.Add($"imtJobOperationID [{partTransaction.imtJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtPartID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { partTransaction.imtPartID })))
			{
				errorsList.Add("imtPartID [" + partTransaction.imtPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtPartRevisionID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { partTransaction.imtPartID, partTransaction.imtPartRevisionID })))
			{
				errorsList.Add("imtPartRevisionID [" + partTransaction.imtPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtPartWarehouseLocationID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { partTransaction.imtPartID, partTransaction.imtPartRevisionID, partTransaction.imtPartWarehouseLocationID })))
			{
				errorsList.Add("imtPartWarehouseLocationID [" + partTransaction.imtPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtPartBinID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { partTransaction.imtPartID, partTransaction.imtPartRevisionID, partTransaction.imtPartWarehouseLocationID, partTransaction.imtPartBinID })))
			{
				errorsList.Add("imtPartBinID [" + partTransaction.imtPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtProjectID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { partTransaction.imtProjectID })))
			{
				errorsList.Add("imtProjectID [" + partTransaction.imtProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtProjectAreaID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { partTransaction.imtProjectID, partTransaction.imtProjectAreaID })))
			{
				errorsList.Add("imtProjectAreaID [" + partTransaction.imtProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(partTransaction.imtPlantID) && !(await base.ERPPartTransactionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { partTransaction.imtPlantID })))
			{
				errorsList.Add("imtPlantID [" + partTransaction.imtPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPPartTransactionDto>>> Process_GetAllPartTransactions(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPPartTransactionDto> allPartTransactionsDto = new List<ERPPartTransactionDto>();
		ERPResponseMessageDto<IList<ERPPartTransactionDto>> result;
		try
		{
			IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
			using (iERPPartTransactionRepository)
			{
				foreach (ERPPartTransactionInformationDto item2 in await base.ERPPartTransactionRepository.GetAllPartTransactions(pageSize, pageNumber, filter, orderBy))
				{
					ERPPartTransactionDto item = new ERPPartTransactionDto
					{
						imtCogsCalculatedDate = item2.imtCogsCalculatedDate,
						imtCreatedBy = item2.imtCreatedBy,
						imtCreatedDate = item2.imtCreatedDate,
						imtUniqueID = item2.imtUniqueID,
						imtHeatLot = item2.imtHeatLot,
						imtInspectionStatus = item2.imtInspectionStatus,
						imtInventoryQuantityReceived = item2.imtInventoryQuantityReceived,
						imtInventoryUnitOfMeasure = item2.imtInventoryUnitOfMeasure,
						imtCogsPostedToGl = item2.imtCogsPostedToGl,
						imtJobCompleteStatus = item2.imtJobCompleteStatus,
						imtNonInventoryTransaction = item2.imtNonInventoryTransaction,
						imtNonNettable = item2.imtNonNettable,
						imtPoLineReceivedComplete = item2.imtPoLineReceivedComplete,
						imtRequiresInspection = item2.imtRequiresInspection,
						imtIssueType = item2.imtIssueType,
						imtJobAssemblyID = item2.imtJobAssemblyID,
						imtJobID = item2.imtJobID,
						imtJobMaterialComponentID = item2.imtJobMaterialComponentID,
						imtJobMaterialID = item2.imtJobMaterialID,
						imtJobOperationID = item2.imtJobOperationID,
						imtJobType = item2.imtJobType,
						imtPartBinID = item2.imtPartBinID,
						imtPartID = item2.imtPartID,
						imtPartRevisionID = item2.imtPartRevisionID,
						imtPartWarehouseLocationID = item2.imtPartWarehouseLocationID,
						imtPlantID = item2.imtPlantID,
						imtPreviousQuantityOnHand = item2.imtPreviousQuantityOnHand,
						imtProjectAreaID = item2.imtProjectAreaID,
						imtProjectID = item2.imtProjectID,
						imtQuantityToInspect = item2.imtQuantityToInspect,
						imtQuantityToReturn = item2.imtQuantityToReturn,
						imtReceiptType = item2.imtReceiptType,
						imtReference = item2.imtReference,
						imtRowVersion = item2.imtRowVersion,
						imtScrapQuantity = item2.imtScrapQuantity,
						imtPartTransactionID = item2.imtPartTransactionID,
						imtSetupCharge = item2.imtSetupCharge,
						imtSource = item2.imtSource,
						imtTableName = item2.imtTableName,
						imtTableUniqueID = item2.imtTableUniqueID,
						imtTransactionDate = item2.imtTransactionDate,
						imtTransactionType = item2.imtTransactionType,
						imtUserID = item2.imtUserID,
						CustomFields = item2.CustomFields
					};
					allPartTransactionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all PartTransactions]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPPartTransactionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allPartTransactionsDto,
				RecordCount = allPartTransactionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_GetPartTransaction(Guid partTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPPartTransactionDto partTransactionDto = null;
		ERPResponseMessageDto<ERPPartTransactionDto> result;
		try
		{
			IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
			using (iERPPartTransactionRepository)
			{
				ERPPartTransactionInformationDto eRPPartTransactionInformationDto = await base.ERPPartTransactionRepository.GetPartTransaction(partTransactionId);
				partTransactionDto = new ERPPartTransactionDto
				{
					imtCogsCalculatedDate = eRPPartTransactionInformationDto.imtCogsCalculatedDate,
					imtCreatedBy = eRPPartTransactionInformationDto.imtCreatedBy,
					imtCreatedDate = eRPPartTransactionInformationDto.imtCreatedDate,
					imtUniqueID = eRPPartTransactionInformationDto.imtUniqueID,
					imtHeatLot = eRPPartTransactionInformationDto.imtHeatLot,
					imtInspectionStatus = eRPPartTransactionInformationDto.imtInspectionStatus,
					imtInventoryQuantityReceived = eRPPartTransactionInformationDto.imtInventoryQuantityReceived,
					imtInventoryUnitOfMeasure = eRPPartTransactionInformationDto.imtInventoryUnitOfMeasure,
					imtCogsPostedToGl = eRPPartTransactionInformationDto.imtCogsPostedToGl,
					imtJobCompleteStatus = eRPPartTransactionInformationDto.imtJobCompleteStatus,
					imtNonInventoryTransaction = eRPPartTransactionInformationDto.imtNonInventoryTransaction,
					imtNonNettable = eRPPartTransactionInformationDto.imtNonNettable,
					imtPoLineReceivedComplete = eRPPartTransactionInformationDto.imtPoLineReceivedComplete,
					imtRequiresInspection = eRPPartTransactionInformationDto.imtRequiresInspection,
					imtIssueType = eRPPartTransactionInformationDto.imtIssueType,
					imtJobAssemblyID = eRPPartTransactionInformationDto.imtJobAssemblyID,
					imtJobID = eRPPartTransactionInformationDto.imtJobID,
					imtJobMaterialComponentID = eRPPartTransactionInformationDto.imtJobMaterialComponentID,
					imtJobMaterialID = eRPPartTransactionInformationDto.imtJobMaterialID,
					imtJobOperationID = eRPPartTransactionInformationDto.imtJobOperationID,
					imtJobType = eRPPartTransactionInformationDto.imtJobType,
					imtPartBinID = eRPPartTransactionInformationDto.imtPartBinID,
					imtPartID = eRPPartTransactionInformationDto.imtPartID,
					imtPartRevisionID = eRPPartTransactionInformationDto.imtPartRevisionID,
					imtPartWarehouseLocationID = eRPPartTransactionInformationDto.imtPartWarehouseLocationID,
					imtPlantID = eRPPartTransactionInformationDto.imtPlantID,
					imtPreviousQuantityOnHand = eRPPartTransactionInformationDto.imtPreviousQuantityOnHand,
					imtProjectAreaID = eRPPartTransactionInformationDto.imtProjectAreaID,
					imtProjectID = eRPPartTransactionInformationDto.imtProjectID,
					imtQuantityToInspect = eRPPartTransactionInformationDto.imtQuantityToInspect,
					imtQuantityToReturn = eRPPartTransactionInformationDto.imtQuantityToReturn,
					imtReceiptType = eRPPartTransactionInformationDto.imtReceiptType,
					imtReference = eRPPartTransactionInformationDto.imtReference,
					imtRowVersion = eRPPartTransactionInformationDto.imtRowVersion,
					imtScrapQuantity = eRPPartTransactionInformationDto.imtScrapQuantity,
					imtPartTransactionID = eRPPartTransactionInformationDto.imtPartTransactionID,
					imtSetupCharge = eRPPartTransactionInformationDto.imtSetupCharge,
					imtSource = eRPPartTransactionInformationDto.imtSource,
					imtTableName = eRPPartTransactionInformationDto.imtTableName,
					imtTableUniqueID = eRPPartTransactionInformationDto.imtTableUniqueID,
					imtTransactionDate = eRPPartTransactionInformationDto.imtTransactionDate,
					imtTransactionType = eRPPartTransactionInformationDto.imtTransactionType,
					imtUserID = eRPPartTransactionInformationDto.imtUserID,
					CustomFields = eRPPartTransactionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the PartTransactions []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = partTransactionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_PutPartTransaction(ERPPartTransactionDto partTransaction)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPPartTransactionDto createdObject = null;
		ERPResponseMessageDto<ERPPartTransactionDto> result;
		try
		{
			IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
			using (iERPPartTransactionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPPartTransactionRepository.SavePartTransaction(partTransaction);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPPartTransactionInformationDto eRPPartTransactionInformationDto = await base.ERPPartTransactionRepository.GetPartTransaction(partTransaction.imtUniqueID);
					createdObject = new ERPPartTransactionDto
					{
						imtCogsCalculatedDate = eRPPartTransactionInformationDto.imtCogsCalculatedDate,
						imtCreatedBy = eRPPartTransactionInformationDto.imtCreatedBy,
						imtCreatedDate = eRPPartTransactionInformationDto.imtCreatedDate,
						imtUniqueID = eRPPartTransactionInformationDto.imtUniqueID,
						imtHeatLot = eRPPartTransactionInformationDto.imtHeatLot,
						imtInspectionStatus = eRPPartTransactionInformationDto.imtInspectionStatus,
						imtInventoryQuantityReceived = eRPPartTransactionInformationDto.imtInventoryQuantityReceived,
						imtInventoryUnitOfMeasure = eRPPartTransactionInformationDto.imtInventoryUnitOfMeasure,
						imtCogsPostedToGl = eRPPartTransactionInformationDto.imtCogsPostedToGl,
						imtJobCompleteStatus = eRPPartTransactionInformationDto.imtJobCompleteStatus,
						imtNonInventoryTransaction = eRPPartTransactionInformationDto.imtNonInventoryTransaction,
						imtNonNettable = eRPPartTransactionInformationDto.imtNonNettable,
						imtPoLineReceivedComplete = eRPPartTransactionInformationDto.imtPoLineReceivedComplete,
						imtRequiresInspection = eRPPartTransactionInformationDto.imtRequiresInspection,
						imtIssueType = eRPPartTransactionInformationDto.imtIssueType,
						imtJobAssemblyID = eRPPartTransactionInformationDto.imtJobAssemblyID,
						imtJobID = eRPPartTransactionInformationDto.imtJobID,
						imtJobMaterialComponentID = eRPPartTransactionInformationDto.imtJobMaterialComponentID,
						imtJobMaterialID = eRPPartTransactionInformationDto.imtJobMaterialID,
						imtJobOperationID = eRPPartTransactionInformationDto.imtJobOperationID,
						imtJobType = eRPPartTransactionInformationDto.imtJobType,
						imtPartBinID = eRPPartTransactionInformationDto.imtPartBinID,
						imtPartID = eRPPartTransactionInformationDto.imtPartID,
						imtPartRevisionID = eRPPartTransactionInformationDto.imtPartRevisionID,
						imtPartWarehouseLocationID = eRPPartTransactionInformationDto.imtPartWarehouseLocationID,
						imtPlantID = eRPPartTransactionInformationDto.imtPlantID,
						imtPreviousQuantityOnHand = eRPPartTransactionInformationDto.imtPreviousQuantityOnHand,
						imtProjectAreaID = eRPPartTransactionInformationDto.imtProjectAreaID,
						imtProjectID = eRPPartTransactionInformationDto.imtProjectID,
						imtQuantityToInspect = eRPPartTransactionInformationDto.imtQuantityToInspect,
						imtQuantityToReturn = eRPPartTransactionInformationDto.imtQuantityToReturn,
						imtReceiptType = eRPPartTransactionInformationDto.imtReceiptType,
						imtReference = eRPPartTransactionInformationDto.imtReference,
						imtRowVersion = eRPPartTransactionInformationDto.imtRowVersion,
						imtScrapQuantity = eRPPartTransactionInformationDto.imtScrapQuantity,
						imtPartTransactionID = eRPPartTransactionInformationDto.imtPartTransactionID,
						imtSetupCharge = eRPPartTransactionInformationDto.imtSetupCharge,
						imtSource = eRPPartTransactionInformationDto.imtSource,
						imtTableName = eRPPartTransactionInformationDto.imtTableName,
						imtTableUniqueID = eRPPartTransactionInformationDto.imtTableUniqueID,
						imtTransactionDate = eRPPartTransactionInformationDto.imtTransactionDate,
						imtTransactionType = eRPPartTransactionInformationDto.imtTransactionType,
						imtUserID = eRPPartTransactionInformationDto.imtUserID,
						CustomFields = eRPPartTransactionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing PartTransaction [{partTransaction.imtUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeletePartTransaction(Guid partTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
		using (iERPPartTransactionRepository)
		{
			if (!(await base.ERPPartTransactionRepository.DoesPartTransactionExist(partTransactionId)))
			{
				base.ErrorsList.Add($"PartTransaction [{partTransactionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPPartTransactionInformationDto eRPPartTransactionInformationDto = await base.ERPPartTransactionRepository.GetPartTransaction(partTransactionId);
				string text = await base.ERPPartTransactionRepository.WhereUsed("PartTransactions", new object[1] { eRPPartTransactionInformationDto.imtPartTransactionID }, new object[1] { "imtPartTransactionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("PartTransaction cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPPartTransactionDto>> Process_DeletePartTransaction(Guid partTransactionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPPartTransactionDto> result;
		try
		{
			IERPPartTransactionRepository iERPPartTransactionRepository = (base.ERPPartTransactionRepository = new ERPPartTransactionRepository(base.ApiClientContext));
			using (iERPPartTransactionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPPartTransactionRepository.DeleteRowFromTable("PartTransactions", "imt", partTransactionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of PartTransaction [{partTransactionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPPartTransactionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPPartTransactionDto()
			};
		}
		return result;
	}
}
