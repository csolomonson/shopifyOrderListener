using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace M1.API.DTOs.BOM;

[Serializable]
[DataContract(Namespace = "", Name = "organizationcontact")]
[XmlRoot(ElementName = "organizationcontact")]
[XmlType(AnonymousType = true)]
public class BOMOrganizationContactDto
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

	[XmlElement(ElementName = "inactive")]
	[DataMember(Name = "inactive", Order = 8)]
	public bool Inactive { get; set; }

	[XmlElement(ElementName = "inactiveDate")]
	[DataMember(Name = "inactiveDate", Order = 9)]
	public DateTime? InactiveDate { get; set; }

	[XmlElement(ElementName = "easyorderenabled")]
	[DataMember(Name = "easyorderenabled", Order = 10)]
	public bool EasyOrderEnabled { get; set; }

	[XmlElement(ElementName = "createdbyeasyorder")]
	[DataMember(Name = "createdbyeasyorder", Order = 11)]
	public bool CreatedByEasyOrder { get; set; }

	[XmlElement(ElementName = "eofirstname")]
	[DataMember(Name = "eofirstname", Order = 12)]
	public string EOFirstName { get; set; }

	[XmlElement(ElementName = "eoinitials")]
	[DataMember(Name = "eoinitials", Order = 13)]
	public string EOInitials { get; set; }

	[XmlElement(ElementName = "eoprefix")]
	[DataMember(Name = "eoprefix", Order = 14)]
	public string EOPrefix { get; set; }

	[XmlElement(ElementName = "eosurname")]
	[DataMember(Name = "eosurname", Order = 15)]
	public string EOSurname { get; set; }

	[XmlElement(ElementName = "eopassword")]
	[DataMember(Name = "eopassword", Order = 16)]
	public string EOPassword { get; set; }

	[XmlElement(ElementName = "eouserrole")]
	[DataMember(Name = "eouserrole", Order = 17)]
	public string EOUserRole { get; set; }

	[XmlElement(ElementName = "eodefsupervisor")]
	[DataMember(Name = "eodefsupervisor", Order = 18)]
	public string EODefSupervisor { get; set; }

	[XmlElement(ElementName = "eosubsupervisor")]
	[DataMember(Name = "eosubsupervisor", Order = 18)]
	public string EOSubSupervisor { get; set; }

	[XmlElement(ElementName = "eocustomergroup")]
	[DataMember(Name = "eocustomergroup", Order = 19)]
	public string EOCustomerGroup { get; set; }

	[XmlElement(ElementName = "eomultishipaddress")]
	[DataMember(Name = "eomultishipaddress", Order = 20)]
	public string EOMultiShipAddress { get; set; }

	[XmlElement(ElementName = "eoreceiveorderconfirmation")]
	[DataMember(Name = "eoreceiveorderconfirmation", Order = 21)]
	public string EOReceiveOrderConfirmation { get; set; }

	[XmlElement(ElementName = "eoeditshippingaddress")]
	[DataMember(Name = "eoeditshippingaddress", Order = 22)]
	public bool EOEditShippingAddress { get; set; }

	[XmlElement(ElementName = "eoreceiveemails")]
	[DataMember(Name = "eoreceiveemails", Order = 23)]
	public bool EOReceiveEMails { get; set; }

	[XmlElement(ElementName = "eohtmlmail")]
	[DataMember(Name = "eohtmlmail", Order = 24)]
	public bool EOHTMLMail { get; set; }

	[XmlElement(ElementName = "eoreminderofopenorders")]
	[DataMember(Name = "eoreminderofopenorders", Order = 25)]
	public bool EOReminderOfOpenOrders { get; set; }

	[XmlElement(ElementName = "eoorderauthorisationmessage")]
	[DataMember(Name = "eoorderauthorisationmessage", Order = 26)]
	public bool EOOrderAuthorisationMessage { get; set; }

	[XmlElement(ElementName = "eoauthorisationrequest")]
	[DataMember(Name = "eoauthorisationrequest", Order = 27)]
	public bool EOAuthorisationRequest { get; set; }

	[XmlElement(ElementName = "eomaynotcreordtemp")]
	[DataMember(Name = "eomaynotcreordtemp", Order = 28)]
	public bool EOMayNotCreOrdTemp { get; set; }
}
