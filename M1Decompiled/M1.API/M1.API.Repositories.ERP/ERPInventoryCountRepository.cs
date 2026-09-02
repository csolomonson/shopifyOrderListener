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

public class ERPInventoryCountRepository : APIBaseRepository, IERPInventoryCountRepository, IAPIBaseRepository, IDisposable
{
	public ERPInventoryCountRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInventoryCountExist(Guid inventoryCountId)
	{
		InitializeParameterLists();
		base.filterList.Add("imnUniqueID|C", inventoryCountId);
		base.selectList.Add("imnUniqueID");
		return Task.FromResult(GetAsObject("InventoryCounts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInventoryCountInformationDto>> GetAllInventoryCounts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInventoryCountInformationDto> collection = new List<ERPInventoryCountInformationDto>();
		InitializeParameterLists();
		string[] array = new string[20]
		{
			"imnCreatedBy", "imnCreatedDate", "imnCycleCodeID", "imnUniqueID", "imnGeneratedDate", "imnExcludeInactivePartBins", "imnIncludeBlankPartClass", "imnIncludeBlankPartGroup", "imnPostedToInventory", "imnRecordsGenerated",
			"imnNumberofRecordsGenerated", "imnPartBinIDs", "imnPartClassIDs", "imnPartGroupIDs", "imnPartWarehouseIDs", "imnPostedDate", "imnRowVersion", "imnInventoryCountID", "imnStatus", "imnSupplierOrganizationIDs"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("InventoryCounts");
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
		using (DataTable dataTable = GetAsDataTable("InventoryCounts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInventoryCountInformationDto eRPInventoryCountInformationDto = new ERPInventoryCountInformationDto();
				eRPInventoryCountInformationDto.imnCreatedBy = dataTable.Rows[i].Field<string>("imnCreatedBy");
				eRPInventoryCountInformationDto.imnCreatedDate = dataTable.Rows[i].Field<DateTime?>("imnCreatedDate");
				eRPInventoryCountInformationDto.imnCycleCodeID = dataTable.Rows[i].Field<string>("imnCycleCodeID");
				eRPInventoryCountInformationDto.imnUniqueID = dataTable.Rows[i].Field<Guid>("imnUniqueID");
				eRPInventoryCountInformationDto.imnGeneratedDate = dataTable.Rows[i].Field<DateTime?>("imnGeneratedDate");
				eRPInventoryCountInformationDto.imnExcludeInactivePartBins = dataTable.Rows[i].Field<bool>("imnExcludeInactivePartBins");
				eRPInventoryCountInformationDto.imnIncludeBlankPartClass = dataTable.Rows[i].Field<bool>("imnIncludeBlankPartClass");
				eRPInventoryCountInformationDto.imnIncludeBlankPartGroup = dataTable.Rows[i].Field<bool>("imnIncludeBlankPartGroup");
				eRPInventoryCountInformationDto.imnPostedToInventory = dataTable.Rows[i].Field<bool>("imnPostedToInventory");
				eRPInventoryCountInformationDto.imnRecordsGenerated = dataTable.Rows[i].Field<bool>("imnRecordsGenerated");
				eRPInventoryCountInformationDto.imnNumberofRecordsGenerated = dataTable.Rows[i].Field<int>("imnNumberofRecordsGenerated");
				eRPInventoryCountInformationDto.imnPartBinIDs = dataTable.Rows[i].Field<string>("imnPartBinIDs");
				eRPInventoryCountInformationDto.imnPartClassIDs = dataTable.Rows[i].Field<string>("imnPartClassIDs");
				eRPInventoryCountInformationDto.imnPartGroupIDs = dataTable.Rows[i].Field<string>("imnPartGroupIDs");
				eRPInventoryCountInformationDto.imnPartWarehouseIDs = dataTable.Rows[i].Field<string>("imnPartWarehouseIDs");
				eRPInventoryCountInformationDto.imnPostedDate = dataTable.Rows[i].Field<DateTime?>("imnPostedDate");
				eRPInventoryCountInformationDto.imnRowVersion = dataTable.Rows[i].Field<byte[]>("imnRowVersion");
				eRPInventoryCountInformationDto.imnInventoryCountID = dataTable.Rows[i].Field<int>("imnInventoryCountID");
				eRPInventoryCountInformationDto.imnStatus = dataTable.Rows[i].Field<byte>("imnStatus");
				eRPInventoryCountInformationDto.imnSupplierOrganizationIDs = dataTable.Rows[i].Field<string>("imnSupplierOrganizationIDs");
				eRPInventoryCountInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInventoryCountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInventoryCountInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInventoryCountInformationDto> GetInventoryCount(Guid inventoryCountId)
	{
		ERPInventoryCountInformationDto eRPInventoryCountInformationDto = new ERPInventoryCountInformationDto();
		InitializeParameterLists();
		string[] collection = new string[20]
		{
			"imnCreatedBy", "imnCreatedDate", "imnCycleCodeID", "imnUniqueID", "imnGeneratedDate", "imnExcludeInactivePartBins", "imnIncludeBlankPartClass", "imnIncludeBlankPartGroup", "imnPostedToInventory", "imnRecordsGenerated",
			"imnNumberofRecordsGenerated", "imnPartBinIDs", "imnPartClassIDs", "imnPartGroupIDs", "imnPartWarehouseIDs", "imnPostedDate", "imnRowVersion", "imnInventoryCountID", "imnStatus", "imnSupplierOrganizationIDs"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imnUniqueID|C", inventoryCountId);
		AddCustomFieldsToSelectList("InventoryCounts");
		using (DataTable dataTable = GetAsDataTable("InventoryCounts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInventoryCountInformationDto);
			}
			eRPInventoryCountInformationDto.imnCreatedBy = dataTable.Rows[0].Field<string>("imnCreatedBy");
			eRPInventoryCountInformationDto.imnCreatedDate = dataTable.Rows[0].Field<DateTime?>("imnCreatedDate");
			eRPInventoryCountInformationDto.imnCycleCodeID = dataTable.Rows[0].Field<string>("imnCycleCodeID");
			eRPInventoryCountInformationDto.imnUniqueID = dataTable.Rows[0].Field<Guid>("imnUniqueID");
			eRPInventoryCountInformationDto.imnGeneratedDate = dataTable.Rows[0].Field<DateTime?>("imnGeneratedDate");
			eRPInventoryCountInformationDto.imnExcludeInactivePartBins = dataTable.Rows[0].Field<bool>("imnExcludeInactivePartBins");
			eRPInventoryCountInformationDto.imnIncludeBlankPartClass = dataTable.Rows[0].Field<bool>("imnIncludeBlankPartClass");
			eRPInventoryCountInformationDto.imnIncludeBlankPartGroup = dataTable.Rows[0].Field<bool>("imnIncludeBlankPartGroup");
			eRPInventoryCountInformationDto.imnPostedToInventory = dataTable.Rows[0].Field<bool>("imnPostedToInventory");
			eRPInventoryCountInformationDto.imnRecordsGenerated = dataTable.Rows[0].Field<bool>("imnRecordsGenerated");
			eRPInventoryCountInformationDto.imnNumberofRecordsGenerated = dataTable.Rows[0].Field<int>("imnNumberofRecordsGenerated");
			eRPInventoryCountInformationDto.imnPartBinIDs = dataTable.Rows[0].Field<string>("imnPartBinIDs");
			eRPInventoryCountInformationDto.imnPartClassIDs = dataTable.Rows[0].Field<string>("imnPartClassIDs");
			eRPInventoryCountInformationDto.imnPartGroupIDs = dataTable.Rows[0].Field<string>("imnPartGroupIDs");
			eRPInventoryCountInformationDto.imnPartWarehouseIDs = dataTable.Rows[0].Field<string>("imnPartWarehouseIDs");
			eRPInventoryCountInformationDto.imnPostedDate = dataTable.Rows[0].Field<DateTime?>("imnPostedDate");
			eRPInventoryCountInformationDto.imnRowVersion = dataTable.Rows[0].Field<byte[]>("imnRowVersion");
			eRPInventoryCountInformationDto.imnInventoryCountID = dataTable.Rows[0].Field<int>("imnInventoryCountID");
			eRPInventoryCountInformationDto.imnStatus = dataTable.Rows[0].Field<byte>("imnStatus");
			eRPInventoryCountInformationDto.imnSupplierOrganizationIDs = dataTable.Rows[0].Field<string>("imnSupplierOrganizationIDs");
			eRPInventoryCountInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInventoryCountInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInventoryCountInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInventoryCount(ERPInventoryCountDto inventoryCount)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM InventoryCounts WHERE imnUniqueID = " + M1Util.ConvertToLinq(inventoryCount.imnUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imnInventoryCountID"] = inventoryCount.imnInventoryCountID;
				inventoryCount.imnUniqueID = ((inventoryCount.imnUniqueID == Guid.Empty) ? Guid.NewGuid() : inventoryCount.imnUniqueID);
				dataRow["imnUniqueID"] = inventoryCount.imnUniqueID;
				dataRow["imnCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imnCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The InventoryCount could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (inventoryCount.imnRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the InventoryCount is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imnRowVersion"], inventoryCount.imnRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the InventoryCount has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the InventoryCount again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imnCycleCodeID"] = inventoryCount.imnCycleCodeID;
			DataRow dataRow2 = dataRow;
			DateTime? imnGeneratedDate = inventoryCount.imnGeneratedDate;
			dataRow2["imnGeneratedDate"] = (imnGeneratedDate.HasValue ? ((object)imnGeneratedDate.GetValueOrDefault()) : dataRow["imnGeneratedDate"]);
			dataRow["imnExcludeInactivePartBins"] = inventoryCount.imnExcludeInactivePartBins;
			dataRow["imnIncludeBlankPartClass"] = inventoryCount.imnIncludeBlankPartClass;
			dataRow["imnIncludeBlankPartGroup"] = inventoryCount.imnIncludeBlankPartGroup;
			dataRow["imnPostedToInventory"] = inventoryCount.imnPostedToInventory;
			dataRow["imnRecordsGenerated"] = inventoryCount.imnRecordsGenerated;
			dataRow["imnNumberofRecordsGenerated"] = inventoryCount.imnNumberofRecordsGenerated;
			dataRow["imnPartBinIDs"] = inventoryCount.imnPartBinIDs ?? dataRow["imnPartBinIDs"];
			dataRow["imnPartClassIDs"] = inventoryCount.imnPartClassIDs ?? dataRow["imnPartClassIDs"];
			dataRow["imnPartGroupIDs"] = inventoryCount.imnPartGroupIDs ?? dataRow["imnPartGroupIDs"];
			dataRow["imnPartWarehouseIDs"] = inventoryCount.imnPartWarehouseIDs ?? dataRow["imnPartWarehouseIDs"];
			DataRow dataRow3 = dataRow;
			imnGeneratedDate = inventoryCount.imnPostedDate;
			dataRow3["imnPostedDate"] = (imnGeneratedDate.HasValue ? ((object)imnGeneratedDate.GetValueOrDefault()) : dataRow["imnPostedDate"]);
			dataRow["imnStatus"] = inventoryCount.imnStatus;
			dataRow["imnSupplierOrganizationIDs"] = inventoryCount.imnSupplierOrganizationIDs ?? dataRow["imnSupplierOrganizationIDs"];
			if (inventoryCount.CustomFields != null && inventoryCount.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inventoryCount.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the InventoryCount [{inventoryCount.imnUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the InventoryCount [{inventoryCount.imnUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
