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

public class ERPLandedCostChargeDetailRepository : APIBaseRepository, IERPLandedCostChargeDetailRepository, IAPIBaseRepository, IDisposable
{
	public ERPLandedCostChargeDetailRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesLandedCostChargeDetailExist(Guid landedCostChargeDetailId)
	{
		InitializeParameterLists();
		base.filterList.Add("rmiUniqueID|C", landedCostChargeDetailId);
		base.selectList.Add("rmiUniqueID");
		return Task.FromResult(GetAsObject("LandedCostChargeDetails", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPLandedCostChargeDetailInformationDto>> GetAllLandedCostChargeDetails(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPLandedCostChargeDetailInformationDto> collection = new List<ERPLandedCostChargeDetailInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"rmiCreatedBy", "rmiCreatedDate", "rmiUniqueID", "rmiEstTotalCost", "rmiEstTotalCostForeign", "rmiLandedCostChargeID", "rmiLandedCostID", "rmiPurchaseOrderID", "rmiPurchaseOrderLineID", "rmiRowVersion",
			"rmiLandedCostChargeDetailID", "rmiTotalCost", "rmiTotalCostForeign"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("LandedCostChargeDetails");
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
		using (DataTable dataTable = GetAsDataTable("LandedCostChargeDetails", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPLandedCostChargeDetailInformationDto eRPLandedCostChargeDetailInformationDto = new ERPLandedCostChargeDetailInformationDto();
				eRPLandedCostChargeDetailInformationDto.rmiCreatedBy = dataTable.Rows[i].Field<string>("rmiCreatedBy");
				eRPLandedCostChargeDetailInformationDto.rmiCreatedDate = dataTable.Rows[i].Field<DateTime?>("rmiCreatedDate");
				eRPLandedCostChargeDetailInformationDto.rmiUniqueID = dataTable.Rows[i].Field<Guid>("rmiUniqueID");
				eRPLandedCostChargeDetailInformationDto.rmiEstTotalCost = dataTable.Rows[i].Field<decimal>("rmiEstTotalCost");
				eRPLandedCostChargeDetailInformationDto.rmiEstTotalCostForeign = dataTable.Rows[i].Field<decimal>("rmiEstTotalCostForeign");
				eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeID = dataTable.Rows[i].Field<short>("rmiLandedCostChargeID");
				eRPLandedCostChargeDetailInformationDto.rmiLandedCostID = dataTable.Rows[i].Field<string>("rmiLandedCostID");
				eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderID = dataTable.Rows[i].Field<string>("rmiPurchaseOrderID");
				eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderLineID = dataTable.Rows[i].Field<short>("rmiPurchaseOrderLineID");
				eRPLandedCostChargeDetailInformationDto.rmiRowVersion = dataTable.Rows[i].Field<byte[]>("rmiRowVersion");
				eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeDetailID = dataTable.Rows[i].Field<int>("rmiLandedCostChargeDetailID");
				eRPLandedCostChargeDetailInformationDto.rmiTotalCost = dataTable.Rows[i].Field<decimal>("rmiTotalCost");
				eRPLandedCostChargeDetailInformationDto.rmiTotalCostForeign = dataTable.Rows[i].Field<decimal>("rmiTotalCostForeign");
				eRPLandedCostChargeDetailInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPLandedCostChargeDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPLandedCostChargeDetailInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPLandedCostChargeDetailInformationDto> GetLandedCostChargeDetail(Guid landedCostChargeDetailId)
	{
		ERPLandedCostChargeDetailInformationDto eRPLandedCostChargeDetailInformationDto = new ERPLandedCostChargeDetailInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"rmiCreatedBy", "rmiCreatedDate", "rmiUniqueID", "rmiEstTotalCost", "rmiEstTotalCostForeign", "rmiLandedCostChargeID", "rmiLandedCostID", "rmiPurchaseOrderID", "rmiPurchaseOrderLineID", "rmiRowVersion",
			"rmiLandedCostChargeDetailID", "rmiTotalCost", "rmiTotalCostForeign"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rmiUniqueID|C", landedCostChargeDetailId);
		AddCustomFieldsToSelectList("LandedCostChargeDetails");
		using (DataTable dataTable = GetAsDataTable("LandedCostChargeDetails", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPLandedCostChargeDetailInformationDto);
			}
			eRPLandedCostChargeDetailInformationDto.rmiCreatedBy = dataTable.Rows[0].Field<string>("rmiCreatedBy");
			eRPLandedCostChargeDetailInformationDto.rmiCreatedDate = dataTable.Rows[0].Field<DateTime?>("rmiCreatedDate");
			eRPLandedCostChargeDetailInformationDto.rmiUniqueID = dataTable.Rows[0].Field<Guid>("rmiUniqueID");
			eRPLandedCostChargeDetailInformationDto.rmiEstTotalCost = dataTable.Rows[0].Field<decimal>("rmiEstTotalCost");
			eRPLandedCostChargeDetailInformationDto.rmiEstTotalCostForeign = dataTable.Rows[0].Field<decimal>("rmiEstTotalCostForeign");
			eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeID = dataTable.Rows[0].Field<short>("rmiLandedCostChargeID");
			eRPLandedCostChargeDetailInformationDto.rmiLandedCostID = dataTable.Rows[0].Field<string>("rmiLandedCostID");
			eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderID = dataTable.Rows[0].Field<string>("rmiPurchaseOrderID");
			eRPLandedCostChargeDetailInformationDto.rmiPurchaseOrderLineID = dataTable.Rows[0].Field<short>("rmiPurchaseOrderLineID");
			eRPLandedCostChargeDetailInformationDto.rmiRowVersion = dataTable.Rows[0].Field<byte[]>("rmiRowVersion");
			eRPLandedCostChargeDetailInformationDto.rmiLandedCostChargeDetailID = dataTable.Rows[0].Field<int>("rmiLandedCostChargeDetailID");
			eRPLandedCostChargeDetailInformationDto.rmiTotalCost = dataTable.Rows[0].Field<decimal>("rmiTotalCost");
			eRPLandedCostChargeDetailInformationDto.rmiTotalCostForeign = dataTable.Rows[0].Field<decimal>("rmiTotalCostForeign");
			eRPLandedCostChargeDetailInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPLandedCostChargeDetailInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPLandedCostChargeDetailInformationDto);
	}

	public Task<APIValidationInfoDto> SaveLandedCostChargeDetail(ERPLandedCostChargeDetailDto landedCostChargeDetail)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM LandedCostChargeDetails WHERE rmiUniqueID = " + M1Util.ConvertToLinq(landedCostChargeDetail.rmiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rmiLandedCostID"] = landedCostChargeDetail.rmiLandedCostID.ToUpper();
				dataRow["rmiLandedCostChargeID"] = landedCostChargeDetail.rmiLandedCostChargeID;
				dataRow["rmiLandedCostChargeDetailID"] = landedCostChargeDetail.rmiLandedCostChargeDetailID;
				landedCostChargeDetail.rmiUniqueID = ((landedCostChargeDetail.rmiUniqueID == Guid.Empty) ? Guid.NewGuid() : landedCostChargeDetail.rmiUniqueID);
				dataRow["rmiUniqueID"] = landedCostChargeDetail.rmiUniqueID;
				dataRow["rmiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rmiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The LandedCostChargeDetail could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (landedCostChargeDetail.rmiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the LandedCostChargeDetail is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rmiRowVersion"], landedCostChargeDetail.rmiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the LandedCostChargeDetail has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the LandedCostChargeDetail again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rmiEstTotalCost"] = landedCostChargeDetail.rmiEstTotalCost;
			dataRow["rmiEstTotalCostForeign"] = landedCostChargeDetail.rmiEstTotalCostForeign;
			dataRow["rmiPurchaseOrderID"] = landedCostChargeDetail.rmiPurchaseOrderID;
			dataRow["rmiPurchaseOrderLineID"] = landedCostChargeDetail.rmiPurchaseOrderLineID;
			dataRow["rmiTotalCost"] = landedCostChargeDetail.rmiTotalCost;
			dataRow["rmiTotalCostForeign"] = landedCostChargeDetail.rmiTotalCostForeign;
			if (landedCostChargeDetail.CustomFields != null && landedCostChargeDetail.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in landedCostChargeDetail.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the LandedCostChargeDetail [{landedCostChargeDetail.rmiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the LandedCostChargeDetail [{landedCostChargeDetail.rmiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
