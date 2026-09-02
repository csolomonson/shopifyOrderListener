using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.Utilities;

namespace M1.API.Repositories.Core.Job;

public class TimecardRepository : APIBaseRepository, ITimecardRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] timecardFields = new string[12]
	{
		"lmpTimecardID", "lmpEmployeeID", "lmpShiftID", "lmpTimecardDate", "lmpActualStartTime", "lmpActualEndTime", "lmpLastEndTime", "lmpPlantID", "lmpPlantDepartmentID", "lmpPostedDate",
		"lmpUniqueID", "lmpRowVersion"
	};

	public TimecardRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesTimecardExistsAsync(string timecardId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmpTimecardID|C", timecardId);
		base.selectList.Add("lmpTimecardID");
		return Task.FromResult(GetAsObject("Timecards", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesTimecardExistsAsync(string timecardId, string employeeId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmpTimecardID|C", timecardId);
		base.filterList.Add("lmpEmployeeID|C", employeeId);
		base.selectList.Add("lmpTimecardID");
		base.selectList.Add("lmpEmployeeID");
		return Task.FromResult(GetAsObject("Timecards", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMTimecardDto>> GetAllTimecards(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMTimecardDto> collection = new List<BOMTimecardDto>();
		InitializeParameterLists();
		base.selectList.AddRange(timecardFields);
		List<string> orderbyList = new List<string> { "lmpTimecardID" };
		using (DataTable dataTable = GetAsDataTable("Timecards", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMTimecardDto bOMTimecardDto = new BOMTimecardDto();
				bOMTimecardDto.TimecardID = dataTable.Rows[0].Field<int>("lmpTimecardID");
				bOMTimecardDto.EmployeeID = dataTable.Rows[0].Field<string>("lmpEmployeeID");
				bOMTimecardDto.ShiftID = dataTable.Rows[0].Field<short>("lmpShiftID");
				bOMTimecardDto.TimecardDate = dataTable.Rows[0].Field<DateTime?>("lmpTimecardDate");
				bOMTimecardDto.ActualStartTime = dataTable.Rows[0].Field<DateTime>("lmpActualStartTime");
				bOMTimecardDto.ActualEndTime = dataTable.Rows[0].Field<DateTime?>("lmpActualEndTime");
				bOMTimecardDto.LastEndTime = dataTable.Rows[0].Field<DateTime?>("lmpActualEndTime");
				bOMTimecardDto.PlantID = dataTable.Rows[0].Field<string>("lmpPlantID");
				bOMTimecardDto.PlantDepartmentID = dataTable.Rows[0].Field<string>("lmpPlantDepartmentID");
				bOMTimecardDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("lmpPostedDate");
				bOMTimecardDto.UniqueID = dataTable.Rows[0].Field<Guid>("lmpUniqueID");
				bOMTimecardDto.RowVersion = dataTable.Rows[0].Field<byte[]>("lmpRowVersion");
				collection.Add(bOMTimecardDto);
			}
		}
		return Task.FromResult(collection);
	}

	public async Task<BOMTimecardDto> GetTimecard(string timecardId)
	{
		InitializeParameterLists();
		base.selectList.AddRange(timecardFields);
		base.filterList.Add(Guid.TryParse(timecardId, out var _) ? "lmpUniqueID|C" : "lmpTimecardID|C", timecardId);
		return await GetTimecardInformationAsync();
	}

	public async Task<BOMTimecardDto> GetTimecard(string timecardId, string employeeId)
	{
		InitializeParameterLists();
		base.selectList.AddRange(timecardFields);
		base.filterList.Add(Guid.TryParse(timecardId, out var _) ? "lmpUniqueID|C" : "lmpTimecardID|C", timecardId);
		base.filterList.Add("lmpEmployeeId|C", employeeId);
		return await GetTimecardInformationAsync();
	}

	private Task<BOMTimecardDto> GetTimecardInformationAsync()
	{
		BOMTimecardDto bOMTimecardDto = new BOMTimecardDto();
		using (DataTable dataTable = GetAsDataTable("Timecards", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMTimecardDto);
			}
			bOMTimecardDto.TimecardID = dataTable.Rows[0].Field<int>("lmpTimecardID");
			bOMTimecardDto.EmployeeID = dataTable.Rows[0].Field<string>("lmpEmployeeID");
			bOMTimecardDto.ShiftID = dataTable.Rows[0].Field<short>("lmpShiftID");
			bOMTimecardDto.TimecardDate = dataTable.Rows[0].Field<DateTime?>("lmpTimecardDate");
			bOMTimecardDto.ActualStartTime = dataTable.Rows[0].Field<DateTime>("lmpActualStartTime");
			bOMTimecardDto.ActualEndTime = dataTable.Rows[0].Field<DateTime?>("lmpActualEndTime");
			bOMTimecardDto.LastEndTime = dataTable.Rows[0].Field<DateTime?>("lmpActualEndTime");
			bOMTimecardDto.PlantID = dataTable.Rows[0].Field<string>("lmpPlantID");
			bOMTimecardDto.PlantDepartmentID = dataTable.Rows[0].Field<string>("lmpPlantDepartmentID");
			bOMTimecardDto.PostedDate = dataTable.Rows[0].Field<DateTime?>("lmpPostedDate");
			bOMTimecardDto.UniqueID = dataTable.Rows[0].Field<Guid>("lmpUniqueID");
			bOMTimecardDto.RowVersion = dataTable.Rows[0].Field<byte[]>("lmpRowVersion");
		}
		return Task.FromResult(bOMTimecardDto);
	}
}
