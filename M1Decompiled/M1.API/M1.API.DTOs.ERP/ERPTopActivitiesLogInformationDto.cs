using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPTopActivitiesLogInformationDto
{
	public int rxlCount { get; set; }

	public string rxlExplorerType { get; set; }

	public string rxlGridID { get; set; }

	public string rxlObjectDataRun { get; set; }

	public string rxlObjectName { get; set; }

	public DateTime rxlProcessedDateTime { get; set; }

	public byte[] rxlRowVersion { get; set; }

	public int rxlTopActivityID { get; set; }

	public string rxlUserID { get; set; }

	public string rxlVisualizerID { get; set; }

	public string rxlVisualizerType { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
