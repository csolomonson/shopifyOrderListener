using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using M1.API.Controllers;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Repositories.ERP;

namespace M1.API.Models.ERP;

public class ERPGLJournalLineModel : ERPBaseModel, IERPGLJournalLineModel, IERPBaseModel, IAPIBaseModel, IDisposable
{
	public async Task<APIValidationInfoDto> ValidateRequest_GetAllGLJournalLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		List<string> list = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpValidationStatusCode = HttpStatusCode.OK;
		IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
		using (iERPGLJournalLineRepository)
		{
			if (filter != null && filter.Length != 0 && !base.ERPGLJournalLineRepository.ValidateFilterClause(filter))
			{
				string text = string.Join(", ", filter);
				list.Add("Filter clause passed '" + text + "' is invalid.");
			}
			if (!string.IsNullOrWhiteSpace(orderBy) && !base.ERPGLJournalLineRepository.ValidateOrderByClause(orderBy))
			{
				list.Add("OrderBy clause passed '" + orderBy + "' is invalid.");
			}
			if (pageSize > base.ERPGLJournalLineRepository.MaxPageSize)
			{
				list.Add($"Page size [{pageSize}] exceeds the maximum allowed value of {base.ERPGLJournalLineRepository.MaxPageSize}.");
			}
		}
		if (list != null && list.Count > 0)
		{
			httpValidationStatusCode = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(list, warningsList, httpValidationStatusCode));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_GetGLJournalLine(Guid gLJournalLineId)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
		using (iERPGLJournalLineRepository)
		{
			if (!(await base.ERPGLJournalLineRepository.DoesGLJournalLineExist(gLJournalLineId)))
			{
				errorsList.Add($"GLJournalLine [{gLJournalLineId}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.NotFound;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<APIValidationInfoDto> ValidateRequest_PutGLJournalLine(ERPGLJournalLineDto gLJournalLine)
	{
		List<string> errorsList = new List<string>();
		List<string> warningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
		using (iERPGLJournalLineRepository)
		{
			if (gLJournalLine.gllGlJournalID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("GLJournals", new object[1] { "GLPGLJOURNALID" }, new object[1] { gLJournalLine.gllGlJournalID })))
			{
				errorsList.Add($"gllGlJournalID [{gLJournalLine.gllGlJournalID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournalLine.gllGlAccountID) && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("GLAccounts", new object[1] { "GLAGLACCOUNTID" }, new object[1] { gLJournalLine.gllGlAccountID })))
			{
				errorsList.Add("gllGlAccountID [" + gLJournalLine.gllGlAccountID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournalLine.gllTaxCodeID) && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("TaxCodes", new object[1] { "XAXTAXCODEID" }, new object[1] { gLJournalLine.gllTaxCodeID })))
			{
				errorsList.Add("gllTaxCodeID [" + gLJournalLine.gllTaxCodeID + "] not found.");
			}
			if (gLJournalLine.gllGlFiscalYearID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("GLFiscalYears", new object[1] { "GLZGLFISCALYEARID" }, new object[1] { gLJournalLine.gllGlFiscalYearID })))
			{
				errorsList.Add($"gllGlFiscalYearID [{gLJournalLine.gllGlFiscalYearID}] not found.");
			}
			if (gLJournalLine.gllGlFiscalYearPeriodID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("GLFiscalYearPeriods", new object[2] { "GLFGLFISCALYEARID", "GLFGLFISCALYEARPERIODID" }, new object[2] { gLJournalLine.gllGlFiscalYearID, gLJournalLine.gllGlFiscalYearPeriodID })))
			{
				errorsList.Add($"gllGlFiscalYearPeriodID [{gLJournalLine.gllGlFiscalYearPeriodID}] not found.");
			}
			if (gLJournalLine.gllPartTransactionID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("PartTransactions", new object[1] { "IMTPARTTRANSACTIONID" }, new object[1] { gLJournalLine.gllPartTransactionID })))
			{
				errorsList.Add($"gllPartTransactionID [{gLJournalLine.gllPartTransactionID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournalLine.gllJobID) && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("Jobs", new object[1] { "JMPJOBID" }, new object[1] { gLJournalLine.gllJobID })))
			{
				errorsList.Add("gllJobID [" + gLJournalLine.gllJobID + "] not found.");
			}
			if (gLJournalLine.gllJobAssemblyID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("JobAssemblies", new object[2] { "JMAJOBID", "JMAJOBASSEMBLYID" }, new object[2] { gLJournalLine.gllJobID, gLJournalLine.gllJobAssemblyID })))
			{
				errorsList.Add($"gllJobAssemblyID [{gLJournalLine.gllJobAssemblyID}] not found.");
			}
			if (gLJournalLine.gllJobMaterialID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("JobMaterials", new object[3] { "JMMJOBID", "JMMJOBASSEMBLYID", "JMMJOBMATERIALID" }, new object[3] { gLJournalLine.gllJobID, gLJournalLine.gllJobAssemblyID, gLJournalLine.gllJobMaterialID })))
			{
				errorsList.Add($"gllJobMaterialID [{gLJournalLine.gllJobMaterialID}] not found.");
			}
			if (gLJournalLine.gllJobOperationID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("JobOperations", new object[3] { "JMOJOBID", "JMOJOBASSEMBLYID", "JMOJOBOPERATIONID" }, new object[3] { gLJournalLine.gllJobID, gLJournalLine.gllJobAssemblyID, gLJournalLine.gllJobOperationID })))
			{
				errorsList.Add($"gllJobOperationID [{gLJournalLine.gllJobOperationID}] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournalLine.gllOrganizationID) && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("Organizations", new object[1] { "CMOORGANIZATIONID" }, new object[1] { gLJournalLine.gllOrganizationID })))
			{
				errorsList.Add("gllOrganizationID [" + gLJournalLine.gllOrganizationID + "] not found.");
			}
			if (!string.IsNullOrWhiteSpace(gLJournalLine.gllLocationID) && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("OrganizationLocations", new object[2] { "CMLORGANIZATIONID", "CMLLOCATIONID" }, new object[2] { gLJournalLine.gllOrganizationID, gLJournalLine.gllLocationID })))
			{
				errorsList.Add("gllLocationID [" + gLJournalLine.gllLocationID + "] not found.");
			}
			if (gLJournalLine.gllArPaymentSessionID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("ARPaymentSessions", new object[1] { "ARSARPAYMENTSESSIONID" }, new object[1] { gLJournalLine.gllArPaymentSessionID })))
			{
				errorsList.Add($"gllArPaymentSessionID [{gLJournalLine.gllArPaymentSessionID}] not found.");
			}
			if (gLJournalLine.gllArPaymentHeaderID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("ARPaymentHeaders", new object[2] { "ARTARPAYMENTSESSIONID", "ARTARPAYMENTHEADERID" }, new object[2] { gLJournalLine.gllArPaymentSessionID, gLJournalLine.gllArPaymentHeaderID })))
			{
				errorsList.Add($"gllArPaymentHeaderID [{gLJournalLine.gllArPaymentHeaderID}] not found.");
			}
			if (gLJournalLine.gllJobMaterialComponentID > 0 && !(await base.ERPGLJournalLineRepository.DoesRecordExistInTableUsingKeys("JobMaterialComponents", new object[4] { "JMTJOBID", "JMTJOBASSEMBLYID", "JMTJOBMATERIALID", "JMTJOBMATERIALCOMPONENTID" }, new object[4] { gLJournalLine.gllJobID, gLJournalLine.gllJobAssemblyID, gLJournalLine.gllJobMaterialID, gLJournalLine.gllJobMaterialComponentID })))
			{
				errorsList.Add($"gllJobMaterialComponentID [{gLJournalLine.gllJobMaterialComponentID}] not found.");
			}
		}
		if (errorsList != null && errorsList.Count > 0)
		{
			httpStatus = HttpStatusCode.BadRequest;
		}
		return await Task.FromResult(new APIValidationInfoDto(errorsList, warningsList, httpStatus));
	}

	public async Task<ERPResponseMessageDto<IList<ERPGLJournalLineDto>>> Process_GetAllGLJournalLines(int pageSize, int pageNumber, string[] filter, string orderBy)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IList<ERPGLJournalLineDto> allGLJournalLinesDto = new List<ERPGLJournalLineDto>();
		ERPResponseMessageDto<IList<ERPGLJournalLineDto>> result;
		try
		{
			IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
			using (iERPGLJournalLineRepository)
			{
				foreach (ERPGLJournalLineInformationDto item2 in await base.ERPGLJournalLineRepository.GetAllGLJournalLines(pageSize, pageNumber, filter, orderBy))
				{
					ERPGLJournalLineDto item = new ERPGLJournalLineDto
					{
						gllArPaymentHeaderID = item2.gllArPaymentHeaderID,
						gllArPaymentSessionID = item2.gllArPaymentSessionID,
						gllCreatedBy = item2.gllCreatedBy,
						gllCreatedDate = item2.gllCreatedDate,
						gllCreditAmount = item2.gllCreditAmount,
						gllDebitAmount = item2.gllDebitAmount,
						gllDescription = item2.gllDescription,
						gllUniqueID = item2.gllUniqueID,
						gllGlAccountID = item2.gllGlAccountID,
						gllGlFiscalYearID = item2.gllGlFiscalYearID,
						gllGlFiscalYearPeriodID = item2.gllGlFiscalYearPeriodID,
						gllGlJournalID = item2.gllGlJournalID,
						gllPosted = item2.gllPosted,
						gllJobAssemblyID = item2.gllJobAssemblyID,
						gllJobID = item2.gllJobID,
						gllJobMaterialComponentID = item2.gllJobMaterialComponentID,
						gllJobMaterialID = item2.gllJobMaterialID,
						gllJobOperationID = item2.gllJobOperationID,
						gllLocationID = item2.gllLocationID,
						gllOrganizationID = item2.gllOrganizationID,
						gllPartTransactionID = item2.gllPartTransactionID,
						gllReference = item2.gllReference,
						gllRowVersion = item2.gllRowVersion,
						gllGlJournalLineID = item2.gllGlJournalLineID,
						gllSourceTableName = item2.gllSourceTableName,
						gllSourceTableUniqueID = item2.gllSourceTableUniqueID,
						gllTaxableAmount = item2.gllTaxableAmount,
						gllTaxCodeID = item2.gllTaxCodeID,
						gllTransactionAmount = item2.gllTransactionAmount,
						gllTransactionDate = item2.gllTransactionDate,
						gllTransactionType = item2.gllTransactionType,
						CustomFields = item2.CustomFields
					};
					allGLJournalLinesDto.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while recovering all GLJournalLines]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<IList<ERPGLJournalLineDto>>
			{
				ValidationInfo = validationInfo,
				ReturnObject = allGLJournalLinesDto,
				RecordCount = allGLJournalLinesDto.Count
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_GetGLJournalLine(Guid gLJournalLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPGLJournalLineDto gLJournalLineDto = null;
		ERPResponseMessageDto<ERPGLJournalLineDto> result;
		try
		{
			IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
			using (iERPGLJournalLineRepository)
			{
				ERPGLJournalLineInformationDto eRPGLJournalLineInformationDto = await base.ERPGLJournalLineRepository.GetGLJournalLine(gLJournalLineId);
				gLJournalLineDto = new ERPGLJournalLineDto
				{
					gllArPaymentHeaderID = eRPGLJournalLineInformationDto.gllArPaymentHeaderID,
					gllArPaymentSessionID = eRPGLJournalLineInformationDto.gllArPaymentSessionID,
					gllCreatedBy = eRPGLJournalLineInformationDto.gllCreatedBy,
					gllCreatedDate = eRPGLJournalLineInformationDto.gllCreatedDate,
					gllCreditAmount = eRPGLJournalLineInformationDto.gllCreditAmount,
					gllDebitAmount = eRPGLJournalLineInformationDto.gllDebitAmount,
					gllDescription = eRPGLJournalLineInformationDto.gllDescription,
					gllUniqueID = eRPGLJournalLineInformationDto.gllUniqueID,
					gllGlAccountID = eRPGLJournalLineInformationDto.gllGlAccountID,
					gllGlFiscalYearID = eRPGLJournalLineInformationDto.gllGlFiscalYearID,
					gllGlFiscalYearPeriodID = eRPGLJournalLineInformationDto.gllGlFiscalYearPeriodID,
					gllGlJournalID = eRPGLJournalLineInformationDto.gllGlJournalID,
					gllPosted = eRPGLJournalLineInformationDto.gllPosted,
					gllJobAssemblyID = eRPGLJournalLineInformationDto.gllJobAssemblyID,
					gllJobID = eRPGLJournalLineInformationDto.gllJobID,
					gllJobMaterialComponentID = eRPGLJournalLineInformationDto.gllJobMaterialComponentID,
					gllJobMaterialID = eRPGLJournalLineInformationDto.gllJobMaterialID,
					gllJobOperationID = eRPGLJournalLineInformationDto.gllJobOperationID,
					gllLocationID = eRPGLJournalLineInformationDto.gllLocationID,
					gllOrganizationID = eRPGLJournalLineInformationDto.gllOrganizationID,
					gllPartTransactionID = eRPGLJournalLineInformationDto.gllPartTransactionID,
					gllReference = eRPGLJournalLineInformationDto.gllReference,
					gllRowVersion = eRPGLJournalLineInformationDto.gllRowVersion,
					gllGlJournalLineID = eRPGLJournalLineInformationDto.gllGlJournalLineID,
					gllSourceTableName = eRPGLJournalLineInformationDto.gllSourceTableName,
					gllSourceTableUniqueID = eRPGLJournalLineInformationDto.gllSourceTableUniqueID,
					gllTaxableAmount = eRPGLJournalLineInformationDto.gllTaxableAmount,
					gllTaxCodeID = eRPGLJournalLineInformationDto.gllTaxCodeID,
					gllTransactionAmount = eRPGLJournalLineInformationDto.gllTransactionAmount,
					gllTransactionDate = eRPGLJournalLineInformationDto.gllTransactionDate,
					gllTransactionType = eRPGLJournalLineInformationDto.gllTransactionType,
					CustomFields = eRPGLJournalLineInformationDto.CustomFields
				};
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add("Error occurred [" + ex.Message + "] while processing the GLJournalLines []");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = gLJournalLineDto
			};
		}
		return result;
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_PutGLJournalLine(ERPGLJournalLineDto gLJournalLine)
	{
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		ERPGLJournalLineDto createdObject = null;
		ERPResponseMessageDto<ERPGLJournalLineDto> result;
		try
		{
			IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
			using (iERPGLJournalLineRepository)
			{
				APIValidationInfoDto postResult = await base.ERPGLJournalLineRepository.SaveGLJournalLine(gLJournalLine);
				httpStatus = postResult.HttpValidationStatusCode;
				if (postResult.APIValidationStatusCode == ResponseMessageBuilderFunctions.ResponseContentHeaderStatus.Success)
				{
					ERPGLJournalLineInformationDto eRPGLJournalLineInformationDto = await base.ERPGLJournalLineRepository.GetGLJournalLine(gLJournalLine.gllUniqueID);
					createdObject = new ERPGLJournalLineDto
					{
						gllArPaymentHeaderID = eRPGLJournalLineInformationDto.gllArPaymentHeaderID,
						gllArPaymentSessionID = eRPGLJournalLineInformationDto.gllArPaymentSessionID,
						gllCreatedBy = eRPGLJournalLineInformationDto.gllCreatedBy,
						gllCreatedDate = eRPGLJournalLineInformationDto.gllCreatedDate,
						gllCreditAmount = eRPGLJournalLineInformationDto.gllCreditAmount,
						gllDebitAmount = eRPGLJournalLineInformationDto.gllDebitAmount,
						gllDescription = eRPGLJournalLineInformationDto.gllDescription,
						gllUniqueID = eRPGLJournalLineInformationDto.gllUniqueID,
						gllGlAccountID = eRPGLJournalLineInformationDto.gllGlAccountID,
						gllGlFiscalYearID = eRPGLJournalLineInformationDto.gllGlFiscalYearID,
						gllGlFiscalYearPeriodID = eRPGLJournalLineInformationDto.gllGlFiscalYearPeriodID,
						gllGlJournalID = eRPGLJournalLineInformationDto.gllGlJournalID,
						gllPosted = eRPGLJournalLineInformationDto.gllPosted,
						gllJobAssemblyID = eRPGLJournalLineInformationDto.gllJobAssemblyID,
						gllJobID = eRPGLJournalLineInformationDto.gllJobID,
						gllJobMaterialComponentID = eRPGLJournalLineInformationDto.gllJobMaterialComponentID,
						gllJobMaterialID = eRPGLJournalLineInformationDto.gllJobMaterialID,
						gllJobOperationID = eRPGLJournalLineInformationDto.gllJobOperationID,
						gllLocationID = eRPGLJournalLineInformationDto.gllLocationID,
						gllOrganizationID = eRPGLJournalLineInformationDto.gllOrganizationID,
						gllPartTransactionID = eRPGLJournalLineInformationDto.gllPartTransactionID,
						gllReference = eRPGLJournalLineInformationDto.gllReference,
						gllRowVersion = eRPGLJournalLineInformationDto.gllRowVersion,
						gllGlJournalLineID = eRPGLJournalLineInformationDto.gllGlJournalLineID,
						gllSourceTableName = eRPGLJournalLineInformationDto.gllSourceTableName,
						gllSourceTableUniqueID = eRPGLJournalLineInformationDto.gllSourceTableUniqueID,
						gllTaxableAmount = eRPGLJournalLineInformationDto.gllTaxableAmount,
						gllTaxCodeID = eRPGLJournalLineInformationDto.gllTaxCodeID,
						gllTransactionAmount = eRPGLJournalLineInformationDto.gllTransactionAmount,
						gllTransactionDate = eRPGLJournalLineInformationDto.gllTransactionDate,
						gllTransactionType = eRPGLJournalLineInformationDto.gllTransactionType,
						CustomFields = eRPGLJournalLineInformationDto.CustomFields
					};
				}
				((List<string>)base.ErrorsList).AddRange(new List<string>(postResult.ErrorsList));
				((List<string>)base.WarningsList).AddRange(new List<string>(postResult.WarningsList));
			}
		}
		catch (Exception ex)
		{
			httpStatus = HttpStatusCode.InternalServerError;
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing GLJournalLine [{gLJournalLine.gllUniqueID}]");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = createdObject
			};
		}
		return result;
	}

	public async Task<APIValidationInfoDto> ValidateRequest_DeleteGLJournalLine(Guid gLJournalLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
		using (iERPGLJournalLineRepository)
		{
			if (!(await base.ERPGLJournalLineRepository.DoesGLJournalLineExist(gLJournalLineId)))
			{
				base.ErrorsList.Add($"GLJournalLine [{gLJournalLineId}] not found.");
				httpStatus = HttpStatusCode.NotFound;
			}
			else
			{
				ERPGLJournalLineInformationDto eRPGLJournalLineInformationDto = await base.ERPGLJournalLineRepository.GetGLJournalLine(gLJournalLineId);
				string text = await base.ERPGLJournalLineRepository.WhereUsed("GLJournalLines", new object[2] { eRPGLJournalLineInformationDto.gllGlJournalID, eRPGLJournalLineInformationDto.gllGlJournalLineID }, new object[2] { "gllGlJournalID", "gllGlJournalLineID" });
				if (text.Length > 0)
				{
					base.ErrorsList.Add("GLJournalLine cannot be deleted because it is used in the following places.\n [" + text.ToString().Trim() + "]");
					httpStatus = HttpStatusCode.BadRequest;
				}
			}
		}
		return new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
	}

	public async Task<ERPResponseMessageDto<ERPGLJournalLineDto>> Process_DeleteGLJournalLine(Guid gLJournalLineId)
	{
		base.ErrorsList = new List<string>();
		base.WarningsList = new List<string>();
		HttpStatusCode httpStatus = HttpStatusCode.OK;
		ERPResponseMessageDto<ERPGLJournalLineDto> result;
		try
		{
			IERPGLJournalLineRepository iERPGLJournalLineRepository = (base.ERPGLJournalLineRepository = new ERPGLJournalLineRepository(base.ApiClientContext));
			using (iERPGLJournalLineRepository)
			{
				APIValidationInfoDto aPIValidationInfoDto = await base.ERPGLJournalLineRepository.DeleteRowFromTable("GLJournalLines", "gll", gLJournalLineId);
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
			base.ErrorsList.Add($"Error occurred [{ex.Message}] while processing the delete of GLJournalLine [{gLJournalLineId}].");
		}
		finally
		{
			APIValidationInfoDto validationInfo = new APIValidationInfoDto(base.ErrorsList, base.WarningsList, httpStatus);
			result = new ERPResponseMessageDto<ERPGLJournalLineDto>
			{
				ValidationInfo = validationInfo,
				ReturnObject = new ERPGLJournalLineDto()
			};
		}
		return result;
	}
}
