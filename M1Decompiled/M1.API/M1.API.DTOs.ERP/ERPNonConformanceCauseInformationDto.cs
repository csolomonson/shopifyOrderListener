using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPNonConformanceCauseInformationDto
{
	public string qauNonConformanceCauseID { get; set; }

	public string qauCreatedBy { get; set; }

	public DateTime? qauCreatedDate { get; set; }

	public string qauDescription { get; set; }

	public Guid qauUniqueID { get; set; }

	public byte[] qauRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
