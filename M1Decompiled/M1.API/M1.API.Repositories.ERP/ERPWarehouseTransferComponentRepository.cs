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

public class ERPWarehouseTransferComponentRepository : APIBaseRepository, IERPWarehouseTransferComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseTransferComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseTransferComponentExist(Guid warehouseTransferComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("mwoUniqueID|C", warehouseTransferComponentId);
		base.selectList.Add("mwoUniqueID");
		return Task.FromResult(GetAsObject("WarehouseTransferComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseTransferComponentInformationDto>> GetAllWarehouseTransferComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseTransferComponentInformationDto> collection = new List<ERPWarehouseTransferComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[32]
		{
			"mwoAdditionalQuantity", "mwoCreatedBy", "mwoCreatedDate", "mwoDescription", "mwoDestinationWarehouseID", "mwoUniqueID", "mwoClosed", "mwoPosted", "mwoReceivedComplete", "mwoReversed",
			"mwoShippedComplete", "mwoParentQuantity", "mwoPartID", "mwoPartRevisionID", "mwoQuantityInTransit", "mwoQuantityPerParent", "mwoReceivedQuantity", "mwoReverseWHTransComponentID", "mwoReverseWHTransferID", "mwoReverseWHTransferLineID",
			"mwoRowVersion", "mwoShipQuantity", "mwoSourcePartBinID", "mwoSourceWarehouseID", "mwoUnitOfMeasure", "mwoWarehouseReqComponentID", "mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseTransComponentID", "mwoWarehouseTransferID",
			"mwoWarehouseTransferLineID", "mwoWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseTransferComponents");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseTransferComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseTransferComponentInformationDto eRPWarehouseTransferComponentInformationDto = new ERPWarehouseTransferComponentInformationDto();
				eRPWarehouseTransferComponentInformationDto.mwoAdditionalQuantity = dataTable.Rows[i].Field<decimal>("mwoAdditionalQuantity");
				eRPWarehouseTransferComponentInformationDto.mwoCreatedBy = dataTable.Rows[i].Field<string>("mwoCreatedBy");
				eRPWarehouseTransferComponentInformationDto.mwoCreatedDate = dataTable.Rows[i].Field<DateTime?>("mwoCreatedDate");
				eRPWarehouseTransferComponentInformationDto.mwoDescription = dataTable.Rows[i].Field<string>("mwoDescription");
				eRPWarehouseTransferComponentInformationDto.mwoDestinationWarehouseID = dataTable.Rows[i].Field<string>("mwoDestinationWarehouseID");
				eRPWarehouseTransferComponentInformationDto.mwoUniqueID = dataTable.Rows[i].Field<Guid>("mwoUniqueID");
				eRPWarehouseTransferComponentInformationDto.mwoClosed = dataTable.Rows[i].Field<bool>("mwoClosed");
				eRPWarehouseTransferComponentInformationDto.mwoPosted = dataTable.Rows[i].Field<bool>("mwoPosted");
				eRPWarehouseTransferComponentInformationDto.mwoReceivedComplete = dataTable.Rows[i].Field<bool>("mwoReceivedComplete");
				eRPWarehouseTransferComponentInformationDto.mwoReversed = dataTable.Rows[i].Field<bool>("mwoReversed");
				eRPWarehouseTransferComponentInformationDto.mwoShippedComplete = dataTable.Rows[i].Field<bool>("mwoShippedComplete");
				eRPWarehouseTransferComponentInformationDto.mwoParentQuantity = dataTable.Rows[i].Field<decimal>("mwoParentQuantity");
				eRPWarehouseTransferComponentInformationDto.mwoPartID = dataTable.Rows[i].Field<string>("mwoPartID");
				eRPWarehouseTransferComponentInformationDto.mwoPartRevisionID = dataTable.Rows[i].Field<string>("mwoPartRevisionID");
				eRPWarehouseTransferComponentInformationDto.mwoQuantityInTransit = dataTable.Rows[i].Field<decimal>("mwoQuantityInTransit");
				eRPWarehouseTransferComponentInformationDto.mwoQuantityPerParent = dataTable.Rows[i].Field<decimal>("mwoQuantityPerParent");
				eRPWarehouseTransferComponentInformationDto.mwoReceivedQuantity = dataTable.Rows[i].Field<decimal>("mwoReceivedQuantity");
				eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransComponentID = dataTable.Rows[i].Field<short>("mwoReverseWHTransComponentID");
				eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferID = dataTable.Rows[i].Field<string>("mwoReverseWHTransferID");
				eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferLineID = dataTable.Rows[i].Field<short>("mwoReverseWHTransferLineID");
				eRPWarehouseTransferComponentInformationDto.mwoRowVersion = dataTable.Rows[i].Field<byte[]>("mwoRowVersion");
				eRPWarehouseTransferComponentInformationDto.mwoShipQuantity = dataTable.Rows[i].Field<decimal>("mwoShipQuantity");
				eRPWarehouseTransferComponentInformationDto.mwoSourcePartBinID = dataTable.Rows[i].Field<string>("mwoSourcePartBinID");
				eRPWarehouseTransferComponentInformationDto.mwoSourceWarehouseID = dataTable.Rows[i].Field<string>("mwoSourceWarehouseID");
				eRPWarehouseTransferComponentInformationDto.mwoUnitOfMeasure = dataTable.Rows[i].Field<string>("mwoUnitOfMeasure");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseReqComponentID = dataTable.Rows[i].Field<short>("mwoWarehouseReqComponentID");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionID = dataTable.Rows[i].Field<string>("mwoWarehouseRequisitionID");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("mwoWarehouseRequisitionLineID");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransComponentID = dataTable.Rows[i].Field<short>("mwoWarehouseTransComponentID");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferID = dataTable.Rows[i].Field<string>("mwoWarehouseTransferID");
				eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferLineID = dataTable.Rows[i].Field<short>("mwoWarehouseTransferLineID");
				eRPWarehouseTransferComponentInformationDto.mwoWeight = dataTable.Rows[i].Field<decimal>("mwoWeight");
				eRPWarehouseTransferComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseTransferComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseTransferComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseTransferComponentInformationDto> GetWarehouseTransferComponent(Guid warehouseTransferComponentId)
	{
		ERPWarehouseTransferComponentInformationDto eRPWarehouseTransferComponentInformationDto = new ERPWarehouseTransferComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[32]
		{
			"mwoAdditionalQuantity", "mwoCreatedBy", "mwoCreatedDate", "mwoDescription", "mwoDestinationWarehouseID", "mwoUniqueID", "mwoClosed", "mwoPosted", "mwoReceivedComplete", "mwoReversed",
			"mwoShippedComplete", "mwoParentQuantity", "mwoPartID", "mwoPartRevisionID", "mwoQuantityInTransit", "mwoQuantityPerParent", "mwoReceivedQuantity", "mwoReverseWHTransComponentID", "mwoReverseWHTransferID", "mwoReverseWHTransferLineID",
			"mwoRowVersion", "mwoShipQuantity", "mwoSourcePartBinID", "mwoSourceWarehouseID", "mwoUnitOfMeasure", "mwoWarehouseReqComponentID", "mwoWarehouseRequisitionID", "mwoWarehouseRequisitionLineID", "mwoWarehouseTransComponentID", "mwoWarehouseTransferID",
			"mwoWarehouseTransferLineID", "mwoWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mwoUniqueID|C", warehouseTransferComponentId);
		AddCustomFieldsToSelectList("WarehouseTransferComponents");
		using (DataTable dataTable = GetAsDataTable("WarehouseTransferComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseTransferComponentInformationDto);
			}
			eRPWarehouseTransferComponentInformationDto.mwoAdditionalQuantity = dataTable.Rows[0].Field<decimal>("mwoAdditionalQuantity");
			eRPWarehouseTransferComponentInformationDto.mwoCreatedBy = dataTable.Rows[0].Field<string>("mwoCreatedBy");
			eRPWarehouseTransferComponentInformationDto.mwoCreatedDate = dataTable.Rows[0].Field<DateTime?>("mwoCreatedDate");
			eRPWarehouseTransferComponentInformationDto.mwoDescription = dataTable.Rows[0].Field<string>("mwoDescription");
			eRPWarehouseTransferComponentInformationDto.mwoDestinationWarehouseID = dataTable.Rows[0].Field<string>("mwoDestinationWarehouseID");
			eRPWarehouseTransferComponentInformationDto.mwoUniqueID = dataTable.Rows[0].Field<Guid>("mwoUniqueID");
			eRPWarehouseTransferComponentInformationDto.mwoClosed = dataTable.Rows[0].Field<bool>("mwoClosed");
			eRPWarehouseTransferComponentInformationDto.mwoPosted = dataTable.Rows[0].Field<bool>("mwoPosted");
			eRPWarehouseTransferComponentInformationDto.mwoReceivedComplete = dataTable.Rows[0].Field<bool>("mwoReceivedComplete");
			eRPWarehouseTransferComponentInformationDto.mwoReversed = dataTable.Rows[0].Field<bool>("mwoReversed");
			eRPWarehouseTransferComponentInformationDto.mwoShippedComplete = dataTable.Rows[0].Field<bool>("mwoShippedComplete");
			eRPWarehouseTransferComponentInformationDto.mwoParentQuantity = dataTable.Rows[0].Field<decimal>("mwoParentQuantity");
			eRPWarehouseTransferComponentInformationDto.mwoPartID = dataTable.Rows[0].Field<string>("mwoPartID");
			eRPWarehouseTransferComponentInformationDto.mwoPartRevisionID = dataTable.Rows[0].Field<string>("mwoPartRevisionID");
			eRPWarehouseTransferComponentInformationDto.mwoQuantityInTransit = dataTable.Rows[0].Field<decimal>("mwoQuantityInTransit");
			eRPWarehouseTransferComponentInformationDto.mwoQuantityPerParent = dataTable.Rows[0].Field<decimal>("mwoQuantityPerParent");
			eRPWarehouseTransferComponentInformationDto.mwoReceivedQuantity = dataTable.Rows[0].Field<decimal>("mwoReceivedQuantity");
			eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransComponentID = dataTable.Rows[0].Field<short>("mwoReverseWHTransComponentID");
			eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferID = dataTable.Rows[0].Field<string>("mwoReverseWHTransferID");
			eRPWarehouseTransferComponentInformationDto.mwoReverseWHTransferLineID = dataTable.Rows[0].Field<short>("mwoReverseWHTransferLineID");
			eRPWarehouseTransferComponentInformationDto.mwoRowVersion = dataTable.Rows[0].Field<byte[]>("mwoRowVersion");
			eRPWarehouseTransferComponentInformationDto.mwoShipQuantity = dataTable.Rows[0].Field<decimal>("mwoShipQuantity");
			eRPWarehouseTransferComponentInformationDto.mwoSourcePartBinID = dataTable.Rows[0].Field<string>("mwoSourcePartBinID");
			eRPWarehouseTransferComponentInformationDto.mwoSourceWarehouseID = dataTable.Rows[0].Field<string>("mwoSourceWarehouseID");
			eRPWarehouseTransferComponentInformationDto.mwoUnitOfMeasure = dataTable.Rows[0].Field<string>("mwoUnitOfMeasure");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseReqComponentID = dataTable.Rows[0].Field<short>("mwoWarehouseReqComponentID");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionID = dataTable.Rows[0].Field<string>("mwoWarehouseRequisitionID");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("mwoWarehouseRequisitionLineID");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransComponentID = dataTable.Rows[0].Field<short>("mwoWarehouseTransComponentID");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferID = dataTable.Rows[0].Field<string>("mwoWarehouseTransferID");
			eRPWarehouseTransferComponentInformationDto.mwoWarehouseTransferLineID = dataTable.Rows[0].Field<short>("mwoWarehouseTransferLineID");
			eRPWarehouseTransferComponentInformationDto.mwoWeight = dataTable.Rows[0].Field<decimal>("mwoWeight");
			eRPWarehouseTransferComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseTransferComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseTransferComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseTransferComponent(ERPWarehouseTransferComponentDto warehouseTransferComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseTransferComponents WHERE mwoUniqueID = " + M1Util.ConvertToLinq(warehouseTransferComponent.mwoUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mwoWarehouseTransferID"] = warehouseTransferComponent.mwoWarehouseTransferID.ToUpper();
				dataRow["mwoWarehouseTransferLineID"] = warehouseTransferComponent.mwoWarehouseTransferLineID;
				dataRow["mwoWarehouseTransComponentID"] = warehouseTransferComponent.mwoWarehouseTransComponentID;
				warehouseTransferComponent.mwoUniqueID = ((warehouseTransferComponent.mwoUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseTransferComponent.mwoUniqueID);
				dataRow["mwoUniqueID"] = warehouseTransferComponent.mwoUniqueID;
				dataRow["mwoCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mwoCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseTransferComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseTransferComponent.mwoRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseTransferComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mwoRowVersion"], warehouseTransferComponent.mwoRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseTransferComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseTransferComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mwoAdditionalQuantity"] = warehouseTransferComponent.mwoAdditionalQuantity;
			dataRow["mwoDescription"] = warehouseTransferComponent.mwoDescription;
			dataRow["mwoDestinationWarehouseID"] = warehouseTransferComponent.mwoDestinationWarehouseID;
			dataRow["mwoClosed"] = warehouseTransferComponent.mwoClosed;
			dataRow["mwoPosted"] = warehouseTransferComponent.mwoPosted;
			dataRow["mwoReceivedComplete"] = warehouseTransferComponent.mwoReceivedComplete;
			dataRow["mwoReversed"] = warehouseTransferComponent.mwoReversed;
			dataRow["mwoShippedComplete"] = warehouseTransferComponent.mwoShippedComplete;
			dataRow["mwoParentQuantity"] = warehouseTransferComponent.mwoParentQuantity;
			dataRow["mwoPartID"] = warehouseTransferComponent.mwoPartID;
			dataRow["mwoPartRevisionID"] = warehouseTransferComponent.mwoPartRevisionID;
			dataRow["mwoQuantityInTransit"] = warehouseTransferComponent.mwoQuantityInTransit;
			dataRow["mwoQuantityPerParent"] = warehouseTransferComponent.mwoQuantityPerParent;
			dataRow["mwoReceivedQuantity"] = warehouseTransferComponent.mwoReceivedQuantity;
			dataRow["mwoReverseWHTransComponentID"] = warehouseTransferComponent.mwoReverseWHTransComponentID;
			dataRow["mwoReverseWHTransferID"] = warehouseTransferComponent.mwoReverseWHTransferID;
			dataRow["mwoReverseWHTransferLineID"] = warehouseTransferComponent.mwoReverseWHTransferLineID;
			dataRow["mwoShipQuantity"] = warehouseTransferComponent.mwoShipQuantity;
			dataRow["mwoSourcePartBinID"] = warehouseTransferComponent.mwoSourcePartBinID;
			dataRow["mwoSourceWarehouseID"] = warehouseTransferComponent.mwoSourceWarehouseID;
			dataRow["mwoUnitOfMeasure"] = warehouseTransferComponent.mwoUnitOfMeasure;
			dataRow["mwoWarehouseReqComponentID"] = warehouseTransferComponent.mwoWarehouseReqComponentID;
			dataRow["mwoWarehouseRequisitionID"] = warehouseTransferComponent.mwoWarehouseRequisitionID;
			dataRow["mwoWarehouseRequisitionLineID"] = warehouseTransferComponent.mwoWarehouseRequisitionLineID;
			dataRow["mwoWeight"] = warehouseTransferComponent.mwoWeight;
			if (warehouseTransferComponent.CustomFields != null && warehouseTransferComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseTransferComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseTransferComponent [{warehouseTransferComponent.mwoUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseTransferComponent [{warehouseTransferComponent.mwoUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
