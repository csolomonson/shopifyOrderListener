using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPChangeRequestInformationDto
{
	public decimal chpActualHours { get; set; }

	public DateTime? chpAssignedDate { get; set; }

	public string chpAssignedToEmployeeID { get; set; }

	public string chpAuthorizedByEmployeeID { get; set; }

	public DateTime? chpAuthorizedDate { get; set; }

	public string chpChangeRequestTypeID { get; set; }

	public string chpClosedByEmployeeID { get; set; }

	public DateTime? chpClosedDate { get; set; }

	public string chpClosedReasonID { get; set; }

	public string chpChangeRequestID { get; set; }

	public string chpCreatedBy { get; set; }

	public DateTime? chpCreatedDate { get; set; }

	public DateTime? chpDueDate { get; set; }

	public Guid chpUniqueID { get; set; }

	public decimal chpEstimatedHours { get; set; }

	public string chpJobID { get; set; }

	public string chpLongDescriptionRtf { get; set; }

	public string chpLongDescriptionText { get; set; }

	public string chpNonConformanceID { get; set; }

	public string chpOpenedByEmployeeID { get; set; }

	public DateTime? chpOpenedDate { get; set; }

	public string chpPartID { get; set; }

	public string chpPartRevisionID { get; set; }

	public byte chpPriorityID { get; set; }

	public string chpProjectAreaID { get; set; }

	public string chpProjectID { get; set; }

	public string chpResolvedPartID { get; set; }

	public string chpResolvedPartRevisionID { get; set; }

	public byte[] chpRowVersion { get; set; }

	public string chpShortDescription { get; set; }

	public string chpStatus { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
