using System;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferJobOperationToPurchaseOrderProcessAll : TransferJobOperationToPurchaseOrderProcess
{
	public TransferJobOperationToPurchaseOrderProcessAll(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}

	protected override void OnLoad()
	{
		KeyValueFieldNames = new string[3] { "jmoJobID", "jmoJobAssemblyID", "jmoJobOperationID" };
		KeyValueTableName = "JobOperations";
		Description = "Select the outside job operations to be purchased.";
		CreatedBindingSourceCaption = "Create Purchase Orders from Job Operations";
		GridID = "M1ADDFROMPOJOBOPR";
		BindingSourceTable = "PurchaseOrders";
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmoClosed", fieldValue: false, "Job is closed."));
		ContinueMessage = "This will create purchase orders from the {0} selected job operations. Are you sure you want to continue?";
		MultipleDestinationRowsCreated = true;
		AdditionalFilterParameters.Add(new AdditionalFilterParameterDateRange("Due Dates")
		{
			IgnoreWhenEmpty = true,
			ValueField = "jmoDueDate",
			AdditionalFields = "jmoDueDate"
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterMultiValue("Invoice Locations", null, new string[2] { "jmoSupplierOrganizationID", "jmoPurchaseLocationID" })
		{
			ValueFields = new string[2] { "jmoSupplierOrganizationID", "jmoPurchaseLocationID" }
		});
		AdditionalFilterParameters.Add(new AdditionalFilterParameterBool("Add Firm Operations Only?")
		{
			AdoFilterExpression = "jmoFirm <> 0",
			IgnoreWhenEmpty = true,
			AdditionalFields = "jmoFirm"
		});
		HeaderSourceFields = new string[4] { "jmpPlantID", "jmpProjectID", "jmoSupplierOrganizationID", "jmoPurchaseLocationID" };
		HeaderDestinationFields = new string[4] { "pmpPlantID", "pmpProjectID", "pmpSupplierOrganizationID", "pmpPurchaseLocationID" };
	}
}
