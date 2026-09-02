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

public class ERPNextIDRepository : APIBaseRepository, IERPNextIDRepository, IAPIBaseRepository, IDisposable
{
	public ERPNextIDRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesNextIDExist(Guid nextIDId)
	{
		InitializeParameterLists();
		base.filterList.Add("xanUniqueID|C", nextIDId);
		base.selectList.Add("xanUniqueID");
		return Task.FromResult(GetAsObject("NextIDs", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPNextIDInformationDto>> GetAllNextIDs(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPNextIDInformationDto> collection = new List<ERPNextIDInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"xanAutoIncrement", "xanCreatedBy", "xanCreatedDate", "xanDatasets", "xanUniqueID", "xanIncrementAmount", "xanLogChanges", "xanNextID", "xanNumericOnly", "xanRowVersion",
			"xanTable"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("NextIDs");
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
		using (DataTable dataTable = GetAsDataTable("NextIDs", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPNextIDInformationDto eRPNextIDInformationDto = new ERPNextIDInformationDto();
				eRPNextIDInformationDto.xanAutoIncrement = dataTable.Rows[i].Field<byte>("xanAutoIncrement");
				eRPNextIDInformationDto.xanCreatedBy = dataTable.Rows[i].Field<string>("xanCreatedBy");
				eRPNextIDInformationDto.xanCreatedDate = dataTable.Rows[i].Field<DateTime?>("xanCreatedDate");
				eRPNextIDInformationDto.xanDatasets = dataTable.Rows[i].Field<string>("xanDatasets");
				eRPNextIDInformationDto.xanUniqueID = dataTable.Rows[i].Field<Guid>("xanUniqueID");
				eRPNextIDInformationDto.xanIncrementAmount = dataTable.Rows[i].Field<short>("xanIncrementAmount");
				eRPNextIDInformationDto.xanLogChanges = dataTable.Rows[i].Field<byte>("xanLogChanges");
				eRPNextIDInformationDto.xanNextID = dataTable.Rows[i].Field<string>("xanNextID");
				eRPNextIDInformationDto.xanNumericOnly = dataTable.Rows[i].Field<byte>("xanNumericOnly");
				eRPNextIDInformationDto.xanRowVersion = dataTable.Rows[i].Field<byte[]>("xanRowVersion");
				eRPNextIDInformationDto.xanTable = dataTable.Rows[i].Field<string>("xanTable");
				eRPNextIDInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPNextIDInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPNextIDInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPNextIDInformationDto> GetNextID(Guid nextIDId)
	{
		ERPNextIDInformationDto eRPNextIDInformationDto = new ERPNextIDInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"xanAutoIncrement", "xanCreatedBy", "xanCreatedDate", "xanDatasets", "xanUniqueID", "xanIncrementAmount", "xanLogChanges", "xanNextID", "xanNumericOnly", "xanRowVersion",
			"xanTable"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xanUniqueID|C", nextIDId);
		AddCustomFieldsToSelectList("NextIDs");
		using (DataTable dataTable = GetAsDataTable("NextIDs", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPNextIDInformationDto);
			}
			eRPNextIDInformationDto.xanAutoIncrement = dataTable.Rows[0].Field<byte>("xanAutoIncrement");
			eRPNextIDInformationDto.xanCreatedBy = dataTable.Rows[0].Field<string>("xanCreatedBy");
			eRPNextIDInformationDto.xanCreatedDate = dataTable.Rows[0].Field<DateTime?>("xanCreatedDate");
			eRPNextIDInformationDto.xanDatasets = dataTable.Rows[0].Field<string>("xanDatasets");
			eRPNextIDInformationDto.xanUniqueID = dataTable.Rows[0].Field<Guid>("xanUniqueID");
			eRPNextIDInformationDto.xanIncrementAmount = dataTable.Rows[0].Field<short>("xanIncrementAmount");
			eRPNextIDInformationDto.xanLogChanges = dataTable.Rows[0].Field<byte>("xanLogChanges");
			eRPNextIDInformationDto.xanNextID = dataTable.Rows[0].Field<string>("xanNextID");
			eRPNextIDInformationDto.xanNumericOnly = dataTable.Rows[0].Field<byte>("xanNumericOnly");
			eRPNextIDInformationDto.xanRowVersion = dataTable.Rows[0].Field<byte[]>("xanRowVersion");
			eRPNextIDInformationDto.xanTable = dataTable.Rows[0].Field<string>("xanTable");
			eRPNextIDInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPNextIDInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPNextIDInformationDto);
	}

	public Task<APIValidationInfoDto> SaveNextID(ERPNextIDDto nextID)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM NextIDs WHERE xanUniqueID = " + M1Util.ConvertToLinq(nextID.xanUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xanTable"] = nextID.xanTable.ToUpper();
				nextID.xanUniqueID = ((nextID.xanUniqueID == Guid.Empty) ? Guid.NewGuid() : nextID.xanUniqueID);
				dataRow["xanUniqueID"] = nextID.xanUniqueID;
				dataRow["xanCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xanCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The NextID could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (nextID.xanRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the NextID is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xanRowVersion"], nextID.xanRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the NextID has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the NextID again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["xanAutoIncrement"] = nextID.xanAutoIncrement;
			dataRow["xanDatasets"] = nextID.xanDatasets ?? dataRow["xanDatasets"];
			dataRow["xanIncrementAmount"] = nextID.xanIncrementAmount;
			dataRow["xanLogChanges"] = nextID.xanLogChanges;
			dataRow["xanNextID"] = nextID.xanNextID;
			dataRow["xanNumericOnly"] = nextID.xanNumericOnly;
			if (nextID.CustomFields != null && nextID.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in nextID.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the NextID [{nextID.xanUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the NextID [{nextID.xanUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
