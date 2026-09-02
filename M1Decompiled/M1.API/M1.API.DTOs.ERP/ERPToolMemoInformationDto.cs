using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPToolMemoInformationDto
{
	public string xtmCreatedBy { get; set; }

	public DateTime? xtmCreatedDate { get; set; }

	public Guid xtmUniqueID { get; set; }

	public string xtmLongDescriptionRtf { get; set; }

	public string xtmLongDescriptionText { get; set; }

	public DateTime? xtmMemoDate { get; set; }

	public byte[] xtmRowVersion { get; set; }

	public short xtmToolMemoID { get; set; }

	public string xtmShortDescription { get; set; }

	public string xtmToolID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
