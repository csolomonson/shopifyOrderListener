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

public class ERPPartPriceBreakRepository : APIBaseRepository, IERPPartPriceBreakRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartPriceBreakRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartPriceBreakExist(Guid partPriceBreakId)
	{
		InitializeParameterLists();
		base.filterList.Add("imjUniqueID|C", partPriceBreakId);
		base.selectList.Add("imjUniqueID");
		return Task.FromResult(GetAsObject("PartPriceBreaks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartPriceBreakInformationDto>> GetAllPartPriceBreaks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartPriceBreakInformationDto> collection = new List<ERPPartPriceBreakInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"imjCreatedBy", "imjCreatedDate", "imjDiscount", "imjUniqueID", "imjLeadTime", "imjPartPriceID", "imjProposedNewPrice", "imjQuantity", "imjRowVersion", "imjPartPriceBreakID",
			"imjUnitPrice"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartPriceBreaks");
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
		using (DataTable dataTable = GetAsDataTable("PartPriceBreaks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartPriceBreakInformationDto eRPPartPriceBreakInformationDto = new ERPPartPriceBreakInformationDto();
				eRPPartPriceBreakInformationDto.imjCreatedBy = dataTable.Rows[i].Field<string>("imjCreatedBy");
				eRPPartPriceBreakInformationDto.imjCreatedDate = dataTable.Rows[i].Field<DateTime?>("imjCreatedDate");
				eRPPartPriceBreakInformationDto.imjDiscount = dataTable.Rows[i].Field<decimal>("imjDiscount");
				eRPPartPriceBreakInformationDto.imjUniqueID = dataTable.Rows[i].Field<Guid>("imjUniqueID");
				eRPPartPriceBreakInformationDto.imjLeadTime = dataTable.Rows[i].Field<short>("imjLeadTime");
				eRPPartPriceBreakInformationDto.imjPartPriceID = dataTable.Rows[i].Field<int>("imjPartPriceID");
				eRPPartPriceBreakInformationDto.imjProposedNewPrice = dataTable.Rows[i].Field<decimal>("imjProposedNewPrice");
				eRPPartPriceBreakInformationDto.imjQuantity = dataTable.Rows[i].Field<decimal>("imjQuantity");
				eRPPartPriceBreakInformationDto.imjRowVersion = dataTable.Rows[i].Field<byte[]>("imjRowVersion");
				eRPPartPriceBreakInformationDto.imjPartPriceBreakID = dataTable.Rows[i].Field<short>("imjPartPriceBreakID");
				eRPPartPriceBreakInformationDto.imjUnitPrice = dataTable.Rows[i].Field<decimal>("imjUnitPrice");
				eRPPartPriceBreakInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartPriceBreakInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartPriceBreakInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartPriceBreakInformationDto> GetPartPriceBreak(Guid partPriceBreakId)
	{
		ERPPartPriceBreakInformationDto eRPPartPriceBreakInformationDto = new ERPPartPriceBreakInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"imjCreatedBy", "imjCreatedDate", "imjDiscount", "imjUniqueID", "imjLeadTime", "imjPartPriceID", "imjProposedNewPrice", "imjQuantity", "imjRowVersion", "imjPartPriceBreakID",
			"imjUnitPrice"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imjUniqueID|C", partPriceBreakId);
		AddCustomFieldsToSelectList("PartPriceBreaks");
		using (DataTable dataTable = GetAsDataTable("PartPriceBreaks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartPriceBreakInformationDto);
			}
			eRPPartPriceBreakInformationDto.imjCreatedBy = dataTable.Rows[0].Field<string>("imjCreatedBy");
			eRPPartPriceBreakInformationDto.imjCreatedDate = dataTable.Rows[0].Field<DateTime?>("imjCreatedDate");
			eRPPartPriceBreakInformationDto.imjDiscount = dataTable.Rows[0].Field<decimal>("imjDiscount");
			eRPPartPriceBreakInformationDto.imjUniqueID = dataTable.Rows[0].Field<Guid>("imjUniqueID");
			eRPPartPriceBreakInformationDto.imjLeadTime = dataTable.Rows[0].Field<short>("imjLeadTime");
			eRPPartPriceBreakInformationDto.imjPartPriceID = dataTable.Rows[0].Field<int>("imjPartPriceID");
			eRPPartPriceBreakInformationDto.imjProposedNewPrice = dataTable.Rows[0].Field<decimal>("imjProposedNewPrice");
			eRPPartPriceBreakInformationDto.imjQuantity = dataTable.Rows[0].Field<decimal>("imjQuantity");
			eRPPartPriceBreakInformationDto.imjRowVersion = dataTable.Rows[0].Field<byte[]>("imjRowVersion");
			eRPPartPriceBreakInformationDto.imjPartPriceBreakID = dataTable.Rows[0].Field<short>("imjPartPriceBreakID");
			eRPPartPriceBreakInformationDto.imjUnitPrice = dataTable.Rows[0].Field<decimal>("imjUnitPrice");
			eRPPartPriceBreakInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartPriceBreakInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartPriceBreakInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartPriceBreak(ERPPartPriceBreakDto partPriceBreak)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartPriceBreaks WHERE imjUniqueID = " + M1Util.ConvertToLinq(partPriceBreak.imjUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imjPartPriceID"] = partPriceBreak.imjPartPriceID;
				dataRow["imjPartPriceBreakID"] = partPriceBreak.imjPartPriceBreakID;
				partPriceBreak.imjUniqueID = ((partPriceBreak.imjUniqueID == Guid.Empty) ? Guid.NewGuid() : partPriceBreak.imjUniqueID);
				dataRow["imjUniqueID"] = partPriceBreak.imjUniqueID;
				dataRow["imjCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imjCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartPriceBreak could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partPriceBreak.imjRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartPriceBreak is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imjRowVersion"], partPriceBreak.imjRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartPriceBreak has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartPriceBreak again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imjDiscount"] = partPriceBreak.imjDiscount;
			dataRow["imjLeadTime"] = partPriceBreak.imjLeadTime;
			dataRow["imjProposedNewPrice"] = partPriceBreak.imjProposedNewPrice;
			dataRow["imjQuantity"] = partPriceBreak.imjQuantity;
			dataRow["imjUnitPrice"] = partPriceBreak.imjUnitPrice;
			if (partPriceBreak.CustomFields != null && partPriceBreak.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partPriceBreak.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartPriceBreak [{partPriceBreak.imjUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartPriceBreak [{partPriceBreak.imjUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
