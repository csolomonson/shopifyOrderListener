using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM.Job;

[Serializable]
[DataContract(Namespace = "", Name = "joboperation")]
[XmlRoot(ElementName = "joboperation")]
[XmlType(AnonymousType = true)]
public class BOMJobOperationDto
{
	[XmlElement(ElementName = "jobID")]
	[DataMember(Name = "jobID", Order = 1)]
	[Required(ErrorMessage = "JobID is invalid or empty.")]
	public string JobID { get; set; }

	[XmlElement(ElementName = "jobAssemblyID")]
	[DataMember(Name = "jobAssemblyID", Order = 2)]
	[Required(ErrorMessage = "JobAssemblyID is invalid or empty.")]
	public int JobAssemblyID { get; set; }

	[XmlElement(ElementName = "jobOperationID")]
	[DataMember(Name = "jobOperationID", Order = 3)]
	[Required(ErrorMessage = "JobOperationID is invalid or empty.")]
	public int JobOperationID { get; set; }

	[XmlElement(ElementName = "operationType")]
	[DataMember(Name = "operationType", Order = 4)]
	[Required(ErrorMessage = "OperationType is invalid or empty.")]
	public byte OperationType { get; set; }

	[XmlElement(ElementName = "workCenterID")]
	[DataMember(Name = "workCenterID", Order = 5)]
	[Required(ErrorMessage = "WorkCenterID is invalid or empty.")]
	public string WorkCenterID { get; set; }

	[XmlElement(ElementName = "processID")]
	[DataMember(Name = "processID", Order = 6)]
	[Required(ErrorMessage = "ProcessID is invalid or empty.")]
	public string ProcessID { get; set; }

	[XmlElement(ElementName = "processShortDescription")]
	[DataMember(Name = "processShortDescription", Order = 7)]
	[Required(ErrorMessage = "ProcessShortDescription is invalid or empty.")]
	public string ProcessShortDescription { get; set; }

	[XmlElement(ElementName = "productionStandard")]
	[DataMember(Name = "productionStandard", Order = 8)]
	public decimal ProductionStandard { get; set; }

	[XmlElement(ElementName = "standardFactor")]
	[DataMember(Name = "standardFactor", Order = 9)]
	[Required(ErrorMessage = "StandardFactor is invalid or empty.")]
	public string StandardFactor { get; set; }

	[XmlElement(ElementName = "machinesToSchedule")]
	[DataMember(Name = "machinesToSchedule", Order = 10)]
	public short MachinesToSchedule { get; set; }

	[XmlElement(ElementName = "machineType")]
	[DataMember(Name = "machineType", Order = 11)]
	[Required(ErrorMessage = "MachineType is invalid or empty.")]
	public byte MachineType { get; set; }

	[XmlElement(ElementName = "quantityPerAssembly")]
	[DataMember(Name = "quantityPerAssembly", Order = 12)]
	[Required(ErrorMessage = "QuantityPerAssembly is invalid or empty.")]
	public decimal QuantityPerAssembly { get; set; }

	[XmlElement(ElementName = "quantityComplete")]
	[DataMember(Name = "quantityComplete", Order = 13)]
	public decimal QuantityComplete { get; set; }

	[XmlElement(ElementName = "operationQuantity")]
	[DataMember(Name = "operationQuantity", Order = 14)]
	[Required(ErrorMessage = "OperationQuantity is invalid or empty.")]
	public decimal OperationQuantity { get; set; }

	[XmlElement(ElementName = "setupRate")]
	[DataMember(Name = "setupRate", Order = 15)]
	public decimal SetupRate { get; set; }

	[XmlElement(ElementName = "productionRate")]
	[DataMember(Name = "productionRate", Order = 16)]
	public decimal ProductionRate { get; set; }

	[XmlElement(ElementName = "overheadRate")]
	[DataMember(Name = "overheadRate", Order = 17)]
	public decimal OverheadRate { get; set; }

	[XmlElement(ElementName = "partID")]
	[DataMember(Name = "partID", Order = 18)]
	public string PartID { get; set; }

	[XmlElement(ElementName = "partRevisionID")]
	[DataMember(Name = "partRevisionID", Order = 19)]
	public string PartRevisionID { get; set; }

	[XmlElement(ElementName = "partWarehouseLocationID")]
	[DataMember(Name = "partWarehouseLocationID", Order = 20)]
	public string PartWarehouseLocationID { get; set; }

	[XmlElement(ElementName = "partBinID")]
	[DataMember(Name = "partBinID", Order = 21)]
	public string PartBinID { get; set; }

	[XmlElement(ElementName = "unitOfMeasure")]
	[DataMember(Name = "unitOfMeasure", Order = 22)]
	public string UnitOfMeasure { get; set; }

	[XmlElement(ElementName = "plantID")]
	[DataMember(Name = "plantID", Order = 23)]
	public string PlantID { get; set; }

	[XmlElement(ElementName = "closed")]
	[DataMember(Name = "closed", Order = 24)]
	public bool Closed { get; set; }

	[XmlElement(ElementName = "dueDate")]
	[DataMember(Name = "dueDate", Order = 25)]
	public DateTime? DueDate { get; set; }
}
