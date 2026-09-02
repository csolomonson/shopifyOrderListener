using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPPartTransactionCostInformationDto
{
	public decimal intActualUnitDutyCost { get; set; }

	public decimal intActualUnitFreightCost { get; set; }

	public decimal intActualUnitLaborCost { get; set; }

	public decimal intActualUnitMaterialCost { get; set; }

	public decimal intActualUnitMiscCost { get; set; }

	public decimal intActualUnitOverheadCost { get; set; }

	public decimal intActualUnitSubcontractCost { get; set; }

	public byte intCostType { get; set; }

	public string intCreatedBy { get; set; }

	public DateTime? intCreatedDate { get; set; }

	public Guid intUniqueID { get; set; }

	public int intPartTransactionID { get; set; }

	public decimal intPrevUnitDutyCost { get; set; }

	public decimal intPrevUnitFreightCost { get; set; }

	public decimal intPrevUnitLaborCost { get; set; }

	public decimal intPrevUnitMaterialCost { get; set; }

	public decimal intPrevUnitMiscCost { get; set; }

	public decimal intPrevUnitOverheadCost { get; set; }

	public decimal intPrevUnitSubcontractCost { get; set; }

	public decimal intQuantity { get; set; }

	public byte[] intRowVersion { get; set; }

	public int intPartTransactionCostID { get; set; }

	public string intSourceTableName { get; set; }

	public Guid intSourceTableUniqueID { get; set; }

	public decimal intUnitDutyCost { get; set; }

	public decimal intUnitFreightCost { get; set; }

	public decimal intUnitLaborCost { get; set; }

	public decimal intUnitMaterialCost { get; set; }

	public decimal intUnitMiscCost { get; set; }

	public decimal intUnitOverheadCost { get; set; }

	public decimal intUnitSubcontractCost { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
