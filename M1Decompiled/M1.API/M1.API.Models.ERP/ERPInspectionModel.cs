using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPInspectionModel : ERPBaseModel, IERPInspectionModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllInspections(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
		using (iERPInspectionRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPInspectionRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPInspectionRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPInspectionRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPInspectionRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetInspection(Guid inspectionId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
		using (iERPInspectionRepository)
		{
			if (!(await base.ERPInspectionRepository.DoesInspectionExist(inspectionId)))
			{
				errorsList.Add($"Inspection [{inspectionId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutInspection(ERPInspectionDto inspection)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
		using (iERPInspectionRepository)
		{
			if (!string.IsNullOrWhiteSpace(inspection.qapPlantDepartmentID) && !(await base.ERPInspectionRepository.DoesRecordExistInTableUsingKeys("PlantDepartments", new object[2] { "XAVPLANTID", "XAVPLANTDEPARTMENTID" }, new object[2] { inspection.qapPlantID, inspection.qapPlantDepartmentID })))
			{
				errorsList.Add("qapPlantDepartmentID [" + inspection.qapPlantDepartmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspection.qapPlantID) && !(await base.ERPInspectionRepository.DoesRecordExistInTableUsingKeys("Plants", new object[1] { "XAUPLANTID" }, new object[1] { inspection.qapPlantID })))
			{
				errorsList.Add("qapPlantID [" + inspection.qapPlantID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspection.qapProjectID) && !(await base.ERPInspectionRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { inspection.qapProjectID })))
			{
				errorsList.Add("qapProjectID [" + inspection.qapProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(inspection.qapOpenedByEmployeeID) && !(await base.ERPInspectionRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { inspection.qapOpenedByEmployeeID })))
			{
				errorsList.Add("qapOpenedByEmployeeID [" + inspection.qapOpenedByEmployeeID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPInspectionDto>>> Process_GetAllInspections(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPInspectionDto> allInspectionsDto = new List<ERPInspectionDto>();
		ERPResponseMessageDto<IList<ERPInspectionDto>> result;
		try
		{
			IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
			using (iERPInspectionRepository)
			{
				foreach (ERPInspectionInformationDto item2 in await base.ERPInspectionRepository.GetAllInspections(pageSize, pageNumber, filter, orderBy))
				{
					ERPInspectionDto item = new ERPInspectionDto
					{
						qapInspectionID = item2.qapInspectionID,
						qapCreatedBy = item2.qapCreatedBy,
						qapCreatedDate = item2.qapCreatedDate,
						qapUniqueID = item2.qapUniqueID,
						qapPosted = item2.qapPosted,
						qapReversalEntry = item2.qapReversalEntry,
						qapOpenedByEmployeeID = item2.qapOpenedByEmployeeID,
						qapOpenedDate = item2.qapOpenedDate,
						qapPlantDepartmentID = item2.qapPlantDepartmentID,
						qapPlantID = item2.qapPlantID,
						qapPostedDate = item2.qapPostedDate,
						qapProjectID = item2.qapProjectID,
						qapRowVersion = item2.qapRowVersion,
						qapSourceTableName = item2.qapSourceTableName,
						qapSourceTableUniqueID = item2.qapSourceTableUniqueID,
						CustomFields = item2.CustomFields
					};
					allInspectionsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Inspections]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPInspectionDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allInspectionsDto,
				RecordCount = allInspectionsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionDto>> Process_GetInspection(Guid inspectionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPInspectionDto inspectionDto = null;
		ERPResponseMessageDto<ERPInspectionDto> result;
		try
		{
			IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
			using (iERPInspectionRepository)
			{
				ERPInspectionInformationDto eRPInspectionInformationDto = await base.ERPInspectionRepository.GetInspection(inspectionId);
				inspectionDto = new ERPInspectionDto
				{
					qapInspectionID = eRPInspectionInformationDto.qapInspectionID,
					qapCreatedBy = eRPInspectionInformationDto.qapCreatedBy,
					qapCreatedDate = eRPInspectionInformationDto.qapCreatedDate,
					qapUniqueID = eRPInspectionInformationDto.qapUniqueID,
					qapPosted = eRPInspectionInformationDto.qapPosted,
					qapReversalEntry = eRPInspectionInformationDto.qapReversalEntry,
					qapOpenedByEmployeeID = eRPInspectionInformationDto.qapOpenedByEmployeeID,
					qapOpenedDate = eRPInspectionInformationDto.qapOpenedDate,
					qapPlantDepartmentID = eRPInspectionInformationDto.qapPlantDepartmentID,
					qapPlantID = eRPInspectionInformationDto.qapPlantID,
					qapPostedDate = eRPInspectionInformationDto.qapPostedDate,
					qapProjectID = eRPInspectionInformationDto.qapProjectID,
					qapRowVersion = eRPInspectionInformationDto.qapRowVersion,
					qapSourceTableName = eRPInspectionInformationDto.qapSourceTableName,
					qapSourceTableUniqueID = eRPInspectionInformationDto.qapSourceTableUniqueID,
					CustomFields = eRPInspectionInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Inspections []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = inspectionDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPInspectionDto>> Process_PutInspection(ERPInspectionDto inspection)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPInspectionDto createdObject = null;
		ERPResponseMessageDto<ERPInspectionDto> result;
		try
		{
			IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
			using (iERPInspectionRepository)
			{
				APIValidationInfoDto postResult = await base.ERPInspectionRepository.SaveInspection(inspection);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPInspectionInformationDto eRPInspectionInformationDto = await base.ERPInspectionRepository.GetInspection(inspection.qapUniqueID);
					createdObject = new ERPInspectionDto
					{
						qapInspectionID = eRPInspectionInformationDto.qapInspectionID,
						qapCreatedBy = eRPInspectionInformationDto.qapCreatedBy,
						qapCreatedDate = eRPInspectionInformationDto.qapCreatedDate,
						qapUniqueID = eRPInspectionInformationDto.qapUniqueID,
						qapPosted = eRPInspectionInformationDto.qapPosted,
						qapReversalEntry = eRPInspectionInformationDto.qapReversalEntry,
						qapOpenedByEmployeeID = eRPInspectionInformationDto.qapOpenedByEmployeeID,
						qapOpenedDate = eRPInspectionInformationDto.qapOpenedDate,
						qapPlantDepartmentID = eRPInspectionInformationDto.qapPlantDepartmentID,
						qapPlantID = eRPInspectionInformationDto.qapPlantID,
						qapPostedDate = eRPInspectionInformationDto.qapPostedDate,
						qapProjectID = eRPInspectionInformationDto.qapProjectID,
						qapRowVersion = eRPInspectionInformationDto.qapRowVersion,
						qapSourceTableName = eRPInspectionInformationDto.qapSourceTableName,
						qapSourceTableUniqueID = eRPInspectionInformationDto.qapSourceTableUniqueID,
						CustomFields = eRPInspectionInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Inspection [{inspection.qapUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteInspection(Guid inspectionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
		using (iERPInspectionRepository)
		{
			if (!(await base.ERPInspectionRepository.DoesInspectionExist(inspectionId)))
			{
				base.ErrorsList.Add($"Inspection [{inspectionId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPInspectionInformationDto eRPInspectionInformationDto = await base.ERPInspectionRepository.GetInspection(inspectionId);
				string text = await base.ERPInspectionRepository.WhereUsed("Inspections", new object[1] { eRPInspectionInformationDto.qapInspectionID }, new object[1] { "qapInspectionID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Inspection cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPInspectionDto>> Process_DeleteInspection(Guid inspectionId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPInspectionDto> result;
		try
		{
			IERPInspectionRepository iERPInspectionRepository = (base.ERPInspectionRepository = new ERPInspectionRepository(base.ApiClientContext));
			using (iERPInspectionRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPInspectionRepository.DeleteRowFromTable("Inspections", "qap", inspectionId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Inspection [{inspectionId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPInspectionDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPInspectionDto()
			};
		}
		return result;
	}
}
