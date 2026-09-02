using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using M1.Ax.Erp.IntegrationService;
using M1.Core;
using M1.Extensions;
using M1.ShippingServices;
using M1.ShippingServices.DTO;
using M1.ShippingServices.FedEx;
using M1.ShippingServices.Repository;
using M1.ShippingServices.UPS;

namespace M1.Ax.Erp;

public class Shipments
{
	private class PartInfo
	{
		public string PartID { get; set; }

		public string PartRevisionID { get; set; }

		public string PartWarehouseID { get; set; }

		public string PartPartBinID { get; set; }

		public decimal QuantityShipped { get; set; }

		public string SalesOrderID { get; set; }

		public int SalesOrderLineID { get; set; }

		public int SalesOrderDeliveryID { get; set; }

		public decimal DeliveryQuantity { get; set; }
	}

	public string GetTrackingLink(M1Database database, string ShipMethodID, string TrackingNumber, string ReferenceNumber, DateTime? FromDate)
	{
		string result = string.Empty;
		if (!string.IsNullOrWhiteSpace(ShipMethodID) && (!string.IsNullOrWhiteSpace(TrackingNumber) || !string.IsNullOrWhiteSpace(ReferenceNumber)))
		{
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT xasTrackingLink,xasReferenceTrackingLink,xasCarrierAccountNumber FROM ShippingMethods WHERE xasShippingMethodID = @ShipMethodID");
			sqlCommand.Parameters.Add(new SqlParameter("@ShipMethodID", SqlDbType.NVarChar)).Value = ShipMethodID;
			DataTable dataTable = database.GetDataTable(sqlCommand);
			if (dataTable.Rows.Count != 0)
			{
				DataRow row = dataTable.Rows[0];
				string text = row.Field<string>("xasTrackingLink");
				string text2 = row.Field<string>("xasReferenceTrackingLink");
				string text3 = row.Field<string>("xasCarrierAccountNumber");
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(TrackingNumber))
				{
					result = text.Replace("<TrackingNumber>", TrackingNumber.Trim());
				}
				else if (!string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3) && !string.IsNullOrWhiteSpace(ReferenceNumber))
				{
					DateTime today = DateTime.Today;
					DateTime dateTime = ((!FromDate.HasValue) ? today.AddMonths(-3) : FromDate.Value);
					result = text2.Replace("<ReferenceID>", ReferenceNumber.Trim());
					result = result.Replace("<AccountNumber>", text3);
					result = result.Replace("<DateRangeFromYear>", dateTime.Year.ToString().Trim());
					result = result.Replace("<DateRangeFromMonth>", dateTime.Month.ToString().Trim());
					result = result.Replace("<DateRangeFromDay>", dateTime.Day.ToString().Trim());
					result = result.Replace("<DateRangeToYear>", today.Year.ToString().Trim());
					result = result.Replace("<DateRangeToMonth>", today.Month.ToString().Trim());
					result = result.Replace("<DateRangeToDay>", today.Day.ToString().Trim());
				}
			}
		}
		return result;
	}

	public bool CheckSumOfShipQtyForDelivery(M1BindingSource m1BindingSource)
	{
		DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return true;
		}
		DataTable dataTable = m1BindingSource.GetDataView().ToTable();
		decimal num = currentAsDataRow.Field<decimal>("smlSOOpenQuantity");
		decimal num2 = default(decimal);
		int num3 = 0;
		DataRow[] array = dataTable.Select("smlSalesOrderID = " + currentAsDataRow.Field<string>("smlSalesOrderID").Trim().ToLinq() + " and smlSalesOrderLineID = " + Convert.ToInt32(currentAsDataRow["smlSalesOrderLineID"]).ToLinq() + " and smlSalesOrderDeliveryID = " + Convert.ToInt32(currentAsDataRow["smlSalesOrderDeliveryID"]).ToLinq());
		foreach (DataRow row in array)
		{
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("smlSalesOrderID")))
			{
				num2 += row.Field<decimal>("smlQuantityShipped") + row.Field<decimal>("smlJobQuantityShipped");
				num3++;
			}
		}
		if (num2 > num && num3 > 1)
		{
			return false;
		}
		return true;
	}

	private static string GetString(object obj)
	{
		if (obj != null)
		{
			return obj.ToString().Trim();
		}
		return string.Empty;
	}

	private static void InitializeSmpFreightFields(ref DataRow hRow)
	{
		hRow.SetField("smpListBaseChargeBase", 0m);
		hRow.SetField("smpListSurchargeBase", 0m);
		hRow.SetField("smpListDiscountBase", 0m);
		hRow.SetField("smpListCarrierFreightBase", 0m);
		hRow.SetField("smpAccBaseChargeBase", 0m);
		hRow.SetField("smpAccSurchargeBase", 0m);
		hRow.SetField("smpAccDiscountBase", 0m);
		hRow.SetField("smpAccCarrierFreightBase", 0m);
		hRow.SetField("smpFreightCharge", 0m);
	}

	private OrganizationAddressInfo GetM1InstalledCompanyAddress(M1BindingSource m1BindingSource)
	{
		OrganizationAddressInfo organizationAddressInfo = new OrganizationAddressInfo();
		M1Database database = m1BindingSource.Database;
		organizationAddressInfo.OrganizationID = string.Empty;
		organizationAddressInfo.Name = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadName")) ? "" : database.Props("DS").Field<string>("xadName").Trim());
		organizationAddressInfo.AddressLine1 = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadAddressLine1")) ? "" : database.Props("DS").Field<string>("xadAddressLine1").Trim());
		organizationAddressInfo.AddressLine2 = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadAddressLine2")) ? "" : database.Props("DS").Field<string>("xadAddressLine2").Trim());
		organizationAddressInfo.AddressLine3 = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadAddressLine3")) ? "" : database.Props("DS").Field<string>("xadAddressLine3").Trim());
		organizationAddressInfo.City = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadCity")) ? "" : database.Props("DS").Field<string>("xadCity").Trim());
		organizationAddressInfo.State = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadState")) ? "" : database.Props("DS").Field<string>("xadState").Trim());
		organizationAddressInfo.PostCode = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadPostCode")) ? "" : database.Props("DS").Field<string>("xadPostCode").Trim());
		organizationAddressInfo.CountryCode = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadCountryCode")) ? "" : database.Props("DS").Field<string>("xadCountryCode").Trim());
		organizationAddressInfo.PhoneNumber = (string.IsNullOrEmpty(database.Props("DS").Field<string>("xadPhoneNumber")) ? "" : database.Props("DS").Field<string>("xadPhoneNumber").Trim());
		return organizationAddressInfo;
	}

	private static OrganizationAddressInfo GetShipFromAddressForShipping(M1BindingSource m1BindingSource)
	{
		OrganizationAddressInfo organizationAddressInfo = new OrganizationAddressInfo();
		M1Database database = m1BindingSource.Database;
		organizationAddressInfo.OrganizationID = string.Empty;
		organizationAddressInfo.Name = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxName")) ? "" : database.Props("SM").Field<string>("xsmFdxName").Trim());
		organizationAddressInfo.AddressLine1 = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxAddressLine1")) ? "" : database.Props("SM").Field<string>("xsmFdxAddressLine1").Trim());
		organizationAddressInfo.AddressLine2 = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxAddressLine2")) ? "" : database.Props("SM").Field<string>("xsmFdxAddressLine2").Trim());
		organizationAddressInfo.AddressLine3 = string.Empty;
		organizationAddressInfo.City = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxCity")) ? "" : database.Props("SM").Field<string>("xsmFdxCity").Trim());
		organizationAddressInfo.State = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxState")) ? "" : database.Props("SM").Field<string>("xsmFdxState").Trim());
		organizationAddressInfo.PostCode = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxPostCode")) ? "" : database.Props("SM").Field<string>("xsmFdxPostCode").Trim());
		organizationAddressInfo.CountryCode = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxCountry")) ? "" : database.Props("SM").Field<string>("xsmFdxCountry").Trim());
		organizationAddressInfo.PhoneNumber = (string.IsNullOrEmpty(database.Props("SM").Field<string>("xsmFdxPhoneNumber")) ? "" : database.Props("SM").Field<string>("xsmFdxPhoneNumber").Trim());
		return organizationAddressInfo;
	}

	private static OrganizationAddressInfo GetPlantAddressInfo(M1BindingSource m1BindingSource, string plantId)
	{
		DataTable dataTable = null;
		SqlCommand sqlCommand = null;
		M1Database database = m1BindingSource.Database;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT xauName, xauAddressLine1, xauAddressLine2, xauAddressLine3, xauCity, xauState, xauPostCode, xauCountryCode, xauPhoneNumber ");
		stringBuilder.Append("FROM Plants ");
		stringBuilder.Append("WHERE xauPlantID =@P1");
		sqlCommand = new SqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.AddWithValue("@P1", plantId);
		dataTable = database.GetDataTable(sqlCommand);
		OrganizationAddressInfo organizationAddressInfo = (from locLine in dataTable.AsEnumerable()
			select new OrganizationAddressInfo
			{
				OrganizationID = string.Empty,
				Name = locLine.Field<string>("xauName").Trim(),
				AddressLine1 = GetString(locLine.Field<string>("xauAddressLine1")),
				AddressLine2 = GetString(locLine.Field<string>("xauAddressLine2")),
				AddressLine3 = GetString(locLine.Field<string>("xauAddressLine3")),
				CountryCode = GetString(locLine.Field<string>("xauCountryCode")),
				State = GetString(locLine.Field<string>("xauState")),
				City = GetString(locLine.Field<string>("xauCity")),
				PostCode = GetString(locLine.Field<string>("xauPostCode")),
				PhoneNumber = GetString(locLine.Field<string>("xauPhoneNumber"))
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationAddressInfo == null)
		{
			organizationAddressInfo = new OrganizationAddressInfo();
		}
		return organizationAddressInfo;
	}

	private static OrganizationLocationInfo GetOrganizationLocationInfo(M1BindingSource m1BindingSource, string organizationId, string locationId)
	{
		DataTable dataTable = null;
		new OrganizationLocationInfo();
		SqlCommand sqlCommand = null;
		M1Database database = m1BindingSource.Database;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT cmlOrganizationID, cmlLocationID, cmlName, cmlAddressLine1, cmlAddressLine2, cmlCity, cmlState, cmlPostCode, cmlCountry, cmlPhoneNumber, cmlShipLocation,");
		stringBuilder.Append("cmlShipContactID, cmlCustomerShippingMethodID, cmlAddressLine3, cmlInactive, cmlCounty, cmlARInvoiceLocation, cmlARInvoiceContactID,");
		stringBuilder.Append("cmlCustomerPaymentTermID, cmlUPSAcctNumber, cmlCountryCode, cmlUPSValidated,cmlEMailAddress,cmlFedExAccountNumber ");
		stringBuilder.Append("FROM OrganizationLocations ");
		stringBuilder.Append("WHERE cmlOrganizationID=@P1 AND cmlLocationID=@p2 AND cmlInactive = 0");
		sqlCommand = new SqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.AddWithValue("@P1", organizationId);
		sqlCommand.Parameters.AddWithValue("@P2", locationId);
		dataTable = database.GetDataTable(sqlCommand);
		OrganizationLocationInfo organizationLocationInfo = (from locLine in dataTable.AsEnumerable()
			select new OrganizationLocationInfo
			{
				OrganizationID = locLine.Field<string>("cmlOrganizationID").Trim(),
				LocationID = locLine.Field<string>("cmlLocationID").Trim(),
				Name = GetString(locLine.Field<string>("cmlName")),
				ARInvoiceLocation = locLine.Field<bool>("cmlARInvoiceLocation"),
				ARInvoiceContactID = GetString(locLine.Field<string>("cmlARInvoiceContactID")),
				ShipLocation = locLine.Field<bool>("cmlShipLocation"),
				ShipContactID = GetString(locLine.Field<string>("cmlShipContactID")),
				AddressLine1 = GetString(locLine.Field<string>("cmlAddressLine1")),
				AddressLine2 = GetString(locLine.Field<string>("cmlAddressLine2")),
				AddressLine3 = GetString(locLine.Field<string>("cmlAddressLine3")),
				CountryCode = GetString(locLine.Field<string>("cmlCountryCode")),
				Country = GetString(locLine.Field<string>("cmlCountry")),
				State = GetString(locLine.Field<string>("cmlState")),
				City = GetString(locLine.Field<string>("cmlCity")),
				PostCode = GetString(locLine.Field<string>("cmlPostCode")),
				EMailAddress = GetString(locLine.Field<string>("cmlEMailAddress")),
				Inactive = locLine.Field<bool>("cmlInactive"),
				PhoneNumber = GetString(locLine.Field<string>("cmlPhoneNumber")),
				UPSAcctNumber = GetString(locLine.Field<string>("cmlUPSAcctNumber")),
				UPSValidated = locLine.Field<bool>("cmlUPSValidated"),
				CustomerShippingMethodID = GetString(locLine.Field<string>("cmlCustomerShippingMethodID")),
				CustomerPaymentTermID = GetString(locLine.Field<string>("cmlCustomerPaymentTermID")),
				FedExAcctNumber = GetString(locLine.Field<string>("cmlFedExAccountNumber"))
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationLocationInfo == null)
		{
			organizationLocationInfo = new OrganizationLocationInfo();
		}
		return organizationLocationInfo;
	}

	public OrganizationContactDto GetOrganizationContactInfor(M1BindingSource m1BindingSource, string organizationId, string orgLocationId, string orgContactId)
	{
		DataTable dataTable = null;
		SqlCommand sqlCommand = null;
		M1Database database = m1BindingSource.Database;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("SELECT cmcOrganizationID, cmcLocationID,cmcContactID,cmcName,cmcPhoneNumber,cmcMobileNumber,cmcEMailAddress,cmcInactive ");
		stringBuilder.Append("FROM OrganizationContacts ");
		stringBuilder.Append("WHERE cmcOrganizationID=@P1 AND cmcLocationID=@p2 AND cmcContactID=@p3 AND cmcInactive=0");
		sqlCommand = new SqlCommand(stringBuilder.ToString());
		sqlCommand.Parameters.AddWithValue("@P1", organizationId);
		sqlCommand.Parameters.AddWithValue("@P2", orgLocationId);
		sqlCommand.Parameters.AddWithValue("@P3", orgContactId);
		dataTable = database.GetDataTable(sqlCommand);
		OrganizationContactDto organizationContactDto = (from locLine in dataTable.AsEnumerable()
			select new OrganizationContactDto
			{
				OrganizationID = locLine.Field<string>("cmcOrganizationID").Trim(),
				LocationID = locLine.Field<string>("cmcLocationID").Trim(),
				ContactID = locLine.Field<string>("cmcContactID").Trim(),
				Name = GetString(locLine.Field<string>("cmcName")),
				EmailAddress = GetString(locLine.Field<string>("cmcEMailAddress")),
				Inactive = locLine.Field<bool>("cmcInactive"),
				MobileNumber = GetString(locLine.Field<string>("cmcMobileNumber")),
				PhoneNumber = GetString(locLine.Field<string>("cmcPhoneNumber"))
			}).FirstOrDefault();
		dataTable.Dispose();
		if (organizationContactDto == null)
		{
			organizationContactDto = new OrganizationContactDto();
		}
		return organizationContactDto;
	}

	private ShippingOrganizationLocationDto GetShipperLocationDto(OrganizationAddressInfo locationInfo, string shippingMethod)
	{
		return new ShippingOrganizationLocationDto
		{
			OrganizationID = locationInfo.OrganizationID,
			OrganizationName = locationInfo.Name,
			LocationID = string.Empty,
			LocationName = locationInfo.Name,
			LocationAddress = new ShippingAddressDto
			{
				AddressLine1 = locationInfo.AddressLine1,
				AddressLine2 = locationInfo.AddressLine2,
				AddressLine3 = locationInfo.AddressLine3,
				City = locationInfo.City,
				StateOrProvinceCode = locationInfo.State,
				PostCodeLowEnd = locationInfo.PostCode,
				CountryCode = locationInfo.CountryCode
			},
			ShippingMethodID = shippingMethod.Trim(),
			AttentionName = locationInfo.Name,
			PhoneNumber = locationInfo.PhoneNumber
		};
	}

	private static ShippingOrganizationLocationDto GetShipperFromLocationDto(OrganizationAddressInfo locationInfo, string shippingMethod)
	{
		return new ShippingOrganizationLocationDto
		{
			OrganizationID = locationInfo.OrganizationID,
			OrganizationName = locationInfo.Name,
			LocationID = string.Empty,
			LocationName = locationInfo.Name,
			LocationAddress = new ShippingAddressDto
			{
				AddressLine1 = locationInfo.AddressLine1,
				AddressLine2 = locationInfo.AddressLine2,
				AddressLine3 = locationInfo.AddressLine3,
				City = locationInfo.City,
				StateOrProvinceCode = locationInfo.State,
				PostCodeLowEnd = locationInfo.PostCode,
				CountryCode = locationInfo.CountryCode
			},
			ShippingMethodID = shippingMethod.Trim(),
			AttentionName = locationInfo.Name,
			PhoneNumber = locationInfo.PhoneNumber
		};
	}

	private static ShippingOrganizationLocationDto GetShipperToLocationDto(OrganizationLocationInfo locationInfo, string shippingMethod, OrganizationContactDto locContactInfo)
	{
		return new ShippingOrganizationLocationDto
		{
			OrganizationID = locationInfo.OrganizationID,
			OrganizationName = locationInfo.Name,
			LocationID = locationInfo.LocationID,
			LocationName = locationInfo.Name,
			LocationAddress = new ShippingAddressDto
			{
				AddressLine1 = locationInfo.AddressLine1,
				AddressLine2 = locationInfo.AddressLine2,
				AddressLine3 = locationInfo.AddressLine3,
				City = locationInfo.City,
				StateOrProvinceCode = locationInfo.State,
				PostCodeLowEnd = locationInfo.PostCode,
				CountryCode = locationInfo.CountryCode,
				PhoneNumber = locationInfo.PhoneNumber
			},
			ShippingMethodID = shippingMethod.Trim(),
			AttentionName = locContactInfo.Name,
			PhoneNumber = locContactInfo.PhoneNumber,
			UPSBillingAccountNo = locationInfo.UPSAcctNumber,
			FedExBillingAccountNo = locationInfo.FedExAcctNumber
		};
	}

	private static Dictionary<int, decimal> GetOpenShipmentLineQuantities(DataTable dtShipmentLines, DataTable dtPackageDetails)
	{
		Dictionary<int, decimal> dictionary = new Dictionary<int, decimal>();
		foreach (DataRow lineRow in dtShipmentLines.Rows)
		{
			IEnumerable<decimal> source = from y in dtPackageDetails.AsEnumerable()
				where y.Field<short>("spdShipmentLineID").Equals(lineRow.Field<short>("smlShipmentLineID"))
				group y by y.Field<short>("spdShipmentLineID") into p
				select p.Sum((DataRow l) => l.Field<decimal>("spdQuantity"));
			if (source.Any())
			{
				dictionary.Add(lineRow.Field<short>("smlShipmentLineID"), lineRow.Field<decimal>("smlQuantityShipped") - decimal.Parse(source.First().ToString()));
			}
			else
			{
				dictionary.Add(lineRow.Field<short>("smlShipmentLineID"), lineRow.Field<decimal>("smlQuantityShipped"));
			}
		}
		return dictionary;
	}

	private static int CreateNewShipmentPackageRow(DataRow selectedPackageRow, M1BindingSource packageBs)
	{
		int result = 0;
		DataRow dataRow = packageBs.AddNew() as DataRow;
		packageBs.SetKeyToNextAvailable();
		if (dataRow != null)
		{
			result = Convert.ToInt32(dataRow["spaShipmentPackageID"]);
			dataRow["spaShipmentID"] = selectedPackageRow["spaShipmentID"];
			dataRow["spaPackageDimensionsUOM"] = selectedPackageRow["spaPackageDimensionsUOM"];
			dataRow["spaPackageHeight"] = selectedPackageRow["spaPackageHeight"];
			dataRow["spaPackageLength"] = selectedPackageRow["spaPackageLength"];
			dataRow["spaPackageWidth"] = selectedPackageRow["spaPackageWidth"];
			dataRow["spaPackageWeight"] = selectedPackageRow["spaPackageWeight"];
			dataRow["spaPackageWeightUOM"] = selectedPackageRow["spaPackageWeightUOM"];
			dataRow["spaLargePackage"] = selectedPackageRow["spaLargePackage"];
			dataRow["spaAdditionalHandlingRequired"] = selectedPackageRow["spaAdditionalHandlingRequired"];
			dataRow["spaVerbalConfirmationRequired"] = selectedPackageRow["spaVerbalConfirmationRequired"];
			dataRow["spaPackageValue"] = selectedPackageRow["spaPackageValue"];
			dataRow["spaPackageValueForeign"] = selectedPackageRow["spaPackageValueForeign"];
			dataRow["spaUPSPackageTypes"] = selectedPackageRow["spaUPSPackageTypes"];
			dataRow["spaFedExPackageTypes"] = selectedPackageRow["spaFedExPackageTypes"];
			dataRow["spaCustomerPackageID"] = selectedPackageRow["spaCustomerPackageID"];
			dataRow["spaCreatedBy"] = packageBs.User.ID.Trim();
			dataRow["spaCreatedDate"] = DateTime.Now;
			dataRow["spaPackageRate"] = 0;
			dataRow["spaPackageRateForeign"] = 0;
			dataRow["spaTrackingNo"] = string.Empty;
			dataRow["spaLabelFilePath"] = string.Empty;
		}
		packageBs.SaveData();
		return result;
	}

	private static void CreateNewShipmentPackageDetailsRows(int newPackageId, DataTable dtpackageDetails, M1BindingSource packageDetailsBs, ref Dictionary<int, decimal> DicTobePackagedLineQty, out decimal newPackageWeightKgs)
	{
		decimal num = default(decimal);
		decimal num2 = default(decimal);
		decimal num3 = decimal.Parse("2.20462");
		foreach (DataRow row in dtpackageDetails.Rows)
		{
			DicTobePackagedLineQty.TryGetValue(int.Parse(row["spdShipmentLineID"].ToString().Trim()), out var value);
			if (!(value > 0m))
			{
				continue;
			}
			decimal num4 = decimal.Parse(row["spdQuantity"].ToString().Trim());
			if (packageDetailsBs.AddNew() is DataRow dataRow2)
			{
				dataRow2["spdShipmentID"] = row["spdShipmentID"];
				dataRow2["spdShipmentLineID"] = row["spdShipmentLineID"];
				dataRow2["spdShipmentPackageID"] = newPackageId;
				dataRow2["spdPartID"] = row["spdPartID"];
				dataRow2["spdPartRevisionID"] = row["spdPartRevisionID"];
				dataRow2["spdCommodityDescription"] = row["spdCommodityDescription"];
				dataRow2["spdCountryOfManufacture"] = row["spdCountryOfManufacture"];
				dataRow2["spdWeightUnitOfMeasure"] = row["spdWeightUnitOfMeasure"];
				dataRow2["spdCreatedBy"] = packageDetailsBs.User.ID.Trim();
				dataRow2["spdCreatedDate"] = DateTime.Now;
				if (value == 0m)
				{
					dataRow2["spdQuantity"] = 0;
				}
				else if (value < num4)
				{
					dataRow2["spdQuantity"] = value;
				}
				else
				{
					dataRow2["spdQuantity"] = num4;
				}
				num = ((!row["spdWeightUnitOfMeasure"].ToString().Trim().Equals("KGS", StringComparison.CurrentCultureIgnoreCase)) ? Math.Round(Convert.ToDecimal(row["spdWeight"]) / Convert.ToDecimal(row["spdQuantity"]) * Convert.ToDecimal(dataRow2["spdQuantity"]) / num3, 2) : Math.Round(Convert.ToDecimal(row["spdWeight"]) / Convert.ToDecimal(row["spdQuantity"]) * Convert.ToDecimal(dataRow2["spdQuantity"]), 2));
				DicTobePackagedLineQty[int.Parse(row["spdShipmentLineID"].ToString().Trim())] = value - Convert.ToDecimal(dataRow2["spdQuantity"]);
			}
			num2 += num;
		}
		packageDetailsBs.SaveData();
		newPackageWeightKgs = num2;
	}

	private static void SetNewPackageProperties(M1BindingSource packageBs, string shipmentId, int newPackageId, decimal newPackageWeightKgs)
	{
		string empty = string.Empty;
		decimal num = decimal.Parse("2.20462");
		empty = "spaShipmentID = " + shipmentId.ToLinq() + " And spaShipmentPackageID = " + newPackageId.ToLinq();
		DataRow[] array = packageBs.GetDataTable().Select(empty);
		if (array.Length != 0)
		{
			DataRow dataRow = array[0];
			if (dataRow["spaPackageWeightUOM"].ToString().Trim().Equals("KGS", StringComparison.CurrentCultureIgnoreCase))
			{
				dataRow["spaPackageWeight"] = newPackageWeightKgs;
			}
			else
			{
				dataRow["spaPackageWeight"] = Math.Round(newPackageWeightKgs * num, 5);
			}
		}
	}

	private string GetCustomerPO(M1BindingSource shipmentPackageDetailsBs, M1BindingSource shipmentBs)
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		string result = string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		DataTable dataTable = new DataTable();
		DataRow currentAsDataRow = shipmentBs.PrimaryTable.GetChildBindingSource("ShipmentPackages").CurrentAsDataRow;
		empty = currentAsDataRow["spaShipmentID"].ToString().Trim();
		empty2 = currentAsDataRow["spaShipmentPackageID"].ToString();
		if (currentAsDataRow != null)
		{
			stringBuilder.Length = 0;
			stringBuilder.Append("Select distinct ompCustomerPO from ShipmentLines Inner Join SalesOrders On smlSalesOrderID = ompSalesOrderID ");
			stringBuilder.Append("Where smlShipmentID = ");
			stringBuilder.Append(empty.ToLinq());
			stringBuilder.Append("And ompCustomerPO <> '' Order By ompCustomerPO");
			dataTable = shipmentPackageDetailsBs.Database.GetDataTable(stringBuilder.ToString());
			if (dataTable.Rows.Count == 1)
			{
				result = (from x in dataTable.AsEnumerable()
					select x.Field<string>("ompCustomerPO")).FirstOrDefault()?.Trim();
			}
			else
			{
				stringBuilder.Length = 0;
				stringBuilder.Append("spdShipmentID =");
				stringBuilder.Append(empty.ToLinq());
				stringBuilder.Append(" And spdShipmentPackageID  = ");
				stringBuilder.Append(empty2.ToLinq());
				List<DataRow> source = shipmentPackageDetailsBs.GetDataTable().Select(stringBuilder.ToString()).ToList();
				if (source.Any())
				{
					string value = string.Join(",", source.Select((DataRow x) => x.Field<short>("spdShipmentLineID")).ToList());
					stringBuilder.Length = 0;
					stringBuilder.Append("Select distinct ompCustomerPO from ShipmentLines Inner Join ");
					stringBuilder.Append("SalesOrders On smlSalesOrderID = ompSalesOrderID ");
					stringBuilder.Append("Where smlShipmentID = ");
					stringBuilder.Append(empty.ToLinq());
					stringBuilder.Append(" And smlShipmentLineID In (");
					stringBuilder.Append(value);
					stringBuilder.Append(") And ompCustomerPO <> '' Order By ompCustomerPO");
					DataTable dataTable2 = shipmentPackageDetailsBs.Database.GetDataTable(stringBuilder.ToString());
					if (dataTable2.Rows.Count > 0)
					{
						result = (from x in dataTable2.AsEnumerable()
							select x.Field<string>("ompCustomerPO")).FirstOrDefault()?.Trim();
					}
				}
			}
		}
		return result;
	}

	private decimal GetUSDCrossCurrencyRateForShipmentCurrency(M1Database m1Database, string shipmentCurrencyCode, decimal shipmentCurrencyExchangeRate)
	{
		decimal num = 1m;
		string currencyRateID = (string.IsNullOrEmpty(m1Database.Props("SM").Field<string>("xsmUSDCurrencyCode")) ? "" : m1Database.Props("SM").Field<string>("xsmUSDCurrencyCode").Trim());
		shipmentCurrencyCode = (string.IsNullOrEmpty(shipmentCurrencyCode) ? m1Database.HomeCurrencyID : shipmentCurrencyCode);
		shipmentCurrencyExchangeRate = ((!string.IsNullOrEmpty(shipmentCurrencyCode)) ? ((shipmentCurrencyExchangeRate == 0m) ? 1m : shipmentCurrencyExchangeRate) : 1m);
		num = m1Database.GetExchangeRate(currencyRateID, DateTime.Today);
		num = ((num == 0m) ? 1m : num);
		return Math.Round(shipmentCurrencyExchangeRate / num, 6);
	}

	private ShipmentRequestParameterDto GetConstructedShipmentRequestParameterDto(M1BindingSource m1BindingSource)
	{
		ShipmentRequestParameterDto shipmentRequestParameterDto = new ShipmentRequestParameterDto();
		DataTable dataTable = m1BindingSource.GetDataTable();
		M1BindingSource primaryBindingSource = m1BindingSource.PrimaryBindingSource;
		M1BindingSource childBindingSource = primaryBindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("ShipmentPackageDetails");
		DataTable dataTable2 = childBindingSource.GetDataTable();
		shipmentRequestParameterDto.ShipmentHeaderInfo = GetShipmentHeaderInfoDto(m1BindingSource, primaryBindingSource.CurrentAsDataRow);
		shipmentRequestParameterDto.ShipmentLineList = GetShipmentLineInfoDtoList(m1BindingSource, dataTable2, shipmentRequestParameterDto.ShipmentHeaderInfo);
		shipmentRequestParameterDto.PackageList = GetShipmentPackageInfoDtoList(m1BindingSource, dataTable, shipmentRequestParameterDto.ShipmentHeaderInfo);
		shipmentRequestParameterDto.PackageDetailsList = GetShipmentPackageDetailsInfoDtoList(childBindingSource2.GetDataTable());
		return shipmentRequestParameterDto;
	}

	private ShipmentHeaderInfoDto GetShipmentHeaderInfoDto(M1BindingSource m1BindingSource, DataRow dataRow)
	{
		ShipmentHeaderInfoDto shipmentHeaderInfoDto = new ShipmentHeaderInfoDto();
		OrganizationLocationInfo organizationLocationInfo = null;
		OrganizationLocationInfo thirdParyLocation = null;
		OrganizationContactDto organizationContactDto = null;
		string text = m1BindingSource.PrimaryBindingSource.Fields["smpShippingMethodID"].RelatedTableGetDataRow("xasCarrier").Field<string>("xasCarrier").Trim();
		shipmentHeaderInfoDto.ShipmentCurrencyCode = (string.IsNullOrEmpty(dataRow.Field<string>("smpCurrencyRateID").ToString().Trim()) ? m1BindingSource.Database.HomeCurrencyID : dataRow.Field<string>("smpCurrencyRateID").ToString().Trim());
		shipmentHeaderInfoDto.USDCurrencyCode = (string.IsNullOrEmpty(m1BindingSource.Database.Props("SM").Field<string>("xsmUSDCurrencyCode")) ? "" : m1BindingSource.Database.Props("SM").Field<string>("xsmUSDCurrencyCode").Trim());
		shipmentHeaderInfoDto.HomeCurrencyCode = m1BindingSource.Database.HomeCurrencyID;
		shipmentHeaderInfoDto.ShipmentCurrencyRate = dataRow.Field<decimal>("smpExchangeRate");
		shipmentHeaderInfoDto.USDCurrencyRate = m1BindingSource.Database.GetExchangeRate(shipmentHeaderInfoDto.USDCurrencyCode, DateTime.Today);
		shipmentHeaderInfoDto.USDCrossCurrencyRateForShipmentCurrency = GetUSDCrossCurrencyRateForShipmentCurrency(m1BindingSource.Database, shipmentHeaderInfoDto.ShipmentCurrencyCode, shipmentHeaderInfoDto.ShipmentCurrencyRate);
		string shippingMethod = dataRow.Field<string>("smpShippingMethodID").ToString().Trim();
		string text2 = dataRow.Field<string>("smpPlantID").ToString().Trim();
		OrganizationAddressInfo locationInfo = (string.IsNullOrEmpty(text2) ? GetShipFromAddressForShipping(m1BindingSource) : GetPlantAddressInfo(m1BindingSource, text2));
		organizationContactDto = GetOrganizationContactInfor(m1BindingSource, dataRow.Field<string>("smpShipOrganizationID").ToString().Trim(), dataRow.Field<string>("smpShipLocationID").ToString().Trim(), dataRow.Field<string>("smpShipContactID").ToString().Trim());
		organizationLocationInfo = GetOrganizationLocationInfo(m1BindingSource, dataRow.Field<string>("smpShipOrganizationID").ToString().Trim(), dataRow.Field<string>("smpShipLocationID").ToString().Trim());
		shipmentHeaderInfoDto.ShipperLocation = GetShipperLocationDto(locationInfo, shippingMethod);
		shipmentHeaderInfoDto.ShipperFromLocation = GetShipperFromLocationDto(locationInfo, shippingMethod);
		shipmentHeaderInfoDto.ShipperToLocation = GetShipperToLocationDto(organizationLocationInfo, shippingMethod, organizationContactDto);
		shipmentHeaderInfoDto.IsInternationalShipment = !shipmentHeaderInfoDto.ShipperFromLocation.LocationAddress.CountryCode.Equals(shipmentHeaderInfoDto.ShipperToLocation.LocationAddress.CountryCode, StringComparison.CurrentCultureIgnoreCase);
		if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("smpBlindShipOrganizationID").Trim()))
		{
			OrganizationLocationInfo organizationLocationInfo2 = GetOrganizationLocationInfo(m1BindingSource, dataRow.Field<string>("smpBlindShipOrganizationID").ToString().Trim(), dataRow.Field<string>("smpBlindShipLocationID").ToString().Trim());
			OrganizationContactDto organizationContactInfor = GetOrganizationContactInfor(m1BindingSource, dataRow.Field<string>("smpBlindShipOrganizationID").ToString().Trim(), dataRow.Field<string>("smpBlindShipLocationID").ToString().Trim(), dataRow.Field<string>("smpBlindShipContactID").ToString().Trim());
			shipmentHeaderInfoDto.BlindShipLocation = GetShipperToLocationDto(organizationLocationInfo2, shippingMethod, organizationContactInfor);
			shipmentHeaderInfoDto.BlindShipContact = organizationContactInfor;
			shipmentHeaderInfoDto.IsBlindShipSet = true;
		}
		if (text.Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
		{
			if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("smpUPS3rdPartyOrganizationID").Trim()))
			{
				thirdParyLocation = GetOrganizationLocationInfo(m1BindingSource, dataRow.Field<string>("smpUPS3rdPartyOrganizationID").ToString().Trim(), dataRow.Field<string>("smpUPS3rdPartyLocationID").ToString().Trim());
			}
			shipmentHeaderInfoDto.BillingOption = dataRow.Field<string>("smpUPSBillingOption").ToString().Trim();
		}
		else if (text.Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
		{
			if (!string.IsNullOrWhiteSpace(dataRow.Field<string>("smpFedEx3rdPartyOrganizationID").Trim()))
			{
				thirdParyLocation = GetOrganizationLocationInfo(m1BindingSource, dataRow.Field<string>("smpFedEx3rdPartyOrganizationID").ToString().Trim(), dataRow.Field<string>("smpFedEx3rdPartyLocationID").ToString().Trim());
			}
			shipmentHeaderInfoDto.BillingOption = dataRow.Field<string>("smpFedExBillingOption").ToString().Trim();
		}
		shipmentHeaderInfoDto.ShippingMethod = shippingMethod;
		shipmentHeaderInfoDto.ShippingCarrier = text;
		shipmentHeaderInfoDto.ShipmentID = dataRow.Field<string>("smpShipmentID").ToString().Trim();
		shipmentHeaderInfoDto.ShipDate = DateTime.Parse(dataRow.Field<DateTime>("smpShipDate").ToString().Trim());
		shipmentHeaderInfoDto.ShipmentUPSAccountNo = dataRow.Field<string>("SmpUPSAccountNumber").Trim();
		shipmentHeaderInfoDto.ShipmentFedExAccountNo = dataRow.Field<string>("smpFedExAccountNumber").Trim();
		shipmentHeaderInfoDto.ShipmentSubtotalInUSD = Math.Round(dataRow.Field<decimal>("smpShipmentSubtotal") * (1m / shipmentHeaderInfoDto.USDCrossCurrencyRateForShipmentCurrency), 4);
		shipmentHeaderInfoDto.FreightChargeInUSD = Math.Round(dataRow.Field<decimal>("smpFreightCharge") * (1m / shipmentHeaderInfoDto.USDCrossCurrencyRateForShipmentCurrency), 4);
		shipmentHeaderInfoDto.ExportingCarrier = dataRow.Field<string>("smpExportingCarrier").Trim();
		shipmentHeaderInfoDto.ReasonForExport = dataRow.Field<string>("smpReasonForExport").Trim();
		shipmentHeaderInfoDto.AESITN = dataRow.Field<string>("smpAESITN").ToString().Trim();
		shipmentHeaderInfoDto.ReturnInstructionsText = dataRow.Field<string>("smpReturnInstructionsText");
		shipmentHeaderInfoDto.ThirdParyLocation = thirdParyLocation;
		shipmentHeaderInfoDto.MasterTrackingNumber = dataRow.Field<string>("smpShipmentIDNumber").Trim();
		shipmentHeaderInfoDto.ShippingComments = dataRow.Field<string>("smpShippingCommentsText").Trim();
		return shipmentHeaderInfoDto;
	}

	private static List<ShipmentLineInfoDto> GetShipmentLineInfoDtoList(M1BindingSource m1BindingSource, DataTable dataTable, ShipmentHeaderInfoDto shipmentHeaderInfo)
	{
		List<ShipmentLineInfoDto> list = new List<ShipmentLineInfoDto>();
		IPartRepository partRepository = new PartRepository();
		foreach (DataRow row in dataTable.Rows)
		{
			ShipmentLineInfoDto shipmentLineInfoDto = new ShipmentLineInfoDto
			{
				PartRevisionInfo = new PartRevisionInformationDto(),
				ShipmentID = row.Field<string>("smlShipmentID"),
				ShipmentLineID = row.Field<short>("smlShipmentLineID"),
				PartID = row.Field<string>("smlPartID"),
				PartRevisionID = row.Field<string>("smlPartRevisionID"),
				Description = row.Field<string>("smlDescription"),
				UnitOfMeasure = row.Field<string>("smlUnitOfMeasure"),
				UnitPriceUSD = Math.Round(row.Field<decimal>("smlUnitPrice") * (1m / shipmentHeaderInfo.USDCrossCurrencyRateForShipmentCurrency), 5),
				QuantityShipped = row.Field<decimal>("smlQuantityShipped")
			};
			PartRevisionInformationDto partRevisionInfo = partRepository.GetPartRevisionInfo(m1BindingSource.Database, shipmentLineInfoDto.PartID, shipmentLineInfoDto.PartRevisionID);
			shipmentLineInfoDto.PartRevisionInfo = partRevisionInfo;
			list.Add(shipmentLineInfoDto);
		}
		return list;
	}

	private static List<ShipmentPackageInfoDto> GetShipmentPackageInfoDtoList(M1BindingSource m1BindingSource, DataTable dataTable, ShipmentHeaderInfoDto shipmentHeaderInfo)
	{
		List<ShipmentPackageInfoDto> list = new List<ShipmentPackageInfoDto>();
		string text = m1BindingSource.PrimaryBindingSource.Fields["smpShippingMethodID"].RelatedTableGetDataRow("xasCarrier").Field<string>("xasCarrier").Trim();
		foreach (DataRow row in dataTable.Rows)
		{
			if (string.IsNullOrEmpty(row.Field<string>("spaTrackingNo")))
			{
				ShipmentPackageInfoDto shipmentPackageInfoDto = new ShipmentPackageInfoDto
				{
					PackageID = row.Field<int>("spaShipmentPackageID")
				};
				if (text.Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
				{
					shipmentPackageInfoDto.PackageType = row.Field<string>("spaUPSPackageTypes");
				}
				else if (text.Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
				{
					shipmentPackageInfoDto.PackageType = row.Field<string>("spaFedExPackageTypes");
				}
				shipmentPackageInfoDto.Height = row.Field<int>("spaPackageHeight");
				shipmentPackageInfoDto.Width = row.Field<int>("spaPackageWidth");
				shipmentPackageInfoDto.Length = row.Field<int>("spaPackageLength");
				if (shipmentPackageInfoDto.Height > 0 && shipmentPackageInfoDto.Width > 0 && shipmentPackageInfoDto.Length > 0)
				{
					shipmentPackageInfoDto.HasDimensions = true;
				}
				shipmentPackageInfoDto.DimensionUOM = row.Field<string>("spaPackageDimensionsUOM");
				shipmentPackageInfoDto.Weight = row.Field<decimal>("spaPackageWeight");
				if (shipmentPackageInfoDto.Weight > 0m)
				{
					shipmentPackageInfoDto.HasWeight = true;
				}
				shipmentPackageInfoDto.WeightUOM = row.Field<string>("spaPackageWeightUOM");
				shipmentPackageInfoDto.IsVerbalConfirmationRequired = row.Field<bool>("spaVerbalConfirmationRequired");
				shipmentPackageInfoDto.IsLargePackage = row.Field<bool>("spaLargePackage");
				shipmentPackageInfoDto.IsAdditionalHandlingRequired = row.Field<bool>("spaAdditionalHandlingRequired");
				shipmentPackageInfoDto.TrackingNo = row.Field<string>("spaTrackingNo");
				shipmentPackageInfoDto.Reference1 = row.Field<string>("spaReference1");
				shipmentPackageInfoDto.Reference2 = row.Field<string>("spaReference2");
				shipmentPackageInfoDto.LabelFilePath = row.Field<string>("spaLabelFilePath");
				shipmentPackageInfoDto.PackageValueInUSD = Math.Round(row.Field<decimal>("spaPackageValue") * (1m / shipmentHeaderInfo.USDCrossCurrencyRateForShipmentCurrency), 4);
				shipmentPackageInfoDto.HasInsured = false;
				shipmentPackageInfoDto.InsuredValueUSD = 0m;
				list.Add(shipmentPackageInfoDto);
			}
		}
		return list;
	}

	private static List<ShipmentPackageDetailsInfoDto> GetShipmentPackageDetailsInfoDtoList(DataTable dataTable)
	{
		List<ShipmentPackageDetailsInfoDto> list = new List<ShipmentPackageDetailsInfoDto>();
		foreach (DataRow row in dataTable.Rows)
		{
			ShipmentPackageDetailsInfoDto item = new ShipmentPackageDetailsInfoDto
			{
				ShipmentLineID = row.Field<short>("spdShipmentLineID"),
				ShipmentPackageID = row.Field<int>("spdShipmentPackageID"),
				PartID = row.Field<string>("spdPartID"),
				PartRevisionID = row.Field<string>("spdPartRevisionID"),
				PackageLineQuantity = row.Field<decimal>("spdQuantity"),
				PackageLineWeight = row.Field<decimal>("spdWeight"),
				CommodityDescription = row.Field<string>("spdCommodityDescription"),
				CountryOfManufacture = row.Field<string>("spdCountryOfManufacture")
			};
			list.Add(item);
		}
		return list;
	}

	private void GetFedExShippingPackageRates(M1BindingSource m1BindingSource)
	{
		FedExRate fedExRate = new FedExRate(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		ShipmentRequestParameterDto constructedShipmentRequestParameterDto = GetConstructedShipmentRequestParameterDto(m1BindingSource);
		if (!fedExRate.GetPackageRate(constructedShipmentRequestParameterDto, out var fedExShipmentRatesResponse, ref errorText))
		{
			MessageBox.Show(errorText.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			SetFedExRatePackageFieldValues(m1BindingSource, fedExShipmentRatesResponse);
		}
	}

	private void GetUPSShippingPackageRates(M1BindingSource m1BindingSource)
	{
		UPSRate uPSRate = new UPSRate(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		ShipmentRequestParameterDto constructedShipmentRequestParameterDto = GetConstructedShipmentRequestParameterDto(m1BindingSource);
		if (!uPSRate.GetPackageRate(constructedShipmentRequestParameterDto, out var shipmentRatesResponseDto, ref errorText))
		{
			MessageBox.Show(errorText.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			SetUPSRatePackageFieldValues(m1BindingSource, shipmentRatesResponseDto);
		}
	}

	private void GetFedExShippingPackageShipment(M1BindingSource m1BindingSource)
	{
		FedExShip fedExShip = new FedExShip(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		ShipmentRequestParameterDto constructedShipmentRequestParameterDto = GetConstructedShipmentRequestParameterDto(m1BindingSource);
		if (!fedExShip.GetPackageShipment(constructedShipmentRequestParameterDto, out var shipmentShipResponseDto, ref errorText))
		{
			MessageBox.Show(errorText.ToString(), "Shipments-Errors", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return;
		}
		SetFedExShipmentPackageFieldValues(m1BindingSource, shipmentShipResponseDto);
		if (errorText.Length > 0)
		{
			MessageBox.Show(errorText.ToString(), "Shipments-Warnings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GetUPSShippingPackageShipment(M1BindingSource m1BindingSource)
	{
		UPSShip uPSShip = new UPSShip(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		ShipmentRequestParameterDto constructedShipmentRequestParameterDto = GetConstructedShipmentRequestParameterDto(m1BindingSource);
		if (!uPSShip.GetPackageShipment(constructedShipmentRequestParameterDto, out var shipmentShipResponseDto, ref errorText))
		{
			MessageBox.Show(errorText.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			SetUPSShipmentPackageFieldValues(m1BindingSource, shipmentShipResponseDto);
		}
	}

	private void GetFedExShipmentTrackingInfo(M1BindingSource m1BindingSource)
	{
		FedExTrack fedExTrack = new FedExTrack(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		ShipmentRequestParameterDto constructedShipmentRequestParameterDto = GetConstructedShipmentRequestParameterDto(m1BindingSource);
		if (!fedExTrack.GetShipmentTrackingInfo(constructedShipmentRequestParameterDto, out var responseMessage, ref errorText))
		{
			MessageBox.Show(errorText.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		else
		{
			MessageBox.Show(responseMessage, "FedEx Shipment Tracking Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void GetUPSShipmentTrackingInfo(M1BindingSource m1BindingSource)
	{
		UPSTrack uPSTrack = new UPSTrack(m1BindingSource.Database);
		StringBuilder errorText = new StringBuilder();
		DataRow currentAsDataRow = m1BindingSource.PrimaryBindingSource.CurrentAsDataRow;
		if (currentAsDataRow == null)
		{
			return;
		}
		string text = currentAsDataRow["smpShipmentIDNumber"].ToString().Trim();
		if (!string.IsNullOrEmpty(text))
		{
			if (!uPSTrack.GetShipmentTrackingInfo(text, out var responseMessage, ref errorText))
			{
				MessageBox.Show(errorText.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				MessageBox.Show(responseMessage, "UPS Shipment Tracking Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}
	}

	private void SetUPSRatePackageFieldValues(M1BindingSource m1BindingSource, UPSShipmentRatesResponseDto shipmentRatesResponseDto)
	{
		List<ShipmentPackageInfoDto> packageList = shipmentRatesResponseDto.PackageList;
		DataTable dataTable = m1BindingSource.GetDataTable();
		int num = 0;
		decimal num2 = default(decimal);
		decimal num3 = 1m;
		DataRow hRow = m1BindingSource.PrimaryBindingSource.CurrentAsDataRow;
		string shipmentCurrencyCode = hRow.Field<string>("smpCurrencyRateID").Trim();
		decimal shipmentCurrencyExchangeRate = hRow.Field<decimal>("smpExchangeRate");
		num3 = GetUSDCrossCurrencyRateForShipmentCurrency(m1BindingSource.Database, shipmentCurrencyCode, shipmentCurrencyExchangeRate);
		if (packageList.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				row.BeginEdit();
				num2 = Math.Round(packageList[num].UPSTotalCharge * num3, 2);
				row.SetField("spaPackageRate", num2);
				row.EndEdit();
				num++;
			}
		}
		else
		{
			foreach (DataRow row2 in dataTable.Rows)
			{
				row2.BeginEdit();
				row2.SetField("spaPackageRate", 0m);
				row2.EndEdit();
				num++;
			}
		}
		m1BindingSource.SaveData();
		hRow.BeginEdit();
		InitializeSmpFreightFields(ref hRow);
		num2 = Math.Round(shipmentRatesResponseDto.UPSTransportationCharge * num3, 2);
		hRow.SetField("smpListBaseChargeBase", num2);
		num2 = Math.Round(shipmentRatesResponseDto.UPSServiceOptionCharge * num3, 2);
		hRow.SetField("smpListSurchargeBase", num2);
		num2 = Math.Round((shipmentRatesResponseDto.UPSNegotiatedCharge.Equals(0m) ? 0m : (shipmentRatesResponseDto.UPSTotalCharge - shipmentRatesResponseDto.UPSNegotiatedCharge)) * num3, 2);
		hRow.SetField("smpListDiscountBase", num2);
		num2 = Math.Round((shipmentRatesResponseDto.UPSNegotiatedCharge.Equals(0m) ? shipmentRatesResponseDto.UPSTotalCharge : shipmentRatesResponseDto.UPSNegotiatedCharge) * num3, 2);
		hRow.SetField("smpListCarrierFreightBase", num2);
		num2 = Math.Round(shipmentRatesResponseDto.UPSTotalCharge * num3, 2);
		hRow.SetField("SmpFreightCharge", num2);
		hRow.EndEdit();
		m1BindingSource.PrimaryBindingSource.SaveData();
	}

	private void SetUPSShipmentPackageFieldValues(M1BindingSource m1BindingSource, UPSShipmentShipResponseDto shipmentShipResponseDto)
	{
		List<ShipmentPackageInfoDto> packageList = shipmentShipResponseDto.PackageList;
		DataTable dataTable = m1BindingSource.GetDataTable();
		int num = 0;
		DataRow hRow = m1BindingSource.PrimaryBindingSource.CurrentAsDataRow;
		decimal num2 = GetUSDCrossCurrencyRateForShipmentCurrency(shipmentCurrencyCode: hRow.Field<string>("smpCurrencyRateID").Trim(), shipmentCurrencyExchangeRate: hRow.Field<decimal>("smpExchangeRate"), m1Database: m1BindingSource.Database);
		foreach (DataRow row in dataTable.Rows)
		{
			row.BeginEdit();
			row.SetField("spaPackageRate", 0m);
			row.SetField("spaTrackingNo", packageList[num].TrackingNo);
			row.SetField("spaLabelFilePath", packageList[num].LabelFilePath);
			row.EndEdit();
			num++;
		}
		m1BindingSource.SaveData();
		InitializeSmpFreightFields(ref hRow);
		hRow.BeginEdit();
		decimal value = Math.Round(shipmentShipResponseDto.TransportationCharge * num2, 2);
		hRow.SetField("smpListBaseChargeBase", value);
		value = Math.Round(shipmentShipResponseDto.ServiceOptionCharge * num2, 2);
		hRow.SetField("smpListSurchargeBase", value);
		value = Math.Round((shipmentShipResponseDto.NegotiatedCharge.Equals(0m) ? 0m : (shipmentShipResponseDto.TotalCharge - shipmentShipResponseDto.NegotiatedCharge)) * num2, 2);
		hRow.SetField("smpListDiscountBase", value);
		value = Math.Round((shipmentShipResponseDto.NegotiatedCharge.Equals(0m) ? shipmentShipResponseDto.TotalCharge : shipmentShipResponseDto.NegotiatedCharge) * num2, 2);
		hRow.SetField("smpListCarrierFreightBase", value);
		value = Math.Round(shipmentShipResponseDto.TotalCharge * num2, 2);
		hRow.SetField("SmpFreightCharge", value);
		hRow.SetField("smpShipmentIDNumber", shipmentShipResponseDto.IdentificationNumber);
		hRow.SetField("smpTrackingNumber", shipmentShipResponseDto.IdentificationNumber);
		hRow.EndEdit();
		m1BindingSource.PrimaryBindingSource.SaveData();
	}

	private void SetFedExRatePackageFieldValues(M1BindingSource m1BindingSource, FedExShipmentRatesResponseDto shipmentRatesResponseDto)
	{
		int num = 0;
		bool flag = true;
		DataRow hRow = m1BindingSource.PrimaryBindingSource.CurrentAsDataRow;
		string shipmentCurrencyCode = hRow.Field<string>("smpCurrencyRateID").Trim();
		decimal shipmentCurrencyExchangeRate = hRow.Field<decimal>("smpExchangeRate");
		decimal uSDCrossCurrencyRateForShipmentCurrency = GetUSDCrossCurrencyRateForShipmentCurrency(m1BindingSource.Database, shipmentCurrencyCode, shipmentCurrencyExchangeRate);
		string[] array = shipmentRatesResponseDto.RateType.Split('_');
		if (array.Length != 0 && array.Any((string rt) => rt.Trim().Equals("LIST", StringComparison.CurrentCultureIgnoreCase)))
		{
			flag = false;
		}
		if (shipmentRatesResponseDto.PackageList.Count > 0)
		{
			foreach (DataRow row in m1BindingSource.GetDataTable().Rows)
			{
				row.BeginEdit();
				decimal value = Math.Round(shipmentRatesResponseDto.PackageList[num].FDXTotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
				row.SetField("spaPackageRate", value);
				row.EndEdit();
				num++;
			}
			m1BindingSource.SaveData();
		}
		else
		{
			foreach (DataRow row2 in m1BindingSource.GetDataTable().Rows)
			{
				row2.BeginEdit();
				row2.SetField("spaPackageRate", 0m);
				row2.EndEdit();
				num++;
			}
			m1BindingSource.SaveData();
		}
		hRow.BeginEdit();
		InitializeSmpFreightFields(ref hRow);
		if (flag)
		{
			decimal value = Math.Round(shipmentRatesResponseDto.TotalBaseCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpAccBaseChargeBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalSurcharges * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpAccSurchargeBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalDiscounts * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpAccDiscountBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpAccCarrierFreightBase", value);
			hRow.SetField("smpFreightCharge", value);
		}
		else
		{
			decimal value = Math.Round(shipmentRatesResponseDto.TotalBaseCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpListBaseChargeBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalSurcharges * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpListSurchargeBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalDiscounts * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpListDiscountBase", value);
			value = Math.Round(shipmentRatesResponseDto.TotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			hRow.SetField("smpListCarrierFreightBase", value);
			hRow.SetField("smpFreightCharge", value);
		}
		hRow.EndEdit();
		m1BindingSource.PrimaryBindingSource.SaveData();
	}

	private void SetFedExShipmentPackageFieldValues(M1BindingSource m1BindingSource, FedExShipmentShipResponseDto shipmentShipResponseDto)
	{
		int num = 0;
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		DataRow hRow = m1BindingSource.PrimaryBindingSource.CurrentAsDataRow;
		string shipmentCurrencyCode = hRow.Field<string>("smpCurrencyRateID").ToString().Trim();
		decimal shipmentCurrencyExchangeRate = hRow.Field<decimal>("smpExchangeRate");
		decimal uSDCrossCurrencyRateForShipmentCurrency = GetUSDCrossCurrencyRateForShipmentCurrency(m1BindingSource.Database, shipmentCurrencyCode, shipmentCurrencyExchangeRate);
		if (shipmentShipResponseDto.PackageList.Count > 0 && !string.IsNullOrEmpty(shipmentShipResponseDto.PackageList[0].FDXRateType) && shipmentShipResponseDto.PackageList[0].FDXRateType.Split('_').ToList()[1].Trim().Equals("LIST", StringComparison.CurrentCultureIgnoreCase))
		{
			flag = false;
		}
		foreach (ShipmentPackageInfoDto package in shipmentShipResponseDto.PackageList)
		{
			string filterExpression = "spaShipmentPackageID = " + M1Util.ConvertToSql(package.PackageID);
			DataRow dataRow = m1BindingSource.GetDataTable().Select(filterExpression)[0];
			if (!package.FDXShipmentProcessingFailed)
			{
				dataRow.BeginEdit();
				decimal value = Math.Round(shipmentShipResponseDto.PackageList[num].FDXTotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
				dataRow.SetField("spaPackageRate", value);
				dataRow.SetField("spaTrackingNo", shipmentShipResponseDto.PackageList[num].TrackingNo);
				dataRow.SetField("spaLabelFilePath", shipmentShipResponseDto.PackageList[num].LabelFilePath);
				dataRow.EndEdit();
			}
			else
			{
				stringBuilder.AppendLine("Package [" + shipmentShipResponseDto.PackageList[num].PackageID.ToString().Trim() + "] processing failed!");
				stringBuilder.AppendLine("Error Message: [" + shipmentShipResponseDto.PackageList[num].FDXShipmentProcessingFailedErrorMsg.Trim() + "].");
			}
			num++;
		}
		if (stringBuilder.Length > 0)
		{
			MessageBox.Show(stringBuilder.ToString(), "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		m1BindingSource.SaveData();
		hRow.BeginEdit();
		if (string.IsNullOrEmpty(hRow.Field<string>("smpShipmentIDNumber")))
		{
			InitializeSmpFreightFields(ref hRow);
		}
		if (flag)
		{
			decimal value = Math.Round(shipmentShipResponseDto.TotalBaseCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			decimal num2 = hRow.Field<decimal>("smpAccBaseChargeBase");
			hRow.SetField("smpAccBaseChargeBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalSurcharges * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpAccSurchargeBase");
			hRow.SetField("smpAccSurchargeBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalDiscounts * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpAccDiscountBase");
			hRow.SetField("smpAccDiscountBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpAccCarrierFreightBase");
			hRow.SetField("smpAccCarrierFreightBase", num2 + value);
			num2 = hRow.Field<decimal>("smpFreightCharge");
			hRow.SetField("smpFreightCharge", num2 + value);
		}
		else
		{
			decimal value = Math.Round(shipmentShipResponseDto.TotalBaseCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			decimal num2 = hRow.Field<decimal>("smpListBaseChargeBase");
			hRow.SetField("smpListBaseChargeBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalSurcharges * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpListSurchargeBase");
			hRow.SetField("smpListSurchargeBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalDiscounts * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpListDiscountBase");
			hRow.SetField("smpListDiscountBase", num2 + value);
			value = Math.Round(shipmentShipResponseDto.TotalNetCharge * uSDCrossCurrencyRateForShipmentCurrency, 2);
			num2 = hRow.Field<decimal>("smpListCarrierFreightBase");
			hRow.SetField("smpListCarrierFreightBase", num2 + value);
			num2 = hRow.Field<decimal>("smpFreightCharge");
			hRow.SetField("smpFreightCharge", num2 + value);
		}
		hRow.SetField("smpShipmentIDNumber", shipmentShipResponseDto.MasterTrackingNumber);
		hRow.SetField("smpTrackingNumber", shipmentShipResponseDto.MasterTrackingNumber);
		hRow.SetField("smpCODLabelFilePath", shipmentShipResponseDto.PackageList[0].FDXCODLabelPath);
		hRow.EndEdit();
		m1BindingSource.PrimaryBindingSource.SaveData();
	}

	private static byte GetDeliveryType(M1BindingSource bindingsource, DataRow lineRow)
	{
		byte b = 0;
		using SqlCommand sqlCommand = new SqlCommand("SELECT omdDeliveryType FROM SalesOrderDeliveries WHERE omdSalesOrderID = @orderId And omdSalesOrderLineID = @lineId AND omdSalesOrderDeliveryID = @deliveryId");
		sqlCommand.Parameters.AddWithValue("@orderId", lineRow.Field<string>("smlSalesOrderID").Trim());
		sqlCommand.Parameters.AddWithValue("@lineId", lineRow.Field<short>("smlSalesOrderLineID"));
		sqlCommand.Parameters.AddWithValue("@deliveryId", lineRow.Field<short>("smlSalesOrderDeliveryID"));
		object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
		return (obj == null) ? Convert.ToByte("0") : Convert.ToByte(obj);
	}

	public static IList<string> VerifyQuantityAgainstInventory(M1BindingSource bindingsource, IDictionary<PartInformation, decimal> dicPartQuantities)
	{
		IList<string> list = new List<string>();
		IList<string> list2 = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<PartInformation, decimal> dicPartQuantity in dicPartQuantities)
		{
			stringBuilder.Length = 0;
			SqlCommand sqlCommand = new SqlCommand("Select impNonStockedItem from Parts where impPartID = @partID");
			sqlCommand.Parameters.Add(new SqlParameter("@partID", dicPartQuantity.Key.Part));
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			bool value = obj == null || Convert.ToBoolean(obj);
			if (obj == null || Convert.ToBoolean(value))
			{
				continue;
			}
			bool flag = (bool)bindingsource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
			using SqlCommand sqlCommand2 = new SqlCommand("SELECT ISNULL(imbQuantityOnHand, 0) AS imbQuantityOnHand FROM PartBins WHERE (imbPartID = @PartID) AND (imbPartRevisionID = @PartRevisionID) AND (imbWarehouseID = @WarehouseID) AND (imbPartBinID = @PartBinID) " + (flag ? string.Empty : " AND (imbQuantityOnHand > 0)"));
			sqlCommand2.Parameters.AddWithValue("@PartID", dicPartQuantity.Key.Part);
			sqlCommand2.Parameters.AddWithValue("@PartRevisionID", dicPartQuantity.Key.PartRevision);
			sqlCommand2.Parameters.AddWithValue("@WarehouseID", dicPartQuantity.Key.PartWarehouse);
			sqlCommand2.Parameters.AddWithValue("@PartBinID", dicPartQuantity.Key.PartBin);
			obj = bindingsource.Database.ExecuteScalar(sqlCommand2);
			decimal num = ((obj == null) ? 0m : Convert.ToDecimal(obj));
			if (num - dicPartQuantity.Value < 0m)
			{
				dicPartQuantity.Key.HasNegativeQOH = true;
				if (dicPartQuantity.Key.IsBinInactive)
				{
					stringBuilder.AppendLine("Quantity to Ship [" + $"{dicPartQuantity.Value}" + "] IS GREATER THAN Quantity On Hand [" + $"{num}" + "]\n[Part: '" + dicPartQuantity.Key.Part + "', Revision: '" + dicPartQuantity.Key.PartRevision + "', Warehouse: '" + dicPartQuantity.Key.PartWarehouse + "', Bin: '" + dicPartQuantity.Key.PartBin + "'].");
					list.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else
				{
					stringBuilder.AppendLine("Quantity to Ship [" + $"{dicPartQuantity.Value}" + "] IS GREATER THAN Quantity On Hand [" + $"{num}" + "]\n[Part: '" + dicPartQuantity.Key.Part + "', Revision: '" + dicPartQuantity.Key.PartRevision + "', Warehouse: '" + dicPartQuantity.Key.PartWarehouse + "', Bin: '" + dicPartQuantity.Key.PartBin + "'].");
					list2.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
			}
		}
		if (!list.Any())
		{
			return list2;
		}
		return list;
	}

	public bool VerifyIfNonStockedPartAndDeliveryType(M1BindingSource bindingsource, DataTable shipmentLineDt)
	{
		foreach (DataRow row in shipmentLineDt.Rows)
		{
			byte deliveryType = GetDeliveryType(bindingsource, row);
			SqlCommand sqlCommand = new SqlCommand("Select impNonStockedItem from Parts where impPartID = @partID and impPhantomOrKitPart != 1");
			sqlCommand.Parameters.Add(new SqlParameter("@partID", row.Field<string>("smlPartID").Trim()));
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand);
			if (obj != null && Convert.ToBoolean(obj) && deliveryType != 0 && deliveryType != Convert.ToByte(SalesOrderDeliveryType.MakeToOrder) && deliveryType != Convert.ToByte(SalesOrderDeliveryType.PurchaseToOrder))
			{
				return true;
			}
		}
		return false;
	}

	private void PopulatePartInfoDictionary(M1BindingSource lineBindingsource, IDictionary<PartInformation, decimal> dicPartInfo, DataRow lineRow)
	{
		M1Database database = lineBindingsource.Database;
		decimal num;
		PartInformation key;
		if (lineRow.Field<bool>("smlKitPart"))
		{
			M1BindingSource childBindingSource = lineBindingsource.PrimaryTable.GetChildBindingSource("ShipmentComponents");
			DataTable dataTable = childBindingSource.GetDataView(lineRow).ToTable();
			if (dataTable.Rows.Count == 0)
			{
				return;
			}
			{
				foreach (DataRow row in dataTable.Rows)
				{
					num = row.Field<decimal>("smoQuantityShipped");
					key = CreatePartInfoKey(database, row, childBindingSource.PrimaryTable.FieldPrefix);
					if (dicPartInfo.ContainsKey(key))
					{
						dicPartInfo[key] += num;
					}
					else
					{
						dicPartInfo.Add(key, num);
					}
				}
				return;
			}
		}
		num = lineRow.Field<decimal>("smlQuantityShipped");
		key = CreatePartInfoKey(database, lineRow, lineBindingsource.PrimaryTable.FieldPrefix);
		if (dicPartInfo.ContainsKey(key))
		{
			dicPartInfo[key] += num;
		}
		else
		{
			dicPartInfo.Add(key, num);
		}
	}

	private static PartInformation CreatePartInfoKey(M1Database database, DataRow row, string prefix)
	{
		Part part = new Part();
		if (row != null)
		{
			string text = row.Field<string>(prefix + "PartID").Trim();
			string partRevision = row.Field<string>(prefix + "PartRevisionID").Trim();
			string text2 = row.Field<string>(prefix + "PartWarehouseLocationID").Trim();
			string text3 = row.Field<string>(prefix + "PartBinID").Trim();
			return new PartInformation
			{
				Part = text,
				PartRevision = partRevision,
				PartWarehouse = text2,
				PartBin = text3,
				IsBinInactive = part.IsPartBinInactive(database, text, partRevision, text2, text3)
			};
		}
		return null;
	}

	public void GetShippingPackageRates(M1BindingSource m1BindingSource)
	{
		string text = m1BindingSource.PrimaryBindingSource.Fields["smpShippingMethodID"].RelatedTableGetDataRow("xasCarrier").Field<string>("xasCarrier").Trim();
		if (text.Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
		{
			GetUPSShippingPackageRates(m1BindingSource);
		}
		else if (text.Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
		{
			GetFedExShippingPackageRates(m1BindingSource);
		}
	}

	public void GetShippingPackageShipment(M1BindingSource m1BindingSource)
	{
		string text = m1BindingSource.PrimaryBindingSource.Fields["smpShippingMethodID"].RelatedTableGetDataRow("xasCarrier").Field<string>("xasCarrier").Trim();
		if (text.Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
		{
			GetUPSShippingPackageShipment(m1BindingSource);
		}
		else if (text.Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
		{
			GetFedExShippingPackageShipment(m1BindingSource);
		}
	}

	public void GetShipmentTrackingInfo(M1BindingSource m1BindingSource)
	{
		string text = m1BindingSource.PrimaryBindingSource.Fields["smpShippingMethodID"].RelatedTableGetDataRow("xasCarrier").Field<string>("xasCarrier").Trim();
		if (text.Equals("UPS", StringComparison.CurrentCultureIgnoreCase))
		{
			GetUPSShipmentTrackingInfo(m1BindingSource);
		}
		else if (text.Equals("FDXG", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FDXE", StringComparison.CurrentCultureIgnoreCase) || text.Equals("FEDEX", StringComparison.CurrentCultureIgnoreCase))
		{
			GetFedExShipmentTrackingInfo(m1BindingSource);
		}
	}

	public void PrintLabel(M1BindingSource m1BindingSource)
	{
		PrintLabel printLabel = new PrintLabel();
		DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null)
		{
			string text = currentAsDataRow["spaLabelFilePath"].ToString().Trim();
			if (!string.IsNullOrEmpty(text))
			{
				object[] fileLocation = new string[1] { text };
				printLabel.PrintThermoLabel(fileLocation);
			}
			else
			{
				MessageBox.Show("No Label is available to Print.", "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}

	public void PrintAllLabels(M1BindingSource m1BindingSource)
	{
		short num = 0;
		PrintLabel printLabel = new PrintLabel();
		bool flag = false;
		DataTable dataTable = m1BindingSource.GetDataTable();
		string[] array = new string[dataTable.Rows.Count];
		foreach (DataRow row in dataTable.Rows)
		{
			if (!string.IsNullOrEmpty(row["spaLabelFilePath"].ToString()))
			{
				array[num] = row["spaLabelFilePath"].ToString().Trim();
				flag = true;
			}
			num++;
		}
		if (flag)
		{
			object[] fileLocation = array;
			printLabel.PrintThermoLabel(fileLocation);
		}
		else
		{
			MessageBox.Show("No Labels are available to Print.", "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public void PrintCODLabel(M1BindingSource m1BindingSource)
	{
		PrintLabel printLabel = new PrintLabel();
		DataRow currentAsDataRow = m1BindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null)
		{
			if (!string.IsNullOrEmpty(currentAsDataRow["smpCODLabelFilePath"].ToString()))
			{
				string text = currentAsDataRow["smpCODLabelFilePath"].ToString().Trim();
				object[] fileLocation = new string[1] { text };
				printLabel.PrintThermoLabel(fileLocation);
			}
			else
			{
				MessageBox.Show("No COD Label is available to Print.", "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
	}

	public bool CreateDuplicatePackages(M1BindingSource shipmentBs, int noOfPackages)
	{
		bool result = true;
		M1BindingSource childBindingSource = shipmentBs.PrimaryTable.GetChildBindingSource("ShipmentPackages");
		M1BindingSource childBindingSource2 = shipmentBs.PrimaryTable.GetChildBindingSource("ShipmentLines");
		M1BindingSource childBindingSource3 = childBindingSource2.PrimaryTable.GetChildBindingSource("ShipmentPackageDetails");
		DataRow currentAsDataRow = childBindingSource.CurrentAsDataRow;
		if (currentAsDataRow != null && noOfPackages > 0)
		{
			DataTable dataTable = childBindingSource2.GetDataTable();
			DataTable dataTable2 = childBindingSource3.GetDataTable().Copy();
			string shipmentId = currentAsDataRow["spaShipmentID"].ToString().Trim();
			Dictionary<int, decimal> DicTobePackagedLineQty = GetOpenShipmentLineQuantities(dataTable, dataTable2);
			try
			{
				if (dataTable2.Rows.Count > 0)
				{
					for (int i = 0; i < noOfPackages; i++)
					{
						int newPackageId = CreateNewShipmentPackageRow(currentAsDataRow, childBindingSource);
						CreateNewShipmentPackageDetailsRows(newPackageId, dataTable2, childBindingSource3, ref DicTobePackagedLineQty, out var newPackageWeightKgs);
						SetNewPackageProperties(childBindingSource, shipmentId, newPackageId, newPackageWeightKgs);
					}
				}
				else
				{
					for (int j = 0; j < noOfPackages; j++)
					{
						CreateNewShipmentPackageRow(currentAsDataRow, childBindingSource);
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				throw new M1Exception(ex.Message);
			}
		}
		return result;
	}

	public void UpdateSpaReferenceField(M1BindingSource shipmentPackageDetailsBs, M1BindingSource shipmentBs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		M1BindingSource childBindingSource = shipmentBs.PrimaryTable.GetChildBindingSource("ShipmentPackages");
		string customerPO = GetCustomerPO(shipmentPackageDetailsBs, shipmentBs);
		if (string.IsNullOrEmpty(customerPO))
		{
			return;
		}
		DataRow currentAsDataRow = childBindingSource.CurrentAsDataRow;
		string s = currentAsDataRow["spaShipmentID"].ToString().Trim();
		string s2 = currentAsDataRow["spaShipmentPackageID"].ToString();
		stringBuilder.Append("spaShipmentID = ");
		stringBuilder.Append(s.ToLinq());
		stringBuilder.Append(" And spaShipmentPackageID = ");
		stringBuilder.Append(s2.ToLinq());
		DataRow[] array = childBindingSource.GetDataTable().Select(stringBuilder.ToString());
		if (array.Length != 0)
		{
			DataRow[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i]["spaReference1"] = customerPO;
			}
		}
	}

	public void DeletePackage(M1BindingSource packageBindingSource)
	{
		M1BindingSource childBindingSource = packageBindingSource.PrimaryBindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines").PrimaryTable.GetChildBindingSource("ShipmentPackageDetails");
		DataRow currentAsDataRow = packageBindingSource.CurrentAsDataRow;
		string filterExpression = "spdShipmentID=" + M1Util.ConvertToLinq(currentAsDataRow["spaShipmentID"].ToString()) + " AND spdShipmentPackageID= " + M1Util.ConvertToLinq(currentAsDataRow["spaShipmentPackageID"].ToString());
		if (childBindingSource.GetDataTable().Select(filterExpression).Length != 0)
		{
			MessageBox.Show("Package cannot be deleted.\nPackage is currently in use in Shipment Package Details.", "Shipments", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return;
		}
		packageBindingSource.Remove(currentAsDataRow);
		packageBindingSource.SaveData();
	}

	public void PostShipment(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		SqlTransaction sqlTransaction = bindingSource.Transaction ?? database.BeginTransaction();
		try
		{
			bindingSource.CurrentAsDataRow.BeginEdit();
			bindingSource.CurrentAsDataRow.SetField("smpPostedToGL", value: true);
			bindingSource.CurrentAsDataRow.AcceptChanges();
			string value = bindingSource.CurrentAsDataRow.Field<string>("smpShipmentID");
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			SqlCommand sqlCommand = database.NewSqlCommand("SELECT DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, smlUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction, (CASE WHEN sntTransactionType = 47 THEN CAST(1 AS Bit) ELSE CAST(0 AS Bit) END) AS sntJobAssigned from ShipmentLines inner join SerialNumberTransactions on smlUniqueID = sntTableUniqueID where smlShipmentID = @ID and smlPostedToGL = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			DataTable dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition = new SerialNumberDefinition();
				foreach (DataRow row in dataTable.Rows)
				{
					byte status = 0;
					byte transType = 0;
					bool flag = row.Field<bool>("sntNegativeTransaction");
					serialNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row.Field<string>("sntSerialNumberID"));
					switch (row.Field<byte>("sntTransactionType"))
					{
					case 51:
						status = (byte)(flag ? 2 : 4);
						transType = 5;
						break;
					case 52:
						status = (byte)(flag ? Convert.ToByte(row.Field<bool>("sntJobAssigned") ? 1 : 0) : 4);
						transType = 40;
						break;
					}
					serialNumberDefinition.AddSerialTransaction(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntPartWarehouseLocationID"), row.Field<string>("sntPartBinID"), row.Field<string>("sntSerialNumberID"), row.Field<decimal>("sntQuantity"), status, transType, "ShipmentLines", row.Field<Guid>("smlUniqueID"), row.Field<string>("sntJobID"), Convert.ToInt32(row["sntJobAssemblyID"]), Convert.ToInt32(row["sntJobMaterialID"]), Convert.ToInt32(row["sntJobMaterialComponentID"]), row.Field<bool>("sntNegativeTransaction"), row.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition.RefreshStatuses(database, sqlTransaction, row.Field<string>("sntPartID"), row.Field<string>("sntPartRevisionID"), row.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, sntTransactionDate) As sntTransactionDate, sntSerialNumberID, sntTransactionType, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntQuantity, smoUniqueID, sntJobID, sntJobAssemblyID, sntJobMaterialID, sntJobMaterialComponentID, sntNegativeTransaction, (CASE WHEN sntTransactionType = 47 THEN CAST(1 AS Bit) ELSE CAST(0 AS Bit) END) AS sntJobAssigned from ShipmentComponents inner join SerialNumberTransactions on smoUniqueID = sntTableUniqueID where smoShipmentID = @ID and smoPostedToGL = 0 order by sntSerialNumberID, sntPartID, sntPartRevisionID, sntPartWarehouseLocationID, sntPartBinID, sntTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				SerialNumberDefinition serialNumberDefinition2 = new SerialNumberDefinition();
				foreach (DataRow row2 in dataTable.Rows)
				{
					byte status2 = 0;
					byte transType2 = 0;
					bool flag2 = row2.Field<bool>("sntNegativeTransaction");
					serialNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row2.Field<string>("sntSerialNumberID"));
					switch (row2.Field<byte>("sntTransactionType"))
					{
					case 51:
						status2 = (byte)(flag2 ? 2 : 4);
						transType2 = 5;
						break;
					case 52:
						status2 = (byte)(flag2 ? Convert.ToByte(row2.Field<bool>("sntJobAssigned") ? 1 : 0) : 4);
						transType2 = 40;
						break;
					}
					serialNumberDefinition2.AddSerialTransaction(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntPartWarehouseLocationID"), row2.Field<string>("sntPartBinID"), row2.Field<string>("sntSerialNumberID"), row2.Field<decimal>("sntQuantity"), status2, transType2, "ShipmentComponents", row2.Field<Guid>("smoUniqueID"), row2.Field<string>("sntJobID"), Convert.ToInt32(row2["sntJobAssemblyID"]), Convert.ToInt32(row2["sntJobMaterialID"]), Convert.ToInt32(row2["sntJobMaterialComponentID"]), row2.Field<bool>("sntNegativeTransaction"), row2.Field<DateTime>("sntTransactionDate"));
					serialNumberDefinition2.RefreshStatuses(database, sqlTransaction, row2.Field<string>("sntPartID"), row2.Field<string>("sntPartRevisionID"), row2.Field<string>("sntSerialNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, smlUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, abtNegativeTransaction, (CASE WHEN abtTransactionType = 47 THEN CAST(1 AS Bit) ELSE CAST(0 AS Bit) END) AS abtJobAssigned from ShipmentLines inner join LotNumberTransactions on smlUniqueID = abtTableUniqueID where smlShipmentID = @ID and smlPostedToGL = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition = new LotNumberDefinition();
				foreach (DataRow row3 in dataTable.Rows)
				{
					byte status3 = 0;
					byte transType3 = 0;
					bool flag3 = row3.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition.LoadLotOrSerialNumbers(database, sqlTransaction, row3.Field<string>("abtLotNumberID"));
					switch (row3.Field<byte>("abtTransactionType"))
					{
					case 51:
						status3 = (byte)(flag3 ? 2 : 4);
						transType3 = 5;
						break;
					case 52:
						status3 = (byte)(flag3 ? Convert.ToByte(row3.Field<bool>("abtJobAssigned") ? 1 : 0) : 4);
						transType3 = 40;
						break;
					}
					lotNumberDefinition.AddLotTransaction(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtPartWarehouseLocationID"), row3.Field<string>("abtPartBinID"), row3.Field<string>("abtLotNumberID"), row3.Field<decimal>("abtQuantity"), status3, transType3, "ShipmentLines", row3.Field<Guid>("smlUniqueID"), row3.Field<string>("abtJobID"), Convert.ToInt32(row3["abtJobAssemblyID"]), Convert.ToInt32(row3["abtJobMaterialID"]), Convert.ToInt32(row3["abtJobMaterialComponentID"]), row3.Field<bool>("abtNegativeTransaction"), row3.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition.RefreshStatuses(database, sqlTransaction, row3.Field<string>("abtPartID"), row3.Field<string>("abtPartRevisionID"), row3.Field<string>("abtLotNumberID"));
				}
			}
			sqlCommand = database.NewSqlCommand("select DateAdd(ss, 1, abtTransactionDate) As abtTransactionDate, abtLotNumberID, abtTransactionType, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtQuantity, smoUniqueID, abtJobID, abtJobAssemblyID, abtJobMaterialID, abtJobMaterialComponentID, (CASE WHEN abtTransactionType = 47 THEN CAST(1 AS Bit) ELSE CAST(0 AS Bit) END) AS abtJobAssigned from ShipmentComponents inner join LotNumberTransactions on smoUniqueID = abtTableUniqueID where smoShipmentID = @ID and smoPostedToGL = 0 order by abtLotNumberID, abtPartID, abtPartRevisionID, abtPartWarehouseLocationID, abtPartBinID, abtTransactionDate");
			sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.VarChar)).Value = value;
			dataTable = database.GetDataTable(sqlCommand, sqlTransaction);
			if (dataTable.Rows.Count != 0)
			{
				LotNumberDefinition lotNumberDefinition2 = new LotNumberDefinition();
				foreach (DataRow row4 in dataTable.Rows)
				{
					byte status4 = 0;
					byte transType4 = 0;
					bool flag4 = row4.Field<bool>("abtNegativeTransaction");
					lotNumberDefinition2.LoadLotOrSerialNumbers(database, sqlTransaction, row4.Field<string>("abtLotNumberID"));
					switch (row4.Field<byte>("abtTransactionType"))
					{
					case 51:
						status4 = (byte)(flag4 ? 2 : 4);
						transType4 = 5;
						break;
					case 52:
						status4 = (byte)(flag4 ? Convert.ToByte(row4.Field<bool>("abtJobAssigned") ? 1 : 0) : 4);
						transType4 = 40;
						break;
					}
					lotNumberDefinition2.AddLotTransaction(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtPartWarehouseLocationID"), row4.Field<string>("abtPartBinID"), row4.Field<string>("abtLotNumberID"), row4.Field<decimal>("abtQuantity"), status4, transType4, "ShipmentComponents", row4.Field<Guid>("smoUniqueID"), row4.Field<string>("abtJobID"), Convert.ToInt32(row4["abtJobAssemblyID"]), Convert.ToInt32(row4["abtJobMaterialID"]), Convert.ToInt32(row4["abtJobMaterialComponentID"]), row4.Field<bool>("abtNegativeTransaction"), row4.Field<DateTime>("abtTransactionDate"));
					lotNumberDefinition2.RefreshStatuses(database, sqlTransaction, row4.Field<string>("abtPartID"), row4.Field<string>("abtPartRevisionID"), row4.Field<string>("abtLotNumberID"));
				}
			}
			M1BindingSource m1BindingSource = bindingSource.PrimaryTable?.GetChildBindingSource("ShipmentLines");
			if (m1BindingSource != null && m1BindingSource.Count > 0)
			{
				IntegrationServiceConstants.EntityType entityType = (bindingSource.CurrentAsDataRow.Field<bool>("smpReversalEntry") ? IntegrationServiceConstants.EntityType.CreditMemo : IntegrationServiceConstants.EntityType.Invoice);
				new M1.Ax.Erp.IntegrationService.IntegrationService().CreateTransactionQueueRecord(database, sqlTransaction, IntegrationServiceConstants.IntegrationType.Financial, IntegrationServiceConstants.ApiAction.Create, entityType, IntegrationServiceConstants.Status.Pending, "Shipments", bindingSource.CurrentAsDataRow.Field<Guid>("smpUniqueId"), 13);
			}
			database.CommitTransaction(sqlTransaction);
		}
		catch
		{
			database.RollbackTransaction(sqlTransaction);
			throw;
		}
	}

	public bool ShipmentPeriodCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DateTime dateTime = default(DateTime);
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		bool result = false;
		if (currentAsDataRow != null)
		{
			if (currentAsDataRow.Table.Columns.Contains("smpShipDate"))
			{
				dateTime = currentAsDataRow.Field<DateTime>("smpShipDate");
			}
			if (new Financial().GetYearAndPeriod(database, dateTime.Date, "").Success)
			{
				result = true;
			}
		}
		return result;
	}

	public IDictionary<PartInformation, decimal> GetDictionaryPartQuantities(M1BindingSource bindingsource)
	{
		IDictionary<PartInformation, decimal> dictionary = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
		DataTable dataTable = bindingsource.PrimaryTable.GetChildBindingSource("ShipmentLines").GetDataTable();
		if (dataTable != null && dataTable.Rows.Count != 0)
		{
			byte[] array = new byte[3] { 2, 4, 5 };
			M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("ShipmentLines");
			foreach (DataRow row in dataTable.Rows)
			{
				if (row.Field<string>("smlSourceTableName").Trim().Equals("SalesOrderDeliveries", StringComparison.CurrentCultureIgnoreCase))
				{
					byte deliveryType = GetDeliveryType(bindingsource, row);
					if (Array.IndexOf(array, deliveryType) != -1)
					{
						PopulatePartInfoDictionary(childBindingSource, dictionary, row);
					}
				}
				else if (row.Field<string>("smlSourceTableName").Trim().Equals(string.Empty))
				{
					PopulatePartInfoDictionary(childBindingSource, dictionary, row);
				}
			}
		}
		return dictionary;
	}

	public string PostShipmentCheck(M1BindingSource bindingsource)
	{
		if (bindingsource.CurrentAsDataRow != null)
		{
			if (ShipmentHasWmsLockedLines(bindingsource.Database, bindingsource.Transaction, bindingsource.CurrentAsDataRow.Field<string>("smpShipmentID")))
			{
				return "This record cannot be posted as it has shipment lines locked by WMS.";
			}
			if (ShipmentPostedCheck(bindingsource.Database, bindingsource.Transaction, bindingsource.CurrentAsDataRow.Field<string>("smpShipmentID")))
			{
				return "This record cannot be saved or posted as it is already marked as being posted in the database.";
			}
			if (!bindingsource.CurrentAsDataRow.Field<bool>("smpReversalEntry"))
			{
				DataTable dataTable = bindingsource.PrimaryTable.GetChildBindingSource("ShipmentLines").GetDataTable();
				IDictionary<PartInformation, decimal> dictionaryPartQuantities = GetDictionaryPartQuantities(bindingsource);
				IList<string> list = VerifyQuantityAgainstInventory(bindingsource, dictionaryPartQuantities);
				bool flag = dictionaryPartQuantities.Any((KeyValuePair<PartInformation, decimal> keyValuePair) => keyValuePair.Key.IsBinInactive && keyValuePair.Key.HasNegativeQOH);
				bool num = (bool)bindingsource.Database.Props("IM")["xapIMAllowNegativeQtyOnHand"];
				bool flag2 = (bool)bindingsource.Database.Props("IM")["xapIMEnableWarningWhenNegative"];
				DateTime t = bindingsource.CurrentAsDataRow.Field<DateTime>("smpShipDate");
				if (num && list.Any())
				{
					if (DateTime.Compare(t, DateTime.Now) > 0)
					{
						return "This transaction CAN NOT be posted because future dating is not supported when the transaction will result in a negative quantity on hand.\n\n" + string.Join("\n", list);
					}
					if (flag)
					{
						return "This transaction CAN NOT be posted because it will result in a negative quantity on hand for an INACTIVE bin for the part(s) indicated.\n\n" + string.Join("\n", list);
					}
					if (flag2)
					{
						return "This transaction WILL RESULT in a negative quantity on hand for the part(s) indicated. Are you sure?\n\n" + string.Join("\n", list);
					}
				}
				if (list.Count > 0)
				{
					if (flag)
					{
						return "This transaction CAN NOT be posted because it will result in a negative quantity on hand for an INACTIVE bin for the part(s) indicated.\n\n" + string.Join("\n", list);
					}
					return "This transaction CAN NOT be posted because it will result in a negative quantity on hand for the part(s) indicated.\n\n" + string.Join("\n", list);
				}
				if (bindingsource.Database.Props("GL").Field<bool>("xafGLCreateStockJournals") && VerifyIfNonStockedPartAndDeliveryType(bindingsource, dataTable))
				{
					return "A non-stocked part can only be shipped from a Purchase To Order or Make To Order delivery";
				}
			}
		}
		return string.Empty;
	}

	public string QtyShippedExceedsQtyOnSalesOrder(M1BindingSource shipmentLinesBs, DataRow selectedRow)
	{
		string result = string.Empty;
		PartInfo partInfo = createPartInfo(selectedRow);
		addQtyShippedToPartInfo(partInfo, shipmentLinesBs);
		addQtyShippedFromTheSystem(partInfo, shipmentLinesBs);
		addDeliveryQtyToPartInfo(partInfo, shipmentLinesBs);
		if (partInfo.QuantityShipped > partInfo.DeliveryQuantity)
		{
			result = string.Format("Warning: the current quantity ({0}), plus the quantity for all other shipments ({1}) for this item ({2}) exceeds the delivery quantity ({3}) for order: {4} line {5} delivery {6}.", selectedRow.Field<decimal>("smlQuantityShipped"), partInfo.QuantityShipped - selectedRow.Field<decimal>("smlQuantityShipped"), partInfo.PartID, partInfo.DeliveryQuantity, partInfo.SalesOrderID, partInfo.SalesOrderLineID, partInfo.SalesOrderDeliveryID);
		}
		return result;
	}

	private PartInfo createPartInfo(DataRow selectedRow)
	{
		return new PartInfo
		{
			PartID = selectedRow.Field<string>("smlPartID"),
			PartRevisionID = selectedRow.Field<string>("smlPartRevisionID"),
			PartWarehouseID = selectedRow.Field<string>("smlPartWarehouseLocationID"),
			PartPartBinID = selectedRow.Field<string>("smlPartBinID"),
			SalesOrderID = selectedRow.Field<string>("smlSalesOrderID"),
			SalesOrderLineID = selectedRow.Field<short>("smlSalesOrderLineID"),
			SalesOrderDeliveryID = selectedRow.Field<short>("smlSalesOrderDeliveryID"),
			QuantityShipped = 0m,
			DeliveryQuantity = 0m
		};
	}

	private void addQtyShippedFromTheSystem(PartInfo partInfo, M1BindingSource shipmentLinesBs)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT smlPartID, smlPartRevisionID, smlPartWarehouseLocationID, smlPartBinID, smlSalesOrderID, smlSalesOrderLineID, smlSalesOrderDeliveryID, smlQuantityShipped FROM ShipmentLines WHERE smlPartID = @PartID AND smlPartRevisionID = @PartRevisionID AND smlPartWarehouseLocationID = @PartWarehouseLocationID AND smlPartBinID = @PartBinID AND smlSalesOrderID = @SalesOrderID AND smlSalesOrderLineID = @SalesOrderLineID AND smlSalesOrderDeliveryID = @SalesOrderDeliveryID");
		sqlCommand.Parameters.AddWithValue("@PartID", partInfo.PartID);
		sqlCommand.Parameters.AddWithValue("@PartRevisionID", partInfo.PartRevisionID);
		sqlCommand.Parameters.AddWithValue("@PartWarehouseLocationID", partInfo.PartWarehouseID);
		sqlCommand.Parameters.AddWithValue("@PartBinID", partInfo.PartPartBinID);
		sqlCommand.Parameters.AddWithValue("@SalesOrderID", partInfo.SalesOrderID);
		sqlCommand.Parameters.AddWithValue("@SalesOrderLineID", partInfo.SalesOrderLineID);
		sqlCommand.Parameters.AddWithValue("@SalesOrderDeliveryID", partInfo.SalesOrderDeliveryID);
		DataTable dataTable = shipmentLinesBs.Database.GetDataTable(sqlCommand, shipmentLinesBs.Transaction);
		if (dataTable == null)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			partInfo.QuantityShipped += row.Field<decimal>("smlQuantityShipped");
		}
	}

	private void addQtyShippedToPartInfo(PartInfo partInfo, M1BindingSource shipmentLinesBs)
	{
		DataTable dataTable = shipmentLinesBs.GetDataTable();
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return;
		}
		foreach (DataRow row in dataTable.Rows)
		{
			if (row.RowState != DataRowState.Detached && row.RowState != DataRowState.Deleted)
			{
				if (getDeliveryType(row, shipmentLinesBs.Database, shipmentLinesBs.Transaction) == SalesOrderDeliveryType.PurchaseToOrder)
				{
					addQtyShippedToPartInfo(partInfo, row);
				}
			}
			else
			{
				cleanDeletedRowsErrors(shipmentLinesBs);
			}
		}
	}

	private void cleanDeletedRowsErrors(M1BindingSource shipmentLinesBs)
	{
		ErrorItemsList errors = shipmentLinesBs.GetErrors();
		for (int i = 0; i < errors.Count; i++)
		{
			ValidationInfo validationInfo = errors[i];
			if (validationInfo.Row.RowState == DataRowState.Deleted || validationInfo.Row.RowState == DataRowState.Detached)
			{
				errors.RemoveValidationInfo(validationInfo);
			}
		}
	}

	private void addDeliveryQtyToPartInfo(PartInfo partInfo, M1BindingSource bindingSource)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT omdDeliveryQuantity FROM SalesOrderDeliveries WHERE omdSalesOrderID = @SalesOrderID AND omdSalesOrderLineID = @SalesOrderLineID AND omdSalesOrderDeliveryID = @SalesOrderDeliveryID AND omdDeliveryType = @DeliveryType");
		sqlCommand.Parameters.AddWithValue("@SalesOrderID", partInfo.SalesOrderID);
		sqlCommand.Parameters.AddWithValue("@SalesOrderLineID", partInfo.SalesOrderLineID);
		sqlCommand.Parameters.AddWithValue("@SalesOrderDeliveryID", partInfo.SalesOrderDeliveryID);
		sqlCommand.Parameters.AddWithValue("@DeliveryType", (short)5);
		object obj = bindingSource.Database.ExecuteScalar(sqlCommand, bindingSource.Transaction);
		if (obj != null)
		{
			partInfo.DeliveryQuantity = Convert.ToDecimal(obj);
		}
	}

	private SalesOrderDeliveryType getDeliveryType(DataRow row, M1Database database, SqlTransaction transaction)
	{
		int num = 0;
		using (SqlCommand sqlCommand = new SqlCommand("SELECT omdDeliveryType FROM SalesOrderDeliveries WHERE omdSalesOrderID = @SalesOrderID AND omdSalesOrderLineID = @SalesOrderLineID AND omdSalesOrderDeliveryID = @SalesOrderDeliveryID"))
		{
			sqlCommand.Parameters.AddWithValue("@SalesOrderID", row.Field<string>("smlSalesOrderID"));
			sqlCommand.Parameters.AddWithValue("@SalesOrderLineID", row.Field<short>("smlSalesOrderLineID"));
			sqlCommand.Parameters.AddWithValue("@SalesOrderDeliveryID", row.Field<short>("smlSalesOrderDeliveryID"));
			object obj = database.ExecuteScalar(sqlCommand, transaction);
			if (obj != null)
			{
				num = Convert.ToInt32(obj);
			}
		}
		return (SalesOrderDeliveryType)num;
	}

	private void addQtyShippedToPartInfo(PartInfo partInfo, DataRow row)
	{
		if (!partInfo.PartID.Equals(row.Field<string>("smlPartID"), StringComparison.CurrentCultureIgnoreCase) || !partInfo.PartRevisionID.Equals(row.Field<string>("smlPartRevisionID"), StringComparison.CurrentCultureIgnoreCase) || !partInfo.PartWarehouseID.Equals(row.Field<string>("smlPartWarehouseLocationID"), StringComparison.CurrentCultureIgnoreCase) || !partInfo.PartPartBinID.Equals(row.Field<string>("smlPartBinID"), StringComparison.CurrentCultureIgnoreCase) || !partInfo.SalesOrderID.Equals(row.Field<string>("smlSalesOrderID"), StringComparison.CurrentCultureIgnoreCase) || partInfo.SalesOrderLineID != row.Field<short>("smlSalesOrderLineID") || partInfo.SalesOrderDeliveryID != row.Field<short>("smlSalesOrderDeliveryID"))
		{
			return;
		}
		if (row.RowState == DataRowState.Added)
		{
			partInfo.QuantityShipped += row.Field<decimal>("smlQuantityShipped");
			return;
		}
		decimal num = row.Field<decimal>("smlQuantityShipped");
		decimal num2 = row.Field<decimal>("smlQuantityShipped", DataRowVersion.Original);
		if (num2 != num)
		{
			partInfo.QuantityShipped += num - num2;
		}
	}

	public bool ShipmentPostedCheck(M1Database database, SqlTransaction transaction, string shipmentID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("Select IsNull(smpPostedToGL,0) As smpPostedToGL From Shipments Where smpShipmentID = @ID");
		sqlCommand.Parameters.Add(new SqlParameter("@ID", SqlDbType.NVarChar)).Value = shipmentID;
		object obj = database.ExecuteScalar(sqlCommand, transaction);
		if (obj == null)
		{
			return false;
		}
		return (bool)obj;
	}

	public bool ShipmentHasWmsLockedLines(M1Database database, SqlTransaction transaction, string shipmentID)
	{
		SqlCommand sqlCommand = database.NewSqlCommand("SELECT COUNT(*) FROM ShipmentLines WHERE smlShipmentID = @ShipmentID AND smlWMSPickInProgress = 1");
		sqlCommand.Parameters.Add(new SqlParameter("@ShipmentID", SqlDbType.NVarChar)).Value = shipmentID;
		return (int)database.ExecuteScalar(sqlCommand, transaction) > 0;
	}

	public string CheckShipmentForFutureAdjustmentTransactions(M1BindingSource bindingsource)
	{
		IDictionary<PartInformation, decimal> dicPartInfo = new Dictionary<PartInformation, decimal>(new PartInformationEqualityComparer());
		if (bindingsource.CurrentAsDataRow != null && !bindingsource.CurrentAsDataRow.Field<bool>("smpReversalEntry"))
		{
			M1BindingSource childBindingSource = bindingsource.PrimaryTable.GetChildBindingSource("ShipmentLines");
			DataTable dataTable = childBindingSource.GetDataTable();
			if (dataTable != null && dataTable.Rows.Count != 0)
			{
				foreach (DataRow row in dataTable.Rows)
				{
					PopulatePartInfoDictionary(childBindingSource, dicPartInfo, row);
				}
				IList<string> list = checkPartForFutureDatesTransactions(bindingsource, dicPartInfo);
				if (list.Count > 0)
				{
					return "There are future quantity adjustments for the following parts. If you continue, the current quantity on hand will not be adjusted for these parts.\n\nDo you wish to continue posting?\n\n" + string.Join("\n", list);
				}
			}
		}
		return string.Empty;
	}

	private static IList<string> checkPartForFutureDatesTransactions(M1BindingSource bindingsource, IDictionary<PartInformation, decimal> dicPartInfo)
	{
		IList<string> list = new List<string>();
		foreach (KeyValuePair<PartInformation, decimal> item in dicPartInfo)
		{
			DateTime? tranDate = bindingsource.CurrentAsDataRow.Field<DateTime?>("smpShipDate");
			SqlCommand sqlCommand = new SqlCommand("Select impNonStockedItem from Parts where impPartID = @partID");
			sqlCommand.Parameters.Add(new SqlParameter("@partID", item.Key.Part));
			object obj = bindingsource.Database.ExecuteScalar(sqlCommand, bindingsource.Transaction);
			bool value = obj == null || Convert.ToBoolean(obj);
			if (obj != null && !Convert.ToBoolean(value) && new Part().GetFutureAdjustmentTransactionStatus(bindingsource.Database, bindingsource.Transaction, item.Key.Part, item.Key.PartRevision, item.Key.PartWarehouse, item.Key.PartBin, tranDate))
			{
				list.Add("Part '" + item.Key.Part + "', Rev '" + item.Key.PartRevision + "', Warehouse '" + item.Key.PartWarehouse + "', Bin '" + item.Key.PartBin + "'");
			}
		}
		return list;
	}

	public string CheckShipmentForZeroDollarTotals(M1BindingSource bindingSource)
	{
		if (bindingSource.CurrentAsDataRow == null)
		{
			return string.Empty;
		}
		if (bindingSource.CurrentAsDataRow.Field<bool>("smpReversalEntry"))
		{
			return string.Empty;
		}
		DataTable dataTable = bindingSource.PrimaryTable.GetChildBindingSource("ShipmentLines").GetDataTable();
		if (dataTable == null || dataTable.Rows.Count == 0)
		{
			return string.Empty;
		}
		if (dataTable.Rows.Cast<DataRow>().Any((DataRow shipmentLineRow) => shipmentLineRow.Field<decimal>("smlExtendedPriceForeign").Equals(0m)))
		{
			return "There are shipment lines that have zero dollar total amounts. If you continue, this will result in a zero dollar invoice line in your financial package.\n\nDo you wish to continue posting?";
		}
		return string.Empty;
	}

	public bool ShipmentLinesHasInactiveBinsGoingNegative(M1Database database, string shipmentId)
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM\r\n                (SELECT sl.smlPartID AS partId, sl.smlPartRevisionID AS partRevisionId, sl.smlPartWarehouseLocationID AS parWarehouseLocationId, sl.smlPartBinID AS partBinId, sl.smlQuantityShipped AS qtyToShip\r\n                FROM ShipmentLines sl \r\n                WHERE sl.smlShipmentID = @ShipmentId AND sl.smlKitPart = 0\r\n                UNION ALL\r\n                SELECT sc.smoPartID AS partId, sc.smoPartRevisionID AS partRevisionId, sc.smoPartWarehouseLocationID AS parWarehouseLocationId, sc.smoPartBinID AS partBinId, sc.smoQuantityShipped AS qtyToShip\r\n                FROM ShipmentComponents sc  \r\n                WHERE sc.smoShipmentID = @ShipmentId)\r\n                AS shipmentLines\r\n                INNER JOIN PartBins pb ON pb.imbPartID = partId AND pb.imbPartRevisionID = partRevisionId AND pb.imbWarehouseID = parWarehouseLocationId AND pb.imbPartBinID = partBinId\r\n                WHERE pb.imbInactiveBin = 1 AND qtyToShip > pb.imbQuantityOnHand");
		sqlCommand.Parameters.Add(new SqlParameter("@ShipmentId", shipmentId));
		return (int)database.ExecuteScalar(sqlCommand) > 0;
	}
}
