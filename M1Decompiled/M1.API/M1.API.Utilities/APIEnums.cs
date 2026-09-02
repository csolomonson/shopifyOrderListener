namespace M1.API.Utilities;

public static class APIEnums
{
	public enum WebAPIModules : byte
	{
		EO,
		EDI,
		SFE,
		BOM,
		ERP
	}

	public enum APISalesOrderProcessingStatus : byte
	{
		Created,
		Processed,
		Failed,
		Validated
	}

	public enum SalesOrderStatuses : byte
	{
		NONE,
		REQUIREAPPROVAL,
		APPROVALREQUESTED,
		APPROVED,
		REJECTED
	}

	public enum XML810SACIndicatorTypes
	{
		C,
		A
	}

	public enum PartTypes : byte
	{
		Purchased = 1,
		Manufactured
	}

	public enum OperationType : byte
	{
		Inside = 1,
		Outside
	}

	public enum MachineType : byte
	{
		FirstAvailable = 1,
		AllMachines,
		SelectedMachine
	}
}
