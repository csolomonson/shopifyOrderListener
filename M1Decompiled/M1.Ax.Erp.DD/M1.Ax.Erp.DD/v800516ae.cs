using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("8.00.516", "Add fields to SHIPMENTS table", "2015-05-19")]
public class v800516ae
{
	public v800516ae(DBConversionParms parms)
	{
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListCarrierFreightBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListCarrierFreightBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListCarrierFreightForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListCarrierFreightForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListBaseChargeForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListBaseChargeForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListSurchargeForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListSurchargeForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccBaseChargeBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccBaseChargeBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccBaseChargeForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccBaseChargeForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccSurchargeBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccSurchargeBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccSurchargeForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccSurchargeForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListDiscountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListDiscountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListDiscountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListDiscountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccDiscountBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccDiscountBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccDiscountForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccDiscountForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccCarrierFreightBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccCarrierFreightBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAccCarrierFreightForeign"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAccCarrierFreightForeign", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpReturnInstructionsRTF"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpReturnInstructionsRTF", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpReturnInstructionsText"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpReturnInstructionsText", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpUPS3rdPartyOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpUPS3rdPartyOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpUPS3rdPartyLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpUPS3rdPartyLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpShipmentIDNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpShipmentIDNumber", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpCarrierDocumentFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpCarrierDocumentFilePath", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpExportingCarrier"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpExportingCarrier", "nvarchar", 35, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpDocuments"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpDocuments", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpAESITN"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpAESITN", "nvarchar", 30, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpReasonForExport"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpReasonForExport", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListBaseChargeBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListBaseChargeBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpListSurchargeBase"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpListSurchargeBase", "money", 12, 2, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpCODLabelFilePath"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpCODLabelFilePath", "nvarchar(max)", 50, 0, verifyIndexes: true, dropTriggers: true, isNullable: true, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpUPSBillingOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpUPSBillingOption", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpUPSAccountNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpUPSAccountNumber", "nvarchar", 6, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpFedExAccountNumber"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpFedExAccountNumber", "nvarchar", 15, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpFedExBillingOption"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpFedExBillingOption", "nvarchar", 20, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpBlindShipOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpBlindShipOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpBlindShipLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpBlindShipLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpBlindShipContactID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpBlindShipContactID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpFedEx3rdPartyOrganizationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpFedEx3rdPartyOrganizationID", "nvarchar", 10, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
		if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "SHIPMENTS", "smpFedEx3rdPartyLocationID"))
		{
			parms.Dmo.AddColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "SHIPMENTS", "smpFedEx3rdPartyLocationID", "nvarchar", 5, 0, verifyIndexes: true, dropTriggers: true, isNullable: false, parms.Messages);
		}
	}
}
