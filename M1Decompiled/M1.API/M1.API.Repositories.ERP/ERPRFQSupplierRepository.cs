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

public class ERPRFQSupplierRepository : APIBaseRepository, IERPRFQSupplierRepository, IAPIBaseRepository, IDisposable
{
	public ERPRFQSupplierRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRFQSupplierExist(Guid rFQSupplierId)
	{
		InitializeParameterLists();
		base.filterList.Add("rqsUniqueID|C", rFQSupplierId);
		base.selectList.Add("rqsUniqueID");
		return Task.FromResult(GetAsObject("RFQSuppliers", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRFQSupplierInformationDto>> GetAllRFQSuppliers(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRFQSupplierInformationDto> collection = new List<ERPRFQSupplierInformationDto>();
		InitializeParameterLists();
		string[] array = new string[20]
		{
			"rqsCreatedBy", "rqsCreatedDate", "rqsCurrencyRateID", "rqsDueDate", "rqsUniqueID", "rqsExchangeRate", "rqsClosed", "rqsComplete", "rqsCustomRate", "rqsSelectedSupplier",
			"rqsUpdatedPartPrices", "rqsOrgPartID", "rqsPurchaseContactID", "rqsPurchaseLocationID", "rqsRfqID", "rqsRfqLineID", "rqsRowVersion", "rqsSelectedSupplierDate", "rqsRfqSupplierID", "rqsSupplierOrganizationID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RFQSuppliers");
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
		using (DataTable dataTable = GetAsDataTable("RFQSuppliers", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRFQSupplierInformationDto eRPRFQSupplierInformationDto = new ERPRFQSupplierInformationDto();
				eRPRFQSupplierInformationDto.rqsCreatedBy = dataTable.Rows[i].Field<string>("rqsCreatedBy");
				eRPRFQSupplierInformationDto.rqsCreatedDate = dataTable.Rows[i].Field<DateTime?>("rqsCreatedDate");
				eRPRFQSupplierInformationDto.rqsCurrencyRateID = dataTable.Rows[i].Field<string>("rqsCurrencyRateID");
				eRPRFQSupplierInformationDto.rqsDueDate = dataTable.Rows[i].Field<DateTime?>("rqsDueDate");
				eRPRFQSupplierInformationDto.rqsUniqueID = dataTable.Rows[i].Field<Guid>("rqsUniqueID");
				eRPRFQSupplierInformationDto.rqsExchangeRate = dataTable.Rows[i].Field<decimal>("rqsExchangeRate");
				eRPRFQSupplierInformationDto.rqsClosed = dataTable.Rows[i].Field<bool>("rqsClosed");
				eRPRFQSupplierInformationDto.rqsComplete = dataTable.Rows[i].Field<bool>("rqsComplete");
				eRPRFQSupplierInformationDto.rqsCustomRate = dataTable.Rows[i].Field<bool>("rqsCustomRate");
				eRPRFQSupplierInformationDto.rqsSelectedSupplier = dataTable.Rows[i].Field<bool>("rqsSelectedSupplier");
				eRPRFQSupplierInformationDto.rqsUpdatedPartPrices = dataTable.Rows[i].Field<bool>("rqsUpdatedPartPrices");
				eRPRFQSupplierInformationDto.rqsOrgPartID = dataTable.Rows[i].Field<string>("rqsOrgPartID");
				eRPRFQSupplierInformationDto.rqsPurchaseContactID = dataTable.Rows[i].Field<string>("rqsPurchaseContactID");
				eRPRFQSupplierInformationDto.rqsPurchaseLocationID = dataTable.Rows[i].Field<string>("rqsPurchaseLocationID");
				eRPRFQSupplierInformationDto.rqsRfqID = dataTable.Rows[i].Field<string>("rqsRfqID");
				eRPRFQSupplierInformationDto.rqsRfqLineID = dataTable.Rows[i].Field<short>("rqsRfqLineID");
				eRPRFQSupplierInformationDto.rqsRowVersion = dataTable.Rows[i].Field<byte[]>("rqsRowVersion");
				eRPRFQSupplierInformationDto.rqsSelectedSupplierDate = dataTable.Rows[i].Field<DateTime?>("rqsSelectedSupplierDate");
				eRPRFQSupplierInformationDto.rqsRfqSupplierID = dataTable.Rows[i].Field<short>("rqsRfqSupplierID");
				eRPRFQSupplierInformationDto.rqsSupplierOrganizationID = dataTable.Rows[i].Field<string>("rqsSupplierOrganizationID");
				eRPRFQSupplierInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRFQSupplierInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRFQSupplierInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRFQSupplierInformationDto> GetRFQSupplier(Guid rFQSupplierId)
	{
		ERPRFQSupplierInformationDto eRPRFQSupplierInformationDto = new ERPRFQSupplierInformationDto();
		InitializeParameterLists();
		string[] collection = new string[20]
		{
			"rqsCreatedBy", "rqsCreatedDate", "rqsCurrencyRateID", "rqsDueDate", "rqsUniqueID", "rqsExchangeRate", "rqsClosed", "rqsComplete", "rqsCustomRate", "rqsSelectedSupplier",
			"rqsUpdatedPartPrices", "rqsOrgPartID", "rqsPurchaseContactID", "rqsPurchaseLocationID", "rqsRfqID", "rqsRfqLineID", "rqsRowVersion", "rqsSelectedSupplierDate", "rqsRfqSupplierID", "rqsSupplierOrganizationID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rqsUniqueID|C", rFQSupplierId);
		AddCustomFieldsToSelectList("RFQSuppliers");
		using (DataTable dataTable = GetAsDataTable("RFQSuppliers", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRFQSupplierInformationDto);
			}
			eRPRFQSupplierInformationDto.rqsCreatedBy = dataTable.Rows[0].Field<string>("rqsCreatedBy");
			eRPRFQSupplierInformationDto.rqsCreatedDate = dataTable.Rows[0].Field<DateTime?>("rqsCreatedDate");
			eRPRFQSupplierInformationDto.rqsCurrencyRateID = dataTable.Rows[0].Field<string>("rqsCurrencyRateID");
			eRPRFQSupplierInformationDto.rqsDueDate = dataTable.Rows[0].Field<DateTime?>("rqsDueDate");
			eRPRFQSupplierInformationDto.rqsUniqueID = dataTable.Rows[0].Field<Guid>("rqsUniqueID");
			eRPRFQSupplierInformationDto.rqsExchangeRate = dataTable.Rows[0].Field<decimal>("rqsExchangeRate");
			eRPRFQSupplierInformationDto.rqsClosed = dataTable.Rows[0].Field<bool>("rqsClosed");
			eRPRFQSupplierInformationDto.rqsComplete = dataTable.Rows[0].Field<bool>("rqsComplete");
			eRPRFQSupplierInformationDto.rqsCustomRate = dataTable.Rows[0].Field<bool>("rqsCustomRate");
			eRPRFQSupplierInformationDto.rqsSelectedSupplier = dataTable.Rows[0].Field<bool>("rqsSelectedSupplier");
			eRPRFQSupplierInformationDto.rqsUpdatedPartPrices = dataTable.Rows[0].Field<bool>("rqsUpdatedPartPrices");
			eRPRFQSupplierInformationDto.rqsOrgPartID = dataTable.Rows[0].Field<string>("rqsOrgPartID");
			eRPRFQSupplierInformationDto.rqsPurchaseContactID = dataTable.Rows[0].Field<string>("rqsPurchaseContactID");
			eRPRFQSupplierInformationDto.rqsPurchaseLocationID = dataTable.Rows[0].Field<string>("rqsPurchaseLocationID");
			eRPRFQSupplierInformationDto.rqsRfqID = dataTable.Rows[0].Field<string>("rqsRfqID");
			eRPRFQSupplierInformationDto.rqsRfqLineID = dataTable.Rows[0].Field<short>("rqsRfqLineID");
			eRPRFQSupplierInformationDto.rqsRowVersion = dataTable.Rows[0].Field<byte[]>("rqsRowVersion");
			eRPRFQSupplierInformationDto.rqsSelectedSupplierDate = dataTable.Rows[0].Field<DateTime?>("rqsSelectedSupplierDate");
			eRPRFQSupplierInformationDto.rqsRfqSupplierID = dataTable.Rows[0].Field<short>("rqsRfqSupplierID");
			eRPRFQSupplierInformationDto.rqsSupplierOrganizationID = dataTable.Rows[0].Field<string>("rqsSupplierOrganizationID");
			eRPRFQSupplierInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRFQSupplierInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRFQSupplierInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRFQSupplier(ERPRFQSupplierDto rFQSupplier)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RFQSuppliers WHERE rqsUniqueID = " + M1Util.ConvertToLinq(rFQSupplier.rqsUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rqsRfqID"] = rFQSupplier.rqsRfqID.ToUpper();
				dataRow["rqsRfqLineID"] = rFQSupplier.rqsRfqLineID;
				dataRow["rqsRfqSupplierID"] = rFQSupplier.rqsRfqSupplierID;
				rFQSupplier.rqsUniqueID = ((rFQSupplier.rqsUniqueID == Guid.Empty) ? Guid.NewGuid() : rFQSupplier.rqsUniqueID);
				dataRow["rqsUniqueID"] = rFQSupplier.rqsUniqueID;
				dataRow["rqsCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rqsCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RFQSupplier could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rFQSupplier.rqsRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RFQSupplier is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rqsRowVersion"], rFQSupplier.rqsRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RFQSupplier has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RFQSupplier again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rqsCurrencyRateID"] = rFQSupplier.rqsCurrencyRateID;
			DataRow dataRow2 = dataRow;
			DateTime? rqsDueDate = rFQSupplier.rqsDueDate;
			dataRow2["rqsDueDate"] = (rqsDueDate.HasValue ? ((object)rqsDueDate.GetValueOrDefault()) : dataRow["rqsDueDate"]);
			dataRow["rqsExchangeRate"] = rFQSupplier.rqsExchangeRate;
			dataRow["rqsClosed"] = rFQSupplier.rqsClosed;
			dataRow["rqsComplete"] = rFQSupplier.rqsComplete;
			dataRow["rqsCustomRate"] = rFQSupplier.rqsCustomRate;
			dataRow["rqsSelectedSupplier"] = rFQSupplier.rqsSelectedSupplier;
			dataRow["rqsUpdatedPartPrices"] = rFQSupplier.rqsUpdatedPartPrices;
			dataRow["rqsOrgPartID"] = rFQSupplier.rqsOrgPartID;
			dataRow["rqsPurchaseContactID"] = rFQSupplier.rqsPurchaseContactID;
			dataRow["rqsPurchaseLocationID"] = rFQSupplier.rqsPurchaseLocationID;
			DataRow dataRow3 = dataRow;
			rqsDueDate = rFQSupplier.rqsSelectedSupplierDate;
			dataRow3["rqsSelectedSupplierDate"] = (rqsDueDate.HasValue ? ((object)rqsDueDate.GetValueOrDefault()) : dataRow["rqsSelectedSupplierDate"]);
			dataRow["rqsSupplierOrganizationID"] = rFQSupplier.rqsSupplierOrganizationID;
			if (rFQSupplier.CustomFields != null && rFQSupplier.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rFQSupplier.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RFQSupplier [{rFQSupplier.rqsUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RFQSupplier [{rFQSupplier.rqsUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
