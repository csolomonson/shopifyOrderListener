using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCallLineInformationDto
{
	public string kblAddedByEmployeeID { get; set; }

	public DateTime? kblAddedDate { get; set; }

	public string kblCallID { get; set; }

	public string kblContactMethodID { get; set; }

	public string kblCreatedBy { get; set; }

	public DateTime? kblCreatedDate { get; set; }

	public Guid kblUniqueID { get; set; }

	public decimal kblExtraTime { get; set; }

	public bool kblBillable { get; set; }

	public bool kblCreatedFromMobile { get; set; }

	public bool kblInbound { get; set; }

	public bool kblInternalOnly { get; set; }

	public string kblLongDescriptionRtf { get; set; }

	public string kblLongDescriptionText { get; set; }

	public byte[] kblRowVersion { get; set; }

	public short kblCallLineID { get; set; }

	public string kblShortDescription { get; set; }

	public decimal kblTimeSpent { get; set; }

	public decimal kblTotalTime { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
