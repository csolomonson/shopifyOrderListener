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

public class ERPWarehouseRequisitionLineRepository : APIBaseRepository, IERPWarehouseRequisitionLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseRequisitionLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseRequisitionLineExist(Guid warehouseRequisitionLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("wqlUniqueID|C", warehouseRequisitionLineId);
		base.selectList.Add("wqlUniqueID");
		return Task.FromResult(GetAsObject("WarehouseRequisitionLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseRequisitionLineInformationDto>> GetAllWarehouseRequisitionLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseRequisitionLineInformationDto> collection = new List<ERPWarehouseRequisitionLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[16]
		{
			"wqlCreatedBy", "wqlCreatedDate", "wqlUniqueID", "wqlClosed", "wqlKitPart", "wqlTransferredComplete", "wqlPartDescription", "wqlPartID", "wqlPartRevisionID", "wqlQuantityTransferred",
			"wqlRequestedQuantity", "wqlRowVersion", "wqlWarehouseRequisitionLineID", "wqlSourceWarehouseID", "wqlUnitOfMeasure", "wqlWarehouseRequisitionID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseRequisitionLines");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitionLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseRequisitionLineInformationDto eRPWarehouseRequisitionLineInformationDto = new ERPWarehouseRequisitionLineInformationDto();
				eRPWarehouseRequisitionLineInformationDto.wqlCreatedBy = dataTable.Rows[i].Field<string>("wqlCreatedBy");
				eRPWarehouseRequisitionLineInformationDto.wqlCreatedDate = dataTable.Rows[i].Field<DateTime?>("wqlCreatedDate");
				eRPWarehouseRequisitionLineInformationDto.wqlUniqueID = dataTable.Rows[i].Field<Guid>("wqlUniqueID");
				eRPWarehouseRequisitionLineInformationDto.wqlClosed = dataTable.Rows[i].Field<bool>("wqlClosed");
				eRPWarehouseRequisitionLineInformationDto.wqlKitPart = dataTable.Rows[i].Field<bool>("wqlKitPart");
				eRPWarehouseRequisitionLineInformationDto.wqlTransferredComplete = dataTable.Rows[i].Field<bool>("wqlTransferredComplete");
				eRPWarehouseRequisitionLineInformationDto.wqlPartDescription = dataTable.Rows[i].Field<string>("wqlPartDescription");
				eRPWarehouseRequisitionLineInformationDto.wqlPartID = dataTable.Rows[i].Field<string>("wqlPartID");
				eRPWarehouseRequisitionLineInformationDto.wqlPartRevisionID = dataTable.Rows[i].Field<string>("wqlPartRevisionID");
				eRPWarehouseRequisitionLineInformationDto.wqlQuantityTransferred = dataTable.Rows[i].Field<decimal>("wqlQuantityTransferred");
				eRPWarehouseRequisitionLineInformationDto.wqlRequestedQuantity = dataTable.Rows[i].Field<decimal>("wqlRequestedQuantity");
				eRPWarehouseRequisitionLineInformationDto.wqlRowVersion = dataTable.Rows[i].Field<byte[]>("wqlRowVersion");
				eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("wqlWarehouseRequisitionLineID");
				eRPWarehouseRequisitionLineInformationDto.wqlSourceWarehouseID = dataTable.Rows[i].Field<string>("wqlSourceWarehouseID");
				eRPWarehouseRequisitionLineInformationDto.wqlUnitOfMeasure = dataTable.Rows[i].Field<string>("wqlUnitOfMeasure");
				eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionID = dataTable.Rows[i].Field<string>("wqlWarehouseRequisitionID");
				eRPWarehouseRequisitionLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseRequisitionLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseRequisitionLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseRequisitionLineInformationDto> GetWarehouseRequisitionLine(Guid warehouseRequisitionLineId)
	{
		ERPWarehouseRequisitionLineInformationDto eRPWarehouseRequisitionLineInformationDto = new ERPWarehouseRequisitionLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[16]
		{
			"wqlCreatedBy", "wqlCreatedDate", "wqlUniqueID", "wqlClosed", "wqlKitPart", "wqlTransferredComplete", "wqlPartDescription", "wqlPartID", "wqlPartRevisionID", "wqlQuantityTransferred",
			"wqlRequestedQuantity", "wqlRowVersion", "wqlWarehouseRequisitionLineID", "wqlSourceWarehouseID", "wqlUnitOfMeasure", "wqlWarehouseRequisitionID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wqlUniqueID|C", warehouseRequisitionLineId);
		AddCustomFieldsToSelectList("WarehouseRequisitionLines");
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitionLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseRequisitionLineInformationDto);
			}
			eRPWarehouseRequisitionLineInformationDto.wqlCreatedBy = dataTable.Rows[0].Field<string>("wqlCreatedBy");
			eRPWarehouseRequisitionLineInformationDto.wqlCreatedDate = dataTable.Rows[0].Field<DateTime?>("wqlCreatedDate");
			eRPWarehouseRequisitionLineInformationDto.wqlUniqueID = dataTable.Rows[0].Field<Guid>("wqlUniqueID");
			eRPWarehouseRequisitionLineInformationDto.wqlClosed = dataTable.Rows[0].Field<bool>("wqlClosed");
			eRPWarehouseRequisitionLineInformationDto.wqlKitPart = dataTable.Rows[0].Field<bool>("wqlKitPart");
			eRPWarehouseRequisitionLineInformationDto.wqlTransferredComplete = dataTable.Rows[0].Field<bool>("wqlTransferredComplete");
			eRPWarehouseRequisitionLineInformationDto.wqlPartDescription = dataTable.Rows[0].Field<string>("wqlPartDescription");
			eRPWarehouseRequisitionLineInformationDto.wqlPartID = dataTable.Rows[0].Field<string>("wqlPartID");
			eRPWarehouseRequisitionLineInformationDto.wqlPartRevisionID = dataTable.Rows[0].Field<string>("wqlPartRevisionID");
			eRPWarehouseRequisitionLineInformationDto.wqlQuantityTransferred = dataTable.Rows[0].Field<decimal>("wqlQuantityTransferred");
			eRPWarehouseRequisitionLineInformationDto.wqlRequestedQuantity = dataTable.Rows[0].Field<decimal>("wqlRequestedQuantity");
			eRPWarehouseRequisitionLineInformationDto.wqlRowVersion = dataTable.Rows[0].Field<byte[]>("wqlRowVersion");
			eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("wqlWarehouseRequisitionLineID");
			eRPWarehouseRequisitionLineInformationDto.wqlSourceWarehouseID = dataTable.Rows[0].Field<string>("wqlSourceWarehouseID");
			eRPWarehouseRequisitionLineInformationDto.wqlUnitOfMeasure = dataTable.Rows[0].Field<string>("wqlUnitOfMeasure");
			eRPWarehouseRequisitionLineInformationDto.wqlWarehouseRequisitionID = dataTable.Rows[0].Field<string>("wqlWarehouseRequisitionID");
			eRPWarehouseRequisitionLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseRequisitionLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseRequisitionLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseRequisitionLine(ERPWarehouseRequisitionLineDto warehouseRequisitionLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseRequisitionLines WHERE wqlUniqueID = " + M1Util.ConvertToLinq(warehouseRequisitionLine.wqlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wqlWarehouseRequisitionID"] = warehouseRequisitionLine.wqlWarehouseRequisitionID.ToUpper();
				dataRow["wqlWarehouseRequisitionLineID"] = warehouseRequisitionLine.wqlWarehouseRequisitionLineID;
				warehouseRequisitionLine.wqlUniqueID = ((warehouseRequisitionLine.wqlUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseRequisitionLine.wqlUniqueID);
				dataRow["wqlUniqueID"] = warehouseRequisitionLine.wqlUniqueID;
				dataRow["wqlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wqlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseRequisitionLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseRequisitionLine.wqlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseRequisitionLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wqlRowVersion"], warehouseRequisitionLine.wqlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseRequisitionLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseRequisitionLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["wqlClosed"] = warehouseRequisitionLine.wqlClosed;
			dataRow["wqlKitPart"] = warehouseRequisitionLine.wqlKitPart;
			dataRow["wqlTransferredComplete"] = warehouseRequisitionLine.wqlTransferredComplete;
			dataRow["wqlPartDescription"] = warehouseRequisitionLine.wqlPartDescription;
			dataRow["wqlPartID"] = warehouseRequisitionLine.wqlPartID;
			dataRow["wqlPartRevisionID"] = warehouseRequisitionLine.wqlPartRevisionID;
			dataRow["wqlQuantityTransferred"] = warehouseRequisitionLine.wqlQuantityTransferred;
			dataRow["wqlRequestedQuantity"] = warehouseRequisitionLine.wqlRequestedQuantity;
			dataRow["wqlSourceWarehouseID"] = warehouseRequisitionLine.wqlSourceWarehouseID;
			dataRow["wqlUnitOfMeasure"] = warehouseRequisitionLine.wqlUnitOfMeasure;
			if (warehouseRequisitionLine.CustomFields != null && warehouseRequisitionLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseRequisitionLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseRequisitionLine [{warehouseRequisitionLine.wqlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseRequisitionLine [{warehouseRequisitionLine.wqlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
