using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductionCalendarDayRepository : APIBaseRepository, IERPProductionCalendarDayRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductionCalendarDayRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductionCalendarDayExist(Guid productionCalendarDayId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmyUniqueID|C", productionCalendarDayId);
		base.selectList.Add("jmyUniqueID");
		return Task.FromResult(GetAsObject("ProductionCalendarDays", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductionCalendarDayInformationDto>> GetAllProductionCalendarDays(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductionCalendarDayInformationDto> collection = new List<ERPProductionCalendarDayInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "jmyDayOfWeek", "jmyDayStartTime", "jmyHours", "jmyHoliday", "jmyPlantID", "jmyProductionCalendarDay", "jmyProductionCalendarMonth", "jmyProductionCalendarYearID", "jmyRowVersion", "jmyWorkCenterID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductionCalendarDays");
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
		using (DataTable dataTable = GetAsDataTable("ProductionCalendarDays", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductionCalendarDayInformationDto eRPProductionCalendarDayInformationDto = new ERPProductionCalendarDayInformationDto();
				eRPProductionCalendarDayInformationDto.jmyDayOfWeek = dataTable.Rows[i].Field<byte>("jmyDayOfWeek");
				eRPProductionCalendarDayInformationDto.jmyDayStartTime = dataTable.Rows[i].Field<decimal>("jmyDayStartTime");
				eRPProductionCalendarDayInformationDto.jmyHours = dataTable.Rows[i].Field<decimal>("jmyHours");
				eRPProductionCalendarDayInformationDto.jmyHoliday = dataTable.Rows[i].Field<bool>("jmyHoliday");
				eRPProductionCalendarDayInformationDto.jmyPlantID = dataTable.Rows[i].Field<string>("jmyPlantID");
				eRPProductionCalendarDayInformationDto.jmyProductionCalendarDay = dataTable.Rows[i].Field<byte>("jmyProductionCalendarDay");
				eRPProductionCalendarDayInformationDto.jmyProductionCalendarMonth = dataTable.Rows[i].Field<byte>("jmyProductionCalendarMonth");
				eRPProductionCalendarDayInformationDto.jmyProductionCalendarYearID = dataTable.Rows[i].Field<short>("jmyProductionCalendarYearID");
				eRPProductionCalendarDayInformationDto.jmyRowVersion = dataTable.Rows[i].Field<byte[]>("jmyRowVersion");
				eRPProductionCalendarDayInformationDto.jmyWorkCenterID = dataTable.Rows[i].Field<string>("jmyWorkCenterID");
				eRPProductionCalendarDayInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductionCalendarDayInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductionCalendarDayInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductionCalendarDayInformationDto> GetProductionCalendarDay(Guid productionCalendarDayId)
	{
		ERPProductionCalendarDayInformationDto eRPProductionCalendarDayInformationDto = new ERPProductionCalendarDayInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "jmyDayOfWeek", "jmyDayStartTime", "jmyHours", "jmyHoliday", "jmyPlantID", "jmyProductionCalendarDay", "jmyProductionCalendarMonth", "jmyProductionCalendarYearID", "jmyRowVersion", "jmyWorkCenterID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("jmyUniqueID|C", productionCalendarDayId);
		AddCustomFieldsToSelectList("ProductionCalendarDays");
		using (DataTable dataTable = GetAsDataTable("ProductionCalendarDays", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductionCalendarDayInformationDto);
			}
			eRPProductionCalendarDayInformationDto.jmyDayOfWeek = dataTable.Rows[0].Field<byte>("jmyDayOfWeek");
			eRPProductionCalendarDayInformationDto.jmyDayStartTime = dataTable.Rows[0].Field<decimal>("jmyDayStartTime");
			eRPProductionCalendarDayInformationDto.jmyHours = dataTable.Rows[0].Field<decimal>("jmyHours");
			eRPProductionCalendarDayInformationDto.jmyHoliday = dataTable.Rows[0].Field<bool>("jmyHoliday");
			eRPProductionCalendarDayInformationDto.jmyPlantID = dataTable.Rows[0].Field<string>("jmyPlantID");
			eRPProductionCalendarDayInformationDto.jmyProductionCalendarDay = dataTable.Rows[0].Field<byte>("jmyProductionCalendarDay");
			eRPProductionCalendarDayInformationDto.jmyProductionCalendarMonth = dataTable.Rows[0].Field<byte>("jmyProductionCalendarMonth");
			eRPProductionCalendarDayInformationDto.jmyProductionCalendarYearID = dataTable.Rows[0].Field<short>("jmyProductionCalendarYearID");
			eRPProductionCalendarDayInformationDto.jmyRowVersion = dataTable.Rows[0].Field<byte[]>("jmyRowVersion");
			eRPProductionCalendarDayInformationDto.jmyWorkCenterID = dataTable.Rows[0].Field<string>("jmyWorkCenterID");
			eRPProductionCalendarDayInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductionCalendarDayInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductionCalendarDayInformationDto);
	}
}
