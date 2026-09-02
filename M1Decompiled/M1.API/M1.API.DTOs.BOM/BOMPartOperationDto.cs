using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "partoperation")]
[XmlRoot(ElementName = "partoperation")]
[XmlType(AnonymousType = true)]
public class BOMPartOperationDto
{
	[XmlElement(ElementName = "methodID")]
	[DataMember(Name = "methodID", Order = 1)]
	[Required(ErrorMessage = "MethodID is invalid or empty.")]
	public string MethodID { get; set; }

	[XmlElement(ElementName = "methodRevisionID")]
	[DataMember(Name = "methodRevisionID", Order = 2)]
	public string MethodRevisionID { get; set; }

	[XmlElement(ElementName = "methodAssemblyID")]
	[DataMember(Name = "methodAssemblyID", Order = 3)]
	[Required(ErrorMessage = "MethodAssemblyID is invalid or empty.")]
	public int MethodAssemblyID { get; set; }

	[XmlElement(ElementName = "methodOperationID")]
	[DataMember(Name = "methodOperationID", Order = 4)]
	[Required(ErrorMessage = "MethodOperationID is invalid or empty.")]
	public int MethodOperationID { get; set; }

	[XmlElement(ElementName = "operationType")]
	[DataMember(Name = "operationType", Order = 5)]
	[Required(ErrorMessage = "OperationType is invalid or empty.")]
	public byte OperationType { get; set; }

	[XmlElement(ElementName = "workCenterID")]
	[DataMember(Name = "workCenterID", Order = 6)]
	[Required(ErrorMessage = "WorkCenterID is invalid or empty.")]
	public string WorkCenterID { get; set; }

	[XmlElement(ElementName = "processID")]
	[DataMember(Name = "processID", Order = 7)]
	[Required(ErrorMessage = "ProcessID is invalid or empty.")]
	public string ProcessID { get; set; }

	[XmlElement(ElementName = "processShortDescription")]
	[DataMember(Name = "processShortDescription", Order = 8)]
	[Required(ErrorMessage = "ProcessShortDescription is invalid or empty.")]
	public string ProcessShortDescription { get; set; }

	[XmlElement(ElementName = "productionStandard")]
	[DataMember(Name = "productionStandard", Order = 9)]
	public decimal ProductionStandard { get; set; }

	[XmlElement(ElementName = "standardFactor")]
	[DataMember(Name = "standardFactor", Order = 10)]
	[Required(ErrorMessage = "StandardFactor is invalid or empty.")]
	public string StandardFactor { get; set; }

	[XmlElement(ElementName = "machinesToSchedule")]
	[DataMember(Name = "machinesToSchedule", Order = 11)]
	public short MachinesToSchedule { get; set; }

	[XmlElement(ElementName = "machineType")]
	[DataMember(Name = "machineType", Order = 12)]
	[Required(ErrorMessage = "MachineType is invalid or empty.")]
	public byte MachineType { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 13)]
	[Required(ErrorMessage = "QuantityPerAssembly is invalid or empty.")]
	public decimal QuantityPerAssembly { get; set; }
}
