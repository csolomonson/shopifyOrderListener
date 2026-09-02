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

public class ERPPartAlternateRepository : APIBaseRepository, IERPPartAlternateRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartAlternateRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartAlternateExist(Guid partAlternateId)
	{
		InitializeParameterLists();
		base.filterList.Add("imeUniqueID|C", partAlternateId);
		base.selectList.Add("imeUniqueID");
		return Task.FromResult(GetAsObject("PartAlternates", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartAlternateInformationDto>> GetAllPartAlternates(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartAlternateInformationDto> collection = new List<ERPPartAlternateInformationDto>();
		InitializeParameterLists();
		string[] array = new string[9] { "imeAlternatePartID", "imeAlternatePartRevisionID", "imeComment", "imeCreatedBy", "imeCreatedDate", "imeUniqueID", "imePartID", "imePartRevisionID", "imeRowVersion" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartAlternates");
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
		using (DataTable dataTable = GetAsDataTable("PartAlternates", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartAlternateInformationDto eRPPartAlternateInformationDto = new ERPPartAlternateInformationDto();
				eRPPartAlternateInformationDto.imeAlternatePartID = dataTable.Rows[i].Field<string>("imeAlternatePartID");
				eRPPartAlternateInformationDto.imeAlternatePartRevisionID = dataTable.Rows[i].Field<string>("imeAlternatePartRevisionID");
				eRPPartAlternateInformationDto.imeComment = dataTable.Rows[i].Field<string>("imeComment");
				eRPPartAlternateInformationDto.imeCreatedBy = dataTable.Rows[i].Field<string>("imeCreatedBy");
				eRPPartAlternateInformationDto.imeCreatedDate = dataTable.Rows[i].Field<DateTime?>("imeCreatedDate");
				eRPPartAlternateInformationDto.imeUniqueID = dataTable.Rows[i].Field<Guid>("imeUniqueID");
				eRPPartAlternateInformationDto.imePartID = dataTable.Rows[i].Field<string>("imePartID");
				eRPPartAlternateInformationDto.imePartRevisionID = dataTable.Rows[i].Field<string>("imePartRevisionID");
				eRPPartAlternateInformationDto.imeRowVersion = dataTable.Rows[i].Field<byte[]>("imeRowVersion");
				eRPPartAlternateInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartAlternateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartAlternateInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartAlternateInformationDto> GetPartAlternate(Guid partAlternateId)
	{
		ERPPartAlternateInformationDto eRPPartAlternateInformationDto = new ERPPartAlternateInformationDto();
		InitializeParameterLists();
		string[] collection = new string[9] { "imeAlternatePartID", "imeAlternatePartRevisionID", "imeComment", "imeCreatedBy", "imeCreatedDate", "imeUniqueID", "imePartID", "imePartRevisionID", "imeRowVersion" };
		base.selectList.AddRange(collection);
		base.filterList.Add("imeUniqueID|C", partAlternateId);
		AddCustomFieldsToSelectList("PartAlternates");
		using (DataTable dataTable = GetAsDataTable("PartAlternates", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartAlternateInformationDto);
			}
			eRPPartAlternateInformationDto.imeAlternatePartID = dataTable.Rows[0].Field<string>("imeAlternatePartID");
			eRPPartAlternateInformationDto.imeAlternatePartRevisionID = dataTable.Rows[0].Field<string>("imeAlternatePartRevisionID");
			eRPPartAlternateInformationDto.imeComment = dataTable.Rows[0].Field<string>("imeComment");
			eRPPartAlternateInformationDto.imeCreatedBy = dataTable.Rows[0].Field<string>("imeCreatedBy");
			eRPPartAlternateInformationDto.imeCreatedDate = dataTable.Rows[0].Field<DateTime?>("imeCreatedDate");
			eRPPartAlternateInformationDto.imeUniqueID = dataTable.Rows[0].Field<Guid>("imeUniqueID");
			eRPPartAlternateInformationDto.imePartID = dataTable.Rows[0].Field<string>("imePartID");
			eRPPartAlternateInformationDto.imePartRevisionID = dataTable.Rows[0].Field<string>("imePartRevisionID");
			eRPPartAlternateInformationDto.imeRowVersion = dataTable.Rows[0].Field<byte[]>("imeRowVersion");
			eRPPartAlternateInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartAlternateInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartAlternateInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartAlternate(ERPPartAlternateDto partAlternate)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartAlternates WHERE imeUniqueID = " + M1Util.ConvertToLinq(partAlternate.imeUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imePartID"] = partAlternate.imePartID.ToUpper();
				dataRow["imePartRevisionID"] = partAlternate.imePartRevisionID.ToUpper();
				dataRow["imeAlternatePartID"] = partAlternate.imeAlternatePartID.ToUpper();
				dataRow["imeAlternatePartRevisionID"] = partAlternate.imeAlternatePartRevisionID.ToUpper();
				partAlternate.imeUniqueID = ((partAlternate.imeUniqueID == Guid.Empty) ? Guid.NewGuid() : partAlternate.imeUniqueID);
				dataRow["imeUniqueID"] = partAlternate.imeUniqueID;
				dataRow["imeCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imeCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartAlternate could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partAlternate.imeRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartAlternate is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imeRowVersion"], partAlternate.imeRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartAlternate has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartAlternate again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imeComment"] = partAlternate.imeComment;
			if (partAlternate.CustomFields != null && partAlternate.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partAlternate.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartAlternate [{partAlternate.imeUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartAlternate [{partAlternate.imeUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
