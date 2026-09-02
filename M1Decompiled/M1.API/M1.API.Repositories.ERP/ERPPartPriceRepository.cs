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

public class ERPPartPriceRepository : APIBaseRepository, IERPPartPriceRepository, IAPIBaseRepository, IDisposable
{
	public ERPPartPriceRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesPartPriceExist(Guid partPriceId)
	{
		InitializeParameterLists();
		base.filterList.Add("imiUniqueID|C", partPriceId);
		base.selectList.Add("imiUniqueID");
		return Task.FromResult(GetAsObject("PartPrices", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPPartPriceInformationDto>> GetAllPartPrices(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPPartPriceInformationDto> collection = new List<ERPPartPriceInformationDto>();
		InitializeParameterLists();
		string[] array = new string[18]
		{
			"imiCreatedBy", "imiCreatedDate", "imiCurrencyRateID", "imiCustomerGroupID", "imiEndDate", "imiUniqueID", "imiInventoryPrice", "imiLocationID", "imiOrganizationID", "imiPartGroupID",
			"imiPartID", "imiPartRevisionID", "imiPriceType", "imiQuoteID", "imiRfqID", "imiRowVersion", "imiPartPriceID", "imiStartDate"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("PartPrices");
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
		using (DataTable dataTable = GetAsDataTable("PartPrices", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPPartPriceInformationDto eRPPartPriceInformationDto = new ERPPartPriceInformationDto();
				eRPPartPriceInformationDto.imiCreatedBy = dataTable.Rows[i].Field<string>("imiCreatedBy");
				eRPPartPriceInformationDto.imiCreatedDate = dataTable.Rows[i].Field<DateTime?>("imiCreatedDate");
				eRPPartPriceInformationDto.imiCurrencyRateID = dataTable.Rows[i].Field<string>("imiCurrencyRateID");
				eRPPartPriceInformationDto.imiCustomerGroupID = dataTable.Rows[i].Field<string>("imiCustomerGroupID");
				eRPPartPriceInformationDto.imiEndDate = dataTable.Rows[i].Field<DateTime?>("imiEndDate");
				eRPPartPriceInformationDto.imiUniqueID = dataTable.Rows[i].Field<Guid>("imiUniqueID");
				eRPPartPriceInformationDto.imiInventoryPrice = dataTable.Rows[i].Field<bool>("imiInventoryPrice");
				eRPPartPriceInformationDto.imiLocationID = dataTable.Rows[i].Field<string>("imiLocationID");
				eRPPartPriceInformationDto.imiOrganizationID = dataTable.Rows[i].Field<string>("imiOrganizationID");
				eRPPartPriceInformationDto.imiPartGroupID = dataTable.Rows[i].Field<string>("imiPartGroupID");
				eRPPartPriceInformationDto.imiPartID = dataTable.Rows[i].Field<string>("imiPartID");
				eRPPartPriceInformationDto.imiPartRevisionID = dataTable.Rows[i].Field<string>("imiPartRevisionID");
				eRPPartPriceInformationDto.imiPriceType = dataTable.Rows[i].Field<byte>("imiPriceType");
				eRPPartPriceInformationDto.imiQuoteID = dataTable.Rows[i].Field<string>("imiQuoteID");
				eRPPartPriceInformationDto.imiRfqID = dataTable.Rows[i].Field<string>("imiRfqID");
				eRPPartPriceInformationDto.imiRowVersion = dataTable.Rows[i].Field<byte[]>("imiRowVersion");
				eRPPartPriceInformationDto.imiPartPriceID = dataTable.Rows[i].Field<int>("imiPartPriceID");
				eRPPartPriceInformationDto.imiStartDate = dataTable.Rows[i].Field<DateTime?>("imiStartDate");
				eRPPartPriceInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPPartPriceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPPartPriceInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPPartPriceInformationDto> GetPartPrice(Guid partPriceId)
	{
		ERPPartPriceInformationDto eRPPartPriceInformationDto = new ERPPartPriceInformationDto();
		InitializeParameterLists();
		string[] collection = new string[18]
		{
			"imiCreatedBy", "imiCreatedDate", "imiCurrencyRateID", "imiCustomerGroupID", "imiEndDate", "imiUniqueID", "imiInventoryPrice", "imiLocationID", "imiOrganizationID", "imiPartGroupID",
			"imiPartID", "imiPartRevisionID", "imiPriceType", "imiQuoteID", "imiRfqID", "imiRowVersion", "imiPartPriceID", "imiStartDate"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("imiUniqueID|C", partPriceId);
		AddCustomFieldsToSelectList("PartPrices");
		using (DataTable dataTable = GetAsDataTable("PartPrices", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPPartPriceInformationDto);
			}
			eRPPartPriceInformationDto.imiCreatedBy = dataTable.Rows[0].Field<string>("imiCreatedBy");
			eRPPartPriceInformationDto.imiCreatedDate = dataTable.Rows[0].Field<DateTime?>("imiCreatedDate");
			eRPPartPriceInformationDto.imiCurrencyRateID = dataTable.Rows[0].Field<string>("imiCurrencyRateID");
			eRPPartPriceInformationDto.imiCustomerGroupID = dataTable.Rows[0].Field<string>("imiCustomerGroupID");
			eRPPartPriceInformationDto.imiEndDate = dataTable.Rows[0].Field<DateTime?>("imiEndDate");
			eRPPartPriceInformationDto.imiUniqueID = dataTable.Rows[0].Field<Guid>("imiUniqueID");
			eRPPartPriceInformationDto.imiInventoryPrice = dataTable.Rows[0].Field<bool>("imiInventoryPrice");
			eRPPartPriceInformationDto.imiLocationID = dataTable.Rows[0].Field<string>("imiLocationID");
			eRPPartPriceInformationDto.imiOrganizationID = dataTable.Rows[0].Field<string>("imiOrganizationID");
			eRPPartPriceInformationDto.imiPartGroupID = dataTable.Rows[0].Field<string>("imiPartGroupID");
			eRPPartPriceInformationDto.imiPartID = dataTable.Rows[0].Field<string>("imiPartID");
			eRPPartPriceInformationDto.imiPartRevisionID = dataTable.Rows[0].Field<string>("imiPartRevisionID");
			eRPPartPriceInformationDto.imiPriceType = dataTable.Rows[0].Field<byte>("imiPriceType");
			eRPPartPriceInformationDto.imiQuoteID = dataTable.Rows[0].Field<string>("imiQuoteID");
			eRPPartPriceInformationDto.imiRfqID = dataTable.Rows[0].Field<string>("imiRfqID");
			eRPPartPriceInformationDto.imiRowVersion = dataTable.Rows[0].Field<byte[]>("imiRowVersion");
			eRPPartPriceInformationDto.imiPartPriceID = dataTable.Rows[0].Field<int>("imiPartPriceID");
			eRPPartPriceInformationDto.imiStartDate = dataTable.Rows[0].Field<DateTime?>("imiStartDate");
			eRPPartPriceInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPPartPriceInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPPartPriceInformationDto);
	}

	public Task<APIValidationInfoDto> SavePartPrice(ERPPartPriceDto partPrice)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM PartPrices WHERE imiUniqueID = " + M1Util.ConvertToLinq(partPrice.imiUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["imiPartPriceID"] = partPrice.imiPartPriceID;
				partPrice.imiUniqueID = ((partPrice.imiUniqueID == Guid.Empty) ? Guid.NewGuid() : partPrice.imiUniqueID);
				dataRow["imiUniqueID"] = partPrice.imiUniqueID;
				dataRow["imiCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["imiCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The PartPrice could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (partPrice.imiRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the PartPrice is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["imiRowVersion"], partPrice.imiRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the PartPrice has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the PartPrice again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["imiCurrencyRateID"] = partPrice.imiCurrencyRateID;
			dataRow["imiCustomerGroupID"] = partPrice.imiCustomerGroupID;
			DataRow dataRow2 = dataRow;
			DateTime? imiEndDate = partPrice.imiEndDate;
			dataRow2["imiEndDate"] = (imiEndDate.HasValue ? ((object)imiEndDate.GetValueOrDefault()) : dataRow["imiEndDate"]);
			dataRow["imiInventoryPrice"] = partPrice.imiInventoryPrice;
			dataRow["imiLocationID"] = partPrice.imiLocationID;
			dataRow["imiOrganizationID"] = partPrice.imiOrganizationID;
			dataRow["imiPartGroupID"] = partPrice.imiPartGroupID;
			dataRow["imiPartID"] = partPrice.imiPartID;
			dataRow["imiPartRevisionID"] = partPrice.imiPartRevisionID;
			dataRow["imiPriceType"] = partPrice.imiPriceType;
			dataRow["imiQuoteID"] = partPrice.imiQuoteID;
			dataRow["imiRfqID"] = partPrice.imiRfqID;
			DataRow dataRow3 = dataRow;
			imiEndDate = partPrice.imiStartDate;
			dataRow3["imiStartDate"] = (imiEndDate.HasValue ? ((object)imiEndDate.GetValueOrDefault()) : dataRow["imiStartDate"]);
			if (partPrice.CustomFields != null && partPrice.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in partPrice.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the PartPrice [{partPrice.imiUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the PartPrice [{partPrice.imiUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
