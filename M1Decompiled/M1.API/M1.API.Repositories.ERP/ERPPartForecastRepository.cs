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

public class ERPPartForecastRepository : APIBaseRepository, IERPPartForecastRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartForecastRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartForecastExist(Guid partForecastId)
	{
		InitializeParameterLists();
		base.filterList.Add("inpUniqueID|C", partForecastId);
		base.selectList.Add("inpUniqueID");
		return Task.FromResult(GetAsObject("PartForecasts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartForecastInformationDto>> GetAllPartForecasts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartForecastInformationDto> collection = new List<ERPPartForecastInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"inpAnnualQuantity", "inpCreatedBy", "inpCreatedDate", "inpEndDate", "inpUniqueID", "inpForecastMethod", "inpForecastNumberOfYears", "inpIntervalType", "inpPartForecastYearID", "inpPartID",
			"inpPartRevisionID", "inpRowVersion", "inpStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartForecasts");
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
		using (DataTable dataTable = GetAsDataTable("PartForecasts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartForecastInformationDto eRPPartForecastInformationDto = new ERPPartForecastInformationDto();
				eRPPartForecastInformationDto.inpAnnualQuantity = dataTable.Rows[i].Field<decimal>("inpAnnualQuantity");
				eRPPartForecastInformationDto.inpCreatedBy = dataTable.Rows[i].Field<string>("inpCreatedBy");
				eRPPartForecastInformationDto.inpCreatedDate = dataTable.Rows[i].Field<DateTime?>("inpCreatedDate");
				eRPPartForecastInformationDto.inpEndDate = dataTable.Rows[i].Field<DateTime?>("inpEndDate");
				eRPPartForecastInformationDto.inpUniqueID = dataTable.Rows[i].Field<Guid>("inpUniqueID");
				eRPPartForecastInformationDto.inpForecastMethod = dataTable.Rows[i].Field<string>("inpForecastMethod");
				eRPPartForecastInformationDto.inpForecastNumberOfYears = dataTable.Rows[i].Field<byte>("inpForecastNumberOfYears");
				eRPPartForecastInformationDto.inpIntervalType = dataTable.Rows[i].Field<string>("inpIntervalType");
				eRPPartForecastInformationDto.inpPartForecastYearID = dataTable.Rows[i].Field<short>("inpPartForecastYearID");
				eRPPartForecastInformationDto.inpPartID = dataTable.Rows[i].Field<string>("inpPartID");
				eRPPartForecastInformationDto.inpPartRevisionID = dataTable.Rows[i].Field<string>("inpPartRevisionID");
				eRPPartForecastInformationDto.inpRowVersion = dataTable.Rows[i].Field<byte[]>("inpRowVersion");
				eRPPartForecastInformationDto.inpStartDate = dataTable.Rows[i].Field<DateTime?>("inpStartDate");
				eRPPartForecastInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartForecastInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartForecastInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartForecastInformationDto> GetPartForecast(Guid partForecastId)
	{
		ERPPartForecastInformationDto eRPPartForecastInformationDto = new ERPPartForecastInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"inpAnnualQuantity", "inpCreatedBy", "inpCreatedDate", "inpEndDate", "inpUniqueID", "inpForecastMethod", "inpForecastNumberOfYears", "inpIntervalType", "inpPartForecastYearID", "inpPartID",
			"inpPartRevisionID", "inpRowVersion", "inpStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("inpUniqueID|C", partForecastId);
		AddCustomFieldsToSelectList("PartForecasts");
		using (DataTable dataTable = GetAsDataTable("PartForecasts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartForecastInformationDto);
			}
			eRPPartForecastInformationDto.inpAnnualQuantity = dataTable.Rows[0].Field<decimal>("inpAnnualQuantity");
			eRPPartForecastInformationDto.inpCreatedBy = dataTable.Rows[0].Field<string>("inpCreatedBy");
			eRPPartForecastInformationDto.inpCreatedDate = dataTable.Rows[0].Field<DateTime?>("inpCreatedDate");
			eRPPartForecastInformationDto.inpEndDate = dataTable.Rows[0].Field<DateTime?>("inpEndDate");
			eRPPartForecastInformationDto.inpUniqueID = dataTable.Rows[0].Field<Guid>("inpUniqueID");
			eRPPartForecastInformationDto.inpForecastMethod = dataTable.Rows[0].Field<string>("inpForecastMethod");
			eRPPartForecastInformationDto.inpForecastNumberOfYears = dataTable.Rows[0].Field<byte>("inpForecastNumberOfYears");
			eRPPartForecastInformationDto.inpIntervalType = dataTable.Rows[0].Field<string>("inpIntervalType");
			eRPPartForecastInformationDto.inpPartForecastYearID = dataTable.Rows[0].Field<short>("inpPartForecastYearID");
			eRPPartForecastInformationDto.inpPartID = dataTable.Rows[0].Field<string>("inpPartID");
			eRPPartForecastInformationDto.inpPartRevisionID = dataTable.Rows[0].Field<string>("inpPartRevisionID");
			eRPPartForecastInformationDto.inpRowVersion = dataTable.Rows[0].Field<byte[]>("inpRowVersion");
			eRPPartForecastInformationDto.inpStartDate = dataTable.Rows[0].Field<DateTime?>("inpStartDate");
			eRPPartForecastInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartForecastInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartForecastInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartForecast(ERPPartForecastDto partForecast)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartForecasts WHERE inpUniqueID = " + M1Util.ConvertToLinq(partForecast.inpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["inpPartID"] = partForecast.inpPartID.ToUpper();
				dataRow["inpPartRevisionID"] = partForecast.inpPartRevisionID.ToUpper();
				dataRow["inpPartForecastYearID"] = partForecast.inpPartForecastYearID;
				partForecast.inpUniqueID = ((partForecast.inpUniqueID == Guid.Empty) ? Guid.NewGuid() : partForecast.inpUniqueID);
				dataRow["inpUniqueID"] = partForecast.inpUniqueID;
				dataRow["inpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["inpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartForecast could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partForecast.inpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartForecast is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["inpRowVersion"], partForecast.inpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartForecast has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartForecast again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["inpAnnualQuantity"] = partForecast.inpAnnualQuantity;
			DataRow dataRow2 = dataRow;
			DateTime? inpEndDate = partForecast.inpEndDate;
			dataRow2["inpEndDate"] = (inpEndDate.HasValue ? ((object)inpEndDate.GetValueOrDefault()) : dataRow["inpEndDate"]);
			dataRow["inpForecastMethod"] = partForecast.inpForecastMethod;
			dataRow["inpForecastNumberOfYears"] = partForecast.inpForecastNumberOfYears;
			dataRow["inpIntervalType"] = partForecast.inpIntervalType;
			DataRow dataRow3 = dataRow;
			inpEndDate = partForecast.inpStartDate;
			dataRow3["inpStartDate"] = (inpEndDate.HasValue ? ((object)inpEndDate.GetValueOrDefault()) : dataRow["inpStartDate"]);
			if (partForecast.CustomFields != null && partForecast.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partForecast.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartForecast [{partForecast.inpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartForecast [{partForecast.inpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
