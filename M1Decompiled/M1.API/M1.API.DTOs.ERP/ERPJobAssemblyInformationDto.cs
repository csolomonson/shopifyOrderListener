using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobAssemblyInformationDto
{
	public byte jmaAssemblyOverlap { get; set; }

	public DateTime? jmaCompletedDate { get; set; }

	public string jmaCreatedBy { get; set; }

	public DateTime? jmaCreatedDate { get; set; }

	public string jmaDocuments { get; set; }

	public Guid jmaUniqueID { get; set; }

	public decimal jmaEstimatedUnitCost { get; set; }

	public decimal jmaInventoryQuantity { get; set; }

	public bool jmaClosed { get; set; }

	public bool jmaIssuedComplete { get; set; }

	public bool jmaProductionComplete { get; set; }

	public bool jmaPullAllFromStock { get; set; }

	public bool jmaReceivedComplete { get; set; }

	public string jmaJobID { get; set; }

	public short jmaLevel { get; set; }

	public decimal jmaOrderQuantity { get; set; }

	public byte jmaOverlapDestinationLink { get; set; }

	public decimal jmaOverlapOffsetTime { get; set; }

	public int jmaOverlapOperationID { get; set; }

	public byte jmaOverlapSourceLink { get; set; }

	public int jmaOverlapSourceOperationID { get; set; }

	public byte jmaOverlapType { get; set; }

	public int jmaParentAssemblyID { get; set; }

	public string jmaPartBinID { get; set; }

	public string jmaPartID { get; set; }

	public string jmaPartLongDescriptionRtf { get; set; }

	public string jmaPartLongDescriptionText { get; set; }

	public string jmaPartRevisionID { get; set; }

	public string jmaPartShortDescription { get; set; }

	public string jmaPartWareHouseLocationID { get; set; }

	public string jmaProductionNotesRTF { get; set; }

	public string jmaProductionNotesText { get; set; }

	public decimal jmaProductionQuantity { get; set; }

	public decimal jmaQuantityCompleted { get; set; }

	public decimal jmaQuantityIssued { get; set; }

	public decimal jmaQuantityPerParent { get; set; }

	public decimal jmaQuantityReceivedToInventory { get; set; }

	public decimal jmaQuantityToInspect { get; set; }

	public decimal jmaQuantityToMake { get; set; }

	public decimal jmaQuantityToPull { get; set; }

	public decimal jmaQuantityToReturn { get; set; }

	public DateTime? jmaReworkDate { get; set; }

	public decimal jmaReworkQuantity { get; set; }

	public byte[] jmaRowVersion { get; set; }

	public DateTime? jmaScheduledDueDate { get; set; }

	public decimal jmaScheduledDueHour { get; set; }

	public DateTime? jmaScheduledStartDate { get; set; }

	public decimal jmaScheduledStartHour { get; set; }

	public decimal jmaScrapQuantity { get; set; }

	public decimal jmaScrapQuantityCompleted { get; set; }

	public int jmaJobAssemblyID { get; set; }

	public string jmaSourceMethodID { get; set; }

	public string jmaSourceRevisionID { get; set; }

	public string jmaUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
