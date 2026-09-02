using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPScheduleTreeInformationDto
{
	public string sxtCreatedBy { get; set; }

	public DateTime? sxtCreatedDate { get; set; }

	public string sxtDescription { get; set; }

	public Guid sxtUniqueID { get; set; }

	public Guid sxtGroupUniqueID { get; set; }

	public string sxtJobScenarioID { get; set; }

	public byte[] sxtRowVersion { get; set; }

	public int sxtScheduleTreeID { get; set; }

	public string sxtSourceTable { get; set; }

	public Guid sxtSourceUniqueID { get; set; }

	public byte sxtType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
