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

public class ERPProcessRepository : APIBaseRepository, IERPProcessRepository, IAPIBaseRepository, IDisposable
{
	public ERPProcessRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProcessExist(Guid processId)
	{
		InitializeParameterLists();
		base.filterList.Add("xacUniqueID|C", processId);
		base.selectList.Add("xacUniqueID");
		return Task.FromResult(GetAsObject("Processes", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProcessInformationDto>> GetAllProcesses(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProcessInformationDto> collection = new List<ERPProcessInformationDto>();
		InitializeParameterLists();
		string[] array = new string[20]
		{
			"xacProcessID", "xacCreatedBy", "xacCreatedDate", "xacUniqueID", "xacInactiveDate", "xacInspectionType", "xacInactive", "xacExcludeFromTMJobs", "xacIgnoreCalendarMove", "xacIgnoreCalendarQueue",
			"xacPrintInspectionLine", "xacLongDescriptionRtf", "xacLongDescriptionText", "xacProductionStandard", "xacProjectedProductionRate", "xacProjectedSetupRate", "xacRowVersion", "xacSetupHours", "xacShortDescription", "xacStandardFactor"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Processes");
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
		using (DataTable dataTable = GetAsDataTable("Processes", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProcessInformationDto eRPProcessInformationDto = new ERPProcessInformationDto();
				eRPProcessInformationDto.xacProcessID = dataTable.Rows[i].Field<string>("xacProcessID");
				eRPProcessInformationDto.xacCreatedBy = dataTable.Rows[i].Field<string>("xacCreatedBy");
				eRPProcessInformationDto.xacCreatedDate = dataTable.Rows[i].Field<DateTime?>("xacCreatedDate");
				eRPProcessInformationDto.xacUniqueID = dataTable.Rows[i].Field<Guid>("xacUniqueID");
				eRPProcessInformationDto.xacInactiveDate = dataTable.Rows[i].Field<DateTime?>("xacInactiveDate");
				eRPProcessInformationDto.xacInspectionType = dataTable.Rows[i].Field<byte>("xacInspectionType");
				eRPProcessInformationDto.xacInactive = dataTable.Rows[i].Field<bool>("xacInactive");
				eRPProcessInformationDto.xacExcludeFromTMJobs = dataTable.Rows[i].Field<bool>("xacExcludeFromTMJobs");
				eRPProcessInformationDto.xacIgnoreCalendarMove = dataTable.Rows[i].Field<bool>("xacIgnoreCalendarMove");
				eRPProcessInformationDto.xacIgnoreCalendarQueue = dataTable.Rows[i].Field<bool>("xacIgnoreCalendarQueue");
				eRPProcessInformationDto.xacPrintInspectionLine = dataTable.Rows[i].Field<bool>("xacPrintInspectionLine");
				eRPProcessInformationDto.xacLongDescriptionRtf = dataTable.Rows[i].Field<string>("xacLongDescriptionRtf");
				eRPProcessInformationDto.xacLongDescriptionText = dataTable.Rows[i].Field<string>("xacLongDescriptionText");
				eRPProcessInformationDto.xacProductionStandard = dataTable.Rows[i].Field<decimal>("xacProductionStandard");
				eRPProcessInformationDto.xacProjectedProductionRate = dataTable.Rows[i].Field<decimal>("xacProjectedProductionRate");
				eRPProcessInformationDto.xacProjectedSetupRate = dataTable.Rows[i].Field<decimal>("xacProjectedSetupRate");
				eRPProcessInformationDto.xacRowVersion = dataTable.Rows[i].Field<byte[]>("xacRowVersion");
				eRPProcessInformationDto.xacSetupHours = dataTable.Rows[i].Field<decimal>("xacSetupHours");
				eRPProcessInformationDto.xacShortDescription = dataTable.Rows[i].Field<string>("xacShortDescription");
				eRPProcessInformationDto.xacStandardFactor = dataTable.Rows[i].Field<string>("xacStandardFactor");
				eRPProcessInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProcessInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProcessInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProcessInformationDto> GetProcess(Guid processId)
	{
		ERPProcessInformationDto eRPProcessInformationDto = new ERPProcessInformationDto();
		InitializeParameterLists();
		string[] collection = new string[20]
		{
			"xacProcessID", "xacCreatedBy", "xacCreatedDate", "xacUniqueID", "xacInactiveDate", "xacInspectionType", "xacInactive", "xacExcludeFromTMJobs", "xacIgnoreCalendarMove", "xacIgnoreCalendarQueue",
			"xacPrintInspectionLine", "xacLongDescriptionRtf", "xacLongDescriptionText", "xacProductionStandard", "xacProjectedProductionRate", "xacProjectedSetupRate", "xacRowVersion", "xacSetupHours", "xacShortDescription", "xacStandardFactor"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("xacUniqueID|C", processId);
		AddCustomFieldsToSelectList("Processes");
		using (DataTable dataTable = GetAsDataTable("Processes", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProcessInformationDto);
			}
			eRPProcessInformationDto.xacProcessID = dataTable.Rows[0].Field<string>("xacProcessID");
			eRPProcessInformationDto.xacCreatedBy = dataTable.Rows[0].Field<string>("xacCreatedBy");
			eRPProcessInformationDto.xacCreatedDate = dataTable.Rows[0].Field<DateTime?>("xacCreatedDate");
			eRPProcessInformationDto.xacUniqueID = dataTable.Rows[0].Field<Guid>("xacUniqueID");
			eRPProcessInformationDto.xacInactiveDate = dataTable.Rows[0].Field<DateTime?>("xacInactiveDate");
			eRPProcessInformationDto.xacInspectionType = dataTable.Rows[0].Field<byte>("xacInspectionType");
			eRPProcessInformationDto.xacInactive = dataTable.Rows[0].Field<bool>("xacInactive");
			eRPProcessInformationDto.xacExcludeFromTMJobs = dataTable.Rows[0].Field<bool>("xacExcludeFromTMJobs");
			eRPProcessInformationDto.xacIgnoreCalendarMove = dataTable.Rows[0].Field<bool>("xacIgnoreCalendarMove");
			eRPProcessInformationDto.xacIgnoreCalendarQueue = dataTable.Rows[0].Field<bool>("xacIgnoreCalendarQueue");
			eRPProcessInformationDto.xacPrintInspectionLine = dataTable.Rows[0].Field<bool>("xacPrintInspectionLine");
			eRPProcessInformationDto.xacLongDescriptionRtf = dataTable.Rows[0].Field<string>("xacLongDescriptionRtf");
			eRPProcessInformationDto.xacLongDescriptionText = dataTable.Rows[0].Field<string>("xacLongDescriptionText");
			eRPProcessInformationDto.xacProductionStandard = dataTable.Rows[0].Field<decimal>("xacProductionStandard");
			eRPProcessInformationDto.xacProjectedProductionRate = dataTable.Rows[0].Field<decimal>("xacProjectedProductionRate");
			eRPProcessInformationDto.xacProjectedSetupRate = dataTable.Rows[0].Field<decimal>("xacProjectedSetupRate");
			eRPProcessInformationDto.xacRowVersion = dataTable.Rows[0].Field<byte[]>("xacRowVersion");
			eRPProcessInformationDto.xacSetupHours = dataTable.Rows[0].Field<decimal>("xacSetupHours");
			eRPProcessInformationDto.xacShortDescription = dataTable.Rows[0].Field<string>("xacShortDescription");
			eRPProcessInformationDto.xacStandardFactor = dataTable.Rows[0].Field<string>("xacStandardFactor");
			eRPProcessInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProcessInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProcessInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProcess(ERPProcessDto process)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Processes WHERE xacUniqueID = " + M1Util.ConvertToLinq(process.xacUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["xacProcessID"] = process.xacProcessID.ToUpper();
				process.xacUniqueID = ((process.xacUniqueID == Guid.Empty) ? Guid.NewGuid() : process.xacUniqueID);
				dataRow["xacUniqueID"] = process.xacUniqueID;
				dataRow["xacCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["xacCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Process could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (process.xacRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Process is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["xacRowVersion"], process.xacRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Process has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Process again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			DataRow dataRow2 = dataRow;
			DateTime? xacInactiveDate = process.xacInactiveDate;
			dataRow2["xacInactiveDate"] = (xacInactiveDate.HasValue ? ((object)xacInactiveDate.GetValueOrDefault()) : dataRow["xacInactiveDate"]);
			dataRow["xacInspectionType"] = process.xacInspectionType;
			dataRow["xacInactive"] = process.xacInactive;
			dataRow["xacExcludeFromTMJobs"] = process.xacExcludeFromTMJobs;
			dataRow["xacIgnoreCalendarMove"] = process.xacIgnoreCalendarMove;
			dataRow["xacIgnoreCalendarQueue"] = process.xacIgnoreCalendarQueue;
			dataRow["xacPrintInspectionLine"] = process.xacPrintInspectionLine;
			dataRow["xacLongDescriptionRtf"] = process.xacLongDescriptionRtf ?? dataRow["xacLongDescriptionRtf"];
			dataRow["xacLongDescriptionText"] = process.xacLongDescriptionText ?? dataRow["xacLongDescriptionText"];
			dataRow["xacProductionStandard"] = process.xacProductionStandard;
			dataRow["xacProjectedProductionRate"] = process.xacProjectedProductionRate;
			dataRow["xacProjectedSetupRate"] = process.xacProjectedSetupRate;
			dataRow["xacSetupHours"] = process.xacSetupHours;
			dataRow["xacShortDescription"] = process.xacShortDescription;
			dataRow["xacStandardFactor"] = process.xacStandardFactor;
			if (process.CustomFields != null && process.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in process.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Process [{process.xacUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Process [{process.xacUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
