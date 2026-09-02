using System;
using System.Collections.Generic;

namespace M1.API.DTOs.ERP;

public class ERPFreightPackageInformationDto
{
	public string fslCreatedBy { get; set; }

	public DateTime? fslCreatedDate { get; set; }

	public string fslDimensionsUnitOfMeasure { get; set; }

	public byte fslDistributeCostsOption { get; set; }

	public Guid fslUniqueID { get; set; }

	public int fslFdxPackageHeight { get; set; }

	public int fslFdxPackageLength { get; set; }

	public int fslFdxPackageWidth { get; set; }

	public string fslFdxPackaging { get; set; }

	public string fslFreightShipmentID { get; set; }

	public bool fslFdxNonstandardContainer { get; set; }

	public bool fslVoidOnUps { get; set; }

	public string fslNotesRTF { get; set; }

	public string fslNotesText { get; set; }

	public decimal fslPackageCharge { get; set; }

	public decimal fslPackageFullWeight { get; set; }

	public decimal fslPackagePublishedCharge { get; set; }

	public byte[] fslRowVersion { get; set; }

	public short fslFreightPackageID { get; set; }

	public string fslTrackingNumber { get; set; }

	public string fslUpsPackageType { get; set; }

	public string fslWeightUnitOfMeasure { get; set; }

	public IDictionary<string, object> CustomFields { get; set; }
}
