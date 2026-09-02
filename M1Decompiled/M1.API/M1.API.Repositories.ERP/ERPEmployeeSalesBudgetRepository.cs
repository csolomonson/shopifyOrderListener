using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPEmployeeSalesBudgetRepository : APIBaseRepository, IERPEmployeeSalesBudgetRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeSalesBudgetRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeSalesBudgetExist(Guid employeeSalesBudgetId)
	{
		InitializeParameterLists();
		base.filterList.Add("lnsUniqueID|C", employeeSalesBudgetId);
		base.selectList.Add("lnsUniqueID");
		return Task.FromResult(GetAsObject("EmployeeSalesBudgets", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeSalesBudgetInformationDto>> GetAllEmployeeSalesBudgets(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeSalesBudgetInformationDto> collection = new List<ERPEmployeeSalesBudgetInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "lnsAnnualAmount", "lnsEmployeeID", "lnsEndDate", "lnsUniqueID", "lnsRowVersion", "lnsSalesBudgetYearID", "lnsStartDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeSalesBudgets");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeSalesBudgets", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeSalesBudgetInformationDto eRPEmployeeSalesBudgetInformationDto = new ERPEmployeeSalesBudgetInformationDto();
				eRPEmployeeSalesBudgetInformationDto.lnsAnnualAmount = dataTable.Rows[i].Field<decimal>("lnsAnnualAmount");
				eRPEmployeeSalesBudgetInformationDto.lnsEmployeeID = dataTable.Rows[i].Field<string>("lnsEmployeeID");
				eRPEmployeeSalesBudgetInformationDto.lnsEndDate = dataTable.Rows[i].Field<DateTime?>("lnsEndDate");
				eRPEmployeeSalesBudgetInformationDto.lnsUniqueID = dataTable.Rows[i].Field<Guid>("lnsUniqueID");
				eRPEmployeeSalesBudgetInformationDto.lnsRowVersion = dataTable.Rows[i].Field<byte[]>("lnsRowVersion");
				eRPEmployeeSalesBudgetInformationDto.lnsSalesBudgetYearID = dataTable.Rows[i].Field<short>("lnsSalesBudgetYearID");
				eRPEmployeeSalesBudgetInformationDto.lnsStartDate = dataTable.Rows[i].Field<DateTime?>("lnsStartDate");
				eRPEmployeeSalesBudgetInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeSalesBudgetInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeSalesBudgetInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeSalesBudgetInformationDto> GetEmployeeSalesBudget(Guid employeeSalesBudgetId)
	{
		ERPEmployeeSalesBudgetInformationDto eRPEmployeeSalesBudgetInformationDto = new ERPEmployeeSalesBudgetInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "lnsAnnualAmount", "lnsEmployeeID", "lnsEndDate", "lnsUniqueID", "lnsRowVersion", "lnsSalesBudgetYearID", "lnsStartDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lnsUniqueID|C", employeeSalesBudgetId);
		AddCustomFieldsToSelectList("EmployeeSalesBudgets");
		using (DataTable dataTable = GetAsDataTable("EmployeeSalesBudgets", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeSalesBudgetInformationDto);
			}
			eRPEmployeeSalesBudgetInformationDto.lnsAnnualAmount = dataTable.Rows[0].Field<decimal>("lnsAnnualAmount");
			eRPEmployeeSalesBudgetInformationDto.lnsEmployeeID = dataTable.Rows[0].Field<string>("lnsEmployeeID");
			eRPEmployeeSalesBudgetInformationDto.lnsEndDate = dataTable.Rows[0].Field<DateTime?>("lnsEndDate");
			eRPEmployeeSalesBudgetInformationDto.lnsUniqueID = dataTable.Rows[0].Field<Guid>("lnsUniqueID");
			eRPEmployeeSalesBudgetInformationDto.lnsRowVersion = dataTable.Rows[0].Field<byte[]>("lnsRowVersion");
			eRPEmployeeSalesBudgetInformationDto.lnsSalesBudgetYearID = dataTable.Rows[0].Field<short>("lnsSalesBudgetYearID");
			eRPEmployeeSalesBudgetInformationDto.lnsStartDate = dataTable.Rows[0].Field<DateTime?>("lnsStartDate");
			eRPEmployeeSalesBudgetInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeSalesBudgetInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeSalesBudgetInformationDto);
	}
}
