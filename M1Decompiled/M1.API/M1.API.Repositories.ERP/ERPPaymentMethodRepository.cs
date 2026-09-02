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

public class ERPPaymentMethodRepository : APIBaseRepository, IERPPaymentMethodRepository, IAPIBaseRepository, IDisposable
{
	public ERPPaymentMethodRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPaymentMethodExist(Guid paymentMethodId)
	{
		InitializeParameterLists();
		base.filterList.Add("xahUniqueID|C", paymentMethodId);
		base.selectList.Add("xahUniqueID");
		return Task.FromResult(GetAsObject("PaymentMethods", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPaymentMethodInformationDto>> GetAllPaymentMethods(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPaymentMethodInformationDto> collection = new List<ERPPaymentMethodInformationDto>();
		InitializeParameterLists();
		string[] array = new string[25]
		{
			"xahArPaymentSessionRule", "xahBankAccountID", "xahPaymentMethodID", "xahCreatedBy", "xahCreatedDate", "xahDescription", "xahUniqueID", "xahInactiveDate", "xahInactive", "xahDoNotOpenCashDrawer",
			"xahPmAmex", "xahPmCash", "xahPmCheck", "xahPmDiners", "xahPmDiscover", "xahPmEnroute", "xahPmJAL", "xahPmJCB", "xahPmMasterCard", "xahPmPurchaseOrder",
			"xahPmStoreCredit", "xahPmVisa", "xahRefundPriority", "xahRowVersion", "xahSettlementTime"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PaymentMethods");
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
		using (DataTable dataTable = GetAsDataTable("PaymentMethods", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPaymentMethodInformationDto eRPPaymentMethodInformationDto = new ERPPaymentMethodInformationDto();
				eRPPaymentMethodInformationDto.xahArPaymentSessionRule = dataTable.Rows[i].Field<byte>("xahArPaymentSessionRule");
				eRPPaymentMethodInformationDto.xahBankAccountID = dataTable.Rows[i].Field<string>("xahBankAccountID");
				eRPPaymentMethodInformationDto.xahPaymentMethodID = dataTable.Rows[i].Field<string>("xahPaymentMethodID");
				eRPPaymentMethodInformationDto.xahCreatedBy = dataTable.Rows[i].Field<string>("xahCreatedBy");
				eRPPaymentMethodInformationDto.xahCreatedDate = dataTable.Rows[i].Field<DateTime?>("xahCreatedDate");
				eRPPaymentMethodInformationDto.xahDescription = dataTable.Rows[i].Field<string>("xahDescription");
				eRPPaymentMethodInformationDto.xahUniqueID = dataTable.Rows[i].Field<Guid>("xahUniqueID");
				eRPPaymentMethodInformationDto.xahInactiveDate = dataTable.Rows[i].Field<DateTime?>("xahInactiveDate");
				eRPPaymentMethodInformationDto.xahInactive = dataTable.Rows[i].Field<bool>("xahInactive");
				eRPPaymentMethodInformationDto.xahDoNotOpenCashDrawer = dataTable.Rows[i].Field<bool>("xahDoNotOpenCashDrawer");
				eRPPaymentMethodInformationDto.xahPmAmex = dataTable.Rows[i].Field<bool>("xahPmAmex");
				eRPPaymentMethodInformationDto.xahPmCash = dataTable.Rows[i].Field<bool>("xahPmCash");
				eRPPaymentMethodInformationDto.xahPmCheck = dataTable.Rows[i].Field<bool>("xahPmCheck");
				eRPPaymentMethodInformationDto.xahPmDiners = dataTable.Rows[i].Field<bool>("xahPmDiners");
				eRPPaymentMethodInformationDto.xahPmDiscover = dataTable.Rows[i].Field<bool>("xahPmDiscover");
				eRPPaymentMethodInformationDto.xahPmEnroute = dataTable.Rows[i].Field<bool>("xahPmEnroute");
				eRPPaymentMethodInformationDto.xahPmJAL = dataTable.Rows[i].Field<bool>("xahPmJAL");
				eRPPaymentMethodInformationDto.xahPmJCB = dataTable.Rows[i].Field<bool>("xahPmJCB");
				eRPPaymentMethodInformationDto.xahPmMasterCard = dataTable.Rows[i].Field<bool>("xahPmMasterCard");
				eRPPaymentMethodInformationDto.xahPmPurchaseOrder = dataTable.Rows[i].Field<bool>("xahPmPurchaseOrder");
				eRPPaymentMethodInformationDto.xahPmStoreCredit = dataTable.Rows[i].Field<bool>("xahPmStoreCredit");
				eRPPaymentMethodInformationDto.xahPmVisa = dataTable.Rows[i].Field<bool>("xahPmVisa");
				eRPPaymentMethodInformationDto.xahRefundPriority = dataTable.Rows[i].Field<byte>("xahRefundPriority");
				eRPPaymentMethodInformationDto.xahRowVersion = dataTable.Rows[i].Field<byte[]>("xahRowVersion");
				eRPPaymentMethodInformationDto.xahSettlementTime = dataTable.Rows[i].Field<decimal>("xahSettlementTime");
				eRPPaymentMethodInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPaymentMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPaymentMethodInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPaymentMethodInformationDto> GetPaymentMethod(Guid paymentMethodId)
	{
		ERPPaymentMethodInformationDto eRPPaymentMethodInformationDto = new ERPPaymentMethodInformationDto();
		InitializeParameterLists();
		string[] collection = new string[25]
		{
			"xahArPaymentSessionRule", "xahBankAccountID", "xahPaymentMethodID", "xahCreatedBy", "xahCreatedDate", "xahDescription", "xahUniqueID", "xahInactiveDate", "xahInactive", "xahDoNotOpenCashDrawer",
			"xahPmAmex", "xahPmCash", "xahPmCheck", "xahPmDiners", "xahPmDiscover", "xahPmEnroute", "xahPmJAL", "xahPmJCB", "xahPmMasterCard", "xahPmPurchaseOrder",
			"xahPmStoreCredit", "xahPmVisa", "xahRefundPriority", "xahRowVersion", "xahSettlementTime"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xahUniqueID|C", paymentMethodId);
		AddCustomFieldsToSelectList("PaymentMethods");
		using (DataTable dataTable = GetAsDataTable("PaymentMethods", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPaymentMethodInformationDto);
			}
			eRPPaymentMethodInformationDto.xahArPaymentSessionRule = dataTable.Rows[0].Field<byte>("xahArPaymentSessionRule");
			eRPPaymentMethodInformationDto.xahBankAccountID = dataTable.Rows[0].Field<string>("xahBankAccountID");
			eRPPaymentMethodInformationDto.xahPaymentMethodID = dataTable.Rows[0].Field<string>("xahPaymentMethodID");
			eRPPaymentMethodInformationDto.xahCreatedBy = dataTable.Rows[0].Field<string>("xahCreatedBy");
			eRPPaymentMethodInformationDto.xahCreatedDate = dataTable.Rows[0].Field<DateTime?>("xahCreatedDate");
			eRPPaymentMethodInformationDto.xahDescription = dataTable.Rows[0].Field<string>("xahDescription");
			eRPPaymentMethodInformationDto.xahUniqueID = dataTable.Rows[0].Field<Guid>("xahUniqueID");
			eRPPaymentMethodInformationDto.xahInactiveDate = dataTable.Rows[0].Field<DateTime?>("xahInactiveDate");
			eRPPaymentMethodInformationDto.xahInactive = dataTable.Rows[0].Field<bool>("xahInactive");
			eRPPaymentMethodInformationDto.xahDoNotOpenCashDrawer = dataTable.Rows[0].Field<bool>("xahDoNotOpenCashDrawer");
			eRPPaymentMethodInformationDto.xahPmAmex = dataTable.Rows[0].Field<bool>("xahPmAmex");
			eRPPaymentMethodInformationDto.xahPmCash = dataTable.Rows[0].Field<bool>("xahPmCash");
			eRPPaymentMethodInformationDto.xahPmCheck = dataTable.Rows[0].Field<bool>("xahPmCheck");
			eRPPaymentMethodInformationDto.xahPmDiners = dataTable.Rows[0].Field<bool>("xahPmDiners");
			eRPPaymentMethodInformationDto.xahPmDiscover = dataTable.Rows[0].Field<bool>("xahPmDiscover");
			eRPPaymentMethodInformationDto.xahPmEnroute = dataTable.Rows[0].Field<bool>("xahPmEnroute");
			eRPPaymentMethodInformationDto.xahPmJAL = dataTable.Rows[0].Field<bool>("xahPmJAL");
			eRPPaymentMethodInformationDto.xahPmJCB = dataTable.Rows[0].Field<bool>("xahPmJCB");
			eRPPaymentMethodInformationDto.xahPmMasterCard = dataTable.Rows[0].Field<bool>("xahPmMasterCard");
			eRPPaymentMethodInformationDto.xahPmPurchaseOrder = dataTable.Rows[0].Field<bool>("xahPmPurchaseOrder");
			eRPPaymentMethodInformationDto.xahPmStoreCredit = dataTable.Rows[0].Field<bool>("xahPmStoreCredit");
			eRPPaymentMethodInformationDto.xahPmVisa = dataTable.Rows[0].Field<bool>("xahPmVisa");
			eRPPaymentMethodInformationDto.xahRefundPriority = dataTable.Rows[0].Field<byte>("xahRefundPriority");
			eRPPaymentMethodInformationDto.xahRowVersion = dataTable.Rows[0].Field<byte[]>("xahRowVersion");
			eRPPaymentMethodInformationDto.xahSettlementTime = dataTable.Rows[0].Field<decimal>("xahSettlementTime");
			eRPPaymentMethodInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPaymentMethodInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPaymentMethodInformationDto);
	}

	public Task<APIValidationInfoDto> SavePaymentMethod(ERPPaymentMethodDto paymentMethod)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PaymentMethods WHERE xahUniqueID = " + M1Util.ConvertToLinq(paymentMethod.xahUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xahPaymentMethodID"] = paymentMethod.xahPaymentMethodID.ToUpper();
				paymentMethod.xahUniqueID = ((paymentMethod.xahUniqueID == Guid.Empty) ? Guid.NewGuid() : paymentMethod.xahUniqueID);
				dataRow["xahUniqueID"] = paymentMethod.xahUniqueID;
				dataRow["xahCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xahCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PaymentMethod could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (paymentMethod.xahRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PaymentMethod is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xahRowVersion"], paymentMethod.xahRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PaymentMethod has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PaymentMethod again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xahArPaymentSessionRule"] = paymentMethod.xahArPaymentSessionRule;
			dataRow["xahBankAccountID"] = paymentMethod.xahBankAccountID;
			dataRow["xahDescription"] = paymentMethod.xahDescription;
			DataRow dataRow2 = dataRow;
			DateTime? xahInactiveDate = paymentMethod.xahInactiveDate;
			dataRow2["xahInactiveDate"] = (xahInactiveDate.HasValue ? ((object)xahInactiveDate.GetValueOrDefault()) : dataRow["xahInactiveDate"]);
			dataRow["xahInactive"] = paymentMethod.xahInactive;
			dataRow["xahDoNotOpenCashDrawer"] = paymentMethod.xahDoNotOpenCashDrawer;
			dataRow["xahPmAmex"] = paymentMethod.xahPmAmex;
			dataRow["xahPmCash"] = paymentMethod.xahPmCash;
			dataRow["xahPmCheck"] = paymentMethod.xahPmCheck;
			dataRow["xahPmDiners"] = paymentMethod.xahPmDiners;
			dataRow["xahPmDiscover"] = paymentMethod.xahPmDiscover;
			dataRow["xahPmEnroute"] = paymentMethod.xahPmEnroute;
			dataRow["xahPmJAL"] = paymentMethod.xahPmJAL;
			dataRow["xahPmJCB"] = paymentMethod.xahPmJCB;
			dataRow["xahPmMasterCard"] = paymentMethod.xahPmMasterCard;
			dataRow["xahPmPurchaseOrder"] = paymentMethod.xahPmPurchaseOrder;
			dataRow["xahPmStoreCredit"] = paymentMethod.xahPmStoreCredit;
			dataRow["xahPmVisa"] = paymentMethod.xahPmVisa;
			dataRow["xahRefundPriority"] = paymentMethod.xahRefundPriority;
			dataRow["xahSettlementTime"] = paymentMethod.xahSettlementTime;
			if (paymentMethod.CustomFields != null && paymentMethod.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in paymentMethod.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PaymentMethod [{paymentMethod.xahUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PaymentMethod [{paymentMethod.xahUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
