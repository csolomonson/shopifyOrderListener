using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPProductionCalendarWorkCenterRepository : APIBaseRepository, IERPProductionCalendarWorkCenterRepository, IAPIBaseRepository, IDisposable
{
	public ERPProductionCalendarWorkCenterRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesProductionCalendarWorkCenterExist(Guid productionCalendarWorkCenterId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmrUniqueID|C", productionCalendarWorkCenterId);
		base.selectList.Add("jmrUniqueID");
		return Task.FromResult(GetAsObject("ProductionCalendarWorkCenters", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPProductionCalendarWorkCenterInformationDto>> GetAllProductionCalendarWorkCenters(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPProductionCalendarWorkCenterInformationDto> collection = new List<ERPProductionCalendarWorkCenterInformationDto>();
		InitializeParameterLists();
		string[] array = new string[7] { "jmrCreatedBy", "jmrCreatedDate", "jmrUniqueID", "jmrProductionCalendarLineID", "jmrProductionCalendarYearID", "jmrRowVersion", "jmrWorkCenterID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ProductionCalendarWorkCenters");
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
		using (DataTable dataTable = GetAsDataTable("ProductionCalendarWorkCenters", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPProductionCalendarWorkCenterInformationDto eRPProductionCalendarWorkCenterInformationDto = new ERPProductionCalendarWorkCenterInformationDto();
				eRPProductionCalendarWorkCenterInformationDto.jmrCreatedBy = dataTable.Rows[i].Field<string>("jmrCreatedBy");
				eRPProductionCalendarWorkCenterInformationDto.jmrCreatedDate = dataTable.Rows[i].Field<DateTime?>("jmrCreatedDate");
				eRPProductionCalendarWorkCenterInformationDto.jmrUniqueID = dataTable.Rows[i].Field<Guid>("jmrUniqueID");
				eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarLineID = dataTable.Rows[i].Field<short>("jmrProductionCalendarLineID");
				eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarYearID = dataTable.Rows[i].Field<short>("jmrProductionCalendarYearID");
				eRPProductionCalendarWorkCenterInformationDto.jmrRowVersion = dataTable.Rows[i].Field<byte[]>("jmrRowVersion");
				eRPProductionCalendarWorkCenterInformationDto.jmrWorkCenterID = dataTable.Rows[i].Field<string>("jmrWorkCenterID");
				eRPProductionCalendarWorkCenterInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPProductionCalendarWorkCenterInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPProductionCalendarWorkCenterInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPProductionCalendarWorkCenterInformationDto> GetProductionCalendarWorkCenter(Guid productionCalendarWorkCenterId)
	{
		ERPProductionCalendarWorkCenterInformationDto eRPProductionCalendarWorkCenterInformationDto = new ERPProductionCalendarWorkCenterInformationDto();
		InitializeParameterLists();
		string[] collection = new string[7] { "jmrCreatedBy", "jmrCreatedDate", "jmrUniqueID", "jmrProductionCalendarLineID", "jmrProductionCalendarYearID", "jmrRowVersion", "jmrWorkCenterID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("jmrUniqueID|C", productionCalendarWorkCenterId);
		AddCustomFieldsToSelectList("ProductionCalendarWorkCenters");
		using (DataTable dataTable = GetAsDataTable("ProductionCalendarWorkCenters", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPProductionCalendarWorkCenterInformationDto);
			}
			eRPProductionCalendarWorkCenterInformationDto.jmrCreatedBy = dataTable.Rows[0].Field<string>("jmrCreatedBy");
			eRPProductionCalendarWorkCenterInformationDto.jmrCreatedDate = dataTable.Rows[0].Field<DateTime?>("jmrCreatedDate");
			eRPProductionCalendarWorkCenterInformationDto.jmrUniqueID = dataTable.Rows[0].Field<Guid>("jmrUniqueID");
			eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarLineID = dataTable.Rows[0].Field<short>("jmrProductionCalendarLineID");
			eRPProductionCalendarWorkCenterInformationDto.jmrProductionCalendarYearID = dataTable.Rows[0].Field<short>("jmrProductionCalendarYearID");
			eRPProductionCalendarWorkCenterInformationDto.jmrRowVersion = dataTable.Rows[0].Field<byte[]>("jmrRowVersion");
			eRPProductionCalendarWorkCenterInformationDto.jmrWorkCenterID = dataTable.Rows[0].Field<string>("jmrWorkCenterID");
			eRPProductionCalendarWorkCenterInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPProductionCalendarWorkCenterInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPProductionCalendarWorkCenterInformationDto);
	}
}
