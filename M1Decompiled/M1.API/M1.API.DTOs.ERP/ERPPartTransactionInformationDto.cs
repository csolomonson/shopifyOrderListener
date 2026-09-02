using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartTransactionInformationDto
{
	public DateTime? imtCogsCalculatedDate { get; set; }

	public string imtCreatedBy { get; set; }

	public DateTime? imtCreatedDate { get; set; }

	public Guid imtUniqueID { get; set; }

	public string imtHeatLot { get; set; }

	public string imtInspectionStatus { get; set; }

	public decimal imtInventoryQuantityReceived { get; set; }

	public string imtInventoryUnitOfMeasure { get; set; }

	public bool imtCogsPostedToGl { get; set; }

	public bool imtJobCompleteStatus { get; set; }

	public bool imtNonInventoryTransaction { get; set; }

	public bool imtNonNettable { get; set; }

	public bool imtPoLineReceivedComplete { get; set; }

	public bool imtRequiresInspection { get; set; }

	public byte imtIssueType { get; set; }

	public int imtJobAssemblyID { get; set; }

	public string imtJobID { get; set; }

	public int imtJobMaterialComponentID { get; set; }

	public int imtJobMaterialID { get; set; }

	public int imtJobOperationID { get; set; }

	public byte imtJobType { get; set; }

	public string imtPartBinID { get; set; }

	public string imtPartID { get; set; }

	public string imtPartRevisionID { get; set; }

	public string imtPartWarehouseLocationID { get; set; }

	public string imtPlantID { get; set; }

	public decimal imtPreviousQuantityOnHand { get; set; }

	public string imtProjectAreaID { get; set; }

	public string imtProjectID { get; set; }

	public decimal imtQuantityToInspect { get; set; }

	public decimal imtQuantityToReturn { get; set; }

	public byte imtReceiptType { get; set; }

	public string imtReference { get; set; }

	public byte[] imtRowVersion { get; set; }

	public decimal imtScrapQuantity { get; set; }

	public int imtPartTransactionID { get; set; }

	public decimal imtSetupCharge { get; set; }

	public byte imtSource { get; set; }

	public string imtTableName { get; set; }

	public Guid imtTableUniqueID { get; set; }

	public DateTime? imtTransactionDate { get; set; }

	public byte imtTransactionType { get; set; }

	public string imtUserID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
