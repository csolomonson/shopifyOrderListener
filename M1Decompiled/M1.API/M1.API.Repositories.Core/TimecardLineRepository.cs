using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.Utilities;

namespace M1.API.Repositories.Core;

public class TimecardLineRepository : APIBaseRepository, ITimecardLineRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] timecardLineFields = new string[20]
	{
		"lmlTimecardID", "lmlTimecardLineID", "lmlJobID", "lmlJobAssemblyID", "lmlJobOperationID", "lmlWorkCenterID", "lmlProcessID", "lmlCompletionType", "lmlWorkType", "lmlGoodQuantity",
		"lmlScrapQuantity", "lmlReworkQuantity", "lmlActualStartTime", "lmlActualEndTime", "lmlEmployeeID", "lmlTimecardType", "lmlMachineHours", "lmlLaborHours", "lmlUniqueID", "lmlRowVersion"
	};

	public TimecardLineRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesTimecardLineExists(string timecardId, string timecardLineId)
	{
		InitializeParameterLists();
		base.filterList.Add("lmlTimecardID|C", timecardId);
		base.filterList.Add("lmlTimecardLineID|C", timecardLineId);
		base.selectList.Add("lmlTimecardLineID");
		return Task.FromResult(GetAsObject("TimecardLines", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMTimecardLineDto>> GetAllTimecardLines(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMTimecardLineDto> collection = new List<BOMTimecardLineDto>();
		InitializeParameterLists();
		base.selectList.AddRange(timecardLineFields);
		List<string> orderbyList = new List<string> { "lmlTimecardLineID" };
		using (DataTable dataTable = GetAsDataTable("TimecardLines", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMTimecardLineDto bOMTimecardLineDto = new BOMTimecardLineDto();
				bOMTimecardLineDto.TimecardID = dataTable.Rows[i].Field<int>("lmlTimecardID");
				bOMTimecardLineDto.TimecardLineID = dataTable.Rows[i].Field<short>("lmlTimecardLineID");
				bOMTimecardLineDto.JobID = dataTable.Rows[i].Field<string>("lmlJobID");
				bOMTimecardLineDto.JobAssemblyID = dataTable.Rows[i].Field<int>("lmlJobAssemblyID");
				bOMTimecardLineDto.JobOperationID = dataTable.Rows[i].Field<int>("lmlJobOperationID");
				bOMTimecardLineDto.EmployeeID = dataTable.Rows[i].Field<string>("lmlEmployeeID");
				bOMTimecardLineDto.WorkCenterID = dataTable.Rows[i].Field<string>("lmlWorkCenterID");
				bOMTimecardLineDto.ProcessID = dataTable.Rows[i].Field<string>("lmlProcessID");
				bOMTimecardLineDto.CompletionType = dataTable.Rows[i].Field<byte>("lmlCompletionType");
				bOMTimecardLineDto.WorkType = dataTable.Rows[i].Field<byte>("lmlWorkType");
				bOMTimecardLineDto.GoodQuantity = dataTable.Rows[i].Field<decimal>("lmlGoodQuantity");
				bOMTimecardLineDto.ScrapQuantity = dataTable.Rows[i].Field<decimal>("lmlScrapQuantity");
				bOMTimecardLineDto.ReworkQuantity = dataTable.Rows[i].Field<decimal>("lmlReworkQuantity");
				bOMTimecardLineDto.ActualStartTime = dataTable.Rows[i].Field<DateTime?>("lmlActualStartTime");
				bOMTimecardLineDto.ActualEndTime = dataTable.Rows[i].Field<DateTime?>("lmlActualEndTime");
				bOMTimecardLineDto.UniqueID = dataTable.Rows[i].Field<Guid>("lmlUniqueID");
				bOMTimecardLineDto.MachineHours = dataTable.Rows[i].Field<decimal>("lmlMachineHours");
				bOMTimecardLineDto.LaborHours = dataTable.Rows[i].Field<decimal>("lmlLaborHours");
				bOMTimecardLineDto.TimecardType = dataTable.Rows[i].Field<byte>("lmlTimecardType");
				bOMTimecardLineDto.UniqueID = dataTable.Rows[0].Field<Guid>("lmlUniqueID");
				bOMTimecardLineDto.RowVersion = dataTable.Rows[0].Field<byte[]>("lmlRowVersion");
				collection.Add(bOMTimecardLineDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMTimecardLineDto> GetTimecardLine(string timecardId, string timecardLineId)
	{
		BOMTimecardLineDto bOMTimecardLineDto = new BOMTimecardLineDto();
		InitializeParameterLists();
		base.selectList.AddRange(timecardLineFields);
		base.filterList.Add("lmlTimecardID|C", timecardId);
		base.filterList.Add(Guid.TryParse(timecardLineId, out var _) ? "lmlUniqueID|C" : "lmlTimecardLineID|C", timecardLineId);
		using (DataTable dataTable = GetAsDataTable("TimecardLines", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMTimecardLineDto);
			}
			bOMTimecardLineDto.TimecardID = dataTable.Rows[0].Field<int>("lmlTimecardID");
			bOMTimecardLineDto.TimecardLineID = dataTable.Rows[0].Field<short>("lmlTimecardLineID");
			bOMTimecardLineDto.JobID = dataTable.Rows[0].Field<string>("lmlJobID");
			bOMTimecardLineDto.JobAssemblyID = dataTable.Rows[0].Field<int>("lmlJobAssemblyID");
			bOMTimecardLineDto.JobOperationID = dataTable.Rows[0].Field<int>("lmlJobOperationID");
			bOMTimecardLineDto.EmployeeID = dataTable.Rows[0].Field<string>("lmlEmployeeID");
			bOMTimecardLineDto.WorkCenterID = dataTable.Rows[0].Field<string>("lmlWorkCenterID");
			bOMTimecardLineDto.ProcessID = dataTable.Rows[0].Field<string>("lmlProcessID");
			bOMTimecardLineDto.CompletionType = dataTable.Rows[0].Field<byte>("lmlCompletionType");
			bOMTimecardLineDto.WorkType = dataTable.Rows[0].Field<byte>("lmlWorkType");
			bOMTimecardLineDto.GoodQuantity = dataTable.Rows[0].Field<decimal>("lmlGoodQuantity");
			bOMTimecardLineDto.ScrapQuantity = dataTable.Rows[0].Field<decimal>("lmlScrapQuantity");
			bOMTimecardLineDto.ReworkQuantity = dataTable.Rows[0].Field<decimal>("lmlReworkQuantity");
			bOMTimecardLineDto.ActualStartTime = dataTable.Rows[0].Field<DateTime?>("lmlActualStartTime");
			bOMTimecardLineDto.ActualEndTime = dataTable.Rows[0].Field<DateTime?>("lmlActualEndTime");
			bOMTimecardLineDto.UniqueID = dataTable.Rows[0].Field<Guid>("lmlUniqueID");
			bOMTimecardLineDto.MachineHours = dataTable.Rows[0].Field<decimal>("lmlMachineHours");
			bOMTimecardLineDto.LaborHours = dataTable.Rows[0].Field<decimal>("lmlLaborHours");
			bOMTimecardLineDto.TimecardType = dataTable.Rows[0].Field<byte>("lmlTimecardType");
			bOMTimecardLineDto.UniqueID = dataTable.Rows[0].Field<Guid>("lmlUniqueID");
			bOMTimecardLineDto.RowVersion = dataTable.Rows[0].Field<byte[]>("lmlRowVersion");
		}
		return Task.FromResult(bOMTimecardLineDto);
	}
}
