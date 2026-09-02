using System.Data;
using System.Data.SqlClient;
using System.Linq;
using M1.Extensions;

namespace M1.Core;

public class LoginCredentials
{
	public string EmployeeID = string.Empty;

	public string OrganizationID = string.Empty;

	public string LocationID = string.Empty;

	public string ContactID = string.Empty;

	public string EmailAddress = string.Empty;

	public string UserID = string.Empty;

	public string Password;

	public DataTable PossibleUsersForEmailTable;

	public int PossibleUsersCount;

	public LoginCredentials()
	{
	}

	public LoginCredentials(string userID, string userPassword)
	{
		UserID = userID;
		Password = userPassword;
	}

	public bool IsMatchingCredentials(LoginCredentials credentialsToTest)
	{
		if (credentialsToTest.EmployeeID == EmployeeID && credentialsToTest.OrganizationID == OrganizationID && credentialsToTest.LocationID == LocationID && credentialsToTest.ContactID == ContactID && credentialsToTest.EmailAddress == EmailAddress)
		{
			return true;
		}
		return false;
	}

	public bool LoadData(DataRow row)
	{
		OrganizationID = row.Field<string>("OrganizationID").Trim();
		LocationID = row.Field<string>("LocationID").Trim();
		ContactID = row.Field<string>("ContactID").Trim();
		EmployeeID = row.Field<string>("EmployeeID").Trim();
		EmailAddress = row.Field<string>("EmailAddress").Trim();
		return true;
	}

	public void DisambiguateUserFromEmailAddress(AppContext context, string databaseToCheck, string dataDictionaryToCheck)
	{
		if (UserID.Contains('@') && databaseToCheck.Length != 0 && dataDictionaryToCheck.Length != 0)
		{
			PossibleUsersForEmailTable = context.DBServerManager.GetDataTable(null, null, databaseToCheck, 0, "Select 'Contact' as UserType,cmcOrganizationID As OrganizationID, cmcLocationID As LocationID, cmcContactID As ContactID,'' As EmployeeID, cmlName As CompanyName,cmcName As Name, cmcWebTemplate As WebTemplate, cmcWebPassword As WebPassword, " + UserID.ToSql() + " As EmailAddress From OrganizationContacts Inner Join OrganizationLocations On cmcOrganizationID = cmlOrganizationID And cmcLocationID = cmlLocationID Where cmcWebLoginEnabled <> 0 And (cmcWebExpirationDate Is Null Or cmcWebExpirationDate > GetDate()) And (cmcInactiveDate Is Null Or cmcInactiveDate > GetDate()) And Convert(nvarchar(50),cmcEmailAddress) = " + UserID.ToSql() + " Union Select 'Employee' as UserType,'' As OrganizationID, '' As LocationID, '' As ContactID, lmeEmployeeID As EmployeeID,xadName As CompanyName,lmeEmployeeName As Name, Case When lmeWebTemplateUseM1UserID <> 0 Then lmeUserID Else lmeWebTemplate End As WebTemplate, lmeWebPassword As WebPassword, " + UserID.ToSql() + " As EmailAddress From Employees, DatasetProperties Where lmeWebLoginEnabled <> 0 And (lmeWebExpirationDate Is Null Or lmeWebExpirationDate > GetDate()) And (lmeTerminationDate Is Null Or lmeTerminationDate > GetDate()) And Convert(nvarchar(50),lmeWorkEmailAddress) = " + UserID.ToSql() + " Order By UserType desc, OrganizationID,LocationID,ContactID,EmployeeID");
			PossibleUsersCount = PossibleUsersForEmailTable.Rows.Count;
			if (PossibleUsersCount != 0)
			{
				SetUserFromEmailDataRow(PossibleUsersForEmailTable.Rows[0], context, dataDictionaryToCheck);
			}
		}
	}

	public void SetUserFromEmailDataRow(DataRow row, AppContext context, string dataDictionaryToCheck)
	{
		if (row != null)
		{
			if (!M1Util.HashString(row.Field<string>("WebPassword").Trim()).Equals(Password))
			{
				throw new M1LoginInvalidUserIDOrPasswordException();
			}
			UserID = row.Field<string>("WebTemplate").Trim();
			SqlCommand sqlCommand = context.DDServerManager.NewSqlCommand(null, null, dataDictionaryToCheck, "Select duPassword From DDUsers Where duUserID = @User");
			sqlCommand.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar)).Value = UserID;
			SqlDataAdapter adapter;
			using (DataTable dataTable = context.DDServerManager.GetDataTable(null, null, dataDictionaryToCheck, 0, sqlCommand, fillSchema: false, out adapter))
			{
				if (dataTable.Rows.Count == 0)
				{
					throw new M1LoginException("M1 is unable to find the " + UserID + " web template.");
				}
				Password = M1Util.HashString(context.DBServerManager.Decrypt(dataTable.Rows[0].Field<string>("duPassword")));
			}
			if (row != null)
			{
				LoadData(row);
			}
		}
		if (row == null)
		{
			UserID = string.Empty;
			Password = null;
		}
	}
}
