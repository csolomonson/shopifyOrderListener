using System;

namespace M1.API.DTOs.Custom;

public class OrganizationContactInformationDto
{
	public string OrganizationID { get; set; }

	public string LocationID { get; set; }

	public string ContactID { get; set; }

	public string Name { get; set; }

	public string PhoneNumber { get; set; }

	public string MobileNumber { get; set; }

	public string EmailAddress { get; set; }

	public string CorrespondenceMethod { get; set; }

	public bool Inactive { get; set; }

	public DateTime? InactiveDate { get; set; }

	public bool EasyOrderEnabled { get; set; }

	public bool CreatedByEasyOrder { get; set; }

	public string EOFirstName { get; set; }

	public string EOInitials { get; set; }

	public string EOPrefix { get; set; }

	public string EOSurname { get; set; }

	public string EOPassword { get; set; }

	public string EOUserRole { get; set; }

	public string EODefSupervisor { get; set; }

	public string EOSubSupervisor { get; set; }

	public string EOCustomerGroup { get; set; }

	public string EOMultiShipAddress { get; set; }

	public string EOReceiveOrderConfirmation { get; set; }

	public bool EOEditShippingAddress { get; set; }

	public bool EOReceiveEMails { get; set; }

	public bool EOHTMLMail { get; set; }

	public bool EOReminderOfOpenOrders { get; set; }

	public bool EOOrderAuthorisationMessage { get; set; }

	public bool EOAuthorisationRequest { get; set; }

	public bool EOMayNotCreOrdTemp { get; set; }

	public string CreatedBy { get; set; }

	public DateTime? CreatedDate { get; set; }

	public Guid UniqueID { get; set; }

	public byte[] RowVersion { get; set; }
}
