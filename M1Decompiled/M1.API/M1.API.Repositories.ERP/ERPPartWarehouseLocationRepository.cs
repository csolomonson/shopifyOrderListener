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

public class ERPPartWarehouseLocationRepository : APIBaseRepository, IERPPartWarehouseLocationRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartWarehouseLocationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartWarehouseLocationExist(Guid partWarehouseLocationId)
	{
		InitializeParameterLists();
		base.filterList.Add("imlUniqueID|C", partWarehouseLocationId);
		base.selectList.Add("imlUniqueID");
		return Task.FromResult(GetAsObject("PartWarehouseLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartWarehouseLocationInformationDto>> GetAllPartWarehouseLocations(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartWarehouseLocationInformationDto> collection = new List<ERPPartWarehouseLocationInformationDto>();
		InitializeParameterLists();
		string[] array = new string[12]
		{
			"imlCreatedBy", "imlCreatedDate", "imlUniqueID", "imlNonNettable", "imLLastRunDatePurchasePlanner", "imlMaximumQuantity", "imlMinimumQuantity", "imlPartID", "imlPartRevisionID", "imlPartWarehouseID",
			"imlQuantityInTransit", "imlRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartWarehouseLocations");
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
		using (DataTable dataTable = GetAsDataTable("PartWarehouseLocations", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartWarehouseLocationInformationDto eRPPartWarehouseLocationInformationDto = new ERPPartWarehouseLocationInformationDto();
				eRPPartWarehouseLocationInformationDto.imlCreatedBy = dataTable.Rows[i].Field<string>("imlCreatedBy");
				eRPPartWarehouseLocationInformationDto.imlCreatedDate = dataTable.Rows[i].Field<DateTime?>("imlCreatedDate");
				eRPPartWarehouseLocationInformationDto.imlUniqueID = dataTable.Rows[i].Field<Guid>("imlUniqueID");
				eRPPartWarehouseLocationInformationDto.imlNonNettable = dataTable.Rows[i].Field<bool>("imlNonNettable");
				eRPPartWarehouseLocationInformationDto.imLLastRunDatePurchasePlanner = dataTable.Rows[i].Field<DateTime?>("imLLastRunDatePurchasePlanner");
				eRPPartWarehouseLocationInformationDto.imlMaximumQuantity = dataTable.Rows[i].Field<decimal>("imlMaximumQuantity");
				eRPPartWarehouseLocationInformationDto.imlMinimumQuantity = dataTable.Rows[i].Field<decimal>("imlMinimumQuantity");
				eRPPartWarehouseLocationInformationDto.imlPartID = dataTable.Rows[i].Field<string>("imlPartID");
				eRPPartWarehouseLocationInformationDto.imlPartRevisionID = dataTable.Rows[i].Field<string>("imlPartRevisionID");
				eRPPartWarehouseLocationInformationDto.imlPartWarehouseID = dataTable.Rows[i].Field<string>("imlPartWarehouseID");
				eRPPartWarehouseLocationInformationDto.imlQuantityInTransit = dataTable.Rows[i].Field<decimal>("imlQuantityInTransit");
				eRPPartWarehouseLocationInformationDto.imlRowVersion = dataTable.Rows[i].Field<byte[]>("imlRowVersion");
				eRPPartWarehouseLocationInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartWarehouseLocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartWarehouseLocationInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartWarehouseLocationInformationDto> GetPartWarehouseLocation(Guid partWarehouseLocationId)
	{
		ERPPartWarehouseLocationInformationDto eRPPartWarehouseLocationInformationDto = new ERPPartWarehouseLocationInformationDto();
		InitializeParameterLists();
		string[] collection = new string[12]
		{
			"imlCreatedBy", "imlCreatedDate", "imlUniqueID", "imlNonNettable", "imLLastRunDatePurchasePlanner", "imlMaximumQuantity", "imlMinimumQuantity", "imlPartID", "imlPartRevisionID", "imlPartWarehouseID",
			"imlQuantityInTransit", "imlRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imlUniqueID|C", partWarehouseLocationId);
		AddCustomFieldsToSelectList("PartWarehouseLocations");
		using (DataTable dataTable = GetAsDataTable("PartWarehouseLocations", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartWarehouseLocationInformationDto);
			}
			eRPPartWarehouseLocationInformationDto.imlCreatedBy = dataTable.Rows[0].Field<string>("imlCreatedBy");
			eRPPartWarehouseLocationInformationDto.imlCreatedDate = dataTable.Rows[0].Field<DateTime?>("imlCreatedDate");
			eRPPartWarehouseLocationInformationDto.imlUniqueID = dataTable.Rows[0].Field<Guid>("imlUniqueID");
			eRPPartWarehouseLocationInformationDto.imlNonNettable = dataTable.Rows[0].Field<bool>("imlNonNettable");
			eRPPartWarehouseLocationInformationDto.imLLastRunDatePurchasePlanner = dataTable.Rows[0].Field<DateTime?>("imLLastRunDatePurchasePlanner");
			eRPPartWarehouseLocationInformationDto.imlMaximumQuantity = dataTable.Rows[0].Field<decimal>("imlMaximumQuantity");
			eRPPartWarehouseLocationInformationDto.imlMinimumQuantity = dataTable.Rows[0].Field<decimal>("imlMinimumQuantity");
			eRPPartWarehouseLocationInformationDto.imlPartID = dataTable.Rows[0].Field<string>("imlPartID");
			eRPPartWarehouseLocationInformationDto.imlPartRevisionID = dataTable.Rows[0].Field<string>("imlPartRevisionID");
			eRPPartWarehouseLocationInformationDto.imlPartWarehouseID = dataTable.Rows[0].Field<string>("imlPartWarehouseID");
			eRPPartWarehouseLocationInformationDto.imlQuantityInTransit = dataTable.Rows[0].Field<decimal>("imlQuantityInTransit");
			eRPPartWarehouseLocationInformationDto.imlRowVersion = dataTable.Rows[0].Field<byte[]>("imlRowVersion");
			eRPPartWarehouseLocationInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartWarehouseLocationInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartWarehouseLocationInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartWarehouseLocation(ERPPartWarehouseLocationDto partWarehouseLocation)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartWarehouseLocations WHERE imlUniqueID = " + M1Util.ConvertToLinq(partWarehouseLocation.imlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imlPartID"] = partWarehouseLocation.imlPartID.ToUpper();
				dataRow["imlPartRevisionID"] = partWarehouseLocation.imlPartRevisionID.ToUpper();
				dataRow["imlPartWarehouseID"] = partWarehouseLocation.imlPartWarehouseID.ToUpper();
				partWarehouseLocation.imlUniqueID = ((partWarehouseLocation.imlUniqueID == Guid.Empty) ? Guid.NewGuid() : partWarehouseLocation.imlUniqueID);
				dataRow["imlUniqueID"] = partWarehouseLocation.imlUniqueID;
				dataRow["imlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartWarehouseLocation could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partWarehouseLocation.imlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartWarehouseLocation is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imlRowVersion"], partWarehouseLocation.imlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartWarehouseLocation has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartWarehouseLocation again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imlNonNettable"] = partWarehouseLocation.imlNonNettable;
			DataRow dataRow2 = dataRow;
			DateTime? imLLastRunDatePurchasePlanner = partWarehouseLocation.imLLastRunDatePurchasePlanner;
			dataRow2["imLLastRunDatePurchasePlanner"] = (imLLastRunDatePurchasePlanner.HasValue ? ((object)imLLastRunDatePurchasePlanner.GetValueOrDefault()) : dataRow["imLLastRunDatePurchasePlanner"]);
			dataRow["imlMaximumQuantity"] = partWarehouseLocation.imlMaximumQuantity;
			dataRow["imlMinimumQuantity"] = partWarehouseLocation.imlMinimumQuantity;
			dataRow["imlQuantityInTransit"] = partWarehouseLocation.imlQuantityInTransit;
			if (partWarehouseLocation.CustomFields != null && partWarehouseLocation.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partWarehouseLocation.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartWarehouseLocation [{partWarehouseLocation.imlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartWarehouseLocation [{partWarehouseLocation.imlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
