using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPToolMovementInformationDto
{
	public string xtaCheckedOutToEmployeeID { get; set; }

	public string xtaCheckoutReasonID { get; set; }

	public string xtaCreatedBy { get; set; }

	public DateTime? xtaCreatedDate { get; set; }

	public Guid xtaUniqueID { get; set; }

	public string xtaJobID { get; set; }

	public string xtaLocation { get; set; }

	public DateTime? xtaMovementDate { get; set; }

	public string xtaMovementType { get; set; }

	public string xtaNotesRTF { get; set; }

	public string xtaNotesText { get; set; }

	public DateTime? xtaPlannedReturnDate { get; set; }

	public string xtaPlantDepartmentID { get; set; }

	public string xtaPlantID { get; set; }

	public string xtaProductionDepartmentID { get; set; }

	public byte[] xtaRowVersion { get; set; }

	public int xtaToolMovementID { get; set; }

	public string xtaToolID { get; set; }

	public string xtaWorkCenterID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
