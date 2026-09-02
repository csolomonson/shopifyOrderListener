using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPWarehouseInformationDto
{
	public string imwAddressLine1 { get; set; }

	public string imwAddressLine2 { get; set; }

	public string imwAddressLine3 { get; set; }

	public string imwCity { get; set; }

	public string imwWarehouseID { get; set; }

	public string imwCountry { get; set; }

	public string imwCreatedBy { get; set; }

	public DateTime? imwCreatedDate { get; set; }

	public int imwDefaultBinCount { get; set; }

	public string imwEmailAddress { get; set; }

	public Guid imwUniqueID { get; set; }

	public DateTime? imwEstablishedDate { get; set; }

	public string imwFaxNumber { get; set; }

	public DateTime? imwInactiveDate { get; set; }

	public bool imwInactive { get; set; }

	public bool imwAvalaraAddressValidated { get; set; }

	public bool imwDefaultWarehouse { get; set; }

	public bool imwDoNotIncludeInJobCosts { get; set; }

	public bool imwNonNettable { get; set; }

	public string imwName { get; set; }

	public byte imwNonNettableType { get; set; }

	public string imwPhoneNumber { get; set; }

	public string imwPlantDepartmentID { get; set; }

	public string imwPlantID { get; set; }

	public string imwPostCode { get; set; }

	public byte[] imwRowVersion { get; set; }

	public string imwShippingMethodID { get; set; }

	public string imwState { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
