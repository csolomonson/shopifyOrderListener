using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPRecentActivitiesLogInformationDto
{
	public int rtlCount { get; set; }

	public string rtlExplorerType { get; set; }

	public DateTime rtlLastOpenedDateTime { get; set; }

	public string rtlObjectDataRun { get; set; }

	public string rtlObjectID { get; set; }

	public string rtlObjectName { get; set; }

	public string rtlParentKey { get; set; }

	public int rtlRecentActivityID { get; set; }

	public byte[] rtlRowVersion { get; set; }

	public string rtlUserID { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
