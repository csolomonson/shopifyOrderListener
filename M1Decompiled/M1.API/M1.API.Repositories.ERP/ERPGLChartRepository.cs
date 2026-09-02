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

public class ERPGLChartRepository : APIBaseRepository, IERPGLChartRepository, IAPIBaseRepository, IDisposable
{
	public ERPGLChartRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesGLChartExist(Guid gLChartId)
	{
		InitializeParameterLists();
		base.filterList.Add("glcUniqueID|C", gLChartId);
		base.selectList.Add("glcUniqueID");
		return Task.FromResult(GetAsObject("GLCharts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPGLChartInformationDto>> GetAllGLCharts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPGLChartInformationDto> collection = new List<ERPGLChartInformationDto>();
		InitializeParameterLists();
		string[] array = new string[15]
		{
			"glcAccountType", "glcCashFlowCategory", "glcGlChartID", "glcCogsAccountType", "glcCreatedBy", "glcCreatedDate", "glcDescription", "glcUniqueID", "glcGlCategoryID", "glcCashEquivalents",
			"glcParentAccount", "glcNormalBalance", "glcParentDescription", "glcParentGlChartID", "glcRowVersion"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("GLCharts");
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
		using (DataTable dataTable = GetAsDataTable("GLCharts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPGLChartInformationDto eRPGLChartInformationDto = new ERPGLChartInformationDto();
				eRPGLChartInformationDto.glcAccountType = dataTable.Rows[i].Field<byte>("glcAccountType");
				eRPGLChartInformationDto.glcCashFlowCategory = dataTable.Rows[i].Field<byte>("glcCashFlowCategory");
				eRPGLChartInformationDto.glcGlChartID = dataTable.Rows[i].Field<string>("glcGlChartID");
				eRPGLChartInformationDto.glcCogsAccountType = dataTable.Rows[i].Field<byte>("glcCogsAccountType");
				eRPGLChartInformationDto.glcCreatedBy = dataTable.Rows[i].Field<string>("glcCreatedBy");
				eRPGLChartInformationDto.glcCreatedDate = dataTable.Rows[i].Field<DateTime?>("glcCreatedDate");
				eRPGLChartInformationDto.glcDescription = dataTable.Rows[i].Field<string>("glcDescription");
				eRPGLChartInformationDto.glcUniqueID = dataTable.Rows[i].Field<Guid>("glcUniqueID");
				eRPGLChartInformationDto.glcGlCategoryID = dataTable.Rows[i].Field<string>("glcGlCategoryID");
				eRPGLChartInformationDto.glcCashEquivalents = dataTable.Rows[i].Field<bool>("glcCashEquivalents");
				eRPGLChartInformationDto.glcParentAccount = dataTable.Rows[i].Field<bool>("glcParentAccount");
				eRPGLChartInformationDto.glcNormalBalance = dataTable.Rows[i].Field<byte>("glcNormalBalance");
				eRPGLChartInformationDto.glcParentDescription = dataTable.Rows[i].Field<string>("glcParentDescription");
				eRPGLChartInformationDto.glcParentGlChartID = dataTable.Rows[i].Field<string>("glcParentGlChartID");
				eRPGLChartInformationDto.glcRowVersion = dataTable.Rows[i].Field<byte[]>("glcRowVersion");
				eRPGLChartInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPGLChartInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPGLChartInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPGLChartInformationDto> GetGLChart(Guid gLChartId)
	{
		ERPGLChartInformationDto eRPGLChartInformationDto = new ERPGLChartInformationDto();
		InitializeParameterLists();
		string[] collection = new string[15]
		{
			"glcAccountType", "glcCashFlowCategory", "glcGlChartID", "glcCogsAccountType", "glcCreatedBy", "glcCreatedDate", "glcDescription", "glcUniqueID", "glcGlCategoryID", "glcCashEquivalents",
			"glcParentAccount", "glcNormalBalance", "glcParentDescription", "glcParentGlChartID", "glcRowVersion"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("glcUniqueID|C", gLChartId);
		AddCustomFieldsToSelectList("GLCharts");
		using (DataTable dataTable = GetAsDataTable("GLCharts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPGLChartInformationDto);
			}
			eRPGLChartInformationDto.glcAccountType = dataTable.Rows[0].Field<byte>("glcAccountType");
			eRPGLChartInformationDto.glcCashFlowCategory = dataTable.Rows[0].Field<byte>("glcCashFlowCategory");
			eRPGLChartInformationDto.glcGlChartID = dataTable.Rows[0].Field<string>("glcGlChartID");
			eRPGLChartInformationDto.glcCogsAccountType = dataTable.Rows[0].Field<byte>("glcCogsAccountType");
			eRPGLChartInformationDto.glcCreatedBy = dataTable.Rows[0].Field<string>("glcCreatedBy");
			eRPGLChartInformationDto.glcCreatedDate = dataTable.Rows[0].Field<DateTime?>("glcCreatedDate");
			eRPGLChartInformationDto.glcDescription = dataTable.Rows[0].Field<string>("glcDescription");
			eRPGLChartInformationDto.glcUniqueID = dataTable.Rows[0].Field<Guid>("glcUniqueID");
			eRPGLChartInformationDto.glcGlCategoryID = dataTable.Rows[0].Field<string>("glcGlCategoryID");
			eRPGLChartInformationDto.glcCashEquivalents = dataTable.Rows[0].Field<bool>("glcCashEquivalents");
			eRPGLChartInformationDto.glcParentAccount = dataTable.Rows[0].Field<bool>("glcParentAccount");
			eRPGLChartInformationDto.glcNormalBalance = dataTable.Rows[0].Field<byte>("glcNormalBalance");
			eRPGLChartInformationDto.glcParentDescription = dataTable.Rows[0].Field<string>("glcParentDescription");
			eRPGLChartInformationDto.glcParentGlChartID = dataTable.Rows[0].Field<string>("glcParentGlChartID");
			eRPGLChartInformationDto.glcRowVersion = dataTable.Rows[0].Field<byte[]>("glcRowVersion");
			eRPGLChartInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPGLChartInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPGLChartInformationDto);
	}

	public Task<APIValidationInfoDto> SaveGLChart(ERPGLChartDto gLChart)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM GLCharts WHERE glcUniqueID = " + M1Util.ConvertToLinq(gLChart.glcUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["glcGlChartID"] = gLChart.glcGlChartID.ToUpper();
				gLChart.glcUniqueID = ((gLChart.glcUniqueID == Guid.Empty) ? Guid.NewGuid() : gLChart.glcUniqueID);
				dataRow["glcUniqueID"] = gLChart.glcUniqueID;
				dataRow["glcCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["glcCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The GLChart could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (gLChart.glcRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the GLChart is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["glcRowVersion"], gLChart.glcRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the GLChart has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the GLChart again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["glcAccountType"] = gLChart.glcAccountType;
			dataRow["glcCashFlowCategory"] = gLChart.glcCashFlowCategory;
			dataRow["glcCogsAccountType"] = gLChart.glcCogsAccountType;
			dataRow["glcDescription"] = gLChart.glcDescription;
			dataRow["glcGlCategoryID"] = gLChart.glcGlCategoryID;
			dataRow["glcCashEquivalents"] = gLChart.glcCashEquivalents;
			dataRow["glcParentAccount"] = gLChart.glcParentAccount;
			dataRow["glcNormalBalance"] = gLChart.glcNormalBalance;
			dataRow["glcParentDescription"] = gLChart.glcParentDescription;
			dataRow["glcParentGlChartID"] = gLChart.glcParentGlChartID;
			if (gLChart.CustomFields != null && gLChart.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in gLChart.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the GLChart [{gLChart.glcUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the GLChart [{gLChart.glcUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
