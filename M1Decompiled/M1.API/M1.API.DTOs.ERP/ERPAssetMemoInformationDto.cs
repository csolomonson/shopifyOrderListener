using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPAssetMemoInformationDto
{
	public string fakAssetID { get; set; }

	public string fakCreatedBy { get; set; }

	public DateTime? fakCreatedDate { get; set; }

	public Guid fakUniqueID { get; set; }

	public string fakLongDescriptionRtf { get; set; }

	public string fakLongDescriptionText { get; set; }

	public DateTime? fakMemoDate { get; set; }

	public byte[] fakRowVersion { get; set; }

	public short fakAssetMemoID { get; set; }

	public string fakShortDescription { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
