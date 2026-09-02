using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPTopActivitiesLogRepository : APIBaseRepository, IERPTopActivitiesLogRepository, IAPIBaseRepository, IDisposable
{
	public ERPTopActivitiesLogRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesTopActivitiesLogExist(Guid topActivitiesLogId)
	{
		InitializeParameterLists();
		base.filterList.Add("rxlUniqueID|C", topActivitiesLogId);
		base.selectList.Add("rxlUniqueID");
		return Task.FromResult(GetAsObject("TopActivitiesLog", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPTopActivitiesLogInformationDto>> GetAllTopActivitiesLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPTopActivitiesLogInformationDto> collection = new List<ERPTopActivitiesLogInformationDto>();
		InitializeParameterLists();
		string[] array = new string[11]
		{
			"rxlCount", "rxlExplorerType", "rxlGridID", "rxlObjectDataRun", "rxlObjectName", "rxlProcessedDateTime", "rxlRowVersion", "rxlTopActivityID", "rxlUserID", "rxlVisualizerID",
			"rxlVisualizerType"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("TopActivitiesLog");
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
		using (DataTable dataTable = GetAsDataTable("TopActivitiesLog", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPTopActivitiesLogInformationDto eRPTopActivitiesLogInformationDto = new ERPTopActivitiesLogInformationDto();
				eRPTopActivitiesLogInformationDto.rxlCount = dataTable.Rows[i].Field<int>("rxlCount");
				eRPTopActivitiesLogInformationDto.rxlExplorerType = dataTable.Rows[i].Field<string>("rxlExplorerType");
				eRPTopActivitiesLogInformationDto.rxlGridID = dataTable.Rows[i].Field<string>("rxlGridID");
				eRPTopActivitiesLogInformationDto.rxlObjectDataRun = dataTable.Rows[i].Field<string>("rxlObjectDataRun");
				eRPTopActivitiesLogInformationDto.rxlObjectName = dataTable.Rows[i].Field<string>("rxlObjectName");
				eRPTopActivitiesLogInformationDto.rxlProcessedDateTime = dataTable.Rows[i].Field<DateTime>("rxlProcessedDateTime");
				eRPTopActivitiesLogInformationDto.rxlRowVersion = dataTable.Rows[i].Field<byte[]>("rxlRowVersion");
				eRPTopActivitiesLogInformationDto.rxlTopActivityID = dataTable.Rows[i].Field<int>("rxlTopActivityID");
				eRPTopActivitiesLogInformationDto.rxlUserID = dataTable.Rows[i].Field<string>("rxlUserID");
				eRPTopActivitiesLogInformationDto.rxlVisualizerID = dataTable.Rows[i].Field<string>("rxlVisualizerID");
				eRPTopActivitiesLogInformationDto.rxlVisualizerType = dataTable.Rows[i].Field<string>("rxlVisualizerType");
				eRPTopActivitiesLogInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPTopActivitiesLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPTopActivitiesLogInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPTopActivitiesLogInformationDto> GetTopActivitiesLog(Guid topActivitiesLogId)
	{
		ERPTopActivitiesLogInformationDto eRPTopActivitiesLogInformationDto = new ERPTopActivitiesLogInformationDto();
		InitializeParameterLists();
		string[] collection = new string[11]
		{
			"rxlCount", "rxlExplorerType", "rxlGridID", "rxlObjectDataRun", "rxlObjectName", "rxlProcessedDateTime", "rxlRowVersion", "rxlTopActivityID", "rxlUserID", "rxlVisualizerID",
			"rxlVisualizerType"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("rxlUniqueID|C", topActivitiesLogId);
		AddCustomFieldsToSelectList("TopActivitiesLog");
		using (DataTable dataTable = GetAsDataTable("TopActivitiesLog", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPTopActivitiesLogInformationDto);
			}
			eRPTopActivitiesLogInformationDto.rxlCount = dataTable.Rows[0].Field<int>("rxlCount");
			eRPTopActivitiesLogInformationDto.rxlExplorerType = dataTable.Rows[0].Field<string>("rxlExplorerType");
			eRPTopActivitiesLogInformationDto.rxlGridID = dataTable.Rows[0].Field<string>("rxlGridID");
			eRPTopActivitiesLogInformationDto.rxlObjectDataRun = dataTable.Rows[0].Field<string>("rxlObjectDataRun");
			eRPTopActivitiesLogInformationDto.rxlObjectName = dataTable.Rows[0].Field<string>("rxlObjectName");
			eRPTopActivitiesLogInformationDto.rxlProcessedDateTime = dataTable.Rows[0].Field<DateTime>("rxlProcessedDateTime");
			eRPTopActivitiesLogInformationDto.rxlRowVersion = dataTable.Rows[0].Field<byte[]>("rxlRowVersion");
			eRPTopActivitiesLogInformationDto.rxlTopActivityID = dataTable.Rows[0].Field<int>("rxlTopActivityID");
			eRPTopActivitiesLogInformationDto.rxlUserID = dataTable.Rows[0].Field<string>("rxlUserID");
			eRPTopActivitiesLogInformationDto.rxlVisualizerID = dataTable.Rows[0].Field<string>("rxlVisualizerID");
			eRPTopActivitiesLogInformationDto.rxlVisualizerType = dataTable.Rows[0].Field<string>("rxlVisualizerType");
			eRPTopActivitiesLogInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPTopActivitiesLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPTopActivitiesLogInformationDto);
	}
}
