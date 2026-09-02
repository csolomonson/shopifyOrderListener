using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPMRPSupplyInformationDto
{
	public string mrsCreatedBy { get; set; }

	public DateTime? mrsCreatedDate { get; set; }

	public string mrsCustomerOrganizationID { get; set; }

	public DateTime? mrsDueDate { get; set; }

	public Guid mrsUniqueID { get; set; }

	public int mrsJobAssemblyID { get; set; }

	public string mrsJobID { get; set; }

	public int mrsLineID { get; set; }

	public string mrsPartBinID { get; set; }

	public string mrsPartID { get; set; }

	public string mrsPartRevisionID { get; set; }

	public string mrsPartWarehouseLocationID { get; set; }

	public decimal mrsQuantityReceived { get; set; }

	public decimal mrsQuantityShipped { get; set; }

	public byte[] mrsRowVersion { get; set; }

	public string mrsSessionID { get; set; }

	public string mrsSource { get; set; }

	public int mrsSupplyID { get; set; }

	public string mrsType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
