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

public class ERPRFQRepository : APIBaseRepository, IERPRFQRepository, IAPIBaseRepository, IDisposable
{
	public ERPRFQRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRFQExist(Guid rFQId)
	{
		InitializeParameterLists();
		base.filterList.Add("rqpUniqueID|C", rFQId);
		base.selectList.Add("rqpUniqueID");
		return Task.FromResult(GetAsObject("RFQs", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRFQInformationDto>> GetAllRFQs(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRFQInformationDto> collection = new List<ERPRFQInformationDto>();
		InitializeParameterLists();
		string[] array = new string[14]
		{
			"rqpBuyerEmployeeID", "rqpClosedDate", "rqpRfqID", "rqpCreatedBy", "rqpCreatedDate", "rqpDueDate", "rqpUniqueID", "rqpClosed", "rqpReadyToPrint", "rqpPlantDepartmentID",
			"rqpPlantID", "rqpRfqDate", "rqpRowVersion", "rqpStandardMessageID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RFQs");
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
		using (DataTable dataTable = GetAsDataTable("RFQs", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRFQInformationDto eRPRFQInformationDto = new ERPRFQInformationDto();
				eRPRFQInformationDto.rqpBuyerEmployeeID = dataTable.Rows[i].Field<string>("rqpBuyerEmployeeID");
				eRPRFQInformationDto.rqpClosedDate = dataTable.Rows[i].Field<DateTime?>("rqpClosedDate");
				eRPRFQInformationDto.rqpRfqID = dataTable.Rows[i].Field<string>("rqpRfqID");
				eRPRFQInformationDto.rqpCreatedBy = dataTable.Rows[i].Field<string>("rqpCreatedBy");
				eRPRFQInformationDto.rqpCreatedDate = dataTable.Rows[i].Field<DateTime?>("rqpCreatedDate");
				eRPRFQInformationDto.rqpDueDate = dataTable.Rows[i].Field<DateTime?>("rqpDueDate");
				eRPRFQInformationDto.rqpUniqueID = dataTable.Rows[i].Field<Guid>("rqpUniqueID");
				eRPRFQInformationDto.rqpClosed = dataTable.Rows[i].Field<bool>("rqpClosed");
				eRPRFQInformationDto.rqpReadyToPrint = dataTable.Rows[i].Field<bool>("rqpReadyToPrint");
				eRPRFQInformationDto.rqpPlantDepartmentID = dataTable.Rows[i].Field<string>("rqpPlantDepartmentID");
				eRPRFQInformationDto.rqpPlantID = dataTable.Rows[i].Field<string>("rqpPlantID");
				eRPRFQInformationDto.rqpRfqDate = dataTable.Rows[i].Field<DateTime?>("rqpRfqDate");
				eRPRFQInformationDto.rqpRowVersion = dataTable.Rows[i].Field<byte[]>("rqpRowVersion");
				eRPRFQInformationDto.rqpStandardMessageID = dataTable.Rows[i].Field<string>("rqpStandardMessageID");
				eRPRFQInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRFQInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRFQInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRFQInformationDto> GetRFQ(Guid rFQId)
	{
		ERPRFQInformationDto eRPRFQInformationDto = new ERPRFQInformationDto();
		InitializeParameterLists();
		string[] collection = new string[14]
		{
			"rqpBuyerEmployeeID", "rqpClosedDate", "rqpRfqID", "rqpCreatedBy", "rqpCreatedDate", "rqpDueDate", "rqpUniqueID", "rqpClosed", "rqpReadyToPrint", "rqpPlantDepartmentID",
			"rqpPlantID", "rqpRfqDate", "rqpRowVersion", "rqpStandardMessageID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rqpUniqueID|C", rFQId);
		AddCustomFieldsToSelectList("RFQs");
		using (DataTable dataTable = GetAsDataTable("RFQs", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRFQInformationDto);
			}
			eRPRFQInformationDto.rqpBuyerEmployeeID = dataTable.Rows[0].Field<string>("rqpBuyerEmployeeID");
			eRPRFQInformationDto.rqpClosedDate = dataTable.Rows[0].Field<DateTime?>("rqpClosedDate");
			eRPRFQInformationDto.rqpRfqID = dataTable.Rows[0].Field<string>("rqpRfqID");
			eRPRFQInformationDto.rqpCreatedBy = dataTable.Rows[0].Field<string>("rqpCreatedBy");
			eRPRFQInformationDto.rqpCreatedDate = dataTable.Rows[0].Field<DateTime?>("rqpCreatedDate");
			eRPRFQInformationDto.rqpDueDate = dataTable.Rows[0].Field<DateTime?>("rqpDueDate");
			eRPRFQInformationDto.rqpUniqueID = dataTable.Rows[0].Field<Guid>("rqpUniqueID");
			eRPRFQInformationDto.rqpClosed = dataTable.Rows[0].Field<bool>("rqpClosed");
			eRPRFQInformationDto.rqpReadyToPrint = dataTable.Rows[0].Field<bool>("rqpReadyToPrint");
			eRPRFQInformationDto.rqpPlantDepartmentID = dataTable.Rows[0].Field<string>("rqpPlantDepartmentID");
			eRPRFQInformationDto.rqpPlantID = dataTable.Rows[0].Field<string>("rqpPlantID");
			eRPRFQInformationDto.rqpRfqDate = dataTable.Rows[0].Field<DateTime?>("rqpRfqDate");
			eRPRFQInformationDto.rqpRowVersion = dataTable.Rows[0].Field<byte[]>("rqpRowVersion");
			eRPRFQInformationDto.rqpStandardMessageID = dataTable.Rows[0].Field<string>("rqpStandardMessageID");
			eRPRFQInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRFQInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRFQInformationDto);
	}

	public Task<APIValidationInfoDto> SaveRFQ(ERPRFQDto rFQ)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM RFQs WHERE rqpUniqueID = " + M1Util.ConvertToLinq(rFQ.rqpUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["rqpRfqID"] = rFQ.rqpRfqID.ToUpper();
				rFQ.rqpUniqueID = ((rFQ.rqpUniqueID == Guid.Empty) ? Guid.NewGuid() : rFQ.rqpUniqueID);
				dataRow["rqpUniqueID"] = rFQ.rqpUniqueID;
				dataRow["rqpCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["rqpCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The RFQ could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (rFQ.rqpRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the RFQ is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["rqpRowVersion"], rFQ.rqpRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the RFQ has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the RFQ again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["rqpBuyerEmployeeID"] = rFQ.rqpBuyerEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? rqpClosedDate = rFQ.rqpClosedDate;
			dataRow2["rqpClosedDate"] = (rqpClosedDate.HasValue ? ((object)rqpClosedDate.GetValueOrDefault()) : dataRow["rqpClosedDate"]);
			DataRow dataRow3 = dataRow;
			rqpClosedDate = rFQ.rqpDueDate;
			dataRow3["rqpDueDate"] = (rqpClosedDate.HasValue ? ((object)rqpClosedDate.GetValueOrDefault()) : dataRow["rqpDueDate"]);
			dataRow["rqpClosed"] = rFQ.rqpClosed;
			dataRow["rqpReadyToPrint"] = rFQ.rqpReadyToPrint;
			dataRow["rqpPlantDepartmentID"] = rFQ.rqpPlantDepartmentID;
			dataRow["rqpPlantID"] = rFQ.rqpPlantID;
			DataRow dataRow4 = dataRow;
			rqpClosedDate = rFQ.rqpRfqDate;
			dataRow4["rqpRfqDate"] = (rqpClosedDate.HasValue ? ((object)rqpClosedDate.GetValueOrDefault()) : dataRow["rqpRfqDate"]);
			dataRow["rqpStandardMessageID"] = rFQ.rqpStandardMessageID;
			if (rFQ.CustomFields != null && rFQ.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in rFQ.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the RFQ [{rFQ.rqpUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the RFQ [{rFQ.rqpUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
