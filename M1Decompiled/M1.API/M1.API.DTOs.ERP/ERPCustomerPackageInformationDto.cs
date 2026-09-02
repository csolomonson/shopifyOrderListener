using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPCustomerPackageInformationDto
{
	public string cpaCustomerPackageID { get; set; }

	public string cpaCreatedBy { get; set; }

	public DateTime? cpaCreatedDate { get; set; }

	public Guid cpaUniqueID { get; set; }

	public DateTime? cpaInactiveDate { get; set; }

	public bool cpaInactive { get; set; }

	public string cpaPackageDescription { get; set; }

	public string cpaPackageDimensionsUom { get; set; }

	public int cpaPackageHeight { get; set; }

	public int cpaPackageLength { get; set; }

	public int cpaPackageWidth { get; set; }

	public byte[] cpaRowVersion { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
