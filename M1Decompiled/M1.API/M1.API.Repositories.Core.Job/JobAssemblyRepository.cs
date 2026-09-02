using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

public class JobAssemblyRepository : APIBaseRepository, IJobAssemblyRepository, IAPIBaseRepository, IDisposable
{
	private readonly APIClientContext _clientContext;

	private readonly string[] jobAssemblyFields = new string[24]
	{
		"jmaJobID", "jmaJobAssemblyID", "jmaLevel", "jmaPartWarehouseLocationID", "jmaPartBinID", "jmaSourceMethodID", "jmaSourceRevisionID", "jmaPartID", "jmaPartRevisionID", "jmaUnitOfMeasure",
		"jmaPartShortDescription", "jmaQuantityPerParent", "jmaOrderQuantity", "jmaProductionQuantity", "jmaQuantityToReturn", "jmaQuantityToMake", "jmaQuantityToPull", "jmaScrapQuantity", "jmaEstimatedUnitCost", "jmaOverlapOperationID",
		"jmaOverlapType", "jmaParentAssemblyID", "jmaClosed", "jmaScheduledDueDate"
	};

	public JobAssemblyRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		_clientContext = clientContext;
	}

	public Task<bool> DoesJobAssemblyExists(string jobAssemblyId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmaJobAssemblyID|C", jobAssemblyId);
		base.selectList.Add("jmaJobAssemblyID");
		return Task.FromResult(GetAsObject("JobAssemblies", base.filterList, base.selectList, null, null) != null);
	}

	public Task<BOMJobAssemblyDto> GetJobAssemblyInfo(string jobId, int jobAssemblyId)
	{
		BOMJobAssemblyDto bOMJobAssemblyDto = new BOMJobAssemblyDto();
		InitializeParameterLists();
		base.selectList.AddRange(jobAssemblyFields);
		base.filterList.Add("jmaJobID|C", jobId);
		base.filterList.Add("jmaJobAssemblyID", jobAssemblyId);
		using (DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				bOMJobAssemblyDto.JobID = dataTable.Rows[0]["jmaJobID"].ToString().Trim();
				bOMJobAssemblyDto.JobAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmaJobAssemblyID"]);
				bOMJobAssemblyDto.Level = Convert.ToInt16(dataTable.Rows[0]["jmaLevel"]);
				bOMJobAssemblyDto.PartBinID = dataTable.Rows[0]["jmaPartBinID"].ToString().Trim();
				bOMJobAssemblyDto.PartWareHouseLocationID = dataTable.Rows[0]["jmaPartWarehouseLocationID"].ToString().Trim();
				bOMJobAssemblyDto.ParentAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmaParentAssemblyID"]);
				bOMJobAssemblyDto.SourceMethodID = dataTable.Rows[0]["jmaSourceMethodID"].ToString().Trim();
				bOMJobAssemblyDto.SourceRevisionID = dataTable.Rows[0]["jmaSourceRevisionID"].ToString().Trim();
				bOMJobAssemblyDto.PartID = dataTable.Rows[0]["jmaPartID"].ToString().Trim();
				bOMJobAssemblyDto.PartRevisionID = dataTable.Rows[0]["jmaPartRevisionID"].ToString().Trim();
				bOMJobAssemblyDto.PartShortDescription = dataTable.Rows[0]["jmaPartShortDescription"].ToString().Trim();
				bOMJobAssemblyDto.UnitOfMeasure = dataTable.Rows[0]["jmaUnitOfMeasure"].ToString().Trim();
				bOMJobAssemblyDto.QuantityPerParent = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityPerParent"]);
				bOMJobAssemblyDto.OrderQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaOrderQuantity"]);
				bOMJobAssemblyDto.ProductionQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaProductionQuantity"]);
				bOMJobAssemblyDto.QuantityToMake = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToMake"]);
				bOMJobAssemblyDto.QuantityToPull = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToPull"]);
				bOMJobAssemblyDto.ScrapQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaScrapQuantity"]);
				bOMJobAssemblyDto.QuantityToReturn = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToReturn"]);
				bOMJobAssemblyDto.EstimatedUnitCost = Convert.ToDecimal(dataTable.Rows[0]["jmaEstimatedUnitCost"]);
				bOMJobAssemblyDto.OverlapOperationID = Convert.ToInt32(dataTable.Rows[0]["jmaOverlapOperationID"]);
				bOMJobAssemblyDto.OverlapType = Convert.ToByte(dataTable.Rows[0]["jmaOverlapType"]);
				bOMJobAssemblyDto.Closed = dataTable.Rows[0].Field<bool>("jmaClosed");
				bOMJobAssemblyDto.DueDate = dataTable.Rows[0].Field<DateTime?>("jmaScheduledDueDate");
			}
		}
		return Task.FromResult(bOMJobAssemblyDto);
	}

	public Task<IList<BOMJobAssemblyDto>> GetJobAssembliesInfo(string jobId)
	{
		List<BOMJobAssemblyDto> result = new List<BOMJobAssemblyDto>();
		InitializeParameterLists();
		base.selectList.AddRange(jobAssemblyFields);
		base.filterList.Add("jmaJobID|C", jobId);
		base.OrderOrGroupByList.Add("jmaJobAssemblyID ASC");
		DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, base.OrderOrGroupByList, null);
		try
		{
			if (dataTable.Rows.Count > 0)
			{
				result = (from asm in dataTable.AsEnumerable()
					select new BOMJobAssemblyDto
					{
						JobID = dataTable.Rows[0]["jmaJobID"].ToString().Trim(),
						JobAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmaJobAssemblyID"]),
						Level = Convert.ToInt16(dataTable.Rows[0]["jmaLevel"]),
						PartBinID = dataTable.Rows[0]["jmaPartBinID"].ToString().Trim(),
						PartWareHouseLocationID = dataTable.Rows[0]["jmaPartWarehouseLocationID"].ToString().Trim(),
						ParentAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmaParentAssemblyID"]),
						SourceMethodID = dataTable.Rows[0]["jmaSourceMethodID"].ToString().Trim(),
						SourceRevisionID = dataTable.Rows[0]["jmaSourceRevisionID"].ToString().Trim(),
						PartID = dataTable.Rows[0]["jmaPartID"].ToString().Trim(),
						PartRevisionID = dataTable.Rows[0]["jmaPartRevisionID"].ToString().Trim(),
						PartShortDescription = dataTable.Rows[0]["jmaPartShortDescription"].ToString().Trim(),
						UnitOfMeasure = dataTable.Rows[0]["jmaUnitOfMeasure"].ToString().Trim(),
						QuantityPerParent = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityPerParent"]),
						OrderQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaOrderQuantity"]),
						ProductionQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaProductionQuantity"]),
						QuantityToMake = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToMake"]),
						QuantityToPull = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToPull"]),
						ScrapQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmaScrapQuantity"]),
						QuantityToReturn = Convert.ToDecimal(dataTable.Rows[0]["jmaQuantityToReturn"]),
						EstimatedUnitCost = Convert.ToDecimal(dataTable.Rows[0]["jmaEstimatedUnitCost"]),
						OverlapOperationID = Convert.ToInt32(dataTable.Rows[0]["jmaOverlapOperationID"]),
						OverlapType = Convert.ToByte(dataTable.Rows[0]["jmaOverlapType"]),
						Closed = dataTable.Rows[0].Field<bool>("jmaClosed"),
						DueDate = dataTable.Rows[0].Field<DateTime?>("jmaScheduledDueDate")
					}).ToList();
			}
		}
		finally
		{
			if (dataTable != null)
			{
				((IDisposable)dataTable).Dispose();
			}
		}
		return Task.FromResult((IList<BOMJobAssemblyDto>)result);
	}

	public Task<ICollection<BOMJobAssemblyDto>> GetAllJobAssemblies(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMJobAssemblyDto> collection = new List<BOMJobAssemblyDto>();
		InitializeParameterLists();
		base.selectList.AddRange(jobAssemblyFields);
		List<string> orderbyList = new List<string> { "jmaJobAssemblyID" };
		using (DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMJobAssemblyDto bOMJobAssemblyDto = new BOMJobAssemblyDto();
				bOMJobAssemblyDto.EstimatedUnitCost = dataTable.Rows[i].Field<decimal>("jmaEstimatedUnitCost");
				bOMJobAssemblyDto.Closed = dataTable.Rows[i].Field<bool>("jmaClosed");
				bOMJobAssemblyDto.JobID = dataTable.Rows[i].Field<string>("jmaJobID");
				bOMJobAssemblyDto.Level = dataTable.Rows[i].Field<short>("jmaLevel");
				bOMJobAssemblyDto.OrderQuantity = dataTable.Rows[i].Field<decimal>("jmaOrderQuantity");
				bOMJobAssemblyDto.OverlapOperationID = dataTable.Rows[i].Field<int>("jmaOverlapOperationID");
				bOMJobAssemblyDto.OverlapType = dataTable.Rows[i].Field<byte>("jmaOverlapType");
				bOMJobAssemblyDto.ParentAssemblyID = dataTable.Rows[i].Field<int>("jmaParentAssemblyID");
				bOMJobAssemblyDto.PartBinID = dataTable.Rows[i].Field<string>("jmaPartBinID");
				bOMJobAssemblyDto.PartID = dataTable.Rows[i].Field<string>("jmaPartID");
				bOMJobAssemblyDto.PartRevisionID = dataTable.Rows[i].Field<string>("jmaPartRevisionID");
				bOMJobAssemblyDto.PartShortDescription = dataTable.Rows[i].Field<string>("jmaPartShortDescription");
				bOMJobAssemblyDto.PartWareHouseLocationID = dataTable.Rows[i].Field<string>("jmaPartWareHouseLocationID");
				bOMJobAssemblyDto.ProductionQuantity = dataTable.Rows[i].Field<decimal>("jmaProductionQuantity");
				bOMJobAssemblyDto.QuantityPerParent = dataTable.Rows[i].Field<decimal>("jmaQuantityPerParent");
				bOMJobAssemblyDto.QuantityToMake = dataTable.Rows[i].Field<decimal>("jmaQuantityToMake");
				bOMJobAssemblyDto.QuantityToPull = dataTable.Rows[i].Field<decimal>("jmaQuantityToPull");
				bOMJobAssemblyDto.ScrapQuantity = dataTable.Rows[i].Field<decimal>("jmaScrapQuantity");
				bOMJobAssemblyDto.QuantityToReturn = dataTable.Rows[i].Field<decimal>("jmaQuantityToReturn");
				bOMJobAssemblyDto.JobAssemblyID = dataTable.Rows[i].Field<int>("jmaJobAssemblyID");
				bOMJobAssemblyDto.SourceMethodID = dataTable.Rows[i].Field<string>("jmaSourceMethodID");
				bOMJobAssemblyDto.SourceRevisionID = dataTable.Rows[i].Field<string>("jmaSourceRevisionID");
				bOMJobAssemblyDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("jmaUnitOfMeasure");
				bOMJobAssemblyDto.DueDate = dataTable.Rows[i].Field<DateTime?>("jmaScheduledDueDate");
				collection.Add(bOMJobAssemblyDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMJobAssemblyDto> GetJobAssembly(string jobAssemblyId)
	{
		BOMJobAssemblyDto bOMJobAssemblyDto = new BOMJobAssemblyDto();
		InitializeParameterLists();
		base.selectList.AddRange(jobAssemblyFields);
		base.filterList.Add(Guid.TryParse(jobAssemblyId, out var _) ? "jmaUniqueID|C" : "jmaJobAssemblyID|C", jobAssemblyId);
		using (DataTable dataTable = GetAsDataTable("JobAssemblies", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(bOMJobAssemblyDto);
			}
			bOMJobAssemblyDto.EstimatedUnitCost = dataTable.Rows[0].Field<decimal>("jmaEstimatedUnitCost");
			bOMJobAssemblyDto.Closed = dataTable.Rows[0].Field<bool>("jmaClosed");
			bOMJobAssemblyDto.JobID = dataTable.Rows[0].Field<string>("jmaJobID");
			bOMJobAssemblyDto.Level = dataTable.Rows[0].Field<short>("jmaLevel");
			bOMJobAssemblyDto.OrderQuantity = dataTable.Rows[0].Field<decimal>("jmaOrderQuantity");
			bOMJobAssemblyDto.OverlapType = dataTable.Rows[0].Field<byte>("jmaOverlapType");
			bOMJobAssemblyDto.ParentAssemblyID = dataTable.Rows[0].Field<int>("jmaParentAssemblyID");
			bOMJobAssemblyDto.PartBinID = dataTable.Rows[0].Field<string>("jmaPartBinID");
			bOMJobAssemblyDto.PartID = dataTable.Rows[0].Field<string>("jmaPartID");
			bOMJobAssemblyDto.PartRevisionID = dataTable.Rows[0].Field<string>("jmaPartRevisionID");
			bOMJobAssemblyDto.PartShortDescription = dataTable.Rows[0].Field<string>("jmaPartShortDescription");
			bOMJobAssemblyDto.PartWareHouseLocationID = dataTable.Rows[0].Field<string>("jmaPartWareHouseLocationID");
			bOMJobAssemblyDto.ProductionQuantity = dataTable.Rows[0].Field<decimal>("jmaProductionQuantity");
			bOMJobAssemblyDto.QuantityPerParent = dataTable.Rows[0].Field<decimal>("jmaQuantityPerParent");
			bOMJobAssemblyDto.QuantityToMake = dataTable.Rows[0].Field<decimal>("jmaQuantityToMake");
			bOMJobAssemblyDto.QuantityToPull = dataTable.Rows[0].Field<decimal>("jmaQuantityToPull");
			bOMJobAssemblyDto.ScrapQuantity = dataTable.Rows[0].Field<decimal>("jmaScrapQuantity");
			bOMJobAssemblyDto.QuantityToReturn = dataTable.Rows[0].Field<decimal>("jmaQuantityToReturn");
			bOMJobAssemblyDto.JobAssemblyID = dataTable.Rows[0].Field<int>("jmaJobAssemblyID");
			bOMJobAssemblyDto.SourceMethodID = dataTable.Rows[0].Field<string>("jmaSourceMethodID");
			bOMJobAssemblyDto.SourceRevisionID = dataTable.Rows[0].Field<string>("jmaSourceRevisionID");
			bOMJobAssemblyDto.UnitOfMeasure = dataTable.Rows[0].Field<string>("jmaUnitOfMeasure");
			bOMJobAssemblyDto.DueDate = dataTable.Rows[0].Field<DateTime?>("jmaScheduledDueDate");
		}
		return Task.FromResult(bOMJobAssemblyDto);
	}

	public async Task<APIValidationInfoDto> SaveJobAssembly(BOMJobAssemblyDto jobAssembly)
	{
		APIValidationInfoDto apiValidationInfoDto = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		Part axErpPart = new Part();
		BOMJobDto bOMJobDto = new BOMJobDto();
		try
		{
			using M1BindingSource jobAsmBs = new M1BindingSource(base.M1database, null);
			jobAsmBs.ClearCache();
			stringBuilder.Append("jmaJobID = " + M1Util.ConvertToLinq(jobAssembly.JobID) + " " + $"And jmaJobAssemblyID = {jobAssembly.JobAssemblyID}");
			jobAsmBs.DataSourceTable = "JobAssemblies";
			jobAsmBs.NavigateTo(stringBuilder.ToString());
			DataRow jobDataRow;
			if (jobAsmBs.Count == 0)
			{
				jobDataRow = jobAsmBs.AddNew() as DataRow;
				jobDataRow["jmaJobID"] = jobAssembly.JobID;
				jobDataRow["jmaJobAssemblyID"] = jobAssembly.JobAssemblyID;
			}
			else
			{
				jobDataRow = jobAsmBs.CurrentAsDataRow;
			}
			jobDataRow["jmaLevel"] = jobAssembly.Level;
			jobDataRow["jmaParentAssemblyID"] = jobAssembly.ParentAssemblyID;
			if (jobAssembly.PartID != null)
			{
				jobDataRow["jmaPartID"] = jobAssembly.PartID;
			}
			if (jobAssembly.PartRevisionID != null)
			{
				jobDataRow["jmaPartRevisionID"] = jobAssembly.PartRevisionID;
			}
			if (jobAssembly.PartShortDescription != null)
			{
				jobDataRow["jmaPartShortDescription"] = jobAssembly.PartShortDescription;
			}
			if (jobAssembly.UnitOfMeasure != null)
			{
				jobDataRow["jmaUnitOfMeasure"] = jobAssembly.UnitOfMeasure;
			}
			if (jobAssembly.SourceMethodID != null)
			{
				jobDataRow["jmaSourceMethodID"] = jobAssembly.SourceMethodID;
			}
			if (jobAssembly.SourceRevisionID != null)
			{
				jobDataRow["jmaSourceRevisionID"] = jobAssembly.SourceRevisionID;
			}
			jobDataRow["jmaQuantityPerParent"] = jobAssembly.QuantityPerParent;
			jobDataRow["jmaOrderQuantity"] = jobAssembly.OrderQuantity;
			jobDataRow["jmaEstimatedUnitCost"] = jobAssembly.EstimatedUnitCost;
			jobDataRow["jmaOverlapOperationID"] = jobAssembly.OverlapOperationID;
			jobDataRow["jmaOverlapType"] = jobAssembly.OverlapType;
			jobDataRow["jmaScrapQuantity"] = jobAssembly.ScrapQuantity;
			jobDataRow["jmaQuantityToPull"] = jobAssembly.QuantityToPull;
			using (JobRepository jobRepository = new JobRepository(_clientContext))
			{
				bOMJobDto = await jobRepository.GetJob(jobAssembly.JobID);
			}
			string text = jobAssembly.PartWareHouseLocationID;
			string value = jobAssembly.PartBinID;
			if (string.IsNullOrEmpty(text))
			{
				text = axErpPart.GetPreferredWarehouse(base.M1database, jobAssembly.PartID, jobAssembly.PartRevisionID, bOMJobDto?.PlantID);
			}
			if (string.IsNullOrEmpty(value))
			{
				value = axErpPart.GetPreferredWarehouseBin(base.M1database, jobAssembly.PartID, jobAssembly.PartRevisionID, text, bOMJobDto?.PlantID);
			}
			jobDataRow["jmaPartWareHouseLocationID"] = text;
			jobDataRow["jmaPartBinID"] = value;
			jobAsmBs.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the job [" + jobAssembly.JobID + "]");
			List<string> errorsList = list;
			apiValidationInfoDto = new APIValidationInfoDto(errorsList, null, HttpStatusCode.InternalServerError);
		}
		return await Task.FromResult(apiValidationInfoDto);
	}
}
