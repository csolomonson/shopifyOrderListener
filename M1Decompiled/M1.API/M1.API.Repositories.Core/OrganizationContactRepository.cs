using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using M1.API.DTOs.BOM;
using M1.API.DTOs.Core;
using M1.API.DTOs.Custom;
using M1.API.Utilities;
using M1.Core;
using M1.Extensions;

namespace M1.API.Repositories.Core;

public class OrganizationContactRepository : APIBaseRepository, IOrganizationContactRepository, IAPIBaseRepository, IDisposable
{
	private readonly string[] orgContactFields = new string[35]
	{
		"cmcOrganizationID", "cmcLocationID", "cmcContactID", "cmcName", "cmcPhoneNumber", "cmcMobileNumber", "cmcEmailAddress", "cmcCorrespondenceMethod", "cmcInactive", "cmcInactiveDate",
		"cmcEasyOrderEnabled", "cmcCreatedByEasyOrder", "cmcEOFirstName", "cmcEOInitials", "cmcEOPrefix", "cmcEOSurname", "cmcEOPassword", "cmcEOUserRole", "cmcEODefSupervisor", "cmcEOSubSupervisor",
		"cmcEOCustomerGroup", "cmcEOMultiShipAddress", "cmcEOReceiveOrderConfirmation", "cmcEOReminderOfOpenOrders", "cmcEOEditShippingAddress", "cmcEOReceiveEMails", "cmcEOHTMLMail", "cmcEOReminderOfOpenOrders", "cmcEOOrderAuthorisationMessage", "cmcEOAuthorisationRequest",
		"cmcEOMayNotCreOrdTemp", "cmcCreatedBy", "cmcCreatedDate", "cmcUniqueID", "cmcRowVersion"
	};

	public OrganizationContactRepository(APIClientContext clientContext)
	{
		base.M1database = clientContext.Database;
	}

	public Task<bool> DoesOrganizationContactExists(string organizationId, string locationId, string contactId)
	{
		InitializeParameterLists();
		base.filterList.Add("cmcOrganizationID|C", organizationId);
		base.filterList.Add("cmcLocationID|C", locationId);
		base.filterList.Add("cmcContactID|C", contactId);
		base.selectList.Add("cmcContactID");
		return Task.FromResult(GetAsObject("OrganizationContacts", base.filterList, base.selectList, null, null) != null);
	}

	public Task<ICollection<OrganizationContactInformationDto>> GetAllOrganizationContacts(int? pageSize = null, int? pageNumber = null)
	{
		ICollection<OrganizationContactInformationDto> collection = new List<OrganizationContactInformationDto>();
		InitializeParameterLists();
		base.selectList.AddRange(orgContactFields);
		List<string> orderbyList = new List<string> { "cmcContactID" };
		using (DataTable dataTable = GetAsDataTable("OrganizationContacts", base.filterList, base.selectList, orderbyList, null, pageSize, pageNumber))
		{
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				OrganizationContactInformationDto organizationContactInformationDto = new OrganizationContactInformationDto();
				organizationContactInformationDto.OrganizationID = dataTable.Rows[i].Field<string>("cmcOrganizationID");
				organizationContactInformationDto.LocationID = dataTable.Rows[i].Field<string>("cmcLocationID");
				organizationContactInformationDto.ContactID = dataTable.Rows[i].Field<string>("cmcContactID");
				organizationContactInformationDto.Name = dataTable.Rows[i].Field<string>("cmcName");
				organizationContactInformationDto.PhoneNumber = dataTable.Rows[i].Field<string>("cmcPhoneNumber");
				organizationContactInformationDto.MobileNumber = dataTable.Rows[i].Field<string>("cmcMobileNumber");
				organizationContactInformationDto.EmailAddress = dataTable.Rows[i].Field<string>("cmcEmailAddress");
				organizationContactInformationDto.CorrespondenceMethod = dataTable.Rows[i].Field<string>("cmcCorrespondenceMethod");
				organizationContactInformationDto.Inactive = dataTable.Rows[i].Field<bool>("cmcInactive");
				organizationContactInformationDto.InactiveDate = dataTable.Rows[i].Field<DateTime?>("cmcInactiveDate");
				organizationContactInformationDto.EasyOrderEnabled = dataTable.Rows[i].Field<bool>("cmcEasyOrderEnabled");
				organizationContactInformationDto.CreatedByEasyOrder = dataTable.Rows[i].Field<bool>("cmcCreatedByEasyOrder");
				organizationContactInformationDto.EOFirstName = dataTable.Rows[i].Field<string>("cmcEOFirstName");
				organizationContactInformationDto.EOInitials = dataTable.Rows[i].Field<string>("cmcEOInitials");
				organizationContactInformationDto.EOPrefix = dataTable.Rows[i].Field<string>("cmcEOPrefix");
				organizationContactInformationDto.EOSurname = dataTable.Rows[i].Field<string>("cmcEOSurname");
				organizationContactInformationDto.EOPassword = dataTable.Rows[i].Field<string>("cmcEOPassword");
				organizationContactInformationDto.EOUserRole = dataTable.Rows[i].Field<string>("cmcEOUserRole");
				organizationContactInformationDto.EODefSupervisor = dataTable.Rows[i].Field<string>("cmcEODefSupervisor");
				organizationContactInformationDto.EOSubSupervisor = dataTable.Rows[i].Field<string>("cmcEOSubSupervisor");
				organizationContactInformationDto.EOCustomerGroup = dataTable.Rows[i].Field<string>("cmcEOCustomerGroup");
				organizationContactInformationDto.EOMultiShipAddress = dataTable.Rows[i].Field<string>("cmcEOMultiShipAddress");
				organizationContactInformationDto.EOReceiveOrderConfirmation = dataTable.Rows[i].Field<string>("cmcEOReceiveOrderConfirmation");
				organizationContactInformationDto.EOEditShippingAddress = dataTable.Rows[i].Field<bool>("cmcEOEditShippingAddress");
				organizationContactInformationDto.EOReceiveEMails = dataTable.Rows[i].Field<bool>("cmcEOEditShippingAddress");
				organizationContactInformationDto.EOHTMLMail = dataTable.Rows[i].Field<bool>("cmcEOHTMLMail");
				organizationContactInformationDto.EOReminderOfOpenOrders = dataTable.Rows[i].Field<bool>("cmcEOReminderOfOpenOrders");
				organizationContactInformationDto.EOOrderAuthorisationMessage = dataTable.Rows[i].Field<bool>("cmcEOOrderAuthorisationMessage");
				organizationContactInformationDto.EOAuthorisationRequest = dataTable.Rows[i].Field<bool>("cmcEOAuthorisationRequest");
				organizationContactInformationDto.EOMayNotCreOrdTemp = dataTable.Rows[i].Field<bool>("cmcEOMayNotCreOrdTemp");
				organizationContactInformationDto.CreatedBy = dataTable.Rows[i].Field<string>("cmcCreatedBy");
				organizationContactInformationDto.CreatedDate = ((!dataTable.Rows[i].Field<DateTime?>("cmcCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[i].Field<DateTime?>("cmcCreatedDate"));
				organizationContactInformationDto.UniqueID = dataTable.Rows[i].Field<Guid>("cmcUniqueID");
				organizationContactInformationDto.RowVersion = dataTable.Rows[i].Field<byte[]>("cmcRowVersion");
				collection.Add(organizationContactInformationDto);
			}
		}
		return Task.FromResult(collection);
	}

	public Task<OrganizationContactInformationDto> GetOrganizationContact(string organizationId, string locationId, string contactId)
	{
		OrganizationContactInformationDto organizationContactInformationDto = new OrganizationContactInformationDto();
		InitializeParameterLists();
		base.selectList.AddRange(orgContactFields);
		base.filterList.Add(Guid.TryParse(contactId, out var _) ? "cmcUniqueID|C" : "cmcContactID|C", contactId);
		base.filterList.Add("cmcOrganizationID|C", organizationId);
		base.filterList.Add("cmcLocationID|C", locationId);
		using (DataTable dataTable = GetAsDataTable("OrganizationContacts", base.filterList, base.selectList, null, null))
		{
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return Task.FromResult(organizationContactInformationDto);
			}
			organizationContactInformationDto.OrganizationID = dataTable.Rows[0].Field<string>("cmcOrganizationID");
			organizationContactInformationDto.LocationID = dataTable.Rows[0].Field<string>("cmcLocationID");
			organizationContactInformationDto.ContactID = dataTable.Rows[0].Field<string>("cmcContactID");
			organizationContactInformationDto.Name = dataTable.Rows[0].Field<string>("cmcName");
			organizationContactInformationDto.PhoneNumber = dataTable.Rows[0].Field<string>("cmcPhoneNumber");
			organizationContactInformationDto.MobileNumber = dataTable.Rows[0].Field<string>("cmcMobileNumber");
			organizationContactInformationDto.EmailAddress = dataTable.Rows[0].Field<string>("cmcEmailAddress");
			organizationContactInformationDto.CorrespondenceMethod = dataTable.Rows[0].Field<string>("cmcCorrespondenceMethod");
			organizationContactInformationDto.Inactive = dataTable.Rows[0].Field<bool>("cmcInactive");
			organizationContactInformationDto.InactiveDate = dataTable.Rows[0].Field<DateTime?>("cmcInactiveDate");
			organizationContactInformationDto.EasyOrderEnabled = dataTable.Rows[0].Field<bool>("cmcEasyOrderEnabled");
			organizationContactInformationDto.CreatedByEasyOrder = dataTable.Rows[0].Field<bool>("cmcCreatedByEasyOrder");
			organizationContactInformationDto.EOFirstName = dataTable.Rows[0].Field<string>("cmcEOFirstName");
			organizationContactInformationDto.EOInitials = dataTable.Rows[0].Field<string>("cmcEOInitials");
			organizationContactInformationDto.EOPrefix = dataTable.Rows[0].Field<string>("cmcEOPrefix");
			organizationContactInformationDto.EOSurname = dataTable.Rows[0].Field<string>("cmcEOSurname");
			organizationContactInformationDto.EOPassword = dataTable.Rows[0].Field<string>("cmcEOPassword");
			organizationContactInformationDto.EOUserRole = dataTable.Rows[0].Field<string>("cmcEOUserRole");
			organizationContactInformationDto.EODefSupervisor = dataTable.Rows[0].Field<string>("cmcEODefSupervisor");
			organizationContactInformationDto.EOSubSupervisor = dataTable.Rows[0].Field<string>("cmcEOSubSupervisor");
			organizationContactInformationDto.EOCustomerGroup = dataTable.Rows[0].Field<string>("cmcEOCustomerGroup");
			organizationContactInformationDto.EOMultiShipAddress = dataTable.Rows[0].Field<string>("cmcEOMultiShipAddress");
			organizationContactInformationDto.EOReceiveOrderConfirmation = dataTable.Rows[0].Field<string>("cmcEOReceiveOrderConfirmation");
			organizationContactInformationDto.EOEditShippingAddress = dataTable.Rows[0].Field<bool>("cmcEOEditShippingAddress");
			organizationContactInformationDto.EOReceiveEMails = dataTable.Rows[0].Field<bool>("cmcEOEditShippingAddress");
			organizationContactInformationDto.EOHTMLMail = dataTable.Rows[0].Field<bool>("cmcEOHTMLMail");
			organizationContactInformationDto.EOReminderOfOpenOrders = dataTable.Rows[0].Field<bool>("cmcEOReminderOfOpenOrders");
			organizationContactInformationDto.EOOrderAuthorisationMessage = dataTable.Rows[0].Field<bool>("cmcEOOrderAuthorisationMessage");
			organizationContactInformationDto.EOAuthorisationRequest = dataTable.Rows[0].Field<bool>("cmcEOAuthorisationRequest");
			organizationContactInformationDto.EOMayNotCreOrdTemp = dataTable.Rows[0].Field<bool>("cmcEOMayNotCreOrdTemp");
			organizationContactInformationDto.CreatedBy = dataTable.Rows[0].Field<string>("cmcCreatedBy");
			organizationContactInformationDto.CreatedDate = ((!dataTable.Rows[0].Field<DateTime?>("cmcCreatedDate").HasValue) ? new DateTime?(DateTime.Parse("01/01/1900")) : dataTable.Rows[0].Field<DateTime?>("cmcCreatedDate"));
			organizationContactInformationDto.UniqueID = dataTable.Rows[0].Field<Guid>("cmcUniqueID");
			organizationContactInformationDto.RowVersion = dataTable.Rows[0].Field<byte[]>("cmcRowVersion");
		}
		return Task.FromResult(organizationContactInformationDto);
	}

	public Task<APIValidationInfoDto> SaveOrganizationContact(BOMOrganizationContactDto organizationContact)
	{
		APIValidationInfoDto result = new APIValidationInfoDto();
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using M1BindingSource m1BindingSource = new M1BindingSource(base.M1database, null);
			m1BindingSource.ClearCache();
			stringBuilder.Append("cmcContactID = " + M1Util.ConvertToLinq(organizationContact.ContactID));
			m1BindingSource.DataSourceTable = "OrganizationContacts";
			m1BindingSource.NavigateTo(stringBuilder.ToString());
			DataRow dataRow;
			if (m1BindingSource.Count == 0)
			{
				dataRow = m1BindingSource.AddNew() as DataRow;
				if (dataRow != null)
				{
					dataRow["cmcOrganizationID"] = organizationContact.OrganizationID;
				}
				if (dataRow != null)
				{
					dataRow["cmcLocationID"] = organizationContact.LocationID;
				}
				if (dataRow != null)
				{
					dataRow["cmcContactID"] = organizationContact.ContactID;
				}
			}
			else
			{
				dataRow = m1BindingSource.CurrentAsDataRow;
			}
			dataRow["cmcName"] = organizationContact.Name ?? dataRow["cmcName"];
			dataRow["cmcEmailAddress"] = organizationContact.EmailAddress ?? dataRow["cmcEmailAddress"];
			dataRow["cmcPhoneNumber"] = organizationContact.PhoneNumber ?? dataRow["cmcPhoneNumber"];
			dataRow["cmcMobileNumber"] = organizationContact.MobileNumber ?? dataRow["cmcMobileNumber"];
			if (!organizationContact.Inactive)
			{
				dataRow["cmcInactive"] = organizationContact.Inactive;
			}
			DataRow dataRow2 = dataRow;
			DateTime? inactiveDate = organizationContact.InactiveDate;
			dataRow2["cmcInactiveDate"] = (inactiveDate.HasValue ? ((object)inactiveDate.GetValueOrDefault()) : dataRow["cmcInactiveDate"]);
			if (!organizationContact.EasyOrderEnabled)
			{
				dataRow["cmcEasyOrderEnabled"] = organizationContact.EasyOrderEnabled;
			}
			if (!organizationContact.CreatedByEasyOrder)
			{
				dataRow["cmcCreatedByEasyOrder"] = organizationContact.CreatedByEasyOrder;
			}
			dataRow["cmcEOFirstName"] = organizationContact.EOFirstName ?? dataRow["cmcEOFirstName"];
			dataRow["cmcEOInitials"] = organizationContact.EOInitials ?? dataRow["cmcEOInitials"];
			dataRow["cmcEOPrefix"] = organizationContact.EOPrefix ?? dataRow["cmcEOPrefix"];
			dataRow["cmcEOSurname"] = organizationContact.EOSurname ?? dataRow["cmcEOSurname"];
			dataRow["cmcEOPassword"] = organizationContact.EOPassword ?? dataRow["cmcEOPassword"];
			dataRow["cmcEOUserRole"] = organizationContact.EOUserRole ?? dataRow["cmcEOUserRole"];
			dataRow["cmcEODefSupervisor"] = organizationContact.EODefSupervisor ?? dataRow["cmcEODefSupervisor"];
			dataRow["cmcEOSubSupervisor"] = organizationContact.EOSubSupervisor ?? dataRow["cmcEOSubSupervisor"];
			dataRow["cmcEOCustomerGroup"] = organizationContact.EOCustomerGroup ?? dataRow["cmcEOCustomerGroup"];
			dataRow["cmcEOMultiShipAddress"] = organizationContact.EOMultiShipAddress ?? dataRow["cmcEOMultiShipAddress"];
			dataRow["cmcEOReceiveOrderConfirmation"] = organizationContact.EOReceiveOrderConfirmation ?? dataRow["cmcEOReceiveOrderConfirmation"];
			if (!organizationContact.EOEditShippingAddress)
			{
				dataRow["cmcEOEditShippingAddress"] = organizationContact.EOEditShippingAddress;
			}
			if (!organizationContact.EOReceiveEMails)
			{
				dataRow["cmcEOReceiveEMails"] = organizationContact.EOEditShippingAddress;
			}
			if (!organizationContact.EOHTMLMail)
			{
				dataRow["cmcEOHTMLMail"] = organizationContact.EOHTMLMail;
			}
			if (!organizationContact.EOReminderOfOpenOrders)
			{
				dataRow["cmcEOReminderOfOpenOrders"] = organizationContact.EOReminderOfOpenOrders;
			}
			if (!organizationContact.EOOrderAuthorisationMessage)
			{
				dataRow["cmcEOOrderAuthorisationMessage"] = organizationContact.EOOrderAuthorisationMessage;
			}
			if (!organizationContact.EOAuthorisationRequest)
			{
				dataRow["cmcEOAuthorisationRequest"] = organizationContact.EOAuthorisationRequest;
			}
			if (!organizationContact.EOMayNotCreOrdTemp)
			{
				dataRow["cmcEOMayNotCreOrdTemp"] = organizationContact.EOMayNotCreOrdTemp;
			}
			m1BindingSource.SaveData();
		}
		catch (Exception ex)
		{
			List<string> list = new List<string>();
			list.Add("Error occurred [" + ex.Message + "] while processing the OrganizationContact [" + organizationContact.ContactID + "]");
			result = new APIValidationInfoDto(list, null, HttpStatusCode.InternalServerError);
		}
		return Task.FromResult(result);
	}
}
