using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPMRPSupplyModel : ERPBaseModel, IERPMRPSupplyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllMRPSupply(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
		using (iERPMRPSupplyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPMRPSupplyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPMRPSupplyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPMRPSupplyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPMRPSupplyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetMRPSupply(Guid mRPSupplyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
		using (iERPMRPSupplyRepository)
		{
			if (!(await base.ERPMRPSupplyRepository.DoesMRPSupplyExist(mRPSupplyId)))
			{
				errorsList.Add($"MRPSupply [{mRPSupplyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutMRPSupply(ERPMRPSupplyDto mRPSupply)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
		using (iERPMRPSupplyRepository)
		{
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsSessionID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("MRPSessions", new object[1] { "mrpSessionID" }, new object[1] { mRPSupply.mrsSessionID })))
			{
				errorsList.Add("mrsSessionID [" + mRPSupply.mrsSessionID + "] not found.");
			}
			if (mRPSupply.mrsLineID > 0 && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("MRPLines", new object[2] { "mrlSessionID", "mrlLineID" }, new object[2] { mRPSupply.mrsSessionID, mRPSupply.mrsLineID })))
			{
				errorsList.Add($"mrsLineID [{mRPSupply.mrsLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsPartID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { mRPSupply.mrsPartID })))
			{
				errorsList.Add("mrsPartID [" + mRPSupply.mrsPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsPartRevisionID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { mRPSupply.mrsPartID, mRPSupply.mrsPartRevisionID })))
			{
				errorsList.Add("mrsPartRevisionID [" + mRPSupply.mrsPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsPartWarehouseLocationID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("PartWarehouseLocations", new object[3] { "IMLPARTID", "IMLPARTREVISIONID", "IMLPARTWAREHOUSEID" }, new object[3] { mRPSupply.mrsPartID, mRPSupply.mrsPartRevisionID, mRPSupply.mrsPartWarehouseLocationID })))
			{
				errorsList.Add("mrsPartWarehouseLocationID [" + mRPSupply.mrsPartWarehouseLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsPartBinID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("PartBins", new object[4] { "IMBPARTID", "IMBPARTREVISIONID", "IMBWAREHOUSEID", "IMBPARTBINID" }, new object[4] { mRPSupply.mrsPartID, mRPSupply.mrsPartRevisionID, mRPSupply.mrsPartWarehouseLocationID, mRPSupply.mrsPartBinID })))
			{
				errorsList.Add("mrsPartBinID [" + mRPSupply.mrsPartBinID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsJobID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { mRPSupply.mrsJobID })))
			{
				errorsList.Add("mrsJobID [" + mRPSupply.mrsJobID + "] not found.");
			}
			if (mRPSupply.mrsJobAssemblyID > 0 && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { mRPSupply.mrsJobID, mRPSupply.mrsJobAssemblyID })))
			{
				errorsList.Add($"mrsJobAssemblyID [{mRPSupply.mrsJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(mRPSupply.mrsCustomerOrganizationID) && !(await base.ERPMRPSupplyRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { mRPSupply.mrsCustomerOrganizationID })))
			{
				errorsList.Add("mrsCustomerOrganizationID [" + mRPSupply.mrsCustomerOrganizationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPMRPSupplyDto>>> Process_GetAllMRPSupply(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPMRPSupplyDto> allMRPSupplyDto = new List<ERPMRPSupplyDto>();
		ERPResponseMessageDto<IList<ERPMRPSupplyDto>> result;
		try
		{
			IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
			using (iERPMRPSupplyRepository)
			{
				foreach (ERPMRPSupplyInformationDto item2 in await base.ERPMRPSupplyRepository.GetAllMRPSupply(pageSize, pageNumber, filter, orderBy))
				{
					ERPMRPSupplyDto item = new ERPMRPSupplyDto
					{
						mrsCreatedBy = item2.mrsCreatedBy,
						mrsCreatedDate = item2.mrsCreatedDate,
						mrsCustomerOrganizationID = item2.mrsCustomerOrganizationID,
						mrsDueDate = item2.mrsDueDate,
						mrsUniqueID = item2.mrsUniqueID,
						mrsJobAssemblyID = item2.mrsJobAssemblyID,
						mrsJobID = item2.mrsJobID,
						mrsLineID = item2.mrsLineID,
						mrsPartBinID = item2.mrsPartBinID,
						mrsPartID = item2.mrsPartID,
						mrsPartRevisionID = item2.mrsPartRevisionID,
						mrsPartWarehouseLocationID = item2.mrsPartWarehouseLocationID,
						mrsQuantityReceived = item2.mrsQuantityReceived,
						mrsQuantityShipped = item2.mrsQuantityShipped,
						mrsRowVersion = item2.mrsRowVersion,
						mrsSessionID = item2.mrsSessionID,
						mrsSource = item2.mrsSource,
						mrsSupplyID = item2.mrsSupplyID,
						mrsType = item2.mrsType,
						CustomFields = item2.CustomFields
					};
					allMRPSupplyDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all MRPSupply]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPMRPSupplyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allMRPSupplyDto,
				RecordCount = allMRPSupplyDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_GetMRPSupply(Guid mRPSupplyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPMRPSupplyDto mRPSupplyDto = null;
		ERPResponseMessageDto<ERPMRPSupplyDto> result;
		try
		{
			IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
			using (iERPMRPSupplyRepository)
			{
				ERPMRPSupplyInformationDto eRPMRPSupplyInformationDto = await base.ERPMRPSupplyRepository.GetMRPSupply(mRPSupplyId);
				mRPSupplyDto = new ERPMRPSupplyDto
				{
					mrsCreatedBy = eRPMRPSupplyInformationDto.mrsCreatedBy,
					mrsCreatedDate = eRPMRPSupplyInformationDto.mrsCreatedDate,
					mrsCustomerOrganizationID = eRPMRPSupplyInformationDto.mrsCustomerOrganizationID,
					mrsDueDate = eRPMRPSupplyInformationDto.mrsDueDate,
					mrsUniqueID = eRPMRPSupplyInformationDto.mrsUniqueID,
					mrsJobAssemblyID = eRPMRPSupplyInformationDto.mrsJobAssemblyID,
					mrsJobID = eRPMRPSupplyInformationDto.mrsJobID,
					mrsLineID = eRPMRPSupplyInformationDto.mrsLineID,
					mrsPartBinID = eRPMRPSupplyInformationDto.mrsPartBinID,
					mrsPartID = eRPMRPSupplyInformationDto.mrsPartID,
					mrsPartRevisionID = eRPMRPSupplyInformationDto.mrsPartRevisionID,
					mrsPartWarehouseLocationID = eRPMRPSupplyInformationDto.mrsPartWarehouseLocationID,
					mrsQuantityReceived = eRPMRPSupplyInformationDto.mrsQuantityReceived,
					mrsQuantityShipped = eRPMRPSupplyInformationDto.mrsQuantityShipped,
					mrsRowVersion = eRPMRPSupplyInformationDto.mrsRowVersion,
					mrsSessionID = eRPMRPSupplyInformationDto.mrsSessionID,
					mrsSource = eRPMRPSupplyInformationDto.mrsSource,
					mrsSupplyID = eRPMRPSupplyInformationDto.mrsSupplyID,
					mrsType = eRPMRPSupplyInformationDto.mrsType,
					CustomFields = eRPMRPSupplyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the MRPSupply []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSupplyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = mRPSupplyDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_PutMRPSupply(ERPMRPSupplyDto mRPSupply)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPMRPSupplyDto createdObject = null;
		ERPResponseMessageDto<ERPMRPSupplyDto> result;
		try
		{
			IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
			using (iERPMRPSupplyRepository)
			{
				APIValidationInfoDto postResult = await base.ERPMRPSupplyRepository.SaveMRPSupply(mRPSupply);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPMRPSupplyInformationDto eRPMRPSupplyInformationDto = await base.ERPMRPSupplyRepository.GetMRPSupply(mRPSupply.mrsUniqueID);
					createdObject = new ERPMRPSupplyDto
					{
						mrsCreatedBy = eRPMRPSupplyInformationDto.mrsCreatedBy,
						mrsCreatedDate = eRPMRPSupplyInformationDto.mrsCreatedDate,
						mrsCustomerOrganizationID = eRPMRPSupplyInformationDto.mrsCustomerOrganizationID,
						mrsDueDate = eRPMRPSupplyInformationDto.mrsDueDate,
						mrsUniqueID = eRPMRPSupplyInformationDto.mrsUniqueID,
						mrsJobAssemblyID = eRPMRPSupplyInformationDto.mrsJobAssemblyID,
						mrsJobID = eRPMRPSupplyInformationDto.mrsJobID,
						mrsLineID = eRPMRPSupplyInformationDto.mrsLineID,
						mrsPartBinID = eRPMRPSupplyInformationDto.mrsPartBinID,
						mrsPartID = eRPMRPSupplyInformationDto.mrsPartID,
						mrsPartRevisionID = eRPMRPSupplyInformationDto.mrsPartRevisionID,
						mrsPartWarehouseLocationID = eRPMRPSupplyInformationDto.mrsPartWarehouseLocationID,
						mrsQuantityReceived = eRPMRPSupplyInformationDto.mrsQuantityReceived,
						mrsQuantityShipped = eRPMRPSupplyInformationDto.mrsQuantityShipped,
						mrsRowVersion = eRPMRPSupplyInformationDto.mrsRowVersion,
						mrsSessionID = eRPMRPSupplyInformationDto.mrsSessionID,
						mrsSource = eRPMRPSupplyInformationDto.mrsSource,
						mrsSupplyID = eRPMRPSupplyInformationDto.mrsSupplyID,
						mrsType = eRPMRPSupplyInformationDto.mrsType,
						CustomFields = eRPMRPSupplyInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing MRPSupply [{mRPSupply.mrsUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSupplyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteMRPSupply(Guid mRPSupplyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
		using (iERPMRPSupplyRepository)
		{
			if (!(await base.ERPMRPSupplyRepository.DoesMRPSupplyExist(mRPSupplyId)))
			{
				base.ErrorsList.Add($"MRPSupply [{mRPSupplyId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPMRPSupplyInformationDto eRPMRPSupplyInformationDto = await base.ERPMRPSupplyRepository.GetMRPSupply(mRPSupplyId);
				string text = await base.ERPMRPSupplyRepository.WhereUsed("MRPSupply", new object[3] { eRPMRPSupplyInformationDto.mrsSessionID, eRPMRPSupplyInformationDto.mrsLineID, eRPMRPSupplyInformationDto.mrsSupplyID }, new object[3] { "mrsSessionID", "mrsLineID", "mrsSupplyID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("MRPSupply cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPMRPSupplyDto>> Process_DeleteMRPSupply(Guid mRPSupplyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPMRPSupplyDto> result;
		try
		{
			IERPMRPSupplyRepository iERPMRPSupplyRepository = (base.ERPMRPSupplyRepository = new ERPMRPSupplyRepository(base.ApiClientContext));
			using (iERPMRPSupplyRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPMRPSupplyRepository.DeleteRowFromTable("MRPSupply", "mrs", mRPSupplyId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of MRPSupply [{mRPSupplyId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPMRPSupplyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPMRPSupplyDto()
			};
		}
		return result;
	}
}
