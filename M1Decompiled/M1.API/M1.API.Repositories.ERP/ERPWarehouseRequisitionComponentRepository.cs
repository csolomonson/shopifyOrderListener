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

public class ERPWarehouseRequisitionComponentRepository : APIBaseRepository, IERPWarehouseRequisitionComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseRequisitionComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseRequisitionComponentExist(Guid warehouseRequisitionComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("wqoUniqueID|C", warehouseRequisitionComponentId);
		base.selectList.Add("wqoUniqueID");
		return Task.FromResult(GetAsObject("WarehouseRequisitionComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseRequisitionComponentInformationDto>> GetAllWarehouseRequisitionComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseRequisitionComponentInformationDto> collection = new List<ERPWarehouseRequisitionComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[20]
		{
			"wqoAdditionalQuantity", "wqoCreatedBy", "wqoCreatedDate", "wqoDescription", "wqoUniqueID", "wqoClosed", "wqoTransferredComplete", "wqoParentQuantity", "wqoPartID", "wqoPartRevisionID",
			"wqoQuantityPerParent", "wqoQuantityRequested", "wqoQuantityTransferred", "wqoRowVersion", "wqoSourceWarehouseID", "wqoUnitOfMeasure", "wqoWarehouseReqComponentID", "wqoWarehouseRequisitionID", "wqoWarehouseRequisitionLineID", "wqoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseRequisitionComponents");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitionComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseRequisitionComponentInformationDto eRPWarehouseRequisitionComponentInformationDto = new ERPWarehouseRequisitionComponentInformationDto();
				eRPWarehouseRequisitionComponentInformationDto.wqoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("wqoAdditionalQuantity");
				eRPWarehouseRequisitionComponentInformationDto.wqoCreatedBy = dataTable.Rows[i].Field<string>("wqoCreatedBy");
				eRPWarehouseRequisitionComponentInformationDto.wqoCreatedDate = dataTable.Rows[i].Field<DateTime?>("wqoCreatedDate");
				eRPWarehouseRequisitionComponentInformationDto.wqoDescription = dataTable.Rows[i].Field<string>("wqoDescription");
				eRPWarehouseRequisitionComponentInformationDto.wqoUniqueID = dataTable.Rows[i].Field<Guid>("wqoUniqueID");
				eRPWarehouseRequisitionComponentInformationDto.wqoClosed = dataTable.Rows[i].Field<bool>("wqoClosed");
				eRPWarehouseRequisitionComponentInformationDto.wqoTransferredComplete = dataTable.Rows[i].Field<bool>("wqoTransferredComplete");
				eRPWarehouseRequisitionComponentInformationDto.wqoParentQuantity = dataTable.Rows[i].Field<decimal>("wqoParentQuantity");
				eRPWarehouseRequisitionComponentInformationDto.wqoPartID = dataTable.Rows[i].Field<string>("wqoPartID");
				eRPWarehouseRequisitionComponentInformationDto.wqoPartRevisionID = dataTable.Rows[i].Field<string>("wqoPartRevisionID");
				eRPWarehouseRequisitionComponentInformationDto.wqoQuantityPerParent = dataTable.Rows[i].Field<decimal>("wqoQuantityPerParent");
				eRPWarehouseRequisitionComponentInformationDto.wqoQuantityRequested = dataTable.Rows[i].Field<decimal>("wqoQuantityRequested");
				eRPWarehouseRequisitionComponentInformationDto.wqoQuantityTransferred = dataTable.Rows[i].Field<decimal>("wqoQuantityTransferred");
				eRPWarehouseRequisitionComponentInformationDto.wqoRowVersion = dataTable.Rows[i].Field<byte[]>("wqoRowVersion");
				eRPWarehouseRequisitionComponentInformationDto.wqoSourceWarehouseID = dataTable.Rows[i].Field<string>("wqoSourceWarehouseID");
				eRPWarehouseRequisitionComponentInformationDto.wqoUnitOfMeasure = dataTable.Rows[i].Field<string>("wqoUnitOfMeasure");
				eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseReqComponentID = dataTable.Rows[i].Field<short>("wqoWarehouseReqComponentID");
				eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionID = dataTable.Rows[i].Field<string>("wqoWarehouseRequisitionID");
				eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("wqoWarehouseRequisitionLineID");
				eRPWarehouseRequisitionComponentInformationDto.wqoWeight = dataTable.Rows[i].Field<decimal>("wqoWeight");
				eRPWarehouseRequisitionComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseRequisitionComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseRequisitionComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseRequisitionComponentInformationDto> GetWarehouseRequisitionComponent(Guid warehouseRequisitionComponentId)
	{
		ERPWarehouseRequisitionComponentInformationDto eRPWarehouseRequisitionComponentInformationDto = new ERPWarehouseRequisitionComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[20]
		{
			"wqoAdditionalQuantity", "wqoCreatedBy", "wqoCreatedDate", "wqoDescription", "wqoUniqueID", "wqoClosed", "wqoTransferredComplete", "wqoParentQuantity", "wqoPartID", "wqoPartRevisionID",
			"wqoQuantityPerParent", "wqoQuantityRequested", "wqoQuantityTransferred", "wqoRowVersion", "wqoSourceWarehouseID", "wqoUnitOfMeasure", "wqoWarehouseReqComponentID", "wqoWarehouseRequisitionID", "wqoWarehouseRequisitionLineID", "wqoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wqoUniqueID|C", warehouseRequisitionComponentId);
		AddCustomFieldsToSelectList("WarehouseRequisitionComponents");
		using (DataTable dataTable = GetAsDataTable("WarehouseRequisitionComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseRequisitionComponentInformationDto);
			}
			eRPWarehouseRequisitionComponentInformationDto.wqoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("wqoAdditionalQuantity");
			eRPWarehouseRequisitionComponentInformationDto.wqoCreatedBy = dataTable.Rows[0].Field<string>("wqoCreatedBy");
			eRPWarehouseRequisitionComponentInformationDto.wqoCreatedDate = dataTable.Rows[0].Field<DateTime?>("wqoCreatedDate");
			eRPWarehouseRequisitionComponentInformationDto.wqoDescription = dataTable.Rows[0].Field<string>("wqoDescription");
			eRPWarehouseRequisitionComponentInformationDto.wqoUniqueID = dataTable.Rows[0].Field<Guid>("wqoUniqueID");
			eRPWarehouseRequisitionComponentInformationDto.wqoClosed = dataTable.Rows[0].Field<bool>("wqoClosed");
			eRPWarehouseRequisitionComponentInformationDto.wqoTransferredComplete = dataTable.Rows[0].Field<bool>("wqoTransferredComplete");
			eRPWarehouseRequisitionComponentInformationDto.wqoParentQuantity = dataTable.Rows[0].Field<decimal>("wqoParentQuantity");
			eRPWarehouseRequisitionComponentInformationDto.wqoPartID = dataTable.Rows[0].Field<string>("wqoPartID");
			eRPWarehouseRequisitionComponentInformationDto.wqoPartRevisionID = dataTable.Rows[0].Field<string>("wqoPartRevisionID");
			eRPWarehouseRequisitionComponentInformationDto.wqoQuantityPerParent = dataTable.Rows[0].Field<decimal>("wqoQuantityPerParent");
			eRPWarehouseRequisitionComponentInformationDto.wqoQuantityRequested = dataTable.Rows[0].Field<decimal>("wqoQuantityRequested");
			eRPWarehouseRequisitionComponentInformationDto.wqoQuantityTransferred = dataTable.Rows[0].Field<decimal>("wqoQuantityTransferred");
			eRPWarehouseRequisitionComponentInformationDto.wqoRowVersion = dataTable.Rows[0].Field<byte[]>("wqoRowVersion");
			eRPWarehouseRequisitionComponentInformationDto.wqoSourceWarehouseID = dataTable.Rows[0].Field<string>("wqoSourceWarehouseID");
			eRPWarehouseRequisitionComponentInformationDto.wqoUnitOfMeasure = dataTable.Rows[0].Field<string>("wqoUnitOfMeasure");
			eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseReqComponentID = dataTable.Rows[0].Field<short>("wqoWarehouseReqComponentID");
			eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionID = dataTable.Rows[0].Field<string>("wqoWarehouseRequisitionID");
			eRPWarehouseRequisitionComponentInformationDto.wqoWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("wqoWarehouseRequisitionLineID");
			eRPWarehouseRequisitionComponentInformationDto.wqoWeight = dataTable.Rows[0].Field<decimal>("wqoWeight");
			eRPWarehouseRequisitionComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseRequisitionComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseRequisitionComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseRequisitionComponent(ERPWarehouseRequisitionComponentDto warehouseRequisitionComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseRequisitionComponents WHERE wqoUniqueID = " + M1Util.ConvertToLinq(warehouseRequisitionComponent.wqoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wqoWarehouseRequisitionID"] = warehouseRequisitionComponent.wqoWarehouseRequisitionID.ToUpper();
				dataRow["wqoWarehouseRequisitionLineID"] = warehouseRequisitionComponent.wqoWarehouseRequisitionLineID;
				dataRow["wqoWarehouseReqComponentID"] = warehouseRequisitionComponent.wqoWarehouseReqComponentID;
				warehouseRequisitionComponent.wqoUniqueID = ((warehouseRequisitionComponent.wqoUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseRequisitionComponent.wqoUniqueID);
				dataRow["wqoUniqueID"] = warehouseRequisitionComponent.wqoUniqueID;
				dataRow["wqoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wqoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseRequisitionComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseRequisitionComponent.wqoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseRequisitionComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wqoRowVersion"], warehouseRequisitionComponent.wqoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseRequisitionComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseRequisitionComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["wqoAdditionalQuantity"] = warehouseRequisitionComponent.wqoAdditionalQuantity;
			dataRow["wqoDescription"] = warehouseRequisitionComponent.wqoDescription;
			dataRow["wqoClosed"] = warehouseRequisitionComponent.wqoClosed;
			dataRow["wqoTransferredComplete"] = warehouseRequisitionComponent.wqoTransferredComplete;
			dataRow["wqoParentQuantity"] = warehouseRequisitionComponent.wqoParentQuantity;
			dataRow["wqoPartID"] = warehouseRequisitionComponent.wqoPartID;
			dataRow["wqoPartRevisionID"] = warehouseRequisitionComponent.wqoPartRevisionID;
			dataRow["wqoQuantityPerParent"] = warehouseRequisitionComponent.wqoQuantityPerParent;
			dataRow["wqoQuantityRequested"] = warehouseRequisitionComponent.wqoQuantityRequested;
			dataRow["wqoQuantityTransferred"] = warehouseRequisitionComponent.wqoQuantityTransferred;
			dataRow["wqoSourceWarehouseID"] = warehouseRequisitionComponent.wqoSourceWarehouseID;
			dataRow["wqoUnitOfMeasure"] = warehouseRequisitionComponent.wqoUnitOfMeasure;
			dataRow["wqoWeight"] = warehouseRequisitionComponent.wqoWeight;
			if (warehouseRequisitionComponent.CustomFields != null && warehouseRequisitionComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseRequisitionComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseRequisitionComponent [{warehouseRequisitionComponent.wqoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseRequisitionComponent [{warehouseRequisitionComponent.wqoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
