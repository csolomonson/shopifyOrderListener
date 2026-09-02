using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPChangeLogInformationDto
{
	public DateTime? xagChangeDate { get; set; }

	public string xagChangeType { get; set; }

	public string xagChangeUserID { get; set; }

	public byte[] xagRowVersion { get; set; }

	public int xagChangeLogID { get; set; }

	public string xagTableKeyValues { get; set; }

	public string xagTableName { get; set; }

	public string xagTableNewValues { get; set; }

	public string xagTableOldValues { get; set; }

	public Guid xagTableUniqueID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
