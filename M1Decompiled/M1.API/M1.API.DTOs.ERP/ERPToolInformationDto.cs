using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPToolInformationDto
{
	public string xttAssetID { get; set; }

	public string xttCheckedOutToEmployeeID { get; set; }

	public string xttCheckoutReasonID { get; set; }

	public string xttToolID { get; set; }

	public string xttCreatedBy { get; set; }

	public DateTime? xttCreatedDate { get; set; }

	public string xttDescription { get; set; }

	public string xttDocuments { get; set; }

	public Guid xttUniqueID { get; set; }

	public string xttIdentificationNumber { get; set; }

	public DateTime? xttInactiveDate { get; set; }

	public bool xttInactive { get; set; }

	public string xttLocation { get; set; }

	public string xttLongDescriptionRtf { get; set; }

	public string xttLongDescriptionText { get; set; }

	public DateTime? xttMovementDate { get; set; }

	public string xttMovementType { get; set; }

	public DateTime? xttPlannedReturnDate { get; set; }

	public byte[] xttRowVersion { get; set; }

	public string xttToolCategoryID { get; set; }

	public string xttWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
