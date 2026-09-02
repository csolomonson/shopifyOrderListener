using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using M1.API.DTOs.ERP;
using M1.API.Utilities;

namespace M1.API.Repositories.ERP;

public class ERPShiftBreakRepository : APIBaseRepository, IERPShiftBreakRepository, IAPIBaseRepository, IDisposable
{
	public ERPShiftBreakRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		base.ApiID = clientContext.APIID;
	}

	public Task<bool> DoesShiftBreakExist(Guid shiftBreakId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmtUniqueID|C", shiftBreakId);
		base.selectList.Add("lmtUniqueID");
		return Task.FromResult(GetAsObject("ShiftBreaks", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<ERPShiftBreakInformationDto>> GetAllShiftBreaks(int? pageSize = null, int? pageNumber = null, string[] filter = null, string orderBy = null)
	{
		ICollection<ERPShiftBreakInformationDto> collection = new List<ERPShiftBreakInformationDto>();
		InitializeParameterLists();
		string[] array = new string[17]
		{
			"lmtBreak1EndTime", "lmtBreak1StartTime", "lmtBreak2EndTime", "lmtBreak2StartTime", "lmtBreak3EndTime", "lmtBreak3StartTime", "lmtCreatedBy", "lmtCreatedDate", "lmtDay", "lmtEndTime",
			"lmtUniqueID", "lmtBreak1Paid", "lmtBreak2Paid", "lmtBreak3Paid", "lmtRowVersion", "lmtShiftID", "lmtStartTime"
		};
		base.selectList.AddRange(array);
		AddCustomFieldsToSelectList("ShiftBreaks");
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
		using (DataTable dataTable = GetAsDataTable("ShiftBreaks", base.filterList, base.selectList, list, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				ERPShiftBreakInformationDto eRPShiftBreakInformationDto = new ERPShiftBreakInformationDto();
				eRPShiftBreakInformationDto.lmtBreak1EndTime = dataTable.Rows[i].Field<decimal>("lmtBreak1EndTime");
				eRPShiftBreakInformationDto.lmtBreak1StartTime = dataTable.Rows[i].Field<decimal>("lmtBreak1StartTime");
				eRPShiftBreakInformationDto.lmtBreak2EndTime = dataTable.Rows[i].Field<decimal>("lmtBreak2EndTime");
				eRPShiftBreakInformationDto.lmtBreak2StartTime = dataTable.Rows[i].Field<decimal>("lmtBreak2StartTime");
				eRPShiftBreakInformationDto.lmtBreak3EndTime = dataTable.Rows[i].Field<decimal>("lmtBreak3EndTime");
				eRPShiftBreakInformationDto.lmtBreak3StartTime = dataTable.Rows[i].Field<decimal>("lmtBreak3StartTime");
				eRPShiftBreakInformationDto.lmtCreatedBy = dataTable.Rows[i].Field<string>("lmtCreatedBy");
				eRPShiftBreakInformationDto.lmtCreatedDate = dataTable.Rows[i].Field<DateTime?>("lmtCreatedDate");
				eRPShiftBreakInformationDto.lmtDay = dataTable.Rows[i].Field<byte>("lmtDay");
				eRPShiftBreakInformationDto.lmtEndTime = dataTable.Rows[i].Field<decimal>("lmtEndTime");
				eRPShiftBreakInformationDto.lmtUniqueID = dataTable.Rows[i].Field<Guid>("lmtUniqueID");
				eRPShiftBreakInformationDto.lmtBreak1Paid = dataTable.Rows[i].Field<bool>("lmtBreak1Paid");
				eRPShiftBreakInformationDto.lmtBreak2Paid = dataTable.Rows[i].Field<bool>("lmtBreak2Paid");
				eRPShiftBreakInformationDto.lmtBreak3Paid = dataTable.Rows[i].Field<bool>("lmtBreak3Paid");
				eRPShiftBreakInformationDto.lmtRowVersion = dataTable.Rows[i].Field<byte[]>("lmtRowVersion");
				eRPShiftBreakInformationDto.lmtShiftID = dataTable.Rows[i].Field<short>("lmtShiftID");
				eRPShiftBreakInformationDto.lmtStartTime = dataTable.Rows[i].Field<decimal>("lmtStartTime");
				eRPShiftBreakInformationDto.CustomFields = new Dictionary<string, object>();
				foreach (DataColumn column in dataTable.Columns)
				{
					if (column.ColumnName.StartsWith("u"))
					{
						eRPShiftBreakInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[i][column.ColumnName]);
					}
				}
				collection.Add(eRPShiftBreakInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<ERPShiftBreakInformationDto> GetShiftBreak(Guid shiftBreakId)
	{
		ERPShiftBreakInformationDto eRPShiftBreakInformationDto = new ERPShiftBreakInformationDto();
		InitializeParameterLists();
		string[] collection = new string[17]
		{
			"lmtBreak1EndTime", "lmtBreak1StartTime", "lmtBreak2EndTime", "lmtBreak2StartTime", "lmtBreak3EndTime", "lmtBreak3StartTime", "lmtCreatedBy", "lmtCreatedDate", "lmtDay", "lmtEndTime",
			"lmtUniqueID", "lmtBreak1Paid", "lmtBreak2Paid", "lmtBreak3Paid", "lmtRowVersion", "lmtShiftID", "lmtStartTime"
		};
		base.selectList.AddRange(collection);
		base.filterList.Add("lmtUniqueID|C", shiftBreakId);
		AddCustomFieldsToSelectList("ShiftBreaks");
		using (DataTable dataTable = GetAsDataTable("ShiftBreaks", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(eRPShiftBreakInformationDto);
			}
			eRPShiftBreakInformationDto.lmtBreak1EndTime = dataTable.Rows[0].Field<decimal>("lmtBreak1EndTime");
			eRPShiftBreakInformationDto.lmtBreak1StartTime = dataTable.Rows[0].Field<decimal>("lmtBreak1StartTime");
			eRPShiftBreakInformationDto.lmtBreak2EndTime = dataTable.Rows[0].Field<decimal>("lmtBreak2EndTime");
			eRPShiftBreakInformationDto.lmtBreak2StartTime = dataTable.Rows[0].Field<decimal>("lmtBreak2StartTime");
			eRPShiftBreakInformationDto.lmtBreak3EndTime = dataTable.Rows[0].Field<decimal>("lmtBreak3EndTime");
			eRPShiftBreakInformationDto.lmtBreak3StartTime = dataTable.Rows[0].Field<decimal>("lmtBreak3StartTime");
			eRPShiftBreakInformationDto.lmtCreatedBy = dataTable.Rows[0].Field<string>("lmtCreatedBy");
			eRPShiftBreakInformationDto.lmtCreatedDate = dataTable.Rows[0].Field<DateTime?>("lmtCreatedDate");
			eRPShiftBreakInformationDto.lmtDay = dataTable.Rows[0].Field<byte>("lmtDay");
			eRPShiftBreakInformationDto.lmtEndTime = dataTable.Rows[0].Field<decimal>("lmtEndTime");
			eRPShiftBreakInformationDto.lmtUniqueID = dataTable.Rows[0].Field<Guid>("lmtUniqueID");
			eRPShiftBreakInformationDto.lmtBreak1Paid = dataTable.Rows[0].Field<bool>("lmtBreak1Paid");
			eRPShiftBreakInformationDto.lmtBreak2Paid = dataTable.Rows[0].Field<bool>("lmtBreak2Paid");
			eRPShiftBreakInformationDto.lmtBreak3Paid = dataTable.Rows[0].Field<bool>("lmtBreak3Paid");
			eRPShiftBreakInformationDto.lmtRowVersion = dataTable.Rows[0].Field<byte[]>("lmtRowVersion");
			eRPShiftBreakInformationDto.lmtShiftID = dataTable.Rows[0].Field<short>("lmtShiftID");
			eRPShiftBreakInformationDto.lmtStartTime = dataTable.Rows[0].Field<decimal>("lmtStartTime");
			eRPShiftBreakInformationDto.CustomFields = new Dictionary<string, object>();
			foreach (DataColumn column in dataTable.Columns)
			{
				if (column.ColumnName.StartsWith("u"))
				{
					eRPShiftBreakInformationDto.CustomFields.Add(column.ColumnName, dataTable.Rows[0][column.ColumnName]);
				}
			}
		}
		return Task.FromResult(eRPShiftBreakInformationDto);
	}
}
