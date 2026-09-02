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

public class ERPProjectedPaymentRepository : APIBaseRepository, IERPProjectedPaymentRepository, IAPIBaseRepository, IDisposable
{
	public ERPProjectedPaymentRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProjectedPaymentExist(Guid projectedPaymentId)
	{
		InitializeParameterLists();
		base.filterList.Add("gloUniqueID|C", projectedPaymentId);
		base.selectList.Add("gloUniqueID");
		return Task.FromResult(GetAsObject("ProjectedPayments", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProjectedPaymentInformationDto>> GetAllProjectedPayments(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProjectedPaymentInformationDto> collection = new List<ERPProjectedPaymentInformationDto>();
		InitializeParameterLists();
		string[] array = new string[15]
		{
			"gloAmount", "gloClosedDate", "gloCreatedBy", "gloCreatedDate", "gloDescription", "gloUniqueID", "gloIgnoreAfterDate", "gloClosed", "gloOrganizationID", "gloPaymentDate",
			"gloPaymentType", "gloPlantDepartmentID", "gloPlantID", "gloRowVersion", "gloProjectedPaymentID"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProjectedPayments");
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
		using (DataTable dataTable = GetAsDataTable("ProjectedPayments", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProjectedPaymentInformationDto eRPProjectedPaymentInformationDto = new ERPProjectedPaymentInformationDto();
				eRPProjectedPaymentInformationDto.gloAmount = dataTable.Rows[i].Field<decimal>("gloAmount");
				eRPProjectedPaymentInformationDto.gloClosedDate = dataTable.Rows[i].Field<DateTime?>("gloClosedDate");
				eRPProjectedPaymentInformationDto.gloCreatedBy = dataTable.Rows[i].Field<string>("gloCreatedBy");
				eRPProjectedPaymentInformationDto.gloCreatedDate = dataTable.Rows[i].Field<DateTime?>("gloCreatedDate");
				eRPProjectedPaymentInformationDto.gloDescription = dataTable.Rows[i].Field<string>("gloDescription");
				eRPProjectedPaymentInformationDto.gloUniqueID = dataTable.Rows[i].Field<Guid>("gloUniqueID");
				eRPProjectedPaymentInformationDto.gloIgnoreAfterDate = dataTable.Rows[i].Field<DateTime?>("gloIgnoreAfterDate");
				eRPProjectedPaymentInformationDto.gloClosed = dataTable.Rows[i].Field<bool>("gloClosed");
				eRPProjectedPaymentInformationDto.gloOrganizationID = dataTable.Rows[i].Field<string>("gloOrganizationID");
				eRPProjectedPaymentInformationDto.gloPaymentDate = dataTable.Rows[i].Field<DateTime?>("gloPaymentDate");
				eRPProjectedPaymentInformationDto.gloPaymentType = dataTable.Rows[i].Field<byte>("gloPaymentType");
				eRPProjectedPaymentInformationDto.gloPlantDepartmentID = dataTable.Rows[i].Field<string>("gloPlantDepartmentID");
				eRPProjectedPaymentInformationDto.gloPlantID = dataTable.Rows[i].Field<string>("gloPlantID");
				eRPProjectedPaymentInformationDto.gloRowVersion = dataTable.Rows[i].Field<byte[]>("gloRowVersion");
				eRPProjectedPaymentInformationDto.gloProjectedPaymentID = dataTable.Rows[i].Field<int>("gloProjectedPaymentID");
				eRPProjectedPaymentInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProjectedPaymentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProjectedPaymentInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProjectedPaymentInformationDto> GetProjectedPayment(Guid projectedPaymentId)
	{
		ERPProjectedPaymentInformationDto eRPProjectedPaymentInformationDto = new ERPProjectedPaymentInformationDto();
		InitializeParameterLists();
		string[] collection = new string[15]
		{
			"gloAmount", "gloClosedDate", "gloCreatedBy", "gloCreatedDate", "gloDescription", "gloUniqueID", "gloIgnoreAfterDate", "gloClosed", "gloOrganizationID", "gloPaymentDate",
			"gloPaymentType", "gloPlantDepartmentID", "gloPlantID", "gloRowVersion", "gloProjectedPaymentID"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("gloUniqueID|C", projectedPaymentId);
		AddCustomFieldsToSelectList("ProjectedPayments");
		using (DataTable dataTable = GetAsDataTable("ProjectedPayments", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProjectedPaymentInformationDto);
			}
			eRPProjectedPaymentInformationDto.gloAmount = dataTable.Rows[0].Field<decimal>("gloAmount");
			eRPProjectedPaymentInformationDto.gloClosedDate = dataTable.Rows[0].Field<DateTime?>("gloClosedDate");
			eRPProjectedPaymentInformationDto.gloCreatedBy = dataTable.Rows[0].Field<string>("gloCreatedBy");
			eRPProjectedPaymentInformationDto.gloCreatedDate = dataTable.Rows[0].Field<DateTime?>("gloCreatedDate");
			eRPProjectedPaymentInformationDto.gloDescription = dataTable.Rows[0].Field<string>("gloDescription");
			eRPProjectedPaymentInformationDto.gloUniqueID = dataTable.Rows[0].Field<Guid>("gloUniqueID");
			eRPProjectedPaymentInformationDto.gloIgnoreAfterDate = dataTable.Rows[0].Field<DateTime?>("gloIgnoreAfterDate");
			eRPProjectedPaymentInformationDto.gloClosed = dataTable.Rows[0].Field<bool>("gloClosed");
			eRPProjectedPaymentInformationDto.gloOrganizationID = dataTable.Rows[0].Field<string>("gloOrganizationID");
			eRPProjectedPaymentInformationDto.gloPaymentDate = dataTable.Rows[0].Field<DateTime?>("gloPaymentDate");
			eRPProjectedPaymentInformationDto.gloPaymentType = dataTable.Rows[0].Field<byte>("gloPaymentType");
			eRPProjectedPaymentInformationDto.gloPlantDepartmentID = dataTable.Rows[0].Field<string>("gloPlantDepartmentID");
			eRPProjectedPaymentInformationDto.gloPlantID = dataTable.Rows[0].Field<string>("gloPlantID");
			eRPProjectedPaymentInformationDto.gloRowVersion = dataTable.Rows[0].Field<byte[]>("gloRowVersion");
			eRPProjectedPaymentInformationDto.gloProjectedPaymentID = dataTable.Rows[0].Field<int>("gloProjectedPaymentID");
			eRPProjectedPaymentInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProjectedPaymentInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProjectedPaymentInformationDto);
	}

	public Task<APIValidationInfoDto> SaveProjectedPayment(ERPProjectedPaymentDto projectedPayment)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM ProjectedPayments WHERE gloUniqueID = " + M1Util.ConvertToLinq(projectedPayment.gloUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["gloProjectedPaymentID"] = projectedPayment.gloProjectedPaymentID;
				projectedPayment.gloUniqueID = ((projectedPayment.gloUniqueID == Guid.Empty) ? Guid.NewGuid() : projectedPayment.gloUniqueID);
				dataRow["gloUniqueID"] = projectedPayment.gloUniqueID;
				dataRow["gloCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["gloCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The ProjectedPayment could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (projectedPayment.gloRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the ProjectedPayment is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["gloRowVersion"], projectedPayment.gloRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the ProjectedPayment has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the ProjectedPayment again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["gloAmount"] = projectedPayment.gloAmount;
			DataRow dataRow2 = dataRow;
			DateTime? gloClosedDate = projectedPayment.gloClosedDate;
			dataRow2["gloClosedDate"] = (gloClosedDate.HasValue ? ((object)gloClosedDate.GetValueOrDefault()) : dataRow["gloClosedDate"]);
			dataRow["gloDescription"] = projectedPayment.gloDescription;
			DataRow dataRow3 = dataRow;
			gloClosedDate = projectedPayment.gloIgnoreAfterDate;
			dataRow3["gloIgnoreAfterDate"] = (gloClosedDate.HasValue ? ((object)gloClosedDate.GetValueOrDefault()) : dataRow["gloIgnoreAfterDate"]);
			dataRow["gloClosed"] = projectedPayment.gloClosed;
			dataRow["gloOrganizationID"] = projectedPayment.gloOrganizationID;
			DataRow dataRow4 = dataRow;
			gloClosedDate = projectedPayment.gloPaymentDate;
			dataRow4["gloPaymentDate"] = (gloClosedDate.HasValue ? ((object)gloClosedDate.GetValueOrDefault()) : dataRow["gloPaymentDate"]);
			dataRow["gloPaymentType"] = projectedPayment.gloPaymentType;
			dataRow["gloPlantDepartmentID"] = projectedPayment.gloPlantDepartmentID;
			dataRow["gloPlantID"] = projectedPayment.gloPlantID;
			if (projectedPayment.CustomFields != null && projectedPayment.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in projectedPayment.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the ProjectedPayment [{projectedPayment.gloUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the ProjectedPayment [{projectedPayment.gloUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
