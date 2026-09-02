using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPQuoteLineModel : ERPBaseModel, IERPQuoteLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllQuoteLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
		using (iERPQuoteLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPQuoteLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPQuoteLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPQuoteLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPQuoteLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetQuoteLine(Guid quoteLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
		using (iERPQuoteLineRepository)
		{
			if (!(await base.ERPQuoteLineRepository.DoesQuoteLineExist(quoteLineId)))
			{
				errorsList.Add($"QuoteLine [{quoteLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutQuoteLine(ERPQuoteLineDto quoteLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
		using (iERPQuoteLineRepository)
		{
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlQuoteID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Quotes", new object[1] { "QMPQUOTEID" }, new object[1] { quoteLine.qmlQuoteID })))
			{
				errorsList.Add("qmlQuoteID [" + quoteLine.qmlQuoteID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlSourceMethodID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteLine.qmlSourceMethodID })))
			{
				errorsList.Add("qmlSourceMethodID [" + quoteLine.qmlSourceMethodID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlSourceRevisionID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteLine.qmlSourceMethodID, quoteLine.qmlSourceRevisionID })))
			{
				errorsList.Add("qmlSourceRevisionID [" + quoteLine.qmlSourceRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlPartID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Parts", new object[1] { "IMPPARTID" }, new object[1] { quoteLine.qmlPartID })))
			{
				errorsList.Add("qmlPartID [" + quoteLine.qmlPartID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlPartRevisionID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("PartRevisions", new object[2] { "IMRPARTID", "IMRPARTREVISIONID" }, new object[2] { quoteLine.qmlPartID, quoteLine.qmlPartRevisionID })))
			{
				errorsList.Add("qmlPartRevisionID [" + quoteLine.qmlPartRevisionID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlPartGroupID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("PartGroups", new object[1] { "IMUPARTGROUPID" }, new object[1] { quoteLine.qmlPartGroupID })))
			{
				errorsList.Add("qmlPartGroupID [" + quoteLine.qmlPartGroupID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlTaxCodeID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { quoteLine.qmlTaxCodeID })))
			{
				errorsList.Add("qmlTaxCodeID [" + quoteLine.qmlTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlNonTaxReasonID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { quoteLine.qmlNonTaxReasonID })))
			{
				errorsList.Add("qmlNonTaxReasonID [" + quoteLine.qmlNonTaxReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlSecondTaxCodeID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { quoteLine.qmlSecondTaxCodeID })))
			{
				errorsList.Add("qmlSecondTaxCodeID [" + quoteLine.qmlSecondTaxCodeID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlResolutionReasonID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Reasons", new object[1] { "XARREASONID" }, new object[1] { quoteLine.qmlResolutionReasonID })))
			{
				errorsList.Add("qmlResolutionReasonID [" + quoteLine.qmlResolutionReasonID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlLeadID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Leads", new object[1] { "LOPLEADID" }, new object[1] { quoteLine.qmlLeadID })))
			{
				errorsList.Add("qmlLeadID [" + quoteLine.qmlLeadID + "] not found.");
			}
			if (quoteLine.qmlLeadLineID > 0 && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("LeadLines", new object[2] { "LOLLEADID", "LOLLEADLINEID" }, new object[2] { quoteLine.qmlLeadID, quoteLine.qmlLeadLineID })))
			{
				errorsList.Add($"qmlLeadLineID [{quoteLine.qmlLeadLineID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlProjectID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Projects", new object[1] { "PRPPROJECTID" }, new object[1] { quoteLine.qmlProjectID })))
			{
				errorsList.Add("qmlProjectID [" + quoteLine.qmlProjectID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlProjectAreaID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("ProjectAreas", new object[2] { "PRAPROJECTID", "PRAPROJECTAREAID" }, new object[2] { quoteLine.qmlProjectID, quoteLine.qmlProjectAreaID })))
			{
				errorsList.Add("qmlProjectAreaID [" + quoteLine.qmlProjectAreaID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlSupplierOrganizationID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { quoteLine.qmlSupplierOrganizationID })))
			{
				errorsList.Add("qmlSupplierOrganizationID [" + quoteLine.qmlSupplierOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(quoteLine.qmlPurchaseLocationID) && !(await base.ERPQuoteLineRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { quoteLine.qmlSupplierOrganizationID, quoteLine.qmlPurchaseLocationID })))
			{
				errorsList.Add("qmlPurchaseLocationID [" + quoteLine.qmlPurchaseLocationID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPQuoteLineDto>>> Process_GetAllQuoteLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPQuoteLineDto> allQuoteLinesDto = new List<ERPQuoteLineDto>();
		ERPResponseMessageDto<IList<ERPQuoteLineDto>> result;
		try
		{
			IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
			using (iERPQuoteLineRepository)
			{
				foreach (ERPQuoteLineInformationDto item2 in await base.ERPQuoteLineRepository.GetAllQuoteLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPQuoteLineDto item = new ERPQuoteLineDto
					{
						qmlCreatedBy = item2.qmlCreatedBy,
						qmlCreatedDate = item2.qmlCreatedDate,
						qmlDocuments = item2.qmlDocuments,
						qmlUniqueID = item2.qmlUniqueID,
						qmlClosed = item2.qmlClosed,
						qmlCreatedFromMobile = item2.qmlCreatedFromMobile,
						qmlFirm = item2.qmlFirm,
						qmlMatrixCalculated = item2.qmlMatrixCalculated,
						qmlPurchaseToOrder = item2.qmlPurchaseToOrder,
						qmlTransferredToOrder = item2.qmlTransferredToOrder,
						qmlLeadID = item2.qmlLeadID,
						qmlLeadLineID = item2.qmlLeadLineID,
						qmlNonTaxReasonID = item2.qmlNonTaxReasonID,
						qmlOrgPartID = item2.qmlOrgPartID,
						qmlOrgPartShortDescription = item2.qmlOrgPartShortDescription,
						qmlPartGroupID = item2.qmlPartGroupID,
						qmlPartID = item2.qmlPartID,
						qmlPartLongDescriptionRtf = item2.qmlPartLongDescriptionRtf,
						qmlPartLongDescriptionText = item2.qmlPartLongDescriptionText,
						qmlPartRevisionID = item2.qmlPartRevisionID,
						qmlPartShortDescription = item2.qmlPartShortDescription,
						qmlProductionNotesRTF = item2.qmlProductionNotesRTF,
						qmlProductionNotesText = item2.qmlProductionNotesText,
						qmlProjectAreaID = item2.qmlProjectAreaID,
						qmlProjectID = item2.qmlProjectID,
						qmlPurchaseLocationID = item2.qmlPurchaseLocationID,
						qmlPurchaseUnitCostBase = item2.qmlPurchaseUnitCostBase,
						qmlPurchaseUnitCostForeign = item2.qmlPurchaseUnitCostForeign,
						qmlQuantityToTotal = item2.qmlQuantityToTotal,
						qmlQuoteID = item2.qmlQuoteID,
						qmlQuoteMarkupType = item2.qmlQuoteMarkupType,
						qmlResolutionReasonID = item2.qmlResolutionReasonID,
						qmlRowVersion = item2.qmlRowVersion,
						qmlSecondTaxCodeID = item2.qmlSecondTaxCodeID,
						qmlQuoteLineID = item2.qmlQuoteLineID,
						qmlSourceMethodID = item2.qmlSourceMethodID,
						qmlSourceRevisionID = item2.qmlSourceRevisionID,
						qmlSupplierOrganizationID = item2.qmlSupplierOrganizationID,
						qmlTaxCodeID = item2.qmlTaxCodeID,
						qmlTaxDate = item2.qmlTaxDate,
						qmlUnitOfMeasure = item2.qmlUnitOfMeasure,
						CustomFields = item2.CustomFields
					};
					allQuoteLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all QuoteLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPQuoteLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allQuoteLinesDto,
				RecordCount = allQuoteLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_GetQuoteLine(Guid quoteLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPQuoteLineDto quoteLineDto = null;
		ERPResponseMessageDto<ERPQuoteLineDto> result;
		try
		{
			IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
			using (iERPQuoteLineRepository)
			{
				ERPQuoteLineInformationDto eRPQuoteLineInformationDto = await base.ERPQuoteLineRepository.GetQuoteLine(quoteLineId);
				quoteLineDto = new ERPQuoteLineDto
				{
					qmlCreatedBy = eRPQuoteLineInformationDto.qmlCreatedBy,
					qmlCreatedDate = eRPQuoteLineInformationDto.qmlCreatedDate,
					qmlDocuments = eRPQuoteLineInformationDto.qmlDocuments,
					qmlUniqueID = eRPQuoteLineInformationDto.qmlUniqueID,
					qmlClosed = eRPQuoteLineInformationDto.qmlClosed,
					qmlCreatedFromMobile = eRPQuoteLineInformationDto.qmlCreatedFromMobile,
					qmlFirm = eRPQuoteLineInformationDto.qmlFirm,
					qmlMatrixCalculated = eRPQuoteLineInformationDto.qmlMatrixCalculated,
					qmlPurchaseToOrder = eRPQuoteLineInformationDto.qmlPurchaseToOrder,
					qmlTransferredToOrder = eRPQuoteLineInformationDto.qmlTransferredToOrder,
					qmlLeadID = eRPQuoteLineInformationDto.qmlLeadID,
					qmlLeadLineID = eRPQuoteLineInformationDto.qmlLeadLineID,
					qmlNonTaxReasonID = eRPQuoteLineInformationDto.qmlNonTaxReasonID,
					qmlOrgPartID = eRPQuoteLineInformationDto.qmlOrgPartID,
					qmlOrgPartShortDescription = eRPQuoteLineInformationDto.qmlOrgPartShortDescription,
					qmlPartGroupID = eRPQuoteLineInformationDto.qmlPartGroupID,
					qmlPartID = eRPQuoteLineInformationDto.qmlPartID,
					qmlPartLongDescriptionRtf = eRPQuoteLineInformationDto.qmlPartLongDescriptionRtf,
					qmlPartLongDescriptionText = eRPQuoteLineInformationDto.qmlPartLongDescriptionText,
					qmlPartRevisionID = eRPQuoteLineInformationDto.qmlPartRevisionID,
					qmlPartShortDescription = eRPQuoteLineInformationDto.qmlPartShortDescription,
					qmlProductionNotesRTF = eRPQuoteLineInformationDto.qmlProductionNotesRTF,
					qmlProductionNotesText = eRPQuoteLineInformationDto.qmlProductionNotesText,
					qmlProjectAreaID = eRPQuoteLineInformationDto.qmlProjectAreaID,
					qmlProjectID = eRPQuoteLineInformationDto.qmlProjectID,
					qmlPurchaseLocationID = eRPQuoteLineInformationDto.qmlPurchaseLocationID,
					qmlPurchaseUnitCostBase = eRPQuoteLineInformationDto.qmlPurchaseUnitCostBase,
					qmlPurchaseUnitCostForeign = eRPQuoteLineInformationDto.qmlPurchaseUnitCostForeign,
					qmlQuantityToTotal = eRPQuoteLineInformationDto.qmlQuantityToTotal,
					qmlQuoteID = eRPQuoteLineInformationDto.qmlQuoteID,
					qmlQuoteMarkupType = eRPQuoteLineInformationDto.qmlQuoteMarkupType,
					qmlResolutionReasonID = eRPQuoteLineInformationDto.qmlResolutionReasonID,
					qmlRowVersion = eRPQuoteLineInformationDto.qmlRowVersion,
					qmlSecondTaxCodeID = eRPQuoteLineInformationDto.qmlSecondTaxCodeID,
					qmlQuoteLineID = eRPQuoteLineInformationDto.qmlQuoteLineID,
					qmlSourceMethodID = eRPQuoteLineInformationDto.qmlSourceMethodID,
					qmlSourceRevisionID = eRPQuoteLineInformationDto.qmlSourceRevisionID,
					qmlSupplierOrganizationID = eRPQuoteLineInformationDto.qmlSupplierOrganizationID,
					qmlTaxCodeID = eRPQuoteLineInformationDto.qmlTaxCodeID,
					qmlTaxDate = eRPQuoteLineInformationDto.qmlTaxDate,
					qmlUnitOfMeasure = eRPQuoteLineInformationDto.qmlUnitOfMeasure,
					CustomFields = eRPQuoteLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the QuoteLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = quoteLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_PutQuoteLine(ERPQuoteLineDto quoteLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPQuoteLineDto createdObject = null;
		ERPResponseMessageDto<ERPQuoteLineDto> result;
		try
		{
			IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
			using (iERPQuoteLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPQuoteLineRepository.SaveQuoteLine(quoteLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPQuoteLineInformationDto eRPQuoteLineInformationDto = await base.ERPQuoteLineRepository.GetQuoteLine(quoteLine.qmlUniqueID);
					createdObject = new ERPQuoteLineDto
					{
						qmlCreatedBy = eRPQuoteLineInformationDto.qmlCreatedBy,
						qmlCreatedDate = eRPQuoteLineInformationDto.qmlCreatedDate,
						qmlDocuments = eRPQuoteLineInformationDto.qmlDocuments,
						qmlUniqueID = eRPQuoteLineInformationDto.qmlUniqueID,
						qmlClosed = eRPQuoteLineInformationDto.qmlClosed,
						qmlCreatedFromMobile = eRPQuoteLineInformationDto.qmlCreatedFromMobile,
						qmlFirm = eRPQuoteLineInformationDto.qmlFirm,
						qmlMatrixCalculated = eRPQuoteLineInformationDto.qmlMatrixCalculated,
						qmlPurchaseToOrder = eRPQuoteLineInformationDto.qmlPurchaseToOrder,
						qmlTransferredToOrder = eRPQuoteLineInformationDto.qmlTransferredToOrder,
						qmlLeadID = eRPQuoteLineInformationDto.qmlLeadID,
						qmlLeadLineID = eRPQuoteLineInformationDto.qmlLeadLineID,
						qmlNonTaxReasonID = eRPQuoteLineInformationDto.qmlNonTaxReasonID,
						qmlOrgPartID = eRPQuoteLineInformationDto.qmlOrgPartID,
						qmlOrgPartShortDescription = eRPQuoteLineInformationDto.qmlOrgPartShortDescription,
						qmlPartGroupID = eRPQuoteLineInformationDto.qmlPartGroupID,
						qmlPartID = eRPQuoteLineInformationDto.qmlPartID,
						qmlPartLongDescriptionRtf = eRPQuoteLineInformationDto.qmlPartLongDescriptionRtf,
						qmlPartLongDescriptionText = eRPQuoteLineInformationDto.qmlPartLongDescriptionText,
						qmlPartRevisionID = eRPQuoteLineInformationDto.qmlPartRevisionID,
						qmlPartShortDescription = eRPQuoteLineInformationDto.qmlPartShortDescription,
						qmlProductionNotesRTF = eRPQuoteLineInformationDto.qmlProductionNotesRTF,
						qmlProductionNotesText = eRPQuoteLineInformationDto.qmlProductionNotesText,
						qmlProjectAreaID = eRPQuoteLineInformationDto.qmlProjectAreaID,
						qmlProjectID = eRPQuoteLineInformationDto.qmlProjectID,
						qmlPurchaseLocationID = eRPQuoteLineInformationDto.qmlPurchaseLocationID,
						qmlPurchaseUnitCostBase = eRPQuoteLineInformationDto.qmlPurchaseUnitCostBase,
						qmlPurchaseUnitCostForeign = eRPQuoteLineInformationDto.qmlPurchaseUnitCostForeign,
						qmlQuantityToTotal = eRPQuoteLineInformationDto.qmlQuantityToTotal,
						qmlQuoteID = eRPQuoteLineInformationDto.qmlQuoteID,
						qmlQuoteMarkupType = eRPQuoteLineInformationDto.qmlQuoteMarkupType,
						qmlResolutionReasonID = eRPQuoteLineInformationDto.qmlResolutionReasonID,
						qmlRowVersion = eRPQuoteLineInformationDto.qmlRowVersion,
						qmlSecondTaxCodeID = eRPQuoteLineInformationDto.qmlSecondTaxCodeID,
						qmlQuoteLineID = eRPQuoteLineInformationDto.qmlQuoteLineID,
						qmlSourceMethodID = eRPQuoteLineInformationDto.qmlSourceMethodID,
						qmlSourceRevisionID = eRPQuoteLineInformationDto.qmlSourceRevisionID,
						qmlSupplierOrganizationID = eRPQuoteLineInformationDto.qmlSupplierOrganizationID,
						qmlTaxCodeID = eRPQuoteLineInformationDto.qmlTaxCodeID,
						qmlTaxDate = eRPQuoteLineInformationDto.qmlTaxDate,
						qmlUnitOfMeasure = eRPQuoteLineInformationDto.qmlUnitOfMeasure,
						CustomFields = eRPQuoteLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing QuoteLine [{quoteLine.qmlUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteQuoteLine(Guid quoteLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
		using (iERPQuoteLineRepository)
		{
			if (!(await base.ERPQuoteLineRepository.DoesQuoteLineExist(quoteLineId)))
			{
				base.ErrorsList.Add($"QuoteLine [{quoteLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPQuoteLineInformationDto eRPQuoteLineInformationDto = await base.ERPQuoteLineRepository.GetQuoteLine(quoteLineId);
				string text = await base.ERPQuoteLineRepository.WhereUsed("QuoteLines", new object[2] { eRPQuoteLineInformationDto.qmlQuoteID, eRPQuoteLineInformationDto.qmlQuoteLineID }, new object[2] { "qmlQuoteID", "qmlQuoteLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("QuoteLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPQuoteLineDto>> Process_DeleteQuoteLine(Guid quoteLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPQuoteLineDto> result;
		try
		{
			IERPQuoteLineRepository iERPQuoteLineRepository = (base.ERPQuoteLineRepository = new ERPQuoteLineRepository(base.ApiClientContext));
			using (iERPQuoteLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPQuoteLineRepository.DeleteRowFromTable("QuoteLines", "qml", quoteLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of QuoteLine [{quoteLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPQuoteLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPQuoteLineDto()
			};
		}
		return result;
	}
}
