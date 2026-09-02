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

public class ERPInspectionLineApprovalRepository : APIBaseRepository, IERPInspectionLineApprovalRepository, IAPIBaseRepository, IDisposable
{
	public ERPInspectionLineApprovalRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInspectionLineApprovalExist(Guid inspectionLineApprovalId)
	{
		InitializeParameterLists();
		base.filterList.Add("qaaUniqueID|C", inspectionLineApprovalId);
		base.selectList.Add("qaaUniqueID");
		return Task.FromResult(GetAsObject("InspectionLineApprovals", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInspectionLineApprovalInformationDto>> GetAllInspectionLineApprovals(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInspectionLineApprovalInformationDto> collection = new List<ERPInspectionLineApprovalInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "qaaApprovalEmployeeID", "qaaCreatedBy", "qaaCreatedDate", "qaaDescription", "qaaUniqueID", "qaaInspectionID", "qaaInspectionLineID", "qaaInspectionLineApprovalID", "qaaStatus", "qaaStatusDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("InspectionLineApprovals");
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
		using (DataTable dataTable = GetAsDataTable("InspectionLineApprovals", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInspectionLineApprovalInformationDto eRPInspectionLineApprovalInformationDto = new ERPInspectionLineApprovalInformationDto();
				eRPInspectionLineApprovalInformationDto.qaaApprovalEmployeeID = dataTable.Rows[i].Field<string>("qaaApprovalEmployeeID");
				eRPInspectionLineApprovalInformationDto.qaaCreatedBy = dataTable.Rows[i].Field<string>("qaaCreatedBy");
				eRPInspectionLineApprovalInformationDto.qaaCreatedDate = dataTable.Rows[i].Field<DateTime?>("qaaCreatedDate");
				eRPInspectionLineApprovalInformationDto.qaaDescription = dataTable.Rows[i].Field<string>("qaaDescription");
				eRPInspectionLineApprovalInformationDto.qaaUniqueID = dataTable.Rows[i].Field<Guid>("qaaUniqueID");
				eRPInspectionLineApprovalInformationDto.qaaInspectionID = dataTable.Rows[i].Field<string>("qaaInspectionID");
				eRPInspectionLineApprovalInformationDto.qaaInspectionLineID = dataTable.Rows[i].Field<short>("qaaInspectionLineID");
				eRPInspectionLineApprovalInformationDto.qaaInspectionLineApprovalID = dataTable.Rows[i].Field<byte>("qaaInspectionLineApprovalID");
				eRPInspectionLineApprovalInformationDto.qaaStatus = dataTable.Rows[i].Field<byte>("qaaStatus");
				eRPInspectionLineApprovalInformationDto.qaaStatusDate = dataTable.Rows[i].Field<DateTime?>("qaaStatusDate");
				eRPInspectionLineApprovalInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInspectionLineApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInspectionLineApprovalInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInspectionLineApprovalInformationDto> GetInspectionLineApproval(Guid inspectionLineApprovalId)
	{
		ERPInspectionLineApprovalInformationDto eRPInspectionLineApprovalInformationDto = new ERPInspectionLineApprovalInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "qaaApprovalEmployeeID", "qaaCreatedBy", "qaaCreatedDate", "qaaDescription", "qaaUniqueID", "qaaInspectionID", "qaaInspectionLineID", "qaaInspectionLineApprovalID", "qaaStatus", "qaaStatusDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("qaaUniqueID|C", inspectionLineApprovalId);
		AddCustomFieldsToSelectList("InspectionLineApprovals");
		using (DataTable dataTable = GetAsDataTable("InspectionLineApprovals", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInspectionLineApprovalInformationDto);
			}
			eRPInspectionLineApprovalInformationDto.qaaApprovalEmployeeID = dataTable.Rows[0].Field<string>("qaaApprovalEmployeeID");
			eRPInspectionLineApprovalInformationDto.qaaCreatedBy = dataTable.Rows[0].Field<string>("qaaCreatedBy");
			eRPInspectionLineApprovalInformationDto.qaaCreatedDate = dataTable.Rows[0].Field<DateTime?>("qaaCreatedDate");
			eRPInspectionLineApprovalInformationDto.qaaDescription = dataTable.Rows[0].Field<string>("qaaDescription");
			eRPInspectionLineApprovalInformationDto.qaaUniqueID = dataTable.Rows[0].Field<Guid>("qaaUniqueID");
			eRPInspectionLineApprovalInformationDto.qaaInspectionID = dataTable.Rows[0].Field<string>("qaaInspectionID");
			eRPInspectionLineApprovalInformationDto.qaaInspectionLineID = dataTable.Rows[0].Field<short>("qaaInspectionLineID");
			eRPInspectionLineApprovalInformationDto.qaaInspectionLineApprovalID = dataTable.Rows[0].Field<byte>("qaaInspectionLineApprovalID");
			eRPInspectionLineApprovalInformationDto.qaaStatus = dataTable.Rows[0].Field<byte>("qaaStatus");
			eRPInspectionLineApprovalInformationDto.qaaStatusDate = dataTable.Rows[0].Field<DateTime?>("qaaStatusDate");
			eRPInspectionLineApprovalInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInspectionLineApprovalInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInspectionLineApprovalInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInspectionLineApproval(ERPInspectionLineApprovalDto inspectionLineApproval)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM InspectionLineApprovals WHERE qaaUniqueID = " + M1Util.ConvertToLinq(inspectionLineApproval.qaaUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qaaInspectionID"] = inspectionLineApproval.qaaInspectionID.ToUpper();
				dataRow["qaaInspectionLineID"] = inspectionLineApproval.qaaInspectionLineID;
				dataRow["qaaApprovalEmployeeID"] = inspectionLineApproval.qaaApprovalEmployeeID.ToUpper();
				dataRow["qaaInspectionLineApprovalID"] = inspectionLineApproval.qaaInspectionLineApprovalID;
				inspectionLineApproval.qaaUniqueID = ((inspectionLineApproval.qaaUniqueID == Guid.Empty) ? Guid.NewGuid() : inspectionLineApproval.qaaUniqueID);
				dataRow["qaaUniqueID"] = inspectionLineApproval.qaaUniqueID;
				dataRow["qaaCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qaaCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The InspectionLineApproval could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qaaDescription"] = inspectionLineApproval.qaaDescription;
			dataRow["qaaStatus"] = inspectionLineApproval.qaaStatus;
			DataRow dataRow2 = dataRow;
			DateTime? qaaStatusDate = inspectionLineApproval.qaaStatusDate;
			dataRow2["qaaStatusDate"] = (qaaStatusDate.HasValue ? ((object)qaaStatusDate.GetValueOrDefault()) : dataRow["qaaStatusDate"]);
			if (inspectionLineApproval.CustomFields != null && inspectionLineApproval.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inspectionLineApproval.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the InspectionLineApproval [{inspectionLineApproval.qaaUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the InspectionLineApproval [{inspectionLineApproval.qaaUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
