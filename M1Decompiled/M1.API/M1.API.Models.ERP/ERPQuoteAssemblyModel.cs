using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteAssemblyModel : ERPBaseModel, IERPQuoteAssemblyModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
		using (iERPQuoteAssemblyRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteAssemblyRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteAssemblyRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteAssemblyRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteAssemblyRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteAssembly(Guid quoteAssemblyId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
		using (iERPQuoteAssemblyRepository)
		{
			if (!(await base.ERPQuoteAssemblyRepository.DoesQuoteAssemblyExist(quoteAssemblyId)))
			{
				errorsList.Add($"QuoteAssembly [{quoteAssemblyId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
		using (iERPQuoteAssemblyRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteAssembly.qmaQuoteID) && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteAssembly.qmaQuoteID })))
			{
				errorsList.Add("qmaQuoteID [" + quoteAssembly.qmaQuoteID + "] not found.");
			}
			if (quoteAssembly.qmaQuoteLineID > 0 && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { quoteAssembly.qmaQuoteID, quoteAssembly.qmaQuoteLineID })))
			{
				errorsList.Add($"qmaQuoteLineID [{quoteAssembly.qmaQuoteLineID}] not found.");
			}
			if (quoteAssembly.qmaParentAssemblyID > 0 && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("QuoteAssemblies", new object[3] { "QMAQUOTEID", "QMAQUOTELINEID", "QMAQUOTEASSEMBLYID" }, new object[3] { quoteAssembly.qmaQuoteID, quoteAssembly.qmaQuoteLineID, quoteAssembly.qmaParentAssemblyID })))
			{
				errorsList.Add($"qmaParentAssemblyID [{quoteAssembly.qmaParentAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteAssembly.qmaSourceMethodID) && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteAssembly.qmaSourceMethodID })))
			{
				errorsList.Add("qmaSourceMethodID [" + quoteAssembly.qmaSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteAssembly.qmaSourceRevisionID) && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteAssembly.qmaSourceMethodID, quoteAssembly.qmaSourceRevisionID })))
			{
				errorsList.Add("qmaSourceRevisionID [" + quoteAssembly.qmaSourceRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteAssembly.qmaPartID) && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteAssembly.qmaPartID })))
			{
				errorsList.Add("qmaPartID [" + quoteAssembly.qmaPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteAssembly.qmaPartRevisionID) && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteAssembly.qmaPartID, quoteAssembly.qmaPartRevisionID })))
			{
				errorsList.Add("qmaPartRevisionID [" + quoteAssembly.qmaPartRevisionID + "] not found.");
			}
			if (quoteAssembly.qmaOverlapOperationID > 0 && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("QuoteOperations", new object[4] { "QMOQUOTEID", "QMOQUOTELINEID", "QMOQUOTEASSEMBLYID", "QMOQUOTEOPERATIONID" }, new object[4] { quoteAssembly.qmaQuoteID, quoteAssembly.qmaQuoteLineID, quoteAssembly.qmaParentAssemblyID, quoteAssembly.qmaOverlapOperationID })))
			{
				errorsList.Add($"qmaOverlapOperationID [{quoteAssembly.qmaOverlapOperationID}] not found.");
			}
			if (quoteAssembly.qmaOverlapSourceOperationID > 0 && !(await base.ERPQuoteAssemblyRepository.DoesRecordExistInTableUsingKeys("QuoteOperations", new object[4] { "QMOQUOTEID", "QMOQUOTELINEID", "QMOQUOTEASSEMBLYID", "QMOQUOTEOPERATIONID" }, new object[4] { quoteAssembly.qmaQuoteID, quoteAssembly.qmaQuoteLineID, quoteAssembly.qmaQuoteAssemblyID, quoteAssembly.qmaOverlapSourceOperationID })))
			{
				errorsList.Add($"qmaOverlapSourceOperationID [{quoteAssembly.qmaOverlapSourceOperationID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteAssemblyDto>>> Process_GetAllQuoteAssemblies(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteAssemblyDto> allQuoteAssembliesDto = new List<ERPQuoteAssemblyDto>();
		ERPResponseMessageDto<IList<ERPQuoteAssemblyDto>> result;
		try
		{
			IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
			using (iERPQuoteAssemblyRepository)
			{
				foreach (ERPQuoteAssemblyInformationDto item2 in await base.ERPQuoteAssemblyRepository.GetAllQuoteAssemblies(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteAssemblyDto item = new ERPQuoteAssemblyDto
					{
						qmaAssemblyOverlap = item2.qmaAssemblyOverlap,
						qmaCreatedBy = item2.qmaCreatedBy,
						qmaCreatedDate = item2.qmaCreatedDate,
						qmaDocuments = item2.qmaDocuments,
						qmaUniqueID = item2.qmaUniqueID,
						qmaClosed = item2.qmaClosed,
						qmaPullAllFromStock = item2.qmaPullAllFromStock,
						qmaLevel = item2.qmaLevel,
						qmaOverlapDestinationLink = item2.qmaOverlapDestinationLink,
						qmaOverlapOffsetTime = item2.qmaOverlapOffsetTime,
						qmaOverlapOperationID = item2.qmaOverlapOperationID,
						qmaOverlapSourceLink = item2.qmaOverlapSourceLink,
						qmaOverlapSourceOperationID = item2.qmaOverlapSourceOperationID,
						qmaOverlapType = item2.qmaOverlapType,
						qmaParentAssemblyID = item2.qmaParentAssemblyID,
						qmaPartID = item2.qmaPartID,
						qmaPartLongDescriptionRtf = item2.qmaPartLongDescriptionRtf,
						qmaPartLongDescriptionText = item2.qmaPartLongDescriptionText,
						qmaPartRevisionID = item2.qmaPartRevisionID,
						qmaPartShortDescription = item2.qmaPartShortDescription,
						qmaProductionNotesRTF = item2.qmaProductionNotesRTF,
						qmaProductionNotesText = item2.qmaProductionNotesText,
						qmaQuantityPerParent = item2.qmaQuantityPerParent,
						qmaQuoteID = item2.qmaQuoteID,
						qmaQuoteLineID = item2.qmaQuoteLineID,
						qmaRowVersion = item2.qmaRowVersion,
						qmaQuoteAssemblyID = item2.qmaQuoteAssemblyID,
						qmaSourceMethodID = item2.qmaSourceMethodID,
						qmaSourceRevisionID = item2.qmaSourceRevisionID,
						qmaUnitOfMeasure = item2.qmaUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allQuoteAssembliesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteAssemblies]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteAssemblyDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteAssembliesDto,
				RecordCount = allQuoteAssembliesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_GetQuoteAssembly(Guid quoteAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteAssemblyDto quoteAssemblyDto = null;
		ERPResponseMessageDto<ERPQuoteAssemblyDto> result;
		try
		{
			IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
			using (iERPQuoteAssemblyRepository)
			{
				ERPQuoteAssemblyInformationDto eRPQuoteAssemblyInformationDto = await base.ERPQuoteAssemblyRepository.GetQuoteAssembly(quoteAssemblyId);
				quoteAssemblyDto = new ERPQuoteAssemblyDto
				{
					qmaAssemblyOverlap = eRPQuoteAssemblyInformationDto.qmaAssemblyOverlap,
					qmaCreatedBy = eRPQuoteAssemblyInformationDto.qmaCreatedBy,
					qmaCreatedDate = eRPQuoteAssemblyInformationDto.qmaCreatedDate,
					qmaDocuments = eRPQuoteAssemblyInformationDto.qmaDocuments,
					qmaUniqueID = eRPQuoteAssemblyInformationDto.qmaUniqueID,
					qmaClosed = eRPQuoteAssemblyInformationDto.qmaClosed,
					qmaPullAllFromStock = eRPQuoteAssemblyInformationDto.qmaPullAllFromStock,
					qmaLevel = eRPQuoteAssemblyInformationDto.qmaLevel,
					qmaOverlapDestinationLink = eRPQuoteAssemblyInformationDto.qmaOverlapDestinationLink,
					qmaOverlapOffsetTime = eRPQuoteAssemblyInformationDto.qmaOverlapOffsetTime,
					qmaOverlapOperationID = eRPQuoteAssemblyInformationDto.qmaOverlapOperationID,
					qmaOverlapSourceLink = eRPQuoteAssemblyInformationDto.qmaOverlapSourceLink,
					qmaOverlapSourceOperationID = eRPQuoteAssemblyInformationDto.qmaOverlapSourceOperationID,
					qmaOverlapType = eRPQuoteAssemblyInformationDto.qmaOverlapType,
					qmaParentAssemblyID = eRPQuoteAssemblyInformationDto.qmaParentAssemblyID,
					qmaPartID = eRPQuoteAssemblyInformationDto.qmaPartID,
					qmaPartLongDescriptionRtf = eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionRtf,
					qmaPartLongDescriptionText = eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionText,
					qmaPartRevisionID = eRPQuoteAssemblyInformationDto.qmaPartRevisionID,
					qmaPartShortDescription = eRPQuoteAssemblyInformationDto.qmaPartShortDescription,
					qmaProductionNotesRTF = eRPQuoteAssemblyInformationDto.qmaProductionNotesRTF,
					qmaProductionNotesText = eRPQuoteAssemblyInformationDto.qmaProductionNotesText,
					qmaQuantityPerParent = eRPQuoteAssemblyInformationDto.qmaQuantityPerParent,
					qmaQuoteID = eRPQuoteAssemblyInformationDto.qmaQuoteID,
					qmaQuoteLineID = eRPQuoteAssemblyInformationDto.qmaQuoteLineID,
					qmaRowVersion = eRPQuoteAssemblyInformationDto.qmaRowVersion,
					qmaQuoteAssemblyID = eRPQuoteAssemblyInformationDto.qmaQuoteAssemblyID,
					qmaSourceMethodID = eRPQuoteAssemblyInformationDto.qmaSourceMethodID,
					qmaSourceRevisionID = eRPQuoteAssemblyInformationDto.qmaSourceRevisionID,
					qmaUnitOfMeasure = eRPQuoteAssemblyInformationDto.qmaUnitOfMeasure,
					CustomFields = eRPQuoteAssemblyInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteAssemblies []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteAssemblyDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_PutQuoteAssembly(ERPQuoteAssemblyDto quoteAssembly)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteAssemblyDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteAssemblyDto> result;
		try
		{
			IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
			using (iERPQuoteAssemblyRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteAssemblyRepository.SaveQuoteAssembly(quoteAssembly);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteAssemblyInformationDto eRPQuoteAssemblyInformationDto = await base.ERPQuoteAssemblyRepository.GetQuoteAssembly(quoteAssembly.qmaUniqueID);
					createdObject = new ERPQuoteAssemblyDto
					{
						qmaAssemblyOverlap = eRPQuoteAssemblyInformationDto.qmaAssemblyOverlap,
						qmaCreatedBy = eRPQuoteAssemblyInformationDto.qmaCreatedBy,
						qmaCreatedDate = eRPQuoteAssemblyInformationDto.qmaCreatedDate,
						qmaDocuments = eRPQuoteAssemblyInformationDto.qmaDocuments,
						qmaUniqueID = eRPQuoteAssemblyInformationDto.qmaUniqueID,
						qmaClosed = eRPQuoteAssemblyInformationDto.qmaClosed,
						qmaPullAllFromStock = eRPQuoteAssemblyInformationDto.qmaPullAllFromStock,
						qmaLevel = eRPQuoteAssemblyInformationDto.qmaLevel,
						qmaOverlapDestinationLink = eRPQuoteAssemblyInformationDto.qmaOverlapDestinationLink,
						qmaOverlapOffsetTime = eRPQuoteAssemblyInformationDto.qmaOverlapOffsetTime,
						qmaOverlapOperationID = eRPQuoteAssemblyInformationDto.qmaOverlapOperationID,
						qmaOverlapSourceLink = eRPQuoteAssemblyInformationDto.qmaOverlapSourceLink,
						qmaOverlapSourceOperationID = eRPQuoteAssemblyInformationDto.qmaOverlapSourceOperationID,
						qmaOverlapType = eRPQuoteAssemblyInformationDto.qmaOverlapType,
						qmaParentAssemblyID = eRPQuoteAssemblyInformationDto.qmaParentAssemblyID,
						qmaPartID = eRPQuoteAssemblyInformationDto.qmaPartID,
						qmaPartLongDescriptionRtf = eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionRtf,
						qmaPartLongDescriptionText = eRPQuoteAssemblyInformationDto.qmaPartLongDescriptionText,
						qmaPartRevisionID = eRPQuoteAssemblyInformationDto.qmaPartRevisionID,
						qmaPartShortDescription = eRPQuoteAssemblyInformationDto.qmaPartShortDescription,
						qmaProductionNotesRTF = eRPQuoteAssemblyInformationDto.qmaProductionNotesRTF,
						qmaProductionNotesText = eRPQuoteAssemblyInformationDto.qmaProductionNotesText,
						qmaQuantityPerParent = eRPQuoteAssemblyInformationDto.qmaQuantityPerParent,
						qmaQuoteID = eRPQuoteAssemblyInformationDto.qmaQuoteID,
						qmaQuoteLineID = eRPQuoteAssemblyInformationDto.qmaQuoteLineID,
						qmaRowVersion = eRPQuoteAssemblyInformationDto.qmaRowVersion,
						qmaQuoteAssemblyID = eRPQuoteAssemblyInformationDto.qmaQuoteAssemblyID,
						qmaSourceMethodID = eRPQuoteAssemblyInformationDto.qmaSourceMethodID,
						qmaSourceRevisionID = eRPQuoteAssemblyInformationDto.qmaSourceRevisionID,
						qmaUnitOfMeasure = eRPQuoteAssemblyInformationDto.qmaUnitOfMeasure,
						CustomFields = eRPQuoteAssemblyInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteAssembly [{quoteAssembly.qmaUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteAssembly(Guid quoteAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
		using (iERPQuoteAssemblyRepository)
		{
			if (!(await base.ERPQuoteAssemblyRepository.DoesQuoteAssemblyExist(quoteAssemblyId)))
			{
				base.ErrorsList.Add($"QuoteAssembly [{quoteAssemblyId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteAssemblyInformationDto eRPQuoteAssemblyInformationDto = await base.ERPQuoteAssemblyRepository.GetQuoteAssembly(quoteAssemblyId);
				string text = await base.ERPQuoteAssemblyRepository.WhereUsed("QuoteAssemblies", new object[3] { eRPQuoteAssemblyInformationDto.qmaQuoteID, eRPQuoteAssemblyInformationDto.qmaQuoteLineID, eRPQuoteAssemblyInformationDto.qmaQuoteAssemblyID }, new object[3] { "qmaQuoteID", "qmaQuoteLineID", "qmaQuoteAssemblyID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteAssembly cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteAssemblyDto>> Process_DeleteQuoteAssembly(Guid quoteAssemblyId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteAssemblyDto> result;
		try
		{
			IERPQuoteAssemblyRepository iERPQuoteAssemblyRepository = (base.ERPQuoteAssemblyRepository = new ERPQuoteAssemblyRepository(base.ApiClientContext));
			using (iERPQuoteAssemblyRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteAssemblyRepository.DeleteRowFromTable("QuoteAssemblies", "qma", quoteAssemblyId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteAssembly [{quoteAssemblyId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteAssemblyDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteAssemblyDto()
			};
		}
		return result;
	}
}
