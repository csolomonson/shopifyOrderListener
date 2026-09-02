using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPShiftInformationDto
{
	public DateTime? lmsAutoClockOutLastRunTime { get; set; }

	public decimal lmsAutoClockOutTime { get; set; }

	public short lmsClockInWindow { get; set; }

	public short lmsClockOutWindow { get; set; }

	public string lmsCreatedBy { get; set; }

	public DateTime? lmsCreatedDate { get; set; }

	public string lmsDescription { get; set; }

	public Guid lmsUniqueID { get; set; }

	public short lmsGraceTimeIn { get; set; }

	public short lmsGraceTimeOut { get; set; }

	public string lmsIdleTimeIndirectLaborID { get; set; }

	public string lmsIdleTimeWorkCenterID { get; set; }

	public DateTime? lmsInactiveDate { get; set; }

	public bool lmsInactive { get; set; }

	public bool lmsRoundClockWithInShift { get; set; }

	public bool lmsRoundJobsOutsideOfShift { get; set; }

	public bool lmsRoundJobsWithinShift { get; set; }

	public bool lmsRoundOutsideOfShift { get; set; }

	public string lmsPlantID { get; set; }

	public string lmsRoundClockInDirection { get; set; }

	public string lmsRoundClockOutDirection { get; set; }

	public byte lmsRoundTo { get; set; }

	public byte[] lmsRowVersion { get; set; }

	public short lmsShiftID { get; set; }

	public byte lmsShiftGroup { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
