using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPQuantityAdjustmentInformationDto
{
	public DateTime? inqAdjustmentDate { get; set; }

	public string inqAdjustmentDescription { get; set; }

	public byte inqAdjustmentType { get; set; }

	public decimal inqBinQuantityReceipted { get; set; }

	public decimal inqBinQuantityTransferred { get; set; }

	public decimal inqChangeQuantity { get; set; }

	public string inqQuantityAdjustmentID { get; set; }

	public decimal inqCountedQuantity { get; set; }

	public string inqCreatedBy { get; set; }

	public DateTime? inqCreatedDate { get; set; }

	public decimal inqCurrentQuantity { get; set; }

	public string inqDestinationPartBinID { get; set; }

	public string inqDestinationWarehouseID { get; set; }

	public Guid inqUniqueID { get; set; }

	public bool inqPosted { get; set; }

	public decimal inqNewQuantity { get; set; }

	public string inqPartBinID { get; set; }

	public string inqPartID { get; set; }

	public string inqPartRevisionID { get; set; }

	public string inqPartShortDescription { get; set; }

	public string inqPartWarehouseLocationID { get; set; }

	public string inqPlantDepartmentID { get; set; }

	public string inqPlantID { get; set; }

	public DateTime? inqPostedDate { get; set; }

	public decimal inqQuantitySince { get; set; }

	public byte[] inqRowVersion { get; set; }

	public short inqTransactionsSince { get; set; }

	public string inqUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
