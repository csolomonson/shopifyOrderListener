using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMRPDemandInformationDto
{
	public string mrrCreatedBy { get; set; }

	public DateTime? mrrCreatedDate { get; set; }

	public string mrrCustomerOrganizationID { get; set; }

	public int mrrDemandID { get; set; }

	public decimal mrrDemandQuantity { get; set; }

	public DateTime? mrrDueDate { get; set; }

	public Guid mrrUniqueID { get; set; }

	public int mrrJobAssemblyID { get; set; }

	public string mrrJobID { get; set; }

	public int mrrJobMaterialID { get; set; }

	public int mrrLineID { get; set; }

	public decimal mrrOriginalQuantity { get; set; }

	public string mrrPartBinID { get; set; }

	public string mrrPartID { get; set; }

	public string mrrPartPlantID { get; set; }

	public string mrrPartRevisionID { get; set; }

	public string mrrPartWarehouseLocationID { get; set; }

	public decimal mrrQuantityReceived { get; set; }

	public decimal mrrQuantityShipped { get; set; }

	public byte[] mrrRowVersion { get; set; }

	public short mrrSalesOrderDeliveryID { get; set; }

	public string mrrSalesOrderID { get; set; }

	public short mrrSalesOrderLineID { get; set; }

	public string mrrSessionID { get; set; }

	public string mrrShipLocationID { get; set; }

	public string mrrShipOrganizationID { get; set; }

	public string mrrSource { get; set; }

	public string mrrType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
