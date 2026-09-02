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

public class JobMaterialRepository : APIBaseRepository, IJobMaterialRepository, IAPIBaseRepository, IDisposable
{
	private readonly APIClientContext _clientContext;

	private readonly string DELETE_JOB_MATERIAL = "DELETE FROM JobMaterials WHERE jmmJobID = @JobID And jmmJobAssemblyID = @JobAssemblyId AND jmmJobMaterialID =@JobMaterialId";

	private readonly string[] jobMaterialFields = new string[36]
	{
		"jmmJobID", "jmmJobAssemblyID", "jmmJobMaterialID", "jmmPartID", "jmmPartRevisionID", "jmmPartWarehouseLocationID", "jmmPartBinID", "jmmUnitOfMeasure", "jmmPartShortDescription", "jmmQuantityPerAssembly",
		"jmmScrapPercent", "jmmScrapQuantity", "jmmEstimatedQuantity", "jmmEstimatedUnitCost", "jmmCalculatedUnitCost", "jmmFirm", "jmmSupplierOrganizationID", "jmmPullAllFromStock", "jmmPurchaseLocationID", "jmmPurchaseOrderID",
		"jmmLeadTime", "jmmMinimumCharge", "jmmDueInDate", "jmmRequiredDate", "jmmQuantityAllocated", "jmmQuantityReceived", "jmmScrapQuantityReceived", "jmmQuantityToInspect", "jmmQuantityToReturn", "jmmReceivedComplete",
		"jmmPurchaseToJobQuantity", "jmmPullFromStockQuantity", "jmmClosed", "jmmUniqueID", "jmmRowVersion", "jmmRelatedJobOperationID"
	};

	public JobMaterialRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
		base.M1DD = clientContext.DataDictionary;
		_clientContext = clientContext;
	}

	public Task<bool> DoesJobMaterialExists(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		InitializeParameterLists();
		base.filterList.Add("jmmJobID|C", jobId);
		base.filterList.Add("jmmJobAssemblyID", jobAssemblyId);
		base.filterList.Add("jmmJobMaterialID", jobMaterialId);
		base.selectList.Add("jmmJobID");
		return Task.FromResult(GetAsObject("JobMaterials", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<BOMJobMaterialDto>> GetAllJobMaterials(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<BOMJobMaterialDto> collection = new List<BOMJobMaterialDto>();
		InitializeParameterLists();
		base.selectList.AddRange(jobMaterialFields);
		List<string> orderbyList = new List<string> { "jmmJobMaterialID" };
		using (DataTable dataTable = GetAsDataTable("JobMaterials", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BOMJobMaterialDto bOMJobMaterialDto = new BOMJobMaterialDto();
				bOMJobMaterialDto.JobID = dataTable.Rows[i].Field<string>("jmmJobID");
				bOMJobMaterialDto.JobAssemblyID = dataTable.Rows[i].Field<int>("jmmJobAssemblyID");
				bOMJobMaterialDto.JobMaterialID = dataTable.Rows[i].Field<int>("jmmJobMaterialID");
				bOMJobMaterialDto.PartID = dataTable.Rows[i].Field<string>("jmmPartID");
				bOMJobMaterialDto.PartRevisionID = dataTable.Rows[i].Field<string>("jmmPartRevisionID");
				bOMJobMaterialDto.PartWarehouseLocationID = dataTable.Rows[i].Field<string>("jmmPartWarehouseLocationID");
				bOMJobMaterialDto.PartBinID = dataTable.Rows[i].Field<string>("jmmPartBinID");
				bOMJobMaterialDto.UnitOfMeasure = dataTable.Rows[i].Field<string>("jmmUnitOfMeasure");
				bOMJobMaterialDto.PartShortDescription = dataTable.Rows[i].Field<string>("jmmPartShortDescription");
				bOMJobMaterialDto.QuantityPerAssembly = dataTable.Rows[i].Field<decimal>("jmmQuantityPerAssembly");
				bOMJobMaterialDto.ScrapPercent = dataTable.Rows[i].Field<decimal>("jmmScrapPercent");
				bOMJobMaterialDto.ScrapQuantity = dataTable.Rows[i].Field<decimal>("jmmScrapQuantity");
				bOMJobMaterialDto.EstimatedQuantity = dataTable.Rows[i].Field<decimal>("jmmEstimatedQuantity");
				bOMJobMaterialDto.EstimatedUnitCost = dataTable.Rows[i].Field<decimal>("jmmEstimatedUnitCost");
				bOMJobMaterialDto.CalculatedUnitCost = dataTable.Rows[i].Field<decimal>("jmmCalculatedUnitCost");
				bOMJobMaterialDto.Firm = dataTable.Rows[i].Field<bool>("jmmFirm");
				bOMJobMaterialDto.SupplierOrganizationID = dataTable.Rows[i].Field<string>("jmmSupplierOrganizationID");
				bOMJobMaterialDto.PurchaseLocationID = dataTable.Rows[i].Field<string>("jmmPurchaseLocationID");
				bOMJobMaterialDto.PurchaseOrderID = dataTable.Rows[i].Field<string>("jmmPurchaseOrderID");
				bOMJobMaterialDto.LeadTime = dataTable.Rows[i].Field<short>("jmmLeadTime");
				bOMJobMaterialDto.MinimumCharge = dataTable.Rows[i].Field<decimal>("jmmMinimumCharge");
				bOMJobMaterialDto.DueInDate = dataTable.Rows[i].Field<DateTime?>("jmmDueInDate");
				bOMJobMaterialDto.RequiredDate = dataTable.Rows[i].Field<DateTime?>("jmmRequiredDate");
				bOMJobMaterialDto.QuantityAllocated = dataTable.Rows[i].Field<decimal>("jmmQuantityAllocated");
				bOMJobMaterialDto.QuantityReceived = dataTable.Rows[i].Field<decimal>("jmmQuantityReceived");
				bOMJobMaterialDto.ScrapQuantityReceived = dataTable.Rows[i].Field<decimal>("jmmScrapQuantityReceived");
				bOMJobMaterialDto.QuantityToInspect = dataTable.Rows[i].Field<decimal>("jmmQuantityToInspect");
				bOMJobMaterialDto.QuantityToReturn = dataTable.Rows[i].Field<decimal>("jmmQuantityToReturn");
				bOMJobMaterialDto.ReceivedComplete = dataTable.Rows[i].Field<bool>("jmmReceivedComplete");
				bOMJobMaterialDto.PurchaseToJobQuantity = dataTable.Rows[i].Field<decimal>("jmmPurchaseToJobQuantity");
				bOMJobMaterialDto.PullAllFromStock = dataTable.Rows[i].Field<bool>("jmmPullAllFromStock");
				bOMJobMaterialDto.PullFromStockQuantity = dataTable.Rows[i].Field<decimal>("jmmPullFromStockQuantity");
				bOMJobMaterialDto.Closed = dataTable.Rows[i].Field<bool>("jmmClosed");
				bOMJobMaterialDto.UniqueID = dataTable.Rows[i].Field<Guid>("jmmUniqueID");
				bOMJobMaterialDto.RowVersion = dataTable.Rows[i].Field<byte[]>("jmmRowVersion");
				bOMJobMaterialDto.RelatedJobOperationID = dataTable.Rows[i].Field<int>("jmmRelatedJobOperationID");
				collection.Add(bOMJobMaterialDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<BOMJobMaterialDto> GetJobMaterialInfo(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		BOMJobMaterialDto bOMJobMaterialDto = new BOMJobMaterialDto();
		InitializeParameterLists();
		base.selectList.AddRange(jobMaterialFields);
		base.filterList.Add("jmmJobID|C", jobId);
		base.filterList.Add("jmmJobAssemblyID", jobAssemblyId);
		base.filterList.Add("jmmJobMaterialID", jobMaterialId);
		using (DataTable dataTable = GetAsDataTable("JobMaterials", base.filterList, base.selectList, null, null))
		{
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				bOMJobMaterialDto.JobID = dataTable.Rows[0]["jmmJobID"].ToString().Trim();
				bOMJobMaterialDto.JobAssemblyID = Convert.ToInt32(dataTable.Rows[0]["jmmJobAssemblyID"]);
				bOMJobMaterialDto.JobMaterialID = Convert.ToInt32(dataTable.Rows[0]["jmmJobMaterialID"]);
				bOMJobMaterialDto.PartID = dataTable.Rows[0]["jmmPartID"].ToString().Trim();
				bOMJobMaterialDto.PartRevisionID = dataTable.Rows[0]["jmmPartRevisionID"].ToString().Trim();
				bOMJobMaterialDto.PartWarehouseLocationID = dataTable.Rows[0]["jmmPartWarehouseLocationID"].ToString().Trim();
				bOMJobMaterialDto.PartBinID = dataTable.Rows[0]["jmmPartBinID"].ToString().Trim();
				bOMJobMaterialDto.UnitOfMeasure = dataTable.Rows[0]["jmmUnitOfMeasure"].ToString().Trim();
				bOMJobMaterialDto.PartShortDescription = dataTable.Rows[0]["jmmPartShortDescription"].ToString().Trim();
				bOMJobMaterialDto.QuantityPerAssembly = Convert.ToDecimal(dataTable.Rows[0]["jmmQuantityPerAssembly"]);
				bOMJobMaterialDto.ScrapPercent = Convert.ToDecimal(dataTable.Rows[0]["jmmScrapPercent"]);
				bOMJobMaterialDto.ScrapQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmmScrapQuantity"]);
				bOMJobMaterialDto.EstimatedQuantity = Convert.ToDecimal(dataTable.Rows[0]["jmmEstimatedQuantity"]);
				bOMJobMaterialDto.EstimatedUnitCost = Convert.ToDecimal(dataTable.Rows[0]["jmmEstimatedUnitCost"]);
				bOMJobMaterialDto.Firm = dataTable.Rows[0].Field<bool>("jmmFirm");
				bOMJobMaterialDto.SupplierOrganizationID = dataTable.Rows[0]["jmmSupplierOrganizationID"].ToString().Trim();
				bOMJobMaterialDto.PurchaseLocationID = dataTable.Rows[0]["jmmPurchaseLocationID"].ToString().Trim();
				bOMJobMaterialDto.PurchaseOrderID = dataTable.Rows[0]["jmmPurchaseOrderID"].ToString().Trim();
				bOMJobMaterialDto.LeadTime = dataTable.Rows[0].Field<short>("jmmLeadTime");
				bOMJobMaterialDto.MinimumCharge = dataTable.Rows[0].Field<decimal>("jmmMinimumCharge");
				bOMJobMaterialDto.DueInDate = dataTable.Rows[0].Field<DateTime?>("jmmDueInDate");
				bOMJobMaterialDto.RequiredDate = dataTable.Rows[0].Field<DateTime?>("jmmRequiredDate");
				bOMJobMaterialDto.QuantityAllocated = dataTable.Rows[0].Field<decimal>("jmmQuantityAllocated");
				bOMJobMaterialDto.QuantityReceived = dataTable.Rows[0].Field<decimal>("jmmQuantityReceived");
				bOMJobMaterialDto.ScrapQuantityReceived = dataTable.Rows[0].Field<decimal>("jmmScrapQuantityReceived");
				bOMJobMaterialDto.QuantityToInspect = dataTable.Rows[0].Field<decimal>("jmmQuantityToInspect");
				bOMJobMaterialDto.QuantityToReturn = dataTable.Rows[0].Field<decimal>("jmmQuantityToReturn");
				bOMJobMaterialDto.ReceivedComplete = dataTable.Rows[0].Field<bool>("jmmReceivedComplete");
				bOMJobMaterialDto.PurchaseToJobQuantity = dataTable.Rows[0].Field<decimal>("jmmPurchaseToJobQuantity");
				bOMJobMaterialDto.PullAllFromStock = dataTable.Rows[0].Field<bool>("jmmPullAllFromStock");
				bOMJobMaterialDto.PullFromStockQuantity = dataTable.Rows[0].Field<decimal>("jmmPullFromStockQuantity");
				bOMJobMaterialDto.Closed = dataTable.Rows[0].Field<bool>("jmmClosed");
				bOMJobMaterialDto.UniqueID = dataTable.Rows[0].Field<Guid>("jmmUniqueID");
				bOMJobMaterialDto.CalculatedUnitCost = Convert.ToDecimal(dataTable.Rows[0]["jmmCalculatedUnitCost"]);
				bOMJobMaterialDto.RowVersion = dataTable.Rows[0].Field<byte[]>("jmmRowVersion");
				bOMJobMaterialDto.RelatedJobOperationID = Convert.ToInt32(dataTable.Rows[0]["jmmRelatedJobOperationID"]);
			}
		}
		return Task.FromResult(bOMJobMaterialDto);
	}

	public async Task<APIValidationInfoDto> SaveJobMaterial(BOMJobMaterialDto jobMaterial)
	{
		APIValidationInfoDto apiValidationInfoDto = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		Part axErpPart = new Part();
		BOMJobDto bOMJobDto = new BOMJobDto();
		try
		{
			using M1BindingSource jobMaterialBs = new M1BindingSource(base.M1database, null);
			jobMaterialBs.ClearCache();
			stringBuilder.Append("jmmJobID = " + M1Util.ConvertToLinq(jobMaterial.JobID) + " " + $"And jmmJobAssemblyID = {jobMaterial.JobAssemblyID} " + $"And jmmJobMaterialID = {jobMaterial.JobMaterialID}");
			jobMaterialBs.DataSourceTable = "JobMaterials";
			jobMaterialBs.NavigateTo(stringBuilder.ToString());
			DataRow jobDataRow;
			if (jobMaterialBs.Count == 0)
			{
				jobDataRow = jobMaterialBs.AddNew() as DataRow;
				jobDataRow["jmmJobID"] = jobMaterial.JobID;
				jobDataRow["jmmJobAssemblyID"] = jobMaterial.JobAssemblyID;
				if (Convert.ToInt32(jobDataRow["jmmJobMaterialID"]) != 0)
				{
					jobDataRow["jmmJobMaterialID"] = jobMaterial.JobMaterialID;
				}
				else
				{
					jobMaterialBs.SetKeyToNextAvailable();
				}
			}
			else
			{
				jobDataRow = jobMaterialBs.CurrentAsDataRow;
			}
			if (jobMaterial.PartID != null)
			{
				jobDataRow["jmmPartID"] = jobMaterial.PartID;
			}
			if (jobMaterial.PartRevisionID != null)
			{
				jobDataRow["jmmPartRevisionID"] = jobMaterial.PartRevisionID;
			}
			if (jobMaterial.PartShortDescription != null)
			{
				jobDataRow["jmmPartShortDescription"] = jobMaterial.PartShortDescription;
			}
			if (jobMaterial.UnitOfMeasure != null)
			{
				jobDataRow["jmmUnitOfMeasure"] = jobMaterial.UnitOfMeasure;
			}
			jobDataRow["jmmRelatedJobOperationID"] = jobMaterial.RelatedJobOperationID;
			jobDataRow["jmmQuantityPerAssembly"] = jobMaterial.QuantityPerAssembly;
			jobDataRow["jmmEstimatedQuantity"] = jobMaterial.EstimatedQuantity;
			jobDataRow["jmmEstimatedUnitCost"] = jobMaterial.EstimatedUnitCost;
			using (JobRepository jobRepository = new JobRepository(_clientContext))
			{
				bOMJobDto = await jobRepository.GetJob(jobMaterial.JobID);
			}
			string text = jobMaterial.PartWarehouseLocationID;
			string value = jobMaterial.PartBinID;
			if (string.IsNullOrEmpty(text))
			{
				text = axErpPart.GetPreferredWarehouse(base.M1database, jobMaterial.PartID, jobMaterial.PartRevisionID, bOMJobDto?.PlantID);
			}
			if (string.IsNullOrEmpty(value))
			{
				value = axErpPart.GetPreferredWarehouseBin(base.M1database, jobMaterial.PartID, jobMaterial.PartRevisionID, text, bOMJobDto?.PlantID);
			}
			jobDataRow["jmmPartWareHouseLocationID"] = text;
			jobDataRow["jmmPartBinID"] = value;
			jobDataRow["jmmScrapPercent"] = jobMaterial.ScrapPercent;
			jobDataRow["jmmScrapQuantity"] = jobMaterial.ScrapQuantity;
			jobDataRow["jmmCalculatedUnitCost"] = jobMaterial.CalculatedUnitCost;
			jobDataRow["jmmFirm"] = jobMaterial.Firm;
			jobDataRow["jmmLeadTime"] = jobMaterial.LeadTime;
			jobDataRow["jmmMinimumCharge"] = jobMaterial.MinimumCharge;
			jobDataRow["jmmQuantityAllocated"] = jobMaterial.QuantityAllocated;
			jobDataRow["jmmQuantityReceived"] = jobMaterial.QuantityReceived;
			jobDataRow["jmmScrapQuantityReceived"] = jobMaterial.ScrapQuantityReceived;
			jobDataRow["jmmQuantityToInspect"] = jobMaterial.QuantityToInspect;
			jobDataRow["jmmQuantityToReturn"] = jobMaterial.QuantityToReturn;
			jobDataRow["jmmPurchaseToJobQuantity"] = jobMaterial.PurchaseToJobQuantity;
			jobDataRow["jmmPullAllFromStock"] = jobMaterial.PullAllFromStock;
			jobDataRow["jmmPullFromStockQuantity"] = jobMaterial.PullFromStockQuantity;
			jobDataRow["jmmReceivedComplete"] = jobMaterial.ReceivedComplete;
			jobDataRow["jmmClosed"] = jobMaterial.Closed;
			if (!string.IsNullOrEmpty(jobMaterial.PurchaseOrderID))
			{
				jobDataRow["jmmPurchaseOrderID"] = jobMaterial.PurchaseOrderID;
			}
			if (!string.IsNullOrEmpty(jobMaterial.PurchaseLocationID))
			{
				jobDataRow["jmmPurchaseLocationID"] = jobMaterial.PurchaseLocationID;
			}
			if (!string.IsNullOrEmpty(jobMaterial.SupplierOrganizationID))
			{
				jobDataRow["jmmSupplierOrganizationID"] = jobMaterial.SupplierOrganizationID;
			}
			if (jobMaterial.DueInDate.HasValue)
			{
				jobDataRow["jmmDueInDate"] = jobMaterial.DueInDate.Value;
			}
			if (jobMaterial.RequiredDate.HasValue)
			{
				jobDataRow["jmmRequiredDate"] = jobMaterial.RequiredDate.Value;
			}
			jobMaterialBs.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the job [" + jobMaterial.JobID + "]");
			apiValidationInfoDto = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return await Task.FromResult(apiValidationInfoDto);
	}

	public Task<APIValidationInfoDto> DeleteJobMaterial(string jobId, int jobAssemblyId, int jobMaterialId)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		SqlTransaction sqlTransaction = null;
		List<string> list = new List<string>();
		try
		{
			sqlTransaction = base.M1database.BeginTransaction();
			using (SqlCommand sqlCommand = base.M1database.NewSqlCommand(DELETE_JOB_MATERIAL))
			{
				sqlCommand.Parameters.AddWithValue("@JobID", jobId);
				sqlCommand.Parameters.AddWithValue("@JobAssemblyId", jobAssemblyId);
				sqlCommand.Parameters.AddWithValue("@JobMaterialId", jobMaterialId);
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
