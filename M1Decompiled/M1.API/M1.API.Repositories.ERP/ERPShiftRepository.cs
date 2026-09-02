using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPShiftRepository : APIBaseRepository, IERPShiftRepository, IAPIBaseRepository, IDisposable
{
	public ERPShiftRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShiftExist(Guid shiftId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmsUniqueID|C", shiftId);
		base.selectList.Add("lmsUniqueID");
		return Task.FromResult(GetAsObject("Shifts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShiftInformationDto>> GetAllShifts(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShiftInformationDto> collection = new List<ERPShiftInformationDto>();
		InitializeParameterLists();
		string[] array = new string[25]
		{
			"lmsAutoClockOutLastRunTime", "lmsAutoClockOutTime", "lmsClockInWindow", "lmsClockOutWindow", "lmsCreatedBy", "lmsCreatedDate", "lmsDescription", "lmsUniqueID", "lmsGraceTimeIn", "lmsGraceTimeOut",
			"lmsIdleTimeIndirectLaborID", "lmsIdleTimeWorkCenterID", "lmsInactiveDate", "lmsInactive", "lmsRoundClockWithInShift", "lmsRoundJobsOutsideOfShift", "lmsRoundJobsWithinShift", "lmsRoundOutsideOfShift", "lmsPlantID", "lmsRoundClockInDirection",
			"lmsRoundClockOutDirection", "lmsRoundTo", "lmsRowVersion", "lmsShiftID", "lmsShiftGroup"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("Shifts");
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
		using (DataTable dataTable = GetAsDataTable("Shifts", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShiftInformationDto eRPShiftInformationDto = new ERPShiftInformationDto();
				eRPShiftInformationDto.lmsAutoClockOutLastRunTime = dataTable.Rows[i].Field<DateTime?>("lmsAutoClockOutLastRunTime");
				eRPShiftInformationDto.lmsAutoClockOutTime = dataTable.Rows[i].Field<decimal>("lmsAutoClockOutTime");
				eRPShiftInformationDto.lmsClockInWindow = dataTable.Rows[i].Field<short>("lmsClockInWindow");
				eRPShiftInformationDto.lmsClockOutWindow = dataTable.Rows[i].Field<short>("lmsClockOutWindow");
				eRPShiftInformationDto.lmsCreatedBy = dataTable.Rows[i].Field<string>("lmsCreatedBy");
				eRPShiftInformationDto.lmsCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmsCreatedDate");
				eRPShiftInformationDto.lmsDescription = dataTable.Rows[i].Field<string>("lmsDescription");
				eRPShiftInformationDto.lmsUniqueID = dataTable.Rows[i].Field<Guid>("lmsUniqueID");
				eRPShiftInformationDto.lmsGraceTimeIn = dataTable.Rows[i].Field<short>("lmsGraceTimeIn");
				eRPShiftInformationDto.lmsGraceTimeOut = dataTable.Rows[i].Field<short>("lmsGraceTimeOut");
				eRPShiftInformationDto.lmsIdleTimeIndirectLaborID = dataTable.Rows[i].Field<string>("lmsIdleTimeIndirectLaborID");
				eRPShiftInformationDto.lmsIdleTimeWorkCenterID = dataTable.Rows[i].Field<string>("lmsIdleTimeWorkCenterID");
				eRPShiftInformationDto.lmsInactiveDate = dataTable.Rows[i].Field<DateTime?>("lmsInactiveDate");
				eRPShiftInformationDto.lmsInactive = dataTable.Rows[i].Field<bool>("lmsInactive");
				eRPShiftInformationDto.lmsRoundClockWithInShift = dataTable.Rows[i].Field<bool>("lmsRoundClockWithInShift");
				eRPShiftInformationDto.lmsRoundJobsOutsideOfShift = dataTable.Rows[i].Field<bool>("lmsRoundJobsOutsideOfShift");
				eRPShiftInformationDto.lmsRoundJobsWithinShift = dataTable.Rows[i].Field<bool>("lmsRoundJobsWithinShift");
				eRPShiftInformationDto.lmsRoundOutsideOfShift = dataTable.Rows[i].Field<bool>("lmsRoundOutsideOfShift");
				eRPShiftInformationDto.lmsPlantID = dataTable.Rows[i].Field<string>("lmsPlantID");
				eRPShiftInformationDto.lmsRoundClockInDirection = dataTable.Rows[i].Field<string>("lmsRoundClockInDirection");
				eRPShiftInformationDto.lmsRoundClockOutDirection = dataTable.Rows[i].Field<string>("lmsRoundClockOutDirection");
				eRPShiftInformationDto.lmsRoundTo = dataTable.Rows[i].Field<byte>("lmsRoundTo");
				eRPShiftInformationDto.lmsRowVersion = dataTable.Rows[i].Field<byte[]>("lmsRowVersion");
				eRPShiftInformationDto.lmsShiftID = dataTable.Rows[i].Field<short>("lmsShiftID");
				eRPShiftInformationDto.lmsShiftGroup = dataTable.Rows[i].Field<byte>("lmsShiftGroup");
				eRPShiftInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShiftInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShiftInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShiftInformationDto> GetShift(Guid shiftId)
	{
		ERPShiftInformationDto eRPShiftInformationDto = new ERPShiftInformationDto();
		InitializeParameterLists();
		string[] collection = new string[25]
		{
			"lmsAutoClockOutLastRunTime", "lmsAutoClockOutTime", "lmsClockInWindow", "lmsClockOutWindow", "lmsCreatedBy", "lmsCreatedDate", "lmsDescription", "lmsUniqueID", "lmsGraceTimeIn", "lmsGraceTimeOut",
			"lmsIdleTimeIndirectLaborID", "lmsIdleTimeWorkCenterID", "lmsInactiveDate", "lmsInactive", "lmsRoundClockWithInShift", "lmsRoundJobsOutsideOfShift", "lmsRoundJobsWithinShift", "lmsRoundOutsideOfShift", "lmsPlantID", "lmsRoundClockInDirection",
			"lmsRoundClockOutDirection", "lmsRoundTo", "lmsRowVersion", "lmsShiftID", "lmsShiftGroup"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmsUniqueID|C", shiftId);
		AddCustomFieldsToSelectList("Shifts");
		using (DataTable dataTable = GetAsDataTable("Shifts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShiftInformationDto);
			}
			eRPShiftInformationDto.lmsAutoClockOutLastRunTime = dataTable.Rows[0].Field<DateTime?>("lmsAutoClockOutLastRunTime");
			eRPShiftInformationDto.lmsAutoClockOutTime = dataTable.Rows[0].Field<decimal>("lmsAutoClockOutTime");
			eRPShiftInformationDto.lmsClockInWindow = dataTable.Rows[0].Field<short>("lmsClockInWindow");
			eRPShiftInformationDto.lmsClockOutWindow = dataTable.Rows[0].Field<short>("lmsClockOutWindow");
			eRPShiftInformationDto.lmsCreatedBy = dataTable.Rows[0].Field<string>("lmsCreatedBy");
			eRPShiftInformationDto.lmsCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmsCreatedDate");
			eRPShiftInformationDto.lmsDescription = dataTable.Rows[0].Field<string>("lmsDescription");
			eRPShiftInformationDto.lmsUniqueID = dataTable.Rows[0].Field<Guid>("lmsUniqueID");
			eRPShiftInformationDto.lmsGraceTimeIn = dataTable.Rows[0].Field<short>("lmsGraceTimeIn");
			eRPShiftInformationDto.lmsGraceTimeOut = dataTable.Rows[0].Field<short>("lmsGraceTimeOut");
			eRPShiftInformationDto.lmsIdleTimeIndirectLaborID = dataTable.Rows[0].Field<string>("lmsIdleTimeIndirectLaborID");
			eRPShiftInformationDto.lmsIdleTimeWorkCenterID = dataTable.Rows[0].Field<string>("lmsIdleTimeWorkCenterID");
			eRPShiftInformationDto.lmsInactiveDate = dataTable.Rows[0].Field<DateTime?>("lmsInactiveDate");
			eRPShiftInformationDto.lmsInactive = dataTable.Rows[0].Field<bool>("lmsInactive");
			eRPShiftInformationDto.lmsRoundClockWithInShift = dataTable.Rows[0].Field<bool>("lmsRoundClockWithInShift");
			eRPShiftInformationDto.lmsRoundJobsOutsideOfShift = dataTable.Rows[0].Field<bool>("lmsRoundJobsOutsideOfShift");
			eRPShiftInformationDto.lmsRoundJobsWithinShift = dataTable.Rows[0].Field<bool>("lmsRoundJobsWithinShift");
			eRPShiftInformationDto.lmsRoundOutsideOfShift = dataTable.Rows[0].Field<bool>("lmsRoundOutsideOfShift");
			eRPShiftInformationDto.lmsPlantID = dataTable.Rows[0].Field<string>("lmsPlantID");
			eRPShiftInformationDto.lmsRoundClockInDirection = dataTable.Rows[0].Field<string>("lmsRoundClockInDirection");
			eRPShiftInformationDto.lmsRoundClockOutDirection = dataTable.Rows[0].Field<string>("lmsRoundClockOutDirection");
			eRPShiftInformationDto.lmsRoundTo = dataTable.Rows[0].Field<byte>("lmsRoundTo");
			eRPShiftInformationDto.lmsRowVersion = dataTable.Rows[0].Field<byte[]>("lmsRowVersion");
			eRPShiftInformationDto.lmsShiftID = dataTable.Rows[0].Field<short>("lmsShiftID");
			eRPShiftInformationDto.lmsShiftGroup = dataTable.Rows[0].Field<byte>("lmsShiftGroup");
			eRPShiftInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShiftInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShiftInformationDto);
	}
}
