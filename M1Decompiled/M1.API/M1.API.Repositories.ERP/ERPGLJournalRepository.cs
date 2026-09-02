using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.ERP;
using M1.API.Utilities;
using M1.Extensions;

namespace M1.API.Repositories.ERP;

public class ERPGLJournalRepository : APIBaseRepository, IERPGLJournalRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLJournalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLJournalExist(Guid gLJournalId)
	{
		InitializeParameterLists();
		base.filterList.Add("glpUniqueID|C", gLJournalId);
		base.selectList.Add("glpUniqueID");
		return Task.FromResult(GetAsObject("GLJournals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLJournalInformationDto>> GetAllGLJournals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLJournalInformationDto> collection = new List<ERPGLJournalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[38]
		{
			"glpApInvoiceID", "glpApPaymentHeaderID", "glpApPaymentSessionID", "glpArInvoiceID", "glpArPaymentHeaderID", "glpArPaymentSessionID", "glpAssetAdjustmentID", "glpAssetID", "glpBankStatementID", "glpCreatedBy",
			"glpCreatedDate", "glpDescription", "glpDetailSource", "glpDmrShipmentID", "glpUniqueID", "glpGlFiscalYearID", "glpGlFiscalYearPeriodID", "glpPosted", "glpReversingEntry", "glpJobAssemblyID",
			"glpJobID", "glpLandedCostID", "glpLocationID", "glpLongDescriptionRtf", "glpLongDescriptionText", "glpOrganizationID", "glpPostedDate", "glpReceiptID", "glpReference", "glpRmaReceiptID",
			"glpRowVersion", "glpGlJournalID", "glpShipmentID", "glpSource", "glpTimecardID", "glpTotalCredits", "glpTotalDebits", "glpTransactionDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLJournals");
		List<string> list = new List<string>();
		string[] fields = ((base.selectList.Count != array.Count()) ? base.selectList.ToArray() : array);
		if (orderBy != null && orderBy.Length > 0)
		{
			ParseAndAddOrderByFields(orderBy, list, fields);
		}
		if (list.Count == 0)
		{
			list = new List<string> { "1" };
		}
		if (filter != null && filter.Length != 0)
		{
			ParseAndAddFilter(filter, base.filterList, fields);
		}
		using (DataTable dataTable = GetAsDataTable("GLJournals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLJournalInformationDto eRPGLJournalInformationDto = new ERPGLJournalInformationDto();
				eRPGLJournalInformationDto.glpApInvoiceID = dataTable.Rows[i].Field<string>("glpApInvoiceID");
				eRPGLJournalInformationDto.glpApPaymentHeaderID = dataTable.Rows[i].Field<int>("glpApPaymentHeaderID");
				eRPGLJournalInformationDto.glpApPaymentSessionID = dataTable.Rows[i].Field<int>("glpApPaymentSessionID");
				eRPGLJournalInformationDto.glpArInvoiceID = dataTable.Rows[i].Field<string>("glpArInvoiceID");
				eRPGLJournalInformationDto.glpArPaymentHeaderID = dataTable.Rows[i].Field<int>("glpArPaymentHeaderID");
				eRPGLJournalInformationDto.glpArPaymentSessionID = dataTable.Rows[i].Field<int>("glpArPaymentSessionID");
				eRPGLJournalInformationDto.glpAssetAdjustmentID = dataTable.Rows[i].Field<int>("glpAssetAdjustmentID");
				eRPGLJournalInformationDto.glpAssetID = dataTable.Rows[i].Field<string>("glpAssetID");
				eRPGLJournalInformationDto.glpBankStatementID = dataTable.Rows[i].Field<int>("glpBankStatementID");
				eRPGLJournalInformationDto.glpCreatedBy = dataTable.Rows[i].Field<string>("glpCreatedBy");
				eRPGLJournalInformationDto.glpCreatedDate = dataTable.Rows[i].Field<DateTime?>("glpCreatedDate");
				eRPGLJournalInformationDto.glpDescription = dataTable.Rows[i].Field<string>("glpDescription");
				eRPGLJournalInformationDto.glpDetailSource = dataTable.Rows[i].Field<byte>("glpDetailSource");
				eRPGLJournalInformationDto.glpDmrShipmentID = dataTable.Rows[i].Field<string>("glpDmrShipmentID");
				eRPGLJournalInformationDto.glpUniqueID = dataTable.Rows[i].Field<Guid>("glpUniqueID");
				eRPGLJournalInformationDto.glpGlFiscalYearID = dataTable.Rows[i].Field<short>("glpGlFiscalYearID");
				eRPGLJournalInformationDto.glpGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("glpGlFiscalYearPeriodID");
				eRPGLJournalInformationDto.glpPosted = dataTable.Rows[i].Field<bool>("glpPosted");
				eRPGLJournalInformationDto.glpReversingEntry = dataTable.Rows[i].Field<bool>("glpReversingEntry");
				eRPGLJournalInformationDto.glpJobAssemblyID = dataTable.Rows[i].Field<int>("glpJobAssemblyID");
				eRPGLJournalInformationDto.glpJobID = dataTable.Rows[i].Field<string>("glpJobID");
				eRPGLJournalInformationDto.glpLandedCostID = dataTable.Rows[i].Field<string>("glpLandedCostID");
				eRPGLJournalInformationDto.glpLocationID = dataTable.Rows[i].Field<string>("glpLocationID");
				eRPGLJournalInformationDto.glpLongDescriptionRtf = dataTable.Rows[i].Field<string>("glpLongDescriptionRtf");
				eRPGLJournalInformationDto.glpLongDescriptionText = dataTable.Rows[i].Field<string>("glpLongDescriptionText");
				eRPGLJournalInformationDto.glpOrganizationID = dataTable.Rows[i].Field<string>("glpOrganizationID");
				eRPGLJournalInformationDto.glpPostedDate = dataTable.Rows[i].Field<DateTime?>("glpPostedDate");
				eRPGLJournalInformationDto.glpReceiptID = dataTable.Rows[i].Field<string>("glpReceiptID");
				eRPGLJournalInformationDto.glpReference = dataTable.Rows[i].Field<string>("glpReference");
				eRPGLJournalInformationDto.glpRmaReceiptID = dataTable.Rows[i].Field<string>("glpRmaReceiptID");
				eRPGLJournalInformationDto.glpRowVersion = dataTable.Rows[i].Field<byte[]>("glpRowVersion");
				eRPGLJournalInformationDto.glpGlJournalID = dataTable.Rows[i].Field<int>("glpGlJournalID");
				eRPGLJournalInformationDto.glpShipmentID = dataTable.Rows[i].Field<string>("glpShipmentID");
				eRPGLJournalInformationDto.glpSource = dataTable.Rows[i].Field<byte>("glpSource");
				eRPGLJournalInformationDto.glpTimecardID = dataTable.Rows[i].Field<int>("glpTimecardID");
				eRPGLJournalInformationDto.glpTotalCredits = dataTable.Rows[i].Field<decimal>("glpTotalCredits");
				eRPGLJournalInformationDto.glpTotalDebits = dataTable.Rows[i].Field<decimal>("glpTotalDebits");
				eRPGLJournalInformationDto.glpTransactionDate = dataTable.Rows[i].Field<DateTime?>("glpTransactionDate");
				eRPGLJournalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLJournalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLJournalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLJournalInformationDto> GetGLJournal(Guid gLJournalId)
	{
		ERPGLJournalInformationDto eRPGLJournalInformationDto = new ERPGLJournalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[38]
		{
			"glpApInvoiceID", "glpApPaymentHeaderID", "glpApPaymentSessionID", "glpArInvoiceID", "glpArPaymentHeaderID", "glpArPaymentSessionID", "glpAssetAdjustmentID", "glpAssetID", "glpBankStatementID", "glpCreatedBy",
			"glpCreatedDate", "glpDescription", "glpDetailSource", "glpDmrShipmentID", "glpUniqueID", "glpGlFiscalYearID", "glpGlFiscalYearPeriodID", "glpPosted", "glpReversingEntry", "glpJobAssemblyID",
			"glpJobID", "glpLandedCostID", "glpLocationID", "glpLongDescriptionRtf", "glpLongDescriptionText", "glpOrganizationID", "glpPostedDate", "glpReceiptID", "glpReference", "glpRmaReceiptID",
			"glpRowVersion", "glpGlJournalID", "glpShipmentID", "glpSource", "glpTimecardID", "glpTotalCredits", "glpTotalDebits", "glpTransactionDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glpUniqueID|C", gLJournalId);
		AddCustomFieldsToSelectList("GLJournals");
		using (DataTable dataTable = GetAsDataTable("GLJournals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLJournalInformationDto);
			}
			eRPGLJournalInformationDto.glpApInvoiceID = dataTable.Rows[0].Field<string>("glpApInvoiceID");
			eRPGLJournalInformationDto.glpApPaymentHeaderID = dataTable.Rows[0].Field<int>("glpApPaymentHeaderID");
			eRPGLJournalInformationDto.glpApPaymentSessionID = dataTable.Rows[0].Field<int>("glpApPaymentSessionID");
			eRPGLJournalInformationDto.glpArInvoiceID = dataTable.Rows[0].Field<string>("glpArInvoiceID");
			eRPGLJournalInformationDto.glpArPaymentHeaderID = dataTable.Rows[0].Field<int>("glpArPaymentHeaderID");
			eRPGLJournalInformationDto.glpArPaymentSessionID = dataTable.Rows[0].Field<int>("glpArPaymentSessionID");
			eRPGLJournalInformationDto.glpAssetAdjustmentID = dataTable.Rows[0].Field<int>("glpAssetAdjustmentID");
			eRPGLJournalInformationDto.glpAssetID = dataTable.Rows[0].Field<string>("glpAssetID");
			eRPGLJournalInformationDto.glpBankStatementID = dataTable.Rows[0].Field<int>("glpBankStatementID");
			eRPGLJournalInformationDto.glpCreatedBy = dataTable.Rows[0].Field<string>("glpCreatedBy");
			eRPGLJournalInformationDto.glpCreatedDate = dataTable.Rows[0].Field<DateTime?>("glpCreatedDate");
			eRPGLJournalInformationDto.glpDescription = dataTable.Rows[0].Field<string>("glpDescription");
			eRPGLJournalInformationDto.glpDetailSource = dataTable.Rows[0].Field<byte>("glpDetailSource");
			eRPGLJournalInformationDto.glpDmrShipmentID = dataTable.Rows[0].Field<string>("glpDmrShipmentID");
			eRPGLJournalInformationDto.glpUniqueID = dataTable.Rows[0].Field<Guid>("glpUniqueID");
			eRPGLJournalInformationDto.glpGlFiscalYearID = dataTable.Rows[0].Field<short>("glpGlFiscalYearID");
			eRPGLJournalInformationDto.glpGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("glpGlFiscalYearPeriodID");
			eRPGLJournalInformationDto.glpPosted = dataTable.Rows[0].Field<bool>("glpPosted");
			eRPGLJournalInformationDto.glpReversingEntry = dataTable.Rows[0].Field<bool>("glpReversingEntry");
			eRPGLJournalInformationDto.glpJobAssemblyID = dataTable.Rows[0].Field<int>("glpJobAssemblyID");
			eRPGLJournalInformationDto.glpJobID = dataTable.Rows[0].Field<string>("glpJobID");
			eRPGLJournalInformationDto.glpLandedCostID = dataTable.Rows[0].Field<string>("glpLandedCostID");
			eRPGLJournalInformationDto.glpLocationID = dataTable.Rows[0].Field<string>("glpLocationID");
			eRPGLJournalInformationDto.glpLongDescriptionRtf = dataTable.Rows[0].Field<string>("glpLongDescriptionRtf");
			eRPGLJournalInformationDto.glpLongDescriptionText = dataTable.Rows[0].Field<string>("glpLongDescriptionText");
			eRPGLJournalInformationDto.glpOrganizationID = dataTable.Rows[0].Field<string>("glpOrganizationID");
			eRPGLJournalInformationDto.glpPostedDate = dataTable.Rows[0].Field<DateTime?>("glpPostedDate");
			eRPGLJournalInformationDto.glpReceiptID = dataTable.Rows[0].Field<string>("glpReceiptID");
			eRPGLJournalInformationDto.glpReference = dataTable.Rows[0].Field<string>("glpReference");
			eRPGLJournalInformationDto.glpRmaReceiptID = dataTable.Rows[0].Field<string>("glpRmaReceiptID");
			eRPGLJournalInformationDto.glpRowVersion = dataTable.Rows[0].Field<byte[]>("glpRowVersion");
			eRPGLJournalInformationDto.glpGlJournalID = dataTable.Rows[0].Field<int>("glpGlJournalID");
			eRPGLJournalInformationDto.glpShipmentID = dataTable.Rows[0].Field<string>("glpShipmentID");
			eRPGLJournalInformationDto.glpSource = dataTable.Rows[0].Field<byte>("glpSource");
			eRPGLJournalInformationDto.glpTimecardID = dataTable.Rows[0].Field<int>("glpTimecardID");
			eRPGLJournalInformationDto.glpTotalCredits = dataTable.Rows[0].Field<decimal>("glpTotalCredits");
			eRPGLJournalInformationDto.glpTotalDebits = dataTable.Rows[0].Field<decimal>("glpTotalDebits");
			eRPGLJournalInformationDto.glpTransactionDate = dataTable.Rows[0].Field<DateTime?>("glpTransactionDate");
			eRPGLJournalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLJournalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLJournalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLJournal(ERPGLJournalDto gLJournal)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLJournals WHERE glpUniqueID = " + M1Util.ConvertToLinq(gLJournal.glpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glpGlJournalID"] = gLJournal.glpGlJournalID;
				gLJournal.glpUniqueID = ((gLJournal.glpUniqueID == Guid.Empty) ? Guid.NewGuid() : gLJournal.glpUniqueID);
				dataRow["glpUniqueID"] = gLJournal.glpUniqueID;
				dataRow["glpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLJournal could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLJournal.glpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLJournal is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glpRowVersion"], gLJournal.glpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLJournal has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLJournal again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glpApInvoiceID"] = gLJournal.glpApInvoiceID;
			dataRow["glpApPaymentHeaderID"] = gLJournal.glpApPaymentHeaderID;
			dataRow["glpApPaymentSessionID"] = gLJournal.glpApPaymentSessionID;
			dataRow["glpArInvoiceID"] = gLJournal.glpArInvoiceID;
			dataRow["glpArPaymentHeaderID"] = gLJournal.glpArPaymentHeaderID;
			dataRow["glpArPaymentSessionID"] = gLJournal.glpArPaymentSessionID;
			dataRow["glpAssetAdjustmentID"] = gLJournal.glpAssetAdjustmentID;
			dataRow["glpAssetID"] = gLJournal.glpAssetID;
			dataRow["glpBankStatementID"] = gLJournal.glpBankStatementID;
			dataRow["glpDescription"] = gLJournal.glpDescription;
			dataRow["glpDetailSource"] = gLJournal.glpDetailSource;
			dataRow["glpDmrShipmentID"] = gLJournal.glpDmrShipmentID;
			dataRow["glpGlFiscalYearID"] = gLJournal.glpGlFiscalYearID;
			dataRow["glpGlFiscalYearPeriodID"] = gLJournal.glpGlFiscalYearPeriodID;
			dataRow["glpPosted"] = gLJournal.glpPosted;
			dataRow["glpReversingEntry"] = gLJournal.glpReversingEntry;
			dataRow["glpJobAssemblyID"] = gLJournal.glpJobAssemblyID;
			dataRow["glpJobID"] = gLJournal.glpJobID;
			dataRow["glpLandedCostID"] = gLJournal.glpLandedCostID;
			dataRow["glpLocationID"] = gLJournal.glpLocationID;
			dataRow["glpLongDescriptionRtf"] = gLJournal.glpLongDescriptionRtf ?? dataRow["glpLongDescriptionRtf"];
			dataRow["glpLongDescriptionText"] = gLJournal.glpLongDescriptionText ?? dataRow["glpLongDescriptionText"];
			dataRow["glpOrganizationID"] = gLJournal.glpOrganizationID;
			DataRow dataRow2 = dataRow;
			DateTime? glpPostedDate = gLJournal.glpPostedDate;
			dataRow2["glpPostedDate"] = (glpPostedDate.HasValue ? ((object)glpPostedDate.GetValueOrDefault()) : dataRow["glpPostedDate"]);
			dataRow["glpReceiptID"] = gLJournal.glpReceiptID;
			dataRow["glpReference"] = gLJournal.glpReference;
			dataRow["glpRmaReceiptID"] = gLJournal.glpRmaReceiptID;
			dataRow["glpShipmentID"] = gLJournal.glpShipmentID;
			dataRow["glpSource"] = gLJournal.glpSource;
			dataRow["glpTimecardID"] = gLJournal.glpTimecardID;
			dataRow["glpTotalCredits"] = gLJournal.glpTotalCredits;
			dataRow["glpTotalDebits"] = gLJournal.glpTotalDebits;
			DataRow dataRow3 = dataRow;
			glpPostedDate = gLJournal.glpTransactionDate;
			dataRow3["glpTransactionDate"] = (glpPostedDate.HasValue ? ((object)glpPostedDate.GetValueOrDefault()) : dataRow["glpTransactionDate"]);
			if (gLJournal.CustomFields != null && gLJournal.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLJournal.CustomFields)
				{
					if (dataTable.Columns.Contains(customField.Key))
					{
						dataRow[customField.Key] = customField.Value;
					}
				}
			}
			dataRow.EndEdit();
			if (flag)
			{
				dataTable.Rows.Add(dataRow);
			}
			if (base.M1database.UpdateData(dataTable, adapter))
			{
				if (flag)
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Created;
				}
				else
				{
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.OK;
				}
			}
			else
			{
				aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.InternalServerError;
			}
		}
		catch (SqlException ex)
		{
			SqlErrorResult httpStatusCodeForSqlException = SqlExceptionMapper.GetHttpStatusCodeForSqlException(ex);
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLJournal [{gLJournal.glpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLJournal [{gLJournal.glpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
