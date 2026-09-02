using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPToolModel : ERPBaseModel, IERPToolModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllTools(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
		using (iERPToolRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPToolRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPToolRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPToolRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPToolRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetTool(Guid toolId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
		using (iERPToolRepository)
		{
			if (!(await base.ERPToolRepository.DoesToolExist(toolId)))
			{
				errorsList.Add($"Tool [{toolId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutTool(ERPToolDto tool)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
		using (iERPToolRepository)
		{
			if (!string.IsNullOrWhiteSpace(tool.xttToolCategoryID) && !(await base.ERPToolRepository.DoesRecordExistInTableUsingKeys("ToolCategories", new object[1] { "xtcToolCategoryID" }, new object[1] { tool.xttToolCategoryID })))
			{
				errorsList.Add("xttToolCategoryID [" + tool.xttToolCategoryID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(tool.xttCheckoutReasonID) && !(await base.ERPToolRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { tool.xttCheckoutReasonID })))
			{
				errorsList.Add("xttCheckoutReasonID [" + tool.xttCheckoutReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(tool.xttCheckedOutToEmployeeID) && !(await base.ERPToolRepository.DoesRecordExistInTableUsingKeys("Employees", new object[1] { "LMEEMPLOYEEID" }, new object[1] { tool.xttCheckedOutToEmployeeID })))
			{
				errorsList.Add("xttCheckedOutToEmployeeID [" + tool.xttCheckedOutToEmployeeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(tool.xttWorkCenterID) && !(await base.ERPToolRepository.DoesRecordExistInTableUsingKeys("WorkCenters", new object[1] { "XAWWORKCENTERID" }, new object[1] { tool.xttWorkCenterID })))
			{
				errorsList.Add("xttWorkCenterID [" + tool.xttWorkCenterID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(tool.xttAssetID) && !(await base.ERPToolRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { tool.xttAssetID })))
			{
				errorsList.Add("xttAssetID [" + tool.xttAssetID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPToolDto>>> Process_GetAllTools(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPToolDto> allToolsDto = new List<ERPToolDto>();
		ERPResponseMessageDto<IList<ERPToolDto>> result;
		try
		{
			IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
			using (iERPToolRepository)
			{
				foreach (ERPToolInformationDto item2 in await base.ERPToolRepository.GetAllTools(pageSize, pageNumber, filter, orderBy))
				{
					ERPToolDto item = new ERPToolDto
					{
						xttAssetID = item2.xttAssetID,
						xttCheckedOutToEmployeeID = item2.xttCheckedOutToEmployeeID,
						xttCheckoutReasonID = item2.xttCheckoutReasonID,
						xttToolID = item2.xttToolID,
						xttCreatedBy = item2.xttCreatedBy,
						xttCreatedDate = item2.xttCreatedDate,
						xttDescription = item2.xttDescription,
						xttDocuments = item2.xttDocuments,
						xttUniqueID = item2.xttUniqueID,
						xttIdentificationNumber = item2.xttIdentificationNumber,
						xttInactiveDate = item2.xttInactiveDate,
						xttInactive = item2.xttInactive,
						xttLocation = item2.xttLocation,
						xttLongDescriptionRtf = item2.xttLongDescriptionRtf,
						xttLongDescriptionText = item2.xttLongDescriptionText,
						xttMovementDate = item2.xttMovementDate,
						xttMovementType = item2.xttMovementType,
						xttPlannedReturnDate = item2.xttPlannedReturnDate,
						xttRowVersion = item2.xttRowVersion,
						xttToolCategoryID = item2.xttToolCategoryID,
						xttWorkCenterID = item2.xttWorkCenterID,
						CustomFields = item2.CustomFields
					};
					allToolsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all Tools]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPToolDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allToolsDto,
				RecordCount = allToolsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolDto>> Process_GetTool(Guid toolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPToolDto toolDto = null;
		ERPResponseMessageDto<ERPToolDto> result;
		try
		{
			IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
			using (iERPToolRepository)
			{
				ERPToolInformationDto eRPToolInformationDto = await base.ERPToolRepository.GetTool(toolId);
				toolDto = new ERPToolDto
				{
					xttAssetID = eRPToolInformationDto.xttAssetID,
					xttCheckedOutToEmployeeID = eRPToolInformationDto.xttCheckedOutToEmployeeID,
					xttCheckoutReasonID = eRPToolInformationDto.xttCheckoutReasonID,
					xttToolID = eRPToolInformationDto.xttToolID,
					xttCreatedBy = eRPToolInformationDto.xttCreatedBy,
					xttCreatedDate = eRPToolInformationDto.xttCreatedDate,
					xttDescription = eRPToolInformationDto.xttDescription,
					xttDocuments = eRPToolInformationDto.xttDocuments,
					xttUniqueID = eRPToolInformationDto.xttUniqueID,
					xttIdentificationNumber = eRPToolInformationDto.xttIdentificationNumber,
					xttInactiveDate = eRPToolInformationDto.xttInactiveDate,
					xttInactive = eRPToolInformationDto.xttInactive,
					xttLocation = eRPToolInformationDto.xttLocation,
					xttLongDescriptionRtf = eRPToolInformationDto.xttLongDescriptionRtf,
					xttLongDescriptionText = eRPToolInformationDto.xttLongDescriptionText,
					xttMovementDate = eRPToolInformationDto.xttMovementDate,
					xttMovementType = eRPToolInformationDto.xttMovementType,
					xttPlannedReturnDate = eRPToolInformationDto.xttPlannedReturnDate,
					xttRowVersion = eRPToolInformationDto.xttRowVersion,
					xttToolCategoryID = eRPToolInformationDto.xttToolCategoryID,
					xttWorkCenterID = eRPToolInformationDto.xttWorkCenterID,
					CustomFields = eRPToolInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the Tools []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = toolDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPToolDto>> Process_PutTool(ERPToolDto tool)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPToolDto createdObject = null;
		ERPResponseMessageDto<ERPToolDto> result;
		try
		{
			IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
			using (iERPToolRepository)
			{
				APIValidationInfoDto postResult = await base.ERPToolRepository.SaveTool(tool);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPToolInformationDto eRPToolInformationDto = await base.ERPToolRepository.GetTool(tool.xttUniqueID);
					createdObject = new ERPToolDto
					{
						xttAssetID = eRPToolInformationDto.xttAssetID,
						xttCheckedOutToEmployeeID = eRPToolInformationDto.xttCheckedOutToEmployeeID,
						xttCheckoutReasonID = eRPToolInformationDto.xttCheckoutReasonID,
						xttToolID = eRPToolInformationDto.xttToolID,
						xttCreatedBy = eRPToolInformationDto.xttCreatedBy,
						xttCreatedDate = eRPToolInformationDto.xttCreatedDate,
						xttDescription = eRPToolInformationDto.xttDescription,
						xttDocuments = eRPToolInformationDto.xttDocuments,
						xttUniqueID = eRPToolInformationDto.xttUniqueID,
						xttIdentificationNumber = eRPToolInformationDto.xttIdentificationNumber,
						xttInactiveDate = eRPToolInformationDto.xttInactiveDate,
						xttInactive = eRPToolInformationDto.xttInactive,
						xttLocation = eRPToolInformationDto.xttLocation,
						xttLongDescriptionRtf = eRPToolInformationDto.xttLongDescriptionRtf,
						xttLongDescriptionText = eRPToolInformationDto.xttLongDescriptionText,
						xttMovementDate = eRPToolInformationDto.xttMovementDate,
						xttMovementType = eRPToolInformationDto.xttMovementType,
						xttPlannedReturnDate = eRPToolInformationDto.xttPlannedReturnDate,
						xttRowVersion = eRPToolInformationDto.xttRowVersion,
						xttToolCategoryID = eRPToolInformationDto.xttToolCategoryID,
						xttWorkCenterID = eRPToolInformationDto.xttWorkCenterID,
						CustomFields = eRPToolInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing Tool [{tool.xttUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteTool(Guid toolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
		using (iERPToolRepository)
		{
			if (!(await base.ERPToolRepository.DoesToolExist(toolId)))
			{
				base.ErrorsList.Add($"Tool [{toolId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPToolInformationDto eRPToolInformationDto = await base.ERPToolRepository.GetTool(toolId);
				string text = await base.ERPToolRepository.WhereUsed("Tools", new object[1] { eRPToolInformationDto.xttToolID }, new object[1] { "xttToolID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("Tool cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPToolDto>> Process_DeleteTool(Guid toolId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPToolDto> result;
		try
		{
			IERPToolRepository iERPToolRepository = (base.ERPToolRepository = new ERPToolRepository(base.ApiClientContext));
			using (iERPToolRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPToolRepository.DeleteRowFromTable("Tools", "xtt", toolId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of Tool [{toolId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPToolDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPToolDto()
			};
		}
		return result;
	}
}
