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

public class ERPARPaymentLineRepository : APIBaseRepository, IERPARPaymentLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPARPaymentLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesARPaymentLineExist(Guid aRPaymentLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("arnUniqueID|C", aRPaymentLineId);
		base.selectList.Add("arnUniqueID");
		return Task.FromResult(GetAsObject("ARPaymentLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPARPaymentLineInformationDto>> GetAllARPaymentLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPARPaymentLineInformationDto> collection = new List<ERPARPaymentLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[44]
		{
			"arnAdjustmentAmount", "arnAdjustmentAmountForeign", "arnAdjustmentGlAccountID", "arnApInvoiceID", "arnArInvoiceID", "arnArPaymentEPayID", "arnArPaymentHeaderID", "arnArPaymentSessionID", "arnCreatedBy", "arnCreatedDate",
			"arnDiscountAmount", "arnDiscountAmountForeign", "arnDiscountGlAccountID", "arnDiscountTaxAmount", "arnDiscountTaxAmountForeign", "arnDiscountTaxCodeID", "arnUniqueID", "arnExchangeAmount", "arnExchangeGlAccountID", "arnAvalaraTaxCalculated",
			"arnOverpayment", "arnPostedToGl", "arnNonTaxReasonID", "arnOriginalInvBalanceForeign", "arnOriginalInvoiceBalance", "arnPaymentAmount", "arnPaymentAmountForeign", "arnRetentionPayAmtForeign", "arnRetentionPaymentAmount", "arnRowVersion",
			"arnSecondDiscountTaxAmount", "arnSecondDiscountTaxCodeID", "arnSecondDisTaxAmtForeign", "arnSecondTaxAmount", "arnSecondTaxAmountForeign", "arnSecondTaxCodeID", "arnArPaymentLineID", "arnTaxAmount", "arnTaxAmountForeign", "arnTaxCodeID",
			"arnTotalDiscountAmount", "arnTotalDiscountAmtForeign", "arnUnrealisedExchangeAmt", "arnUnrealisedExGlAccountID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ARPaymentLines");
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
		using (DataTable dataTable = GetAsDataTable("ARPaymentLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPARPaymentLineInformationDto eRPARPaymentLineInformationDto = new ERPARPaymentLineInformationDto();
				eRPARPaymentLineInformationDto.arnAdjustmentAmount = dataTable.Rows[i].Field<decimal>("arnAdjustmentAmount");
				eRPARPaymentLineInformationDto.arnAdjustmentAmountForeign = dataTable.Rows[i].Field<decimal>("arnAdjustmentAmountForeign");
				eRPARPaymentLineInformationDto.arnAdjustmentGlAccountID = dataTable.Rows[i].Field<string>("arnAdjustmentGlAccountID");
				eRPARPaymentLineInformationDto.arnApInvoiceID = dataTable.Rows[i].Field<string>("arnApInvoiceID");
				eRPARPaymentLineInformationDto.arnArInvoiceID = dataTable.Rows[i].Field<string>("arnArInvoiceID");
				eRPARPaymentLineInformationDto.arnArPaymentEPayID = dataTable.Rows[i].Field<int>("arnArPaymentEPayID");
				eRPARPaymentLineInformationDto.arnArPaymentHeaderID = dataTable.Rows[i].Field<int>("arnArPaymentHeaderID");
				eRPARPaymentLineInformationDto.arnArPaymentSessionID = dataTable.Rows[i].Field<int>("arnArPaymentSessionID");
				eRPARPaymentLineInformationDto.arnCreatedBy = dataTable.Rows[i].Field<string>("arnCreatedBy");
				eRPARPaymentLineInformationDto.arnCreatedDate = dataTable.Rows[i].Field<DateTime?>("arnCreatedDate");
				eRPARPaymentLineInformationDto.arnDiscountAmount = dataTable.Rows[i].Field<decimal>("arnDiscountAmount");
				eRPARPaymentLineInformationDto.arnDiscountAmountForeign = dataTable.Rows[i].Field<decimal>("arnDiscountAmountForeign");
				eRPARPaymentLineInformationDto.arnDiscountGlAccountID = dataTable.Rows[i].Field<string>("arnDiscountGlAccountID");
				eRPARPaymentLineInformationDto.arnDiscountTaxAmount = dataTable.Rows[i].Field<decimal>("arnDiscountTaxAmount");
				eRPARPaymentLineInformationDto.arnDiscountTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arnDiscountTaxAmountForeign");
				eRPARPaymentLineInformationDto.arnDiscountTaxCodeID = dataTable.Rows[i].Field<string>("arnDiscountTaxCodeID");
				eRPARPaymentLineInformationDto.arnUniqueID = dataTable.Rows[i].Field<Guid>("arnUniqueID");
				eRPARPaymentLineInformationDto.arnExchangeAmount = dataTable.Rows[i].Field<decimal>("arnExchangeAmount");
				eRPARPaymentLineInformationDto.arnExchangeGlAccountID = dataTable.Rows[i].Field<string>("arnExchangeGlAccountID");
				eRPARPaymentLineInformationDto.arnAvalaraTaxCalculated = dataTable.Rows[i].Field<bool>("arnAvalaraTaxCalculated");
				eRPARPaymentLineInformationDto.arnOverpayment = dataTable.Rows[i].Field<bool>("arnOverpayment");
				eRPARPaymentLineInformationDto.arnPostedToGl = dataTable.Rows[i].Field<bool>("arnPostedToGl");
				eRPARPaymentLineInformationDto.arnNonTaxReasonID = dataTable.Rows[i].Field<string>("arnNonTaxReasonID");
				eRPARPaymentLineInformationDto.arnOriginalInvBalanceForeign = dataTable.Rows[i].Field<decimal>("arnOriginalInvBalanceForeign");
				eRPARPaymentLineInformationDto.arnOriginalInvoiceBalance = dataTable.Rows[i].Field<decimal>("arnOriginalInvoiceBalance");
				eRPARPaymentLineInformationDto.arnPaymentAmount = dataTable.Rows[i].Field<decimal>("arnPaymentAmount");
				eRPARPaymentLineInformationDto.arnPaymentAmountForeign = dataTable.Rows[i].Field<decimal>("arnPaymentAmountForeign");
				eRPARPaymentLineInformationDto.arnRetentionPayAmtForeign = dataTable.Rows[i].Field<decimal>("arnRetentionPayAmtForeign");
				eRPARPaymentLineInformationDto.arnRetentionPaymentAmount = dataTable.Rows[i].Field<decimal>("arnRetentionPaymentAmount");
				eRPARPaymentLineInformationDto.arnRowVersion = dataTable.Rows[i].Field<byte[]>("arnRowVersion");
				eRPARPaymentLineInformationDto.arnSecondDiscountTaxAmount = dataTable.Rows[i].Field<decimal>("arnSecondDiscountTaxAmount");
				eRPARPaymentLineInformationDto.arnSecondDiscountTaxCodeID = dataTable.Rows[i].Field<string>("arnSecondDiscountTaxCodeID");
				eRPARPaymentLineInformationDto.arnSecondDisTaxAmtForeign = dataTable.Rows[i].Field<decimal>("arnSecondDisTaxAmtForeign");
				eRPARPaymentLineInformationDto.arnSecondTaxAmount = dataTable.Rows[i].Field<decimal>("arnSecondTaxAmount");
				eRPARPaymentLineInformationDto.arnSecondTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arnSecondTaxAmountForeign");
				eRPARPaymentLineInformationDto.arnSecondTaxCodeID = dataTable.Rows[i].Field<string>("arnSecondTaxCodeID");
				eRPARPaymentLineInformationDto.arnArPaymentLineID = dataTable.Rows[i].Field<short>("arnArPaymentLineID");
				eRPARPaymentLineInformationDto.arnTaxAmount = dataTable.Rows[i].Field<decimal>("arnTaxAmount");
				eRPARPaymentLineInformationDto.arnTaxAmountForeign = dataTable.Rows[i].Field<decimal>("arnTaxAmountForeign");
				eRPARPaymentLineInformationDto.arnTaxCodeID = dataTable.Rows[i].Field<string>("arnTaxCodeID");
				eRPARPaymentLineInformationDto.arnTotalDiscountAmount = dataTable.Rows[i].Field<decimal>("arnTotalDiscountAmount");
				eRPARPaymentLineInformationDto.arnTotalDiscountAmtForeign = dataTable.Rows[i].Field<decimal>("arnTotalDiscountAmtForeign");
				eRPARPaymentLineInformationDto.arnUnrealisedExchangeAmt = dataTable.Rows[i].Field<decimal>("arnUnrealisedExchangeAmt");
				eRPARPaymentLineInformationDto.arnUnrealisedExGlAccountID = dataTable.Rows[i].Field<string>("arnUnrealisedExGlAccountID");
				eRPARPaymentLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPARPaymentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPARPaymentLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPARPaymentLineInformationDto> GetARPaymentLine(Guid aRPaymentLineId)
	{
		ERPARPaymentLineInformationDto eRPARPaymentLineInformationDto = new ERPARPaymentLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[44]
		{
			"arnAdjustmentAmount", "arnAdjustmentAmountForeign", "arnAdjustmentGlAccountID", "arnApInvoiceID", "arnArInvoiceID", "arnArPaymentEPayID", "arnArPaymentHeaderID", "arnArPaymentSessionID", "arnCreatedBy", "arnCreatedDate",
			"arnDiscountAmount", "arnDiscountAmountForeign", "arnDiscountGlAccountID", "arnDiscountTaxAmount", "arnDiscountTaxAmountForeign", "arnDiscountTaxCodeID", "arnUniqueID", "arnExchangeAmount", "arnExchangeGlAccountID", "arnAvalaraTaxCalculated",
			"arnOverpayment", "arnPostedToGl", "arnNonTaxReasonID", "arnOriginalInvBalanceForeign", "arnOriginalInvoiceBalance", "arnPaymentAmount", "arnPaymentAmountForeign", "arnRetentionPayAmtForeign", "arnRetentionPaymentAmount", "arnRowVersion",
			"arnSecondDiscountTaxAmount", "arnSecondDiscountTaxCodeID", "arnSecondDisTaxAmtForeign", "arnSecondTaxAmount", "arnSecondTaxAmountForeign", "arnSecondTaxCodeID", "arnArPaymentLineID", "arnTaxAmount", "arnTaxAmountForeign", "arnTaxCodeID",
			"arnTotalDiscountAmount", "arnTotalDiscountAmtForeign", "arnUnrealisedExchangeAmt", "arnUnrealisedExGlAccountID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("arnUniqueID|C", aRPaymentLineId);
		AddCustomFieldsToSelectList("ARPaymentLines");
		using (DataTable dataTable = GetAsDataTable("ARPaymentLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPARPaymentLineInformationDto);
			}
			eRPARPaymentLineInformationDto.arnAdjustmentAmount = dataTable.Rows[0].Field<decimal>("arnAdjustmentAmount");
			eRPARPaymentLineInformationDto.arnAdjustmentAmountForeign = dataTable.Rows[0].Field<decimal>("arnAdjustmentAmountForeign");
			eRPARPaymentLineInformationDto.arnAdjustmentGlAccountID = dataTable.Rows[0].Field<string>("arnAdjustmentGlAccountID");
			eRPARPaymentLineInformationDto.arnApInvoiceID = dataTable.Rows[0].Field<string>("arnApInvoiceID");
			eRPARPaymentLineInformationDto.arnArInvoiceID = dataTable.Rows[0].Field<string>("arnArInvoiceID");
			eRPARPaymentLineInformationDto.arnArPaymentEPayID = dataTable.Rows[0].Field<int>("arnArPaymentEPayID");
			eRPARPaymentLineInformationDto.arnArPaymentHeaderID = dataTable.Rows[0].Field<int>("arnArPaymentHeaderID");
			eRPARPaymentLineInformationDto.arnArPaymentSessionID = dataTable.Rows[0].Field<int>("arnArPaymentSessionID");
			eRPARPaymentLineInformationDto.arnCreatedBy = dataTable.Rows[0].Field<string>("arnCreatedBy");
			eRPARPaymentLineInformationDto.arnCreatedDate = dataTable.Rows[0].Field<DateTime?>("arnCreatedDate");
			eRPARPaymentLineInformationDto.arnDiscountAmount = dataTable.Rows[0].Field<decimal>("arnDiscountAmount");
			eRPARPaymentLineInformationDto.arnDiscountAmountForeign = dataTable.Rows[0].Field<decimal>("arnDiscountAmountForeign");
			eRPARPaymentLineInformationDto.arnDiscountGlAccountID = dataTable.Rows[0].Field<string>("arnDiscountGlAccountID");
			eRPARPaymentLineInformationDto.arnDiscountTaxAmount = dataTable.Rows[0].Field<decimal>("arnDiscountTaxAmount");
			eRPARPaymentLineInformationDto.arnDiscountTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arnDiscountTaxAmountForeign");
			eRPARPaymentLineInformationDto.arnDiscountTaxCodeID = dataTable.Rows[0].Field<string>("arnDiscountTaxCodeID");
			eRPARPaymentLineInformationDto.arnUniqueID = dataTable.Rows[0].Field<Guid>("arnUniqueID");
			eRPARPaymentLineInformationDto.arnExchangeAmount = dataTable.Rows[0].Field<decimal>("arnExchangeAmount");
			eRPARPaymentLineInformationDto.arnExchangeGlAccountID = dataTable.Rows[0].Field<string>("arnExchangeGlAccountID");
			eRPARPaymentLineInformationDto.arnAvalaraTaxCalculated = dataTable.Rows[0].Field<bool>("arnAvalaraTaxCalculated");
			eRPARPaymentLineInformationDto.arnOverpayment = dataTable.Rows[0].Field<bool>("arnOverpayment");
			eRPARPaymentLineInformationDto.arnPostedToGl = dataTable.Rows[0].Field<bool>("arnPostedToGl");
			eRPARPaymentLineInformationDto.arnNonTaxReasonID = dataTable.Rows[0].Field<string>("arnNonTaxReasonID");
			eRPARPaymentLineInformationDto.arnOriginalInvBalanceForeign = dataTable.Rows[0].Field<decimal>("arnOriginalInvBalanceForeign");
			eRPARPaymentLineInformationDto.arnOriginalInvoiceBalance = dataTable.Rows[0].Field<decimal>("arnOriginalInvoiceBalance");
			eRPARPaymentLineInformationDto.arnPaymentAmount = dataTable.Rows[0].Field<decimal>("arnPaymentAmount");
			eRPARPaymentLineInformationDto.arnPaymentAmountForeign = dataTable.Rows[0].Field<decimal>("arnPaymentAmountForeign");
			eRPARPaymentLineInformationDto.arnRetentionPayAmtForeign = dataTable.Rows[0].Field<decimal>("arnRetentionPayAmtForeign");
			eRPARPaymentLineInformationDto.arnRetentionPaymentAmount = dataTable.Rows[0].Field<decimal>("arnRetentionPaymentAmount");
			eRPARPaymentLineInformationDto.arnRowVersion = dataTable.Rows[0].Field<byte[]>("arnRowVersion");
			eRPARPaymentLineInformationDto.arnSecondDiscountTaxAmount = dataTable.Rows[0].Field<decimal>("arnSecondDiscountTaxAmount");
			eRPARPaymentLineInformationDto.arnSecondDiscountTaxCodeID = dataTable.Rows[0].Field<string>("arnSecondDiscountTaxCodeID");
			eRPARPaymentLineInformationDto.arnSecondDisTaxAmtForeign = dataTable.Rows[0].Field<decimal>("arnSecondDisTaxAmtForeign");
			eRPARPaymentLineInformationDto.arnSecondTaxAmount = dataTable.Rows[0].Field<decimal>("arnSecondTaxAmount");
			eRPARPaymentLineInformationDto.arnSecondTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arnSecondTaxAmountForeign");
			eRPARPaymentLineInformationDto.arnSecondTaxCodeID = dataTable.Rows[0].Field<string>("arnSecondTaxCodeID");
			eRPARPaymentLineInformationDto.arnArPaymentLineID = dataTable.Rows[0].Field<short>("arnArPaymentLineID");
			eRPARPaymentLineInformationDto.arnTaxAmount = dataTable.Rows[0].Field<decimal>("arnTaxAmount");
			eRPARPaymentLineInformationDto.arnTaxAmountForeign = dataTable.Rows[0].Field<decimal>("arnTaxAmountForeign");
			eRPARPaymentLineInformationDto.arnTaxCodeID = dataTable.Rows[0].Field<string>("arnTaxCodeID");
			eRPARPaymentLineInformationDto.arnTotalDiscountAmount = dataTable.Rows[0].Field<decimal>("arnTotalDiscountAmount");
			eRPARPaymentLineInformationDto.arnTotalDiscountAmtForeign = dataTable.Rows[0].Field<decimal>("arnTotalDiscountAmtForeign");
			eRPARPaymentLineInformationDto.arnUnrealisedExchangeAmt = dataTable.Rows[0].Field<decimal>("arnUnrealisedExchangeAmt");
			eRPARPaymentLineInformationDto.arnUnrealisedExGlAccountID = dataTable.Rows[0].Field<string>("arnUnrealisedExGlAccountID");
			eRPARPaymentLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPARPaymentLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPARPaymentLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveARPaymentLine(ERPARPaymentLineDto aRPaymentLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ARPaymentLines WHERE arnUniqueID = " + M1Util.ConvertToLinq(aRPaymentLine.arnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["arnArPaymentSessionID"] = aRPaymentLine.arnArPaymentSessionID;
				dataRow["arnArPaymentHeaderID"] = aRPaymentLine.arnArPaymentHeaderID;
				dataRow["arnArPaymentLineID"] = aRPaymentLine.arnArPaymentLineID;
				aRPaymentLine.arnUniqueID = ((aRPaymentLine.arnUniqueID == Guid.Empty) ? Guid.NewGuid() : aRPaymentLine.arnUniqueID);
				dataRow["arnUniqueID"] = aRPaymentLine.arnUniqueID;
				dataRow["arnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["arnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ARPaymentLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (aRPaymentLine.arnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ARPaymentLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["arnRowVersion"], aRPaymentLine.arnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ARPaymentLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ARPaymentLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["arnAdjustmentAmount"] = aRPaymentLine.arnAdjustmentAmount;
			dataRow["arnAdjustmentAmountForeign"] = aRPaymentLine.arnAdjustmentAmountForeign;
			dataRow["arnAdjustmentGlAccountID"] = aRPaymentLine.arnAdjustmentGlAccountID;
			dataRow["arnApInvoiceID"] = aRPaymentLine.arnApInvoiceID;
			dataRow["arnArInvoiceID"] = aRPaymentLine.arnArInvoiceID;
			dataRow["arnArPaymentEPayID"] = aRPaymentLine.arnArPaymentEPayID;
			dataRow["arnDiscountAmount"] = aRPaymentLine.arnDiscountAmount;
			dataRow["arnDiscountAmountForeign"] = aRPaymentLine.arnDiscountAmountForeign;
			dataRow["arnDiscountGlAccountID"] = aRPaymentLine.arnDiscountGlAccountID;
			dataRow["arnDiscountTaxAmount"] = aRPaymentLine.arnDiscountTaxAmount;
			dataRow["arnDiscountTaxAmountForeign"] = aRPaymentLine.arnDiscountTaxAmountForeign;
			dataRow["arnDiscountTaxCodeID"] = aRPaymentLine.arnDiscountTaxCodeID;
			dataRow["arnExchangeAmount"] = aRPaymentLine.arnExchangeAmount;
			dataRow["arnExchangeGlAccountID"] = aRPaymentLine.arnExchangeGlAccountID;
			dataRow["arnAvalaraTaxCalculated"] = aRPaymentLine.arnAvalaraTaxCalculated;
			dataRow["arnOverpayment"] = aRPaymentLine.arnOverpayment;
			dataRow["arnPostedToGl"] = aRPaymentLine.arnPostedToGl;
			dataRow["arnNonTaxReasonID"] = aRPaymentLine.arnNonTaxReasonID;
			dataRow["arnOriginalInvBalanceForeign"] = aRPaymentLine.arnOriginalInvBalanceForeign;
			dataRow["arnOriginalInvoiceBalance"] = aRPaymentLine.arnOriginalInvoiceBalance;
			dataRow["arnPaymentAmount"] = aRPaymentLine.arnPaymentAmount;
			dataRow["arnPaymentAmountForeign"] = aRPaymentLine.arnPaymentAmountForeign;
			dataRow["arnRetentionPayAmtForeign"] = aRPaymentLine.arnRetentionPayAmtForeign;
			dataRow["arnRetentionPaymentAmount"] = aRPaymentLine.arnRetentionPaymentAmount;
			dataRow["arnSecondDiscountTaxAmount"] = aRPaymentLine.arnSecondDiscountTaxAmount;
			dataRow["arnSecondDiscountTaxCodeID"] = aRPaymentLine.arnSecondDiscountTaxCodeID;
			dataRow["arnSecondDisTaxAmtForeign"] = aRPaymentLine.arnSecondDisTaxAmtForeign;
			dataRow["arnSecondTaxAmount"] = aRPaymentLine.arnSecondTaxAmount;
			dataRow["arnSecondTaxAmountForeign"] = aRPaymentLine.arnSecondTaxAmountForeign;
			dataRow["arnSecondTaxCodeID"] = aRPaymentLine.arnSecondTaxCodeID;
			dataRow["arnTaxAmount"] = aRPaymentLine.arnTaxAmount;
			dataRow["arnTaxAmountForeign"] = aRPaymentLine.arnTaxAmountForeign;
			dataRow["arnTaxCodeID"] = aRPaymentLine.arnTaxCodeID;
			dataRow["arnTotalDiscountAmount"] = aRPaymentLine.arnTotalDiscountAmount;
			dataRow["arnTotalDiscountAmtForeign"] = aRPaymentLine.arnTotalDiscountAmtForeign;
			dataRow["arnUnrealisedExchangeAmt"] = aRPaymentLine.arnUnrealisedExchangeAmt;
			dataRow["arnUnrealisedExGlAccountID"] = aRPaymentLine.arnUnrealisedExGlAccountID;
			if (aRPaymentLine.CustomFields != null && aRPaymentLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in aRPaymentLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ARPaymentLine [{aRPaymentLine.arnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ARPaymentLine [{aRPaymentLine.arnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
