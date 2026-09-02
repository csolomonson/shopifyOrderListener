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

public class ERPSalesOrderComponentRepository : APIBaseRepository, IERPSalesOrderComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPSalesOrderComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesSalesOrderComponentExist(Guid salesOrderComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("omoUniqueID|C", salesOrderComponentId);
		base.selectList.Add("omoUniqueID");
		return Task.FromResult(GetAsObject("SalesOrderComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPSalesOrderComponentInformationDto>> GetAllSalesOrderComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPSalesOrderComponentInformationDto> collection = new List<ERPSalesOrderComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[23]
		{
			"omoAdditionalQuantity", "omoCreatedBy", "omoCreatedDate", "omoDeliveryQuantity", "omoDescription", "omoUniqueID", "omoClosed", "omoShippedComplete", "omoParentQuantity", "omoPartBinID",
			"omoPartID", "omoPartRevisionID", "omoPartWarehouseLocationID", "omoQuantityAllocated", "omoQuantityPerParent", "omoQuantityShipped", "omoRowVersion", "omoSalesOrderDeliveryID", "omoSalesOrderID", "omoSalesOrderLineID",
			"omoSalesOrderComponentID", "omoUnitOfMeasure", "omoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("SalesOrderComponents");
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
		using (DataTable dataTable = GetAsDataTable("SalesOrderComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPSalesOrderComponentInformationDto eRPSalesOrderComponentInformationDto = new ERPSalesOrderComponentInformationDto();
				eRPSalesOrderComponentInformationDto.omoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("omoAdditionalQuantity");
				eRPSalesOrderComponentInformationDto.omoCreatedBy = dataTable.Rows[i].Field<string>("omoCreatedBy");
				eRPSalesOrderComponentInformationDto.omoCreatedDate = dataTable.Rows[i].Field<DateTime?>("omoCreatedDate");
				eRPSalesOrderComponentInformationDto.omoDeliveryQuantity = dataTable.Rows[i].Field<decimal>("omoDeliveryQuantity");
				eRPSalesOrderComponentInformationDto.omoDescription = dataTable.Rows[i].Field<string>("omoDescription");
				eRPSalesOrderComponentInformationDto.omoUniqueID = dataTable.Rows[i].Field<Guid>("omoUniqueID");
				eRPSalesOrderComponentInformationDto.omoClosed = dataTable.Rows[i].Field<bool>("omoClosed");
				eRPSalesOrderComponentInformationDto.omoShippedComplete = dataTable.Rows[i].Field<bool>("omoShippedComplete");
				eRPSalesOrderComponentInformationDto.omoParentQuantity = dataTable.Rows[i].Field<decimal>("omoParentQuantity");
				eRPSalesOrderComponentInformationDto.omoPartBinID = dataTable.Rows[i].Field<string>("omoPartBinID");
				eRPSalesOrderComponentInformationDto.omoPartID = dataTable.Rows[i].Field<string>("omoPartID");
				eRPSalesOrderComponentInformationDto.omoPartRevisionID = dataTable.Rows[i].Field<string>("omoPartRevisionID");
				eRPSalesOrderComponentInformationDto.omoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("omoPartWarehouseLocationID");
				eRPSalesOrderComponentInformationDto.omoQuantityAllocated = dataTable.Rows[i].Field<decimal>("omoQuantityAllocated");
				eRPSalesOrderComponentInformationDto.omoQuantityPerParent = dataTable.Rows[i].Field<decimal>("omoQuantityPerParent");
				eRPSalesOrderComponentInformationDto.omoQuantityShipped = dataTable.Rows[i].Field<decimal>("omoQuantityShipped");
				eRPSalesOrderComponentInformationDto.omoRowVersion = dataTable.Rows[i].Field<byte[]>("omoRowVersion");
				eRPSalesOrderComponentInformationDto.omoSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("omoSalesOrderDeliveryID");
				eRPSalesOrderComponentInformationDto.omoSalesOrderID = dataTable.Rows[i].Field<string>("omoSalesOrderID");
				eRPSalesOrderComponentInformationDto.omoSalesOrderLineID = dataTable.Rows[i].Field<short>("omoSalesOrderLineID");
				eRPSalesOrderComponentInformationDto.omoSalesOrderComponentID = dataTable.Rows[i].Field<short>("omoSalesOrderComponentID");
				eRPSalesOrderComponentInformationDto.omoUnitOfMeasure = dataTable.Rows[i].Field<string>("omoUnitOfMeasure");
				eRPSalesOrderComponentInformationDto.omoWeight = dataTable.Rows[i].Field<decimal>("omoWeight");
				eRPSalesOrderComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPSalesOrderComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPSalesOrderComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPSalesOrderComponentInformationDto> GetSalesOrderComponent(Guid salesOrderComponentId)
	{
		ERPSalesOrderComponentInformationDto eRPSalesOrderComponentInformationDto = new ERPSalesOrderComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[23]
		{
			"omoAdditionalQuantity", "omoCreatedBy", "omoCreatedDate", "omoDeliveryQuantity", "omoDescription", "omoUniqueID", "omoClosed", "omoShippedComplete", "omoParentQuantity", "omoPartBinID",
			"omoPartID", "omoPartRevisionID", "omoPartWarehouseLocationID", "omoQuantityAllocated", "omoQuantityPerParent", "omoQuantityShipped", "omoRowVersion", "omoSalesOrderDeliveryID", "omoSalesOrderID", "omoSalesOrderLineID",
			"omoSalesOrderComponentID", "omoUnitOfMeasure", "omoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("omoUniqueID|C", salesOrderComponentId);
		AddCustomFieldsToSelectList("SalesOrderComponents");
		using (DataTable dataTable = GetAsDataTable("SalesOrderComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPSalesOrderComponentInformationDto);
			}
			eRPSalesOrderComponentInformationDto.omoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("omoAdditionalQuantity");
			eRPSalesOrderComponentInformationDto.omoCreatedBy = dataTable.Rows[0].Field<string>("omoCreatedBy");
			eRPSalesOrderComponentInformationDto.omoCreatedDate = dataTable.Rows[0].Field<DateTime?>("omoCreatedDate");
			eRPSalesOrderComponentInformationDto.omoDeliveryQuantity = dataTable.Rows[0].Field<decimal>("omoDeliveryQuantity");
			eRPSalesOrderComponentInformationDto.omoDescription = dataTable.Rows[0].Field<string>("omoDescription");
			eRPSalesOrderComponentInformationDto.omoUniqueID = dataTable.Rows[0].Field<Guid>("omoUniqueID");
			eRPSalesOrderComponentInformationDto.omoClosed = dataTable.Rows[0].Field<bool>("omoClosed");
			eRPSalesOrderComponentInformationDto.omoShippedComplete = dataTable.Rows[0].Field<bool>("omoShippedComplete");
			eRPSalesOrderComponentInformationDto.omoParentQuantity = dataTable.Rows[0].Field<decimal>("omoParentQuantity");
			eRPSalesOrderComponentInformationDto.omoPartBinID = dataTable.Rows[0].Field<string>("omoPartBinID");
			eRPSalesOrderComponentInformationDto.omoPartID = dataTable.Rows[0].Field<string>("omoPartID");
			eRPSalesOrderComponentInformationDto.omoPartRevisionID = dataTable.Rows[0].Field<string>("omoPartRevisionID");
			eRPSalesOrderComponentInformationDto.omoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("omoPartWarehouseLocationID");
			eRPSalesOrderComponentInformationDto.omoQuantityAllocated = dataTable.Rows[0].Field<decimal>("omoQuantityAllocated");
			eRPSalesOrderComponentInformationDto.omoQuantityPerParent = dataTable.Rows[0].Field<decimal>("omoQuantityPerParent");
			eRPSalesOrderComponentInformationDto.omoQuantityShipped = dataTable.Rows[0].Field<decimal>("omoQuantityShipped");
			eRPSalesOrderComponentInformationDto.omoRowVersion = dataTable.Rows[0].Field<byte[]>("omoRowVersion");
			eRPSalesOrderComponentInformationDto.omoSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("omoSalesOrderDeliveryID");
			eRPSalesOrderComponentInformationDto.omoSalesOrderID = dataTable.Rows[0].Field<string>("omoSalesOrderID");
			eRPSalesOrderComponentInformationDto.omoSalesOrderLineID = dataTable.Rows[0].Field<short>("omoSalesOrderLineID");
			eRPSalesOrderComponentInformationDto.omoSalesOrderComponentID = dataTable.Rows[0].Field<short>("omoSalesOrderComponentID");
			eRPSalesOrderComponentInformationDto.omoUnitOfMeasure = dataTable.Rows[0].Field<string>("omoUnitOfMeasure");
			eRPSalesOrderComponentInformationDto.omoWeight = dataTable.Rows[0].Field<decimal>("omoWeight");
			eRPSalesOrderComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPSalesOrderComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPSalesOrderComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveSalesOrderComponent(ERPSalesOrderComponentDto salesOrderComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM SalesOrderComponents WHERE omoUniqueID = " + M1Util.ConvertToLinq(salesOrderComponent.omoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["omoSalesOrderID"] = salesOrderComponent.omoSalesOrderID.ToUpper();
				dataRow["omoSalesOrderLineID"] = salesOrderComponent.omoSalesOrderLineID;
				dataRow["omoSalesOrderDeliveryID"] = salesOrderComponent.omoSalesOrderDeliveryID;
				dataRow["omoSalesOrderComponentID"] = salesOrderComponent.omoSalesOrderComponentID;
				salesOrderComponent.omoUniqueID = ((salesOrderComponent.omoUniqueID == Guid.Empty) ? Guid.NewGuid() : salesOrderComponent.omoUniqueID);
				dataRow["omoUniqueID"] = salesOrderComponent.omoUniqueID;
				dataRow["omoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["omoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The SalesOrderComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (salesOrderComponent.omoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the SalesOrderComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["omoRowVersion"], salesOrderComponent.omoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the SalesOrderComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the SalesOrderComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["omoAdditionalQuantity"] = salesOrderComponent.omoAdditionalQuantity;
			dataRow["omoDeliveryQuantity"] = salesOrderComponent.omoDeliveryQuantity;
			dataRow["omoDescription"] = salesOrderComponent.omoDescription;
			dataRow["omoClosed"] = salesOrderComponent.omoClosed;
			dataRow["omoShippedComplete"] = salesOrderComponent.omoShippedComplete;
			dataRow["omoParentQuantity"] = salesOrderComponent.omoParentQuantity;
			dataRow["omoPartBinID"] = salesOrderComponent.omoPartBinID;
			dataRow["omoPartID"] = salesOrderComponent.omoPartID;
			dataRow["omoPartRevisionID"] = salesOrderComponent.omoPartRevisionID;
			dataRow["omoPartWarehouseLocationID"] = salesOrderComponent.omoPartWarehouseLocationID;
			dataRow["omoQuantityAllocated"] = salesOrderComponent.omoQuantityAllocated;
			dataRow["omoQuantityPerParent"] = salesOrderComponent.omoQuantityPerParent;
			dataRow["omoQuantityShipped"] = salesOrderComponent.omoQuantityShipped;
			dataRow["omoUnitOfMeasure"] = salesOrderComponent.omoUnitOfMeasure;
			dataRow["omoWeight"] = salesOrderComponent.omoWeight;
			if (salesOrderComponent.CustomFields != null && salesOrderComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in salesOrderComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the SalesOrderComponent [{salesOrderComponent.omoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the SalesOrderComponent [{salesOrderComponent.omoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
