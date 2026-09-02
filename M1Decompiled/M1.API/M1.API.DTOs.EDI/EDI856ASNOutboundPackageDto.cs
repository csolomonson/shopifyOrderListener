using System;
using System.Runtime.Serialization;

namespace M1.API.DTOs.EDI;

[Serializable]
[DataContract(Namespace = "", Name = "edI856ShipmentPackage")]
public class EDI856ASNOutboundPackageDto
{
	[DataMember(Name = "shipmentPackageNo", Order = 1)]
	public int ShipmentPackageNo { get; set; }

	[DataMember(Name = "shipmentPackageQuantity", Order = 2)]
	public decimal ShipmentPackageQuantity { get; set; }

	[DataMember(Name = "numberofLoads", Order = 3)]
	public int NumberofLoads { get; set; }

	[DataMember(Name = "labelNumber", Order = 4)]
	public string LabelNumber { get; set; }

	[DataMember(Name = "packagingCode", Order = 5)]
	public string PackagingCode { get; set; }

	[DataMember(Name = "packageWeight", Order = 6)]
	public decimal PackageWeight { get; set; }

	[DataMember(Name = "packageWeightUOM", Order = 7)]
	public string PackageWeightUOM { get; set; }

	[DataMember(Name = "countryOfManufacture", Order = 8)]
	public string CountryOfManufacture { get; set; }

	[DataMember(Name = "additionalNote", Order = 9)]
	public string AdditionalNote { get; set; }

	public void SetLabelForAdditionalNoteField(string newFieldName)
	{
		object[] customAttributes = typeof(EDI856ASNOutboundPackageDto).GetProperty("AdditionalNote").GetCustomAttributes(typeof(DataMemberAttribute), inherit: true);
		for (int i = 0; i < customAttributes.Length; i++)
		{
			DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)customAttributes[i];
			if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
			{
				dataMemberAttribute.Name = newFieldName;
			}
		}
	}
}
