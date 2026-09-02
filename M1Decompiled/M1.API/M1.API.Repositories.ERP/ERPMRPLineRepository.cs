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

public class ERPMRPLineRepository : APIBaseRepository, IERPMRPLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPMRPLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMRPLineExist(Guid mRPLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("mrlUniqueID|C", mRPLineId);
		base.selectList.Add("mrlUniqueID");
		return Task.FromResult(GetAsObject("MRPLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMRPLineInformationDto>> GetAllMRPLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMRPLineInformationDto> collection = new List<ERPMRPLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[21]
		{
			"mrlCreatedBy", "mrlCreatedDate", "mrlUniqueID", "mrlForecastDemand", "mrlInvQtyInProduction", "mrlCompleted", "mrlDataMissing", "mrlLineID", "mrlMaximumQuantity", "mrlMfgLotSize",
			"mrlMinimumQuantity", "mrlPartID", "mrlPartRevisionID", "mrlPartShortDescription", "mrlPlantIDs", "mrlQuantityAllocated", "mrlQuantityOnHand", "mrlQuantityToInspect", "mrlRowVersion", "mrlSessionID",
			"mrlWarehouseIDs"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MRPLines");
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
		using (DataTable dataTable = GetAsDataTable("MRPLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMRPLineInformationDto eRPMRPLineInformationDto = new ERPMRPLineInformationDto();
				eRPMRPLineInformationDto.mrlCreatedBy = dataTable.Rows[i].Field<string>("mrlCreatedBy");
				eRPMRPLineInformationDto.mrlCreatedDate = dataTable.Rows[i].Field<DateTime?>("mrlCreatedDate");
				eRPMRPLineInformationDto.mrlUniqueID = dataTable.Rows[i].Field<Guid>("mrlUniqueID");
				eRPMRPLineInformationDto.mrlForecastDemand = dataTable.Rows[i].Field<decimal>("mrlForecastDemand");
				eRPMRPLineInformationDto.mrlInvQtyInProduction = dataTable.Rows[i].Field<decimal>("mrlInvQtyInProduction");
				eRPMRPLineInformationDto.mrlCompleted = dataTable.Rows[i].Field<bool>("mrlCompleted");
				eRPMRPLineInformationDto.mrlDataMissing = dataTable.Rows[i].Field<bool>("mrlDataMissing");
				eRPMRPLineInformationDto.mrlLineID = dataTable.Rows[i].Field<int>("mrlLineID");
				eRPMRPLineInformationDto.mrlMaximumQuantity = dataTable.Rows[i].Field<decimal>("mrlMaximumQuantity");
				eRPMRPLineInformationDto.mrlMfgLotSize = dataTable.Rows[i].Field<decimal>("mrlMfgLotSize");
				eRPMRPLineInformationDto.mrlMinimumQuantity = dataTable.Rows[i].Field<decimal>("mrlMinimumQuantity");
				eRPMRPLineInformationDto.mrlPartID = dataTable.Rows[i].Field<string>("mrlPartID");
				eRPMRPLineInformationDto.mrlPartRevisionID = dataTable.Rows[i].Field<string>("mrlPartRevisionID");
				eRPMRPLineInformationDto.mrlPartShortDescription = dataTable.Rows[i].Field<string>("mrlPartShortDescription");
				eRPMRPLineInformationDto.mrlPlantIDs = dataTable.Rows[i].Field<string>("mrlPlantIDs");
				eRPMRPLineInformationDto.mrlQuantityAllocated = dataTable.Rows[i].Field<decimal>("mrlQuantityAllocated");
				eRPMRPLineInformationDto.mrlQuantityOnHand = dataTable.Rows[i].Field<decimal>("mrlQuantityOnHand");
				eRPMRPLineInformationDto.mrlQuantityToInspect = dataTable.Rows[i].Field<decimal>("mrlQuantityToInspect");
				eRPMRPLineInformationDto.mrlRowVersion = dataTable.Rows[i].Field<byte[]>("mrlRowVersion");
				eRPMRPLineInformationDto.mrlSessionID = dataTable.Rows[i].Field<string>("mrlSessionID");
				eRPMRPLineInformationDto.mrlWarehouseIDs = dataTable.Rows[i].Field<string>("mrlWarehouseIDs");
				eRPMRPLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMRPLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMRPLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMRPLineInformationDto> GetMRPLine(Guid mRPLineId)
	{
		ERPMRPLineInformationDto eRPMRPLineInformationDto = new ERPMRPLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[21]
		{
			"mrlCreatedBy", "mrlCreatedDate", "mrlUniqueID", "mrlForecastDemand", "mrlInvQtyInProduction", "mrlCompleted", "mrlDataMissing", "mrlLineID", "mrlMaximumQuantity", "mrlMfgLotSize",
			"mrlMinimumQuantity", "mrlPartID", "mrlPartRevisionID", "mrlPartShortDescription", "mrlPlantIDs", "mrlQuantityAllocated", "mrlQuantityOnHand", "mrlQuantityToInspect", "mrlRowVersion", "mrlSessionID",
			"mrlWarehouseIDs"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("mrlUniqueID|C", mRPLineId);
		AddCustomFieldsToSelectList("MRPLines");
		using (DataTable dataTable = GetAsDataTable("MRPLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMRPLineInformationDto);
			}
			eRPMRPLineInformationDto.mrlCreatedBy = dataTable.Rows[0].Field<string>("mrlCreatedBy");
			eRPMRPLineInformationDto.mrlCreatedDate = dataTable.Rows[0].Field<DateTime?>("mrlCreatedDate");
			eRPMRPLineInformationDto.mrlUniqueID = dataTable.Rows[0].Field<Guid>("mrlUniqueID");
			eRPMRPLineInformationDto.mrlForecastDemand = dataTable.Rows[0].Field<decimal>("mrlForecastDemand");
			eRPMRPLineInformationDto.mrlInvQtyInProduction = dataTable.Rows[0].Field<decimal>("mrlInvQtyInProduction");
			eRPMRPLineInformationDto.mrlCompleted = dataTable.Rows[0].Field<bool>("mrlCompleted");
			eRPMRPLineInformationDto.mrlDataMissing = dataTable.Rows[0].Field<bool>("mrlDataMissing");
			eRPMRPLineInformationDto.mrlLineID = dataTable.Rows[0].Field<int>("mrlLineID");
			eRPMRPLineInformationDto.mrlMaximumQuantity = dataTable.Rows[0].Field<decimal>("mrlMaximumQuantity");
			eRPMRPLineInformationDto.mrlMfgLotSize = dataTable.Rows[0].Field<decimal>("mrlMfgLotSize");
			eRPMRPLineInformationDto.mrlMinimumQuantity = dataTable.Rows[0].Field<decimal>("mrlMinimumQuantity");
			eRPMRPLineInformationDto.mrlPartID = dataTable.Rows[0].Field<string>("mrlPartID");
			eRPMRPLineInformationDto.mrlPartRevisionID = dataTable.Rows[0].Field<string>("mrlPartRevisionID");
			eRPMRPLineInformationDto.mrlPartShortDescription = dataTable.Rows[0].Field<string>("mrlPartShortDescription");
			eRPMRPLineInformationDto.mrlPlantIDs = dataTable.Rows[0].Field<string>("mrlPlantIDs");
			eRPMRPLineInformationDto.mrlQuantityAllocated = dataTable.Rows[0].Field<decimal>("mrlQuantityAllocated");
			eRPMRPLineInformationDto.mrlQuantityOnHand = dataTable.Rows[0].Field<decimal>("mrlQuantityOnHand");
			eRPMRPLineInformationDto.mrlQuantityToInspect = dataTable.Rows[0].Field<decimal>("mrlQuantityToInspect");
			eRPMRPLineInformationDto.mrlRowVersion = dataTable.Rows[0].Field<byte[]>("mrlRowVersion");
			eRPMRPLineInformationDto.mrlSessionID = dataTable.Rows[0].Field<string>("mrlSessionID");
			eRPMRPLineInformationDto.mrlWarehouseIDs = dataTable.Rows[0].Field<string>("mrlWarehouseIDs");
			eRPMRPLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMRPLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMRPLineInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMRPLine(ERPMRPLineDto mRPLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MRPLines WHERE mrlUniqueID = " + M1Util.ConvertToLinq(mRPLine.mrlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["mrlSessionID"] = mRPLine.mrlSessionID.ToUpper();
				dataRow["mrlLineID"] = mRPLine.mrlLineID;
				mRPLine.mrlUniqueID = ((mRPLine.mrlUniqueID == Guid.Empty) ? Guid.NewGuid() : mRPLine.mrlUniqueID);
				dataRow["mrlUniqueID"] = mRPLine.mrlUniqueID;
				dataRow["mrlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["mrlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MRPLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (mRPLine.mrlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MRPLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["mrlRowVersion"], mRPLine.mrlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MRPLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MRPLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["mrlForecastDemand"] = mRPLine.mrlForecastDemand;
			dataRow["mrlInvQtyInProduction"] = mRPLine.mrlInvQtyInProduction;
			dataRow["mrlCompleted"] = mRPLine.mrlCompleted;
			dataRow["mrlDataMissing"] = mRPLine.mrlDataMissing;
			dataRow["mrlMaximumQuantity"] = mRPLine.mrlMaximumQuantity;
			dataRow["mrlMfgLotSize"] = mRPLine.mrlMfgLotSize;
			dataRow["mrlMinimumQuantity"] = mRPLine.mrlMinimumQuantity;
			dataRow["mrlPartID"] = mRPLine.mrlPartID;
			dataRow["mrlPartRevisionID"] = mRPLine.mrlPartRevisionID;
			dataRow["mrlPartShortDescription"] = mRPLine.mrlPartShortDescription;
			dataRow["mrlPlantIDs"] = mRPLine.mrlPlantIDs ?? dataRow["mrlPlantIDs"];
			dataRow["mrlQuantityAllocated"] = mRPLine.mrlQuantityAllocated;
			dataRow["mrlQuantityOnHand"] = mRPLine.mrlQuantityOnHand;
			dataRow["mrlQuantityToInspect"] = mRPLine.mrlQuantityToInspect;
			dataRow["mrlWarehouseIDs"] = mRPLine.mrlWarehouseIDs ?? dataRow["mrlWarehouseIDs"];
			if (mRPLine.CustomFields != null && mRPLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in mRPLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MRPLine [{mRPLine.mrlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MRPLine [{mRPLine.mrlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
