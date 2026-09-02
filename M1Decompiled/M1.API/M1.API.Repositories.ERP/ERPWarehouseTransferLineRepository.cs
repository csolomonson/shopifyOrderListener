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

public class ERPWarehouseTransferLineRepository : APIBaseRepository, IERPWarehouseTransferLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseTransferLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseTransferLineExist(Guid warehouseTransferLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("mwlUniqueID|C", warehouseTransferLineId);
		base.selectList.Add("mwlUniqueID");
		return Task.FromResult(GetAsObject("WarehouseTransferLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseTransferLineInformationDto>> GetAllWarehouseTransferLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseTransferLineInformationDto> collection = new List<ERPWarehouseTransferLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[29]
		{
			"mwlCreatedBy", "mwlCreatedDate", "mwlDestinationWarehouseID", "mwlUniqueID", "mwlClosed", "mwlKitPart", "mwlPosted", "mwlReceivedComplete", "mwlReversed", "mwlShippedComplete",
			"mwlPartDescription", "mwlPartID", "mwlPartRevisionID", "mwlQuantityInTransit", "mwlReceivedDate", "mwlReceivedQuantity", "mwlReverseWHTransferID", "mwlReverseWHTransferLineID", "mwlRowVersion", "mwlWarehouseTransferLineID",
			"mwlShipQuantity", "mwlSourcePartBinID", "mwlSourceWarehouseID", "mwlUnitOfMeasure", "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlWarehouseTransferID", "mwlWROpenQuantity", "mwlWRRequestedQuantity"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseTransferLines");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseTransferLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseTransferLineInformationDto eRPWarehouseTransferLineInformationDto = new ERPWarehouseTransferLineInformationDto();
				eRPWarehouseTransferLineInformationDto.mwlCreatedBy = dataTable.Rows[i].Field<string>("mwlCreatedBy");
				eRPWarehouseTransferLineInformationDto.mwlCreatedDate = dataTable.Rows[i].Field<DateTime?>("mwlCreatedDate");
				eRPWarehouseTransferLineInformationDto.mwlDestinationWarehouseID = dataTable.Rows[i].Field<string>("mwlDestinationWarehouseID");
				eRPWarehouseTransferLineInformationDto.mwlUniqueID = dataTable.Rows[i].Field<Guid>("mwlUniqueID");
				eRPWarehouseTransferLineInformationDto.mwlClosed = dataTable.Rows[i].Field<bool>("mwlClosed");
				eRPWarehouseTransferLineInformationDto.mwlKitPart = dataTable.Rows[i].Field<bool>("mwlKitPart");
				eRPWarehouseTransferLineInformationDto.mwlPosted = dataTable.Rows[i].Field<bool>("mwlPosted");
				eRPWarehouseTransferLineInformationDto.mwlReceivedComplete = dataTable.Rows[i].Field<bool>("mwlReceivedComplete");
				eRPWarehouseTransferLineInformationDto.mwlReversed = dataTable.Rows[i].Field<bool>("mwlReversed");
				eRPWarehouseTransferLineInformationDto.mwlShippedComplete = dataTable.Rows[i].Field<bool>("mwlShippedComplete");
				eRPWarehouseTransferLineInformationDto.mwlPartDescription = dataTable.Rows[i].Field<string>("mwlPartDescription");
				eRPWarehouseTransferLineInformationDto.mwlPartID = dataTable.Rows[i].Field<string>("mwlPartID");
				eRPWarehouseTransferLineInformationDto.mwlPartRevisionID = dataTable.Rows[i].Field<string>("mwlPartRevisionID");
				eRPWarehouseTransferLineInformationDto.mwlQuantityInTransit = dataTable.Rows[i].Field<decimal>("mwlQuantityInTransit");
				eRPWarehouseTransferLineInformationDto.mwlReceivedDate = dataTable.Rows[i].Field<DateTime?>("mwlReceivedDate");
				eRPWarehouseTransferLineInformationDto.mwlReceivedQuantity = dataTable.Rows[i].Field<decimal>("mwlReceivedQuantity");
				eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferID = dataTable.Rows[i].Field<string>("mwlReverseWHTransferID");
				eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferLineID = dataTable.Rows[i].Field<short>("mwlReverseWHTransferLineID");
				eRPWarehouseTransferLineInformationDto.mwlRowVersion = dataTable.Rows[i].Field<byte[]>("mwlRowVersion");
				eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferLineID = dataTable.Rows[i].Field<short>("mwlWarehouseTransferLineID");
				eRPWarehouseTransferLineInformationDto.mwlShipQuantity = dataTable.Rows[i].Field<decimal>("mwlShipQuantity");
				eRPWarehouseTransferLineInformationDto.mwlSourcePartBinID = dataTable.Rows[i].Field<string>("mwlSourcePartBinID");
				eRPWarehouseTransferLineInformationDto.mwlSourceWarehouseID = dataTable.Rows[i].Field<string>("mwlSourceWarehouseID");
				eRPWarehouseTransferLineInformationDto.mwlUnitOfMeasure = dataTable.Rows[i].Field<string>("mwlUnitOfMeasure");
				eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionID = dataTable.Rows[i].Field<string>("mwlWarehouseRequisitionID");
				eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("mwlWarehouseRequisitionLineID");
				eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferID = dataTable.Rows[i].Field<string>("mwlWarehouseTransferID");
				eRPWarehouseTransferLineInformationDto.mwlWROpenQuantity = dataTable.Rows[i].Field<decimal>("mwlWROpenQuantity");
				eRPWarehouseTransferLineInformationDto.mwlWRRequestedQuantity = dataTable.Rows[i].Field<decimal>("mwlWRRequestedQuantity");
				eRPWarehouseTransferLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseTransferLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseTransferLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseTransferLineInformationDto> GetWarehouseTransferLine(Guid warehouseTransferLineId)
	{
		ERPWarehouseTransferLineInformationDto eRPWarehouseTransferLineInformationDto = new ERPWarehouseTransferLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[29]
		{
			"mwlCreatedBy", "mwlCreatedDate", "mwlDestinationWarehouseID", "mwlUniqueID", "mwlClosed", "mwlKitPart", "mwlPosted", "mwlReceivedComplete", "mwlReversed", "mwlShippedComplete",
			"mwlPartDescription", "mwlPartID", "mwlPartRevisionID", "mwlQuantityInTransit", "mwlReceivedDate", "mwlReceivedQuantity", "mwlReverseWHTransferID", "mwlReverseWHTransferLineID", "mwlRowVersion", "mwlWarehouseTransferLineID",
			"mwlShipQuantity", "mwlSourcePartBinID", "mwlSourceWarehouseID", "mwlUnitOfMeasure", "mwlWarehouseRequisitionID", "mwlWarehouseRequisitionLineID", "mwlWarehouseTransferID", "mwlWROpenQuantity", "mwlWRRequestedQuantity"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mwlUniqueID|C", warehouseTransferLineId);
		AddCustomFieldsToSelectList("WarehouseTransferLines");
		using (DataTable dataTable = GetAsDataTable("WarehouseTransferLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseTransferLineInformationDto);
			}
			eRPWarehouseTransferLineInformationDto.mwlCreatedBy = dataTable.Rows[0].Field<string>("mwlCreatedBy");
			eRPWarehouseTransferLineInformationDto.mwlCreatedDate = dataTable.Rows[0].Field<DateTime?>("mwlCreatedDate");
			eRPWarehouseTransferLineInformationDto.mwlDestinationWarehouseID = dataTable.Rows[0].Field<string>("mwlDestinationWarehouseID");
			eRPWarehouseTransferLineInformationDto.mwlUniqueID = dataTable.Rows[0].Field<Guid>("mwlUniqueID");
			eRPWarehouseTransferLineInformationDto.mwlClosed = dataTable.Rows[0].Field<bool>("mwlClosed");
			eRPWarehouseTransferLineInformationDto.mwlKitPart = dataTable.Rows[0].Field<bool>("mwlKitPart");
			eRPWarehouseTransferLineInformationDto.mwlPosted = dataTable.Rows[0].Field<bool>("mwlPosted");
			eRPWarehouseTransferLineInformationDto.mwlReceivedComplete = dataTable.Rows[0].Field<bool>("mwlReceivedComplete");
			eRPWarehouseTransferLineInformationDto.mwlReversed = dataTable.Rows[0].Field<bool>("mwlReversed");
			eRPWarehouseTransferLineInformationDto.mwlShippedComplete = dataTable.Rows[0].Field<bool>("mwlShippedComplete");
			eRPWarehouseTransferLineInformationDto.mwlPartDescription = dataTable.Rows[0].Field<string>("mwlPartDescription");
			eRPWarehouseTransferLineInformationDto.mwlPartID = dataTable.Rows[0].Field<string>("mwlPartID");
			eRPWarehouseTransferLineInformationDto.mwlPartRevisionID = dataTable.Rows[0].Field<string>("mwlPartRevisionID");
			eRPWarehouseTransferLineInformationDto.mwlQuantityInTransit = dataTable.Rows[0].Field<decimal>("mwlQuantityInTransit");
			eRPWarehouseTransferLineInformationDto.mwlReceivedDate = dataTable.Rows[0].Field<DateTime?>("mwlReceivedDate");
			eRPWarehouseTransferLineInformationDto.mwlReceivedQuantity = dataTable.Rows[0].Field<decimal>("mwlReceivedQuantity");
			eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferID = dataTable.Rows[0].Field<string>("mwlReverseWHTransferID");
			eRPWarehouseTransferLineInformationDto.mwlReverseWHTransferLineID = dataTable.Rows[0].Field<short>("mwlReverseWHTransferLineID");
			eRPWarehouseTransferLineInformationDto.mwlRowVersion = dataTable.Rows[0].Field<byte[]>("mwlRowVersion");
			eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferLineID = dataTable.Rows[0].Field<short>("mwlWarehouseTransferLineID");
			eRPWarehouseTransferLineInformationDto.mwlShipQuantity = dataTable.Rows[0].Field<decimal>("mwlShipQuantity");
			eRPWarehouseTransferLineInformationDto.mwlSourcePartBinID = dataTable.Rows[0].Field<string>("mwlSourcePartBinID");
			eRPWarehouseTransferLineInformationDto.mwlSourceWarehouseID = dataTable.Rows[0].Field<string>("mwlSourceWarehouseID");
			eRPWarehouseTransferLineInformationDto.mwlUnitOfMeasure = dataTable.Rows[0].Field<string>("mwlUnitOfMeasure");
			eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionID = dataTable.Rows[0].Field<string>("mwlWarehouseRequisitionID");
			eRPWarehouseTransferLineInformationDto.mwlWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("mwlWarehouseRequisitionLineID");
			eRPWarehouseTransferLineInformationDto.mwlWarehouseTransferID = dataTable.Rows[0].Field<string>("mwlWarehouseTransferID");
			eRPWarehouseTransferLineInformationDto.mwlWROpenQuantity = dataTable.Rows[0].Field<decimal>("mwlWROpenQuantity");
			eRPWarehouseTransferLineInformationDto.mwlWRRequestedQuantity = dataTable.Rows[0].Field<decimal>("mwlWRRequestedQuantity");
			eRPWarehouseTransferLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseTransferLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseTransferLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseTransferLine(ERPWarehouseTransferLineDto warehouseTransferLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseTransferLines WHERE mwlUniqueID = " + M1Util.ConvertToLinq(warehouseTransferLine.mwlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mwlWarehouseTransferID"] = warehouseTransferLine.mwlWarehouseTransferID.ToUpper();
				dataRow["mwlWarehouseTransferLineID"] = warehouseTransferLine.mwlWarehouseTransferLineID;
				warehouseTransferLine.mwlUniqueID = ((warehouseTransferLine.mwlUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseTransferLine.mwlUniqueID);
				dataRow["mwlUniqueID"] = warehouseTransferLine.mwlUniqueID;
				dataRow["mwlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mwlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseTransferLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseTransferLine.mwlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseTransferLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mwlRowVersion"], warehouseTransferLine.mwlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseTransferLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseTransferLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mwlDestinationWarehouseID"] = warehouseTransferLine.mwlDestinationWarehouseID;
			dataRow["mwlClosed"] = warehouseTransferLine.mwlClosed;
			dataRow["mwlKitPart"] = warehouseTransferLine.mwlKitPart;
			dataRow["mwlPosted"] = warehouseTransferLine.mwlPosted;
			dataRow["mwlReceivedComplete"] = warehouseTransferLine.mwlReceivedComplete;
			dataRow["mwlReversed"] = warehouseTransferLine.mwlReversed;
			dataRow["mwlShippedComplete"] = warehouseTransferLine.mwlShippedComplete;
			dataRow["mwlPartDescription"] = warehouseTransferLine.mwlPartDescription;
			dataRow["mwlPartID"] = warehouseTransferLine.mwlPartID;
			dataRow["mwlPartRevisionID"] = warehouseTransferLine.mwlPartRevisionID;
			dataRow["mwlQuantityInTransit"] = warehouseTransferLine.mwlQuantityInTransit;
			DataRow dataRow2 = dataRow;
			DateTime? mwlReceivedDate = warehouseTransferLine.mwlReceivedDate;
			dataRow2["mwlReceivedDate"] = (mwlReceivedDate.HasValue ? ((object)mwlReceivedDate.GetValueOrDefault()) : dataRow["mwlReceivedDate"]);
			dataRow["mwlReceivedQuantity"] = warehouseTransferLine.mwlReceivedQuantity;
			dataRow["mwlReverseWHTransferID"] = warehouseTransferLine.mwlReverseWHTransferID;
			dataRow["mwlReverseWHTransferLineID"] = warehouseTransferLine.mwlReverseWHTransferLineID;
			dataRow["mwlShipQuantity"] = warehouseTransferLine.mwlShipQuantity;
			dataRow["mwlSourcePartBinID"] = warehouseTransferLine.mwlSourcePartBinID;
			dataRow["mwlSourceWarehouseID"] = warehouseTransferLine.mwlSourceWarehouseID;
			dataRow["mwlUnitOfMeasure"] = warehouseTransferLine.mwlUnitOfMeasure;
			dataRow["mwlWarehouseRequisitionID"] = warehouseTransferLine.mwlWarehouseRequisitionID;
			dataRow["mwlWarehouseRequisitionLineID"] = warehouseTransferLine.mwlWarehouseRequisitionLineID;
			dataRow["mwlWROpenQuantity"] = warehouseTransferLine.mwlWROpenQuantity;
			dataRow["mwlWRRequestedQuantity"] = warehouseTransferLine.mwlWRRequestedQuantity;
			if (warehouseTransferLine.CustomFields != null && warehouseTransferLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseTransferLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseTransferLine [{warehouseTransferLine.mwlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseTransferLine [{warehouseTransferLine.mwlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
