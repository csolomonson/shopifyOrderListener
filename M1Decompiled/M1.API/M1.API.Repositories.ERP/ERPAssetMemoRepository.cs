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

public class ERPAssetMemoRepository : APIBaseRepository, IERPAssetMemoRepository, IAPIBaseRepository, IDisposable
{
	public ERPAssetMemoRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesAssetMemoExist(Guid assetMemoId)
	{
		InitializeParameterLists();
		base.filterList.Add("fakUniqueID|C", assetMemoId);
		base.selectList.Add("fakUniqueID");
		return Task.FromResult(GetAsObject("AssetMemos", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPAssetMemoInformationDto>> GetAllAssetMemos(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPAssetMemoInformationDto> collection = new List<ERPAssetMemoInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "fakAssetID", "fakCreatedBy", "fakCreatedDate", "fakUniqueID", "fakLongDescriptionRtf", "fakLongDescriptionText", "fakMemoDate", "fakRowVersion", "fakAssetMemoID", "fakShortDescription" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("AssetMemos");
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
		using (DataTable dataTable = GetAsDataTable("AssetMemos", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPAssetMemoInformationDto eRPAssetMemoInformationDto = new ERPAssetMemoInformationDto();
				eRPAssetMemoInformationDto.fakAssetID = dataTable.Rows[i].Field<string>("fakAssetID");
				eRPAssetMemoInformationDto.fakCreatedBy = dataTable.Rows[i].Field<string>("fakCreatedBy");
				eRPAssetMemoInformationDto.fakCreatedDate = dataTable.Rows[i].Field<DateTime?>("fakCreatedDate");
				eRPAssetMemoInformationDto.fakUniqueID = dataTable.Rows[i].Field<Guid>("fakUniqueID");
				eRPAssetMemoInformationDto.fakLongDescriptionRtf = dataTable.Rows[i].Field<string>("fakLongDescriptionRtf");
				eRPAssetMemoInformationDto.fakLongDescriptionText = dataTable.Rows[i].Field<string>("fakLongDescriptionText");
				eRPAssetMemoInformationDto.fakMemoDate = dataTable.Rows[i].Field<DateTime?>("fakMemoDate");
				eRPAssetMemoInformationDto.fakRowVersion = dataTable.Rows[i].Field<byte[]>("fakRowVersion");
				eRPAssetMemoInformationDto.fakAssetMemoID = dataTable.Rows[i].Field<short>("fakAssetMemoID");
				eRPAssetMemoInformationDto.fakShortDescription = dataTable.Rows[i].Field<string>("fakShortDescription");
				eRPAssetMemoInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPAssetMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPAssetMemoInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPAssetMemoInformationDto> GetAssetMemo(Guid assetMemoId)
	{
		ERPAssetMemoInformationDto eRPAssetMemoInformationDto = new ERPAssetMemoInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "fakAssetID", "fakCreatedBy", "fakCreatedDate", "fakUniqueID", "fakLongDescriptionRtf", "fakLongDescriptionText", "fakMemoDate", "fakRowVersion", "fakAssetMemoID", "fakShortDescription" };
		base.selectList.AddRange(collection);
		base.filterList.Add("fakUniqueID|C", assetMemoId);
		AddCustomFieldsToSelectList("AssetMemos");
		using (DataTable dataTable = GetAsDataTable("AssetMemos", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPAssetMemoInformationDto);
			}
			eRPAssetMemoInformationDto.fakAssetID = dataTable.Rows[0].Field<string>("fakAssetID");
			eRPAssetMemoInformationDto.fakCreatedBy = dataTable.Rows[0].Field<string>("fakCreatedBy");
			eRPAssetMemoInformationDto.fakCreatedDate = dataTable.Rows[0].Field<DateTime?>("fakCreatedDate");
			eRPAssetMemoInformationDto.fakUniqueID = dataTable.Rows[0].Field<Guid>("fakUniqueID");
			eRPAssetMemoInformationDto.fakLongDescriptionRtf = dataTable.Rows[0].Field<string>("fakLongDescriptionRtf");
			eRPAssetMemoInformationDto.fakLongDescriptionText = dataTable.Rows[0].Field<string>("fakLongDescriptionText");
			eRPAssetMemoInformationDto.fakMemoDate = dataTable.Rows[0].Field<DateTime?>("fakMemoDate");
			eRPAssetMemoInformationDto.fakRowVersion = dataTable.Rows[0].Field<byte[]>("fakRowVersion");
			eRPAssetMemoInformationDto.fakAssetMemoID = dataTable.Rows[0].Field<short>("fakAssetMemoID");
			eRPAssetMemoInformationDto.fakShortDescription = dataTable.Rows[0].Field<string>("fakShortDescription");
			eRPAssetMemoInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPAssetMemoInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPAssetMemoInformationDto);
	}

	public Task<APIValidationInfoDto> SaveAssetMemo(ERPAssetMemoDto assetMemo)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM AssetMemos WHERE fakUniqueID = " + M1Util.ConvertToLinq(assetMemo.fakUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["fakAssetID"] = assetMemo.fakAssetID.ToUpper();
				dataRow["fakAssetMemoID"] = assetMemo.fakAssetMemoID;
				assetMemo.fakUniqueID = ((assetMemo.fakUniqueID == Guid.Empty) ? Guid.NewGuid() : assetMemo.fakUniqueID);
				dataRow["fakUniqueID"] = assetMemo.fakUniqueID;
				dataRow["fakCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["fakCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The AssetMemo could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (assetMemo.fakRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the AssetMemo is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["fakRowVersion"], assetMemo.fakRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the AssetMemo has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the AssetMemo again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["fakLongDescriptionRtf"] = assetMemo.fakLongDescriptionRtf ?? dataRow["fakLongDescriptionRtf"];
			dataRow["fakLongDescriptionText"] = assetMemo.fakLongDescriptionText ?? dataRow["fakLongDescriptionText"];
			DataRow dataRow2 = dataRow;
			DateTime? fakMemoDate = assetMemo.fakMemoDate;
			dataRow2["fakMemoDate"] = (fakMemoDate.HasValue ? ((object)fakMemoDate.GetValueOrDefault()) : dataRow["fakMemoDate"]);
			dataRow["fakShortDescription"] = assetMemo.fakShortDescription;
			if (assetMemo.CustomFields != null && assetMemo.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in assetMemo.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the AssetMemo [{assetMemo.fakUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the AssetMemo [{assetMemo.fakUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
