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

public class ERPPartUnitSalePriceRepository : APIBaseRepository, IERPPartUnitSalePriceRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartUnitSalePriceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartUnitSalePriceExist(Guid partUnitSalePriceId)
	{
		InitializeParameterLists();
		base.filterList.Add("imhUniqueID|C", partUnitSalePriceId);
		base.selectList.Add("imhUniqueID");
		return Task.FromResult(GetAsObject("PartUnitSalePrices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartUnitSalePriceInformationDto>> GetAllPartUnitSalePrices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartUnitSalePriceInformationDto> collection = new List<ERPPartUnitSalePriceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"imhCreatedBy", "imhCreatedDate", "imhCurrencyRateID", "imhEndDate", "imhUniqueID", "imhPartID", "imhPartRevisionID", "imhRowVersion", "imhPartUnitSalePriceID", "imhStartDate",
			"imhUnitSalePrice"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartUnitSalePrices");
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
		using (DataTable dataTable = GetAsDataTable("PartUnitSalePrices", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartUnitSalePriceInformationDto eRPPartUnitSalePriceInformationDto = new ERPPartUnitSalePriceInformationDto();
				eRPPartUnitSalePriceInformationDto.imhCreatedBy = dataTable.Rows[i].Field<string>("imhCreatedBy");
				eRPPartUnitSalePriceInformationDto.imhCreatedDate = dataTable.Rows[i].Field<DateTime?>("imhCreatedDate");
				eRPPartUnitSalePriceInformationDto.imhCurrencyRateID = dataTable.Rows[i].Field<string>("imhCurrencyRateID");
				eRPPartUnitSalePriceInformationDto.imhEndDate = dataTable.Rows[i].Field<DateTime?>("imhEndDate");
				eRPPartUnitSalePriceInformationDto.imhUniqueID = dataTable.Rows[i].Field<Guid>("imhUniqueID");
				eRPPartUnitSalePriceInformationDto.imhPartID = dataTable.Rows[i].Field<string>("imhPartID");
				eRPPartUnitSalePriceInformationDto.imhPartRevisionID = dataTable.Rows[i].Field<string>("imhPartRevisionID");
				eRPPartUnitSalePriceInformationDto.imhRowVersion = dataTable.Rows[i].Field<byte[]>("imhRowVersion");
				eRPPartUnitSalePriceInformationDto.imhPartUnitSalePriceID = dataTable.Rows[i].Field<short>("imhPartUnitSalePriceID");
				eRPPartUnitSalePriceInformationDto.imhStartDate = dataTable.Rows[i].Field<DateTime?>("imhStartDate");
				eRPPartUnitSalePriceInformationDto.imhUnitSalePrice = dataTable.Rows[i].Field<decimal>("imhUnitSalePrice");
				eRPPartUnitSalePriceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartUnitSalePriceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartUnitSalePriceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartUnitSalePriceInformationDto> GetPartUnitSalePrice(Guid partUnitSalePriceId)
	{
		ERPPartUnitSalePriceInformationDto eRPPartUnitSalePriceInformationDto = new ERPPartUnitSalePriceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"imhCreatedBy", "imhCreatedDate", "imhCurrencyRateID", "imhEndDate", "imhUniqueID", "imhPartID", "imhPartRevisionID", "imhRowVersion", "imhPartUnitSalePriceID", "imhStartDate",
			"imhUnitSalePrice"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imhUniqueID|C", partUnitSalePriceId);
		AddCustomFieldsToSelectList("PartUnitSalePrices");
		using (DataTable dataTable = GetAsDataTable("PartUnitSalePrices", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartUnitSalePriceInformationDto);
			}
			eRPPartUnitSalePriceInformationDto.imhCreatedBy = dataTable.Rows[0].Field<string>("imhCreatedBy");
			eRPPartUnitSalePriceInformationDto.imhCreatedDate = dataTable.Rows[0].Field<DateTime?>("imhCreatedDate");
			eRPPartUnitSalePriceInformationDto.imhCurrencyRateID = dataTable.Rows[0].Field<string>("imhCurrencyRateID");
			eRPPartUnitSalePriceInformationDto.imhEndDate = dataTable.Rows[0].Field<DateTime?>("imhEndDate");
			eRPPartUnitSalePriceInformationDto.imhUniqueID = dataTable.Rows[0].Field<Guid>("imhUniqueID");
			eRPPartUnitSalePriceInformationDto.imhPartID = dataTable.Rows[0].Field<string>("imhPartID");
			eRPPartUnitSalePriceInformationDto.imhPartRevisionID = dataTable.Rows[0].Field<string>("imhPartRevisionID");
			eRPPartUnitSalePriceInformationDto.imhRowVersion = dataTable.Rows[0].Field<byte[]>("imhRowVersion");
			eRPPartUnitSalePriceInformationDto.imhPartUnitSalePriceID = dataTable.Rows[0].Field<short>("imhPartUnitSalePriceID");
			eRPPartUnitSalePriceInformationDto.imhStartDate = dataTable.Rows[0].Field<DateTime?>("imhStartDate");
			eRPPartUnitSalePriceInformationDto.imhUnitSalePrice = dataTable.Rows[0].Field<decimal>("imhUnitSalePrice");
			eRPPartUnitSalePriceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartUnitSalePriceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartUnitSalePriceInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartUnitSalePrice(ERPPartUnitSalePriceDto partUnitSalePrice)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartUnitSalePrices WHERE imhUniqueID = " + M1Util.ConvertToLinq(partUnitSalePrice.imhUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imhPartID"] = partUnitSalePrice.imhPartID.ToUpper();
				dataRow["imhPartRevisionID"] = partUnitSalePrice.imhPartRevisionID.ToUpper();
				dataRow["imhPartUnitSalePriceID"] = partUnitSalePrice.imhPartUnitSalePriceID;
				partUnitSalePrice.imhUniqueID = ((partUnitSalePrice.imhUniqueID == Guid.Empty) ? Guid.NewGuid() : partUnitSalePrice.imhUniqueID);
				dataRow["imhUniqueID"] = partUnitSalePrice.imhUniqueID;
				dataRow["imhCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imhCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartUnitSalePrice could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partUnitSalePrice.imhRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartUnitSalePrice is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imhRowVersion"], partUnitSalePrice.imhRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartUnitSalePrice has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartUnitSalePrice again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imhCurrencyRateID"] = partUnitSalePrice.imhCurrencyRateID;
			DataRow dataRow2 = dataRow;
			DateTime? imhEndDate = partUnitSalePrice.imhEndDate;
			dataRow2["imhEndDate"] = (imhEndDate.HasValue ? ((object)imhEndDate.GetValueOrDefault()) : dataRow["imhEndDate"]);
			DataRow dataRow3 = dataRow;
			imhEndDate = partUnitSalePrice.imhStartDate;
			dataRow3["imhStartDate"] = (imhEndDate.HasValue ? ((object)imhEndDate.GetValueOrDefault()) : dataRow["imhStartDate"]);
			dataRow["imhUnitSalePrice"] = partUnitSalePrice.imhUnitSalePrice;
			if (partUnitSalePrice.CustomFields != null && partUnitSalePrice.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partUnitSalePrice.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartUnitSalePrice [{partUnitSalePrice.imhUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartUnitSalePrice [{partUnitSalePrice.imhUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
