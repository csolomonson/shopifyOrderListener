using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPRecentActivitiesLogRepository : APIBaseRepository, IERPRecentActivitiesLogRepository, IAPIBaseRepository, IDisposable
{
	public ERPRecentActivitiesLogRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesRecentActivitiesLogExist(Guid recentActivitiesLogId)
	{
		InitializeParameterLists();
		base.filterList.Add("rtlUniqueID|C", recentActivitiesLogId);
		base.selectList.Add("rtlUniqueID");
		return Task.FromResult(GetAsObject("RecentActivitiesLog", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPRecentActivitiesLogInformationDto>> GetAllRecentActivitiesLog(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPRecentActivitiesLogInformationDto> collection = new List<ERPRecentActivitiesLogInformationDto>();
		InitializeParameterLists();
		string[] array = new string[10] { "rtlCount", "rtlExplorerType", "rtlLastOpenedDateTime", "rtlObjectDataRun", "rtlObjectID", "rtlObjectName", "rtlParentKey", "rtlRecentActivityID", "rtlRowVersion", "rtlUserID" };
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("RecentActivitiesLog");
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
		using (DataTable dataTable = GetAsDataTable("RecentActivitiesLog", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPRecentActivitiesLogInformationDto eRPRecentActivitiesLogInformationDto = new ERPRecentActivitiesLogInformationDto();
				eRPRecentActivitiesLogInformationDto.rtlCount = dataTable.Rows[i].Field<int>("rtlCount");
				eRPRecentActivitiesLogInformationDto.rtlExplorerType = dataTable.Rows[i].Field<string>("rtlExplorerType");
				eRPRecentActivitiesLogInformationDto.rtlLastOpenedDateTime = dataTable.Rows[i].Field<DateTime>("rtlLastOpenedDateTime");
				eRPRecentActivitiesLogInformationDto.rtlObjectDataRun = dataTable.Rows[i].Field<string>("rtlObjectDataRun");
				eRPRecentActivitiesLogInformationDto.rtlObjectID = dataTable.Rows[i].Field<string>("rtlObjectID");
				eRPRecentActivitiesLogInformationDto.rtlObjectName = dataTable.Rows[i].Field<string>("rtlObjectName");
				eRPRecentActivitiesLogInformationDto.rtlParentKey = dataTable.Rows[i].Field<string>("rtlParentKey");
				eRPRecentActivitiesLogInformationDto.rtlRecentActivityID = dataTable.Rows[i].Field<int>("rtlRecentActivityID");
				eRPRecentActivitiesLogInformationDto.rtlRowVersion = dataTable.Rows[i].Field<byte[]>("rtlRowVersion");
				eRPRecentActivitiesLogInformationDto.rtlUserID = dataTable.Rows[i].Field<string>("rtlUserID");
				eRPRecentActivitiesLogInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPRecentActivitiesLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPRecentActivitiesLogInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPRecentActivitiesLogInformationDto> GetRecentActivitiesLog(Guid recentActivitiesLogId)
	{
		ERPRecentActivitiesLogInformationDto eRPRecentActivitiesLogInformationDto = new ERPRecentActivitiesLogInformationDto();
		InitializeParameterLists();
		string[] collection = new string[10] { "rtlCount", "rtlExplorerType", "rtlLastOpenedDateTime", "rtlObjectDataRun", "rtlObjectID", "rtlObjectName", "rtlParentKey", "rtlRecentActivityID", "rtlRowVersion", "rtlUserID" };
		base.selectList.AddRange(collection);
		base.filterList.Add("rtlUniqueID|C", recentActivitiesLogId);
		AddCustomFieldsToSelectList("RecentActivitiesLog");
		using (DataTable dataTable = GetAsDataTable("RecentActivitiesLog", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPRecentActivitiesLogInformationDto);
			}
			eRPRecentActivitiesLogInformationDto.rtlCount = dataTable.Rows[0].Field<int>("rtlCount");
			eRPRecentActivitiesLogInformationDto.rtlExplorerType = dataTable.Rows[0].Field<string>("rtlExplorerType");
			eRPRecentActivitiesLogInformationDto.rtlLastOpenedDateTime = dataTable.Rows[0].Field<DateTime>("rtlLastOpenedDateTime");
			eRPRecentActivitiesLogInformationDto.rtlObjectDataRun = dataTable.Rows[0].Field<string>("rtlObjectDataRun");
			eRPRecentActivitiesLogInformationDto.rtlObjectID = dataTable.Rows[0].Field<string>("rtlObjectID");
			eRPRecentActivitiesLogInformationDto.rtlObjectName = dataTable.Rows[0].Field<string>("rtlObjectName");
			eRPRecentActivitiesLogInformationDto.rtlParentKey = dataTable.Rows[0].Field<string>("rtlParentKey");
			eRPRecentActivitiesLogInformationDto.rtlRecentActivityID = dataTable.Rows[0].Field<int>("rtlRecentActivityID");
			eRPRecentActivitiesLogInformationDto.rtlRowVersion = dataTable.Rows[0].Field<byte[]>("rtlRowVersion");
			eRPRecentActivitiesLogInformationDto.rtlUserID = dataTable.Rows[0].Field<string>("rtlUserID");
			eRPRecentActivitiesLogInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPRecentActivitiesLogInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPRecentActivitiesLogInformationDto);
	}
}
