using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobCostInformationDto
{
	public string jmcApInvoiceID { get; set; }

	public short jmcApInvoiceLineID { get; set; }

	public int jmcCostSequence { get; set; }

	public string jmcCreatedBy { get; set; }

	public DateTime? jmcCreatedDate { get; set; }

	public Guid jmcUniqueID { get; set; }

	public string jmcHeatLot { get; set; }

	public int jmcJobAssemblyID { get; set; }

	public string jmcJobID { get; set; }

	public int jmcJobMaterialComponentID { get; set; }

	public int jmcJobMaterialID { get; set; }

	public int jmcJobOperationID { get; set; }

	public int jmcJobSequence { get; set; }

	public byte jmcJobType { get; set; }

	public string jmcPartDescription { get; set; }

	public string jmcPartID { get; set; }

	public string jmcPartRevisionID { get; set; }

	public decimal jmcQuantityReceived { get; set; }

	public short jmcReceiptComponentID { get; set; }

	public string jmcReceiptID { get; set; }

	public short jmcReceiptLineID { get; set; }

	public string jmcReceivedUnitOfMeasure { get; set; }

	public string jmcReference { get; set; }

	public byte[] jmcRowVersion { get; set; }

	public byte jmcSource { get; set; }

	public string jmcSupplierOrganizationID { get; set; }

	public decimal jmcTotalCogsCost { get; set; }

	public decimal jmcTotalCost { get; set; }

	public DateTime? jmcTransactionDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
