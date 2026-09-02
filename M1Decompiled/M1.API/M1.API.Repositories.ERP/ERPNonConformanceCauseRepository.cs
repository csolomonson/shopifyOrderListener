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

public class ERPNonConformanceCauseRepository : APIBaseRepository, IERPNonConformanceCauseRepository, IAPIBaseRepository, IDisposable
{
	public ERPNonConformanceCauseRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesNonConformanceCauseExist(Guid nonConformanceCauseId)
	{
		InitializeParameterLists();
		base.filterList.Add("qauUniqueID|C", nonConformanceCauseId);
		base.selectList.Add("qauUniqueID");
		return Task.FromResult(GetAsObject("NonConformanceCauses", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPNonConformanceCauseInformationDto>> GetAllNonConformanceCauses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPNonConformanceCauseInformationDto> collection = new List<ERPNonConformanceCauseInformationDto>();
		InitializeParameterLists();
		string[] array = new string[6] { "qauNonConformanceCauseID", "qauCreatedBy", "qauCreatedDate", "qauDescription", "qauUniqueID", "qauRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("NonConformanceCauses");
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
		using (DataTable dataTable = GetAsDataTable("NonConformanceCauses", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPNonConformanceCauseInformationDto eRPNonConformanceCauseInformationDto = new ERPNonConformanceCauseInformationDto();
				eRPNonConformanceCauseInformationDto.qauNonConformanceCauseID = dataTable.Rows[i].Field<string>("qauNonConformanceCauseID");
				eRPNonConformanceCauseInformationDto.qauCreatedBy = dataTable.Rows[i].Field<string>("qauCreatedBy");
				eRPNonConformanceCauseInformationDto.qauCreatedDate = dataTable.Rows[i].Field<DateTime?>("qauCreatedDate");
				eRPNonConformanceCauseInformationDto.qauDescription = dataTable.Rows[i].Field<string>("qauDescription");
				eRPNonConformanceCauseInformationDto.qauUniqueID = dataTable.Rows[i].Field<Guid>("qauUniqueID");
				eRPNonConformanceCauseInformationDto.qauRowVersion = dataTable.Rows[i].Field<byte[]>("qauRowVersion");
				eRPNonConformanceCauseInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPNonConformanceCauseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPNonConformanceCauseInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPNonConformanceCauseInformationDto> GetNonConformanceCause(Guid nonConformanceCauseId)
	{
		ERPNonConformanceCauseInformationDto eRPNonConformanceCauseInformationDto = new ERPNonConformanceCauseInformationDto();
		InitializeParameterLists();
		string[] collection = new string[6] { "qauNonConformanceCauseID", "qauCreatedBy", "qauCreatedDate", "qauDescription", "qauUniqueID", "qauRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qauUniqueID|C", nonConformanceCauseId);
		AddCustomFieldsToSelectList("NonConformanceCauses");
		using (DataTable dataTable = GetAsDataTable("NonConformanceCauses", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPNonConformanceCauseInformationDto);
			}
			eRPNonConformanceCauseInformationDto.qauNonConformanceCauseID = dataTable.Rows[0].Field<string>("qauNonConformanceCauseID");
			eRPNonConformanceCauseInformationDto.qauCreatedBy = dataTable.Rows[0].Field<string>("qauCreatedBy");
			eRPNonConformanceCauseInformationDto.qauCreatedDate = dataTable.Rows[0].Field<DateTime?>("qauCreatedDate");
			eRPNonConformanceCauseInformationDto.qauDescription = dataTable.Rows[0].Field<string>("qauDescription");
			eRPNonConformanceCauseInformationDto.qauUniqueID = dataTable.Rows[0].Field<Guid>("qauUniqueID");
			eRPNonConformanceCauseInformationDto.qauRowVersion = dataTable.Rows[0].Field<byte[]>("qauRowVersion");
			eRPNonConformanceCauseInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPNonConformanceCauseInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPNonConformanceCauseInformationDto);
	}

	public Task<APIValidationInfoDto> SaveNonConformanceCause(ERPNonConformanceCauseDto nonConformanceCause)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM NonConformanceCauses WHERE qauUniqueID = " + M1Util.ConvertToLinq(nonConformanceCause.qauUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qauNonConformanceCauseID"] = nonConformanceCause.qauNonConformanceCauseID.ToUpper();
				nonConformanceCause.qauUniqueID = ((nonConformanceCause.qauUniqueID == Guid.Empty) ? Guid.NewGuid() : nonConformanceCause.qauUniqueID);
				dataRow["qauUniqueID"] = nonConformanceCause.qauUniqueID;
				dataRow["qauCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qauCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The NonConformanceCause could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (nonConformanceCause.qauRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the NonConformanceCause is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qauRowVersion"], nonConformanceCause.qauRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the NonConformanceCause has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the NonConformanceCause again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qauDescription"] = nonConformanceCause.qauDescription;
			if (nonConformanceCause.CustomFields != null && nonConformanceCause.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in nonConformanceCause.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the NonConformanceCause [{nonConformanceCause.qauUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the NonConformanceCause [{nonConformanceCause.qauUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
