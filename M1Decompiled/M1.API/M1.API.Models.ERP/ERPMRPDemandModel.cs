using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMRPDemandModel : ERPBaseModel, IERPMRPDemandModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMRPDemands(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
		using (iERPMRPDemandRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMRPDemandRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMRPDemandRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMRPDemandRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMRPDemandRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMRPDemand(Guid mRPDemandId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
		using (iERPMRPDemandRepository)
		{
			if (!(await base.ERPMRPDemandRepository.DoesMRPDemandExist(mRPDemandId)))
			{
				errorsList.Add($"MRPDemand [{mRPDemandId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMRPDemand(ERPMRPDemandDto mRPDemand)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
		using (iERPMRPDemandRepository)
		{
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrSessionID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("MRPSessions", new object[1] { "mrpSessionID" }, new object[1] { mRPDemand.mrrSessionID })))
			{
				errorsList.Add("mrrSessionID [" + mRPDemand.mrrSessionID + "] not found.");
			}
			if (mRPDemand.mrrLineID > 0 && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("MRPLines", new object[2] { "mrlSessionID", "mrlLineID" }, new object[2] { mRPDemand.mrrSessionID, mRPDemand.mrrLineID })))
			{
				errorsList.Add($"mrrLineID [{mRPDemand.mrrLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrPartID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mRPDemand.mrrPartID })))
			{
				errorsList.Add("mrrPartID [" + mRPDemand.mrrPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrPartRevisionID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mRPDemand.mrrPartID, mRPDemand.mrrPartRevisionID })))
			{
				errorsList.Add("mrrPartRevisionID [" + mRPDemand.mrrPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrPartWarehouseLocationID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { mRPDemand.mrrPartID, mRPDemand.mrrPartRevisionID, mRPDemand.mrrPartWarehouseLocationID })))
			{
				errorsList.Add("mrrPartWarehouseLocationID [" + mRPDemand.mrrPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrPartBinID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { mRPDemand.mrrPartID, mRPDemand.mrrPartRevisionID, mRPDemand.mrrPartWarehouseLocationID, mRPDemand.mrrPartBinID })))
			{
				errorsList.Add("mrrPartBinID [" + mRPDemand.mrrPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrSalesOrderID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("SalesOrders", new object[1] { "OMPSALESORDERID" }, new object[1] { mRPDemand.mrrSalesOrderID })))
			{
				errorsList.Add("mrrSalesOrderID [" + mRPDemand.mrrSalesOrderID + "] not found.");
			}
			if (mRPDemand.mrrSalesOrderLineID > 0 && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("SalesOrderLines", new object[2] { "OMLSALESORDERID", "OMLSALESORDERLINEID" }, new object[2] { mRPDemand.mrrSalesOrderID, mRPDemand.mrrSalesOrderLineID })))
			{
				errorsList.Add($"mrrSalesOrderLineID [{mRPDemand.mrrSalesOrderLineID}] not found.");
			}
			if (mRPDemand.mrrSalesOrderDeliveryID > 0 && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("SalesOrderDeliveries", new object[3] { "OMDSALESORDERID", "OMDSALESORDERLINEID", "OMDSALESORDERDELIVERYID" }, new object[3] { mRPDemand.mrrSalesOrderID, mRPDemand.mrrSalesOrderLineID, mRPDemand.mrrSalesOrderDeliveryID })))
			{
				errorsList.Add($"mrrSalesOrderDeliveryID [{mRPDemand.mrrSalesOrderDeliveryID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrJobID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { mRPDemand.mrrJobID })))
			{
				errorsList.Add("mrrJobID [" + mRPDemand.mrrJobID + "] not found.");
			}
			if (mRPDemand.mrrJobAssemblyID > 0 && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { mRPDemand.mrrJobID, mRPDemand.mrrJobAssemblyID })))
			{
				errorsList.Add($"mrrJobAssemblyID [{mRPDemand.mrrJobAssemblyID}] not found.");
			}
			if (mRPDemand.mrrJobMaterialID > 0 && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { mRPDemand.mrrJobID, mRPDemand.mrrJobAssemblyID, mRPDemand.mrrJobMaterialID })))
			{
				errorsList.Add($"mrrJobMaterialID [{mRPDemand.mrrJobMaterialID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrShipOrganizationID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mRPDemand.mrrShipOrganizationID })))
			{
				errorsList.Add("mrrShipOrganizationID [" + mRPDemand.mrrShipOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrCustomerOrganizationID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mRPDemand.mrrCustomerOrganizationID })))
			{
				errorsList.Add("mrrCustomerOrganizationID [" + mRPDemand.mrrCustomerOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrShipLocationID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { mRPDemand.mrrShipOrganizationID, mRPDemand.mrrShipLocationID })))
			{
				errorsList.Add("mrrShipLocationID [" + mRPDemand.mrrShipLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPDemand.mrrPartPlantID) && !(await base.ERPMRPDemandRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { mRPDemand.mrrPartPlantID })))
			{
				errorsList.Add("mrrPartPlantID [" + mRPDemand.mrrPartPlantID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMRPDemandDto>>> Process_GetAllMRPDemands(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMRPDemandDto> allMRPDemandsDto = new List<ERPMRPDemandDto>();
		ERPResponseMessageDto<IList<ERPMRPDemandDto>> result;
		try
		{
			IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
			using (iERPMRPDemandRepository)
			{
				foreach (ERPMRPDemandInformationDto item2 in await base.ERPMRPDemandRepository.GetAllMRPDemands(pageSize, pageNumber, filter, orderBy))
				{
					ERPMRPDemandDto item = new ERPMRPDemandDto
					{
						mrrCreatedBy = item2.mrrCreatedBy,
						mrrCreatedDate = item2.mrrCreatedDate,
						mrrCustomerOrganizationID = item2.mrrCustomerOrganizationID,
						mrrDemandID = item2.mrrDemandID,
						mrrDemandQuantity = item2.mrrDemandQuantity,
						mrrDueDate = item2.mrrDueDate,
						mrrUniqueID = item2.mrrUniqueID,
						mrrJobAssemblyID = item2.mrrJobAssemblyID,
						mrrJobID = item2.mrrJobID,
						mrrJobMaterialID = item2.mrrJobMaterialID,
						mrrLineID = item2.mrrLineID,
						mrrOriginalQuantity = item2.mrrOriginalQuantity,
						mrrPartBinID = item2.mrrPartBinID,
						mrrPartID = item2.mrrPartID,
						mrrPartPlantID = item2.mrrPartPlantID,
						mrrPartRevisionID = item2.mrrPartRevisionID,
						mrrPartWarehouseLocationID = item2.mrrPartWarehouseLocationID,
						mrrQuantityReceived = item2.mrrQuantityReceived,
						mrrQuantityShipped = item2.mrrQuantityShipped,
						mrrRowVersion = item2.mrrRowVersion,
						mrrSalesOrderDeliveryID = item2.mrrSalesOrderDeliveryID,
						mrrSalesOrderID = item2.mrrSalesOrderID,
						mrrSalesOrderLineID = item2.mrrSalesOrderLineID,
						mrrSessionID = item2.mrrSessionID,
						mrrShipLocationID = item2.mrrShipLocationID,
						mrrShipOrganizationID = item2.mrrShipOrganizationID,
						mrrSource = item2.mrrSource,
						mrrType = item2.mrrType,
						CustomFields = item2.CustomFields
					};
					allMRPDemandsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MRPDemands]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMRPDemandDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMRPDemandsDto,
				RecordCount = allMRPDemandsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_GetMRPDemand(Guid mRPDemandId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMRPDemandDto mRPDemandDto = null;
		ERPResponseMessageDto<ERPMRPDemandDto> result;
		try
		{
			IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
			using (iERPMRPDemandRepository)
			{
				ERPMRPDemandInformationDto eRPMRPDemandInformationDto = await base.ERPMRPDemandRepository.GetMRPDemand(mRPDemandId);
				mRPDemandDto = new ERPMRPDemandDto
				{
					mrrCreatedBy = eRPMRPDemandInformationDto.mrrCreatedBy,
					mrrCreatedDate = eRPMRPDemandInformationDto.mrrCreatedDate,
					mrrCustomerOrganizationID = eRPMRPDemandInformationDto.mrrCustomerOrganizationID,
					mrrDemandID = eRPMRPDemandInformationDto.mrrDemandID,
					mrrDemandQuantity = eRPMRPDemandInformationDto.mrrDemandQuantity,
					mrrDueDate = eRPMRPDemandInformationDto.mrrDueDate,
					mrrUniqueID = eRPMRPDemandInformationDto.mrrUniqueID,
					mrrJobAssemblyID = eRPMRPDemandInformationDto.mrrJobAssemblyID,
					mrrJobID = eRPMRPDemandInformationDto.mrrJobID,
					mrrJobMaterialID = eRPMRPDemandInformationDto.mrrJobMaterialID,
					mrrLineID = eRPMRPDemandInformationDto.mrrLineID,
					mrrOriginalQuantity = eRPMRPDemandInformationDto.mrrOriginalQuantity,
					mrrPartBinID = eRPMRPDemandInformationDto.mrrPartBinID,
					mrrPartID = eRPMRPDemandInformationDto.mrrPartID,
					mrrPartPlantID = eRPMRPDemandInformationDto.mrrPartPlantID,
					mrrPartRevisionID = eRPMRPDemandInformationDto.mrrPartRevisionID,
					mrrPartWarehouseLocationID = eRPMRPDemandInformationDto.mrrPartWarehouseLocationID,
					mrrQuantityReceived = eRPMRPDemandInformationDto.mrrQuantityReceived,
					mrrQuantityShipped = eRPMRPDemandInformationDto.mrrQuantityShipped,
					mrrRowVersion = eRPMRPDemandInformationDto.mrrRowVersion,
					mrrSalesOrderDeliveryID = eRPMRPDemandInformationDto.mrrSalesOrderDeliveryID,
					mrrSalesOrderID = eRPMRPDemandInformationDto.mrrSalesOrderID,
					mrrSalesOrderLineID = eRPMRPDemandInformationDto.mrrSalesOrderLineID,
					mrrSessionID = eRPMRPDemandInformationDto.mrrSessionID,
					mrrShipLocationID = eRPMRPDemandInformationDto.mrrShipLocationID,
					mrrShipOrganizationID = eRPMRPDemandInformationDto.mrrShipOrganizationID,
					mrrSource = eRPMRPDemandInformationDto.mrrSource,
					mrrType = eRPMRPDemandInformationDto.mrrType,
					CustomFields = eRPMRPDemandInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MRPDemands []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPDemandDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mRPDemandDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_PutMRPDemand(ERPMRPDemandDto mRPDemand)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMRPDemandDto createdObject = null;
		ERPResponseMessageDto<ERPMRPDemandDto> result;
		try
		{
			IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
			using (iERPMRPDemandRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMRPDemandRepository.SaveMRPDemand(mRPDemand);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMRPDemandInformationDto eRPMRPDemandInformationDto = await base.ERPMRPDemandRepository.GetMRPDemand(mRPDemand.mrrUniqueID);
					createdObject = new ERPMRPDemandDto
					{
						mrrCreatedBy = eRPMRPDemandInformationDto.mrrCreatedBy,
						mrrCreatedDate = eRPMRPDemandInformationDto.mrrCreatedDate,
						mrrCustomerOrganizationID = eRPMRPDemandInformationDto.mrrCustomerOrganizationID,
						mrrDemandID = eRPMRPDemandInformationDto.mrrDemandID,
						mrrDemandQuantity = eRPMRPDemandInformationDto.mrrDemandQuantity,
						mrrDueDate = eRPMRPDemandInformationDto.mrrDueDate,
						mrrUniqueID = eRPMRPDemandInformationDto.mrrUniqueID,
						mrrJobAssemblyID = eRPMRPDemandInformationDto.mrrJobAssemblyID,
						mrrJobID = eRPMRPDemandInformationDto.mrrJobID,
						mrrJobMaterialID = eRPMRPDemandInformationDto.mrrJobMaterialID,
						mrrLineID = eRPMRPDemandInformationDto.mrrLineID,
						mrrOriginalQuantity = eRPMRPDemandInformationDto.mrrOriginalQuantity,
						mrrPartBinID = eRPMRPDemandInformationDto.mrrPartBinID,
						mrrPartID = eRPMRPDemandInformationDto.mrrPartID,
						mrrPartPlantID = eRPMRPDemandInformationDto.mrrPartPlantID,
						mrrPartRevisionID = eRPMRPDemandInformationDto.mrrPartRevisionID,
						mrrPartWarehouseLocationID = eRPMRPDemandInformationDto.mrrPartWarehouseLocationID,
						mrrQuantityReceived = eRPMRPDemandInformationDto.mrrQuantityReceived,
						mrrQuantityShipped = eRPMRPDemandInformationDto.mrrQuantityShipped,
						mrrRowVersion = eRPMRPDemandInformationDto.mrrRowVersion,
						mrrSalesOrderDeliveryID = eRPMRPDemandInformationDto.mrrSalesOrderDeliveryID,
						mrrSalesOrderID = eRPMRPDemandInformationDto.mrrSalesOrderID,
						mrrSalesOrderLineID = eRPMRPDemandInformationDto.mrrSalesOrderLineID,
						mrrSessionID = eRPMRPDemandInformationDto.mrrSessionID,
						mrrShipLocationID = eRPMRPDemandInformationDto.mrrShipLocationID,
						mrrShipOrganizationID = eRPMRPDemandInformationDto.mrrShipOrganizationID,
						mrrSource = eRPMRPDemandInformationDto.mrrSource,
						mrrType = eRPMRPDemandInformationDto.mrrType,
						CustomFields = eRPMRPDemandInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MRPDemand [{mRPDemand.mrrUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPDemandDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMRPDemand(Guid mRPDemandId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
		using (iERPMRPDemandRepository)
		{
			if (!(await base.ERPMRPDemandRepository.DoesMRPDemandExist(mRPDemandId)))
			{
				base.ErrorsList.Add($"MRPDemand [{mRPDemandId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMRPDemandInformationDto eRPMRPDemandInformationDto = await base.ERPMRPDemandRepository.GetMRPDemand(mRPDemandId);
				string text = await base.ERPMRPDemandRepository.WhereUsed("MRPDemands", new object[3] { eRPMRPDemandInformationDto.mrrSessionID, eRPMRPDemandInformationDto.mrrLineID, eRPMRPDemandInformationDto.mrrDemandID }, new object[3] { "mrrSessionID", "mrrLineID", "mrrDemandID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MRPDemand cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMRPDemandDto>> Process_DeleteMRPDemand(Guid mRPDemandId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMRPDemandDto> result;
		try
		{
			IERPMRPDemandRepository iERPMRPDemandRepository = (base.ERPMRPDemandRepository = new ERPMRPDemandRepository(base.ApiClientContext));
			using (iERPMRPDemandRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMRPDemandRepository.DeleteRowFromTable("MRPDemands", "mrr", mRPDemandId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MRPDemand [{mRPDemandId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPDemandDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMRPDemandDto()
			};
		}
		return result;
	}
}
