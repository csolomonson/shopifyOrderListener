using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPProcessInformationDto
{
	public string xacProcessID { get; set; }

	public string xacCreatedBy { get; set; }

	public DateTime? xacCreatedDate { get; set; }

	public Guid xacUniqueID { get; set; }

	public DateTime? xacInactiveDate { get; set; }

	public byte xacInspectionType { get; set; }

	public bool xacInactive { get; set; }

	public bool xacExcludeFromTMJobs { get; set; }

	public bool xacIgnoreCalendarMove { get; set; }

	public bool xacIgnoreCalendarQueue { get; set; }

	public bool xacPrintInspectionLine { get; set; }

	public string xacLongDescriptionRtf { get; set; }

	public string xacLongDescriptionText { get; set; }

	public decimal xacProductionStandard { get; set; }

	public decimal xacProjectedProductionRate { get; set; }

	public decimal xacProjectedSetupRate { get; set; }

	public byte[] xacRowVersion { get; set; }

	public decimal xacSetupHours { get; set; }

	public string xacShortDescription { get; set; }

	public string xacStandardFactor { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
