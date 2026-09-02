using System;
using M1.Core;

namespace M1.Ax.Erp;

public class TransferJobToShipmentProcess : TransferSalesOrderToShipmentProcess
{
	public TransferJobToShipmentProcess(IServiceProvider provider)
		: base(provider)
	{
	}

	protected override void OnLoad()
	{
		base.OnLoad();
		GridID = "M1ADDFROMSHIPMENTJOB";
		Description = "Select the job deliveries to be shipped.";
		PromptFieldAllowMultiples = true;
		PromptFieldNames = new string[1] { "jmpJobID" };
		PromptFieldValidations.Clear();
		PromptFieldValidations.Add(new PromptFieldValidationBool("jmpClosed", fieldValue: false, "Job is closed."));
		HelpLink = "SM_TransferJobToShipment.htm";
		CreatedBindingSourceCaption = "Create Shipment from Job";
	}
}
