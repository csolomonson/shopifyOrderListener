using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPCustomTableModel : ERPBaseModel, IERPCustomTableModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllCustomTableRecords(string tableName, int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
		using (iERPCustomTableRepository)
		{
			if (!base.ERPCustomTableRepository.DoesCustomTableExist(tableName))
			{
				list.Add("Table passed " + tableName + " does not exist as a custom table.");
			}
			string customTablePrefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
			if (string.IsNullOrEmpty(customTablePrefix))
			{
				list.Add("Table passed has no prefix defined.");
			}
			else if (!base.ERPCustomTableRepository.CheckCustomTableForUniqueIDField(tableName, customTablePrefix))
			{
				list.Add("Table passed " + tableName + " does not have a UniqueID field defined.");
			}
			if (filter != null && filter.Length != 0 && !base.ERPCustomTableRepository.ValidateFilterClause(filter))
			{
				list.Add($"Filter clause passed {filter} is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPCustomTableRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed " + orderBy + " is invalid.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetCustomTableRecord(string tableName, Guid customTableUniqueId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
		using (iERPCustomTableRepository)
		{
			if (!base.ERPCustomTableRepository.DoesCustomTableExist(tableName))
			{
				errorsList.Add("CustomTable [" + tableName + "] not found.");
			}
			string customTablePrefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
			if (string.IsNullOrEmpty(customTablePrefix))
			{
				errorsList.Add("Table passed has no prefix defined.");
			}
			else if (!base.ERPCustomTableRepository.CheckCustomTableForUniqueIDField(tableName, customTablePrefix))
			{
				errorsList.Add("Table passed " + tableName + " does not have a UniqueID field defined.");
			}
			else if (!(await base.ERPCustomTableRepository.DoesCustomTableRecordExist(tableName, customTableUniqueId)))
			{
				errorsList.Add($"CustomTable [{tableName}] record with Unique ID [{customTableUniqueId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
		}
		if (errorsList != null && errorsList.Count > 0 && httpStatus != HttpStatusCode.NotFound)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutCustomTableRecord(string tableName, ERPCustomTableDto customTableObject)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
		using (iERPCustomTableRepository)
		{
			if (!base.ERPCustomTableRepository.DoesCustomTableExist(tableName))
			{
				list.Add("CustomTable [" + tableName + "] not found.");
			}
			string customTablePrefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
			if (string.IsNullOrEmpty(customTablePrefix))
			{
				list.Add("Table passed has no prefix defined.");
			}
			else if (!base.ERPCustomTableRepository.CheckCustomTableForUniqueIDField(tableName, customTablePrefix))
			{
				list.Add("Table passed " + tableName + " does not have a UniqueID field defined.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<ERPResponseMessageDto<IList<ERPCustomTableDto>>> Process_GetAllCustomTableRecords(string tableName, int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPCustomTableDto> allCustomTablesDto = new List<ERPCustomTableDto>();
		ERPResponseMessageDto<IList<ERPCustomTableDto>> result;
		try
		{
			IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
			using (iERPCustomTableRepository)
			{
				foreach (ERPCustomTableInformationDto item2 in await base.ERPCustomTableRepository.GetAllCustomTableRecords(tableName, pageSize, pageNumber, filter, orderBy))
				{
					ERPCustomTableDto item = new ERPCustomTableDto
					{
						CustomFields = item2.CustomFields
					};
					allCustomTablesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all CustomTable records.");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPCustomTableDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allCustomTablesDto,
				RecordCount = allCustomTablesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_GetCustomTableRecord(string tableName, Guid customTableUniqueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPCustomTableDto customTableDto = null;
		ERPResponseMessageDto<ERPCustomTableDto> result;
		try
		{
			IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
			using (iERPCustomTableRepository)
			{
				ERPCustomTableInformationDto eRPCustomTableInformationDto = await base.ERPCustomTableRepository.GetCustomTableRecord(tableName, customTableUniqueId);
				customTableDto = new ERPCustomTableDto
				{
					CustomFields = eRPCustomTableInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the CustomTable [" + tableName + "].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomTableDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = customTableDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_PutCustomTableRecord(string tableName, ERPCustomTableDto customTable)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPCustomTableDto createdObject = null;
		string customTablePrefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
		if (string.IsNullOrEmpty(customTablePrefix))
		{
			httpStatus = HttpStatusCode.BadRequest;
			return new ERPResponseMessageDto<ERPCustomTableDto>
			{
				ValidationInfo = 
				{
					HttpValidationStatusCode = httpStatus
				}
			};
		}
		if (!customTable.CustomFields.TryGetValue(customTablePrefix + "UniqueID", out var value) || !Guid.TryParse(value.ToString(), out var uniqueIDGuidResult))
		{
			httpStatus = HttpStatusCode.BadRequest;
			base.ErrorsList.Add("Cannot parse unique id for CustomTable [" + tableName + "].");
			return new ERPResponseMessageDto<ERPCustomTableDto>
			{
				ValidationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus)
			};
		}
		ERPResponseMessageDto<ERPCustomTableDto> result;
		try
		{
			IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
			using (iERPCustomTableRepository)
			{
				APIValidationInfoDto postResult = await base.ERPCustomTableRepository.SaveCustomTableRecord(tableName, customTable);
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPCustomTableInformationDto eRPCustomTableInformationDto = await base.ERPCustomTableRepository.GetCustomTableRecord(tableName, uniqueIDGuidResult);
					createdObject = new ERPCustomTableDto
					{
						CustomFields = eRPCustomTableInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing CustomTable [{tableName}] record for Unique ID [{uniqueIDGuidResult}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomTableDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteCustomTableRecord(string tableName, Guid customTableUniqueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
		using (iERPCustomTableRepository)
		{
			if (!base.ERPCustomTableRepository.DoesCustomTableExist(tableName))
			{
				base.ErrorsList.Add("CustomTable [" + tableName + "] not found.");
			}
			string prefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
			if (string.IsNullOrEmpty(prefix))
			{
				base.ErrorsList.Add("Table passed has no prefix defined.");
			}
			else if (!base.ERPCustomTableRepository.CheckCustomTableForUniqueIDField(tableName, prefix))
			{
				base.ErrorsList.Add("Table passed " + tableName + " does not have a UniqueID field defined.");
			}
			else if (!(await base.ERPCustomTableRepository.DoesCustomTableRecordExist(tableName, customTableUniqueId)))
			{
				base.ErrorsList.Add($"CustomTable [{tableName}] record with Unique ID [{customTableUniqueId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				string text = await base.ERPCustomTableRepository.WhereUsed(tableName, new object[1] { customTableUniqueId }, new object[1] { prefix + "UniqueID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("CustomTable record cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
				}
			}
		}
		IList<string> errorsList = base.ErrorsList;
		if (errorsList != null && errorsList.Count > 0 && httpStatus != HttpStatusCode.NotFound)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPCustomTableDto>> Process_DeleteCustomTableRecord(string tableName, Guid customTableUniqueId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPCustomTableDto> result;
		try
		{
			IERPCustomTableRepository iERPCustomTableRepository = (base.ERPCustomTableRepository = new ERPCustomTableRepository(base.ApiClientContext));
			using (iERPCustomTableRepository)
			{
				string customTablePrefix = base.ERPCustomTableRepository.GetCustomTablePrefix(tableName);
				if (string.IsNullOrEmpty(customTablePrefix))
				{
					httpStatus = HttpStatusCode.BadRequest;
					return new ERPResponseMessageDto<ERPCustomTableDto>
					{
						ValidationInfo = 
						{
							HttpValidationStatusCode = httpStatus
						}
					};
				}
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPCustomTableRepository.DeleteRowFromTable(tableName, customTablePrefix, customTableUniqueId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of CustomTable [{tableName}] with Unique ID [{customTableUniqueId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPCustomTableDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPCustomTableDto()
			};
		}
		return result;
	}
}
