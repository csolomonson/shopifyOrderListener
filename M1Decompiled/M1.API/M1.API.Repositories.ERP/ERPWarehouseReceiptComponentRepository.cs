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

public class ERPWarehouseReceiptComponentRepository : APIBaseRepository, IERPWarehouseReceiptComponentRepository, IAPIBaseRepository, IDisposable
{
	public ERPWarehouseReceiptComponentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesWarehouseReceiptComponentExist(Guid warehouseReceiptComponentId)
	{
		InitializeParameterLists();
		base.filterList.Add("wroUniqueID|C", warehouseReceiptComponentId);
		base.selectList.Add("wroUniqueID");
		return Task.FromResult(GetAsObject("WarehouseReceiptComponents", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPWarehouseReceiptComponentInformationDto>> GetAllWarehouseReceiptComponents(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPWarehouseReceiptComponentInformationDto> collection = new List<ERPWarehouseReceiptComponentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[35]
		{
			"wroAdditionalQuantity", "wroCreatedBy", "wroCreatedDate", "wroDescription", "wroDestinationPartBinID", "wroDestinationWarehouseID", "wroUniqueID", "wroClosed", "wroPosted", "wroReceivedComplete",
			"wroReversed", "wroParentQuantity", "wroPartID", "wroPartRevisionID", "wroQuantityPerParent", "wroQuantityReceived", "wroReverseWHReceiptCompID", "wroReverseWHReceiptID", "wroReverseWHReceiptLineID", "wroRowVersion",
			"wroWarehouseReceiptComponentID", "wroSourcePartBinID", "wroSourceTableName", "wroSourceTableUniqueID", "wroSourceWarehouseID", "wroUnitOfMeasure", "wroWarehouseReceiptID", "wroWarehouseReceiptLineID", "wroWarehouseReqComponentID", "wroWarehouseRequisitionID",
			"wroWarehouseRequisitionLineID", "wroWarehouseTransComponentID", "wroWarehouseTransferID", "wroWarehouseTransferLineID", "wroWeight"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("WarehouseReceiptComponents");
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
		using (DataTable dataTable = GetAsDataTable("WarehouseReceiptComponents", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPWarehouseReceiptComponentInformationDto eRPWarehouseReceiptComponentInformationDto = new ERPWarehouseReceiptComponentInformationDto();
				eRPWarehouseReceiptComponentInformationDto.wroAdditionalQuantity = dataTable.Rows[i].Field<decimal>("wroAdditionalQuantity");
				eRPWarehouseReceiptComponentInformationDto.wroCreatedBy = dataTable.Rows[i].Field<string>("wroCreatedBy");
				eRPWarehouseReceiptComponentInformationDto.wroCreatedDate = dataTable.Rows[i].Field<DateTime?>("wroCreatedDate");
				eRPWarehouseReceiptComponentInformationDto.wroDescription = dataTable.Rows[i].Field<string>("wroDescription");
				eRPWarehouseReceiptComponentInformationDto.wroDestinationPartBinID = dataTable.Rows[i].Field<string>("wroDestinationPartBinID");
				eRPWarehouseReceiptComponentInformationDto.wroDestinationWarehouseID = dataTable.Rows[i].Field<string>("wroDestinationWarehouseID");
				eRPWarehouseReceiptComponentInformationDto.wroUniqueID = dataTable.Rows[i].Field<Guid>("wroUniqueID");
				eRPWarehouseReceiptComponentInformationDto.wroClosed = dataTable.Rows[i].Field<bool>("wroClosed");
				eRPWarehouseReceiptComponentInformationDto.wroPosted = dataTable.Rows[i].Field<bool>("wroPosted");
				eRPWarehouseReceiptComponentInformationDto.wroReceivedComplete = dataTable.Rows[i].Field<bool>("wroReceivedComplete");
				eRPWarehouseReceiptComponentInformationDto.wroReversed = dataTable.Rows[i].Field<bool>("wroReversed");
				eRPWarehouseReceiptComponentInformationDto.wroParentQuantity = dataTable.Rows[i].Field<decimal>("wroParentQuantity");
				eRPWarehouseReceiptComponentInformationDto.wroPartID = dataTable.Rows[i].Field<string>("wroPartID");
				eRPWarehouseReceiptComponentInformationDto.wroPartRevisionID = dataTable.Rows[i].Field<string>("wroPartRevisionID");
				eRPWarehouseReceiptComponentInformationDto.wroQuantityPerParent = dataTable.Rows[i].Field<decimal>("wroQuantityPerParent");
				eRPWarehouseReceiptComponentInformationDto.wroQuantityReceived = dataTable.Rows[i].Field<decimal>("wroQuantityReceived");
				eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptCompID = dataTable.Rows[i].Field<string>("wroReverseWHReceiptCompID");
				eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptID = dataTable.Rows[i].Field<string>("wroReverseWHReceiptID");
				eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptLineID = dataTable.Rows[i].Field<short>("wroReverseWHReceiptLineID");
				eRPWarehouseReceiptComponentInformationDto.wroRowVersion = dataTable.Rows[i].Field<byte[]>("wroRowVersion");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptComponentID = dataTable.Rows[i].Field<short>("wroWarehouseReceiptComponentID");
				eRPWarehouseReceiptComponentInformationDto.wroSourcePartBinID = dataTable.Rows[i].Field<string>("wroSourcePartBinID");
				eRPWarehouseReceiptComponentInformationDto.wroSourceTableName = dataTable.Rows[i].Field<string>("wroSourceTableName");
				eRPWarehouseReceiptComponentInformationDto.wroSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("wroSourceTableUniqueID");
				eRPWarehouseReceiptComponentInformationDto.wroSourceWarehouseID = dataTable.Rows[i].Field<string>("wroSourceWarehouseID");
				eRPWarehouseReceiptComponentInformationDto.wroUnitOfMeasure = dataTable.Rows[i].Field<string>("wroUnitOfMeasure");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptID = dataTable.Rows[i].Field<string>("wroWarehouseReceiptID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptLineID = dataTable.Rows[i].Field<short>("wroWarehouseReceiptLineID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseReqComponentID = dataTable.Rows[i].Field<short>("wroWarehouseReqComponentID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionID = dataTable.Rows[i].Field<string>("wroWarehouseRequisitionID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionLineID = dataTable.Rows[i].Field<short>("wroWarehouseRequisitionLineID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransComponentID = dataTable.Rows[i].Field<short>("wroWarehouseTransComponentID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferID = dataTable.Rows[i].Field<string>("wroWarehouseTransferID");
				eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferLineID = dataTable.Rows[i].Field<short>("wroWarehouseTransferLineID");
				eRPWarehouseReceiptComponentInformationDto.wroWeight = dataTable.Rows[i].Field<decimal>("wroWeight");
				eRPWarehouseReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPWarehouseReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPWarehouseReceiptComponentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPWarehouseReceiptComponentInformationDto> GetWarehouseReceiptComponent(Guid warehouseReceiptComponentId)
	{
		ERPWarehouseReceiptComponentInformationDto eRPWarehouseReceiptComponentInformationDto = new ERPWarehouseReceiptComponentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[35]
		{
			"wroAdditionalQuantity", "wroCreatedBy", "wroCreatedDate", "wroDescription", "wroDestinationPartBinID", "wroDestinationWarehouseID", "wroUniqueID", "wroClosed", "wroPosted", "wroReceivedComplete",
			"wroReversed", "wroParentQuantity", "wroPartID", "wroPartRevisionID", "wroQuantityPerParent", "wroQuantityReceived", "wroReverseWHReceiptCompID", "wroReverseWHReceiptID", "wroReverseWHReceiptLineID", "wroRowVersion",
			"wroWarehouseReceiptComponentID", "wroSourcePartBinID", "wroSourceTableName", "wroSourceTableUniqueID", "wroSourceWarehouseID", "wroUnitOfMeasure", "wroWarehouseReceiptID", "wroWarehouseReceiptLineID", "wroWarehouseReqComponentID", "wroWarehouseRequisitionID",
			"wroWarehouseRequisitionLineID", "wroWarehouseTransComponentID", "wroWarehouseTransferID", "wroWarehouseTransferLineID", "wroWeight"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("wroUniqueID|C", warehouseReceiptComponentId);
		AddCustomFieldsToSelectList("WarehouseReceiptComponents");
		using (DataTable dataTable = GetAsDataTable("WarehouseReceiptComponents", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPWarehouseReceiptComponentInformationDto);
			}
			eRPWarehouseReceiptComponentInformationDto.wroAdditionalQuantity = dataTable.Rows[0].Field<decimal>("wroAdditionalQuantity");
			eRPWarehouseReceiptComponentInformationDto.wroCreatedBy = dataTable.Rows[0].Field<string>("wroCreatedBy");
			eRPWarehouseReceiptComponentInformationDto.wroCreatedDate = dataTable.Rows[0].Field<DateTime?>("wroCreatedDate");
			eRPWarehouseReceiptComponentInformationDto.wroDescription = dataTable.Rows[0].Field<string>("wroDescription");
			eRPWarehouseReceiptComponentInformationDto.wroDestinationPartBinID = dataTable.Rows[0].Field<string>("wroDestinationPartBinID");
			eRPWarehouseReceiptComponentInformationDto.wroDestinationWarehouseID = dataTable.Rows[0].Field<string>("wroDestinationWarehouseID");
			eRPWarehouseReceiptComponentInformationDto.wroUniqueID = dataTable.Rows[0].Field<Guid>("wroUniqueID");
			eRPWarehouseReceiptComponentInformationDto.wroClosed = dataTable.Rows[0].Field<bool>("wroClosed");
			eRPWarehouseReceiptComponentInformationDto.wroPosted = dataTable.Rows[0].Field<bool>("wroPosted");
			eRPWarehouseReceiptComponentInformationDto.wroReceivedComplete = dataTable.Rows[0].Field<bool>("wroReceivedComplete");
			eRPWarehouseReceiptComponentInformationDto.wroReversed = dataTable.Rows[0].Field<bool>("wroReversed");
			eRPWarehouseReceiptComponentInformationDto.wroParentQuantity = dataTable.Rows[0].Field<decimal>("wroParentQuantity");
			eRPWarehouseReceiptComponentInformationDto.wroPartID = dataTable.Rows[0].Field<string>("wroPartID");
			eRPWarehouseReceiptComponentInformationDto.wroPartRevisionID = dataTable.Rows[0].Field<string>("wroPartRevisionID");
			eRPWarehouseReceiptComponentInformationDto.wroQuantityPerParent = dataTable.Rows[0].Field<decimal>("wroQuantityPerParent");
			eRPWarehouseReceiptComponentInformationDto.wroQuantityReceived = dataTable.Rows[0].Field<decimal>("wroQuantityReceived");
			eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptCompID = dataTable.Rows[0].Field<string>("wroReverseWHReceiptCompID");
			eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptID = dataTable.Rows[0].Field<string>("wroReverseWHReceiptID");
			eRPWarehouseReceiptComponentInformationDto.wroReverseWHReceiptLineID = dataTable.Rows[0].Field<short>("wroReverseWHReceiptLineID");
			eRPWarehouseReceiptComponentInformationDto.wroRowVersion = dataTable.Rows[0].Field<byte[]>("wroRowVersion");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptComponentID = dataTable.Rows[0].Field<short>("wroWarehouseReceiptComponentID");
			eRPWarehouseReceiptComponentInformationDto.wroSourcePartBinID = dataTable.Rows[0].Field<string>("wroSourcePartBinID");
			eRPWarehouseReceiptComponentInformationDto.wroSourceTableName = dataTable.Rows[0].Field<string>("wroSourceTableName");
			eRPWarehouseReceiptComponentInformationDto.wroSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("wroSourceTableUniqueID");
			eRPWarehouseReceiptComponentInformationDto.wroSourceWarehouseID = dataTable.Rows[0].Field<string>("wroSourceWarehouseID");
			eRPWarehouseReceiptComponentInformationDto.wroUnitOfMeasure = dataTable.Rows[0].Field<string>("wroUnitOfMeasure");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptID = dataTable.Rows[0].Field<string>("wroWarehouseReceiptID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseReceiptLineID = dataTable.Rows[0].Field<short>("wroWarehouseReceiptLineID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseReqComponentID = dataTable.Rows[0].Field<short>("wroWarehouseReqComponentID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionID = dataTable.Rows[0].Field<string>("wroWarehouseRequisitionID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseRequisitionLineID = dataTable.Rows[0].Field<short>("wroWarehouseRequisitionLineID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransComponentID = dataTable.Rows[0].Field<short>("wroWarehouseTransComponentID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferID = dataTable.Rows[0].Field<string>("wroWarehouseTransferID");
			eRPWarehouseReceiptComponentInformationDto.wroWarehouseTransferLineID = dataTable.Rows[0].Field<short>("wroWarehouseTransferLineID");
			eRPWarehouseReceiptComponentInformationDto.wroWeight = dataTable.Rows[0].Field<decimal>("wroWeight");
			eRPWarehouseReceiptComponentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPWarehouseReceiptComponentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPWarehouseReceiptComponentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveWarehouseReceiptComponent(ERPWarehouseReceiptComponentDto warehouseReceiptComponent)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM WarehouseReceiptComponents WHERE wroUniqueID = " + M1Util.ConvertToLinq(warehouseReceiptComponent.wroUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["wroWarehouseReceiptID"] = warehouseReceiptComponent.wroWarehouseReceiptID.ToUpper();
				dataRow["wroWarehouseReceiptLineID"] = warehouseReceiptComponent.wroWarehouseReceiptLineID;
				dataRow["wroWarehouseReceiptComponentID"] = warehouseReceiptComponent.wroWarehouseReceiptComponentID;
				warehouseReceiptComponent.wroUniqueID = ((warehouseReceiptComponent.wroUniqueID == Guid.Empty) ? Guid.NewGuid() : warehouseReceiptComponent.wroUniqueID);
				dataRow["wroUniqueID"] = warehouseReceiptComponent.wroUniqueID;
				dataRow["wroCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["wroCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The WarehouseReceiptComponent could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (warehouseReceiptComponent.wroRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the WarehouseReceiptComponent is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["wroRowVersion"], warehouseReceiptComponent.wroRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the WarehouseReceiptComponent has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the WarehouseReceiptComponent again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["wroAdditionalQuantity"] = warehouseReceiptComponent.wroAdditionalQuantity;
			dataRow["wroDescription"] = warehouseReceiptComponent.wroDescription;
			dataRow["wroDestinationPartBinID"] = warehouseReceiptComponent.wroDestinationPartBinID;
			dataRow["wroDestinationWarehouseID"] = warehouseReceiptComponent.wroDestinationWarehouseID;
			dataRow["wroClosed"] = warehouseReceiptComponent.wroClosed;
			dataRow["wroPosted"] = warehouseReceiptComponent.wroPosted;
			dataRow["wroReceivedComplete"] = warehouseReceiptComponent.wroReceivedComplete;
			dataRow["wroReversed"] = warehouseReceiptComponent.wroReversed;
			dataRow["wroParentQuantity"] = warehouseReceiptComponent.wroParentQuantity;
			dataRow["wroPartID"] = warehouseReceiptComponent.wroPartID;
			dataRow["wroPartRevisionID"] = warehouseReceiptComponent.wroPartRevisionID;
			dataRow["wroQuantityPerParent"] = warehouseReceiptComponent.wroQuantityPerParent;
			dataRow["wroQuantityReceived"] = warehouseReceiptComponent.wroQuantityReceived;
			dataRow["wroReverseWHReceiptCompID"] = warehouseReceiptComponent.wroReverseWHReceiptCompID;
			dataRow["wroReverseWHReceiptID"] = warehouseReceiptComponent.wroReverseWHReceiptID;
			dataRow["wroReverseWHReceiptLineID"] = warehouseReceiptComponent.wroReverseWHReceiptLineID;
			dataRow["wroSourcePartBinID"] = warehouseReceiptComponent.wroSourcePartBinID;
			dataRow["wroSourceTableName"] = warehouseReceiptComponent.wroSourceTableName;
			dataRow["wroSourceTableUniqueID"] = warehouseReceiptComponent.wroSourceTableUniqueID;
			dataRow["wroSourceWarehouseID"] = warehouseReceiptComponent.wroSourceWarehouseID;
			dataRow["wroUnitOfMeasure"] = warehouseReceiptComponent.wroUnitOfMeasure;
			dataRow["wroWarehouseReqComponentID"] = warehouseReceiptComponent.wroWarehouseReqComponentID;
			dataRow["wroWarehouseRequisitionID"] = warehouseReceiptComponent.wroWarehouseRequisitionID;
			dataRow["wroWarehouseRequisitionLineID"] = warehouseReceiptComponent.wroWarehouseRequisitionLineID;
			dataRow["wroWarehouseTransComponentID"] = warehouseReceiptComponent.wroWarehouseTransComponentID;
			dataRow["wroWarehouseTransferID"] = warehouseReceiptComponent.wroWarehouseTransferID;
			dataRow["wroWarehouseTransferLineID"] = warehouseReceiptComponent.wroWarehouseTransferLineID;
			dataRow["wroWeight"] = warehouseReceiptComponent.wroWeight;
			if (warehouseReceiptComponent.CustomFields != null && warehouseReceiptComponent.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in warehouseReceiptComponent.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the WarehouseReceiptComponent [{warehouseReceiptComponent.wroUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the WarehouseReceiptComponent [{warehouseReceiptComponent.wroUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
