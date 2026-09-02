using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPOrganizationLocSalesPersonInformationDto
{
	public string cmkCreatedBy { get; set; }

	public DateTime? cmkCreatedDate { get; set; }

	public Guid cmkUniqueID { get; set; }

	public string cmkLocationID { get; set; }

	public string cmkOrganizationID { get; set; }

	public decimal cmkPercent { get; set; }

	public byte[] cmkRowVersion { get; set; }

	public string cmkSalesEmployeeID { get; set; }

	public short cmkSequenceID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
