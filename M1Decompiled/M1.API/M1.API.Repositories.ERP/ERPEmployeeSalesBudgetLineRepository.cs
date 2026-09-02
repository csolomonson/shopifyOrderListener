using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPEmployeeSalesBudgetLineRepository : APIBaseRepository, IERPEmployeeSalesBudgetLineRepository, IAPIBaseRepository, IDisposable
{
	public ERPEmployeeSalesBudgetLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesEmployeeSalesBudgetLineExist(Guid employeeSalesBudgetLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("lnlUniqueID|C", employeeSalesBudgetLineId);
		base.selectList.Add("lnlUniqueID");
		return Task.FromResult(GetAsObject("EmployeeSalesBudgetLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPEmployeeSalesBudgetLineInformationDto>> GetAllEmployeeSalesBudgetLines(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPEmployeeSalesBudgetLineInformationDto> collection = new List<ERPEmployeeSalesBudgetLineInformationDto>();
		InitializeParameterLists();
		string[] array = new string[8] { "lnlBudgetAmount", "lnlEmployeeID", "lnlEndDate", "lnlUniqueID", "lnlRowVersion", "lnlSalesBudgetPeriodID", "lnlSalesBudgetYearID", "lnlStartDate" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("EmployeeSalesBudgetLines");
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
		using (DataTable dataTable = GetAsDataTable("EmployeeSalesBudgetLines", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPEmployeeSalesBudgetLineInformationDto eRPEmployeeSalesBudgetLineInformationDto = new ERPEmployeeSalesBudgetLineInformationDto();
				eRPEmployeeSalesBudgetLineInformationDto.lnlBudgetAmount = dataTable.Rows[i].Field<decimal>("lnlBudgetAmount");
				eRPEmployeeSalesBudgetLineInformationDto.lnlEmployeeID = dataTable.Rows[i].Field<string>("lnlEmployeeID");
				eRPEmployeeSalesBudgetLineInformationDto.lnlEndDate = dataTable.Rows[i].Field<DateTime?>("lnlEndDate");
				eRPEmployeeSalesBudgetLineInformationDto.lnlUniqueID = dataTable.Rows[i].Field<Guid>("lnlUniqueID");
				eRPEmployeeSalesBudgetLineInformationDto.lnlRowVersion = dataTable.Rows[i].Field<byte[]>("lnlRowVersion");
				eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetPeriodID = dataTable.Rows[i].Field<short>("lnlSalesBudgetPeriodID");
				eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetYearID = dataTable.Rows[i].Field<short>("lnlSalesBudgetYearID");
				eRPEmployeeSalesBudgetLineInformationDto.lnlStartDate = dataTable.Rows[i].Field<DateTime?>("lnlStartDate");
				eRPEmployeeSalesBudgetLineInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPEmployeeSalesBudgetLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPEmployeeSalesBudgetLineInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPEmployeeSalesBudgetLineInformationDto> GetEmployeeSalesBudgetLine(Guid employeeSalesBudgetLineId)
	{
		ERPEmployeeSalesBudgetLineInformationDto eRPEmployeeSalesBudgetLineInformationDto = new ERPEmployeeSalesBudgetLineInformationDto();
		InitializeParameterLists();
		string[] collection = new string[8] { "lnlBudgetAmount", "lnlEmployeeID", "lnlEndDate", "lnlUniqueID", "lnlRowVersion", "lnlSalesBudgetPeriodID", "lnlSalesBudgetYearID", "lnlStartDate" };
		base.selectList.AddRange(collection);
		base.filterList.Add("lnlUniqueID|C", employeeSalesBudgetLineId);
		AddCustomFieldsToSelectList("EmployeeSalesBudgetLines");
		using (DataTable dataTable = GetAsDataTable("EmployeeSalesBudgetLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPEmployeeSalesBudgetLineInformationDto);
			}
			eRPEmployeeSalesBudgetLineInformationDto.lnlBudgetAmount = dataTable.Rows[0].Field<decimal>("lnlBudgetAmount");
			eRPEmployeeSalesBudgetLineInformationDto.lnlEmployeeID = dataTable.Rows[0].Field<string>("lnlEmployeeID");
			eRPEmployeeSalesBudgetLineInformationDto.lnlEndDate = dataTable.Rows[0].Field<DateTime?>("lnlEndDate");
			eRPEmployeeSalesBudgetLineInformationDto.lnlUniqueID = dataTable.Rows[0].Field<Guid>("lnlUniqueID");
			eRPEmployeeSalesBudgetLineInformationDto.lnlRowVersion = dataTable.Rows[0].Field<byte[]>("lnlRowVersion");
			eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetPeriodID = dataTable.Rows[0].Field<short>("lnlSalesBudgetPeriodID");
			eRPEmployeeSalesBudgetLineInformationDto.lnlSalesBudgetYearID = dataTable.Rows[0].Field<short>("lnlSalesBudgetYearID");
			eRPEmployeeSalesBudgetLineInformationDto.lnlStartDate = dataTable.Rows[0].Field<DateTime?>("lnlStartDate");
			eRPEmployeeSalesBudgetLineInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPEmployeeSalesBudgetLineInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPEmployeeSalesBudgetLineInformationDto);
	}
}
