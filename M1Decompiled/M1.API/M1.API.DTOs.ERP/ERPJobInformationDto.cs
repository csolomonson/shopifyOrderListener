using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPJobInformationDto
{
	public string jmpCallID { get; set; }

	public DateTime? jmpClosedDate { get; set; }

	public string jmpJobID { get; set; }

	public DateTime? jmpCompletedDate { get; set; }

	public string jmpCreatedBy { get; set; }

	public DateTime? jmpCreatedDate { get; set; }

	public string jmpCustomerOrganizationID { get; set; }

	public string jmpDocuments { get; set; }

	public Guid jmpUniqueID { get; set; }

	public decimal jmpInventoryQuantity { get; set; }

	public bool jmpClosed { get; set; }

	public bool jmpFirm { get; set; }

	public bool jmpNestlinkProcessed { get; set; }

	public bool jmpOnHold { get; set; }

	public bool jmpPlanningComplete { get; set; }

	public bool jmpProductionComplete { get; set; }

	public bool jmpReadyToPrint { get; set; }

	public bool jmpReleasedToFloor { get; set; }

	public bool jmpScheduleComplete { get; set; }

	public bool jmpScheduleLocked { get; set; }

	public bool jmpTimeAndMaterial { get; set; }

	public DateTime? jmpJobDate { get; set; }

	public short jmpJobPriorityID { get; set; }

	public string jmpNonConformanceID { get; set; }

	public decimal jmpOrderQuantity { get; set; }

	public string jmpPartBinID { get; set; }

	public short jmpPartForecastPeriodID { get; set; }

	public short jmpPartForecastYearID { get; set; }

	public string jmpPartID { get; set; }

	public string jmpPartLongDescriptionRtf { get; set; }

	public string jmpPartLongDescriptionText { get; set; }

	public string jmpPartRevisionID { get; set; }

	public string jmpPartShortDescription { get; set; }

	public string jmpPartWareHouseLocationID { get; set; }

	public string jmpPlannerEmployeeID { get; set; }

	public string jmpPlantDepartmentID { get; set; }

	public string jmpPlantID { get; set; }

	public DateTime? jmpProductionDueDate { get; set; }

	public string jmpProductionNotesRTF { get; set; }

	public string jmpProductionNotesText { get; set; }

	public decimal jmpProductionQuantity { get; set; }

	public string jmpProjectAreaID { get; set; }

	public string jmpProjectID { get; set; }

	public decimal jmpQuantityCompleted { get; set; }

	public decimal jmpQuantityReceivedToInventory { get; set; }

	public decimal jmpQuantityShipped { get; set; }

	public string jmpQuoteID { get; set; }

	public short jmpQuoteLineID { get; set; }

	public DateTime? jmpReworkDate { get; set; }

	public decimal jmpReworkQuantity { get; set; }

	public string jmpRmaClaimID { get; set; }

	public short jmpRmaClaimLineID { get; set; }

	public byte[] jmpRowVersion { get; set; }

	public DateTime? jmpScheduledDueDate { get; set; }

	public decimal jmpScheduledDueHour { get; set; }

	public DateTime? jmpScheduledStartDate { get; set; }

	public decimal jmpScheduledStartHour { get; set; }

	public decimal jmpScrapQuantity { get; set; }

	public decimal jmpScrapQuantityCompleted { get; set; }

	public string jmpShipLocationID { get; set; }

	public string jmpShipOrganizationID { get; set; }

	public string jmpSourceMethodID { get; set; }

	public string jmpSourceRevisionID { get; set; }

	public string jmpUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
