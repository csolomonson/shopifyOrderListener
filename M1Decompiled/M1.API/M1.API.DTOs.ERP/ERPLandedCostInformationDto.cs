using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLandedCostInformationDto
{
	public string rmcCarrierName { get; set; }

	public DateTime? rmcClosedDate { get; set; }

	public string rmcLandedCostID { get; set; }

	public string rmcConsigneeContactID { get; set; }

	public string rmcConsigneeLocationID { get; set; }

	public string rmcConsigneeOrganizationID { get; set; }

	public string rmcCreatedBy { get; set; }

	public DateTime? rmcCreatedDate { get; set; }

	public string rmcDischargePoint { get; set; }

	public Guid rmcUniqueID { get; set; }

	public short rmcGlFiscalYearID { get; set; }

	public byte rmcGlFiscalYearPeriodID { get; set; }

	public bool rmcChargesComplete { get; set; }

	public bool rmcChargesJournalsCreated { get; set; }

	public bool rmcClosed { get; set; }

	public bool rmcPoInTransitComplete { get; set; }

	public bool rmcPoInTransitJournalsCreated { get; set; }

	public bool rmcPostedToGl { get; set; }

	public bool rmcReversalEntry { get; set; }

	public bool rmcReversed { get; set; }

	public decimal rmcLandedCostChargesTotal { get; set; }

	public DateTime? rmcLandedCostDate { get; set; }

	public decimal rmcLandedCostPurchasesTotal { get; set; }

	public decimal rmcLandedCostReceiptsTotal { get; set; }

	public decimal rmcLandedCostTotal { get; set; }

	public string rmcLoadingPoint { get; set; }

	public string rmcLongDescriptionRtf { get; set; }

	public string rmcLongDescriptionText { get; set; }

	public string rmcPlantDepartmentID { get; set; }

	public string rmcPlantID { get; set; }

	public DateTime? rmcPostedDate { get; set; }

	public string rmcReverseLandedCostID { get; set; }

	public byte[] rmcRowVersion { get; set; }

	public string rmcShipContactID { get; set; }

	public string rmcShipLocationID { get; set; }

	public string rmcShipOrganizationID { get; set; }

	public string rmcTrackingNumber { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
