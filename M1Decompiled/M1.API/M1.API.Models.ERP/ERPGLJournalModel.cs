using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLJournalModel : ERPBaseModel, IERPGLJournalModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
		using (iERPGLJournalRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLJournalRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLJournalRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLJournalRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLJournalRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLJournal(Guid gLJournalId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
		using (iERPGLJournalRepository)
		{
			if (!(await base.ERPGLJournalRepository.DoesGLJournalExist(gLJournalId)))
			{
				errorsList.Add($"GLJournal [{gLJournalId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLJournal(ERPGLJournalDto gLJournal)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
		using (iERPGLJournalRepository)
		{
			if (gLJournal.glpGlFiscalYearID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLJournal.glpGlFiscalYearID })))
			{
				errorsList.Add($"glpGlFiscalYearID [{gLJournal.glpGlFiscalYearID}] not found.");
			}
			if (gLJournal.glpGlFiscalYearPeriodID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { gLJournal.glpGlFiscalYearID, gLJournal.glpGlFiscalYearPeriodID })))
			{
				errorsList.Add($"glpGlFiscalYearPeriodID [{gLJournal.glpGlFiscalYearPeriodID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpOrganizationID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { gLJournal.glpOrganizationID })))
			{
				errorsList.Add("glpOrganizationID [" + gLJournal.glpOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpLocationID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { gLJournal.glpOrganizationID, gLJournal.glpLocationID })))
			{
				errorsList.Add("glpLocationID [" + gLJournal.glpLocationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpArInvoiceID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("ARInvoices", new object[1] { "ARPARINVOICEID" }, new object[1] { gLJournal.glpArInvoiceID })))
			{
				errorsList.Add("glpArInvoiceID [" + gLJournal.glpArInvoiceID + "] not found.");
			}
			if (gLJournal.glpArPaymentSessionID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { gLJournal.glpArPaymentSessionID })))
			{
				errorsList.Add($"glpArPaymentSessionID [{gLJournal.glpArPaymentSessionID}] not found.");
			}
			if (gLJournal.glpArPaymentHeaderID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { gLJournal.glpArPaymentSessionID, gLJournal.glpArPaymentHeaderID })))
			{
				errorsList.Add($"glpArPaymentHeaderID [{gLJournal.glpArPaymentHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpApInvoiceID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("APInvoices", new object[1] { "APPAPINVOICEID" }, new object[1] { gLJournal.glpApInvoiceID })))
			{
				errorsList.Add("glpApInvoiceID [" + gLJournal.glpApInvoiceID + "] not found.");
			}
			if (gLJournal.glpApPaymentSessionID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("APPaymentSessions", new object[1] { "APSAPPAYMENTSESSIONID" }, new object[1] { gLJournal.glpApPaymentSessionID })))
			{
				errorsList.Add($"glpApPaymentSessionID [{gLJournal.glpApPaymentSessionID}] not found.");
			}
			if (gLJournal.glpApPaymentHeaderID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("APPaymentHeaders", new object[2] { "APTAPPAYMENTSESSIONID", "APTAPPAYMENTHEADERID" }, new object[2] { gLJournal.glpApPaymentSessionID, gLJournal.glpApPaymentHeaderID })))
			{
				errorsList.Add($"glpApPaymentHeaderID [{gLJournal.glpApPaymentHeaderID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpReceiptID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Receipts", new object[1] { "RMPRECEIPTID" }, new object[1] { gLJournal.glpReceiptID })))
			{
				errorsList.Add("glpReceiptID [" + gLJournal.glpReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpShipmentID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Shipments", new object[1] { "SMPSHIPMENTID" }, new object[1] { gLJournal.glpShipmentID })))
			{
				errorsList.Add("glpShipmentID [" + gLJournal.glpShipmentID + "] not found.");
			}
			if (gLJournal.glpTimecardID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Timecards", new object[1] { "LMPTIMECARDID" }, new object[1] { gLJournal.glpTimecardID })))
			{
				errorsList.Add($"glpTimecardID [{gLJournal.glpTimecardID}] not found.");
			}
			if (gLJournal.glpBankStatementID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("BankStatements", new object[1] { "GLSBANKSTATEMENTID" }, new object[1] { gLJournal.glpBankStatementID })))
			{
				errorsList.Add($"glpBankStatementID [{gLJournal.glpBankStatementID}] not found.");
			}
			if (gLJournal.glpAssetAdjustmentID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("AssetAdjustments", new object[1] { "FAAASSETADJUSTMENTID" }, new object[1] { gLJournal.glpAssetAdjustmentID })))
			{
				errorsList.Add($"glpAssetAdjustmentID [{gLJournal.glpAssetAdjustmentID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpAssetID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Assets", new object[1] { "FAPASSETID" }, new object[1] { gLJournal.glpAssetID })))
			{
				errorsList.Add("glpAssetID [" + gLJournal.glpAssetID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpJobID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { gLJournal.glpJobID })))
			{
				errorsList.Add("glpJobID [" + gLJournal.glpJobID + "] not found.");
			}
			if (gLJournal.glpJobAssemblyID > 0 && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { gLJournal.glpJobID, gLJournal.glpJobAssemblyID })))
			{
				errorsList.Add($"glpJobAssemblyID [{gLJournal.glpJobAssemblyID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpRmaReceiptID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("RMAReceipts", new object[1] { "RRPRMARECEIPTID" }, new object[1] { gLJournal.glpRmaReceiptID })))
			{
				errorsList.Add("glpRmaReceiptID [" + gLJournal.glpRmaReceiptID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpDmrShipmentID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("DMRShipments", new object[1] { "DSPDMRSHIPMENTID" }, new object[1] { gLJournal.glpDmrShipmentID })))
			{
				errorsList.Add("glpDmrShipmentID [" + gLJournal.glpDmrShipmentID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournal.glpLandedCostID) && !(await base.ERPGLJournalRepository.DoesRecordExistInTableUsingKeys("LandedCosts", new object[1] { "RMCLANDEDCOSTID" }, new object[1] { gLJournal.glpLandedCostID })))
			{
				errorsList.Add("glpLandedCostID [" + gLJournal.glpLandedCostID + "] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLJournalDto>>> Process_GetAllGLJournals(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLJournalDto> allGLJournalsDto = new List<ERPGLJournalDto>();
		ERPResponseMessageDto<IList<ERPGLJournalDto>> result;
		try
		{
			IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
			using (iERPGLJournalRepository)
			{
				foreach (ERPGLJournalInformationDto item2 in await base.ERPGLJournalRepository.GetAllGLJournals(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLJournalDto item = new ERPGLJournalDto
					{
						glpApInvoiceID = item2.glpApInvoiceID,
						glpApPaymentHeaderID = item2.glpApPaymentHeaderID,
						glpApPaymentSessionID = item2.glpApPaymentSessionID,
						glpArInvoiceID = item2.glpArInvoiceID,
						glpArPaymentHeaderID = item2.glpArPaymentHeaderID,
						glpArPaymentSessionID = item2.glpArPaymentSessionID,
						glpAssetAdjustmentID = item2.glpAssetAdjustmentID,
						glpAssetID = item2.glpAssetID,
						glpBankStatementID = item2.glpBankStatementID,
						glpCreatedBy = item2.glpCreatedBy,
						glpCreatedDate = item2.glpCreatedDate,
						glpDescription = item2.glpDescription,
						glpDetailSource = item2.glpDetailSource,
						glpDmrShipmentID = item2.glpDmrShipmentID,
						glpUniqueID = item2.glpUniqueID,
						glpGlFiscalYearID = item2.glpGlFiscalYearID,
						glpGlFiscalYearPeriodID = item2.glpGlFiscalYearPeriodID,
						glpPosted = item2.glpPosted,
						glpReversingEntry = item2.glpReversingEntry,
						glpJobAssemblyID = item2.glpJobAssemblyID,
						glpJobID = item2.glpJobID,
						glpLandedCostID = item2.glpLandedCostID,
						glpLocationID = item2.glpLocationID,
						glpLongDescriptionRtf = item2.glpLongDescriptionRtf,
						glpLongDescriptionText = item2.glpLongDescriptionText,
						glpOrganizationID = item2.glpOrganizationID,
						glpPostedDate = item2.glpPostedDate,
						glpReceiptID = item2.glpReceiptID,
						glpReference = item2.glpReference,
						glpRmaReceiptID = item2.glpRmaReceiptID,
						glpRowVersion = item2.glpRowVersion,
						glpGlJournalID = item2.glpGlJournalID,
						glpShipmentID = item2.glpShipmentID,
						glpSource = item2.glpSource,
						glpTimecardID = item2.glpTimecardID,
						glpTotalCredits = item2.glpTotalCredits,
						glpTotalDebits = item2.glpTotalDebits,
						glpTransactionDate = item2.glpTransactionDate,
						CustomFields = item2.CustomFields
					};
					allGLJournalsDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLJournals]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLJournalDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLJournalsDto,
				RecordCount = allGLJournalsDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_GetGLJournal(Guid gLJournalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLJournalDto gLJournalDto = null;
		ERPResponseMessageDto<ERPGLJournalDto> result;
		try
		{
			IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
			using (iERPGLJournalRepository)
			{
				ERPGLJournalInformationDto eRPGLJournalInformationDto = await base.ERPGLJournalRepository.GetGLJournal(gLJournalId);
				gLJournalDto = new ERPGLJournalDto
				{
					glpApInvoiceID = eRPGLJournalInformationDto.glpApInvoiceID,
					glpApPaymentHeaderID = eRPGLJournalInformationDto.glpApPaymentHeaderID,
					glpApPaymentSessionID = eRPGLJournalInformationDto.glpApPaymentSessionID,
					glpArInvoiceID = eRPGLJournalInformationDto.glpArInvoiceID,
					glpArPaymentHeaderID = eRPGLJournalInformationDto.glpArPaymentHeaderID,
					glpArPaymentSessionID = eRPGLJournalInformationDto.glpArPaymentSessionID,
					glpAssetAdjustmentID = eRPGLJournalInformationDto.glpAssetAdjustmentID,
					glpAssetID = eRPGLJournalInformationDto.glpAssetID,
					glpBankStatementID = eRPGLJournalInformationDto.glpBankStatementID,
					glpCreatedBy = eRPGLJournalInformationDto.glpCreatedBy,
					glpCreatedDate = eRPGLJournalInformationDto.glpCreatedDate,
					glpDescription = eRPGLJournalInformationDto.glpDescription,
					glpDetailSource = eRPGLJournalInformationDto.glpDetailSource,
					glpDmrShipmentID = eRPGLJournalInformationDto.glpDmrShipmentID,
					glpUniqueID = eRPGLJournalInformationDto.glpUniqueID,
					glpGlFiscalYearID = eRPGLJournalInformationDto.glpGlFiscalYearID,
					glpGlFiscalYearPeriodID = eRPGLJournalInformationDto.glpGlFiscalYearPeriodID,
					glpPosted = eRPGLJournalInformationDto.glpPosted,
					glpReversingEntry = eRPGLJournalInformationDto.glpReversingEntry,
					glpJobAssemblyID = eRPGLJournalInformationDto.glpJobAssemblyID,
					glpJobID = eRPGLJournalInformationDto.glpJobID,
					glpLandedCostID = eRPGLJournalInformationDto.glpLandedCostID,
					glpLocationID = eRPGLJournalInformationDto.glpLocationID,
					glpLongDescriptionRtf = eRPGLJournalInformationDto.glpLongDescriptionRtf,
					glpLongDescriptionText = eRPGLJournalInformationDto.glpLongDescriptionText,
					glpOrganizationID = eRPGLJournalInformationDto.glpOrganizationID,
					glpPostedDate = eRPGLJournalInformationDto.glpPostedDate,
					glpReceiptID = eRPGLJournalInformationDto.glpReceiptID,
					glpReference = eRPGLJournalInformationDto.glpReference,
					glpRmaReceiptID = eRPGLJournalInformationDto.glpRmaReceiptID,
					glpRowVersion = eRPGLJournalInformationDto.glpRowVersion,
					glpGlJournalID = eRPGLJournalInformationDto.glpGlJournalID,
					glpShipmentID = eRPGLJournalInformationDto.glpShipmentID,
					glpSource = eRPGLJournalInformationDto.glpSource,
					glpTimecardID = eRPGLJournalInformationDto.glpTimecardID,
					glpTotalCredits = eRPGLJournalInformationDto.glpTotalCredits,
					glpTotalDebits = eRPGLJournalInformationDto.glpTotalDebits,
					glpTransactionDate = eRPGLJournalInformationDto.glpTransactionDate,
					CustomFields = eRPGLJournalInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLJournals []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLJournalDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_PutGLJournal(ERPGLJournalDto gLJournal)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLJournalDto createdObject = null;
		ERPResponseMessageDto<ERPGLJournalDto> result;
		try
		{
			IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
			using (iERPGLJournalRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLJournalRepository.SaveGLJournal(gLJournal);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLJournalInformationDto eRPGLJournalInformationDto = await base.ERPGLJournalRepository.GetGLJournal(gLJournal.glpUniqueID);
					createdObject = new ERPGLJournalDto
					{
						glpApInvoiceID = eRPGLJournalInformationDto.glpApInvoiceID,
						glpApPaymentHeaderID = eRPGLJournalInformationDto.glpApPaymentHeaderID,
						glpApPaymentSessionID = eRPGLJournalInformationDto.glpApPaymentSessionID,
						glpArInvoiceID = eRPGLJournalInformationDto.glpArInvoiceID,
						glpArPaymentHeaderID = eRPGLJournalInformationDto.glpArPaymentHeaderID,
						glpArPaymentSessionID = eRPGLJournalInformationDto.glpArPaymentSessionID,
						glpAssetAdjustmentID = eRPGLJournalInformationDto.glpAssetAdjustmentID,
						glpAssetID = eRPGLJournalInformationDto.glpAssetID,
						glpBankStatementID = eRPGLJournalInformationDto.glpBankStatementID,
						glpCreatedBy = eRPGLJournalInformationDto.glpCreatedBy,
						glpCreatedDate = eRPGLJournalInformationDto.glpCreatedDate,
						glpDescription = eRPGLJournalInformationDto.glpDescription,
						glpDetailSource = eRPGLJournalInformationDto.glpDetailSource,
						glpDmrShipmentID = eRPGLJournalInformationDto.glpDmrShipmentID,
						glpUniqueID = eRPGLJournalInformationDto.glpUniqueID,
						glpGlFiscalYearID = eRPGLJournalInformationDto.glpGlFiscalYearID,
						glpGlFiscalYearPeriodID = eRPGLJournalInformationDto.glpGlFiscalYearPeriodID,
						glpPosted = eRPGLJournalInformationDto.glpPosted,
						glpReversingEntry = eRPGLJournalInformationDto.glpReversingEntry,
						glpJobAssemblyID = eRPGLJournalInformationDto.glpJobAssemblyID,
						glpJobID = eRPGLJournalInformationDto.glpJobID,
						glpLandedCostID = eRPGLJournalInformationDto.glpLandedCostID,
						glpLocationID = eRPGLJournalInformationDto.glpLocationID,
						glpLongDescriptionRtf = eRPGLJournalInformationDto.glpLongDescriptionRtf,
						glpLongDescriptionText = eRPGLJournalInformationDto.glpLongDescriptionText,
						glpOrganizationID = eRPGLJournalInformationDto.glpOrganizationID,
						glpPostedDate = eRPGLJournalInformationDto.glpPostedDate,
						glpReceiptID = eRPGLJournalInformationDto.glpReceiptID,
						glpReference = eRPGLJournalInformationDto.glpReference,
						glpRmaReceiptID = eRPGLJournalInformationDto.glpRmaReceiptID,
						glpRowVersion = eRPGLJournalInformationDto.glpRowVersion,
						glpGlJournalID = eRPGLJournalInformationDto.glpGlJournalID,
						glpShipmentID = eRPGLJournalInformationDto.glpShipmentID,
						glpSource = eRPGLJournalInformationDto.glpSource,
						glpTimecardID = eRPGLJournalInformationDto.glpTimecardID,
						glpTotalCredits = eRPGLJournalInformationDto.glpTotalCredits,
						glpTotalDebits = eRPGLJournalInformationDto.glpTotalDebits,
						glpTransactionDate = eRPGLJournalInformationDto.glpTransactionDate,
						CustomFields = eRPGLJournalInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLJournal [{gLJournal.glpUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournal(Guid gLJournalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
		using (iERPGLJournalRepository)
		{
			if (!(await base.ERPGLJournalRepository.DoesGLJournalExist(gLJournalId)))
			{
				base.ErrorsList.Add($"GLJournal [{gLJournalId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLJournalInformationDto eRPGLJournalInformationDto = await base.ERPGLJournalRepository.GetGLJournal(gLJournalId);
				string text = await base.ERPGLJournalRepository.WhereUsed("GLJournals", new object[1] { eRPGLJournalInformationDto.glpGlJournalID }, new object[1] { "glpGlJournalID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLJournal cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalDto>> Process_DeleteGLJournal(Guid gLJournalId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLJournalDto> result;
		try
		{
			IERPGLJournalRepository iERPGLJournalRepository = (base.ERPGLJournalRepository = new ERPGLJournalRepository(base.ApiClientContext));
			using (iERPGLJournalRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLJournalRepository.DeleteRowFromTable("GLJournals", "glp", gLJournalId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLJournal [{gLJournalId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLJournalDto()
			};
		}
		return result;
	}
}
