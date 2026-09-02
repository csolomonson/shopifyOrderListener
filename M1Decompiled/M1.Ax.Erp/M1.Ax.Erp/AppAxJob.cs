using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using M1.Ax.Erp.JobSchedule;
using M1.Ax.Erp.JobSchedule.ShopLoadExplorer;
using M1.Core;
using M1.Core.Script;
using Microsoft.Exchange.WebServices.Data;

namespace M1.Ax.Erp;

[AxScript("Job")]
[ComVisible(true)]
public class AppAxJob : IDisposable
{
	private IServiceProvider provider;

	private M1Database database;

	private Job jobFunc;

	private Job getJobRef()
	{
		if (jobFunc == null)
		{
			jobFunc = new Job();
		}
		return jobFunc;
	}

	public AppAxJob(IServiceProvider parentProvider)
	{
		provider = parentProvider;
		database = provider.GetService(typeof(M1Database)) as M1Database;
	}

	public void UnscheduleJob(string jobID, SqlTransaction transaction = null)
	{
		getJobRef().UnscheduleJob(provider.GetService(typeof(M1Database)) as M1Database, jobID, transaction);
	}

	public void UnscheduleAssembly(string jobId, int assemblyId, SqlTransaction transaction = null)
	{
		getJobRef().UnscheduleAssembly(provider.GetService(typeof(M1Database)) as M1Database, jobId, assemblyId, transaction);
	}

	public void ExportScheduleToExchange(string workCenter)
	{
		ExchangeUtilities exchangeUtilities = new ExchangeUtilities();
		ScheduleExport scheduleExport = new ScheduleExport();
		SqlCommand sqlCommand = database.NewSqlCommand("Select xawCalendarLocation From WorkCenters Where xawWorkCenterID = @WorKCenter");
		sqlCommand.Parameters.Add(new SqlParameter("@WorkCenter", SqlDbType.VarChar)).Value = workCenter;
		string text = Convert.ToString(database.ExecuteScalar(sqlCommand));
		if (!string.IsNullOrEmpty(text))
		{
			ExchangeService exchangeService = exchangeUtilities.GetExchangeService(database);
			Folder publicFolderByPath = exchangeUtilities.GetPublicFolderByPath(exchangeService, text);
			scheduleExport.ExportSchedule(database, workCenter, exchangeService, publicFolderByPath);
		}
	}

	public void UpdateOperationCompleteFlags(string jobID, int asmID, int seq, SqlTransaction transaction = null)
	{
		getJobRef().UpdateOperationCompleteFlags(database, jobID, asmID, seq, transaction);
	}

	public void UpdateOperationCompleteFlagsForAsm(string jobID, int asmID, SqlTransaction transaction = null)
	{
		getJobRef().UpdateOperationCompleteFlags(database, jobID, asmID, transaction);
	}

	public void UpdateOperationCompleteFlagsForJob(string jobID, SqlTransaction transaction = null)
	{
		getJobRef().UpdateOperationCompleteFlags(database, jobID, transaction);
	}

	public double CalculateProductionHours(double nOperationQty, double nProductionStandard, string cStandardFactor, string cWorkCenter, short nRound = 0)
	{
		return getJobRef().CalculateProductionHours(database, nOperationQty, nProductionStandard, cStandardFactor, cWorkCenter, nRound);
	}

	public bool IsJobAssemblyProductionComplete(object oTransaction, string cJob, int nAsm)
	{
		if (oTransaction == DBNull.Value)
		{
			oTransaction = null;
		}
		return getJobRef().IsJobAssemblyProductionComplete(provider.GetService(typeof(M1Database)) as M1Database, (SqlTransaction)oTransaction, cJob, nAsm);
	}

	public void ChangeProductionQty(object oTransaction, string cJob, int nAsm, double nNewQty, double nOldQty = 0.0, bool bUpdateAsm = true)
	{
		if (oTransaction == DBNull.Value)
		{
			oTransaction = null;
		}
		getJobRef().ChangeProductionQty(provider.GetService(typeof(M1Database)) as M1Database, (SqlTransaction)oTransaction, cJob, nAsm, nNewQty, nOldQty, bUpdateAsm);
	}

	public void CompleteJob(string jobID, bool complete, bool updateJobs = true, double qtyComplete = 0.0, int asmID = 0, object completionDate = null, bool prodCompleteChanged = true, bool qtyCompleteChanged = true, bool completeDateChanged = true, object transaction = null)
	{
		DateTime? completionDate2 = ((completionDate != null && completionDate != DBNull.Value && completionDate is DateTime) ? new DateTime?((DateTime)completionDate) : ((DateTime?)null));
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		getJobRef().CompleteJob(database, (SqlTransaction)transaction, jobID, complete, updateJobs, qtyComplete, asmID, completionDate2, prodCompleteChanged, qtyCompleteChanged, completeDateChanged);
	}

	public bool IsParentJobOnHold(string jobID)
	{
		return new Job().IsParentJobOnHold(database, jobID);
	}

	public bool IsParentJobOnTimeAndMaterial(string jobID)
	{
		return new Job().IsParentJobOnTimeAndMaterial(database, jobID);
	}

	public double CalcAllocation(double estimatedQty, double receivedQty, bool complete)
	{
		return getJobRef().CalcAllocation(estimatedQty, receivedQty, complete);
	}

	public void RefreshJobAsmQuantities(object asmRow)
	{
		getJobRef().RefreshJobAsmQuantities((DataRow)asmRow);
	}

	public void RefreshMaterialPriceBreaks(object transaction, object materialRow)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		if (materialRow is M1AdoRecordsetProxy)
		{
			getJobRef().RefreshMaterialPriceBreaks(database, (SqlTransaction)transaction, ((M1AdoRecordsetProxy)materialRow).CurrentDataRow);
		}
		else
		{
			getJobRef().RefreshMaterialPriceBreaks(database, (SqlTransaction)transaction, (DataRow)materialRow);
		}
	}

	public double GetJobPercentageComplete(string jobID)
	{
		return getJobRef().GetJobPercentageComplete(database, jobID);
	}

	public void DeleteJobAssembly(object transaction, string jobID, int asmID)
	{
		getJobRef().DeleteJobAssembly(database, (SqlTransaction)transaction, jobID, asmID);
	}

	public double CalculateJobOperationCalculatedCost(double quantity, double estimatedUnitCost, double minimumCharge, double setupCharge, double qtyBreak1, double unitCost1, double qtyBreak2, double unitCost2, double qtyBreak3, double unitCost3, double qtyBreak4, double unitCost4, double qtyBreak5, double unitCost5, double qtyBreak6, double unitCost6, double qtyBreak7, double unitCost7, double qtyBreak8, double unitCost8, double qtyBreak9, double unitCost9, bool excludeSetupCharge = false)
	{
		return getJobRef().CalculateJobOperationCalculatedCost(quantity, estimatedUnitCost, minimumCharge, setupCharge, qtyBreak1, unitCost1, qtyBreak2, unitCost2, qtyBreak3, unitCost3, qtyBreak4, unitCost4, qtyBreak5, unitCost5, qtyBreak6, unitCost6, qtyBreak7, unitCost7, qtyBreak8, unitCost8, qtyBreak9, unitCost9, excludeSetupCharge);
	}

	public double CalculateJobMaterialCalculatedCost(double estimatedQty, double estimatedUnitCost, double minimumCharge, double qtyBreak1, double unitCost1, double qtyBreak2, double unitCost2, double qtyBreak3, double unitCost3, double qtyBreak4, double unitCost4, double qtyBreak5, double unitCost5, double qtyBreak6, double unitCost6, double qtyBreak7, double unitCost7, double qtyBreak8, double unitCost8, double qtyBreak9, double unitCost9)
	{
		return getJobRef().CalculateJobMaterialCalculatedCost(estimatedQty, estimatedUnitCost, minimumCharge, qtyBreak1, unitCost1, qtyBreak2, unitCost2, qtyBreak3, unitCost3, qtyBreak4, unitCost4, qtyBreak5, unitCost5, qtyBreak6, unitCost6, qtyBreak7, unitCost7, qtyBreak8, unitCost8, qtyBreak9, unitCost9);
	}

	public double CalculateQtyWithScrap(double quantity, double scrapPercent, double scrapQty, short roundTo = 5)
	{
		return getJobRef().CalculateQtyWithScrap(database, quantity, scrapPercent, scrapQty, roundTo);
	}

	public void Dispose()
	{
		jobFunc = null;
		database = null;
		provider = null;
	}

	public int CreateJobSequenceFromPOLine(M1BindingSource bindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		return new Job().CreateJobSequenceFromPOLine(bindingSource, transaction, currentRow);
	}

	public int CreateJobSequenceFromMaterialIssue(M1BindingSource bindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		return new Job().CreateJobSequenceFromMaterialIssue(bindingSource, transaction, currentRow);
	}

	public int CreateJobSequenceFromMfgReceipt(M1BindingSource bindingSource, SqlTransaction transaction, DataRow currentRow)
	{
		return new Job().CreateJobSequenceFromMfgReceipt(bindingSource, transaction, currentRow);
	}

	public JobCost GetJobCosts(SqlTransaction transaction, string jobID, int assemblyID, decimal qtyCompleted)
	{
		return new Job().GetJobCosts(database, transaction, jobID, assemblyID, qtyCompleted, 0);
	}

	public void JobMaterialSaveAsEvent(object transaction, string whereClause)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		getJobRef().JobMaterialSaveAsEvent(provider.GetService(typeof(M1Database)) as M1Database, (SqlTransaction)transaction, whereClause);
	}

	public bool DoesJobExist(string jobID, object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Job().DoesJobExist(database, (SqlTransaction)transaction, jobID);
	}

	public void DoesJobExistCheck(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		if (e2.Row.Table.Columns.Contains("JobID") && !string.IsNullOrWhiteSpace(e2.Row.Field<string>("JobID")) && new Job().DoesJobExist(e2.Database, e2.SqlTransaction, e2.Row.Field<string>("JobID")))
		{
			e2.Cancel = string.Format("Job {0} already exists in the Jobs table.", e2.Row.Field<string>("JobID"));
		}
	}

	public int GetJobMaterialLeadTime(string jobID, int asmID, int matID, double qty)
	{
		return getJobRef().GetJobMaterialLeadTime(database, jobID, asmID, matID, qty);
	}

	public void UpdateQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new Job().UpdateQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public void UpdateReturnQuantitiesInGrid(object sender, object e)
	{
		FieldDefinition.FieldValueChangedEventArgs e2 = (FieldDefinition.FieldValueChangedEventArgs)e;
		FieldDefinition fieldDefinition = (FieldDefinition)sender;
		new Job().UpdateReturnQuantitiesInGrid(e2.Row, fieldDefinition.FieldName);
	}

	public object GetShopLoadData(string fromDateObject, string bucketType, int perBucket, string plantId, string departmentId, bool showPastLoad, bool showFutureLoad, bool excludeSetupLoad, object[] workCenters = null)
	{
		string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		DateTime fromDate = DateTime.ParseExact(fromDateObject, shortDatePattern, CultureInfo.InvariantCulture);
		M1Database obj = provider.GetService(typeof(M1Database)) as M1Database;
		string[] workCenters2 = workCenters?.Select((object workCenter) => workCenter.ToString()).ToArray();
		return new ShopLoadExplorer(obj, fromDate, 7, bucketType, perBucket, plantId, departmentId, workCenters2, isFromShopLoadReport: true, excludeSetupLoad, showPastLoad, showFutureLoad).FillDataTable(isFromShopLoadReport: true);
	}

	public string GetJobIDForOrder(string orderID, short lineID, bool? includeLineInJobOverride = false)
	{
		return new Job().GetJobIDForOrder(database, orderID, lineID, includeLineInJobOverride);
	}

	public void CreateJob(string orderID, int orderLineID, int orderDeliveryID, string jobID, double productionQty, DateTime? requiredDate, bool planningComplete)
	{
		new Job().CreateJob(database, orderID, orderLineID, orderDeliveryID, jobID, productionQty, requiredDate, planningComplete);
	}

	public string CreateJobEx(string jobID, string partID, string revisionID, string partDesc, string uoM, double orderQty, DateTime? requiredDate, string orderID, int orderLineID, int orderDeliveryID, double inventoryQty = 0.0, string plantID = "", string plantDept = "", string callID = "", string orgID = "", string shipOrgID = "", string shipLocationID = "", object transaction = null)
	{
		if (transaction == DBNull.Value)
		{
			transaction = null;
		}
		return new Job().CreateJobEx(database, (SqlTransaction)transaction, jobID, partID, revisionID, partDesc, uoM, orderQty, requiredDate, orderID, orderLineID, orderDeliveryID, inventoryQty, plantID, plantDept, callID, orgID, shipOrgID, shipLocationID);
	}

	public DateTime DateAddByDays(string plantID, DateTime date, int daysToChange)
	{
		return ScheduleProcess.DateAddByDays(database, plantID, date, daysToChange);
	}
}
