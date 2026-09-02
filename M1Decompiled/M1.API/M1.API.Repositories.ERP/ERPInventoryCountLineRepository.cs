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

public class ERPInventoryCountLineRepository : APIBaseRepository, IERPInventoryCountLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPInventoryCountLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInventoryCountLineExist(Guid inventoryCountLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("imqUniqueID|C", inventoryCountLineId);
		base.selectList.Add("imqUniqueID");
		return Task.FromResult(GetAsObject("InventoryCountLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInventoryCountLineInformationDto>> GetAllInventoryCountLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInventoryCountLineInformationDto> collection = new List<ERPInventoryCountLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"imqBinDescription", "imqCountedBy", "imqCountedDate", "imqCreatedBy", "imqCreatedDate", "imqUniqueID", "imqFinalCount", "imqInventoryCountID", "imqPartBinID", "imqPartClassID",
			"imqPartID", "imqPartRevisionID", "imqPartShortDescription", "imqPartWarehouseLocationID", "imqQuantityOnHand", "imqRowVersion", "imqInventoryCountLineID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("InventoryCountLines");
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
		using (DataTable dataTable = GetAsDataTable("InventoryCountLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInventoryCountLineInformationDto eRPInventoryCountLineInformationDto = new ERPInventoryCountLineInformationDto();
				eRPInventoryCountLineInformationDto.imqBinDescription = dataTable.Rows[i].Field<string>("imqBinDescription");
				eRPInventoryCountLineInformationDto.imqCountedBy = dataTable.Rows[i].Field<string>("imqCountedBy");
				eRPInventoryCountLineInformationDto.imqCountedDate = dataTable.Rows[i].Field<DateTime?>("imqCountedDate");
				eRPInventoryCountLineInformationDto.imqCreatedBy = dataTable.Rows[i].Field<string>("imqCreatedBy");
				eRPInventoryCountLineInformationDto.imqCreatedDate = dataTable.Rows[i].Field<DateTime?>("imqCreatedDate");
				eRPInventoryCountLineInformationDto.imqUniqueID = dataTable.Rows[i].Field<Guid>("imqUniqueID");
				eRPInventoryCountLineInformationDto.imqFinalCount = dataTable.Rows[i].Field<decimal>("imqFinalCount");
				eRPInventoryCountLineInformationDto.imqInventoryCountID = dataTable.Rows[i].Field<int>("imqInventoryCountID");
				eRPInventoryCountLineInformationDto.imqPartBinID = dataTable.Rows[i].Field<string>("imqPartBinID");
				eRPInventoryCountLineInformationDto.imqPartClassID = dataTable.Rows[i].Field<string>("imqPartClassID");
				eRPInventoryCountLineInformationDto.imqPartID = dataTable.Rows[i].Field<string>("imqPartID");
				eRPInventoryCountLineInformationDto.imqPartRevisionID = dataTable.Rows[i].Field<string>("imqPartRevisionID");
				eRPInventoryCountLineInformationDto.imqPartShortDescription = dataTable.Rows[i].Field<string>("imqPartShortDescription");
				eRPInventoryCountLineInformationDto.imqPartWarehouseLocationID = dataTable.Rows[i].Field<string>("imqPartWarehouseLocationID");
				eRPInventoryCountLineInformationDto.imqQuantityOnHand = dataTable.Rows[i].Field<decimal>("imqQuantityOnHand");
				eRPInventoryCountLineInformationDto.imqRowVersion = dataTable.Rows[i].Field<byte[]>("imqRowVersion");
				eRPInventoryCountLineInformationDto.imqInventoryCountLineID = dataTable.Rows[i].Field<int>("imqInventoryCountLineID");
				eRPInventoryCountLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInventoryCountLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInventoryCountLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInventoryCountLineInformationDto> GetInventoryCountLine(Guid inventoryCountLineId)
	{
		ERPInventoryCountLineInformationDto eRPInventoryCountLineInformationDto = new ERPInventoryCountLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"imqBinDescription", "imqCountedBy", "imqCountedDate", "imqCreatedBy", "imqCreatedDate", "imqUniqueID", "imqFinalCount", "imqInventoryCountID", "imqPartBinID", "imqPartClassID",
			"imqPartID", "imqPartRevisionID", "imqPartShortDescription", "imqPartWarehouseLocationID", "imqQuantityOnHand", "imqRowVersion", "imqInventoryCountLineID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imqUniqueID|C", inventoryCountLineId);
		AddCustomFieldsToSelectList("InventoryCountLines");
		using (DataTable dataTable = GetAsDataTable("InventoryCountLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInventoryCountLineInformationDto);
			}
			eRPInventoryCountLineInformationDto.imqBinDescription = dataTable.Rows[0].Field<string>("imqBinDescription");
			eRPInventoryCountLineInformationDto.imqCountedBy = dataTable.Rows[0].Field<string>("imqCountedBy");
			eRPInventoryCountLineInformationDto.imqCountedDate = dataTable.Rows[0].Field<DateTime?>("imqCountedDate");
			eRPInventoryCountLineInformationDto.imqCreatedBy = dataTable.Rows[0].Field<string>("imqCreatedBy");
			eRPInventoryCountLineInformationDto.imqCreatedDate = dataTable.Rows[0].Field<DateTime?>("imqCreatedDate");
			eRPInventoryCountLineInformationDto.imqUniqueID = dataTable.Rows[0].Field<Guid>("imqUniqueID");
			eRPInventoryCountLineInformationDto.imqFinalCount = dataTable.Rows[0].Field<decimal>("imqFinalCount");
			eRPInventoryCountLineInformationDto.imqInventoryCountID = dataTable.Rows[0].Field<int>("imqInventoryCountID");
			eRPInventoryCountLineInformationDto.imqPartBinID = dataTable.Rows[0].Field<string>("imqPartBinID");
			eRPInventoryCountLineInformationDto.imqPartClassID = dataTable.Rows[0].Field<string>("imqPartClassID");
			eRPInventoryCountLineInformationDto.imqPartID = dataTable.Rows[0].Field<string>("imqPartID");
			eRPInventoryCountLineInformationDto.imqPartRevisionID = dataTable.Rows[0].Field<string>("imqPartRevisionID");
			eRPInventoryCountLineInformationDto.imqPartShortDescription = dataTable.Rows[0].Field<string>("imqPartShortDescription");
			eRPInventoryCountLineInformationDto.imqPartWarehouseLocationID = dataTable.Rows[0].Field<string>("imqPartWarehouseLocationID");
			eRPInventoryCountLineInformationDto.imqQuantityOnHand = dataTable.Rows[0].Field<decimal>("imqQuantityOnHand");
			eRPInventoryCountLineInformationDto.imqRowVersion = dataTable.Rows[0].Field<byte[]>("imqRowVersion");
			eRPInventoryCountLineInformationDto.imqInventoryCountLineID = dataTable.Rows[0].Field<int>("imqInventoryCountLineID");
			eRPInventoryCountLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInventoryCountLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInventoryCountLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInventoryCountLine(ERPInventoryCountLineDto inventoryCountLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM InventoryCountLines WHERE imqUniqueID = " + M1Util.ConvertToLinq(inventoryCountLine.imqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imqInventoryCountID"] = inventoryCountLine.imqInventoryCountID;
				dataRow["imqInventoryCountLineID"] = inventoryCountLine.imqInventoryCountLineID;
				inventoryCountLine.imqUniqueID = ((inventoryCountLine.imqUniqueID == Guid.Empty) ? Guid.NewGuid() : inventoryCountLine.imqUniqueID);
				dataRow["imqUniqueID"] = inventoryCountLine.imqUniqueID;
				dataRow["imqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The InventoryCountLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (inventoryCountLine.imqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the InventoryCountLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imqRowVersion"], inventoryCountLine.imqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the InventoryCountLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the InventoryCountLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imqBinDescription"] = inventoryCountLine.imqBinDescription;
			dataRow["imqCountedBy"] = inventoryCountLine.imqCountedBy;
			DataRow dataRow2 = dataRow;
			DateTime? imqCountedDate = inventoryCountLine.imqCountedDate;
			dataRow2["imqCountedDate"] = (imqCountedDate.HasValue ? ((object)imqCountedDate.GetValueOrDefault()) : dataRow["imqCountedDate"]);
			dataRow["imqFinalCount"] = inventoryCountLine.imqFinalCount;
			dataRow["imqPartBinID"] = inventoryCountLine.imqPartBinID;
			dataRow["imqPartClassID"] = inventoryCountLine.imqPartClassID;
			dataRow["imqPartID"] = inventoryCountLine.imqPartID;
			dataRow["imqPartRevisionID"] = inventoryCountLine.imqPartRevisionID;
			dataRow["imqPartShortDescription"] = inventoryCountLine.imqPartShortDescription;
			dataRow["imqPartWarehouseLocationID"] = inventoryCountLine.imqPartWarehouseLocationID;
			dataRow["imqQuantityOnHand"] = inventoryCountLine.imqQuantityOnHand;
			if (inventoryCountLine.CustomFields != null && inventoryCountLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inventoryCountLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the InventoryCountLine [{inventoryCountLine.imqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the InventoryCountLine [{inventoryCountLine.imqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
