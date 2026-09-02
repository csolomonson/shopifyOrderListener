using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPServiceContractMemoInformationDto
{
	public string kbmCreatedBy { get; set; }

	public DateTime? kbmCreatedDate { get; set; }

	public Guid kbmUniqueID { get; set; }

	public string kbmLongDescriptionRtf { get; set; }

	public string kbmLongDescriptionText { get; set; }

	public DateTime? kbmMemoDate { get; set; }

	public byte[] kbmRowVersion { get; set; }

	public short kbmServiceContractMemoID { get; set; }

	public string kbmServiceContractID { get; set; }

	public string kbmShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
