using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPJobCostModel : ERPBaseModel, IERPJobCostModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllJobCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
		using (iERPJobCostRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPJobCostRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPJobCostRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPJobCostRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPJobCostRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetJobCost(Guid jobCostId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
		using (iERPJobCostRepository)
		{
			if (!(await base.ERPJobCostRepository.DoesJobCostExist(jobCostId)))
			{
				errorsList.Add($"JobCost [{jobCostId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutJobCost(ERPJobCostDto jobCost)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
		using (iERPJobCostRepository)
		{
			if (!string.IsNullOrWhiteSpace(jobCost.jmcJobID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { jobCost.jmcJobID })))
			{
				errorsList.Add("jmcJobID [" + jobCost.jmcJobID + "] not found.");
			}
			if (jobCost.jmcJobAssemblyID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { jobCost.jmcJobID, jobCost.jmcJobAssemblyID })))
			{
				errorsList.Add($"jmcJobAssemblyID [{jobCost.jmcJobAssemblyID}] not found.");
			}
			if (jobCost.jmcJobMaterialID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { jobCost.jmcJobID, jobCost.jmcJobAssemblyID, jobCost.jmcJobMaterialID })))
			{
				errorsList.Add($"jmcJobMaterialID [{jobCost.jmcJobMaterialID}] not found.");
			}
			if (jobCost.jmcJobOperationID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { jobCost.jmcJobID, jobCost.jmcJobAssemblyID, jobCost.jmcJobOperationID })))
			{
				errorsList.Add($"jmcJobOperationID [{jobCost.jmcJobOperationID}] not found.");
			}
			if (jobCost.jmcJobMaterialComponentID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { jobCost.jmcJobID, jobCost.jmcJobAssemblyID, jobCost.jmcJobMaterialID, jobCost.jmcJobMaterialComponentID })))
			{
				errorsList.Add($"jmcJobMaterialComponentID [{jobCost.jmcJobMaterialComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobCost.jmcPartID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { jobCost.jmcPartID })))
			{
				errorsList.Add("jmcPartID [" + jobCost.jmcPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobCost.jmcPartRevisionID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { jobCost.jmcPartID, jobCost.jmcPartRevisionID })))
			{
				errorsList.Add("jmcPartRevisionID [" + jobCost.jmcPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobCost.jmcSupplierOrganizationID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { jobCost.jmcSupplierOrganizationID })))
			{
				errorsList.Add("jmcSupplierOrganizationID [" + jobCost.jmcSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobCost.jmcReceiptID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { jobCost.jmcReceiptID })))
			{
				errorsList.Add("jmcReceiptID [" + jobCost.jmcReceiptID + "] not found.");
			}
			if (jobCost.jmcReceiptLineID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("ReceiptLines", new object[2] { "RMLRECEIPTID", "RMLRECEIPTLINEID" }, new object[2] { jobCost.jmcReceiptID, jobCost.jmcReceiptLineID })))
			{
				errorsList.Add($"jmcReceiptLineID [{jobCost.jmcReceiptLineID}] not found.");
			}
			if (jobCost.jmcReceiptComponentID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("ReceiptComponents", new object[3] { "RMORECEIPTID", "RMORECEIPTLINEID", "RMORECEIPTCOMPONENTID" }, new object[3] { jobCost.jmcReceiptID, jobCost.jmcReceiptLineID, jobCost.jmcReceiptComponentID })))
			{
				errorsList.Add($"jmcReceiptComponentID [{jobCost.jmcReceiptComponentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(jobCost.jmcApInvoiceID) && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { jobCost.jmcApInvoiceID })))
			{
				errorsList.Add("jmcApInvoiceID [" + jobCost.jmcApInvoiceID + "] not found.");
			}
			if (jobCost.jmcApInvoiceLineID > 0 && !(await base.ERPJobCostRepository.DoesRecordExistInTableUsingKeys("APInvoiceLines", new object[2] { "APLAPINVOICEID", "APLAPINVOICELINEID" }, new object[2] { jobCost.jmcApInvoiceID, jobCost.jmcApInvoiceLineID })))
			{
				errorsList.Add($"jmcApInvoiceLineID [{jobCost.jmcApInvoiceLineID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPJobCostDto>>> Process_GetAllJobCosts(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPJobCostDto> allJobCostsDto = new List<ERPJobCostDto>();
		ERPResponseMessageDto<IList<ERPJobCostDto>> result;
		try
		{
			IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
			using (iERPJobCostRepository)
			{
				foreach (ERPJobCostInformationDto item2 in await base.ERPJobCostRepository.GetAllJobCosts(pageSize, pageNumber, filter, orderBy))
				{
					ERPJobCostDto item = new ERPJobCostDto
					{
						jmcApInvoiceID = item2.jmcApInvoiceID,
						jmcApInvoiceLineID = item2.jmcApInvoiceLineID,
						jmcCostSequence = item2.jmcCostSequence,
						jmcCreatedBy = item2.jmcCreatedBy,
						jmcCreatedDate = item2.jmcCreatedDate,
						jmcUniqueID = item2.jmcUniqueID,
						jmcHeatLot = item2.jmcHeatLot,
						jmcJobAssemblyID = item2.jmcJobAssemblyID,
						jmcJobID = item2.jmcJobID,
						jmcJobMaterialComponentID = item2.jmcJobMaterialComponentID,
						jmcJobMaterialID = item2.jmcJobMaterialID,
						jmcJobOperationID = item2.jmcJobOperationID,
						jmcJobSequence = item2.jmcJobSequence,
						jmcJobType = item2.jmcJobType,
						jmcPartDescription = item2.jmcPartDescription,
						jmcPartID = item2.jmcPartID,
						jmcPartRevisionID = item2.jmcPartRevisionID,
						jmcQuantityReceived = item2.jmcQuantityReceived,
						jmcReceiptComponentID = item2.jmcReceiptComponentID,
						jmcReceiptID = item2.jmcReceiptID,
						jmcReceiptLineID = item2.jmcReceiptLineID,
						jmcReceivedUnitOfMeasure = item2.jmcReceivedUnitOfMeasure,
						jmcReference = item2.jmcReference,
						jmcRowVersion = item2.jmcRowVersion,
						jmcSource = item2.jmcSource,
						jmcSupplierOrganizationID = item2.jmcSupplierOrganizationID,
						jmcTotalCogsCost = item2.jmcTotalCogsCost,
						jmcTotalCost = item2.jmcTotalCost,
						jmcTransactionDate = item2.jmcTransactionDate,
						CustomFields = item2.CustomFields
					};
					allJobCostsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all JobCosts]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPJobCostDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allJobCostsDto,
				RecordCount = allJobCostsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobCostDto>> Process_GetJobCost(Guid jobCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPJobCostDto jobCostDto = null;
		ERPResponseMessageDto<ERPJobCostDto> result;
		try
		{
			IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
			using (iERPJobCostRepository)
			{
				ERPJobCostInformationDto eRPJobCostInformationDto = await base.ERPJobCostRepository.GetJobCost(jobCostId);
				jobCostDto = new ERPJobCostDto
				{
					jmcApInvoiceID = eRPJobCostInformationDto.jmcApInvoiceID,
					jmcApInvoiceLineID = eRPJobCostInformationDto.jmcApInvoiceLineID,
					jmcCostSequence = eRPJobCostInformationDto.jmcCostSequence,
					jmcCreatedBy = eRPJobCostInformationDto.jmcCreatedBy,
					jmcCreatedDate = eRPJobCostInformationDto.jmcCreatedDate,
					jmcUniqueID = eRPJobCostInformationDto.jmcUniqueID,
					jmcHeatLot = eRPJobCostInformationDto.jmcHeatLot,
					jmcJobAssemblyID = eRPJobCostInformationDto.jmcJobAssemblyID,
					jmcJobID = eRPJobCostInformationDto.jmcJobID,
					jmcJobMaterialComponentID = eRPJobCostInformationDto.jmcJobMaterialComponentID,
					jmcJobMaterialID = eRPJobCostInformationDto.jmcJobMaterialID,
					jmcJobOperationID = eRPJobCostInformationDto.jmcJobOperationID,
					jmcJobSequence = eRPJobCostInformationDto.jmcJobSequence,
					jmcJobType = eRPJobCostInformationDto.jmcJobType,
					jmcPartDescription = eRPJobCostInformationDto.jmcPartDescription,
					jmcPartID = eRPJobCostInformationDto.jmcPartID,
					jmcPartRevisionID = eRPJobCostInformationDto.jmcPartRevisionID,
					jmcQuantityReceived = eRPJobCostInformationDto.jmcQuantityReceived,
					jmcReceiptComponentID = eRPJobCostInformationDto.jmcReceiptComponentID,
					jmcReceiptID = eRPJobCostInformationDto.jmcReceiptID,
					jmcReceiptLineID = eRPJobCostInformationDto.jmcReceiptLineID,
					jmcReceivedUnitOfMeasure = eRPJobCostInformationDto.jmcReceivedUnitOfMeasure,
					jmcReference = eRPJobCostInformationDto.jmcReference,
					jmcRowVersion = eRPJobCostInformationDto.jmcRowVersion,
					jmcSource = eRPJobCostInformationDto.jmcSource,
					jmcSupplierOrganizationID = eRPJobCostInformationDto.jmcSupplierOrganizationID,
					jmcTotalCogsCost = eRPJobCostInformationDto.jmcTotalCogsCost,
					jmcTotalCost = eRPJobCostInformationDto.jmcTotalCost,
					jmcTransactionDate = eRPJobCostInformationDto.jmcTransactionDate,
					CustomFields = eRPJobCostInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the JobCosts []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = jobCostDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPJobCostDto>> Process_PutJobCost(ERPJobCostDto jobCost)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPJobCostDto createdObject = null;
		ERPResponseMessageDto<ERPJobCostDto> result;
		try
		{
			IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
			using (iERPJobCostRepository)
			{
				APIValidationInfoDto postResult = await base.ERPJobCostRepository.SaveJobCost(jobCost);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPJobCostInformationDto eRPJobCostInformationDto = await base.ERPJobCostRepository.GetJobCost(jobCost.jmcUniqueID);
					createdObject = new ERPJobCostDto
					{
						jmcApInvoiceID = eRPJobCostInformationDto.jmcApInvoiceID,
						jmcApInvoiceLineID = eRPJobCostInformationDto.jmcApInvoiceLineID,
						jmcCostSequence = eRPJobCostInformationDto.jmcCostSequence,
						jmcCreatedBy = eRPJobCostInformationDto.jmcCreatedBy,
						jmcCreatedDate = eRPJobCostInformationDto.jmcCreatedDate,
						jmcUniqueID = eRPJobCostInformationDto.jmcUniqueID,
						jmcHeatLot = eRPJobCostInformationDto.jmcHeatLot,
						jmcJobAssemblyID = eRPJobCostInformationDto.jmcJobAssemblyID,
						jmcJobID = eRPJobCostInformationDto.jmcJobID,
						jmcJobMaterialComponentID = eRPJobCostInformationDto.jmcJobMaterialComponentID,
						jmcJobMaterialID = eRPJobCostInformationDto.jmcJobMaterialID,
						jmcJobOperationID = eRPJobCostInformationDto.jmcJobOperationID,
						jmcJobSequence = eRPJobCostInformationDto.jmcJobSequence,
						jmcJobType = eRPJobCostInformationDto.jmcJobType,
						jmcPartDescription = eRPJobCostInformationDto.jmcPartDescription,
						jmcPartID = eRPJobCostInformationDto.jmcPartID,
						jmcPartRevisionID = eRPJobCostInformationDto.jmcPartRevisionID,
						jmcQuantityReceived = eRPJobCostInformationDto.jmcQuantityReceived,
						jmcReceiptComponentID = eRPJobCostInformationDto.jmcReceiptComponentID,
						jmcReceiptID = eRPJobCostInformationDto.jmcReceiptID,
						jmcReceiptLineID = eRPJobCostInformationDto.jmcReceiptLineID,
						jmcReceivedUnitOfMeasure = eRPJobCostInformationDto.jmcReceivedUnitOfMeasure,
						jmcReference = eRPJobCostInformationDto.jmcReference,
						jmcRowVersion = eRPJobCostInformationDto.jmcRowVersion,
						jmcSource = eRPJobCostInformationDto.jmcSource,
						jmcSupplierOrganizationID = eRPJobCostInformationDto.jmcSupplierOrganizationID,
						jmcTotalCogsCost = eRPJobCostInformationDto.jmcTotalCogsCost,
						jmcTotalCost = eRPJobCostInformationDto.jmcTotalCost,
						jmcTransactionDate = eRPJobCostInformationDto.jmcTransactionDate,
						CustomFields = eRPJobCostInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing JobCost [{jobCost.jmcUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteJobCost(Guid jobCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
		using (iERPJobCostRepository)
		{
			if (!(await base.ERPJobCostRepository.DoesJobCostExist(jobCostId)))
			{
				base.ErrorsList.Add($"JobCost [{jobCostId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPJobCostInformationDto eRPJobCostInformationDto = await base.ERPJobCostRepository.GetJobCost(jobCostId);
				string text = await base.ERPJobCostRepository.WhereUsed("JobCosts", new object[5] { eRPJobCostInformationDto.jmcJobID, eRPJobCostInformationDto.jmcJobAssemblyID, eRPJobCostInformationDto.jmcJobType, eRPJobCostInformationDto.jmcJobSequence, eRPJobCostInformationDto.jmcCostSequence }, new object[5] { "jmcJobID", "jmcJobAssemblyID", "jmcJobType", "jmcJobSequence", "jmcCostSequence" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("JobCost cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPJobCostDto>> Process_DeleteJobCost(Guid jobCostId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPJobCostDto> result;
		try
		{
			IERPJobCostRepository iERPJobCostRepository = (base.ERPJobCostRepository = new ERPJobCostRepository(base.ApiClientContext));
			using (iERPJobCostRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPJobCostRepository.DeleteRowFromTable("JobCosts", "jmc", jobCostId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of JobCost [{jobCostId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPJobCostDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPJobCostDto()
			};
		}
		return result;
	}
}
