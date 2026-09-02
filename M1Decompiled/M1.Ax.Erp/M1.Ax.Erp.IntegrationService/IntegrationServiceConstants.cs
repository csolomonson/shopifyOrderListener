namespace M1.Ax.Erp.IntegrationService;

public class IntegrationServiceConstants
{
	public enum IntegrationType
	{
		Financial,
		ShopFloor
	}

	public enum ApiAction
	{
		Update,
		Create,
		Retrieve,
		Sync,
		Delete
	}

	public enum EntityType
	{
		Invoice,
		CreditMemo,
		GLAccount,
		PaymentTerm,
		Bill,
		VendorCredit,
		TaxCode,
		TimeActivity,
		Customer,
		Vendor,
		Item,
		Employee,
		Shift,
		Timecard,
		TimecardLine,
		ProductionDepartment,
		WorkCenter,
		IndirectLaborCode,
		ApplicationProperty,
		Part,
		PartRevision,
		Process,
		Job,
		JobAssembly,
		JobOperation,
		JobMaterial,
		JobMaterialComponent,
		PartBin,
		PartWarehouseLocation,
		Reason,
		SerialNumber,
		SerialNumberStatus,
		LotNumber,
		LotNumberStatus,
		Warehouse
	}

	public enum Status
	{
		Pending,
		Success,
		Failed
	}

	public const string FinancialIntegrationModuleRole = "FINANCIALINT";

	public const int FinancialIntegrationCustomModuleId = 13;

	public const string ShopFloorModuleRole = "SHOPFLOOR";

	public const int ShopFloorCustomModuleId = 14;
}
