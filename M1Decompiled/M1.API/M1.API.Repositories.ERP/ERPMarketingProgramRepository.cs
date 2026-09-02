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

public class ERPMarketingProgramRepository : APIBaseRepository, IERPMarketingProgramRepository, IAPIBaseRepository, IDisposable
{
	public ERPMarketingProgramRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesMarketingProgramExist(Guid marketingProgramId)
	{
		InitializeParameterLists();
		base.filterList.Add("looUniqueID|C", marketingProgramId);
		base.selectList.Add("looUniqueID");
		return Task.FromResult(GetAsObject("MarketingPrograms", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPMarketingProgramInformationDto>> GetAllMarketingPrograms(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPMarketingProgramInformationDto> collection = new List<ERPMarketingProgramInformationDto>();
		InitializeParameterLists();
		string[] array = new string[15]
		{
			"looActivityType", "looMarketingProgramID", "looCreatedBy", "looCreatedDate", "looEndDate", "looUniqueID", "looExpectedRevenue", "looInactiveDate", "looInactive", "looLongDescriptionRtf",
			"looLongDescriptionText", "looMarketingCost", "looRowVersion", "looShortDescription", "looStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("MarketingPrograms");
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
		using (DataTable dataTable = GetAsDataTable("MarketingPrograms", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPMarketingProgramInformationDto eRPMarketingProgramInformationDto = new ERPMarketingProgramInformationDto();
				eRPMarketingProgramInformationDto.looActivityType = dataTable.Rows[i].Field<string>("looActivityType");
				eRPMarketingProgramInformationDto.looMarketingProgramID = dataTable.Rows[i].Field<string>("looMarketingProgramID");
				eRPMarketingProgramInformationDto.looCreatedBy = dataTable.Rows[i].Field<string>("looCreatedBy");
				eRPMarketingProgramInformationDto.looCreatedDate = dataTable.Rows[i].Field<DateTime?>("looCreatedDate");
				eRPMarketingProgramInformationDto.looEndDate = dataTable.Rows[i].Field<DateTime?>("looEndDate");
				eRPMarketingProgramInformationDto.looUniqueID = dataTable.Rows[i].Field<Guid>("looUniqueID");
				eRPMarketingProgramInformationDto.looExpectedRevenue = dataTable.Rows[i].Field<decimal>("looExpectedRevenue");
				eRPMarketingProgramInformationDto.looInactiveDate = dataTable.Rows[i].Field<DateTime?>("looInactiveDate");
				eRPMarketingProgramInformationDto.looInactive = dataTable.Rows[i].Field<bool>("looInactive");
				eRPMarketingProgramInformationDto.looLongDescriptionRtf = dataTable.Rows[i].Field<string>("looLongDescriptionRtf");
				eRPMarketingProgramInformationDto.looLongDescriptionText = dataTable.Rows[i].Field<string>("looLongDescriptionText");
				eRPMarketingProgramInformationDto.looMarketingCost = dataTable.Rows[i].Field<decimal>("looMarketingCost");
				eRPMarketingProgramInformationDto.looRowVersion = dataTable.Rows[i].Field<byte[]>("looRowVersion");
				eRPMarketingProgramInformationDto.looShortDescription = dataTable.Rows[i].Field<string>("looShortDescription");
				eRPMarketingProgramInformationDto.looStartDate = dataTable.Rows[i].Field<DateTime?>("looStartDate");
				eRPMarketingProgramInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPMarketingProgramInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPMarketingProgramInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPMarketingProgramInformationDto> GetMarketingProgram(Guid marketingProgramId)
	{
		ERPMarketingProgramInformationDto eRPMarketingProgramInformationDto = new ERPMarketingProgramInformationDto();
		InitializeParameterLists();
		string[] collection = new string[15]
		{
			"looActivityType", "looMarketingProgramID", "looCreatedBy", "looCreatedDate", "looEndDate", "looUniqueID", "looExpectedRevenue", "looInactiveDate", "looInactive", "looLongDescriptionRtf",
			"looLongDescriptionText", "looMarketingCost", "looRowVersion", "looShortDescription", "looStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("looUniqueID|C", marketingProgramId);
		AddCustomFieldsToSelectList("MarketingPrograms");
		using (DataTable dataTable = GetAsDataTable("MarketingPrograms", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPMarketingProgramInformationDto);
			}
			eRPMarketingProgramInformationDto.looActivityType = dataTable.Rows[0].Field<string>("looActivityType");
			eRPMarketingProgramInformationDto.looMarketingProgramID = dataTable.Rows[0].Field<string>("looMarketingProgramID");
			eRPMarketingProgramInformationDto.looCreatedBy = dataTable.Rows[0].Field<string>("looCreatedBy");
			eRPMarketingProgramInformationDto.looCreatedDate = dataTable.Rows[0].Field<DateTime?>("looCreatedDate");
			eRPMarketingProgramInformationDto.looEndDate = dataTable.Rows[0].Field<DateTime?>("looEndDate");
			eRPMarketingProgramInformationDto.looUniqueID = dataTable.Rows[0].Field<Guid>("looUniqueID");
			eRPMarketingProgramInformationDto.looExpectedRevenue = dataTable.Rows[0].Field<decimal>("looExpectedRevenue");
			eRPMarketingProgramInformationDto.looInactiveDate = dataTable.Rows[0].Field<DateTime?>("looInactiveDate");
			eRPMarketingProgramInformationDto.looInactive = dataTable.Rows[0].Field<bool>("looInactive");
			eRPMarketingProgramInformationDto.looLongDescriptionRtf = dataTable.Rows[0].Field<string>("looLongDescriptionRtf");
			eRPMarketingProgramInformationDto.looLongDescriptionText = dataTable.Rows[0].Field<string>("looLongDescriptionText");
			eRPMarketingProgramInformationDto.looMarketingCost = dataTable.Rows[0].Field<decimal>("looMarketingCost");
			eRPMarketingProgramInformationDto.looRowVersion = dataTable.Rows[0].Field<byte[]>("looRowVersion");
			eRPMarketingProgramInformationDto.looShortDescription = dataTable.Rows[0].Field<string>("looShortDescription");
			eRPMarketingProgramInformationDto.looStartDate = dataTable.Rows[0].Field<DateTime?>("looStartDate");
			eRPMarketingProgramInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPMarketingProgramInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPMarketingProgramInformationDto);
	}

	public Task<APIValidationInfoDto> SaveMarketingProgram(ERPMarketingProgramDto marketingProgram)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM MarketingPrograms WHERE looUniqueID = " + M1Util.ConvertToLinq(marketingProgram.looUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["looMarketingProgramID"] = marketingProgram.looMarketingProgramID.ToUpper();
				marketingProgram.looUniqueID = ((marketingProgram.looUniqueID == Guid.Empty) ? Guid.NewGuid() : marketingProgram.looUniqueID);
				dataRow["looUniqueID"] = marketingProgram.looUniqueID;
				dataRow["looCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["looCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The MarketingProgram could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (marketingProgram.looRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the MarketingProgram is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["looRowVersion"], marketingProgram.looRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the MarketingProgram has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the MarketingProgram again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["looActivityType"] = marketingProgram.looActivityType;
			DataRow dataRow2 = dataRow;
			DateTime? looEndDate = marketingProgram.looEndDate;
			dataRow2["looEndDate"] = (looEndDate.HasValue ? ((object)looEndDate.GetValueOrDefault()) : dataRow["looEndDate"]);
			dataRow["looExpectedRevenue"] = marketingProgram.looExpectedRevenue;
			DataRow dataRow3 = dataRow;
			looEndDate = marketingProgram.looInactiveDate;
			dataRow3["looInactiveDate"] = (looEndDate.HasValue ? ((object)looEndDate.GetValueOrDefault()) : dataRow["looInactiveDate"]);
			dataRow["looInactive"] = marketingProgram.looInactive;
			dataRow["looLongDescriptionRtf"] = marketingProgram.looLongDescriptionRtf ?? dataRow["looLongDescriptionRtf"];
			dataRow["looLongDescriptionText"] = marketingProgram.looLongDescriptionText ?? dataRow["looLongDescriptionText"];
			dataRow["looMarketingCost"] = marketingProgram.looMarketingCost;
			dataRow["looShortDescription"] = marketingProgram.looShortDescription;
			DataRow dataRow4 = dataRow;
			looEndDate = marketingProgram.looStartDate;
			dataRow4["looStartDate"] = (looEndDate.HasValue ? ((object)looEndDate.GetValueOrDefault()) : dataRow["looStartDate"]);
			if (marketingProgram.CustomFields != null && marketingProgram.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in marketingProgram.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the MarketingProgram [{marketingProgram.looUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the MarketingProgram [{marketingProgram.looUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
