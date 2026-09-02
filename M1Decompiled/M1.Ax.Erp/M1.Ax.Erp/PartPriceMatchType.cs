namespace M1.Ax.Erp;

public enum PartPriceMatchType : byte
{
	PartAndCustomer = 1,
	PartAndCustomerGroup,
	PartGroupAndCustomer,
	PartGroupAndCustomerGroup,
	PartAndEmptyCustomer,
	PartGroupAndEmptyCustomer,
	CustomerAndEmptyPart,
	CustomerGroupAndEmptyPart
}
