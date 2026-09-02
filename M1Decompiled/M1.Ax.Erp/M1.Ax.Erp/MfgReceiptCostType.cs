using System.Data;
using M1.Core;

namespace M1.Ax.Erp;

public class MfgReceiptCostType
{
	private const string UnitLaborCost = "rmmUnitLaborCost";

	private const string UnitOverheadCost = "rmmUnitOverheadCost";

	private const string UnitMaterialCost = "rmmUnitMaterialCost";

	private const string UnitSubcontractCost = "rmmUnitSubcontractCost";

	private readonly DataRow _dataRow;

	private readonly M1Database _database;

	public MfgReceiptCostType(DataRow dataRow, M1Database database)
	{
		_database = database;
		_dataRow = dataRow;
	}

	public void UseActualJobCosts()
	{
		string jobID = _dataRow.Field<string>("rmmJobId");
		int assemblyID = _dataRow.Field<int>("rmmJobAssemblyID");
		decimal qtyCompleted = _dataRow.Field<decimal>("rmmQuantityCompleted");
		JobCost jobCosts = new Job().GetJobCosts(_database, null, jobID, assemblyID, qtyCompleted, 0);
		if (jobCosts == null)
		{
			SetCostsToZero();
			return;
		}
		_dataRow.SetField("rmmUnitLaborCost", jobCosts.LaborCost);
		_dataRow.SetField("rmmUnitOverheadCost", jobCosts.OverheadCost);
		_dataRow.SetField("rmmUnitMaterialCost", jobCosts.MaterialCost);
		_dataRow.SetField("rmmUnitSubcontractCost", jobCosts.SubcontractCost);
	}

	public void UsePartRevisionCosts()
	{
		string partID = _dataRow.Field<string>("rmmPartID");
		string partRevisionID = _dataRow.Field<string>("rmmPartRevisionID");
		PartCost partCosts = new Part().GetPartCosts(_database, null, partID, partRevisionID);
		if (partCosts == null)
		{
			SetCostsToZero();
			return;
		}
		_dataRow.SetField("rmmUnitLaborCost", partCosts.LaborCost);
		_dataRow.SetField("rmmUnitOverheadCost", partCosts.OverheadCost);
		_dataRow.SetField("rmmUnitMaterialCost", partCosts.MaterialCost);
		_dataRow.SetField("rmmUnitSubcontractCost", partCosts.SubcontractCost);
	}

	public void UseEstimatedJobCost()
	{
		string jobId = _dataRow.Field<string>("rmmJobId");
		int jobAssemblyId = _dataRow.Field<int>("rmmJobAssemblyID");
		decimal num = _dataRow.Field<decimal>("rmmProductionQuantity");
		decimal num2 = _dataRow.Field<decimal>("rmmInventoryQuantity");
		decimal value = default(decimal);
		decimal value2 = default(decimal);
		decimal value3 = default(decimal);
		decimal value4 = default(decimal);
		if (num != 0m || num2 != 0m)
		{
			MfgReceiptEstimatedJobCost mfgReceiptEstimatedJobCost = new MfgReceiptEstimatedJobCost(jobId, jobAssemblyId, _database);
			value = mfgReceiptEstimatedJobCost.CalculateUnitLaborCost();
			value2 = mfgReceiptEstimatedJobCost.CalculateUnitOverheadCost();
			value3 = mfgReceiptEstimatedJobCost.CalculateUnitMaterialCost();
			value4 = mfgReceiptEstimatedJobCost.CalculateUnitSubContractCost();
		}
		_dataRow.SetField("rmmUnitLaborCost", value);
		_dataRow.SetField("rmmUnitOverheadCost", value2);
		_dataRow.SetField("rmmUnitMaterialCost", value3);
		_dataRow.SetField("rmmUnitSubcontractCost", value4);
	}

	public void UseManualOverride()
	{
		decimal num = _dataRow.Field<decimal>("rmmInventoryQuantityReceived");
		decimal num2 = _dataRow.Field<decimal>("rmmQuantityToInspect");
		if (num + num2 == 0m)
		{
			SetCostsToZero();
		}
	}

	private void SetCostsToZero()
	{
		_dataRow.SetField("rmmUnitLaborCost", (byte)0);
		_dataRow.SetField("rmmUnitOverheadCost", (byte)0);
		_dataRow.SetField("rmmUnitMaterialCost", (byte)0);
		_dataRow.SetField("rmmUnitSubcontractCost", (byte)0);
	}
}
