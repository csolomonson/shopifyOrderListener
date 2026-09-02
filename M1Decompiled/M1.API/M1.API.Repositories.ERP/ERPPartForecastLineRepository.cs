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

public class ERPPartForecastLineRepository : APIBaseRepository, IERPPartForecastLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartForecastLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartForecastLineExist(Guid partForecastLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("inlUniqueID|C", partForecastLineId);
		base.selectList.Add("inlUniqueID");
		return Task.FromResult(GetAsObject("PartForecastLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartForecastLineInformationDto>> GetAllPartForecastLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartForecastLineInformationDto> collection = new List<ERPPartForecastLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"inlActualBalance", "inlActualQuantity", "inlCreatedBy", "inlCreatedDate", "inlEndDate", "inlUniqueID", "inlForecastBalance", "inlForecastQuantity", "inlIncludeInMRP", "inlPartForecastPeriodID",
			"inlPartForecastYearID", "inlPartID", "inlPartRevisionID", "inlRemainingQuantity", "inlRemainingQuantityBalance", "inlRowVersion", "inlStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartForecastLines");
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
		using (DataTable dataTable = GetAsDataTable("PartForecastLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartForecastLineInformationDto eRPPartForecastLineInformationDto = new ERPPartForecastLineInformationDto();
				eRPPartForecastLineInformationDto.inlActualBalance = dataTable.Rows[i].Field<decimal>("inlActualBalance");
				eRPPartForecastLineInformationDto.inlActualQuantity = dataTable.Rows[i].Field<decimal>("inlActualQuantity");
				eRPPartForecastLineInformationDto.inlCreatedBy = dataTable.Rows[i].Field<string>("inlCreatedBy");
				eRPPartForecastLineInformationDto.inlCreatedDate = dataTable.Rows[i].Field<DateTime?>("inlCreatedDate");
				eRPPartForecastLineInformationDto.inlEndDate = dataTable.Rows[i].Field<DateTime?>("inlEndDate");
				eRPPartForecastLineInformationDto.inlUniqueID = dataTable.Rows[i].Field<Guid>("inlUniqueID");
				eRPPartForecastLineInformationDto.inlForecastBalance = dataTable.Rows[i].Field<decimal>("inlForecastBalance");
				eRPPartForecastLineInformationDto.inlForecastQuantity = dataTable.Rows[i].Field<decimal>("inlForecastQuantity");
				eRPPartForecastLineInformationDto.inlIncludeInMRP = dataTable.Rows[i].Field<bool>("inlIncludeInMRP");
				eRPPartForecastLineInformationDto.inlPartForecastPeriodID = dataTable.Rows[i].Field<short>("inlPartForecastPeriodID");
				eRPPartForecastLineInformationDto.inlPartForecastYearID = dataTable.Rows[i].Field<short>("inlPartForecastYearID");
				eRPPartForecastLineInformationDto.inlPartID = dataTable.Rows[i].Field<string>("inlPartID");
				eRPPartForecastLineInformationDto.inlPartRevisionID = dataTable.Rows[i].Field<string>("inlPartRevisionID");
				eRPPartForecastLineInformationDto.inlRemainingQuantity = dataTable.Rows[i].Field<decimal>("inlRemainingQuantity");
				eRPPartForecastLineInformationDto.inlRemainingQuantityBalance = dataTable.Rows[i].Field<decimal>("inlRemainingQuantityBalance");
				eRPPartForecastLineInformationDto.inlRowVersion = dataTable.Rows[i].Field<byte[]>("inlRowVersion");
				eRPPartForecastLineInformationDto.inlStartDate = dataTable.Rows[i].Field<DateTime?>("inlStartDate");
				eRPPartForecastLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartForecastLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartForecastLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartForecastLineInformationDto> GetPartForecastLine(Guid partForecastLineId)
	{
		ERPPartForecastLineInformationDto eRPPartForecastLineInformationDto = new ERPPartForecastLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"inlActualBalance", "inlActualQuantity", "inlCreatedBy", "inlCreatedDate", "inlEndDate", "inlUniqueID", "inlForecastBalance", "inlForecastQuantity", "inlIncludeInMRP", "inlPartForecastPeriodID",
			"inlPartForecastYearID", "inlPartID", "inlPartRevisionID", "inlRemainingQuantity", "inlRemainingQuantityBalance", "inlRowVersion", "inlStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("inlUniqueID|C", partForecastLineId);
		AddCustomFieldsToSelectList("PartForecastLines");
		using (DataTable dataTable = GetAsDataTable("PartForecastLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartForecastLineInformationDto);
			}
			eRPPartForecastLineInformationDto.inlActualBalance = dataTable.Rows[0].Field<decimal>("inlActualBalance");
			eRPPartForecastLineInformationDto.inlActualQuantity = dataTable.Rows[0].Field<decimal>("inlActualQuantity");
			eRPPartForecastLineInformationDto.inlCreatedBy = dataTable.Rows[0].Field<string>("inlCreatedBy");
			eRPPartForecastLineInformationDto.inlCreatedDate = dataTable.Rows[0].Field<DateTime?>("inlCreatedDate");
			eRPPartForecastLineInformationDto.inlEndDate = dataTable.Rows[0].Field<DateTime?>("inlEndDate");
			eRPPartForecastLineInformationDto.inlUniqueID = dataTable.Rows[0].Field<Guid>("inlUniqueID");
			eRPPartForecastLineInformationDto.inlForecastBalance = dataTable.Rows[0].Field<decimal>("inlForecastBalance");
			eRPPartForecastLineInformationDto.inlForecastQuantity = dataTable.Rows[0].Field<decimal>("inlForecastQuantity");
			eRPPartForecastLineInformationDto.inlIncludeInMRP = dataTable.Rows[0].Field<bool>("inlIncludeInMRP");
			eRPPartForecastLineInformationDto.inlPartForecastPeriodID = dataTable.Rows[0].Field<short>("inlPartForecastPeriodID");
			eRPPartForecastLineInformationDto.inlPartForecastYearID = dataTable.Rows[0].Field<short>("inlPartForecastYearID");
			eRPPartForecastLineInformationDto.inlPartID = dataTable.Rows[0].Field<string>("inlPartID");
			eRPPartForecastLineInformationDto.inlPartRevisionID = dataTable.Rows[0].Field<string>("inlPartRevisionID");
			eRPPartForecastLineInformationDto.inlRemainingQuantity = dataTable.Rows[0].Field<decimal>("inlRemainingQuantity");
			eRPPartForecastLineInformationDto.inlRemainingQuantityBalance = dataTable.Rows[0].Field<decimal>("inlRemainingQuantityBalance");
			eRPPartForecastLineInformationDto.inlRowVersion = dataTable.Rows[0].Field<byte[]>("inlRowVersion");
			eRPPartForecastLineInformationDto.inlStartDate = dataTable.Rows[0].Field<DateTime?>("inlStartDate");
			eRPPartForecastLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartForecastLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartForecastLineInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartForecastLine(ERPPartForecastLineDto partForecastLine)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartForecastLines WHERE inlUniqueID = " + M1Util.ConvertToLinq(partForecastLine.inlUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["inlPartID"] = partForecastLine.inlPartID.ToUpper();
				dataRow["inlPartRevisionID"] = partForecastLine.inlPartRevisionID.ToUpper();
				dataRow["inlPartForecastYearID"] = partForecastLine.inlPartForecastYearID;
				dataRow["inlPartForecastPeriodID"] = partForecastLine.inlPartForecastPeriodID;
				partForecastLine.inlUniqueID = ((partForecastLine.inlUniqueID == Guid.Empty) ? Guid.NewGuid() : partForecastLine.inlUniqueID);
				dataRow["inlUniqueID"] = partForecastLine.inlUniqueID;
				dataRow["inlCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["inlCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartForecastLine could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partForecastLine.inlRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartForecastLine is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["inlRowVersion"], partForecastLine.inlRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartForecastLine has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartForecastLine again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["inlActualBalance"] = partForecastLine.inlActualBalance;
			dataRow["inlActualQuantity"] = partForecastLine.inlActualQuantity;
			DataRow dataRow2 = dataRow;
			DateTime? inlEndDate = partForecastLine.inlEndDate;
			dataRow2["inlEndDate"] = (inlEndDate.HasValue ? ((object)inlEndDate.GetValueOrDefault()) : dataRow["inlEndDate"]);
			dataRow["inlForecastBalance"] = partForecastLine.inlForecastBalance;
			dataRow["inlForecastQuantity"] = partForecastLine.inlForecastQuantity;
			dataRow["inlIncludeInMRP"] = partForecastLine.inlIncludeInMRP;
			dataRow["inlRemainingQuantity"] = partForecastLine.inlRemainingQuantity;
			dataRow["inlRemainingQuantityBalance"] = partForecastLine.inlRemainingQuantityBalance;
			DataRow dataRow3 = dataRow;
			inlEndDate = partForecastLine.inlStartDate;
			dataRow3["inlStartDate"] = (inlEndDate.HasValue ? ((object)inlEndDate.GetValueOrDefault()) : dataRow["inlStartDate"]);
			if (partForecastLine.CustomFields != null && partForecastLine.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partForecastLine.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartForecastLine [{partForecastLine.inlUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartForecastLine [{partForecastLine.inlUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
