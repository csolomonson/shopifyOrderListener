using M1.Core;

namespace M1.Ax.Erp.DD;

[DBConversion("9.00.036", "Rename mobile fields", "2015-05-19")]
public class v900036
{
	public v900036(DBConversionParms parms)
	{
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationContacts", "cmcCreateFromMobile"))
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationContacts", "cmcCreatedFromMobile"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContacts", "cmcCreateFromMobile", "cmcCreatedFromMobile", dropTriggers: true);
			}
			else
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationContacts", "cmcCreateFromMobile", dropTriggers: true);
			}
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Followups", "cmfCreateFromMobile"))
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Followups", "cmfCreatedFromMobile"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Followups", "cmfCreateFromMobile", "cmfCreatedFromMobile", dropTriggers: true);
			}
			else
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Followups", "cmfCreateFromMobile", dropTriggers: true);
			}
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlCreateFromMobile"))
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "OrganizationLocations", "cmlCreatedFromMobile"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlCreateFromMobile", "cmlCreatedFromMobile", dropTriggers: true);
			}
			else
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "OrganizationLocations", "cmlCreateFromMobile", dropTriggers: true);
			}
		}
		if (parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoCreateFromMobile"))
		{
			if (!parms.Dmo.DoesFieldExist(null, parms.User, parms.DatabaseName, "Organizations", "cmoCreatedFromMobile"))
			{
				parms.Dmo.RenameColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoCreateFromMobile", "cmoCreatedFromMobile", dropTriggers: true);
			}
			else
			{
				parms.Dmo.DropColumn(null, parms.User, parms.DataDictionary, parms.DatabaseName, "Organizations", "cmoCreateFromMobile", dropTriggers: true);
			}
		}
	}
}
