using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMRPJobDetailInformationDto
{
	public string mrjCreatedBy { get; set; }

	public DateTime? mrjCreatedDate { get; set; }

	public string mrjCustomerOrganizationID { get; set; }

	public Guid mrjUniqueID { get; set; }

	public decimal mrjInventoryQuantity { get; set; }

	public bool mrjCompleted { get; set; }

	public bool mrjConsolidated { get; set; }

	public bool mrjDataMissing { get; set; }

	public bool mrjDirectLink { get; set; }

	public bool mrjExistingJob { get; set; }

	public bool mrjFirm { get; set; }

	public bool mrjGetPartMethod { get; set; }

	public bool mrjIndirectLink { get; set; }

	public int mrjJobAssemblyID { get; set; }

	public int mrjJobDetailID { get; set; }

	public string mrjJobID { get; set; }

	public int mrjLineID { get; set; }

	public decimal mrjOrderQuantity { get; set; }

	public string mrjPartBinID { get; set; }

	public string mrjPartID { get; set; }

	public string mrjPartPlantID { get; set; }

	public string mrjPartRevisionID { get; set; }

	public string mrjPartWarehouseLocationID { get; set; }

	public DateTime? mrjProductionDueDate { get; set; }

	public byte[] mrjRowVersion { get; set; }

	public short mrjSalesOrderDeliveryID { get; set; }

	public string mrjSalesOrderID { get; set; }

	public short mrjSalesOrderLineID { get; set; }

	public string mrjSessionID { get; set; }

	public string mrjShipLocationID { get; set; }

	public string mrjShipOrganizationID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
