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

public class ERPShipmentComponentRepository : APIBaseRepository, IERPShipmentComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPShipmentComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShipmentComponentExist(Guid shipmentComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("smoUniqueID|C", shipmentComponentId);
		base.selectList.Add("smoUniqueID");
		return Task.FromResult(GetAsObject("ShipmentComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShipmentComponentInformationDto>> GetAllShipmentComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShipmentComponentInformationDto> collection = new List<ERPShipmentComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[34]
		{
			"smoAdditionalQuantity", "smoCreatedBy", "smoCreatedDate", "smoDescription", "smoUniqueID", "smoClosed", "smoPostedToGl", "smoReversed", "smoShippedComplete", "smoJobID",
			"smoJobParentQuantity", "smoJobQuantityShipped", "smoParentQuantity", "smoPartBinID", "smoPartID", "smoPartRevisionID", "smoPartWarehouseLocationID", "smoQuantityPerParent", "smoQuantityShipped", "smoReverseShipmentComponentID",
			"smoReverseShipmentID", "smoReverseShipmentLineID", "smoRowVersion", "smoSalesOrderComponentID", "smoSalesOrderDeliveryID", "smoSalesOrderID", "smoSalesOrderLineID", "smoShipmentComponentID", "smoShipmentID", "smoShipmentLineID",
			"smoSourceTableName", "smoSourceTableUniqueID", "smoUnitOfMeasure", "smoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShipmentComponents");
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
		using (DataTable dataTable = GetAsDataTable("ShipmentComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShipmentComponentInformationDto eRPShipmentComponentInformationDto = new ERPShipmentComponentInformationDto();
				eRPShipmentComponentInformationDto.smoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("smoAdditionalQuantity");
				eRPShipmentComponentInformationDto.smoCreatedBy = dataTable.Rows[i].Field<string>("smoCreatedBy");
				eRPShipmentComponentInformationDto.smoCreatedDate = dataTable.Rows[i].Field<DateTime?>("smoCreatedDate");
				eRPShipmentComponentInformationDto.smoDescription = dataTable.Rows[i].Field<string>("smoDescription");
				eRPShipmentComponentInformationDto.smoUniqueID = dataTable.Rows[i].Field<Guid>("smoUniqueID");
				eRPShipmentComponentInformationDto.smoClosed = dataTable.Rows[i].Field<bool>("smoClosed");
				eRPShipmentComponentInformationDto.smoPostedToGl = dataTable.Rows[i].Field<bool>("smoPostedToGl");
				eRPShipmentComponentInformationDto.smoReversed = dataTable.Rows[i].Field<bool>("smoReversed");
				eRPShipmentComponentInformationDto.smoShippedComplete = dataTable.Rows[i].Field<bool>("smoShippedComplete");
				eRPShipmentComponentInformationDto.smoJobID = dataTable.Rows[i].Field<string>("smoJobID");
				eRPShipmentComponentInformationDto.smoJobParentQuantity = dataTable.Rows[i].Field<decimal>("smoJobParentQuantity");
				eRPShipmentComponentInformationDto.smoJobQuantityShipped = dataTable.Rows[i].Field<decimal>("smoJobQuantityShipped");
				eRPShipmentComponentInformationDto.smoParentQuantity = dataTable.Rows[i].Field<decimal>("smoParentQuantity");
				eRPShipmentComponentInformationDto.smoPartBinID = dataTable.Rows[i].Field<string>("smoPartBinID");
				eRPShipmentComponentInformationDto.smoPartID = dataTable.Rows[i].Field<string>("smoPartID");
				eRPShipmentComponentInformationDto.smoPartRevisionID = dataTable.Rows[i].Field<string>("smoPartRevisionID");
				eRPShipmentComponentInformationDto.smoPartWarehouseLocationID = dataTable.Rows[i].Field<string>("smoPartWarehouseLocationID");
				eRPShipmentComponentInformationDto.smoQuantityPerParent = dataTable.Rows[i].Field<decimal>("smoQuantityPerParent");
				eRPShipmentComponentInformationDto.smoQuantityShipped = dataTable.Rows[i].Field<decimal>("smoQuantityShipped");
				eRPShipmentComponentInformationDto.smoReverseShipmentComponentID = dataTable.Rows[i].Field<short>("smoReverseShipmentComponentID");
				eRPShipmentComponentInformationDto.smoReverseShipmentID = dataTable.Rows[i].Field<string>("smoReverseShipmentID");
				eRPShipmentComponentInformationDto.smoReverseShipmentLineID = dataTable.Rows[i].Field<short>("smoReverseShipmentLineID");
				eRPShipmentComponentInformationDto.smoRowVersion = dataTable.Rows[i].Field<byte[]>("smoRowVersion");
				eRPShipmentComponentInformationDto.smoSalesOrderComponentID = dataTable.Rows[i].Field<short>("smoSalesOrderComponentID");
				eRPShipmentComponentInformationDto.smoSalesOrderDeliveryID = dataTable.Rows[i].Field<short>("smoSalesOrderDeliveryID");
				eRPShipmentComponentInformationDto.smoSalesOrderID = dataTable.Rows[i].Field<string>("smoSalesOrderID");
				eRPShipmentComponentInformationDto.smoSalesOrderLineID = dataTable.Rows[i].Field<short>("smoSalesOrderLineID");
				eRPShipmentComponentInformationDto.smoShipmentComponentID = dataTable.Rows[i].Field<short>("smoShipmentComponentID");
				eRPShipmentComponentInformationDto.smoShipmentID = dataTable.Rows[i].Field<string>("smoShipmentID");
				eRPShipmentComponentInformationDto.smoShipmentLineID = dataTable.Rows[i].Field<short>("smoShipmentLineID");
				eRPShipmentComponentInformationDto.smoSourceTableName = dataTable.Rows[i].Field<string>("smoSourceTableName");
				eRPShipmentComponentInformationDto.smoSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("smoSourceTableUniqueID");
				eRPShipmentComponentInformationDto.smoUnitOfMeasure = dataTable.Rows[i].Field<string>("smoUnitOfMeasure");
				eRPShipmentComponentInformationDto.smoWeight = dataTable.Rows[i].Field<decimal>("smoWeight");
				eRPShipmentComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShipmentComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShipmentComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShipmentComponentInformationDto> GetShipmentComponent(Guid shipmentComponentId)
	{
		ERPShipmentComponentInformationDto eRPShipmentComponentInformationDto = new ERPShipmentComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[34]
		{
			"smoAdditionalQuantity", "smoCreatedBy", "smoCreatedDate", "smoDescription", "smoUniqueID", "smoClosed", "smoPostedToGl", "smoReversed", "smoShippedComplete", "smoJobID",
			"smoJobParentQuantity", "smoJobQuantityShipped", "smoParentQuantity", "smoPartBinID", "smoPartID", "smoPartRevisionID", "smoPartWarehouseLocationID", "smoQuantityPerParent", "smoQuantityShipped", "smoReverseShipmentComponentID",
			"smoReverseShipmentID", "smoReverseShipmentLineID", "smoRowVersion", "smoSalesOrderComponentID", "smoSalesOrderDeliveryID", "smoSalesOrderID", "smoSalesOrderLineID", "smoShipmentComponentID", "smoShipmentID", "smoShipmentLineID",
			"smoSourceTableName", "smoSourceTableUniqueID", "smoUnitOfMeasure", "smoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("smoUniqueID|C", shipmentComponentId);
		AddCustomFieldsToSelectList("ShipmentComponents");
		using (DataTable dataTable = GetAsDataTable("ShipmentComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShipmentComponentInformationDto);
			}
			eRPShipmentComponentInformationDto.smoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("smoAdditionalQuantity");
			eRPShipmentComponentInformationDto.smoCreatedBy = dataTable.Rows[0].Field<string>("smoCreatedBy");
			eRPShipmentComponentInformationDto.smoCreatedDate = dataTable.Rows[0].Field<DateTime?>("smoCreatedDate");
			eRPShipmentComponentInformationDto.smoDescription = dataTable.Rows[0].Field<string>("smoDescription");
			eRPShipmentComponentInformationDto.smoUniqueID = dataTable.Rows[0].Field<Guid>("smoUniqueID");
			eRPShipmentComponentInformationDto.smoClosed = dataTable.Rows[0].Field<bool>("smoClosed");
			eRPShipmentComponentInformationDto.smoPostedToGl = dataTable.Rows[0].Field<bool>("smoPostedToGl");
			eRPShipmentComponentInformationDto.smoReversed = dataTable.Rows[0].Field<bool>("smoReversed");
			eRPShipmentComponentInformationDto.smoShippedComplete = dataTable.Rows[0].Field<bool>("smoShippedComplete");
			eRPShipmentComponentInformationDto.smoJobID = dataTable.Rows[0].Field<string>("smoJobID");
			eRPShipmentComponentInformationDto.smoJobParentQuantity = dataTable.Rows[0].Field<decimal>("smoJobParentQuantity");
			eRPShipmentComponentInformationDto.smoJobQuantityShipped = dataTable.Rows[0].Field<decimal>("smoJobQuantityShipped");
			eRPShipmentComponentInformationDto.smoParentQuantity = dataTable.Rows[0].Field<decimal>("smoParentQuantity");
			eRPShipmentComponentInformationDto.smoPartBinID = dataTable.Rows[0].Field<string>("smoPartBinID");
			eRPShipmentComponentInformationDto.smoPartID = dataTable.Rows[0].Field<string>("smoPartID");
			eRPShipmentComponentInformationDto.smoPartRevisionID = dataTable.Rows[0].Field<string>("smoPartRevisionID");
			eRPShipmentComponentInformationDto.smoPartWarehouseLocationID = dataTable.Rows[0].Field<string>("smoPartWarehouseLocationID");
			eRPShipmentComponentInformationDto.smoQuantityPerParent = dataTable.Rows[0].Field<decimal>("smoQuantityPerParent");
			eRPShipmentComponentInformationDto.smoQuantityShipped = dataTable.Rows[0].Field<decimal>("smoQuantityShipped");
			eRPShipmentComponentInformationDto.smoReverseShipmentComponentID = dataTable.Rows[0].Field<short>("smoReverseShipmentComponentID");
			eRPShipmentComponentInformationDto.smoReverseShipmentID = dataTable.Rows[0].Field<string>("smoReverseShipmentID");
			eRPShipmentComponentInformationDto.smoReverseShipmentLineID = dataTable.Rows[0].Field<short>("smoReverseShipmentLineID");
			eRPShipmentComponentInformationDto.smoRowVersion = dataTable.Rows[0].Field<byte[]>("smoRowVersion");
			eRPShipmentComponentInformationDto.smoSalesOrderComponentID = dataTable.Rows[0].Field<short>("smoSalesOrderComponentID");
			eRPShipmentComponentInformationDto.smoSalesOrderDeliveryID = dataTable.Rows[0].Field<short>("smoSalesOrderDeliveryID");
			eRPShipmentComponentInformationDto.smoSalesOrderID = dataTable.Rows[0].Field<string>("smoSalesOrderID");
			eRPShipmentComponentInformationDto.smoSalesOrderLineID = dataTable.Rows[0].Field<short>("smoSalesOrderLineID");
			eRPShipmentComponentInformationDto.smoShipmentComponentID = dataTable.Rows[0].Field<short>("smoShipmentComponentID");
			eRPShipmentComponentInformationDto.smoShipmentID = dataTable.Rows[0].Field<string>("smoShipmentID");
			eRPShipmentComponentInformationDto.smoShipmentLineID = dataTable.Rows[0].Field<short>("smoShipmentLineID");
			eRPShipmentComponentInformationDto.smoSourceTableName = dataTable.Rows[0].Field<string>("smoSourceTableName");
			eRPShipmentComponentInformationDto.smoSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("smoSourceTableUniqueID");
			eRPShipmentComponentInformationDto.smoUnitOfMeasure = dataTable.Rows[0].Field<string>("smoUnitOfMeasure");
			eRPShipmentComponentInformationDto.smoWeight = dataTable.Rows[0].Field<decimal>("smoWeight");
			eRPShipmentComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShipmentComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShipmentComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveShipmentComponent(ERPShipmentComponentDto shipmentComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ShipmentComponents WHERE smoUniqueID = " + M1Util.ConvertToLinq(shipmentComponent.smoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["smoShipmentID"] = shipmentComponent.smoShipmentID.ToUpper();
				dataRow["smoShipmentLineID"] = shipmentComponent.smoShipmentLineID;
				dataRow["smoShipmentComponentID"] = shipmentComponent.smoShipmentComponentID;
				shipmentComponent.smoUniqueID = ((shipmentComponent.smoUniqueID == Guid.Empty) ? Guid.NewGuid() : shipmentComponent.smoUniqueID);
				dataRow["smoUniqueID"] = shipmentComponent.smoUniqueID;
				dataRow["smoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["smoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ShipmentComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (shipmentComponent.smoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ShipmentComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["smoRowVersion"], shipmentComponent.smoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ShipmentComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ShipmentComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["smoAdditionalQuantity"] = shipmentComponent.smoAdditionalQuantity;
			dataRow["smoDescription"] = shipmentComponent.smoDescription;
			dataRow["smoClosed"] = shipmentComponent.smoClosed;
			dataRow["smoPostedToGl"] = shipmentComponent.smoPostedToGl;
			dataRow["smoReversed"] = shipmentComponent.smoReversed;
			dataRow["smoShippedComplete"] = shipmentComponent.smoShippedComplete;
			dataRow["smoJobID"] = shipmentComponent.smoJobID;
			dataRow["smoJobParentQuantity"] = shipmentComponent.smoJobParentQuantity;
			dataRow["smoJobQuantityShipped"] = shipmentComponent.smoJobQuantityShipped;
			dataRow["smoParentQuantity"] = shipmentComponent.smoParentQuantity;
			dataRow["smoPartBinID"] = shipmentComponent.smoPartBinID;
			dataRow["smoPartID"] = shipmentComponent.smoPartID;
			dataRow["smoPartRevisionID"] = shipmentComponent.smoPartRevisionID;
			dataRow["smoPartWarehouseLocationID"] = shipmentComponent.smoPartWarehouseLocationID;
			dataRow["smoQuantityPerParent"] = shipmentComponent.smoQuantityPerParent;
			dataRow["smoQuantityShipped"] = shipmentComponent.smoQuantityShipped;
			dataRow["smoReverseShipmentComponentID"] = shipmentComponent.smoReverseShipmentComponentID;
			dataRow["smoReverseShipmentID"] = shipmentComponent.smoReverseShipmentID;
			dataRow["smoReverseShipmentLineID"] = shipmentComponent.smoReverseShipmentLineID;
			dataRow["smoSalesOrderComponentID"] = shipmentComponent.smoSalesOrderComponentID;
			dataRow["smoSalesOrderDeliveryID"] = shipmentComponent.smoSalesOrderDeliveryID;
			dataRow["smoSalesOrderID"] = shipmentComponent.smoSalesOrderID;
			dataRow["smoSalesOrderLineID"] = shipmentComponent.smoSalesOrderLineID;
			dataRow["smoSourceTableName"] = shipmentComponent.smoSourceTableName;
			dataRow["smoSourceTableUniqueID"] = shipmentComponent.smoSourceTableUniqueID;
			dataRow["smoUnitOfMeasure"] = shipmentComponent.smoUnitOfMeasure;
			dataRow["smoWeight"] = shipmentComponent.smoWeight;
			if (shipmentComponent.CustomFields != null && shipmentComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in shipmentComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ShipmentComponent [{shipmentComponent.smoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ShipmentComponent [{shipmentComponent.smoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
