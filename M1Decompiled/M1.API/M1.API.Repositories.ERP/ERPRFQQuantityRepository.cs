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

public class ERPRFQQuantityRepository : APIBaseRepository, IERPRFQQuantityRepository, IAPIBaseRepository, IDisposable
{
	public ERPRFQQuantityRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRFQQuantityExist(Guid rFQQuantityId)
	{
		InitializeParameterLists();
		base.filterList.Add("rqqUniqueID|C", rFQQuantityId);
		base.selectList.Add("rqqUniqueID");
		return Task.FromResult(GetAsObject("RFQQuantities", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRFQQuantityInformationDto>> GetAllRFQQuantities(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRFQQuantityInformationDto> collection = new List<ERPRFQQuantityInformationDto>();
		InitializeParameterLists();
		string[] array = new string[13]
		{
			"rqqCreatedBy", "rqqCreatedDate", "rqqUniqueID", "rqqClosed", "rqqLeadTime", "rqqPriceBase", "rqqPriceForeign", "rqqQuantity", "rqqRfqID", "rqqRfqLineID",
			"rqqRfqSupplierID", "rqqRowVersion", "rqqRfqQuantityID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RFQQuantities");
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
		using (DataTable dataTable = GetAsDataTable("RFQQuantities", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRFQQuantityInformationDto eRPRFQQuantityInformationDto = new ERPRFQQuantityInformationDto();
				eRPRFQQuantityInformationDto.rqqCreatedBy = dataTable.Rows[i].Field<string>("rqqCreatedBy");
				eRPRFQQuantityInformationDto.rqqCreatedDate = dataTable.Rows[i].Field<DateTime?>("rqqCreatedDate");
				eRPRFQQuantityInformationDto.rqqUniqueID = dataTable.Rows[i].Field<Guid>("rqqUniqueID");
				eRPRFQQuantityInformationDto.rqqClosed = dataTable.Rows[i].Field<bool>("rqqClosed");
				eRPRFQQuantityInformationDto.rqqLeadTime = dataTable.Rows[i].Field<short>("rqqLeadTime");
				eRPRFQQuantityInformationDto.rqqPriceBase = dataTable.Rows[i].Field<decimal>("rqqPriceBase");
				eRPRFQQuantityInformationDto.rqqPriceForeign = dataTable.Rows[i].Field<decimal>("rqqPriceForeign");
				eRPRFQQuantityInformationDto.rqqQuantity = dataTable.Rows[i].Field<decimal>("rqqQuantity");
				eRPRFQQuantityInformationDto.rqqRfqID = dataTable.Rows[i].Field<string>("rqqRfqID");
				eRPRFQQuantityInformationDto.rqqRfqLineID = dataTable.Rows[i].Field<short>("rqqRfqLineID");
				eRPRFQQuantityInformationDto.rqqRfqSupplierID = dataTable.Rows[i].Field<short>("rqqRfqSupplierID");
				eRPRFQQuantityInformationDto.rqqRowVersion = dataTable.Rows[i].Field<byte[]>("rqqRowVersion");
				eRPRFQQuantityInformationDto.rqqRfqQuantityID = dataTable.Rows[i].Field<short>("rqqRfqQuantityID");
				eRPRFQQuantityInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRFQQuantityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRFQQuantityInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRFQQuantityInformationDto> GetRFQQuantity(Guid rFQQuantityId)
	{
		ERPRFQQuantityInformationDto eRPRFQQuantityInformationDto = new ERPRFQQuantityInformationDto();
		InitializeParameterLists();
		string[] collection = new string[13]
		{
			"rqqCreatedBy", "rqqCreatedDate", "rqqUniqueID", "rqqClosed", "rqqLeadTime", "rqqPriceBase", "rqqPriceForeign", "rqqQuantity", "rqqRfqID", "rqqRfqLineID",
			"rqqRfqSupplierID", "rqqRowVersion", "rqqRfqQuantityID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rqqUniqueID|C", rFQQuantityId);
		AddCustomFieldsToSelectList("RFQQuantities");
		using (DataTable dataTable = GetAsDataTable("RFQQuantities", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRFQQuantityInformationDto);
			}
			eRPRFQQuantityInformationDto.rqqCreatedBy = dataTable.Rows[0].Field<string>("rqqCreatedBy");
			eRPRFQQuantityInformationDto.rqqCreatedDate = dataTable.Rows[0].Field<DateTime?>("rqqCreatedDate");
			eRPRFQQuantityInformationDto.rqqUniqueID = dataTable.Rows[0].Field<Guid>("rqqUniqueID");
			eRPRFQQuantityInformationDto.rqqClosed = dataTable.Rows[0].Field<bool>("rqqClosed");
			eRPRFQQuantityInformationDto.rqqLeadTime = dataTable.Rows[0].Field<short>("rqqLeadTime");
			eRPRFQQuantityInformationDto.rqqPriceBase = dataTable.Rows[0].Field<decimal>("rqqPriceBase");
			eRPRFQQuantityInformationDto.rqqPriceForeign = dataTable.Rows[0].Field<decimal>("rqqPriceForeign");
			eRPRFQQuantityInformationDto.rqqQuantity = dataTable.Rows[0].Field<decimal>("rqqQuantity");
			eRPRFQQuantityInformationDto.rqqRfqID = dataTable.Rows[0].Field<string>("rqqRfqID");
			eRPRFQQuantityInformationDto.rqqRfqLineID = dataTable.Rows[0].Field<short>("rqqRfqLineID");
			eRPRFQQuantityInformationDto.rqqRfqSupplierID = dataTable.Rows[0].Field<short>("rqqRfqSupplierID");
			eRPRFQQuantityInformationDto.rqqRowVersion = dataTable.Rows[0].Field<byte[]>("rqqRowVersion");
			eRPRFQQuantityInformationDto.rqqRfqQuantityID = dataTable.Rows[0].Field<short>("rqqRfqQuantityID");
			eRPRFQQuantityInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRFQQuantityInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRFQQuantityInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRFQQuantity(ERPRFQQuantityDto rFQQuantity)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RFQQuantities WHERE rqqUniqueID = " + M1Util.ConvertToLinq(rFQQuantity.rqqUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rqqRfqID"] = rFQQuantity.rqqRfqID.ToUpper();
				dataRow["rqqRfqLineID"] = rFQQuantity.rqqRfqLineID;
				dataRow["rqqRfqSupplierID"] = rFQQuantity.rqqRfqSupplierID;
				dataRow["rqqRfqQuantityID"] = rFQQuantity.rqqRfqQuantityID;
				rFQQuantity.rqqUniqueID = ((rFQQuantity.rqqUniqueID == Guid.Empty) ? Guid.NewGuid() : rFQQuantity.rqqUniqueID);
				dataRow["rqqUniqueID"] = rFQQuantity.rqqUniqueID;
				dataRow["rqqCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rqqCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RFQQuantity could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rFQQuantity.rqqRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RFQQuantity is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rqqRowVersion"], rFQQuantity.rqqRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RFQQuantity has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RFQQuantity again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rqqClosed"] = rFQQuantity.rqqClosed;
			dataRow["rqqLeadTime"] = rFQQuantity.rqqLeadTime;
			dataRow["rqqPriceBase"] = rFQQuantity.rqqPriceBase;
			dataRow["rqqPriceForeign"] = rFQQuantity.rqqPriceForeign;
			dataRow["rqqQuantity"] = rFQQuantity.rqqQuantity;
			if (rFQQuantity.CustomFields != null && rFQQuantity.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rFQQuantity.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RFQQuantity [{rFQQuantity.rqqUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RFQQuantity [{rFQQuantity.rqqUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
