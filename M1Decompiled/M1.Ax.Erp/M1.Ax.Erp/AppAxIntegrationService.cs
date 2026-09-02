using System;
using System.Data;
using System.Runtime.InteropServices;
using M1.Ax.Erp.IntegrationService;
using M1.Core;
using M1.Core.Script;

namespace M1.Ax.Erp;

[AxScript("IntegrationService")]
[ComVisible(true)]
public class AppAxIntegrationService
{
	private readonly IServiceProvider _provider;

	private readonly M1Database _database;

	public AppAxIntegrationService(IServiceProvider parentProvider)
	{
		_provider = parentProvider;
		_database = parentProvider.GetService(typeof(M1Database)) as M1Database;
	}

	public void ResetStatusInGrid(M1BindingSource bindingSource)
	{
		if (bindingSource?.CurrentAsDataRow != null && Enum.TryParse<IntegrationServiceConstants.IntegrationType>(bindingSource.CurrentAsDataRow.Field<string>("itqIntegrationType"), out var result))
		{
			int integrationCustomModuleId = (result.Equals(IntegrationServiceConstants.IntegrationType.Financial) ? 13 : (result.Equals(IntegrationServiceConstants.IntegrationType.ShopFloor) ? 14 : 0));
			string integrationModuleRole = (result.Equals(IntegrationServiceConstants.IntegrationType.Financial) ? "FINANCIALINT" : (result.Equals(IntegrationServiceConstants.IntegrationType.ShopFloor) ? "SHOPFLOOR" : string.Empty));
			new M1.Ax.Erp.IntegrationService.IntegrationService().ResetStatusInGrid(bindingSource, result, integrationCustomModuleId, integrationModuleRole);
		}
	}

	public bool IsIntegrationTypeInactive(string integrationType)
	{
		if (!(_database.GetService(typeof(M1DataDictionary)) is M1DataDictionary dataDictionary))
		{
			return false;
		}
		if (!Enum.TryParse<IntegrationServiceConstants.IntegrationType>(integrationType, out var result))
		{
			return true;
		}
		return new M1.Ax.Erp.IntegrationService.IntegrationService().IsIntegrationTypeInactive(dataDictionary, result, _database.ID);
	}
}
