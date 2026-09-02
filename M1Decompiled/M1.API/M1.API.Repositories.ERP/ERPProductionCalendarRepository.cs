using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductionCalendarRepository : APIBaseRepository, IERPProductionCalendarRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductionCalendarRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductionCalendarExist(Guid productionCalendarId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmlUniqueID|C", productionCalendarId);
		base.selectList.Add("jmlUniqueID");
		return Task.FromResult(GetAsObject("ProductionCalendars", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductionCalendarInformationDto>> GetAllProductionCalendars(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductionCalendarInformationDto> collection = new List<ERPProductionCalendarInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "jmlCreatedBy", "jmlCreatedDate", "jmlUniqueID", "jmlPlantID", "jmlProductionCalendarYearID", "jmlRowVersion", "jmlWorkCenterID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductionCalendars");
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
		using (DataTable dataTable = GetAsDataTable("ProductionCalendars", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductionCalendarInformationDto eRPProductionCalendarInformationDto = new ERPProductionCalendarInformationDto();
				eRPProductionCalendarInformationDto.jmlCreatedBy = dataTable.Rows[i].Field<string>("jmlCreatedBy");
				eRPProductionCalendarInformationDto.jmlCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmlCreatedDate");
				eRPProductionCalendarInformationDto.jmlUniqueID = dataTable.Rows[i].Field<Guid>("jmlUniqueID");
				eRPProductionCalendarInformationDto.jmlPlantID = dataTable.Rows[i].Field<string>("jmlPlantID");
				eRPProductionCalendarInformationDto.jmlProductionCalendarYearID = dataTable.Rows[i].Field<short>("jmlProductionCalendarYearID");
				eRPProductionCalendarInformationDto.jmlRowVersion = dataTable.Rows[i].Field<byte[]>("jmlRowVersion");
				eRPProductionCalendarInformationDto.jmlWorkCenterID = dataTable.Rows[i].Field<string>("jmlWorkCenterID");
				eRPProductionCalendarInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductionCalendarInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductionCalendarInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductionCalendarInformationDto> GetProductionCalendar(Guid productionCalendarId)
	{
		ERPProductionCalendarInformationDto eRPProductionCalendarInformationDto = new ERPProductionCalendarInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "jmlCreatedBy", "jmlCreatedDate", "jmlUniqueID", "jmlPlantID", "jmlProductionCalendarYearID", "jmlRowVersion", "jmlWorkCenterID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("jmlUniqueID|C", productionCalendarId);
		AddCustomFieldsToSelectList("ProductionCalendars");
		using (DataTable dataTable = GetAsDataTable("ProductionCalendars", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductionCalendarInformationDto);
			}
			eRPProductionCalendarInformationDto.jmlCreatedBy = dataTable.Rows[0].Field<string>("jmlCreatedBy");
			eRPProductionCalendarInformationDto.jmlCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmlCreatedDate");
			eRPProductionCalendarInformationDto.jmlUniqueID = dataTable.Rows[0].Field<Guid>("jmlUniqueID");
			eRPProductionCalendarInformationDto.jmlPlantID = dataTable.Rows[0].Field<string>("jmlPlantID");
			eRPProductionCalendarInformationDto.jmlProductionCalendarYearID = dataTable.Rows[0].Field<short>("jmlProductionCalendarYearID");
			eRPProductionCalendarInformationDto.jmlRowVersion = dataTable.Rows[0].Field<byte[]>("jmlRowVersion");
			eRPProductionCalendarInformationDto.jmlWorkCenterID = dataTable.Rows[0].Field<string>("jmlWorkCenterID");
			eRPProductionCalendarInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductionCalendarInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductionCalendarInformationDto);
	}
}
