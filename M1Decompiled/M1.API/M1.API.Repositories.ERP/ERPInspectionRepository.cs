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

public class ERPInspectionRepository : APIBaseRepository, IERPInspectionRepository, IAPIBaseRepository, IDisposable
{
	public ERPInspectionRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesInspectionExist(Guid inspectionId)
	{
		InitializeParameterLists();
		base.filterList.Add("qapUniqueID|C", inspectionId);
		base.selectList.Add("qapUniqueID");
		return Task.FromResult(GetAsObject("Inspections", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPInspectionInformationDto>> GetAllInspections(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPInspectionInformationDto> collection = new List<ERPInspectionInformationDto>();
		InitializeParameterLists();
		string[] array = new string[15]
		{
			"qapInspectionID", "qapCreatedBy", "qapCreatedDate", "qapUniqueID", "qapPosted", "qapReversalEntry", "qapOpenedByEmployeeID", "qapOpenedDate", "qapPlantDepartmentID", "qapPlantID",
			"qapPostedDate", "qapProjectID", "qapRowVersion", "qapSourceTableName", "qapSourceTableUniqueID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Inspections");
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
		using (DataTable dataTable = GetAsDataTable("Inspections", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPInspectionInformationDto eRPInspectionInformationDto = new ERPInspectionInformationDto();
				eRPInspectionInformationDto.qapInspectionID = dataTable.Rows[i].Field<string>("qapInspectionID");
				eRPInspectionInformationDto.qapCreatedBy = dataTable.Rows[i].Field<string>("qapCreatedBy");
				eRPInspectionInformationDto.qapCreatedDate = dataTable.Rows[i].Field<DateTime?>("qapCreatedDate");
				eRPInspectionInformationDto.qapUniqueID = dataTable.Rows[i].Field<Guid>("qapUniqueID");
				eRPInspectionInformationDto.qapPosted = dataTable.Rows[i].Field<bool>("qapPosted");
				eRPInspectionInformationDto.qapReversalEntry = dataTable.Rows[i].Field<bool>("qapReversalEntry");
				eRPInspectionInformationDto.qapOpenedByEmployeeID = dataTable.Rows[i].Field<string>("qapOpenedByEmployeeID");
				eRPInspectionInformationDto.qapOpenedDate = dataTable.Rows[i].Field<DateTime?>("qapOpenedDate");
				eRPInspectionInformationDto.qapPlantDepartmentID = dataTable.Rows[i].Field<string>("qapPlantDepartmentID");
				eRPInspectionInformationDto.qapPlantID = dataTable.Rows[i].Field<string>("qapPlantID");
				eRPInspectionInformationDto.qapPostedDate = dataTable.Rows[i].Field<DateTime?>("qapPostedDate");
				eRPInspectionInformationDto.qapProjectID = dataTable.Rows[i].Field<string>("qapProjectID");
				eRPInspectionInformationDto.qapRowVersion = dataTable.Rows[i].Field<byte[]>("qapRowVersion");
				eRPInspectionInformationDto.qapSourceTableName = dataTable.Rows[i].Field<string>("qapSourceTableName");
				eRPInspectionInformationDto.qapSourceTableUniqueID = dataTable.Rows[i].Field<Guid>("qapSourceTableUniqueID");
				eRPInspectionInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPInspectionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPInspectionInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPInspectionInformationDto> GetInspection(Guid inspectionId)
	{
		ERPInspectionInformationDto eRPInspectionInformationDto = new ERPInspectionInformationDto();
		InitializeParameterLists();
		string[] collection = new string[15]
		{
			"qapInspectionID", "qapCreatedBy", "qapCreatedDate", "qapUniqueID", "qapPosted", "qapReversalEntry", "qapOpenedByEmployeeID", "qapOpenedDate", "qapPlantDepartmentID", "qapPlantID",
			"qapPostedDate", "qapProjectID", "qapRowVersion", "qapSourceTableName", "qapSourceTableUniqueID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("qapUniqueID|C", inspectionId);
		AddCustomFieldsToSelectList("Inspections");
		using (DataTable dataTable = GetAsDataTable("Inspections", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPInspectionInformationDto);
			}
			eRPInspectionInformationDto.qapInspectionID = dataTable.Rows[0].Field<string>("qapInspectionID");
			eRPInspectionInformationDto.qapCreatedBy = dataTable.Rows[0].Field<string>("qapCreatedBy");
			eRPInspectionInformationDto.qapCreatedDate = dataTable.Rows[0].Field<DateTime?>("qapCreatedDate");
			eRPInspectionInformationDto.qapUniqueID = dataTable.Rows[0].Field<Guid>("qapUniqueID");
			eRPInspectionInformationDto.qapPosted = dataTable.Rows[0].Field<bool>("qapPosted");
			eRPInspectionInformationDto.qapReversalEntry = dataTable.Rows[0].Field<bool>("qapReversalEntry");
			eRPInspectionInformationDto.qapOpenedByEmployeeID = dataTable.Rows[0].Field<string>("qapOpenedByEmployeeID");
			eRPInspectionInformationDto.qapOpenedDate = dataTable.Rows[0].Field<DateTime?>("qapOpenedDate");
			eRPInspectionInformationDto.qapPlantDepartmentID = dataTable.Rows[0].Field<string>("qapPlantDepartmentID");
			eRPInspectionInformationDto.qapPlantID = dataTable.Rows[0].Field<string>("qapPlantID");
			eRPInspectionInformationDto.qapPostedDate = dataTable.Rows[0].Field<DateTime?>("qapPostedDate");
			eRPInspectionInformationDto.qapProjectID = dataTable.Rows[0].Field<string>("qapProjectID");
			eRPInspectionInformationDto.qapRowVersion = dataTable.Rows[0].Field<byte[]>("qapRowVersion");
			eRPInspectionInformationDto.qapSourceTableName = dataTable.Rows[0].Field<string>("qapSourceTableName");
			eRPInspectionInformationDto.qapSourceTableUniqueID = dataTable.Rows[0].Field<Guid>("qapSourceTableUniqueID");
			eRPInspectionInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPInspectionInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPInspectionInformationDto);
	}

	public Task<APIValidationInfoDto> SaveInspection(ERPInspectionDto inspection)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Inspections WHERE qapUniqueID = " + M1Util.ConvertToLinq(inspection.qapUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["qapInspectionID"] = inspection.qapInspectionID.ToUpper();
				inspection.qapUniqueID = ((inspection.qapUniqueID == Guid.Empty) ? Guid.NewGuid() : inspection.qapUniqueID);
				dataRow["qapUniqueID"] = inspection.qapUniqueID;
				dataRow["qapCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["qapCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Inspection could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (inspection.qapRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Inspection is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["qapRowVersion"], inspection.qapRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Inspection has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Inspection again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["qapPosted"] = inspection.qapPosted;
			dataRow["qapReversalEntry"] = inspection.qapReversalEntry;
			dataRow["qapOpenedByEmployeeID"] = inspection.qapOpenedByEmployeeID;
			DataRow dataRow2 = dataRow;
			DateTime? qapOpenedDate = inspection.qapOpenedDate;
			dataRow2["qapOpenedDate"] = (qapOpenedDate.HasValue ? ((object)qapOpenedDate.GetValueOrDefault()) : dataRow["qapOpenedDate"]);
			dataRow["qapPlantDepartmentID"] = inspection.qapPlantDepartmentID;
			dataRow["qapPlantID"] = inspection.qapPlantID;
			DataRow dataRow3 = dataRow;
			qapOpenedDate = inspection.qapPostedDate;
			dataRow3["qapPostedDate"] = (qapOpenedDate.HasValue ? ((object)qapOpenedDate.GetValueOrDefault()) : dataRow["qapPostedDate"]);
			dataRow["qapProjectID"] = inspection.qapProjectID;
			dataRow["qapSourceTableName"] = inspection.qapSourceTableName;
			dataRow["qapSourceTableUniqueID"] = inspection.qapSourceTableUniqueID;
			if (inspection.CustomFields != null && inspection.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in inspection.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Inspection [{inspection.qapUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Inspection [{inspection.qapUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
