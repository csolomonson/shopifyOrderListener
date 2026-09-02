using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM.Job;
using M1.API.DTOs.Core;
using M1.API.Utilities;
using M1.Ax.Erp;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core.Job;

public class JobOperationRepository : APIBaseRepository, IJobOperationRepository, IAPIBaseRepository, IDisposable
{
	private readonly ICoreRepository _coreRepository;

	private readonly string DELETE_JOB_OPERATION = "DELETE FROM JobOperations WHERE jmoJobID = @JobID And jmoJobAssemblyID = @JobAssemblyId AND jmoJobOperationID =@JobOperationId";

	private readonly string[] jobOperationFields = new string[25]
	{
		"jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID", "jmoOperationType", "jmoPartWarehouseLocationID", "jmoWorkCenterID", "jmoProcessID", "jmoProcessShortDescription", "jmoProductionStandard", "jmoPartBinID",
		"jmoStandardFactor", "jmoMachinesToSchedule", "jmoMachineType", "jmoQuantityPerAssembly", "jmoOperationQuantity", "jmoSetupRate", "jmoProductionRate", "jmoOverheadRate", "jmoPartID", "jmoPartRevisionID",
		"jmoUnitOfMeasure", "jmoQuantityComplete", "jmoPlantID", "jmoClosed", "jmoDueDate"
	};

	public JobOperationRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		_coreRepository = new CoreRepository(clientContext);
	}

	public Task<bool> DoesJobOperationExists(string jobOperationId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmoJobOperationID|C", jobOperationId);
		base.selectList.Add("jmoJobOperationID");
		return Task.FromResult(GetAsObject("JobOperations", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMJobOperationDto>> GetAllJobOperations(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMJobOperationDto> collection = new List<BOMJobOperationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(jobOperationFields);
		List<string> orderbyList = new List<string> { "jmoJobOperationID" };
		using (DataTable dataTable = GetAsDataTable("JobOperations", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMJobOperationDto bOMJobOperationDto = new BOMJobOperationDto();
				bOMJobOperationDto.JobID = dataTable.Rows[i].Field<string>("jmoJobID");
				bOMJobOperationDto.JobAssemblyID = dataTable.Rows[i].Field<int>("jmoJobAssemblyID");
				bOMJobOperationDto.JobOperationID = dataTable.Rows[i].Field<int>("jmoJobOperationID");
				bOMJobOperationDto.OperationType = dataTable.Rows[i].Field<byte>("jmoOperationType");
				bOMJobOperationDto.WorkCenterID = dataTable.Rows[i].Field<string>("jmoWorkCenterID");
				bOMJobOperationDto.ProcessID = dataTable.Rows[i].Field<string>("jmoProcessID");
				bOMJobOperationDto.ProcessShortDescription = dataTable.Rows[i].Field<string>("jmoProcessShortDescription");
				bOMJobOperationDto.ProductionStandard = dataTable.Rows[i].Field<decimal>("jmoProductionStandard");
				bOMJobOperationDto.StandardFactor = dataTable.Rows[i].Field<string>("jmoStandardFactor");
				bOMJobOperationDto.MachinesToSchedule = dataTable.Rows[i].Field<short>("jmoMachinesToSchedule");
				bOMJobOperationDto.MachineType = dataTable.Rows[i].Field<byte>("jmoMachineType");
				bOMJobOperationDto.QuantityPerAssembly = dataTable.Rows[i].Field<decimal>("jmoQuantityPerAssembly");
				bOMJobOperationDto.QuantityComplete = dataTable.Rows[i].Field<decimal>("jmoQuantityComplete");
				bOMJobOperationDto.OperationQuantity = dataTable.Rows[i].Field<decimal>("jmoOperationQuantity");
				bOMJobOperationDto.SetupRate = dataTable.Rows[i].Field<decimal>("jmoSetupRate");
				bOMJobOperationDto.ProductionRate = dataTable.Rows[i].Field<decimal>("jmoProductionRate");
				bOMJobOperationDto.OverheadRate = dataTable.Rows[i].Field<decimal>("jmoOverheadRate");
				bOMJobOperationDto.PartID = dataTable.Rows[i].Field<string>("jmoPartID");
				bOMJobOperationDto.PartRevisionID = dataTable.Rows[i].Field<string>("jmoPartRevisionID");
				bOMJobOperationDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("jmoPartWarehouseLocationID");
				bOMJobOperationDto.PartBinID = dataTable.Rows[i].Field<string>("jmoPartBinID");
				bOMJobOperationDto.PlantID = dataTable.Rows[i].Field<string>("jmoPlantID");
				bOMJobOperationDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("jmoUnitOfMeasure");
				bOMJobOperationDto.Closed = dataTable.Rows[i].Field<bool>("jmoClosed");
				bOMJobOperationDto.DueDate = dataTable.Rows[i].Field<DateTime>("jmoDueDate");
				collection.Add(bOMJobOperationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMJobOperationDto> GetJobOperationInfo(string jobId, int jobAssemblyId, int jobOperationId)
	{
		BOMJobOperationDto bOMJobOperationDto = new BOMJobOperationDto();
		InitializeParameterLists();
		base.selectList.AddRange(jobOperationFields);
		base.filterList.Add("jmoJobID|C", jobId);
		base.filterList.Add("jmoJobAssemblyID", jobAssemblyId);
		base.filterList.Add("jmoJobOperationID", jobOperationId);
		using (DataTable dataTable = GetAsDataTable("JobOperations", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				bOMJobOperationDto.JobID = dataTable.Rows[0]["jmoJobID"].ToString().Trim();
				bOMJobOperationDto.JobAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmoJobAssemblyID"]);
				bOMJobOperationDto.OperationType = Convert.ToByte(dataTable.Rows[0]["jmoOperationType"]);
				bOMJobOperationDto.JobOperationID = Convert.ToInt32(dataTable.Rows[0]["jmoJobOperationID"]);
				bOMJobOperationDto.WorkCenterID = dataTable.Rows[0]["jmoWorkCenterID"].ToString().Trim();
				bOMJobOperationDto.ProcessID = dataTable.Rows[0]["jmoProcessID"].ToString().Trim();
				bOMJobOperationDto.PartID = dataTable.Rows[0]["jmoPartID"].ToString().Trim();
				bOMJobOperationDto.PlantID = dataTable.Rows[0]["jmoPlantID"].ToString().Trim();
				bOMJobOperationDto.OperationQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmoOperationQuantity"]);
				bOMJobOperationDto.QuantityComplete = Convert.ToDecimal(dataTable.Rows[0]["jmoQuantityComplete"]);
				bOMJobOperationDto.PartRevisionID = dataTable.Rows[0]["jmoPartRevisionID"].ToString().Trim();
				bOMJobOperationDto.PartWarehouseLocationID = dataTable.Rows[0]["jmoPartWarehouseLocationID"].ToString().Trim();
				bOMJobOperationDto.PartBinID = dataTable.Rows[0]["jmoPartBinID"].ToString().Trim();
				bOMJobOperationDto.ProcessShortDescription = dataTable.Rows[0]["jmoProcessShortDescription"].ToString().Trim();
				bOMJobOperationDto.UnitOfMeasure = dataTable.Rows[0]["jmoUnitOfMeasure"].ToString().Trim();
				bOMJobOperationDto.ProductionStandard = Convert.ToDecimal(dataTable.Rows[0]["jmoProductionStandard"]);
				bOMJobOperationDto.StandardFactor = dataTable.Rows[0]["jmoStandardFactor"].ToString().Trim();
				bOMJobOperationDto.MachinesToSchedule = Convert.ToInt16(dataTable.Rows[0]["jmoMachinesToSchedule"]);
				bOMJobOperationDto.QuantityPerAssembly = Convert.ToDecimal(dataTable.Rows[0]["jmoQuantityPerAssembly"]);
				bOMJobOperationDto.SetupRate = Convert.ToDecimal(dataTable.Rows[0]["jmoSetupRate"]);
				bOMJobOperationDto.ProductionRate = Convert.ToDecimal(dataTable.Rows[0]["jmoProductionRate"]);
				bOMJobOperationDto.OverheadRate = Convert.ToDecimal(dataTable.Rows[0]["jmoOverheadRate"]);
				bOMJobOperationDto.MachineType = Convert.ToByte(dataTable.Rows[0]["jmoMachineType"]);
				bOMJobOperationDto.Closed = dataTable.Rows[0].Field<bool>("jmoClosed");
				bOMJobOperationDto.DueDate = dataTable.Rows[0].Field<DateTime?>("jmoDueDate");
			}
		}
		return Task.FromResult(bOMJobOperationDto);
	}

	public async Task<APIValidationInfoDto> SaveJobOperationAsync(BOMJobOperationDto jobOperation)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		Part part = new Part();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("jmoJobID = " + M1Util.ConvertToLinq(jobOperation.JobID) + " " + $"And jmoJobAssemblyID = {jobOperation.JobAssemblyID} " + $"And jmoJobOperationID = {jobOperation.JobOperationID}");
			m1BindingSource.DataSourceTable = "JobOperations";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				dataRow["jmoJobID"] = jobOperation.JobID;
				dataRow["jmoJobAssemblyID"] = jobOperation.JobAssemblyID;
				dataRow["jmoJobOperationID"] = jobOperation.JobOperationID;
				if (jobOperation.JobOperationID == 0)
				{
					m1BindingSource.SetKeyToNextAvailable();
				}
				else
				{
					dataRow["jmoJobOperationID"] = jobOperation.JobOperationID;
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["jmoOperationType"] = jobOperation.OperationType;
			if (jobOperation.WorkCenterID != null)
			{
				dataRow["jmoWorkCenterID"] = jobOperation.WorkCenterID;
			}
			if (jobOperation.ProcessID != null)
			{
				dataRow["jmoProcessID"] = jobOperation.ProcessID;
			}
			if (jobOperation.ProcessShortDescription != null)
			{
				dataRow["jmoProcessShortDescription"] = jobOperation.ProcessShortDescription;
			}
			dataRow["jmoProductionStandard"] = jobOperation.ProductionStandard;
			if (jobOperation.StandardFactor != null)
			{
				dataRow["jmoStandardFactor"] = jobOperation.StandardFactor.ToUpper();
			}
			dataRow["jmoMachinesToSchedule"] = jobOperation.MachinesToSchedule;
			dataRow["jmoMachineType"] = jobOperation.MachineType;
			dataRow["jmoQuantityPerAssembly"] = jobOperation.QuantityPerAssembly;
			dataRow["jmoSetupRate"] = jobOperation.SetupRate;
			dataRow["jmoProductionRate"] = jobOperation.ProductionRate;
			dataRow["jmoOverheadRate"] = jobOperation.OverheadRate;
			dataRow["jmoPartID"] = jobOperation.PartID;
			dataRow["jmoPartRevisionID"] = jobOperation.PartRevisionID;
			dataRow["jmoUnitOfMeasure"] = jobOperation.UnitOfMeasure;
			dataRow["jmoPlantID"] = jobOperation.PlantID;
			string text = jobOperation.PartWarehouseLocationID;
			string value = jobOperation.PartBinID;
			if (string.IsNullOrEmpty(text))
			{
				text = part.GetPreferredWarehouse(base.M1database, jobOperation.PartID, jobOperation.PartRevisionID, jobOperation.PlantID);
			}
			if (string.IsNullOrEmpty(value))
			{
				value = part.GetPreferredWarehouseBin(base.M1database, jobOperation.PartID, jobOperation.PartRevisionID, text, jobOperation.PlantID);
			}
			dataRow["jmoPartWareHouseLocationID"] = text;
			dataRow["jmoPartBinID"] = value;
			dataRow["jmoOperationQuantity"] = jobOperation.OperationQuantity;
			dataRow["jmoQuantityComplete"] = jobOperation.QuantityComplete;
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the job [" + jobOperation.JobID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return await Task.FromResult(result);
	}

	public Task<APIValidationInfoDto> DeleteJobOperation(string jobId, int jobAssemblyId, int jobOperationId)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		SqlTransaction sqlTransaction = null;
		List<string> list = new List<string>();
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using (SqlCommand sqlCommand = base.M1database.NewSqlCommand(DELETE_JOB_OPERATION))
			{
				sqlCommand.Parameters.AddWithValue("@JobID", jobId);
				sqlCommand.Parameters.AddWithValue("@JobAssemblyId", jobAssemblyId);
				sqlCommand.Parameters.AddWithValue("@JobOperationId", jobOperationId);
				base.M1database.ExecuteCommand(sqlCommand, sqlTransaction);
			}
			base.M1database.CommitTransaction(sqlTransaction);
		}
		catch (Exception ex)
		{
			base.M1database.RollbackTransaction(sqlTransaction);
			list.Add("Error occurred [" + ex.Message + "] while processing the Job [" + jobId + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}
}
