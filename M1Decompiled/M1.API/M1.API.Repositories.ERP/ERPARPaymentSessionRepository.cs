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

public class ERPARPaymentSessionRepository : APIBaseRepository, IERPARPaymentSessionRepository, IAPIBaseRepository, IDisposable
{
	public ERPARPaymentSessionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARPaymentSessionExist(Guid aRPaymentSessionId)
	{
		InitializeParameterLists();
		base.filterList.Add("arsUniqueID|C", aRPaymentSessionId);
		base.selectList.Add("arsUniqueID");
		return Task.FromResult(GetAsObject("ARPaymentSessions", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARPaymentSessionInformationDto>> GetAllARPaymentSessions(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARPaymentSessionInformationDto> collection = new List<ERPARPaymentSessionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[28]
		{
			"arsApDiscountGlAccountID", "arsApGlAccountID", "arsArGlAccountID", "arsBankAccountID", "arsCashGlAccountID", "arsCreatedBy", "arsCreatedDate", "arsCurrencyRateID", "arsDepositAmount", "arsDepositAmountForeign",
			"arsDiscountGlAccountID", "arsUniqueID", "arsExchangeRate", "arsGlFiscalYearID", "arsGlFiscalYearPeriodID", "arsAvalaraTaxCalculated", "arsCustomRate", "arsGroupBySettlement", "arsOpenPaymentLoad", "arsPostedToGl",
			"arsPlantDepartmentID", "arsPlantID", "arsPostedDate", "arsReceiptDate", "arsRowVersion", "arsArPaymentSessionID", "arsSettlementEndTime", "arsSettlementStartTime"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARPaymentSessions");
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
		using (DataTable dataTable = GetAsDataTable("ARPaymentSessions", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARPaymentSessionInformationDto eRPARPaymentSessionInformationDto = new ERPARPaymentSessionInformationDto();
				eRPARPaymentSessionInformationDto.arsApDiscountGlAccountID = dataTable.Rows[i].Field<string>("arsApDiscountGlAccountID");
				eRPARPaymentSessionInformationDto.arsApGlAccountID = dataTable.Rows[i].Field<string>("arsApGlAccountID");
				eRPARPaymentSessionInformationDto.arsArGlAccountID = dataTable.Rows[i].Field<string>("arsArGlAccountID");
				eRPARPaymentSessionInformationDto.arsBankAccountID = dataTable.Rows[i].Field<string>("arsBankAccountID");
				eRPARPaymentSessionInformationDto.arsCashGlAccountID = dataTable.Rows[i].Field<string>("arsCashGlAccountID");
				eRPARPaymentSessionInformationDto.arsCreatedBy = dataTable.Rows[i].Field<string>("arsCreatedBy");
				eRPARPaymentSessionInformationDto.arsCreatedDate = dataTable.Rows[i].Field<DateTime?>("arsCreatedDate");
				eRPARPaymentSessionInformationDto.arsCurrencyRateID = dataTable.Rows[i].Field<string>("arsCurrencyRateID");
				eRPARPaymentSessionInformationDto.arsDepositAmount = dataTable.Rows[i].Field<decimal>("arsDepositAmount");
				eRPARPaymentSessionInformationDto.arsDepositAmountForeign = dataTable.Rows[i].Field<decimal>("arsDepositAmountForeign");
				eRPARPaymentSessionInformationDto.arsDiscountGlAccountID = dataTable.Rows[i].Field<string>("arsDiscountGlAccountID");
				eRPARPaymentSessionInformationDto.arsUniqueID = dataTable.Rows[i].Field<Guid>("arsUniqueID");
				eRPARPaymentSessionInformationDto.arsExchangeRate = dataTable.Rows[i].Field<decimal>("arsExchangeRate");
				eRPARPaymentSessionInformationDto.arsGlFiscalYearID = dataTable.Rows[i].Field<short>("arsGlFiscalYearID");
				eRPARPaymentSessionInformationDto.arsGlFiscalYearPeriodID = dataTable.Rows[i].Field<byte>("arsGlFiscalYearPeriodID");
				eRPARPaymentSessionInformationDto.arsAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("arsAvalaraTaxCalculated");
				eRPARPaymentSessionInformationDto.arsCustomRate = dataTable.Rows[i].Field<bool>("arsCustomRate");
				eRPARPaymentSessionInformationDto.arsGroupBySettlement = dataTable.Rows[i].Field<bool>("arsGroupBySettlement");
				eRPARPaymentSessionInformationDto.arsOpenPaymentLoad = dataTable.Rows[i].Field<bool>("arsOpenPaymentLoad");
				eRPARPaymentSessionInformationDto.arsPostedToGl = dataTable.Rows[i].Field<bool>("arsPostedToGl");
				eRPARPaymentSessionInformationDto.arsPlantDepartmentID = dataTable.Rows[i].Field<string>("arsPlantDepartmentID");
				eRPARPaymentSessionInformationDto.arsPlantID = dataTable.Rows[i].Field<string>("arsPlantID");
				eRPARPaymentSessionInformationDto.arsPostedDate = dataTable.Rows[i].Field<DateTime?>("arsPostedDate");
				eRPARPaymentSessionInformationDto.arsReceiptDate = dataTable.Rows[i].Field<DateTime?>("arsReceiptDate");
				eRPARPaymentSessionInformationDto.arsRowVersion = dataTable.Rows[i].Field<byte[]>("arsRowVersion");
				eRPARPaymentSessionInformationDto.arsArPaymentSessionID = dataTable.Rows[i].Field<int>("arsArPaymentSessionID");
				eRPARPaymentSessionInformationDto.arsSettlementEndTime = dataTable.Rows[i].Field<DateTime?>("arsSettlementEndTime");
				eRPARPaymentSessionInformationDto.arsSettlementStartTime = dataTable.Rows[i].Field<DateTime?>("arsSettlementStartTime");
				eRPARPaymentSessionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARPaymentSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARPaymentSessionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARPaymentSessionInformationDto> GetARPaymentSession(Guid aRPaymentSessionId)
	{
		ERPARPaymentSessionInformationDto eRPARPaymentSessionInformationDto = new ERPARPaymentSessionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[28]
		{
			"arsApDiscountGlAccountID", "arsApGlAccountID", "arsArGlAccountID", "arsBankAccountID", "arsCashGlAccountID", "arsCreatedBy", "arsCreatedDate", "arsCurrencyRateID", "arsDepositAmount", "arsDepositAmountForeign",
			"arsDiscountGlAccountID", "arsUniqueID", "arsExchangeRate", "arsGlFiscalYearID", "arsGlFiscalYearPeriodID", "arsAvalaraTaxCalculated", "arsCustomRate", "arsGroupBySettlement", "arsOpenPaymentLoad", "arsPostedToGl",
			"arsPlantDepartmentID", "arsPlantID", "arsPostedDate", "arsReceiptDate", "arsRowVersion", "arsArPaymentSessionID", "arsSettlementEndTime", "arsSettlementStartTime"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("arsUniqueID|C", aRPaymentSessionId);
		AddCustomFieldsToSelectList("ARPaymentSessions");
		using (DataTable dataTable = GetAsDataTable("ARPaymentSessions", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARPaymentSessionInformationDto);
			}
			eRPARPaymentSessionInformationDto.arsApDiscountGlAccountID = dataTable.Rows[0].Field<string>("arsApDiscountGlAccountID");
			eRPARPaymentSessionInformationDto.arsApGlAccountID = dataTable.Rows[0].Field<string>("arsApGlAccountID");
			eRPARPaymentSessionInformationDto.arsArGlAccountID = dataTable.Rows[0].Field<string>("arsArGlAccountID");
			eRPARPaymentSessionInformationDto.arsBankAccountID = dataTable.Rows[0].Field<string>("arsBankAccountID");
			eRPARPaymentSessionInformationDto.arsCashGlAccountID = dataTable.Rows[0].Field<string>("arsCashGlAccountID");
			eRPARPaymentSessionInformationDto.arsCreatedBy = dataTable.Rows[0].Field<string>("arsCreatedBy");
			eRPARPaymentSessionInformationDto.arsCreatedDate = dataTable.Rows[0].Field<DateTime?>("arsCreatedDate");
			eRPARPaymentSessionInformationDto.arsCurrencyRateID = dataTable.Rows[0].Field<string>("arsCurrencyRateID");
			eRPARPaymentSessionInformationDto.arsDepositAmount = dataTable.Rows[0].Field<decimal>("arsDepositAmount");
			eRPARPaymentSessionInformationDto.arsDepositAmountForeign = dataTable.Rows[0].Field<decimal>("arsDepositAmountForeign");
			eRPARPaymentSessionInformationDto.arsDiscountGlAccountID = dataTable.Rows[0].Field<string>("arsDiscountGlAccountID");
			eRPARPaymentSessionInformationDto.arsUniqueID = dataTable.Rows[0].Field<Guid>("arsUniqueID");
			eRPARPaymentSessionInformationDto.arsExchangeRate = dataTable.Rows[0].Field<decimal>("arsExchangeRate");
			eRPARPaymentSessionInformationDto.arsGlFiscalYearID = dataTable.Rows[0].Field<short>("arsGlFiscalYearID");
			eRPARPaymentSessionInformationDto.arsGlFiscalYearPeriodID = dataTable.Rows[0].Field<byte>("arsGlFiscalYearPeriodID");
			eRPARPaymentSessionInformationDto.arsAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("arsAvalaraTaxCalculated");
			eRPARPaymentSessionInformationDto.arsCustomRate = dataTable.Rows[0].Field<bool>("arsCustomRate");
			eRPARPaymentSessionInformationDto.arsGroupBySettlement = dataTable.Rows[0].Field<bool>("arsGroupBySettlement");
			eRPARPaymentSessionInformationDto.arsOpenPaymentLoad = dataTable.Rows[0].Field<bool>("arsOpenPaymentLoad");
			eRPARPaymentSessionInformationDto.arsPostedToGl = dataTable.Rows[0].Field<bool>("arsPostedToGl");
			eRPARPaymentSessionInformationDto.arsPlantDepartmentID = dataTable.Rows[0].Field<string>("arsPlantDepartmentID");
			eRPARPaymentSessionInformationDto.arsPlantID = dataTable.Rows[0].Field<string>("arsPlantID");
			eRPARPaymentSessionInformationDto.arsPostedDate = dataTable.Rows[0].Field<DateTime?>("arsPostedDate");
			eRPARPaymentSessionInformationDto.arsReceiptDate = dataTable.Rows[0].Field<DateTime?>("arsReceiptDate");
			eRPARPaymentSessionInformationDto.arsRowVersion = dataTable.Rows[0].Field<byte[]>("arsRowVersion");
			eRPARPaymentSessionInformationDto.arsArPaymentSessionID = dataTable.Rows[0].Field<int>("arsArPaymentSessionID");
			eRPARPaymentSessionInformationDto.arsSettlementEndTime = dataTable.Rows[0].Field<DateTime?>("arsSettlementEndTime");
			eRPARPaymentSessionInformationDto.arsSettlementStartTime = dataTable.Rows[0].Field<DateTime?>("arsSettlementStartTime");
			eRPARPaymentSessionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARPaymentSessionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARPaymentSessionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARPaymentSession(ERPARPaymentSessionDto aRPaymentSession)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARPaymentSessions WHERE arsUniqueID = " + M1Util.ConvertToLinq(aRPaymentSession.arsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["arsArPaymentSessionID"] = aRPaymentSession.arsArPaymentSessionID;
				aRPaymentSession.arsUniqueID = ((aRPaymentSession.arsUniqueID == Guid.Empty) ? Guid.NewGuid() : aRPaymentSession.arsUniqueID);
				dataRow["arsUniqueID"] = aRPaymentSession.arsUniqueID;
				dataRow["arsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["arsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARPaymentSession could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRPaymentSession.arsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARPaymentSession is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["arsRowVersion"], aRPaymentSession.arsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARPaymentSession has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARPaymentSession again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["arsApDiscountGlAccountID"] = aRPaymentSession.arsApDiscountGlAccountID;
			dataRow["arsApGlAccountID"] = aRPaymentSession.arsApGlAccountID;
			dataRow["arsArGlAccountID"] = aRPaymentSession.arsArGlAccountID;
			dataRow["arsBankAccountID"] = aRPaymentSession.arsBankAccountID;
			dataRow["arsCashGlAccountID"] = aRPaymentSession.arsCashGlAccountID;
			dataRow["arsCurrencyRateID"] = aRPaymentSession.arsCurrencyRateID;
			dataRow["arsDepositAmount"] = aRPaymentSession.arsDepositAmount;
			dataRow["arsDepositAmountForeign"] = aRPaymentSession.arsDepositAmountForeign;
			dataRow["arsDiscountGlAccountID"] = aRPaymentSession.arsDiscountGlAccountID;
			dataRow["arsExchangeRate"] = aRPaymentSession.arsExchangeRate;
			dataRow["arsGlFiscalYearID"] = aRPaymentSession.arsGlFiscalYearID;
			dataRow["arsGlFiscalYearPeriodID"] = aRPaymentSession.arsGlFiscalYearPeriodID;
			dataRow["arsAvalaraTaxCalculated"] = aRPaymentSession.arsAvalaraTaxCalculated;
			dataRow["arsCustomRate"] = aRPaymentSession.arsCustomRate;
			dataRow["arsGroupBySettlement"] = aRPaymentSession.arsGroupBySettlement;
			dataRow["arsOpenPaymentLoad"] = aRPaymentSession.arsOpenPaymentLoad;
			dataRow["arsPostedToGl"] = aRPaymentSession.arsPostedToGl;
			dataRow["arsPlantDepartmentID"] = aRPaymentSession.arsPlantDepartmentID;
			dataRow["arsPlantID"] = aRPaymentSession.arsPlantID;
			DataRow dataRow2 = dataRow;
			DateTime? arsPostedDate = aRPaymentSession.arsPostedDate;
			dataRow2["arsPostedDate"] = (arsPostedDate.HasValue ? ((object)arsPostedDate.GetValueOrDefault()) : dataRow["arsPostedDate"]);
			DataRow dataRow3 = dataRow;
			arsPostedDate = aRPaymentSession.arsReceiptDate;
			dataRow3["arsReceiptDate"] = (arsPostedDate.HasValue ? ((object)arsPostedDate.GetValueOrDefault()) : dataRow["arsReceiptDate"]);
			DataRow dataRow4 = dataRow;
			arsPostedDate = aRPaymentSession.arsSettlementEndTime;
			dataRow4["arsSettlementEndTime"] = (arsPostedDate.HasValue ? ((object)arsPostedDate.GetValueOrDefault()) : dataRow["arsSettlementEndTime"]);
			DataRow dataRow5 = dataRow;
			arsPostedDate = aRPaymentSession.arsSettlementStartTime;
			dataRow5["arsSettlementStartTime"] = (arsPostedDate.HasValue ? ((object)arsPostedDate.GetValueOrDefault()) : dataRow["arsSettlementStartTime"]);
			if (aRPaymentSession.CustomFields != null && aRPaymentSession.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRPaymentSession.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARPaymentSession [{aRPaymentSession.arsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARPaymentSession [{aRPaymentSession.arsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
