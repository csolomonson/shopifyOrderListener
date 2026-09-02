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

public class ERPEmployeeRepository : APIBaseRepository, IERPEmployeeRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeExist(Guid employeeId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmeUniqueID|C", employeeId);
		base.selectList.Add("lmeUniqueID");
		return Task.FromResult(GetAsObject("Employees", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeInformationDto>> GetAllEmployees(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeInformationDto> collection = new List<ERPEmployeeInformationDto>();
		InitializeParameterLists();
		string[] array = new string[43]
		{
			"lmeCallTypeID", "lmeCessationType", "lmeEmployeeID", "lmeCommissionRate", "lmeContactTitleID", "lmeCountyCodeID", "lmeCreatedBy", "lmeCreatedDate", "lmeDefaultShiftID", "lmeDefaultWorkCenterID",
			"lmeDirectExpenseID", "lmeEarningType", "lmeEmployeeName", "lmeUniqueID", "lmeHireDate", "lmeHomeProductionDepartmentID", "lmeIndirectExpenseID", "lmeBuyerEmployee", "lmeEngineerEmployee", "lmeInspectorEmployee",
			"lmeLockShift", "lmePayrollEmployee", "lmePlannerEmployee", "lmeProjectManagerEmployee", "lmeQuoterEmployee", "lmeSalesEmployee", "lmeShopEmployee", "lmeSortSfebyWorkcenter", "lmeSupportEmployee", "lmeLanguage",
			"lmePassword", "lmePlantDepartmentID", "lmePlantID", "lmePoApprovalAmount", "lmePreviousEmployeeID", "lmeRowVersion", "lmeSOApprovalAmount", "lmeTerminationDate", "lmeTerminationReasonID", "lmeUseEmail",
			"lmeUseEmailPayslips", "lmeUserID", "lmeWorkEmailAddress"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Employees");
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
		using (DataTable dataTable = GetAsDataTable("Employees", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeInformationDto eRPEmployeeInformationDto = new ERPEmployeeInformationDto();
				eRPEmployeeInformationDto.lmeCallTypeID = dataTable.Rows[i].Field<string>("lmeCallTypeID");
				eRPEmployeeInformationDto.lmeCessationType = dataTable.Rows[i].Field<string>("lmeCessationType");
				eRPEmployeeInformationDto.lmeEmployeeID = dataTable.Rows[i].Field<string>("lmeEmployeeID");
				eRPEmployeeInformationDto.lmeCommissionRate = dataTable.Rows[i].Field<decimal>("lmeCommissionRate");
				eRPEmployeeInformationDto.lmeContactTitleID = dataTable.Rows[i].Field<string>("lmeContactTitleID");
				eRPEmployeeInformationDto.lmeCountyCodeID = dataTable.Rows[i].Field<string>("lmeCountyCodeID");
				eRPEmployeeInformationDto.lmeCreatedBy = dataTable.Rows[i].Field<string>("lmeCreatedBy");
				eRPEmployeeInformationDto.lmeCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmeCreatedDate");
				eRPEmployeeInformationDto.lmeDefaultShiftID = dataTable.Rows[i].Field<short>("lmeDefaultShiftID");
				eRPEmployeeInformationDto.lmeDefaultWorkCenterID = dataTable.Rows[i].Field<string>("lmeDefaultWorkCenterID");
				eRPEmployeeInformationDto.lmeDirectExpenseID = dataTable.Rows[i].Field<string>("lmeDirectExpenseID");
				eRPEmployeeInformationDto.lmeEarningType = dataTable.Rows[i].Field<byte>("lmeEarningType");
				eRPEmployeeInformationDto.lmeEmployeeName = dataTable.Rows[i].Field<string>("lmeEmployeeName");
				eRPEmployeeInformationDto.lmeUniqueID = dataTable.Rows[i].Field<Guid>("lmeUniqueID");
				eRPEmployeeInformationDto.lmeHireDate = dataTable.Rows[i].Field<DateTime?>("lmeHireDate");
				eRPEmployeeInformationDto.lmeHomeProductionDepartmentID = dataTable.Rows[i].Field<string>("lmeHomeProductionDepartmentID");
				eRPEmployeeInformationDto.lmeIndirectExpenseID = dataTable.Rows[i].Field<string>("lmeIndirectExpenseID");
				eRPEmployeeInformationDto.lmeBuyerEmployee = dataTable.Rows[i].Field<bool>("lmeBuyerEmployee");
				eRPEmployeeInformationDto.lmeEngineerEmployee = dataTable.Rows[i].Field<bool>("lmeEngineerEmployee");
				eRPEmployeeInformationDto.lmeInspectorEmployee = dataTable.Rows[i].Field<bool>("lmeInspectorEmployee");
				eRPEmployeeInformationDto.lmeLockShift = dataTable.Rows[i].Field<bool>("lmeLockShift");
				eRPEmployeeInformationDto.lmePayrollEmployee = dataTable.Rows[i].Field<bool>("lmePayrollEmployee");
				eRPEmployeeInformationDto.lmePlannerEmployee = dataTable.Rows[i].Field<bool>("lmePlannerEmployee");
				eRPEmployeeInformationDto.lmeProjectManagerEmployee = dataTable.Rows[i].Field<bool>("lmeProjectManagerEmployee");
				eRPEmployeeInformationDto.lmeQuoterEmployee = dataTable.Rows[i].Field<bool>("lmeQuoterEmployee");
				eRPEmployeeInformationDto.lmeSalesEmployee = dataTable.Rows[i].Field<bool>("lmeSalesEmployee");
				eRPEmployeeInformationDto.lmeShopEmployee = dataTable.Rows[i].Field<bool>("lmeShopEmployee");
				eRPEmployeeInformationDto.lmeSortSfebyWorkcenter = dataTable.Rows[i].Field<bool>("lmeSortSfebyWorkcenter");
				eRPEmployeeInformationDto.lmeSupportEmployee = dataTable.Rows[i].Field<bool>("lmeSupportEmployee");
				eRPEmployeeInformationDto.lmeLanguage = dataTable.Rows[i].Field<string>("lmeLanguage");
				eRPEmployeeInformationDto.lmePassword = dataTable.Rows[i].Field<string>("lmePassword");
				eRPEmployeeInformationDto.lmePlantDepartmentID = dataTable.Rows[i].Field<string>("lmePlantDepartmentID");
				eRPEmployeeInformationDto.lmePlantID = dataTable.Rows[i].Field<string>("lmePlantID");
				eRPEmployeeInformationDto.lmePoApprovalAmount = dataTable.Rows[i].Field<decimal>("lmePoApprovalAmount");
				eRPEmployeeInformationDto.lmePreviousEmployeeID = dataTable.Rows[i].Field<string>("lmePreviousEmployeeID");
				eRPEmployeeInformationDto.lmeRowVersion = dataTable.Rows[i].Field<byte[]>("lmeRowVersion");
				eRPEmployeeInformationDto.lmeSOApprovalAmount = dataTable.Rows[i].Field<decimal>("lmeSOApprovalAmount");
				eRPEmployeeInformationDto.lmeTerminationDate = dataTable.Rows[i].Field<DateTime?>("lmeTerminationDate");
				eRPEmployeeInformationDto.lmeTerminationReasonID = dataTable.Rows[i].Field<string>("lmeTerminationReasonID");
				eRPEmployeeInformationDto.lmeUseEmail = dataTable.Rows[i].Field<byte>("lmeUseEmail");
				eRPEmployeeInformationDto.lmeUseEmailPayslips = dataTable.Rows[i].Field<byte>("lmeUseEmailPayslips");
				eRPEmployeeInformationDto.lmeUserID = dataTable.Rows[i].Field<string>("lmeUserID");
				eRPEmployeeInformationDto.lmeWorkEmailAddress = dataTable.Rows[i].Field<string>("lmeWorkEmailAddress");
				eRPEmployeeInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeInformationDto> GetEmployee(Guid employeeId)
	{
		ERPEmployeeInformationDto eRPEmployeeInformationDto = new ERPEmployeeInformationDto();
		InitializeParameterLists();
		string[] collection = new string[43]
		{
			"lmeCallTypeID", "lmeCessationType", "lmeEmployeeID", "lmeCommissionRate", "lmeContactTitleID", "lmeCountyCodeID", "lmeCreatedBy", "lmeCreatedDate", "lmeDefaultShiftID", "lmeDefaultWorkCenterID",
			"lmeDirectExpenseID", "lmeEarningType", "lmeEmployeeName", "lmeUniqueID", "lmeHireDate", "lmeHomeProductionDepartmentID", "lmeIndirectExpenseID", "lmeBuyerEmployee", "lmeEngineerEmployee", "lmeInspectorEmployee",
			"lmeLockShift", "lmePayrollEmployee", "lmePlannerEmployee", "lmeProjectManagerEmployee", "lmeQuoterEmployee", "lmeSalesEmployee", "lmeShopEmployee", "lmeSortSfebyWorkcenter", "lmeSupportEmployee", "lmeLanguage",
			"lmePassword", "lmePlantDepartmentID", "lmePlantID", "lmePoApprovalAmount", "lmePreviousEmployeeID", "lmeRowVersion", "lmeSOApprovalAmount", "lmeTerminationDate", "lmeTerminationReasonID", "lmeUseEmail",
			"lmeUseEmailPayslips", "lmeUserID", "lmeWorkEmailAddress"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmeUniqueID|C", employeeId);
		AddCustomFieldsToSelectList("Employees");
		using (DataTable dataTable = GetAsDataTable("Employees", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeInformationDto);
			}
			eRPEmployeeInformationDto.lmeCallTypeID = dataTable.Rows[0].Field<string>("lmeCallTypeID");
			eRPEmployeeInformationDto.lmeCessationType = dataTable.Rows[0].Field<string>("lmeCessationType");
			eRPEmployeeInformationDto.lmeEmployeeID = dataTable.Rows[0].Field<string>("lmeEmployeeID");
			eRPEmployeeInformationDto.lmeCommissionRate = dataTable.Rows[0].Field<decimal>("lmeCommissionRate");
			eRPEmployeeInformationDto.lmeContactTitleID = dataTable.Rows[0].Field<string>("lmeContactTitleID");
			eRPEmployeeInformationDto.lmeCountyCodeID = dataTable.Rows[0].Field<string>("lmeCountyCodeID");
			eRPEmployeeInformationDto.lmeCreatedBy = dataTable.Rows[0].Field<string>("lmeCreatedBy");
			eRPEmployeeInformationDto.lmeCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmeCreatedDate");
			eRPEmployeeInformationDto.lmeDefaultShiftID = dataTable.Rows[0].Field<short>("lmeDefaultShiftID");
			eRPEmployeeInformationDto.lmeDefaultWorkCenterID = dataTable.Rows[0].Field<string>("lmeDefaultWorkCenterID");
			eRPEmployeeInformationDto.lmeDirectExpenseID = dataTable.Rows[0].Field<string>("lmeDirectExpenseID");
			eRPEmployeeInformationDto.lmeEarningType = dataTable.Rows[0].Field<byte>("lmeEarningType");
			eRPEmployeeInformationDto.lmeEmployeeName = dataTable.Rows[0].Field<string>("lmeEmployeeName");
			eRPEmployeeInformationDto.lmeUniqueID = dataTable.Rows[0].Field<Guid>("lmeUniqueID");
			eRPEmployeeInformationDto.lmeHireDate = dataTable.Rows[0].Field<DateTime?>("lmeHireDate");
			eRPEmployeeInformationDto.lmeHomeProductionDepartmentID = dataTable.Rows[0].Field<string>("lmeHomeProductionDepartmentID");
			eRPEmployeeInformationDto.lmeIndirectExpenseID = dataTable.Rows[0].Field<string>("lmeIndirectExpenseID");
			eRPEmployeeInformationDto.lmeBuyerEmployee = dataTable.Rows[0].Field<bool>("lmeBuyerEmployee");
			eRPEmployeeInformationDto.lmeEngineerEmployee = dataTable.Rows[0].Field<bool>("lmeEngineerEmployee");
			eRPEmployeeInformationDto.lmeInspectorEmployee = dataTable.Rows[0].Field<bool>("lmeInspectorEmployee");
			eRPEmployeeInformationDto.lmeLockShift = dataTable.Rows[0].Field<bool>("lmeLockShift");
			eRPEmployeeInformationDto.lmePayrollEmployee = dataTable.Rows[0].Field<bool>("lmePayrollEmployee");
			eRPEmployeeInformationDto.lmePlannerEmployee = dataTable.Rows[0].Field<bool>("lmePlannerEmployee");
			eRPEmployeeInformationDto.lmeProjectManagerEmployee = dataTable.Rows[0].Field<bool>("lmeProjectManagerEmployee");
			eRPEmployeeInformationDto.lmeQuoterEmployee = dataTable.Rows[0].Field<bool>("lmeQuoterEmployee");
			eRPEmployeeInformationDto.lmeSalesEmployee = dataTable.Rows[0].Field<bool>("lmeSalesEmployee");
			eRPEmployeeInformationDto.lmeShopEmployee = dataTable.Rows[0].Field<bool>("lmeShopEmployee");
			eRPEmployeeInformationDto.lmeSortSfebyWorkcenter = dataTable.Rows[0].Field<bool>("lmeSortSfebyWorkcenter");
			eRPEmployeeInformationDto.lmeSupportEmployee = dataTable.Rows[0].Field<bool>("lmeSupportEmployee");
			eRPEmployeeInformationDto.lmeLanguage = dataTable.Rows[0].Field<string>("lmeLanguage");
			eRPEmployeeInformationDto.lmePassword = dataTable.Rows[0].Field<string>("lmePassword");
			eRPEmployeeInformationDto.lmePlantDepartmentID = dataTable.Rows[0].Field<string>("lmePlantDepartmentID");
			eRPEmployeeInformationDto.lmePlantID = dataTable.Rows[0].Field<string>("lmePlantID");
			eRPEmployeeInformationDto.lmePoApprovalAmount = dataTable.Rows[0].Field<decimal>("lmePoApprovalAmount");
			eRPEmployeeInformationDto.lmePreviousEmployeeID = dataTable.Rows[0].Field<string>("lmePreviousEmployeeID");
			eRPEmployeeInformationDto.lmeRowVersion = dataTable.Rows[0].Field<byte[]>("lmeRowVersion");
			eRPEmployeeInformationDto.lmeSOApprovalAmount = dataTable.Rows[0].Field<decimal>("lmeSOApprovalAmount");
			eRPEmployeeInformationDto.lmeTerminationDate = dataTable.Rows[0].Field<DateTime?>("lmeTerminationDate");
			eRPEmployeeInformationDto.lmeTerminationReasonID = dataTable.Rows[0].Field<string>("lmeTerminationReasonID");
			eRPEmployeeInformationDto.lmeUseEmail = dataTable.Rows[0].Field<byte>("lmeUseEmail");
			eRPEmployeeInformationDto.lmeUseEmailPayslips = dataTable.Rows[0].Field<byte>("lmeUseEmailPayslips");
			eRPEmployeeInformationDto.lmeUserID = dataTable.Rows[0].Field<string>("lmeUserID");
			eRPEmployeeInformationDto.lmeWorkEmailAddress = dataTable.Rows[0].Field<string>("lmeWorkEmailAddress");
			eRPEmployeeInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeInformationDto);
	}

	public Task<APIValidationInfoDto> SaveEmployee(ERPEmployeeDto employee)
	{
		APIValidationInfoDto aPIValidationInfoDto = new APIValidationInfoDto();
		bool flag = false;
		try
		{
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataTable dataTable = base.M1database.GetDataTable("SELECT * FROM Employees WHERE lmeUniqueID = " + M1Util.ConvertToLinq(employee.lmeUniqueID), fillSchema: false, out adapter);
			DataRow dataRow;
			if (dataTable.Rows.Count == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow["lmeEmployeeID"] = employee.lmeEmployeeID.ToUpper();
				employee.lmeUniqueID = ((employee.lmeUniqueID == Guid.Empty) ? Guid.NewGuid() : employee.lmeUniqueID);
				dataRow["lmeUniqueID"] = employee.lmeUniqueID;
				dataRow["lmeCreatedBy"] = "API-" + base.ApiID.Substring(0, 8);
				dataRow["lmeCreatedDate"] = DateTime.Now;
				flag = true;
			}
			else
			{
				dataRow = dataTable.Rows[0];
				if (dataRow == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The Employee could not be found.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.NotFound;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (employee.lmeRowVersion == null)
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version passed in the Employee is null. Please pass in the current row version to continue with the request.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.BadRequest;
					return Task.FromResult(aPIValidationInfoDto);
				}
				if (!ConcurrencyHelpers.AreRowVersionsEqual((byte[])dataRow["lmeRowVersion"], employee.lmeRowVersion))
				{
					aPIValidationInfoDto.ErrorsList.Add("The row version of the Employee has changed. This is likely an indication that another user has modified this record since you retrieved it. Please retrieve the Employee again and include the current row version and try again.");
					aPIValidationInfoDto.HttpValidationStatusCode = HttpStatusCode.Conflict;
					return Task.FromResult(aPIValidationInfoDto);
				}
			}
			dataRow.BeginEdit();
			dataRow["lmeCallTypeID"] = employee.lmeCallTypeID;
			dataRow["lmeCessationType"] = employee.lmeCessationType;
			dataRow["lmeCommissionRate"] = employee.lmeCommissionRate;
			dataRow["lmeContactTitleID"] = employee.lmeContactTitleID;
			dataRow["lmeCountyCodeID"] = employee.lmeCountyCodeID;
			dataRow["lmeDefaultShiftID"] = employee.lmeDefaultShiftID;
			dataRow["lmeDefaultWorkCenterID"] = employee.lmeDefaultWorkCenterID;
			dataRow["lmeDirectExpenseID"] = employee.lmeDirectExpenseID;
			dataRow["lmeEarningType"] = employee.lmeEarningType;
			dataRow["lmeEmployeeName"] = employee.lmeEmployeeName;
			DataRow dataRow2 = dataRow;
			DateTime? lmeHireDate = employee.lmeHireDate;
			dataRow2["lmeHireDate"] = (lmeHireDate.HasValue ? ((object)lmeHireDate.GetValueOrDefault()) : dataRow["lmeHireDate"]);
			dataRow["lmeHomeProductionDepartmentID"] = employee.lmeHomeProductionDepartmentID;
			dataRow["lmeIndirectExpenseID"] = employee.lmeIndirectExpenseID;
			dataRow["lmeBuyerEmployee"] = employee.lmeBuyerEmployee;
			dataRow["lmeEngineerEmployee"] = employee.lmeEngineerEmployee;
			dataRow["lmeInspectorEmployee"] = employee.lmeInspectorEmployee;
			dataRow["lmeLockShift"] = employee.lmeLockShift;
			dataRow["lmePayrollEmployee"] = employee.lmePayrollEmployee;
			dataRow["lmePlannerEmployee"] = employee.lmePlannerEmployee;
			dataRow["lmeProjectManagerEmployee"] = employee.lmeProjectManagerEmployee;
			dataRow["lmeQuoterEmployee"] = employee.lmeQuoterEmployee;
			dataRow["lmeSalesEmployee"] = employee.lmeSalesEmployee;
			dataRow["lmeShopEmployee"] = employee.lmeShopEmployee;
			dataRow["lmeSortSfebyWorkcenter"] = employee.lmeSortSfebyWorkcenter;
			dataRow["lmeSupportEmployee"] = employee.lmeSupportEmployee;
			dataRow["lmeLanguage"] = employee.lmeLanguage;
			dataRow["lmePassword"] = employee.lmePassword;
			dataRow["lmePlantDepartmentID"] = employee.lmePlantDepartmentID;
			dataRow["lmePlantID"] = employee.lmePlantID;
			dataRow["lmePoApprovalAmount"] = employee.lmePoApprovalAmount;
			dataRow["lmePreviousEmployeeID"] = employee.lmePreviousEmployeeID;
			dataRow["lmeSOApprovalAmount"] = employee.lmeSOApprovalAmount;
			DataRow dataRow3 = dataRow;
			lmeHireDate = employee.lmeTerminationDate;
			dataRow3["lmeTerminationDate"] = (lmeHireDate.HasValue ? ((object)lmeHireDate.GetValueOrDefault()) : dataRow["lmeTerminationDate"]);
			dataRow["lmeTerminationReasonID"] = employee.lmeTerminationReasonID;
			dataRow["lmeUseEmail"] = employee.lmeUseEmail;
			dataRow["lmeUseEmailPayslips"] = employee.lmeUseEmailPayslips;
			dataRow["lmeUserID"] = employee.lmeUserID;
			dataRow["lmeWorkEmailAddress"] = employee.lmeWorkEmailAddress ?? dataRow["lmeWorkEmailAddress"];
			if (employee.CustomFields != null && employee.CustomFields.Any())
			{
				foreach (KeyValuePair<string, object> customField in employee.CustomFields)
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
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{httpStatusCodeForSqlException.ErrorDescription}] while processing the Employee [{employee.lmeUniqueID}]" }, null, httpStatusCodeForSqlException.StatusCode);
		}
		catch (Exception ex2)
		{
			aPIValidationInfoDto = new APIValidationInfoDto(new List<string> { $"Error occurred [{ex2.Message}] while processing the Employee [{employee.lmeUniqueID}]" }, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(aPIValidationInfoDto);
	}
}
