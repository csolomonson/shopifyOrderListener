using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMRPJobDetailModel : ERPBaseModel, IERPMRPJobDetailModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMRPJobDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
		using (iERPMRPJobDetailRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMRPJobDetailRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMRPJobDetailRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMRPJobDetailRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMRPJobDetailRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMRPJobDetail(Guid mRPJobDetailId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
		using (iERPMRPJobDetailRepository)
		{
			if (!(await base.ERPMRPJobDetailRepository.DoesMRPJobDetailExist(mRPJobDetailId)))
			{
				errorsList.Add($"MRPJobDetail [{mRPJobDetailId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
		using (iERPMRPJobDetailRepository)
		{
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjSessionID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("MRPSessions", new object[1] { "mrpSessionID" }, new object[1] { mRPJobDetail.mrjSessionID })))
			{
				errorsList.Add("mrjSessionID [" + mRPJobDetail.mrjSessionID + "] not found.");
			}
			if (mRPJobDetail.mrjLineID > 0 && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("MRPLines", new object[2] { "mrlSessionID", "mrlLineID" }, new object[2] { mRPJobDetail.mrjSessionID, mRPJobDetail.mrjLineID })))
			{
				errorsList.Add($"mrjLineID [{mRPJobDetail.mrjLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjCustomerOrganizationID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mRPJobDetail.mrjCustomerOrganizationID })))
			{
				errorsList.Add("mrjCustomerOrganizationID [" + mRPJobDetail.mrjCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjPartID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mRPJobDetail.mrjPartID })))
			{
				errorsList.Add("mrjPartID [" + mRPJobDetail.mrjPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjPartRevisionID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mRPJobDetail.mrjPartID, mRPJobDetail.mrjPartRevisionID })))
			{
				errorsList.Add("mrjPartRevisionID [" + mRPJobDetail.mrjPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjPartWarehouseLocationID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { mRPJobDetail.mrjPartID, mRPJobDetail.mrjPartRevisionID, mRPJobDetail.mrjPartWarehouseLocationID })))
			{
				errorsList.Add("mrjPartWarehouseLocationID [" + mRPJobDetail.mrjPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjPartBinID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { mRPJobDetail.mrjPartID, mRPJobDetail.mrjPartRevisionID, mRPJobDetail.mrjPartWarehouseLocationID, mRPJobDetail.mrjPartBinID })))
			{
				errorsList.Add("mrjPartBinID [" + mRPJobDetail.mrjPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjSalesOrderID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { mRPJobDetail.mrjSalesOrderID })))
			{
				errorsList.Add("mrjSalesOrderID [" + mRPJobDetail.mrjSalesOrderID + "] not found.");
			}
			if (mRPJobDetail.mrjSalesOrderLineID > 0 && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { mRPJobDetail.mrjSalesOrderID, mRPJobDetail.mrjSalesOrderLineID })))
			{
				errorsList.Add($"mrjSalesOrderLineID [{mRPJobDetail.mrjSalesOrderLineID}] not found.");
			}
			if (mRPJobDetail.mrjSalesOrderDeliveryID > 0 && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { mRPJobDetail.mrjSalesOrderID, mRPJobDetail.mrjSalesOrderLineID, mRPJobDetail.mrjSalesOrderDeliveryID })))
			{
				errorsList.Add($"mrjSalesOrderDeliveryID [{mRPJobDetail.mrjSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjJobID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { mRPJobDetail.mrjJobID })))
			{
				errorsList.Add("mrjJobID [" + mRPJobDetail.mrjJobID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjShipLocationID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { mRPJobDetail.mrjShipOrganizationID, mRPJobDetail.mrjShipLocationID })))
			{
				errorsList.Add("mrjShipLocationID [" + mRPJobDetail.mrjShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjShipOrganizationID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mRPJobDetail.mrjShipOrganizationID })))
			{
				errorsList.Add("mrjShipOrganizationID [" + mRPJobDetail.mrjShipOrganizationID + "] not found.");
			}
			if (mRPJobDetail.mrjJobAssemblyID > 0 && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { mRPJobDetail.mrjJobID, mRPJobDetail.mrjJobAssemblyID })))
			{
				errorsList.Add($"mrjJobAssemblyID [{mRPJobDetail.mrjJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPJobDetail.mrjPartPlantID) && !(await base.ERPMRPJobDetailRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { mRPJobDetail.mrjPartPlantID })))
			{
				errorsList.Add("mrjPartPlantID [" + mRPJobDetail.mrjPartPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMRPJobDetailDto>>> Process_GetAllMRPJobDetails(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMRPJobDetailDto> allMRPJobDetailsDto = new List<ERPMRPJobDetailDto>();
		ERPResponseMessageDto<IList<ERPMRPJobDetailDto>> result;
		try
		{
			IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
			using (iERPMRPJobDetailRepository)
			{
				foreach (ERPMRPJobDetailInformationDto item2 in await base.ERPMRPJobDetailRepository.GetAllMRPJobDetails(pageSize, pageNumber, filter, orderBy))
				{
					ERPMRPJobDetailDto item = new ERPMRPJobDetailDto
					{
						mrjCreatedBy = item2.mrjCreatedBy,
						mrjCreatedDate = item2.mrjCreatedDate,
						mrjCustomerOrganizationID = item2.mrjCustomerOrganizationID,
						mrjUniqueID = item2.mrjUniqueID,
						mrjInventoryQuantity = item2.mrjInventoryQuantity,
						mrjCompleted = item2.mrjCompleted,
						mrjConsolidated = item2.mrjConsolidated,
						mrjDataMissing = item2.mrjDataMissing,
						mrjDirectLink = item2.mrjDirectLink,
						mrjExistingJob = item2.mrjExistingJob,
						mrjFirm = item2.mrjFirm,
						mrjGetPartMethod = item2.mrjGetPartMethod,
						mrjIndirectLink = item2.mrjIndirectLink,
						mrjJobAssemblyID = item2.mrjJobAssemblyID,
						mrjJobDetailID = item2.mrjJobDetailID,
						mrjJobID = item2.mrjJobID,
						mrjLineID = item2.mrjLineID,
						mrjOrderQuantity = item2.mrjOrderQuantity,
						mrjPartBinID = item2.mrjPartBinID,
						mrjPartID = item2.mrjPartID,
						mrjPartPlantID = item2.mrjPartPlantID,
						mrjPartRevisionID = item2.mrjPartRevisionID,
						mrjPartWarehouseLocationID = item2.mrjPartWarehouseLocationID,
						mrjProductionDueDate = item2.mrjProductionDueDate,
						mrjRowVersion = item2.mrjRowVersion,
						mrjSalesOrderDeliveryID = item2.mrjSalesOrderDeliveryID,
						mrjSalesOrderID = item2.mrjSalesOrderID,
						mrjSalesOrderLineID = item2.mrjSalesOrderLineID,
						mrjSessionID = item2.mrjSessionID,
						mrjShipLocationID = item2.mrjShipLocationID,
						mrjShipOrganizationID = item2.mrjShipOrganizationID,
						CustomFields = item2.CustomFields
					};
					allMRPJobDetailsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MRPJobDetails]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMRPJobDetailDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMRPJobDetailsDto,
				RecordCount = allMRPJobDetailsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_GetMRPJobDetail(Guid mRPJobDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMRPJobDetailDto mRPJobDetailDto = null;
		ERPResponseMessageDto<ERPMRPJobDetailDto> result;
		try
		{
			IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
			using (iERPMRPJobDetailRepository)
			{
				ERPMRPJobDetailInformationDto eRPMRPJobDetailInformationDto = await base.ERPMRPJobDetailRepository.GetMRPJobDetail(mRPJobDetailId);
				mRPJobDetailDto = new ERPMRPJobDetailDto
				{
					mrjCreatedBy = eRPMRPJobDetailInformationDto.mrjCreatedBy,
					mrjCreatedDate = eRPMRPJobDetailInformationDto.mrjCreatedDate,
					mrjCustomerOrganizationID = eRPMRPJobDetailInformationDto.mrjCustomerOrganizationID,
					mrjUniqueID = eRPMRPJobDetailInformationDto.mrjUniqueID,
					mrjInventoryQuantity = eRPMRPJobDetailInformationDto.mrjInventoryQuantity,
					mrjCompleted = eRPMRPJobDetailInformationDto.mrjCompleted,
					mrjConsolidated = eRPMRPJobDetailInformationDto.mrjConsolidated,
					mrjDataMissing = eRPMRPJobDetailInformationDto.mrjDataMissing,
					mrjDirectLink = eRPMRPJobDetailInformationDto.mrjDirectLink,
					mrjExistingJob = eRPMRPJobDetailInformationDto.mrjExistingJob,
					mrjFirm = eRPMRPJobDetailInformationDto.mrjFirm,
					mrjGetPartMethod = eRPMRPJobDetailInformationDto.mrjGetPartMethod,
					mrjIndirectLink = eRPMRPJobDetailInformationDto.mrjIndirectLink,
					mrjJobAssemblyID = eRPMRPJobDetailInformationDto.mrjJobAssemblyID,
					mrjJobDetailID = eRPMRPJobDetailInformationDto.mrjJobDetailID,
					mrjJobID = eRPMRPJobDetailInformationDto.mrjJobID,
					mrjLineID = eRPMRPJobDetailInformationDto.mrjLineID,
					mrjOrderQuantity = eRPMRPJobDetailInformationDto.mrjOrderQuantity,
					mrjPartBinID = eRPMRPJobDetailInformationDto.mrjPartBinID,
					mrjPartID = eRPMRPJobDetailInformationDto.mrjPartID,
					mrjPartPlantID = eRPMRPJobDetailInformationDto.mrjPartPlantID,
					mrjPartRevisionID = eRPMRPJobDetailInformationDto.mrjPartRevisionID,
					mrjPartWarehouseLocationID = eRPMRPJobDetailInformationDto.mrjPartWarehouseLocationID,
					mrjProductionDueDate = eRPMRPJobDetailInformationDto.mrjProductionDueDate,
					mrjRowVersion = eRPMRPJobDetailInformationDto.mrjRowVersion,
					mrjSalesOrderDeliveryID = eRPMRPJobDetailInformationDto.mrjSalesOrderDeliveryID,
					mrjSalesOrderID = eRPMRPJobDetailInformationDto.mrjSalesOrderID,
					mrjSalesOrderLineID = eRPMRPJobDetailInformationDto.mrjSalesOrderLineID,
					mrjSessionID = eRPMRPJobDetailInformationDto.mrjSessionID,
					mrjShipLocationID = eRPMRPJobDetailInformationDto.mrjShipLocationID,
					mrjShipOrganizationID = eRPMRPJobDetailInformationDto.mrjShipOrganizationID,
					CustomFields = eRPMRPJobDetailInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MRPJobDetails []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPJobDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mRPJobDetailDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_PutMRPJobDetail(ERPMRPJobDetailDto mRPJobDetail)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMRPJobDetailDto createdObject = null;
		ERPResponseMessageDto<ERPMRPJobDetailDto> result;
		try
		{
			IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
			using (iERPMRPJobDetailRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMRPJobDetailRepository.SaveMRPJobDetail(mRPJobDetail);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMRPJobDetailInformationDto eRPMRPJobDetailInformationDto = await base.ERPMRPJobDetailRepository.GetMRPJobDetail(mRPJobDetail.mrjUniqueID);
					createdObject = new ERPMRPJobDetailDto
					{
						mrjCreatedBy = eRPMRPJobDetailInformationDto.mrjCreatedBy,
						mrjCreatedDate = eRPMRPJobDetailInformationDto.mrjCreatedDate,
						mrjCustomerOrganizationID = eRPMRPJobDetailInformationDto.mrjCustomerOrganizationID,
						mrjUniqueID = eRPMRPJobDetailInformationDto.mrjUniqueID,
						mrjInventoryQuantity = eRPMRPJobDetailInformationDto.mrjInventoryQuantity,
						mrjCompleted = eRPMRPJobDetailInformationDto.mrjCompleted,
						mrjConsolidated = eRPMRPJobDetailInformationDto.mrjConsolidated,
						mrjDataMissing = eRPMRPJobDetailInformationDto.mrjDataMissing,
						mrjDirectLink = eRPMRPJobDetailInformationDto.mrjDirectLink,
						mrjExistingJob = eRPMRPJobDetailInformationDto.mrjExistingJob,
						mrjFirm = eRPMRPJobDetailInformationDto.mrjFirm,
						mrjGetPartMethod = eRPMRPJobDetailInformationDto.mrjGetPartMethod,
						mrjIndirectLink = eRPMRPJobDetailInformationDto.mrjIndirectLink,
						mrjJobAssemblyID = eRPMRPJobDetailInformationDto.mrjJobAssemblyID,
						mrjJobDetailID = eRPMRPJobDetailInformationDto.mrjJobDetailID,
						mrjJobID = eRPMRPJobDetailInformationDto.mrjJobID,
						mrjLineID = eRPMRPJobDetailInformationDto.mrjLineID,
						mrjOrderQuantity = eRPMRPJobDetailInformationDto.mrjOrderQuantity,
						mrjPartBinID = eRPMRPJobDetailInformationDto.mrjPartBinID,
						mrjPartID = eRPMRPJobDetailInformationDto.mrjPartID,
						mrjPartPlantID = eRPMRPJobDetailInformationDto.mrjPartPlantID,
						mrjPartRevisionID = eRPMRPJobDetailInformationDto.mrjPartRevisionID,
						mrjPartWarehouseLocationID = eRPMRPJobDetailInformationDto.mrjPartWarehouseLocationID,
						mrjProductionDueDate = eRPMRPJobDetailInformationDto.mrjProductionDueDate,
						mrjRowVersion = eRPMRPJobDetailInformationDto.mrjRowVersion,
						mrjSalesOrderDeliveryID = eRPMRPJobDetailInformationDto.mrjSalesOrderDeliveryID,
						mrjSalesOrderID = eRPMRPJobDetailInformationDto.mrjSalesOrderID,
						mrjSalesOrderLineID = eRPMRPJobDetailInformationDto.mrjSalesOrderLineID,
						mrjSessionID = eRPMRPJobDetailInformationDto.mrjSessionID,
						mrjShipLocationID = eRPMRPJobDetailInformationDto.mrjShipLocationID,
						mrjShipOrganizationID = eRPMRPJobDetailInformationDto.mrjShipOrganizationID,
						CustomFields = eRPMRPJobDetailInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MRPJobDetail [{mRPJobDetail.mrjUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPJobDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMRPJobDetail(Guid mRPJobDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
		using (iERPMRPJobDetailRepository)
		{
			if (!(await base.ERPMRPJobDetailRepository.DoesMRPJobDetailExist(mRPJobDetailId)))
			{
				base.ErrorsList.Add($"MRPJobDetail [{mRPJobDetailId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMRPJobDetailInformationDto eRPMRPJobDetailInformationDto = await base.ERPMRPJobDetailRepository.GetMRPJobDetail(mRPJobDetailId);
				string text = await base.ERPMRPJobDetailRepository.WhereUsed("MRPJobDetails", new object[3] { eRPMRPJobDetailInformationDto.mrjSessionID, eRPMRPJobDetailInformationDto.mrjLineID, eRPMRPJobDetailInformationDto.mrjJobDetailID }, new object[3] { "mrjSessionID", "mrjLineID", "mrjJobDetailID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MRPJobDetail cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMRPJobDetailDto>> Process_DeleteMRPJobDetail(Guid mRPJobDetailId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMRPJobDetailDto> result;
		try
		{
			IERPMRPJobDetailRepository iERPMRPJobDetailRepository = (base.ERPMRPJobDetailRepository = new ERPMRPJobDetailRepository(base.ApiClientContext));
			using (iERPMRPJobDetailRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMRPJobDetailRepository.DeleteRowFromTable("MRPJobDetails", "mrj", mRPJobDetailId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MRPJobDetail [{mRPJobDetailId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPJobDetailDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMRPJobDetailDto()
			};
		}
		return result;
	}
}
