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

public class ERPProductionDepartmentRepository : APIBaseRepository, IERPProductionDepartmentRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductionDepartmentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductionDepartmentExist(Guid productionDepartmentId)
	{
		InitializeParameterLists();
		base.filterList.Add("xaeUniqueID|C", productionDepartmentId);
		base.selectList.Add("xaeUniqueID");
		return Task.FromResult(GetAsObject("ProductionDepartments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductionDepartmentInformationDto>> GetAllProductionDepartments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductionDepartmentInformationDto> collection = new List<ERPProductionDepartmentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "xaeProductionDepartmentID", "xaeCreatedBy", "xaeCreatedDate", "xaeDescription", "xaeUniqueID", "xaeRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductionDepartments");
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
		using (DataTable dataTable = GetAsDataTable("ProductionDepartments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductionDepartmentInformationDto eRPProductionDepartmentInformationDto = new ERPProductionDepartmentInformationDto();
				eRPProductionDepartmentInformationDto.xaeProductionDepartmentID = dataTable.Rows[i].Field<string>("xaeProductionDepartmentID");
				eRPProductionDepartmentInformationDto.xaeCreatedBy = dataTable.Rows[i].Field<string>("xaeCreatedBy");
				eRPProductionDepartmentInformationDto.xaeCreatedDate = dataTable.Rows[i].Field<DateTime?>("xaeCreatedDate");
				eRPProductionDepartmentInformationDto.xaeDescription = dataTable.Rows[i].Field<string>("xaeDescription");
				eRPProductionDepartmentInformationDto.xaeUniqueID = dataTable.Rows[i].Field<Guid>("xaeUniqueID");
				eRPProductionDepartmentInformationDto.xaeRowVersion = dataTable.Rows[i].Field<byte[]>("xaeRowVersion");
				eRPProductionDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductionDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductionDepartmentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductionDepartmentInformationDto> GetProductionDepartment(Guid productionDepartmentId)
	{
		ERPProductionDepartmentInformationDto eRPProductionDepartmentInformationDto = new ERPProductionDepartmentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "xaeProductionDepartmentID", "xaeCreatedBy", "xaeCreatedDate", "xaeDescription", "xaeUniqueID", "xaeRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("xaeUniqueID|C", productionDepartmentId);
		AddCustomFieldsToSelectList("ProductionDepartments");
		using (DataTable dataTable = GetAsDataTable("ProductionDepartments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductionDepartmentInformationDto);
			}
			eRPProductionDepartmentInformationDto.xaeProductionDepartmentID = dataTable.Rows[0].Field<string>("xaeProductionDepartmentID");
			eRPProductionDepartmentInformationDto.xaeCreatedBy = dataTable.Rows[0].Field<string>("xaeCreatedBy");
			eRPProductionDepartmentInformationDto.xaeCreatedDate = dataTable.Rows[0].Field<DateTime?>("xaeCreatedDate");
			eRPProductionDepartmentInformationDto.xaeDescription = dataTable.Rows[0].Field<string>("xaeDescription");
			eRPProductionDepartmentInformationDto.xaeUniqueID = dataTable.Rows[0].Field<Guid>("xaeUniqueID");
			eRPProductionDepartmentInformationDto.xaeRowVersion = dataTable.Rows[0].Field<byte[]>("xaeRowVersion");
			eRPProductionDepartmentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductionDepartmentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductionDepartmentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProductionDepartment(ERPProductionDepartmentDto productionDepartment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ProductionDepartments WHERE xaeUniqueID = " + M1Util.ConvertToLinq(productionDepartment.xaeUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xaeProductionDepartmentID"] = productionDepartment.xaeProductionDepartmentID.ToUpper();
				productionDepartment.xaeUniqueID = ((productionDepartment.xaeUniqueID == Guid.Empty) ? Guid.NewGuid() : productionDepartment.xaeUniqueID);
				dataRow["xaeUniqueID"] = productionDepartment.xaeUniqueID;
				dataRow["xaeCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xaeCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ProductionDepartment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (productionDepartment.xaeRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ProductionDepartment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xaeRowVersion"], productionDepartment.xaeRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ProductionDepartment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ProductionDepartment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xaeDescription"] = productionDepartment.xaeDescription;
			if (productionDepartment.CustomFields != null && productionDepartment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in productionDepartment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ProductionDepartment [{productionDepartment.xaeUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ProductionDepartment [{productionDepartment.xaeUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
