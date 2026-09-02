using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;

namespace M1.API.Repositories.Core;

public class CoreRepository : APIBaseRepository, ICoreRepository, IAPIBaseRepository, IDisposable
{
	public CoreRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public CoreRepository(M1Database database)
	{
		base.M1database = database;
	}

	public Task<CTMProcessDto> GetAllProcesses()
	{
		CTMProcessDto cTMProcessDto = new CTMProcessDto();
		List<ProcessDto> list = new List<ProcessDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[3] { "xacProcessID", "xacShortDescription", "xacInactive" });
		base.filterList.Add("xacInactive", 0);
		base.OrderOrGroupByList.Add("xacProcessID ASC");
		using (DataTable dataTable = GetAsDataTable("Processes", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new ProcessDto
					{
						ProcessID = row["xacProcessID"].ToString().Trim(),
						ShortDescription = row["xacShortDescription"].ToString().Trim()
					});
				}
			}
		}
		cTMProcessDto.Processes = list;
		return Task.FromResult(cTMProcessDto);
	}

	public Task<CTMWorkCenterDto> GetAllWorkCenters()
	{
		CTMWorkCenterDto cTMWorkCenterDto = new CTMWorkCenterDto();
		List<WorkCenterDto> list = new List<WorkCenterDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[3] { "xawWorkCenterID", "xawDescription", "xawInactive" });
		base.filterList.Add("xawInactive", 0);
		base.OrderOrGroupByList.Add("xawWorkCenterID ASC");
		using (DataTable dataTable = GetAsDataTable("WorkCenters", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new WorkCenterDto
					{
						WorkCenterID = row["xawWorkCenterID"].ToString().Trim(),
						Description = row["xawDescription"].ToString().Trim()
					});
				}
			}
		}
		cTMWorkCenterDto.WorkCenters = list;
		return Task.FromResult(cTMWorkCenterDto);
	}

	public Task<CTMWarehousesDto> GetAllWarehouses()
	{
		CTMWarehousesDto cTMWarehousesDto = new CTMWarehousesDto();
		List<WarehouseDto> list = new List<WarehouseDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[5] { "imwWarehouseID", "imwName", "imwPlantID", "imwInactive", "imwDefaultWarehouse" });
		base.filterList.Add("imwInactive", 0);
		base.OrderOrGroupByList.Add("imwWarehouseID ASC");
		using (DataTable dataTable = GetAsDataTable("Warehouses", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new WarehouseDto
					{
						WarehouseID = row["imwWarehouseID"].ToString().Trim(),
						Name = row["imwName"].ToString().Trim(),
						PlantID = row["imwPlantID"].ToString().Trim(),
						DefaultWarehouse = row.Field<bool>("imwDefaultWarehouse")
					});
				}
			}
		}
		cTMWarehousesDto.Warehouses = list;
		return Task.FromResult(cTMWarehousesDto);
	}

	public Task<CTMWarehouseBinsDto> GetAllWarehouseBins()
	{
		CTMWarehouseBinsDto cTMWarehouseBinsDto = new CTMWarehouseBinsDto();
		List<WarehouseBinDto> list = new List<WarehouseBinDto>();
		InitializeParameterLists();
		base.selectList.AddRange(new string[5] { "inbWarehouseID", "inbWarehouseBinID", "inbDescription", "inbInactive", "inbDefaultBin" });
		base.filterList.Add("inbInactive", 0);
		base.OrderOrGroupByList.Add("inbWarehouseID,inbWarehouseBinID");
		using (DataTable dataTable = GetAsDataTable("WarehouseBins", base.filterList, base.selectList, base.OrderOrGroupByList, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					list.Add(new WarehouseBinDto
					{
						WarehouseID = row["inbWarehouseID"].ToString().Trim(),
						WarehouseBinID = row["inbWarehouseBinID"].ToString().Trim(),
						Description = row["inbDescription"].ToString().Trim(),
						DefaultBin = row.Field<bool>("inbDefaultBin")
					});
				}
			}
		}
		cTMWarehouseBinsDto.WarehouseBins = list;
		return Task.FromResult(cTMWarehouseBinsDto);
	}

	public Task<bool> DoesWarehouseExistAsync(string partId, string partRevisionId, string warehouseId)
	{
		InitializeParameterLists();
		base.filterList.Add("imlPartID|C", partId);
		base.filterList.Add("imlPartRevisionID|C", partRevisionId);
		base.filterList.Add("imlPartWarehouseID|C", warehouseId);
		base.selectList.Add("imlPartWarehouseID");
		return Task.FromResult(GetAsObject("PartWarehouseLocations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<bool> DoesBinExistAsync(string partId, string partRevisionId, string warehouseId, string binId)
	{
		InitializeParameterLists();
		base.filterList.Add("imbPartID|C", partId);
		base.filterList.Add("imbPartRevisionID|C", partRevisionId);
		base.filterList.Add("imbWarehouseID|C", warehouseId);
		base.filterList.Add("imbPartBinID|C", binId);
		base.filterList.Add("imbInactiveBin|C", false);
		base.selectList.Add("imbPartBinID");
		return Task.FromResult(GetAsObject("PartBins", base.filterList, base.selectList, null, null) != null);
	}

	public new void Dispose()
	{
		base.Dispose(disposing: true);
	}
}
