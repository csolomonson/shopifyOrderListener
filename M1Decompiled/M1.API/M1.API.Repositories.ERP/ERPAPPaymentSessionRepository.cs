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

public class ERPAPPaymentSessionRepository : APIBaseRepository, IERPAPPaymentSessionRepository, IAPIBaseRepository, IDisposable
{
	public ERPAPPaymentSessionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAPPaymentSessionExist(Guid aPPaymentSessionId)
	{
		InitializeParameterLists();
		base.filterList.Add("apsUniqueID|C", aPPaymentSessionId);
		base.selectList.Add("apsUniqueID");
		return Task.FromResult(GetAsObject("APPaymentSessions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAPPaymentSessionInformationDto>> GetAllAPPaymentSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAPPaymentSessionInformationDto> collection = new List<ERPAPPaymentSessionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[29]
		{
			"apsApGlAccountID", "apsArGlAccountID", "apsBankAccountID", "apsCashGlAccountID", "apsCompletedDate", "apsCreatedBy", "apsCreatedDate", "apsCurrencyRateID", "apsEftDescription", "apsEftReferenceNumber",
			"apsEftSettlementDate", "apsUniqueID", "apsExchangeRate", "apsGlFiscalYearID", "apsGlFiscalYearPeriodID", "apsCompleted", "apsCustomRate", "apsOpenPaymentLoad", "apsPaymentsPrinted", "apsPostedToGl",
			"apsPaymentAmount", "apsPaymentAmountForeign", "apsPaymentDate", "apsPlantDepartmentID", "apsPlantID", "apsPostedDate", "apsRowVersion", "apsApPaymentSessionID", "apsSessionType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("APPaymentSessions");
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
		using (DataTable dataTable = GetAsDataTable("APPaymentSessions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAPPaymentSessionInformationDto eRPAPPaymentSessionInformationDto = new ERPAPPaymentSessionInformationDto();
				eRPAPPaymentSessionInformationDto.apsApGlAccountID = dataTable.Rows[i].Field<string>("apsApGlAccountID");
				eRPAPPaymentSessionInformationDto.apsArGlAccountID = dataTable.Rows[i].Field<string>("apsArGlAccountID");
				eRPAPPaymentSessionInformationDto.apsBankAccountID = dataTable.Rows[i].Field<string>("apsBankAccountID");
				eRPAPPaymentSessionInformationDto.apsCashGlAccountID = dataTable.Rows[i].Field<string>("apsCashGlAccountID");
				eRPAPPaymentSessionInformationDto.apsCompletedDate = dataTable.Rows[i].Field<DateTime?>("apsCompletedDate");
				eRPAPPaymentSessionInformationDto.apsCreatedBy = dataTable.Rows[i].Field<string>("apsCreatedBy");
				eRPAPPaymentSessionInformationDto.apsCreatedDate = dataTable.Rows[i].Field<DateTime?>("apsCreatedDate");
				eRPAPPaymentSessionInformationDto.apsCurrencyRateID = dataTable.Rows[i].Field<string>("apsCurrencyRateID");
				eRPAPPaymentSessionInformationDto.apsEftDescription = dataTable.Rows[i].Field<string>("apsEftDescription");
				eRPAPPaymentSessionInformationDto.apsEftReferenceNumber = dataTable.Rows[i].Field<string>("apsEftReferenceNumber");
				eRPAPPaymentSessionInformationDto.apsEftSettlementDate = dataTable.Rows[i].Field<DateTime?>("apsEftSettlementDate");
				eRPAPPaymentSessionInformationDto.apsUniqueID = dataTable.Rows[i].Field<Guid>("apsUniqueID");
				eRPAPPaymentSessionInformationDto.apsExchangeRate = dataTable.Rows[i].Field<decimal>("apsExchangeRate");
				eRPAPPaymentSessionInformationDto.apsGlFiscalYearID = dataTable.Rows[i].Field<short>("apsGlFiscalYearID");
				eRPAPPaymentSessionInformationDto.apsGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("apsGlFiscalYearPeriodID");
				eRPAPPaymentSessionInformationDto.apsCompleted = dataTable.Rows[i].Field<bool>("apsCompleted");
				eRPAPPaymentSessionInformationDto.apsCustomRate = dataTable.Rows[i].Field<bool>("apsCustomRate");
				eRPAPPaymentSessionInformationDto.apsOpenPaymentLoad = dataTable.Rows[i].Field<bool>("apsOpenPaymentLoad");
				eRPAPPaymentSessionInformationDto.apsPaymentsPrinted = dataTable.Rows[i].Field<bool>("apsPaymentsPrinted");
				eRPAPPaymentSessionInformationDto.apsPostedToGl = dataTable.Rows[i].Field<bool>("apsPostedToGl");
				eRPAPPaymentSessionInformationDto.apsPaymentAmount = dataTable.Rows[i].Field<decimal>("apsPaymentAmount");
				eRPAPPaymentSessionInformationDto.apsPaymentAmountForeign = dataTable.Rows[i].Field<decimal>("apsPaymentAmountForeign");
				eRPAPPaymentSessionInformationDto.apsPaymentDate = dataTable.Rows[i].Field<DateTime?>("apsPaymentDate");
				eRPAPPaymentSessionInformationDto.apsPlantDepartmentID = dataTable.Rows[i].Field<string>("apsPlantDepartmentID");
				eRPAPPaymentSessionInformationDto.apsPlantID = dataTable.Rows[i].Field<string>("apsPlantID");
				eRPAPPaymentSessionInformationDto.apsPostedDate = dataTable.Rows[i].Field<DateTime?>("apsPostedDate");
				eRPAPPaymentSessionInformationDto.apsRowVersion = dataTable.Rows[i].Field<byte[]>("apsRowVersion");
				eRPAPPaymentSessionInformationDto.apsApPaymentSessionID = dataTable.Rows[i].Field<int>("apsApPaymentSessionID");
				eRPAPPaymentSessionInformationDto.apsSessionType = dataTable.Rows[i].Field<byte>("apsSessionType");
				eRPAPPaymentSessionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAPPaymentSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAPPaymentSessionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAPPaymentSessionInformationDto> GetAPPaymentSession(Guid aPPaymentSessionId)
	{
		ERPAPPaymentSessionInformationDto eRPAPPaymentSessionInformationDto = new ERPAPPaymentSessionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[29]
		{
			"apsApGlAccountID", "apsArGlAccountID", "apsBankAccountID", "apsCashGlAccountID", "apsCompletedDate", "apsCreatedBy", "apsCreatedDate", "apsCurrencyRateID", "apsEftDescription", "apsEftReferenceNumber",
			"apsEftSettlementDate", "apsUniqueID", "apsExchangeRate", "apsGlFiscalYearID", "apsGlFiscalYearPeriodID", "apsCompleted", "apsCustomRate", "apsOpenPaymentLoad", "apsPaymentsPrinted", "apsPostedToGl",
			"apsPaymentAmount", "apsPaymentAmountForeign", "apsPaymentDate", "apsPlantDepartmentID", "apsPlantID", "apsPostedDate", "apsRowVersion", "apsApPaymentSessionID", "apsSessionType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("apsUniqueID|C", aPPaymentSessionId);
		AddCustomFieldsToSelectList("APPaymentSessions");
		using (DataTable dataTable = GetAsDataTable("APPaymentSessions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAPPaymentSessionInformationDto);
			}
			eRPAPPaymentSessionInformationDto.apsApGlAccountID = dataTable.Rows[0].Field<string>("apsApGlAccountID");
			eRPAPPaymentSessionInformationDto.apsArGlAccountID = dataTable.Rows[0].Field<string>("apsArGlAccountID");
			eRPAPPaymentSessionInformationDto.apsBankAccountID = dataTable.Rows[0].Field<string>("apsBankAccountID");
			eRPAPPaymentSessionInformationDto.apsCashGlAccountID = dataTable.Rows[0].Field<string>("apsCashGlAccountID");
			eRPAPPaymentSessionInformationDto.apsCompletedDate = dataTable.Rows[0].Field<DateTime?>("apsCompletedDate");
			eRPAPPaymentSessionInformationDto.apsCreatedBy = dataTable.Rows[0].Field<string>("apsCreatedBy");
			eRPAPPaymentSessionInformationDto.apsCreatedDate = dataTable.Rows[0].Field<DateTime?>("apsCreatedDate");
			eRPAPPaymentSessionInformationDto.apsCurrencyRateID = dataTable.Rows[0].Field<string>("apsCurrencyRateID");
			eRPAPPaymentSessionInformationDto.apsEftDescription = dataTable.Rows[0].Field<string>("apsEftDescription");
			eRPAPPaymentSessionInformationDto.apsEftReferenceNumber = dataTable.Rows[0].Field<string>("apsEftReferenceNumber");
			eRPAPPaymentSessionInformationDto.apsEftSettlementDate = dataTable.Rows[0].Field<DateTime?>("apsEftSettlementDate");
			eRPAPPaymentSessionInformationDto.apsUniqueID = dataTable.Rows[0].Field<Guid>("apsUniqueID");
			eRPAPPaymentSessionInformationDto.apsExchangeRate = dataTable.Rows[0].Field<decimal>("apsExchangeRate");
			eRPAPPaymentSessionInformationDto.apsGlFiscalYearID = dataTable.Rows[0].Field<short>("apsGlFiscalYearID");
			eRPAPPaymentSessionInformationDto.apsGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("apsGlFiscalYearPeriodID");
			eRPAPPaymentSessionInformationDto.apsCompleted = dataTable.Rows[0].Field<bool>("apsCompleted");
			eRPAPPaymentSessionInformationDto.apsCustomRate = dataTable.Rows[0].Field<bool>("apsCustomRate");
			eRPAPPaymentSessionInformationDto.apsOpenPaymentLoad = dataTable.Rows[0].Field<bool>("apsOpenPaymentLoad");
			eRPAPPaymentSessionInformationDto.apsPaymentsPrinted = dataTable.Rows[0].Field<bool>("apsPaymentsPrinted");
			eRPAPPaymentSessionInformationDto.apsPostedToGl = dataTable.Rows[0].Field<bool>("apsPostedToGl");
			eRPAPPaymentSessionInformationDto.apsPaymentAmount = dataTable.Rows[0].Field<decimal>("apsPaymentAmount");
			eRPAPPaymentSessionInformationDto.apsPaymentAmountForeign = dataTable.Rows[0].Field<decimal>("apsPaymentAmountForeign");
			eRPAPPaymentSessionInformationDto.apsPaymentDate = dataTable.Rows[0].Field<DateTime?>("apsPaymentDate");
			eRPAPPaymentSessionInformationDto.apsPlantDepartmentID = dataTable.Rows[0].Field<string>("apsPlantDepartmentID");
			eRPAPPaymentSessionInformationDto.apsPlantID = dataTable.Rows[0].Field<string>("apsPlantID");
			eRPAPPaymentSessionInformationDto.apsPostedDate = dataTable.Rows[0].Field<DateTime?>("apsPostedDate");
			eRPAPPaymentSessionInformationDto.apsRowVersion = dataTable.Rows[0].Field<byte[]>("apsRowVersion");
			eRPAPPaymentSessionInformationDto.apsApPaymentSessionID = dataTable.Rows[0].Field<int>("apsApPaymentSessionID");
			eRPAPPaymentSessionInformationDto.apsSessionType = dataTable.Rows[0].Field<byte>("apsSessionType");
			eRPAPPaymentSessionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAPPaymentSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAPPaymentSessionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAPPaymentSession(ERPAPPaymentSessionDto aPPaymentSession)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM APPaymentSessions WHERE apsUniqueID = " + M1Util.ConvertToLinq(aPPaymentSession.apsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["apsApPaymentSessionID"] = aPPaymentSession.apsApPaymentSessionID;
				aPPaymentSession.apsUniqueID = ((aPPaymentSession.apsUniqueID == Guid.Empty) ? Guid.NewGuid() : aPPaymentSession.apsUniqueID);
				dataRow["apsUniqueID"] = aPPaymentSession.apsUniqueID;
				dataRow["apsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["apsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The APPaymentSession could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aPPaymentSession.apsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the APPaymentSession is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["apsRowVersion"], aPPaymentSession.apsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the APPaymentSession has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the APPaymentSession again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["apsApGlAccountID"] = aPPaymentSession.apsApGlAccountID;
			dataRow["apsArGlAccountID"] = aPPaymentSession.apsArGlAccountID;
			dataRow["apsBankAccountID"] = aPPaymentSession.apsBankAccountID;
			dataRow["apsCashGlAccountID"] = aPPaymentSession.apsCashGlAccountID;
			DataRow dataRow2 = dataRow;
			DateTime? apsCompletedDate = aPPaymentSession.apsCompletedDate;
			dataRow2["apsCompletedDate"] = (apsCompletedDate.HasValue ? ((object)apsCompletedDate.GetValueOrDefault()) : dataRow["apsCompletedDate"]);
			dataRow["apsCurrencyRateID"] = aPPaymentSession.apsCurrencyRateID;
			dataRow["apsEftDescription"] = aPPaymentSession.apsEftDescription;
			dataRow["apsEftReferenceNumber"] = aPPaymentSession.apsEftReferenceNumber;
			DataRow dataRow3 = dataRow;
			apsCompletedDate = aPPaymentSession.apsEftSettlementDate;
			dataRow3["apsEftSettlementDate"] = (apsCompletedDate.HasValue ? ((object)apsCompletedDate.GetValueOrDefault()) : dataRow["apsEftSettlementDate"]);
			dataRow["apsExchangeRate"] = aPPaymentSession.apsExchangeRate;
			dataRow["apsGlFiscalYearID"] = aPPaymentSession.apsGlFiscalYearID;
			dataRow["apsGlFiscalYearPeriodID"] = aPPaymentSession.apsGlFiscalYearPeriodID;
			dataRow["apsCompleted"] = aPPaymentSession.apsCompleted;
			dataRow["apsCustomRate"] = aPPaymentSession.apsCustomRate;
			dataRow["apsOpenPaymentLoad"] = aPPaymentSession.apsOpenPaymentLoad;
			dataRow["apsPaymentsPrinted"] = aPPaymentSession.apsPaymentsPrinted;
			dataRow["apsPostedToGl"] = aPPaymentSession.apsPostedToGl;
			dataRow["apsPaymentAmount"] = aPPaymentSession.apsPaymentAmount;
			dataRow["apsPaymentAmountForeign"] = aPPaymentSession.apsPaymentAmountForeign;
			DataRow dataRow4 = dataRow;
			apsCompletedDate = aPPaymentSession.apsPaymentDate;
			dataRow4["apsPaymentDate"] = (apsCompletedDate.HasValue ? ((object)apsCompletedDate.GetValueOrDefault()) : dataRow["apsPaymentDate"]);
			dataRow["apsPlantDepartmentID"] = aPPaymentSession.apsPlantDepartmentID;
			dataRow["apsPlantID"] = aPPaymentSession.apsPlantID;
			DataRow dataRow5 = dataRow;
			apsCompletedDate = aPPaymentSession.apsPostedDate;
			dataRow5["apsPostedDate"] = (apsCompletedDate.HasValue ? ((object)apsCompletedDate.GetValueOrDefault()) : dataRow["apsPostedDate"]);
			dataRow["apsSessionType"] = aPPaymentSession.apsSessionType;
			if (aPPaymentSession.CustomFields != null && aPPaymentSession.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aPPaymentSession.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the APPaymentSession [{aPPaymentSession.apsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the APPaymentSession [{aPPaymentSession.apsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
