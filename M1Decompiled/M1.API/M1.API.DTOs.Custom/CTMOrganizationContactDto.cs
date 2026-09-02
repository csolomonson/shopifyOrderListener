using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.Custom;

[Serializable]
[DataContract(Namespace = "", Name = "organizationcontact")]
[XmlRoot(ElementName = "organizationcontact")]
[XmlType(AnonymousType = true)]
public class CTMOrganizationContactDto
{
	[XmlElement(ElementName = "organizationID")]
	[DataMember(Name = "organizationID", Order = 1)]
	[Required(ErrorMessage = "OrganizationID is invalid or empty.")]
	public string OrganizationID { get; set; }

	[XmlElement(ElementName = "locationID")]
	[DataMember(Name = "locationID", Order = 2)]
	public string LocationID { get; set; }

	[XmlElement(ElementName = "contactID")]
	[DataMember(Name = "contactID", Order = 3)]
	[Required(ErrorMessage = "ContactID is invalid or empty.")]
	public string ContactID { get; set; }

	[XmlElement(ElementName = "name")]
	[DataMember(Name = "name", Order = 4)]
	[Required(ErrorMessage = "Name is invalid or empty.")]
	public string Name { get; set; }

	[XmlElement(ElementName = "phoneNumber")]
	[DataMember(Name = "phoneNumber", Order = 5)]
	public string PhoneNumber { get; set; }

	[XmlElement(ElementName = "mobileNumber")]
	[DataMember(Name = "mobileNumber", Order = 6)]
	public string MobileNumber { get; set; }

	[XmlElement(ElementName = "emailAddress")]
	[DataMember(Name = "emailAddress", Order = 7)]
	public string EmailAddress { get; set; }

	[XmlElement(ElementName = "correspondenceMethod")]
	[DataMember(Name = "correspondenceMethod", Order = 8)]
	public string CorrespondenceMethod { get; set; }

	[XmlElement(ElementName = "inactive")]
	[DataMember(Name = "inactive", Order = 9)]
	public bool Inactive { get; set; }

	[XmlElement(ElementName = "inactiveDate")]
	[DataMember(Name = "inactiveDate", Order = 10)]
	public DateTime? InactiveDate { get; set; }

	[XmlElement(ElementName = "easyorderenabled")]
	[DataMember(Name = "easyorderenabled", Order = 11)]
	public bool EasyOrderEnabled { get; set; }

	[XmlElement(ElementName = "createdbyeasyorder")]
	[DataMember(Name = "createdbyeasyorder", Order = 12)]
	public bool CreatedByEasyOrder { get; set; }

	[XmlElement(ElementName = "eoffirstname")]
	[DataMember(Name = "eoffirstname", Order = 13)]
	public string EOFirstName { get; set; }

	[XmlElement(ElementName = "eoinitials")]
	[DataMember(Name = "eoinitials", Order = 14)]
	public string EOInitials { get; set; }

	[XmlElement(ElementName = "eoprefix")]
	[DataMember(Name = "eoprefix", Order = 15)]
	public string EOPrefix { get; set; }

	[XmlElement(ElementName = "eosurname")]
	[DataMember(Name = "eosurname", Order = 16)]
	public string EOSurname { get; set; }

	[XmlElement(ElementName = "eopassword")]
	[DataMember(Name = "eopassword", Order = 17)]
	public string EOPassword { get; set; }

	[XmlElement(ElementName = "eouserrole")]
	[DataMember(Name = "eouserrole", Order = 18)]
	public string EOUserRole { get; set; }

	[XmlElement(ElementName = "eodefsupervisor")]
	[DataMember(Name = "eodefsupervisor", Order = 19)]
	public string EODefSupervisor { get; set; }

	[XmlElement(ElementName = "eosubsupervisor")]
	[DataMember(Name = "eosubsupervisor", Order = 20)]
	public string EOSubSupervisor { get; set; }

	[XmlElement(ElementName = "eocustomergroup")]
	[DataMember(Name = "eocustomergroup", Order = 21)]
	public string EOCustomerGroup { get; set; }

	[XmlElement(ElementName = "eomultishipaddress")]
	[DataMember(Name = "eomultishipaddress", Order = 22)]
	public string EOMultiShipAddress { get; set; }

	[XmlElement(ElementName = "eoreceiveorderconfirmation")]
	[DataMember(Name = "eoreceiveorderconfirmation", Order = 23)]
	public string EOReceiveOrderConfirmation { get; set; }

	[XmlElement(ElementName = "eoeditshippingaddress")]
	[DataMember(Name = "eoeditshippingaddress", Order = 24)]
	public bool EOEditShippingAddress { get; set; }

	[XmlElement(ElementName = "eoreceiveemails")]
	[DataMember(Name = "eoreceiveemails", Order = 25)]
	public bool EOReceiveEMails { get; set; }

	[XmlElement(ElementName = "eohtmlmail")]
	[DataMember(Name = "eohtmlmail", Order = 26)]
	public bool EOHTMLMail { get; set; }

	[XmlElement(ElementName = "eoreminderofopenorders")]
	[DataMember(Name = "eoreminderofopenorders", Order = 27)]
	public bool EOReminderOfOpenOrders { get; set; }

	[XmlElement(ElementName = "eoorderauthorisationmessage")]
	[DataMember(Name = "eoorderauthorisationmessage", Order = 28)]
	public bool EOOrderAuthorisationMessage { get; set; }

	[XmlElement(ElementName = "eoauthorisationrequest")]
	[DataMember(Name = "eoauthorisationrequest", Order = 29)]
	public bool EOAuthorisationRequest { get; set; }

	[XmlElement(ElementName = "eomaynotcreordtemp")]
	[DataMember(Name = "eomaynotcreordtemp", Order = 30)]
	public bool EOMayNotCreOrdTemp { get; set; }

	[XmlElement(ElementName = "createdby")]
	[DataMember(Name = "createdby", Order = 31)]
	public string CreatedBy { get; set; }

	[XmlElement(ElementName = "createdDate")]
	[DataMember(Name = "createdDate", Order = 32)]
	public DateTime? CreatedDate { get; set; }

	[XmlElement(ElementName = "uniqueID")]
	[DataMember(Name = "uniqueID", Order = 33)]
	public Guid UniqueID { get; set; }

	[XmlElement(ElementName = "rowVersion")]
	[DataMember(Name = "rowVersion", Order = 34)]
	public byte[] RowVersion { get; set; }
}
