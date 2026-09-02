using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPLotNumberInformationDto
{
	public string ablAddedByUserID { get; set; }

	public DateTime? ablAddedDate { get; set; }

	public string ablLotNumberID { get; set; }

	public string ablCreatedBy { get; set; }

	public DateTime? ablCreatedDate { get; set; }

	public Guid ablUniqueID { get; set; }

	public DateTime? ablExpirationDate { get; set; }

	public DateTime? ablInactiveDate { get; set; }

	public bool ablInactive { get; set; }

	public string ablPartID { get; set; }

	public string ablPartRevisionID { get; set; }

	public byte[] ablRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
