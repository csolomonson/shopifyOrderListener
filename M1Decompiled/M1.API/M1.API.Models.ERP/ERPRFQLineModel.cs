using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPRFQLineModel : ERPBaseModel, IERPRFQLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllRFQLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
		using (iERPRFQLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPRFQLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPRFQLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPRFQLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPRFQLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetRFQLine(Guid rFQLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
		using (iERPRFQLineRepository)
		{
			if (!(await base.ERPRFQLineRepository.DoesRFQLineExist(rFQLineId)))
			{
				errorsList.Add($"RFQLine [{rFQLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutRFQLine(ERPRFQLineDto rFQLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
		using (iERPRFQLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlRfqID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("RFQs", new object[1] { "RQPRFQID" }, new object[1] { rFQLine.rqlRfqID })))
			{
				errorsList.Add("rqlRfqID [" + rFQLine.rqlRfqID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlPartID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { rFQLine.rqlPartID })))
			{
				errorsList.Add("rqlPartID [" + rFQLine.rqlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlPartRevisionID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { rFQLine.rqlPartID, rFQLine.rqlPartRevisionID })))
			{
				errorsList.Add("rqlPartRevisionID [" + rFQLine.rqlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlQuoteID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { rFQLine.rqlQuoteID })))
			{
				errorsList.Add("rqlQuoteID [" + rFQLine.rqlQuoteID + "] not found.");
			}
			if (rFQLine.rqlQuoteLineID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("QuoteLines", new object[2] { "QMLQUOTEID", "QMLQUOTELINEID" }, new object[2] { rFQLine.rqlQuoteID, rFQLine.rqlQuoteLineID })))
			{
				errorsList.Add($"rqlQuoteLineID [{rFQLine.rqlQuoteLineID}] not found.");
			}
			if (rFQLine.rqlQuoteAssemblyID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("QuoteAssemblies", new object[3] { "QMAQUOTEID", "QMAQUOTELINEID", "QMAQUOTEASSEMBLYID" }, new object[3] { rFQLine.rqlQuoteID, rFQLine.rqlQuoteLineID, rFQLine.rqlQuoteAssemblyID })))
			{
				errorsList.Add($"rqlQuoteAssemblyID [{rFQLine.rqlQuoteAssemblyID}] not found.");
			}
			if (rFQLine.rqlQuoteMaterialID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("QuoteMaterials", new object[4] { "QMMQUOTEID", "QMMQUOTELINEID", "QMMQUOTEASSEMBLYID", "QMMQUOTEMATERIALID" }, new object[4] { rFQLine.rqlQuoteID, rFQLine.rqlQuoteLineID, rFQLine.rqlQuoteAssemblyID, rFQLine.rqlQuoteMaterialID })))
			{
				errorsList.Add($"rqlQuoteMaterialID [{rFQLine.rqlQuoteMaterialID}] not found.");
			}
			if (rFQLine.rqlQuoteOperationID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("QuoteOperations", new object[4] { "QMOQUOTEID", "QMOQUOTELINEID", "QMOQUOTEASSEMBLYID", "QMOQUOTEOPERATIONID" }, new object[4] { rFQLine.rqlQuoteID, rFQLine.rqlQuoteLineID, rFQLine.rqlQuoteAssemblyID, rFQLine.rqlQuoteOperationID })))
			{
				errorsList.Add($"rqlQuoteOperationID [{rFQLine.rqlQuoteOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlJobID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { rFQLine.rqlJobID })))
			{
				errorsList.Add("rqlJobID [" + rFQLine.rqlJobID + "] not found.");
			}
			if (rFQLine.rqlJobAssemblyID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { rFQLine.rqlJobID, rFQLine.rqlJobAssemblyID })))
			{
				errorsList.Add($"rqlJobAssemblyID [{rFQLine.rqlJobAssemblyID}] not found.");
			}
			if (rFQLine.rqlJobMaterialID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { rFQLine.rqlJobID, rFQLine.rqlJobAssemblyID, rFQLine.rqlJobMaterialID })))
			{
				errorsList.Add($"rqlJobMaterialID [{rFQLine.rqlJobMaterialID}] not found.");
			}
			if (rFQLine.rqlJobOperationID > 0 && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { rFQLine.rqlJobID, rFQLine.rqlJobAssemblyID, rFQLine.rqlJobOperationID })))
			{
				errorsList.Add($"rqlJobOperationID [{rFQLine.rqlJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlProjectID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { rFQLine.rqlProjectID })))
			{
				errorsList.Add("rqlProjectID [" + rFQLine.rqlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(rFQLine.rqlProjectAreaID) && !(await base.ERPRFQLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { rFQLine.rqlProjectID, rFQLine.rqlProjectAreaID })))
			{
				errorsList.Add("rqlProjectAreaID [" + rFQLine.rqlProjectAreaID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPRFQLineDto>>> Process_GetAllRFQLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPRFQLineDto> allRFQLinesDto = new List<ERPRFQLineDto>();
		ERPResponseMessageDto<IList<ERPRFQLineDto>> result;
		try
		{
			IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
			using (iERPRFQLineRepository)
			{
				foreach (ERPRFQLineInformationDto item2 in await base.ERPRFQLineRepository.GetAllRFQLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPRFQLineDto item = new ERPRFQLineDto
					{
						rqlCreatedBy = item2.rqlCreatedBy,
						rqlCreatedDate = item2.rqlCreatedDate,
						rqlDocuments = item2.rqlDocuments,
						rqlUniqueID = item2.rqlUniqueID,
						rqlInventoryUnitOfMeasure = item2.rqlInventoryUnitOfMeasure,
						rqlAlternatePart = item2.rqlAlternatePart,
						rqlClosed = item2.rqlClosed,
						rqlJobAssemblyID = item2.rqlJobAssemblyID,
						rqlJobEstimatedQty = item2.rqlJobEstimatedQty,
						rqlJobID = item2.rqlJobID,
						rqlJobMaterialID = item2.rqlJobMaterialID,
						rqlJobOperationID = item2.rqlJobOperationID,
						rqlPartID = item2.rqlPartID,
						rqlPartLongDescriptionRtf = item2.rqlPartLongDescriptionRtf,
						rqlPartLongDescriptionText = item2.rqlPartLongDescriptionText,
						rqlPartRevisionID = item2.rqlPartRevisionID,
						rqlPartShortDescription = item2.rqlPartShortDescription,
						rqlProjectAreaID = item2.rqlProjectAreaID,
						rqlProjectID = item2.rqlProjectID,
						rqlPurchaseUnitOfMeasure = item2.rqlPurchaseUnitOfMeasure,
						rqlQuoteAssemblyID = item2.rqlQuoteAssemblyID,
						rqlQuoteID = item2.rqlQuoteID,
						rqlQuoteLineID = item2.rqlQuoteLineID,
						rqlQuoteMaterialID = item2.rqlQuoteMaterialID,
						rqlQuoteOperationID = item2.rqlQuoteOperationID,
						rqlRfqID = item2.rqlRfqID,
						rqlRfqType = item2.rqlRfqType,
						rqlRowVersion = item2.rqlRowVersion,
						rqlRfqLineID = item2.rqlRfqLineID,
						CustomFields = item2.CustomFields
					};
					allRFQLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all RFQLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPRFQLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allRFQLinesDto,
				RecordCount = allRFQLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_GetRFQLine(Guid rFQLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPRFQLineDto rFQLineDto = null;
		ERPResponseMessageDto<ERPRFQLineDto> result;
		try
		{
			IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
			using (iERPRFQLineRepository)
			{
				ERPRFQLineInformationDto eRPRFQLineInformationDto = await base.ERPRFQLineRepository.GetRFQLine(rFQLineId);
				rFQLineDto = new ERPRFQLineDto
				{
					rqlCreatedBy = eRPRFQLineInformationDto.rqlCreatedBy,
					rqlCreatedDate = eRPRFQLineInformationDto.rqlCreatedDate,
					rqlDocuments = eRPRFQLineInformationDto.rqlDocuments,
					rqlUniqueID = eRPRFQLineInformationDto.rqlUniqueID,
					rqlInventoryUnitOfMeasure = eRPRFQLineInformationDto.rqlInventoryUnitOfMeasure,
					rqlAlternatePart = eRPRFQLineInformationDto.rqlAlternatePart,
					rqlClosed = eRPRFQLineInformationDto.rqlClosed,
					rqlJobAssemblyID = eRPRFQLineInformationDto.rqlJobAssemblyID,
					rqlJobEstimatedQty = eRPRFQLineInformationDto.rqlJobEstimatedQty,
					rqlJobID = eRPRFQLineInformationDto.rqlJobID,
					rqlJobMaterialID = eRPRFQLineInformationDto.rqlJobMaterialID,
					rqlJobOperationID = eRPRFQLineInformationDto.rqlJobOperationID,
					rqlPartID = eRPRFQLineInformationDto.rqlPartID,
					rqlPartLongDescriptionRtf = eRPRFQLineInformationDto.rqlPartLongDescriptionRtf,
					rqlPartLongDescriptionText = eRPRFQLineInformationDto.rqlPartLongDescriptionText,
					rqlPartRevisionID = eRPRFQLineInformationDto.rqlPartRevisionID,
					rqlPartShortDescription = eRPRFQLineInformationDto.rqlPartShortDescription,
					rqlProjectAreaID = eRPRFQLineInformationDto.rqlProjectAreaID,
					rqlProjectID = eRPRFQLineInformationDto.rqlProjectID,
					rqlPurchaseUnitOfMeasure = eRPRFQLineInformationDto.rqlPurchaseUnitOfMeasure,
					rqlQuoteAssemblyID = eRPRFQLineInformationDto.rqlQuoteAssemblyID,
					rqlQuoteID = eRPRFQLineInformationDto.rqlQuoteID,
					rqlQuoteLineID = eRPRFQLineInformationDto.rqlQuoteLineID,
					rqlQuoteMaterialID = eRPRFQLineInformationDto.rqlQuoteMaterialID,
					rqlQuoteOperationID = eRPRFQLineInformationDto.rqlQuoteOperationID,
					rqlRfqID = eRPRFQLineInformationDto.rqlRfqID,
					rqlRfqType = eRPRFQLineInformationDto.rqlRfqType,
					rqlRowVersion = eRPRFQLineInformationDto.rqlRowVersion,
					rqlRfqLineID = eRPRFQLineInformationDto.rqlRfqLineID,
					CustomFields = eRPRFQLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the RFQLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = rFQLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_PutRFQLine(ERPRFQLineDto rFQLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPRFQLineDto createdObject = null;
		ERPResponseMessageDto<ERPRFQLineDto> result;
		try
		{
			IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
			using (iERPRFQLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPRFQLineRepository.SaveRFQLine(rFQLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPRFQLineInformationDto eRPRFQLineInformationDto = await base.ERPRFQLineRepository.GetRFQLine(rFQLine.rqlUniqueID);
					createdObject = new ERPRFQLineDto
					{
						rqlCreatedBy = eRPRFQLineInformationDto.rqlCreatedBy,
						rqlCreatedDate = eRPRFQLineInformationDto.rqlCreatedDate,
						rqlDocuments = eRPRFQLineInformationDto.rqlDocuments,
						rqlUniqueID = eRPRFQLineInformationDto.rqlUniqueID,
						rqlInventoryUnitOfMeasure = eRPRFQLineInformationDto.rqlInventoryUnitOfMeasure,
						rqlAlternatePart = eRPRFQLineInformationDto.rqlAlternatePart,
						rqlClosed = eRPRFQLineInformationDto.rqlClosed,
						rqlJobAssemblyID = eRPRFQLineInformationDto.rqlJobAssemblyID,
						rqlJobEstimatedQty = eRPRFQLineInformationDto.rqlJobEstimatedQty,
						rqlJobID = eRPRFQLineInformationDto.rqlJobID,
						rqlJobMaterialID = eRPRFQLineInformationDto.rqlJobMaterialID,
						rqlJobOperationID = eRPRFQLineInformationDto.rqlJobOperationID,
						rqlPartID = eRPRFQLineInformationDto.rqlPartID,
						rqlPartLongDescriptionRtf = eRPRFQLineInformationDto.rqlPartLongDescriptionRtf,
						rqlPartLongDescriptionText = eRPRFQLineInformationDto.rqlPartLongDescriptionText,
						rqlPartRevisionID = eRPRFQLineInformationDto.rqlPartRevisionID,
						rqlPartShortDescription = eRPRFQLineInformationDto.rqlPartShortDescription,
						rqlProjectAreaID = eRPRFQLineInformationDto.rqlProjectAreaID,
						rqlProjectID = eRPRFQLineInformationDto.rqlProjectID,
						rqlPurchaseUnitOfMeasure = eRPRFQLineInformationDto.rqlPurchaseUnitOfMeasure,
						rqlQuoteAssemblyID = eRPRFQLineInformationDto.rqlQuoteAssemblyID,
						rqlQuoteID = eRPRFQLineInformationDto.rqlQuoteID,
						rqlQuoteLineID = eRPRFQLineInformationDto.rqlQuoteLineID,
						rqlQuoteMaterialID = eRPRFQLineInformationDto.rqlQuoteMaterialID,
						rqlQuoteOperationID = eRPRFQLineInformationDto.rqlQuoteOperationID,
						rqlRfqID = eRPRFQLineInformationDto.rqlRfqID,
						rqlRfqType = eRPRFQLineInformationDto.rqlRfqType,
						rqlRowVersion = eRPRFQLineInformationDto.rqlRowVersion,
						rqlRfqLineID = eRPRFQLineInformationDto.rqlRfqLineID,
						CustomFields = eRPRFQLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing RFQLine [{rFQLine.rqlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteRFQLine(Guid rFQLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
		using (iERPRFQLineRepository)
		{
			if (!(await base.ERPRFQLineRepository.DoesRFQLineExist(rFQLineId)))
			{
				base.ErrorsList.Add($"RFQLine [{rFQLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPRFQLineInformationDto eRPRFQLineInformationDto = await base.ERPRFQLineRepository.GetRFQLine(rFQLineId);
				string text = await base.ERPRFQLineRepository.WhereUsed("RFQLines", new object[2] { eRPRFQLineInformationDto.rqlRfqID, eRPRFQLineInformationDto.rqlRfqLineID }, new object[2] { "rqlRfqID", "rqlRfqLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("RFQLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPRFQLineDto>> Process_DeleteRFQLine(Guid rFQLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPRFQLineDto> result;
		try
		{
			IERPRFQLineRepository iERPRFQLineRepository = (base.ERPRFQLineRepository = new ERPRFQLineRepository(base.ApiClientContext));
			using (iERPRFQLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPRFQLineRepository.DeleteRowFromTable("RFQLines", "rql", rFQLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of RFQLine [{rFQLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPRFQLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPRFQLineDto()
			};
		}
		return result;
	}
}
