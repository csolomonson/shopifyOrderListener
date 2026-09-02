using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPInspectionLineApprovalInformationDto
{
	public string qaaApprovalEmployeeID { get; set; }

	public string qaaCreatedBy { get; set; }

	public DateTime? qaaCreatedDate { get; set; }

	public string qaaDescription { get; set; }

	public Guid qaaUniqueID { get; set; }

	public string qaaInspectionID { get; set; }

	public short qaaInspectionLineID { get; set; }

	public byte qaaInspectionLineApprovalID { get; set; }

	public byte qaaStatus { get; set; }

	public DateTime? qaaStatusDate { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
