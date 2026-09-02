using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShiftBreakInformationDto
{
	public decimal lmtBreak1EndTime { get; set; }

	public decimal lmtBreak1StartTime { get; set; }

	public decimal lmtBreak2EndTime { get; set; }

	public decimal lmtBreak2StartTime { get; set; }

	public decimal lmtBreak3EndTime { get; set; }

	public decimal lmtBreak3StartTime { get; set; }

	public string lmtCreatedBy { get; set; }

	public DateTime? lmtCreatedDate { get; set; }

	public byte lmtDay { get; set; }

	public decimal lmtEndTime { get; set; }

	public Guid lmtUniqueID { get; set; }

	public bool lmtBreak1Paid { get; set; }

	public bool lmtBreak2Paid { get; set; }

	public bool lmtBreak3Paid { get; set; }

	public byte[] lmtRowVersion { get; set; }

	public short lmtShiftID { get; set; }

	public decimal lmtStartTime { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
