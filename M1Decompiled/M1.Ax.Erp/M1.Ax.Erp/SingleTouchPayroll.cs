using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;
using M1.Core;
using M1.Extensions;

namespace M1.Ax.Erp;

public class SingleTouchPayroll
{
	private static bool IsUpdateAction(DataRow stpDataRow)
	{
		decimal num = stpDataRow.Field<decimal>("stpPayerTotalGrossPay");
		decimal num2 = stpDataRow.Field<decimal>("stpPayerTotalPAYGW");
		if (num == 0m)
		{
			return num2 == 0m;
		}
		return false;
	}

	private static StpMetadata StpGetMetaData(DataRow stpDataRow, bool isUpdateAction)
	{
		string text = PayrollHelpers.RemovePunctuation(string.IsNullOrEmpty(stpDataRow.Field<string>("stpABN")) ? "" : stpDataRow.Field<string>("stpABN").Trim());
		string conversationId = Guid.NewGuid().ToString("N");
		string action = (isUpdateAction ? "Update.004.00" : "Submit.004.00");
		return new StpMetadata
		{
			From = text,
			Role = "Business",
			ConversationId = conversationId,
			Action = action
		};
	}

	private static StpMetadata StpWriteMetaData(ref List<string> metaData, DataRow stpDataRow, bool isUpdateAction)
	{
		StpMetadata stpMetadata = StpGetMetaData(stpDataRow, isUpdateAction);
		metaData.Add("from");
		metaData.Add(stpMetadata.From);
		metaData.Add("role");
		metaData.Add(stpMetadata.Role);
		metaData.Add("conversationid");
		metaData.Add(stpMetadata.ConversationId.Replace("_", "-"));
		metaData.Add("action");
		metaData.Add(stpMetadata.Action);
		return stpMetadata;
	}

	private static void StpWriteHeaderLabels(ref List<string> headerRowLabels)
	{
		headerRowLabels.Add("Line_ID");
		headerRowLabels.Add("BMS Identifier");
		headerRowLabels.Add("Payer ABN");
		headerRowLabels.Add("Payer WPN");
		headerRowLabels.Add("Payer Branch Code");
		headerRowLabels.Add("Previous BMS Identifier");
		headerRowLabels.Add("Payer Organisation Name");
		headerRowLabels.Add("Payer Contact Name");
		headerRowLabels.Add("Payer Email Address");
		headerRowLabels.Add("Payer Business Hours Phone Number");
		headerRowLabels.Add("Payer Postcode");
		headerRowLabels.Add("Payer Country Code");
		headerRowLabels.Add("Pay/Update Date");
		headerRowLabels.Add("Payee Record Count");
		headerRowLabels.Add("Run Date/Time Stamp");
		headerRowLabels.Add("Submission ID");
		headerRowLabels.Add("Full File Replacement Indicator");
		headerRowLabels.Add("Payer Total PAYGW Amount");
		headerRowLabels.Add("Payer Total Gross Amount");
		headerRowLabels.Add("Child Support Total Garnishee Amount");
		headerRowLabels.Add("Child Support Total Deduction Amount");
		headerRowLabels.Add("Payer Declarer Identifier");
		headerRowLabels.Add("Payer Declaration Date");
		headerRowLabels.Add("Payer Declaration Acceptance Indicator");
		headerRowLabels.Add("Intermediary ABN");
		headerRowLabels.Add("Registered Agent Number");
		headerRowLabels.Add("Intermediary Contact Name");
		headerRowLabels.Add("Intermediary Email Address");
		headerRowLabels.Add("Intermediary Business Hours Phone Number");
		headerRowLabels.Add("Intermediary Declarer Identifier");
		headerRowLabels.Add("Intermediary Declaration Date");
		headerRowLabels.Add("Intermediary Declaration Acceptance Indicator");
		headerRowLabels.Add("Payee TFN");
		headerRowLabels.Add("Contractor ABN");
		headerRowLabels.Add("Payee Payroll ID");
		headerRowLabels.Add("Previous Payroll ID");
		headerRowLabels.Add("Payee Family Name");
		headerRowLabels.Add("Payee First Name");
		headerRowLabels.Add("Payee Other Name");
		headerRowLabels.Add("Payee Day of Birth");
		headerRowLabels.Add("Payee Month of Birth");
		headerRowLabels.Add("Payee Year of Birth");
		headerRowLabels.Add("Payee Address Line 1");
		headerRowLabels.Add("Payee Address Line 2");
		headerRowLabels.Add("Payee Suburb/Town");
		headerRowLabels.Add("Payee State/Territory");
		headerRowLabels.Add("Payee Postcode");
		headerRowLabels.Add("Payee Country Code");
		headerRowLabels.Add("Payee E-mail Address");
		headerRowLabels.Add("Payee Phone Number");
		headerRowLabels.Add("Payee Commencement Date");
		headerRowLabels.Add("Payee Cessation Date");
		headerRowLabels.Add("Employement Basis Code");
		headerRowLabels.Add("Cessation Type Code");
		headerRowLabels.Add("Tax Treatment Code");
		headerRowLabels.Add("Tax Offset Amount");
		headerRowLabels.Add("Period Start Date");
		headerRowLabels.Add("Period End Date");
		headerRowLabels.Add("Final Event Indicator");
		headerRowLabels.Add("Income Stream Type Code");
		headerRowLabels.Add("Country Code");
		headerRowLabels.Add("PAYGW Amount");
		headerRowLabels.Add("Foreign Tax Paid Amount");
		headerRowLabels.Add("Exempt Foreign Income Amount");
		headerRowLabels.Add("Gross Amount");
		headerRowLabels.Add("Paid Leave Payment Code");
		headerRowLabels.Add("Paid Leave Payment Amount");
		headerRowLabels.Add("Allowance Type Code");
		headerRowLabels.Add("Other Allowance Type Description");
		headerRowLabels.Add("Payee Allowance Amount");
		headerRowLabels.Add("Overtime Amount");
		headerRowLabels.Add("Bonuses and Commissions Amount");
		headerRowLabels.Add("Director's Fees Amount");
		headerRowLabels.Add("CDEP Amount");
		headerRowLabels.Add("Salary Sacrifice Type Code");
		headerRowLabels.Add("Salary Sacrifice Amount");
		headerRowLabels.Add("Lump Sum Type Code");
		headerRowLabels.Add("Lump Sum Financial Year");
		headerRowLabels.Add("Lump Sum Payment Amount");
		headerRowLabels.Add("ETP Code");
		headerRowLabels.Add("Payee ETP Payment Date");
		headerRowLabels.Add("Payee Termination Payment Tax Free Component");
		headerRowLabels.Add("Payee Termination Payment Taxable Component");
		headerRowLabels.Add("Payee Total ETP PAYGW Amount");
		headerRowLabels.Add("Deduction Type");
		headerRowLabels.Add("Payee Deduction Amount");
		headerRowLabels.Add("Super Entitlement Type Code");
		headerRowLabels.Add("Super Entitlement Amount");
		headerRowLabels.Add("RFB Exemption Status Code");
		headerRowLabels.Add("Payee RFB Amount");
	}

	private void StpWriteUnusedAnnualOrLongServiceLeave(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("LumpSumTypeCode"));
		lineString.Add((!lineDataRow.Field<int>("TaxYear").Equals(0)) ? lineDataRow.Field<int>("TaxYear").ToString() : string.Empty);
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("LumpSumPaymentAmount")));
	}

	private void StpWriteAllowanceStreamCollection(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("stlOvertimeAmount")));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("stlBonusAmount")));
		lineString.Add(lineDataRow.Field<string>("stlHomeCountry").ToString().Equals(string.Empty) ? PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("stlDirectorsFees")) : string.Empty);
		lineString.Add("");
	}

	private StpDbOptions StpGetDbOptions(M1BindingSource bindingSource)
	{
		try
		{
			StpDbOptions stpDbOptions = new StpDbOptions();
			M1Database database = bindingSource.Database;
			M1.Core.AppContext appContext = database.GetService(typeof(M1.Core.AppContext)) as M1.Core.AppContext;
			stpDbOptions.InProduction = database.Props("FinancialProperties").Field<bool>("xafSTPProduction");
			stpDbOptions.SenderId = database.Props("FinancialProperties").Field<string>("xafSTPSenderID")?.Trim();
			if (appContext != null)
			{
				stpDbOptions.Password = (string.IsNullOrEmpty(database.Props("FinancialProperties").Field<string>("xafSTPPassword")) ? "" : appContext.DBServerManager.Decrypt(database.Props("FinancialProperties").Field<string>("xafSTPPassword").Trim()));
			}
			stpDbOptions.ProjectKey = database.Props("FinancialProperties").Field<string>("xafSTPProjectKey")?.Trim();
			stpDbOptions.DataStoreKey = database.Props("FinancialProperties").Field<string>("xafSTPDatastoreKey")?.Trim();
			string messageUrl = ((!database.Props("FinancialProperties").Field<bool>("xafSTPProduction")) ? database.Props("FinancialProperties").Field<string>("xafSTPTestMessageXchangeURI")?.Trim() : database.Props("FinancialProperties").Field<string>("xafSTPMessageXchangeURI")?.Trim());
			stpDbOptions.MessageUrl = messageUrl;
			if (!string.IsNullOrWhiteSpace(stpDbOptions.DataStoreKey) && !string.IsNullOrWhiteSpace(stpDbOptions.MessageUrl) && !string.IsNullOrWhiteSpace(stpDbOptions.Password) && !string.IsNullOrWhiteSpace(stpDbOptions.ProjectKey) && !string.IsNullOrWhiteSpace(stpDbOptions.SenderId))
			{
				stpDbOptions.IsPopulated = true;
			}
			return stpDbOptions;
		}
		catch (M1Exception ex)
		{
			throw new M1Exception(ex.Message);
		}
	}

	private static SessionTotals GetSessionTotals(int sessionID, DataTable dataTableLines, DataTable dataTableETP, DataTable dataTableAllowances)
	{
		SessionTotals sessionTotals = new SessionTotals();
		decimal num = dataTableETP.Select("sttSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("sttTerminationPmtTaxableComp"));
		decimal num2 = dataTableAllowances.Select("staSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("staPayeeAllowanceAmount"));
		decimal num3 = dataTableLines.Select("stlSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("stlGrossPayments") + x.Field<decimal>("stlWorkingHolidayGrossPay") + x.Field<decimal>("stlBonusAmount") + x.Field<decimal>("stlOvertimeAmount") + x.Field<decimal>("stlCashOutLeave") + x.Field<decimal>("stlUnusedLeave") + x.Field<decimal>("stlPaidParentalLeave") + x.Field<decimal>("stlWorkersComp") + x.Field<decimal>("stlAncillaryDefenceLeave") + x.Field<decimal>("stlOtherPaidLeave") + x.Field<decimal>("stlPayeeLumpSumPaymentA") + x.Field<decimal>("stlPayeeLumpSumPaymentB") + x.Field<decimal>("stlPayeeLumpSumPaymentE") + x.Field<decimal>("stlPayeeLumpSumPaymentW") + x.Field<decimal>("stlDirectorsFees")) + num + num2;
		decimal num4 = dataTableLines.Select("stlSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("stlSalarySacrificeSuper") + x.Field<decimal>("stlSalarySacrificeOther"));
		sessionTotals.TotalGrossPayments = num3 - num4;
		decimal num5 = dataTableETP.Select("sttSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("sttPayeeTotalETPPAYGWAmount"));
		sessionTotals.TotalPayGwAmount = dataTableLines.Select("stlSessionID = " + sessionID.ToLinq()).Sum((DataRow x) => x.Field<decimal>("stlWorkingHolidayPAYGWAmount") + x.Field<decimal>("stlTotalFEIPAYGWAmount") + x.Field<decimal>("stlTotalFEIJPDAPAYGWAmount") + x.Field<decimal>("stlTotalLabourHirePAYGWAmt") + x.Field<decimal>("stlTotalVolAgreementPAYGWAmt") + x.Field<decimal>("stlTotalOtherSpecifiedPAYGWAmt") + x.Field<decimal>("stlTotalINBPAYGWAmount")) + num5;
		return sessionTotals;
	}

	private static SessionTotals GetLastSessionData(M1Database database, DataRow row, SqlTransaction transaction)
	{
		int num = 0;
		SqlCommand sqlCommand = new SqlCommand("Select Top 1 IsNull(stpSessionID,0) as stpSessionID From STPSessions where stpTaxYear = @TaxYear and stpSessionID <> @CurrentSessionID and (stpPayerTotalGrossPay <> 0 or stpPayerTotalPAYGW <> 0) order by stpSTPSubmittedDate desc");
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = row["stpTaxYear"];
		sqlCommand.Parameters.Add(new SqlParameter("@CurrentSessionID", SqlDbType.Int)).Value = row["stpSessionID"];
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		if (dataTable.Rows.Count != 0)
		{
			num = (int)dataTable.Rows[0]["stpSessionID"];
		}
		SessionTotals result = new SessionTotals();
		if (num != 0)
		{
			SqlCommand sqlCommand2 = new SqlCommand("Select * From STPLines where stlSessionID = @LastSessionID");
			sqlCommand2.Parameters.Add(new SqlParameter("@LastSessionID", SqlDbType.Int)).Value = num;
			DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
			SqlCommand sqlCommand3 = new SqlCommand("Select * From STPAllowances where staSessionID = @LastSessionID");
			sqlCommand3.Parameters.Add(new SqlParameter("@LastSessionID", SqlDbType.Int)).Value = num;
			DataTable dataTable3 = database.GetDataTable(sqlCommand3, transaction);
			SqlCommand sqlCommand4 = new SqlCommand("Select * From STPTerminationPayment where sttSessionID = @LastSessionID");
			sqlCommand4.Parameters.Add(new SqlParameter("@LastSessionID", SqlDbType.Int)).Value = num;
			DataTable dataTable4 = database.GetDataTable(sqlCommand4, transaction);
			result = GetSessionTotals(num, dataTable2, dataTable4, dataTable3);
		}
		return result;
	}

	private void StpWriteIncomeStreamTypes(ref List<string> lineString, DataRow lineDataRow)
	{
		string item = lineDataRow.Field<string>("IncomeStreamTypeCode").Trim();
		string item2 = lineDataRow.Field<string>("stlHomeCountry").Trim();
		lineString.Add(item);
		lineString.Add(item2);
		decimal amount = lineDataRow.Field<decimal>("PayGwAmount");
		lineString.Add(PayrollHelpers.FormatAmount(amount));
		lineString.Add("");
		lineString.Add("");
		decimal amount2 = lineDataRow.Field<decimal>("GrossAmount");
		lineString.Add(PayrollHelpers.FormatAmount(amount2));
	}

	private void StpWritePaidLeaves(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("PaidLeavePaymentCode").Trim());
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("PaidLeavePaymentAmount")));
	}

	private void StpWriteSacrifices(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("SalarySacrificeTypeCode"));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("SalarySacrificeAmount")));
	}

	private void StpWriteAllowances(ref List<string> lineString, DataRow lineDataRow)
	{
		string item = (string.IsNullOrEmpty(lineDataRow.Field<string>("staAllowanceType").Trim()) ? string.Empty : lineDataRow.Field<string>("staAllowanceType").Trim());
		lineString.Add(item);
		lineString.Add(lineDataRow.Field<string>("staAllowanceType").Trim().Equals("OD", StringComparison.CurrentCultureIgnoreCase) ? lineDataRow.Field<string>("staOtherAllowanceType").Trim() : string.Empty);
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("staPayeeAllowanceAmount")));
	}

	private void StpWriteDeductions(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("stdDeductionType").Trim());
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("stdPayeeDeductionAmount")));
	}

	private void StpWriteHeaderLine(ref List<string> headerLineString, DataRow stpDataRow, int recordCount, decimal totalPayerDeductionChildSupportGarnishee, decimal totalPayerDeductionChildSupportDeductions)
	{
		string empty = string.Empty;
		int num = 0;
		headerLineString.Add("1");
		headerLineString.Add(PayrollHelpers.AddDoubleQuotesToString(stpDataRow.Field<string>("stpBMSIdentifier").Trim()));
		headerLineString.Add(PayrollHelpers.RemovePunctuation(stpDataRow.Field<string>("stpABN").Trim()));
		headerLineString.Add("");
		headerLineString.Add((!string.IsNullOrWhiteSpace(stpDataRow.Field<string>("stpPayerBranchCode").Trim())) ? stpDataRow.Field<string>("stpPayerBranchCode").Trim() : "1");
		headerLineString.Add("");
		headerLineString.Add(PayrollHelpers.AddDoubleQuotesToString(stpDataRow.Field<string>("stpPayerOrganisationName").Trim()));
		headerLineString.Add(PayrollHelpers.AddDoubleQuotesToString(stpDataRow.Field<string>("stpContactName").Trim()));
		headerLineString.Add(stpDataRow.Field<string>("stpEmailAddress").Trim());
		empty = PayrollHelpers.RemovePunctuation(stpDataRow.Field<string>("stpPhoneNumber").Trim());
		headerLineString.Add(empty);
		headerLineString.Add(stpDataRow.Field<string>("stpPostCode").Trim());
		headerLineString.Add((!stpDataRow.Field<string>("stpCountryCode").Equals("AU", StringComparison.CurrentCultureIgnoreCase)) ? stpDataRow.Field<string>("stpCountryCode").ToLower() : "");
		headerLineString.Add(stpDataRow.Field<DateTime>("stpPayUpdateDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo));
		headerLineString.Add(recordCount.ToString());
		string uTCISO8601FormatString = PayrollHelpers.GetUTCISO8601FormatString(stpDataRow.Field<DateTime>("stpRunDateTimeStamp"));
		headerLineString.Add(uTCISO8601FormatString);
		headerLineString.Add(stpDataRow.Field<int>("stpSessionID").ToString());
		headerLineString.Add(stpDataRow.Field<bool>("stpFullFileReplacement") ? "Y" : "N");
		if (!stpDataRow.Field<decimal>("stpPayerTotalGrossPay").Equals(0m) || !stpDataRow.Field<decimal>("stpPayerTotalPAYGW").Equals(0m))
		{
			headerLineString.Add(PayrollHelpers.FormatAmount(stpDataRow.Field<decimal>("stpPayerTotalPAYGW")));
			headerLineString.Add(PayrollHelpers.FormatAmount(stpDataRow.Field<decimal>("stpPayerTotalGrossPay")));
		}
		else
		{
			headerLineString.Add(PayrollHelpers.FormatAmount(stpDataRow.Field<decimal>("stpPayerTotalPAYGW"), suppressWhenZero: true));
			headerLineString.Add(PayrollHelpers.FormatAmount(stpDataRow.Field<decimal>("stpPayerTotalGrossPay"), suppressWhenZero: true));
		}
		headerLineString.Add(PayrollHelpers.FormatAmount(totalPayerDeductionChildSupportGarnishee));
		headerLineString.Add(PayrollHelpers.FormatAmount(totalPayerDeductionChildSupportDeductions));
		headerLineString.Add(stpDataRow.Field<string>("stpPayerDeclarerIdentifier").Trim());
		headerLineString.Add(stpDataRow.Field<DateTime>("stpDeclarationDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo));
		headerLineString.Add(stpDataRow.Field<bool>("stpPayerDeclaration") ? "Y" : "N");
		num = 8;
		for (int i = 1; i <= num; i++)
		{
			headerLineString.Add("");
		}
	}

	public bool StpEmployeeHasSubmittedSession(M1Database database, string employeeId)
	{
		using SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(stlSessionID) FROM STPLines WHERE (stlSTPSubmitted = 1 OR stlSTPFFRSubmitted = 1) AND stlEmployeeID = @EmployeeId");
		sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand)) != 0m;
	}

	public bool StpEmployeeHasNotAcceptedSession(M1Database database, string employeeId, bool evaluateSTPSubmissionId = false)
	{
		string text = (evaluateSTPSubmissionId ? "AND stpSTPSubmissionID <> ''" : string.Empty);
		using SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(stpSessionID) FROM STPSessions INNER JOIN STPLines ON stlSessionID = stpSessionID WHERE stpSTPSubmitted = 0 AND stpSTPFFRSubmitted = 0 AND stlEmployeeID = @EmployeeId " + text);
		sqlCommand.Parameters.AddWithValue("@EmployeeId", employeeId);
		return Convert.ToDecimal(database.ExecuteScalar(sqlCommand)) != 0m;
	}

	public bool StpIsUpdateAction(M1BindingSource bindingSource)
	{
		return IsUpdateAction(bindingSource.CurrentAsDataRow);
	}

	public void StpChangeEmployeesDates(M1BindingSource bindingSource)
	{
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		M1Database database = bindingSource.Database;
		DateTime dateTime = currentAsDataRow.Field<DateTime>("stpPayUpdateDate");
		int num = currentAsDataRow.Field<int>("stpSessionID");
		SqlCommand sqlCommand = new SqlCommand("UPDATE STPLines set stlPeriodStartDate = @PayUpdateDate, stlPeriodEndDate = @PayUpdateDate where stlSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@PayUpdateDate", SqlDbType.DateTime)).Value = dateTime;
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = num;
		database.ExecuteCommand(sqlCommand);
	}

	public string StpCheckStatusProcess(M1BindingSource bindingSource)
	{
		string result = "";
		try
		{
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			if (!string.IsNullOrEmpty(currentAsDataRow.Field<string>("stpSTPSubmissionID")))
			{
				HttpResponseMessage result2 = StpGetMessage(bindingSource, currentAsDataRow).Result;
				string result3 = result2.Content.ReadAsStringAsync().Result;
				if (!result2.IsSuccessStatusCode)
				{
					return ((SendMessageResult)new XmlSerializer(typeof(SendMessageResult)).Deserialize(new StringReader(result3))).Description;
				}
				StpLog stpLog = (StpLog)new XmlSerializer(typeof(StpLog)).Deserialize(new StringReader(result3));
				string stpProcessStatus = stpLog.Record.StatusCode;
				if (stpProcessStatus.Equals("Error", StringComparison.CurrentCultureIgnoreCase) || stpProcessStatus.Equals("Partial", StringComparison.CurrentCultureIgnoreCase))
				{
					stpLog.Record.AtoResponse.Event.Select((Event x) => x.EventItems).FirstOrDefault().ToList()
						.ForEach(delegate(EventItem err)
						{
							stpProcessStatus = stpProcessStatus + " " + err.ShortDescription + ".";
						});
				}
				result = stpProcessStatus;
			}
		}
		catch (Exception ex)
		{
			result = ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
		}
		return result;
	}

	private async Task<HttpResponseMessage> StpGetMessage(M1BindingSource bindingSource, DataRow stpDataRow)
	{
		try
		{
			string value = stpDataRow.Field<string>("stpSTPSubmissionID");
			StpDbOptions stpDbOptions = StpGetDbOptions(bindingSource);
			if (!stpDbOptions.IsPopulated)
			{
				throw new M1Exception("E-STP Database options are not filled out correctly.");
			}
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
			nameValueCollection["P"] = stpDbOptions.ProjectKey.Trim();
			nameValueCollection["D"] = stpDbOptions.DataStoreKey.Trim();
			nameValueCollection["conversationid"] = value;
			string text = "DataStoreRead_Auth?" + nameValueCollection.ToString();
			string text2 = stpDbOptions.SenderId.Trim();
			string text3 = stpDbOptions.Password.Trim();
			string s = text2 + ":" + text3;
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			string text4 = "Basic " + Convert.ToBase64String(bytes);
			string uriString = stpDbOptions.MessageUrl.Trim();
			HttpClient val = new HttpClient
			{
				BaseAddress = new Uri(uriString)
			};
			val.DefaultRequestHeaders.Accept.Clear();
			((HttpHeaders)val.DefaultRequestHeaders).Add("Sender-ID", text2);
			((HttpHeaders)val.DefaultRequestHeaders).Add("Authorization", text4);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			return val.GetAsync(text).Result;
		}
		catch (Exception ex)
		{
			throw new M1Exception((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
		}
	}

	private void StpWriteSuperRFB(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("SuperEntitlementTypeCode"));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("SuperEntitlementAmount")));
	}

	public void StpClear(M1Database database, int sessionId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Delete From STPLines Where stlSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From STPAllowances Where staSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From STPDeductions Where stdSessionID = @SessionID");
		stringBuilder.AppendLine("Delete From STPTerminationPayment Where sttSessionID = @SessionID");
		string queryString = stringBuilder.ToString();
		SqlCommand sqlCommand = database.NewSqlCommand(queryString);
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionId;
		database.ExecuteCommand(sqlCommand);
		sqlCommand = database.NewSqlCommand("Update STPSessions Set stpPayerTotalPAYGW = 0, stpPayerTotalGrossPay = 0, stpRunDateTimeStamp = null, stpSTPCalculated = 0, stpPayerDeclaration = 0, stpDeclarationDate = null Where stpSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionId;
		database.ExecuteCommand(sqlCommand);
		sqlCommand = database.NewSqlCommand("Update PayrollSessions Set pasTransferredToSTP = 0, pasSTPSessionID = 0 Where pasTransferredToSTP <> 0 And pasSTPSessionID = @SessionID");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionId;
		database.ExecuteCommand(sqlCommand);
	}

	public bool IsDateBetweenStpSessionDates(int stpTaxYear, DateTime date)
	{
		DateTime minStpSessionDate = PayrollHelpers.GetMinStpSessionDate(stpTaxYear);
		DateTime maxStpSessionDate = PayrollHelpers.GetMaxStpSessionDate(stpTaxYear);
		if (date <= maxStpSessionDate)
		{
			return date >= minStpSessionDate;
		}
		return false;
	}

	private void StpWriteIdentifiers(ref List<string> lineString, DataRow lineDataRow, int identifierType, int lineCounter, string BMSIdentifier, string abn, string branchcode, string payerOrgName, ref bool payeefullInfo, bool isUpdateAction)
	{
		int num = 0;
		switch (identifierType)
		{
		case 1:
		{
			lineString.Add(lineCounter.ToString());
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(BMSIdentifier));
			lineString.Add(abn);
			lineString.Add("");
			lineString.Add(branchcode);
			lineString.Add("");
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(payerOrgName));
			num = 25;
			for (int j = 1; j <= num; j++)
			{
				lineString.Add("");
			}
			break;
		}
		case 2:
		{
			lineString.Add(PayrollHelpers.RemovePunctuation(lineDataRow.Field<string>("stlTaxFileNumber").Trim()));
			lineString.Add(PayrollHelpers.RemovePunctuation(lineDataRow.Field<string>("stlContractorABN").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlEmployeeID").Trim()));
			num = 24;
			for (int i = 1; i <= num; i++)
			{
				lineString.Add("");
			}
			break;
		}
		case 3:
		{
			lineString.Add(PayrollHelpers.RemovePunctuation(lineDataRow.Field<string>("stlTaxFileNumber").Trim()));
			lineString.Add(PayrollHelpers.RemovePunctuation(lineDataRow.Field<string>("stlContractorABN").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlEmployeeID").Trim()));
			string inputString = (isUpdateAction ? lineDataRow.Field<string>("stlPreviousEmployeeID").Trim() : string.Empty);
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(inputString));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlPayeeFamilyName").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlPayeeFirstName").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlPayeeOtherName").Trim()));
			if (lineDataRow.Field<DateTime?>("stlPayeeBirthdate").HasValue)
			{
				lineString.Add(lineDataRow.Field<DateTime>("stlPayeeBirthdate").Day.ToString());
				lineString.Add(lineDataRow.Field<DateTime>("stlPayeeBirthdate").Month.ToString());
				lineString.Add(lineDataRow.Field<DateTime>("stlPayeeBirthdate").Year.ToString());
			}
			else
			{
				lineString.Add("");
				lineString.Add("");
				lineString.Add("");
			}
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlAddressLine1").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlAddressLine2").Trim()));
			lineString.Add(PayrollHelpers.AddDoubleQuotesToString(lineDataRow.Field<string>("stlSuburb").Trim()));
			lineString.Add(lineDataRow.Field<string>("stlState").Trim().ToUpper());
			lineString.Add(lineDataRow.Field<string>("stlPostCode").Trim());
			lineString.Add((!lineDataRow.Field<string>("stlCountryCode").Equals("AU", StringComparison.CurrentCultureIgnoreCase)) ? lineDataRow.Field<string>("stlCountryCode").ToLower() : "");
			lineString.Add(lineDataRow.Field<string>("stlEmailAddress"));
			lineString.Add(PayrollHelpers.RemovePunctuation(lineDataRow.Field<string>("stlPhoneNumber").Trim()));
			lineString.Add(lineDataRow.Field<DateTime?>("stlCommencementDate").HasValue ? lineDataRow.Field<DateTime>("stlCommencementDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo) : "");
			lineString.Add(lineDataRow.Field<DateTime?>("stlCessationDate").HasValue ? lineDataRow.Field<DateTime>("stlCessationDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo) : "");
			lineString.Add(lineDataRow.Field<string>("stlEmployeeBasisCode").Trim());
			lineString.Add(lineDataRow.Field<string>("stlCessationType").Trim());
			lineString.Add(lineDataRow.Field<string>("stlTaxTreatmentCode"));
			lineString.Add("");
			lineString.Add(lineDataRow.Field<DateTime>("stlPeriodStartDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo));
			lineString.Add(lineDataRow.Field<DateTime>("stlPeriodEndDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo));
			lineString.Add((!lineDataRow.Field<bool>("stlFinalEventIndicator")) ? "N" : "Y");
			payeefullInfo = true;
			break;
		}
		}
	}

	public bool StpUpdateSessionExists(M1Database database, int sessionId, string action = "")
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT stpSessionID FROM STPSessions WHERE stpSessionID <> @SessionID AND stpSTPSubmitted = 1 AND stpSTPCalculated = 1 AND stpFullFileReplacement = 1 AND stpSTPFFRSubmitted = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionId;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count > 0)
		{
			int num = rows[0].Field<int>("stpSessionID");
			MessageBox.Show($"{action} STP may not be run until session {num} with full file replacement checked has been submitted and accepted", "STP Verification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return true;
		}
		return false;
	}

	public bool StpSessionExists(M1Database database, int sessionId, string action = "")
	{
		SqlCommand sqlCommand = new SqlCommand("SELECT stpSessionID FROM STPSessions WHERE stpSessionID <> @SessionID AND stpSTPSubmitted = 0");
		sqlCommand.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = sessionId;
		DataRowCollection rows = database.GetDataTable(sqlCommand).Rows;
		if (rows.Count > 0)
		{
			int num = rows[0].Field<int>("stpSessionID");
			MessageBox.Show($"{action} STP may not be run until session {num} has been submitted and accepted", "STP Verification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return true;
		}
		return false;
	}

	public bool StpProcessSession(M1Database database, M1BindingSource bsSTPSession)
	{
		DataRow currentAsDataRow = bsSTPSession.CurrentAsDataRow;
		M1BindingSource childBindingSource = bsSTPSession.PrimaryTable.GetChildBindingSource("STPLines");
		M1BindingSource childBindingSource2 = childBindingSource.PrimaryTable.GetChildBindingSource("STPTerminationPayment");
		M1BindingSource childBindingSource3 = childBindingSource.PrimaryTable.GetChildBindingSource("STPAllowances");
		M1BindingSource childBindingSource4 = childBindingSource.PrimaryTable.GetChildBindingSource("STPDeductions");
		SqlTransaction transaction = bsSTPSession.Transaction;
		int num = Convert.ToInt32(currentAsDataRow["stpTaxYear"]);
		int num2 = Convert.ToInt32(currentAsDataRow["stpSessionID"]);
		SqlCommand sqlCommand = new SqlCommand("SELECT patPayrollEmployeeID, ISNULL(SUM(panAmount),0) As panAmount, ISNULL(SUM(panAppliedPayAmount),0) As panAppliedPayAmount\r\n                                                                FROM PayrollSessions\r\n                                                                INNER JOIN PayrollHeaders on pasPayrollSessionID = patPayrollSessionID\r\n                                                                INNER JOIN PayrollLines on patPayrollSessionID = panPayrollSessionID AND patPayrollHeaderID = panPayrollHeaderID\r\n                                                                INNER JOIN Allowances on panAllowanceID = paoAllowanceID\r\n                                                                WHERE pasPostedToGL = 1\r\n                                                                AND pasTransferredToSTP = 0\r\n                                                                AND pasTaxYear = @TaxYear\r\n                                                                AND panPayrollLineType = 'A'\r\n                                                                AND paoIncludeInGrossPAYG = 1 \r\n                                                                GROUP BY patPayrollEmployeeID");
		sqlCommand.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable = database.GetDataTable(sqlCommand, transaction);
		SqlCommand sqlCommand2 = new SqlCommand("SELECT ph.patPayrollEmployeeID, ISNULL(SUM(CASE WHEN itt.pafTaxCategory = 'T' AND it.paxTaxAuthority = 1 THEN pl.panAmount ELSE 0 END), 0) AS AusTotalTaxWithheld\r\n                                                    FROM PayrollSessions ps\r\n                                                    INNER JOIN PayrollHeaders ph on ph.patPayrollSessionID = ps.pasPayrollSessionID\r\n                                                    INNER JOIN PayrollLines pl on pl.panPayrollSessionID = ph.patPayrollSessionID AND pl.panPayrollHeaderID = ph.patPayrollHeaderID\r\n                                                    INNER JOIN IncomeTaxes it on it.paxIncomeTaxID = pl.panIncomeTaxID\r\n                                                    INNER JOIN IncomeTaxTypes itt on itt.pafIncomeTaxID = it.paxIncomeTaxID AND itt.pafIncomeTaxTypeID = pl.panIncomeTaxTypeID\r\n                                                    WHERE ps.pasPostedToGL = 1\r\n                                                    AND ps.pasTransferredToSTP = 0\r\n                                                    AND ps.pasTaxYear = @TaxYear\r\n                                                    AND pl.panPayrollLineType = 'E' AND pl.panAusETPCode = ''\r\n                                                    GROUP BY ph.patPayrollEmployeeID");
		sqlCommand2.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable2 = database.GetDataTable(sqlCommand2, transaction);
		SqlCommand sqlCommand3 = new SqlCommand("SELECT patPayrollEmployeeID\r\n                                                            , SUM(IsNull(panAmount, 0)) AS SuperAmount\r\n                                                            , SUM(IsNull(panAUSReportableAmount, 0)) AS RESCAmount\r\n                                                        FROM PayrollSessions\r\n                                                        INNER JOIN PayrollHeaders ON pasPayrollSessionID = patPayrollSessionID\r\n                                                        INNER JOIN PayrollLines ON patPayrollSessionID = panPayrollSessionID AND patPayrollHeaderID = panPayrollHeaderID\r\n                                                        INNER JOIN Allowances ON panAllowanceID = paoAllowanceID\r\n                                                        LEFT OUTER JOIN EmployeeAllowances ON pawEmployeeID = patPayrollEmployeeID AND panEmployeeAllowanceID = pawEmployeeAllowanceID AND panAllowanceID = pawAllowanceID\r\n                                                        WHERE pasPostedToGL <> 0\r\n                                                        AND pasTaxYear = @TaxYear\r\n                                                        AND paoSuperannuation = 1\r\n                                                        AND paoPaidBy = 1\r\n                                                        AND IsNull(pawAllowanceTaxMethod, paoAllowanceTaxMethod) = 1\r\n                                                        GROUP BY patPayrollEmployeeID");
		sqlCommand3.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable3 = database.GetDataTable(sqlCommand3, transaction);
		SqlCommand sqlCommand4 = new SqlCommand("Select patPayrollEmployeeID, \r\n                                                SUM(ISNULL(panAmount,0)) As SSSuperAmount \r\n                                                FROM PayrollSessions \r\n                                                    INNER JOIN PayrollHeaders On pasPayrollSessionID = patPayrollSessionID \r\n                                                    INNER JOIN PayrollLines On patPayrollSessionID = panPayrollSessionID And patPayrollHeaderID = panPayrollHeaderID \r\n                                                    INNER JOIN Deductions On panDeductionID = padDeductionID \r\n                                                    LEFT OUTER JOIN EmployeeDeductions On paeEmployeeID = patPayrollEmployeeID And panEmployeeDeductionID = paeEmployeeDeductionID And panDeductionID = paeDeductionID \r\n                                                WHERE pasPostedToGL <> 0 \r\n                                                AND pasTaxYear = @TaxYear \r\n                                                AND IsNull(paeSuperannuation, padSuperannuation) <> 0 \r\n                                                AND panSalarySacrifice = 1 \r\n                                                AND IsNull(paeDeductionTaxMethod, padDeductionTaxMethod) = 1 \r\n                                                GROUP BY patPayrollEmployeeID");
		sqlCommand4.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable4 = database.GetDataTable(sqlCommand4, transaction);
		SqlCommand sqlCommand5 = new SqlCommand("SELECT patPayrollEmployeeID, \r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'A' AND pagAusLumpSumAType = 'R' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumAR,\r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'A' AND pagAusLumpSumAType = 'T' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumAT,\r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'B' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumB, \r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'D' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumD, \r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'E' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumE, \r\n                                            SUM(ISNULL((CASE WHEN pagAUSLumpSumType = 'W' THEN pagSubTotal ELSE 0 END), 0)) AS LumpSumW\r\n                                            FROM PayrollSessions \r\n                                                INNER JOIN PayrollHeaders ON pasPayrollSessionID = patPayrollSessionID \r\n                                                INNER JOIN PayrollHeaderTotals ON patPayrollSessionID = pagPayrollSessionID AND patPayrollHeaderID = pagPayrollHeaderID\r\n                                            WHERE pasPostedToGL <> 0 \r\n                                            AND pasTaxYear = @TaxYear \r\n                                            AND pagAusLumpSumType <> '' \r\n                                            AND pagAusIsETP <> 1\r\n                                            GROUP BY patPayrollEmployeeID");
		sqlCommand5.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable5 = database.GetDataTable(sqlCommand5, transaction);
		SqlCommand sqlCommand6 = new SqlCommand("SELECT patPayrollEmployeeID, \r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'C' THEN pagSubTotal ELSE 0 END), 0)) AS CashOutLeave,\r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'U' THEN pagSubTotal ELSE 0 END), 0)) AS UnusedLeave,\r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'P' THEN pagSubTotal ELSE 0 END), 0)) AS PaidParentalLeave,\r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'W' THEN pagSubTotal ELSE 0 END), 0)) AS WorkersComp,\r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'A' THEN pagSubTotal ELSE 0 END), 0)) AS AncillaryDefenceLeave,\r\n                                                SUM(ISNULL((CASE WHEN payLeaveType = 'O' THEN pagSubTotal ELSE 0 END), 0)) AS OtherPaidLeave\r\n                                                FROM PayrollSessions\r\n                                                    INNER JOIN PayrollHeaders ON pasPayrollSessionID = patPayrollSessionID\r\n                                                    INNER JOIN PayrollHeaderTotals ON patPayrollSessionID = pagPayrollSessionID AND patPayrollHeaderID = pagPayrollHeaderID\r\n                                                    INNER JOIN PayrollRates ON payPayrollRateID = pagPayrollRateID\r\n                                                WHERE pasPostedToGL<> 0\r\n                                                AND pasTaxYear = @TaxYear\r\n                                                AND payLeaveType<> ''\r\n                                                AND pagAUSLumpSumType = ''\r\n                                                AND pagAusIsETP <> 1\r\n                                                GROUP BY patPayrollEmployeeID");
		sqlCommand6.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable6 = database.GetDataTable(sqlCommand6, transaction);
		SqlCommand sqlCommand7 = new SqlCommand("SELECT ph.patPayrollEmployeeID,\r\n                                                    SUM(ISNULL((CASE WHEN pl.panSalarySacrifice = 1 AND ISNULL(ed.paeSuperannuation, d.padSuperannuation) = 1 THEN pl.panAmount ELSE 0 END), 0)) AS SalarySacrificeSuper,\r\n                                                    SUM(ISNULL((CASE WHEN pl.panSalarySacrifice = 1 AND ISNULL(ed.paeSuperannuation, d.padSuperannuation) = 0 THEN pl.panAmount ELSE 0 END), 0)) AS SalarySacrificeOther\r\n                                                    FROM PayrollSessions ps\r\n                                                        INNER JOIN PayrollHeaders ph on ph.patPayrollSessionID = ps.pasPayrollSessionID\r\n                                                        INNER JOIN PayrollLines pl on pl.panPayrollSessionID = ps.pasPayrollSessionID and pl.panPayrollHeaderID = ph.patPayrollHeaderID\r\n                                                        INNER JOIN Deductions d On pl.panDeductionID = d.padDeductionID\r\n                                                        LEFT OUTER Join EmployeeDeductions ed On ed.paeEmployeeID = ph.patPayrollEmployeeID And pl.panEmployeeDeductionID = ed.paeEmployeeDeductionID And pl.panDeductionID = ed.paeDeductionID\r\n                                                    WHERE ps.pasPostedToGL <> 0\r\n                                                    AND ps.pasTaxYear = @TaxYear\r\n                                                    AND ISNULL(ed.paeDeductionTaxMethod, d.padDeductionTaxMethod) = 1\r\n                                                    GROUP BY ph.patPayrollEmployeeID");
		sqlCommand7.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable7 = database.GetDataTable(sqlCommand7, transaction);
		SqlCommand sqlCommand8 = new SqlCommand("SELECT ph.patPayrollEmployeeID,\r\n                                                        SUM(ISNULL((CASE WHEN pr.payPayType = 'S' THEN pht.pagSubTotal ELSE CASE WHEN pr.payPayType = 'R' AND pr.payLeaveType = '' THEN pht.pagSubTotal ELSE 0 END END), 0)) AS AusGrossPayments\r\n                                                        FROM PayrollSessions ps\r\n                                                            INNER JOIN PayrollHeaders ph ON ph.patPayrollSessionID = ps.pasPayrollSessionID\r\n                                                            INNER JOIN PayrollHeaderTotals pht ON pht.pagPayrollSessionID = ph.patPayrollSessionID AND pht.pagPayrollHeaderID = ph.patPayrollHeaderID\r\n                                                            INNER JOIN PayrollRates pr ON pr.payPayrollRateID = pht.pagPayrollRateID\r\n                                                        WHERE ps.pasPostedToGL = 1 \r\n                                                        AND ps.pasTransferredToSTP = 0\r\n                                                        AND ps.pasTaxYear = @TaxYear\r\n                                                        AND pht.pagAusLumpSumType = ''\r\n                                                        AND pht.pagAusIsETP <> 1\r\n                                                        GROUP BY ph.patPayrollEmployeeID");
		sqlCommand8.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable8 = database.GetDataTable(sqlCommand8, transaction);
		SqlCommand sqlCommand9 = new SqlCommand("SELECT patPayrollEmployeeID,\r\n                                                        SUM(ISNULL((CASE WHEN payPayType = 'O' THEN pagSubTotal ELSE 0 END), 0)) AS OvertimeAmount,\r\n                                                        SUM(ISNULL((CASE WHEN payPayType = 'B' THEN pagSubTotal ELSE 0 END), 0)) AS BonusAmount\r\n                                                        FROM PayrollSessions\r\n                                                            INNER JOIN PayrollHeaders ON pasPayrollSessionID = patPayrollSessionID\r\n                                                            INNER JOIN PayrollHeaderTotals ON patPayrollSessionID = pagPayrollSessionID AND patPayrollHeaderID = pagPayrollHeaderID\r\n                                                            INNER JOIN PayrollRates ON payPayrollRateID = pagPayrollRateID\r\n                                                        WHERE pasPostedToGL<> 0\r\n                                                        AND pasTaxYear = @TaxYear\r\n                                                        AND payPayType<> ''\r\n                                                        AND pagAusLumpSumType = ''\r\n                                                        AND pagAusIsETP <> 1\r\n                                                        GROUP BY patPayrollEmployeeID");
		sqlCommand9.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable9 = database.GetDataTable(sqlCommand9, transaction);
		SqlCommand sqlCommand10 = new SqlCommand("SELECT patPayrollEmployeeID, panAusAllowanceType, panAusOtherAllowanceType, SUM(ISNULL(panAmount,0)) As panAmount\r\n                                                    FROM PayrollSessions \r\n\t                                                    INNER JOIN PayrollHeaders On pasPayrollSessionID = patPayrollSessionID \r\n\t                                                    INNER JOIN PayrollLines On patPayrollSessionID = panPayrollSessionID AND patPayrollHeaderID = panPayrollHeaderID \r\n\t                                                    INNER JOIN Allowances on panAllowanceID = paoAllowanceID\r\n                                                    WHERE pasPostedToGL <> 0 AND pasTaxYear = @TaxYear And panAusAllowanceType = 'OD' AND panPayrollLineType = 'A' AND paoIncludeInGrossPAYG = 0\r\n                                                    GROUP BY patPayrollEmployeeID, panAusAllowanceType, panAusOtherAllowanceType\r\n                                                    UNION\r\n                                                    SELECT patPayrollEmployeeID, panAusAllowanceType, '' AS panAusOtherAllowanceType, SUM(ISNULL(panAmount,0)) As panAmount\r\n                                                    FROM PayrollSessions \r\n\t                                                    INNER JOIN PayrollHeaders On pasPayrollSessionID = patPayrollSessionID \r\n\t                                                    INNER JOIN PayrollLines On patPayrollSessionID = panPayrollSessionID AND patPayrollHeaderID = panPayrollHeaderID \r\n\t                                                    INNER JOIN Allowances on panAllowanceID = paoAllowanceID\r\n                                                    WHERE pasPostedToGL <> 0 AND pasTaxYear = @TaxYear AND panAusAllowanceType <> 'OD' AND panAusAllowanceType <> '' AND panPayrollLineType = 'A' AND paoIncludeInGrossPAYG = 0\r\n                                                    GROUP BY patPayrollEmployeeID, panAusAllowanceType");
		sqlCommand10.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable10 = database.GetDataTable(sqlCommand10, transaction);
		SqlCommand sqlCommand11 = new SqlCommand("SELECT patPayrollEmployeeID, panAusDeductionType, SUM(ISNULL(panAmount,0)) As panAmount \r\n                                                FROM PayrollSessions \r\n                                                    INNER JOIN PayrollHeaders On pasPayrollSessionID = patPayrollSessionID \r\n                                                    INNER JOIN PayrollLines On patPayrollSessionID = panPayrollSessionID And patPayrollHeaderID = panPayrollHeaderID \r\n                                                WHERE pasPostedToGL <> 0 \r\n                                                AND pasTaxYear = @TaxYear \r\n                                                AND panAusDeductionType <> '' \r\n                                                AND panPayrollLineType = 'D' \r\n                                                GROUP BY patPayrollEmployeeID, panAusDeductionType");
		sqlCommand11.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable11 = database.GetDataTable(sqlCommand11, transaction);
		SqlCommand sqlCommand12 = new SqlCommand("SELECT patPayrollEmployeeID, \r\n                                        pasPayrollDate, \r\n                                        panAusETPCode, \r\n                                        SUM(ISNULL(panAmount,0)) As panAmount, \r\n                                        SUM(ISNULL(panAusETPTaxFreeComponent,0)) As panAusETPTaxFreeComponent, \r\n                                        SUM(ISNULL(panAusETPTaxableComponent,0)) As panAusETPTaxableComponent \r\n                                        FROM PayrollSessions\r\n                                            INNER JOIN PayrollHeaders on pasPayrollSessionID = patPayrollSessionID \r\n                                            INNER JOIN PayrollLines on patPayrollSessionID = panPayrollSessionID And patPayrollHeaderID = panPayrollHeaderID \r\n                                        WHERE pasPostedToGL <> 0 \r\n                                        AND pasTaxYear = @TaxYear \r\n                                        AND panPayrollLineType = 'E' \r\n                                        AND panAusETPCode <> '' \r\n                                        GROUP BY patPayrollEmployeeID, pasPayrollDate, panAusETPCode");
		sqlCommand12.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable12 = database.GetDataTable(sqlCommand12, transaction);
		SqlCommand sqlCommand13 = new SqlCommand("Select Distinct patPayrollEmployeeID, lmeCessationType, lmdHomeCountry, lmdWorkingHolidayMaker, lmdResidencyStatus, \r\n                                            lmdTaxFreeThresholdClaimed, lmdStdntFinSupplSchemeLoan, lmdStudyTrainLoanRepayment, lmdBasisOfPayment, lmeHireDate, \r\n                                            lmeTerminationDate, lmdBirthDate, \r\n                                            ISNULL((Select Top 1 SS.pasPayrollStartDate \r\n                                                FROM PayrollSessions SS \r\n                                                    INNER JOIN PayrollHeaders SH On SS.pasPayrollSessionID = SH.patPayrollSessionID Where SH.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID And SS.pasPostedToGL <> 0 And SS.pasTaxYear = PayrollSessions.pasTaxYear And SS.pasTransferredToSTP = 0 Order by SS.pasPayrollStartDate ASC),GetDate()) As StartDate, \r\n                                            ISNULL((Select Top 1 ES.pasPayrollEndDate \r\n                                                FROM PayrollSessions ES \r\n                                                    INNER JOIN PayrollHeaders EH On ES.pasPayrollSessionID = EH.patPayrollSessionID Where EH.patPayrollEmployeeID = PayrollHeaders.patPayrollEmployeeID And ES.pasPostedToGL <> 0 And ES.pasTaxYear = PayrollSessions.pasTaxYear And ES.pasTransferredToSTP = 0 Order by ES.pasPayrollEndDate DESC),GetDate()) As EndDate, \r\n                                            lmdEmploymentStatus, lmePreviousEmployeeID, lmdWorkingHolidayMaker \r\n                                            FROM PayrollSessions \r\n                                                INNER JOIN PayrollHeaders on pasPayrollSessionID = patPayrollSessionID \r\n                                                INNER JOIN EmployeePersonalData on patPayrollEmployeeID = lmdEmployeeID \r\n                                                INNER JOIN Employees On lmdEmployeeID = lmeEmployeeID \r\n                                            WHERE pasPostedToGL <> 0 \r\n                                            AND pasTaxYear = @TaxYear \r\n                                            ORDER BY patPayrollEmployeeID ASC");
		sqlCommand13.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable13 = database.GetDataTable(sqlCommand13, transaction);
		SqlCommand sqlCommand14 = new SqlCommand("SELECT ph.patPayrollEmployeeID, \r\n                                                                SUM(ISNULL((CASE WHEN pr.payPayType = 'D' THEN pht.pagSubTotal ELSE 0 END), 0)) AS AusDirectorsFees\r\n                                                                FROM PayrollSessions ps\r\n                                                                    INNER JOIN PayrollHeaders ph ON ph.patPayrollSessionID = ps.pasPayrollSessionID\r\n                                                                    INNER JOIN PayrollHeaderTotals pht ON pht.pagPayrollSessionID = ph.patPayrollSessionID AND pht.pagPayrollHeaderID = ph.patPayrollHeaderID\r\n                                                                    INNER JOIN PayrollRates pr ON pr.payPayrollRateID = pht.pagPayrollRateID \r\n                                                                WHERE ps.pasPostedToGL = 1\r\n                                                                AND ps.pasTaxYear = @TaxYear\r\n                                                                AND pht.pagAusLumpSumType = ''\r\n                                                                AND pht.pagAusIsETP <> 1\r\n                                                                GROUP BY ph.patPayrollEmployeeID");
		sqlCommand14.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		DataTable dataTable14 = database.GetDataTable(sqlCommand14, transaction);
		foreach (DataRow row in dataTable13.Rows)
		{
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			bool flag = row.Field<bool>("lmdWorkingHolidayMaker");
			DataRow dataRow2 = (DataRow)childBindingSource.AddNew();
			childBindingSource.SetKeyToNextAvailable(dataRow2);
			dataRow2["stlEmployeeID"] = row["patPayrollEmployeeID"];
			dataRow2["stlPreviousEmployeeID"] = row["lmePreviousEmployeeID"];
			DateTime dateTime = Convert.ToDateTime(row["StartDate"]);
			DateTime dateTime2 = Convert.ToDateTime(row["EndDate"]);
			DateTime maxStpSessionDate = PayrollHelpers.GetMaxStpSessionDate(num);
			DateTime dateTime3 = maxStpSessionDate;
			DateTime dateTime4 = maxStpSessionDate;
			bool num6 = IsDateBetweenStpSessionDates(num, dateTime);
			bool flag2 = IsDateBetweenStpSessionDates(num, dateTime2);
			if (num6 && flag2)
			{
				dateTime3 = dateTime;
				dateTime4 = dateTime2;
			}
			if (flag)
			{
				dataRow2["stlHomeCountry"] = row["lmdHomeCountry"].ToString().ToLower();
			}
			dataRow2["stlPeriodStartDate"] = dateTime3;
			dataRow2["stlPeriodEndDate"] = dateTime4;
			dataRow2["stlPayeeResidencyStatus"] = row["lmdResidencyStatus"];
			dataRow2["stlPayeeBirthDate"] = row["lmdBirthDate"];
			dataRow2["stlCessationDate"] = row["lmeTerminationDate"];
			if (dataRow2["stlCessationDate"] != DBNull.Value)
			{
				dataRow2["stlPayeeTerminatedIndicator"] = true;
				dataRow2["stlCessationType"] = row["lmeCessationType"];
			}
			dataRow2["stlTaxFreeThresholdClaimed"] = row["lmdTaxFreeThresholdClaimed"];
			dataRow2["stlStudyAndTrnLoanRepmtInd"] = row["lmdStudyTrainLoanRepayment"];
			dataRow2["stlStdntFinSupplSchemeLoanInd"] = row["lmdStdntFinSupplSchemeLoan"];
			dataRow2["stlBasisOfPaymentCode"] = row["lmdBasisOfPayment"];
			string text = row["lmdEmploymentStatus"].ToString();
			if (!(text == "Full-Time"))
			{
				if (text == "Part-Time")
				{
					dataRow2["stlEmployeeBasisCode"] = "P";
				}
				else
				{
					dataRow2["stlEmployeeBasisCode"] = "C";
				}
			}
			else
			{
				dataRow2["stlEmployeeBasisCode"] = "F";
			}
			string filterExpression = "patPayrollEmployeeID = " + row.Field<string>("patPayrollEmployeeID").Trim().ToLinq();
			DataRow[] employeeGrossPay = dataTable8.Select(filterExpression);
			DataRow[] employeeAdditionalAllow = dataTable.Select(filterExpression);
			DataRow[] employeePayGw = dataTable2.Select(filterExpression);
			DataRowCollection rows = PayrollHelpers.EmployeeStpLine(database, row.Field<string>("patPayrollEmployeeID"), flag, num).Rows;
			ProcessGrossAndHolidayMakersPayments(database, rows, dataRow2, employeeGrossPay, employeeAdditionalAllow, employeePayGw, num2, row, num, flag);
			DataRow[] array = dataTable3.Select(filterExpression);
			if (array.Length != 0)
			{
				dataRow2["stlReportableEmpSuperContrib"] = array[0].Field<decimal>("RESCAmount");
				dataRow2["stlSuperLiabilityAmount"] = array[0].Field<decimal>("SuperAmount");
			}
			DataRow[] array2 = dataTable4.Select(filterExpression);
			if (array2.Length != 0)
			{
				dataRow2["stlReportableEmpSuperContrib"] = dataRow2.Field<decimal>("stlReportableEmpSuperContrib") + array2[0].Field<decimal>("SSSuperAmount");
			}
			DataRow[] array3 = dataTable5.Select(filterExpression);
			if (array3.Length != 0)
			{
				if (array3[0].Field<decimal>("LumpSumAR") != 0m)
				{
					dataRow2["stlPayeeLumpSumPaymentA"] = array3[0].Field<decimal>("LumpSumAR");
					dataRow2["stlPayeeLumpSumPaymentAType"] = "R";
				}
				if (array3[0].Field<decimal>("LumpSumAT") != 0m)
				{
					dataRow2["stlPayeeLumpSumPaymentA"] = array3[0].Field<decimal>("LumpSumAT");
					dataRow2["stlPayeeLumpSumPaymentAType"] = "T";
				}
				dataRow2["stlPayeeLumpSumPaymentB"] = array3[0].Field<decimal>("LumpSumB");
				dataRow2["stlPayeeLumpSumPaymentD"] = array3[0].Field<decimal>("LumpSumD");
				dataRow2["stlPayeeLumpSumPaymentE"] = array3[0].Field<decimal>("LumpSumE");
				dataRow2["stlPayeeLumpSumPaymentW"] = array3[0].Field<decimal>("LumpSumW");
			}
			DataRow[] array4 = dataTable6.Select(filterExpression);
			dataRow2["stlCashOutLeave"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("CashOutLeave") : 0m);
			dataRow2["stlUnusedLeave"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("UnusedLeave") : 0m);
			dataRow2["stlWorkersComp"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("WorkersComp") : 0m);
			dataRow2["stlAncillaryDefenceLeave"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("AncillaryDefenceLeave") : 0m);
			dataRow2["stlOtherPaidLeave"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("OtherPaidLeave") : 0m);
			dataRow2["stlPaidParentalLeave"] = ((array4.Count() != 0) ? array4[0].Field<decimal>("PaidParentalLeave") : 0m);
			DataRow[] array5 = dataTable9.Select(filterExpression);
			dataRow2["stlOvertimeAmount"] = ((array5.Count() != 0) ? array5[0].Field<decimal>("OvertimeAmount") : 0m);
			dataRow2["stlBonusAmount"] = ((array5.Count() != 0) ? array5[0].Field<decimal>("BonusAmount") : 0m);
			DataRow[] array6 = dataTable14.Select(filterExpression);
			dataRow2["stlDirectorsFees"] = ((array6.Count() != 0) ? array6[0].Field<decimal>("AusDirectorsFees") : 0m);
			DataRow[] array7 = dataTable7.Select(filterExpression);
			dataRow2["stlSalarySacrificeSuper"] = ((array7.Count() != 0) ? array7[0].Field<decimal>("SalarySacrificeSuper") : 0m);
			dataRow2["stlSalarySacrificeOther"] = ((array7.Count() != 0) ? array7[0].Field<decimal>("SalarySacrificeOther") : 0m);
			dataRow2["stlOrdinaryTimeEarningsAmount"] = dataRow2.Field<decimal>("stlGrossPayments") + dataRow2.Field<decimal>("stlWorkingHolidayGrossPay") + dataRow2.Field<decimal>("stlBonusAmount") + dataRow2.Field<decimal>("stlOtherPaidLeave") + dataRow2.Field<decimal>("stlDirectorsFees");
			DataRow[] array8 = dataTable10.Select(filterExpression);
			foreach (DataRow dataRow3 in array8)
			{
				DataRow obj = (DataRow)childBindingSource3.AddNew();
				obj["staSessionID"] = dataRow2["stlSessionID"];
				obj["staLineID"] = dataRow2["stlLineID"];
				obj["staAllowanceID"] = ++num3;
				obj["staAllowanceType"] = dataRow3["panAusAllowanceType"];
				obj["staOtherAllowanceType"] = dataRow3["panAusOtherAllowanceType"];
				obj["staPayeeAllowanceAmount"] = dataRow3["panAmount"];
			}
			array8 = dataTable11.Select(filterExpression);
			foreach (DataRow dataRow4 in array8)
			{
				DataRow obj2 = (DataRow)childBindingSource4.AddNew();
				obj2["stdSessionID"] = dataRow2["stlSessionID"];
				obj2["stdLineID"] = dataRow2["stlLineID"];
				obj2["stdDeductionID"] = ++num4;
				obj2["stdDeductionType"] = dataRow4["panAusDeductionType"];
				obj2["stdPayeeDeductionAmount"] = dataRow4["panAmount"];
			}
			array8 = dataTable12.Select(filterExpression);
			foreach (DataRow dataRow5 in array8)
			{
				DataRow obj3 = (DataRow)childBindingSource2.AddNew();
				obj3["sttSessionID"] = dataRow2["stlSessionID"];
				obj3["sttLineID"] = dataRow2["stlLineID"];
				obj3["sttTerminationID"] = ++num5;
				obj3["sttTerminationCode"] = dataRow5["panAusETPCode"];
				DateTime dateTime5 = Convert.ToDateTime(dataRow5["pasPayrollDate"]);
				DateTime dateTime6 = maxStpSessionDate;
				if (IsDateBetweenStpSessionDates(num, dateTime5))
				{
					dateTime6 = dateTime5;
				}
				obj3["sttPayeeETPPaymentDate"] = dateTime6;
				obj3["sttTerminationPmtTaxFreeComp"] = dataRow5["panAusETPTaxFreeComponent"];
				obj3["sttTerminationPmtTaxableComp"] = dataRow5["panAusETPTaxableComponent"];
				obj3["sttPayeeTotalETPPAYGWAmount"] = dataRow5["panAmount"];
			}
			string taxFileNumber = PayrollHelpers.RemovePunctuation(dataRow2.Field<string>("stlTaxFileNumber").Trim());
			string employeeId = dataRow2.Field<string>("stlEmployeeID").Trim();
			dataRow2["stlTaxTreatmentCode"] = PayrollHelpers.GetTaxTreatmentCode(database, employeeId, taxFileNumber, flag);
			dataRow2["stlWorkingHolidayMaker"] = row["lmdWorkingHolidayMaker"];
		}
		SessionTotals sessionTotals = GetSessionTotals((int)currentAsDataRow["stpSessionID"], childBindingSource.GetDataTable(), childBindingSource2.GetDataTable(), childBindingSource3.GetDataTable());
		SessionTotals lastSessionData = GetLastSessionData(database, currentAsDataRow, transaction);
		currentAsDataRow["stpPayerTotalPAYGW"] = sessionTotals?.TotalPayGwAmount - lastSessionData?.TotalPayGwAmount;
		currentAsDataRow["stpPayerTotalGrossPay"] = sessionTotals?.TotalGrossPayments - lastSessionData?.TotalGrossPayments;
		currentAsDataRow["stpRunDateTimeStamp"] = DateTime.Now;
		SqlCommand sqlCommand15 = new SqlCommand("Update PayrollSessions Set pasTransferredToSTP = 1, pasSTPSessionID = @SessionID Where pasPostedToGL <> 0 And pasTaxYear = @TaxYear And pasTransferredToSTP = 0");
		sqlCommand15.Parameters.Add(new SqlParameter("@TaxYear", SqlDbType.Int)).Value = num;
		sqlCommand15.Parameters.Add(new SqlParameter("@SessionID", SqlDbType.Int)).Value = num2;
		database.ExecuteCommand(sqlCommand15);
		return true;
	}

	private static void ProcessGrossAndHolidayMakersPayments(M1Database database, DataRowCollection employeeInStpLine, DataRow lineDataRow, DataRow[] employeeGrossPay, DataRow[] employeeAdditionalAllow, DataRow[] employeePayGw, int stpSessionID, DataRow employeeRow, int stpTaxYear, bool isHolidayMaker)
	{
		string columnName = (isHolidayMaker ? "stlWorkingHolidayGrossPay" : "stlGrossPayments");
		string columnName2 = (isHolidayMaker ? "stlWorkingHolidayPAYGWAmount" : "stlTotalINBPAYGWAmount");
		if (employeeInStpLine.Count == 0)
		{
			decimal num = (employeeGrossPay.Any() ? employeeGrossPay[0].Field<decimal>("AusGrossPayments") : 0m);
			decimal num2 = (employeePayGw.Any() ? employeePayGw[0].Field<decimal>("AusTotalTaxWithheld") : 0m);
			lineDataRow[columnName] = num;
			lineDataRow[columnName2] = num2;
		}
		else
		{
			GatherGrossAndWorkingHolidayMakers(database, stpSessionID, employeeRow, stpTaxYear, lineDataRow, employeeGrossPay, employeePayGw, isHolidayMaker);
		}
		UpdateGrossPaymentsWithAllowances(employeeAdditionalAllow, lineDataRow, isHolidayMaker);
		RefreshGrossPayments(database, stpSessionID, employeeRow, stpTaxYear, lineDataRow, !isHolidayMaker);
	}

	private static void RefreshGrossPayments(M1Database database, int stpSessionId, DataRow employeeRow, int stpTaxYear, DataRow lineDataRow, bool isWorkingHolidayMaker)
	{
		string employeeId = employeeRow.Field<string>("patPayrollEmployeeID");
		decimal num = PayrollHelpers.TotalPayeeGrossAmount(database, stpSessionId, employeeId, isWorkingHolidayMaker, stpTaxYear);
		decimal num2 = PayrollHelpers.TotalPayeeTotalPayGwAmount(database, stpSessionId, employeeId, isWorkingHolidayMaker, stpTaxYear);
		string columnName = (isWorkingHolidayMaker ? "stlWorkingHolidayGrossPay" : "stlGrossPayments");
		string columnName2 = (isWorkingHolidayMaker ? "stlWorkingHolidayPAYGWAmount" : "stlTotalINBPAYGWAmount");
		lineDataRow[columnName] = num;
		lineDataRow[columnName2] = num2;
	}

	private static void UpdateGrossPaymentsWithAllowances(DataRow[] employeeAdditionalAllow, DataRow lineDataRow, bool isWorkingHolidayMaker)
	{
		if (employeeAdditionalAllow.Any())
		{
			string columnName = (isWorkingHolidayMaker ? "stlWorkingHolidayGrossPay" : "stlGrossPayments");
			decimal num = employeeAdditionalAllow[0].Field<decimal>("panAmount");
			lineDataRow[columnName] = lineDataRow.Field<decimal>(columnName) + num;
		}
	}

	private static void GatherGrossAndWorkingHolidayMakers(M1Database database, int stpSessionId, DataRow employeeRow, int stpTaxYear, DataRow lineDataRow, DataRow[] employeeGrossPay, DataRow[] employeePayGw, bool isWorkingHolidayMaker)
	{
		string employeeId = employeeRow.Field<string>("patPayrollEmployeeID");
		decimal num = PayrollHelpers.TotalPayeeGrossAmount(database, stpSessionId, employeeId, isWorkingHolidayMaker, stpTaxYear);
		decimal num2 = PayrollHelpers.TotalPayeeTotalPayGwAmount(database, stpSessionId, employeeId, isWorkingHolidayMaker, stpTaxYear);
		string columnName = (isWorkingHolidayMaker ? "stlWorkingHolidayGrossPay" : "stlGrossPayments");
		decimal num3 = (employeeGrossPay.Any() ? employeeGrossPay[0].Field<decimal>("AusGrossPayments") : 0m);
		lineDataRow[columnName] = num + num3;
		string columnName2 = (isWorkingHolidayMaker ? "stlWorkingHolidayPAYGWAmount" : "stlTotalINBPAYGWAmount");
		decimal num4 = (employeePayGw.Any() ? employeePayGw[0].Field<decimal>("AusTotalTaxWithheld") : 0m);
		lineDataRow[columnName2] = num2 + num4;
	}

	private void StpWriteTerminationPayments(ref List<string> lineString, DataRow lineDataRow)
	{
		lineString.Add(lineDataRow.Field<string>("sttTerminationCode").ToString());
		lineString.Add(lineDataRow.Field<DateTime>("sttPayeeETPPaymentDate").ToString("yyyy-MM-dd", DateTimeFormatInfo.CurrentInfo));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("sttTerminationPmtTaxFreeComp")));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("sttTerminationPmtTaxableComp")));
		lineString.Add(PayrollHelpers.FormatAmount(lineDataRow.Field<decimal>("sttPayeeTotalETPPAYGWAmount")));
	}

	private int NumberOfRowsToExport(DataTable dtTermination, DataTable dtAllowances, DataTable dtDeductions, DataTable dtPaidLeaves, DataTable dtEntitlements, DataTable dtLumpSums, DataTable dtSalarySacrifices, DataTable dtAllowancesIncomeStreamCollections, DataTable dtIncomeStreamType)
	{
		return new int[9]
		{
			dtTermination.Rows.Count,
			dtAllowances.Rows.Count,
			dtDeductions.Rows.Count,
			dtPaidLeaves.Rows.Count,
			dtSalarySacrifices.Rows.Count,
			dtEntitlements.Rows.Count,
			dtLumpSums.Rows.Count,
			dtAllowancesIncomeStreamCollections.Rows.Count,
			dtIncomeStreamType.Rows.Count
		}.Max();
	}

	private string StpCreateCSVString(M1BindingSource bindingSource, bool includeHeaderLabels, out StpMetadata stpMetadataObject)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		StringBuilder stringBuilder = new StringBuilder();
		try
		{
			using DataTable dataTable = database.GetDataTable("Select * from STPLines Where stlSessionID = " + M1Util.ConvertToSql(currentAsDataRow.Field<int>("stpSessionID")) + " order by stlLineID");
			List<string> metaData = new List<string>();
			List<string> headerRowLabels = new List<string>();
			List<string> headerLineString = new List<string>();
			List<string> lineString = new List<string>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			string empty = string.Empty;
			StringBuilder stringBuilder2 = new StringBuilder();
			stpMetadataObject = new StpMetadata();
			int count = dataTable.Rows.Count;
			string payerOrgName = currentAsDataRow.Field<string>("stpPayerOrganisationName").Trim();
			int sessionId = currentAsDataRow.Field<int>("stpSessionID");
			short taxYear = currentAsDataRow.Field<short>("stpTaxYear");
			empty = ((!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpPayerBranchCode").Trim())) ? currentAsDataRow.Field<string>("stpPayerBranchCode").Trim() : "1");
			if (dataTable.Rows.Count <= 0)
			{
				throw new M1Exception("There is no Single Touch Payroll session to be exported.");
			}
			bool isUpdateAction = IsUpdateAction(currentAsDataRow);
			stpMetadataObject = StpWriteMetaData(ref metaData, currentAsDataRow, isUpdateAction);
			StpWriteHeaderLabels(ref headerRowLabels);
			num = 1;
			num3 = 0;
			foreach (DataRow row in dataTable.Rows)
			{
				bool payeefullInfo = false;
				string sessionId2 = M1Util.ConvertToSql(row.Field<int>("stlSessionID"));
				string lineId = M1Util.ConvertToSql(row.Field<short>("stlLineID"));
				string employeeId = row.Field<string>("stlEmployeeID");
				DataTable terminationData = CsvQueries.GetTerminationData(database, sessionId2, lineId);
				DataTable allowancesData = CsvQueries.GetAllowancesData(database, sessionId2, lineId);
				DataTable deductionsData = CsvQueries.GetDeductionsData(database, sessionId2, lineId);
				DataTable paidLeavesData = CsvQueries.GetPaidLeavesData(database, sessionId2, lineId);
				DataTable entitlementsData = CsvQueries.GetEntitlementsData(database, sessionId2, lineId);
				DataTable lumpSumsData = CsvQueries.GetLumpSumsData(database, sessionId2, lineId);
				DataTable salarySacrificesData = CsvQueries.GetSalarySacrificesData(database, sessionId2, lineId);
				DataTable allowancesIncomeStreamCollectionsData = CsvQueries.GetAllowancesIncomeStreamCollectionsData(database, sessionId2, lineId);
				CsvQueries.GetWithoutPayeeServicesData(database, sessionId2, lineId);
				DataTable incomeStreamTypeData = CsvQueries.GetIncomeStreamTypeData(database, sessionId2, lineId, employeeId);
				int num14 = NumberOfRowsToExport(terminationData, allowancesData, deductionsData, paidLeavesData, entitlementsData, lumpSumsData, salarySacrificesData, allowancesIncomeStreamCollectionsData, incomeStreamTypeData);
				if (num14 <= 0)
				{
					continue;
				}
				num6 = 0;
				num5 = 0;
				num4 = 0;
				num7 = 0;
				num8 = 0;
				num9 = 0;
				num10 = 0;
				num12 = 0;
				num13 = 0;
				num11 = 0;
				for (int i = 0; i < num14; i++)
				{
					num4++;
					num3++;
					List<string> lineString2 = new List<string>();
					if (num3 != 1)
					{
						string abn = PayrollHelpers.RemovePunctuation(currentAsDataRow.Field<string>("stpABN").Trim());
						StpWriteIdentifiers(ref lineString2, row, 1, num, currentAsDataRow.Field<string>("stpBMSIdentifier").Trim(), abn, empty, payerOrgName, ref payeefullInfo, isUpdateAction);
					}
					if (num3 == 1)
					{
						StpWriteIdentifiers(ref lineString, row, 3, num, string.Empty, string.Empty, string.Empty, string.Empty, ref payeefullInfo, isUpdateAction);
					}
					else if (num4 == 1)
					{
						StpWriteIdentifiers(ref lineString2, row, 3, num, string.Empty, string.Empty, string.Empty, string.Empty, ref payeefullInfo, isUpdateAction);
					}
					else
					{
						StpWriteIdentifiers(ref lineString2, row, 2, num, string.Empty, string.Empty, string.Empty, string.Empty, ref payeefullInfo, isUpdateAction);
					}
					if (incomeStreamTypeData.Rows.Count != 0 && incomeStreamTypeData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num13 + 1 <= incomeStreamTypeData.Rows.Count && num13 + 1 <= incomeStreamTypeData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteIncomeStreamTypes(ref lineString, incomeStreamTypeData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteIncomeStreamTypes(ref lineString2, incomeStreamTypeData.Rows[num4 - 1]);
							}
							num13++;
						}
						else
						{
							lineString2.Add("");
							lineString2.Add("");
							lineString2.Add("");
							lineString2.Add("");
							lineString2.Add("");
							lineString2.Add("");
						}
					}
					else if (num3 == 1)
					{
						lineString.Add("");
						lineString.Add("");
						lineString.Add("");
						lineString.Add("");
						lineString.Add("");
						lineString.Add("");
					}
					else
					{
						lineString2.Add("");
						lineString2.Add("");
						lineString2.Add("");
						lineString2.Add("");
						lineString2.Add("");
						lineString2.Add("");
					}
					if (paidLeavesData.Rows.Count != 0 && paidLeavesData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num9 + 1 <= paidLeavesData.Rows.Count && num9 + 1 <= paidLeavesData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWritePaidLeaves(ref lineString, paidLeavesData.Rows[num4 - 1]);
							}
							else
							{
								StpWritePaidLeaves(ref lineString2, paidLeavesData.Rows[num4 - 1]);
							}
							num9++;
						}
						else
						{
							lineString2.Add("");
							lineString2.Add("");
						}
					}
					else if (num3 == 1)
					{
						lineString.Add("");
						lineString.Add("");
					}
					else
					{
						lineString2.Add("");
						lineString2.Add("");
					}
					if (allowancesData.Rows.Count != 0 && allowancesData.Rows[0].Field<int?>("staSessionID").HasValue)
					{
						if (num6 + 1 <= allowancesData.Rows.Count && num6 + 1 <= allowancesData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteAllowances(ref lineString, allowancesData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteAllowances(ref lineString2, allowancesData.Rows[num4 - 1]);
							}
							num6++;
						}
						else
						{
							num2 = 3;
							for (int j = 1; j <= num2; j++)
							{
								if (num3 == 1)
								{
									lineString.Add("");
								}
								else
								{
									lineString2.Add("");
								}
							}
						}
					}
					else
					{
						num2 = 3;
						for (int k = 1; k <= num2; k++)
						{
							if (num3 == 1)
							{
								lineString.Add("");
							}
							else
							{
								lineString2.Add("");
							}
						}
					}
					if (allowancesIncomeStreamCollectionsData.Rows.Count != 0 && allowancesIncomeStreamCollectionsData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num12 + 1 <= allowancesIncomeStreamCollectionsData.Rows.Count && num12 + 1 <= allowancesIncomeStreamCollectionsData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteAllowanceStreamCollection(ref lineString, allowancesIncomeStreamCollectionsData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteAllowanceStreamCollection(ref lineString2, allowancesIncomeStreamCollectionsData.Rows[num4 - 1]);
							}
							num12++;
						}
						else
						{
							num2 = 4;
							for (int l = 1; l <= num2; l++)
							{
								if (num3 == 1)
								{
									lineString.Add("");
								}
								else
								{
									lineString2.Add("");
								}
							}
						}
					}
					else
					{
						num2 = 4;
						for (int m = 1; m <= num2; m++)
						{
							if (num3 == 1)
							{
								lineString.Add("");
							}
							else
							{
								lineString2.Add("");
							}
						}
					}
					if (salarySacrificesData.Rows.Count != 0 && salarySacrificesData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num8 + 1 <= salarySacrificesData.Rows.Count && num8 + 1 <= salarySacrificesData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteSacrifices(ref lineString, salarySacrificesData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteSacrifices(ref lineString2, salarySacrificesData.Rows[num4 - 1]);
							}
							num8++;
						}
						else
						{
							lineString2.Add("");
							lineString2.Add("");
						}
					}
					else if (num3 == 1)
					{
						lineString.Add("");
						lineString.Add("");
					}
					else
					{
						lineString2.Add("");
						lineString2.Add("");
					}
					if (lumpSumsData.Rows.Count != 0 && lumpSumsData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num10 + 1 <= lumpSumsData.Rows.Count && num10 + 1 <= lumpSumsData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteUnusedAnnualOrLongServiceLeave(ref lineString, lumpSumsData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteUnusedAnnualOrLongServiceLeave(ref lineString2, lumpSumsData.Rows[num4 - 1]);
							}
							num10++;
						}
						else
						{
							num2 = 3;
							for (int n = 1; n <= num2; n++)
							{
								if (num3 == 1)
								{
									lineString.Add("");
								}
								else
								{
									lineString2.Add("");
								}
							}
						}
					}
					else
					{
						num2 = 3;
						for (int num15 = 1; num15 <= num2; num15++)
						{
							if (num3 == 1)
							{
								lineString.Add("");
							}
							else
							{
								lineString2.Add("");
							}
						}
					}
					if (terminationData.Rows.Count != 0 && terminationData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num11 + 1 <= terminationData.Rows.Count && num11 + 1 <= terminationData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteTerminationPayments(ref lineString, terminationData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteTerminationPayments(ref lineString2, terminationData.Rows[num4 - 1]);
							}
							num11++;
						}
						else
						{
							num2 = 5;
							for (int num16 = 1; num16 <= num2; num16++)
							{
								if (num3 == 1)
								{
									lineString.Add("");
								}
								else
								{
									lineString2.Add("");
								}
							}
						}
					}
					else
					{
						num2 = 5;
						for (int num17 = 1; num17 <= num2; num17++)
						{
							if (num3 == 1)
							{
								lineString.Add("");
							}
							else
							{
								lineString2.Add("");
							}
						}
					}
					if (deductionsData.Rows.Count != 0 && deductionsData.Rows[0].Field<int?>("stdSessionID").HasValue)
					{
						if (num5 + 1 <= deductionsData.Rows.Count && num5 + 1 <= deductionsData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteDeductions(ref lineString, deductionsData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteDeductions(ref lineString2, deductionsData.Rows[num4 - 1]);
							}
							num5++;
						}
						else
						{
							lineString2.Add("");
							lineString2.Add("");
						}
					}
					else if (num3 == 1)
					{
						lineString.Add("");
						lineString.Add("");
					}
					else
					{
						lineString2.Add("");
						lineString2.Add("");
					}
					if (entitlementsData.Rows.Count != 0 && entitlementsData.Rows[0].Field<int?>("stlSessionID").HasValue)
					{
						if (num7 + 1 <= entitlementsData.Rows.Count && num7 + 1 <= entitlementsData.Rows.Count)
						{
							if (num3 == 1)
							{
								StpWriteSuperRFB(ref lineString, entitlementsData.Rows[num4 - 1]);
							}
							else
							{
								StpWriteSuperRFB(ref lineString2, entitlementsData.Rows[num4 - 1]);
							}
							num7++;
						}
						else if (num3 == 1)
						{
							lineString.Add("");
							lineString.Add("");
						}
						else
						{
							lineString2.Add("");
							lineString2.Add("");
						}
					}
					num2 = 2;
					for (int num18 = 1; num18 <= num2; num18++)
					{
						if (num3 == 1)
						{
							lineString.Add("");
						}
						else
						{
							lineString2.Add("");
						}
					}
					if (num3 != 1)
					{
						stringBuilder2.AppendLine(string.Join(",", lineString2.ToArray()));
					}
					num++;
				}
			}
			if (includeHeaderLabels)
			{
				stringBuilder.AppendLine(string.Join(",", metaData.ToArray()));
			}
			decimal totalPayerDeductionChildSupportGarnishee = PayrollHelpers.TotalChildSupportGarnishees(database, sessionId, taxYear);
			decimal totalPayerDeductionChildSupportDeductions = PayrollHelpers.TotalChildSupportDeductions(database, sessionId, taxYear);
			stringBuilder.AppendLine(string.Join(",", headerRowLabels.ToArray()));
			StpWriteHeaderLine(ref headerLineString, currentAsDataRow, count, totalPayerDeductionChildSupportGarnishee, totalPayerDeductionChildSupportDeductions);
			headerLineString.AddRange(lineString);
			stringBuilder.AppendLine(string.Join(",", headerLineString.ToArray()));
			stringBuilder.AppendLine(stringBuilder2.ToString());
		}
		catch (M1Exception ex)
		{
			throw new M1Exception(ex.Message);
		}
		return stringBuilder.ToString();
	}

	private string GetBase64ConvertedCSVString(M1BindingSource bindingSource, out StpMetadata stpMetaData)
	{
		stpMetaData = null;
		string s = StpCreateCSVString(bindingSource, includeHeaderLabels: false, out stpMetaData);
		return Convert.ToBase64String(Encoding.ASCII.GetBytes(s));
	}

	private async Task<HttpResponseMessage> StpPostMessage(M1BindingSource bindingSource, DataRow stpDataRow)
	{
		try
		{
			StpMetadata stpMetaData = null;
			StpDbOptions stpDbOptions = StpGetDbOptions(bindingSource);
			if (!stpDbOptions.IsPopulated)
			{
				throw new M1Exception("STP Database options are not filled out correctly.");
			}
			string base64ConvertedCSVString = GetBase64ConvertedCSVString(bindingSource, out stpMetaData);
			string text = "SendMessage_Auth?P=" + stpDbOptions.ProjectKey.Trim();
			string text2 = Guid.NewGuid().ToString("N");
			string text3 = stpDbOptions.SenderId.Trim();
			string uriString = stpDbOptions.MessageUrl.Trim();
			string text4 = stpDbOptions.Password.Trim();
			string text5 = "multipart/mixed; boundary=\"---" + text2 + "\"";
			string s = text3 + ":" + text4;
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			string text6 = "Basic " + Convert.ToBase64String(bytes);
			string text7 = "<metadata><from>" + stpMetaData.From + "</from><role>" + stpMetaData.Role + "</role><conversationid>" + stpMetaData.ConversationId + "</conversationid><action>" + stpMetaData.Action + "</action></metadata>";
			HttpClient val = new HttpClient();
			val.BaseAddress = new Uri(uriString);
			val.DefaultRequestHeaders.Accept.Clear();
			((HttpHeaders)val.DefaultRequestHeaders).TryAddWithoutValidation("Content-Type", text5);
			((HttpHeaders)val.DefaultRequestHeaders).Add("Sender-ID", text3);
			((HttpHeaders)val.DefaultRequestHeaders).Add("Authorization", text6);
			MultipartContent val2 = new MultipartContent();
			try
			{
				HttpContent val3 = (HttpContent)new StringContent(text7, Encoding.UTF8, "application/xml");
				val3.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
				((HttpHeaders)val3.Headers).Add("content-transfer-encoding", "7bit");
				val2.Add(val3);
				HttpContent val4 = (HttpContent)new StringContent(base64ConvertedCSVString);
				((HttpHeaders)val4.Headers).Clear();
				((HttpHeaders)val4.Headers).TryAddWithoutValidation("Content-Type", "application/octet-stream");
				((HttpHeaders)val4.Headers).Add("content-transfer-encoding", "base64");
				((HttpHeaders)val4.Headers).Add("Content-Disposition", "attachment;filename=\"payevnt.csv\"");
				((HttpHeaders)val4.Headers).Add("PayloadName", "STP_CSV_2020");
				val2.Add(val4);
				string result = ((HttpContent)val2).ReadAsStringAsync().GetAwaiter().GetResult();
				HttpContent val5 = (HttpContent)new StringContent("Content-Type: " + ((object)((HttpContent)val2).Headers.ContentType)?.ToString() + "\r\n\r\nThis is a multi - part message in MIME format.\r\n" + result);
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				HttpResponseMessage result2 = val.PostAsync(text, val5).Result;
				stpDataRow.SetField("stpSTPSubmissionID", stpMetaData.ConversationId);
				return result2;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch (M1Exception ex)
		{
			throw new M1Exception((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
		}
	}

	public string StpProcessGet(M1BindingSource bindingSource)
	{
		try
		{
			string result = string.Empty;
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			HttpResponseMessage result2 = StpGetMessage(bindingSource, currentAsDataRow).Result;
			string result3 = result2.Content.ReadAsStringAsync().Result;
			if (!result2.IsSuccessStatusCode)
			{
				return "E-STP Database options are not filled out correctly. \n\n " + result2.ReasonPhrase;
			}
			StpLog stpLog = (StpLog)new XmlSerializer(typeof(StpLog)).Deserialize(new StringReader(result3));
			if (stpLog.Record != null)
			{
				DateTime result4 = DateTime.Now;
				DateTime.TryParseExact(stpLog.Record.Time, "yyyyMMddhhmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result4);
				stringBuilder.Append("Message Log In ID => ");
				stringBuilder.AppendLine(stpLog.Record.MessageLogInId);
				stringBuilder.Append("Submission ID => ");
				stringBuilder.AppendLine(stpLog.Record.ConversationId);
				stringBuilder.Append("Status Code => ");
				stringBuilder.AppendLine(stpLog.Record.StatusCode);
				stringBuilder.Append("Status Description => ");
				stringBuilder.AppendLine(stpLog.Record.StatusDescription);
				if (stpLog.Record.StatusCode.Equals("Pending", StringComparison.CurrentCultureIgnoreCase))
				{
					result = "P-ATO submission is pending.\nSee status log for more details.";
				}
				else if (stpLog.Record.StatusCode.Equals("Success", StringComparison.CurrentCultureIgnoreCase))
				{
					currentAsDataRow.SetField((!currentAsDataRow.Field<bool>("stpFullFileReplacement")) ? "stpSTPSubmitted" : "stpSTPFFRSubmitted", value: true);
					result = "S-Success";
				}
				else
				{
					if (!stpLog.Record.StatusCode.Equals("Error", StringComparison.CurrentCultureIgnoreCase))
					{
						currentAsDataRow.SetField((!currentAsDataRow.Field<bool>("stpFullFileReplacement")) ? "stpSTPSubmitted" : "stpSTPFFRSubmitted", value: true);
					}
					stringBuilder.AppendLine();
					if (stpLog.Record.AtoResponse != null)
					{
						Event[] array = stpLog.Record.AtoResponse.Event;
						foreach (Event obj in array)
						{
							num++;
							stringBuilder.AppendLine($"----------Event {num} error items-start----------");
							stringBuilder.AppendLine();
							stringBuilder.Append("Maximum Severity Code => ");
							stringBuilder.AppendLine(obj.MaximumSeverityCode);
							stringBuilder.AppendLine();
							EventItem[] eventItems = obj.EventItems;
							foreach (EventItem eventItem in eventItems)
							{
								stringBuilder.Append("Error Code => ");
								stringBuilder.AppendLine(eventItem.ErrorCode);
								stringBuilder.Append("Severity Code => ");
								stringBuilder.AppendLine(eventItem.SeverityCode);
								if (eventItem.ShortDescription != null)
								{
									stringBuilder.Append("Short Description => ");
									stringBuilder.AppendLine(eventItem.ShortDescription);
								}
								if (eventItem.Locations != null && eventItem.Locations.Any())
								{
									EventItemLocation[] locations = eventItem.Locations;
									foreach (EventItemLocation eventItemLocation in locations)
									{
										stringBuilder.Append(eventItemLocation.LocationInstanceIdentifier);
										stringBuilder.Append(" => ");
										stringBuilder.AppendLine(eventItemLocation.LocationPathText);
									}
								}
								if (eventItem.Parameters != null && eventItem.Parameters.Any())
								{
									EventItemParameter[] parameters = eventItem.Parameters;
									foreach (EventItemParameter eventItemParameter in parameters)
									{
										stringBuilder.Append(eventItemParameter.ParameterIdentifier);
										stringBuilder.Append(" => ");
										stringBuilder.AppendLine(eventItemParameter.ParameterText);
									}
								}
								stringBuilder.AppendLine();
							}
							stringBuilder.AppendLine($"----------Event {num} error items-end----------");
						}
					}
					result = (stpLog.Record.StatusCode.Equals("Partial", StringComparison.CurrentCultureIgnoreCase) ? "R" : (stpLog.Record.StatusCode.Equals("Warning", StringComparison.CurrentCultureIgnoreCase) ? "W" : "E")) + "-ATO submission has failed or only been partially accepted due to some errors/warnings [" + stpLog.Record.StatusDescription + "].\nSee status log for more details.";
				}
			}
			currentAsDataRow.SetField("stpSTPResponseText", string.Empty);
			currentAsDataRow.SetField("stpSTPResponseText", stringBuilder.ToString());
			return result;
		}
		catch (Exception ex)
		{
			return (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
		}
	}

	public void StpCleanPreviousEmployeeId(M1BindingSource bindingSource, int sessionId)
	{
		M1Database database = bindingSource.Database;
		string queryString = $"SELECT stlEmployeeID from STPLines WHERE stlSessionID = {sessionId}";
		foreach (DataRow row in database.GetDataTable(queryString).Rows)
		{
			string text = row.Field<string>("stlEmployeeID").ToSql();
			string queryString2 = "UPDATE Employees SET lmePreviousEmployeeID = '' WHERE lmeEmployeeID = " + text;
			database.ExecuteCommand(queryString2);
		}
	}

	public string StpProcessPost(M1BindingSource bindingSource)
	{
		try
		{
			string empty = string.Empty;
			DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
			string result = StpPostMessage(bindingSource, currentAsDataRow).Result.Content.ReadAsStringAsync().Result;
			SendMessageResult sendMessageResult = (SendMessageResult)new XmlSerializer(typeof(SendMessageResult)).Deserialize(new StringReader(result));
			if (!currentAsDataRow.Field<bool>("stpFullFileReplacement"))
			{
				currentAsDataRow.SetField("stpSTPSubmitted", value: false);
				currentAsDataRow.SetField<DateTime?>("stpSTPSubmittedDate", null);
			}
			else
			{
				currentAsDataRow.SetField("stpSTPFFRSubmitted", value: false);
				currentAsDataRow.SetField<DateTime?>("stpSTPFFRSubmittedDate", null);
			}
			currentAsDataRow.SetField("stpSTPResponseText", string.Empty);
			currentAsDataRow.SetField("stpSTPResponseRtf", string.Empty);
			if (sendMessageResult.Result.Equals("error", StringComparison.CurrentCultureIgnoreCase))
			{
				currentAsDataRow.SetField("stpSTPSubmissionID", string.Empty);
				empty = sendMessageResult.Description ?? "Error with empty description";
			}
			else
			{
				currentAsDataRow.SetField((!currentAsDataRow.Field<bool>("stpFullFileReplacement")) ? "stpSTPSubmittedDate" : "stpSTPFFRSubmittedDate", (DateTime?)DateTime.Now);
				empty = sendMessageResult.Result;
			}
			return empty;
		}
		catch (Exception ex)
		{
			return (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
		}
	}

	public bool StpExportCSV(M1BindingSource bindingSource)
	{
		bool result = false;
		StpMetadata stpMetadataObject = null;
		string value = StpCreateCSVString(bindingSource, includeHeaderLabels: true, out stpMetadataObject);
		if (string.IsNullOrWhiteSpace(value))
		{
			return false;
		}
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		string fileName = "STPSessionExport" + currentAsDataRow.Field<int>("stpSessionID") + ".csv";
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			Filter = "CSV Files|*.csv",
			FilterIndex = 2,
			RestoreDirectory = true,
			Title = "Save As",
			FileName = fileName,
			CheckPathExists = true,
			ValidateNames = true,
			AutoUpgradeEnabled = (bindingSource.Context == null || !bindingSource.Context.DisableOpenFileHelp)
		};
		switch (saveFileDialog.ShowDialog())
		{
		case DialogResult.OK:
		{
			StreamWriter streamWriter = File.CreateText(saveFileDialog.FileName);
			streamWriter.WriteLine(value);
			streamWriter.Close();
			streamWriter.Dispose();
			result = true;
			break;
		}
		case DialogResult.Cancel:
			result = false;
			break;
		}
		saveFileDialog.Dispose();
		return result;
	}

	public string StpExportCSVCheck(M1BindingSource bindingSource)
	{
		M1Database database = bindingSource.Database;
		DataRow currentAsDataRow = bindingSource.CurrentAsDataRow;
		Regex regex = new Regex("^[a-zA-Z0-9_.,?!(){}:; '|=\\/@#$%\\*&amp;&quot;-]*$");
		Regex regex2 = new Regex("^[a-zA-Z0-9_.,?(){}:; '|=\\/@#$%\\*&amp;&quot;-]*$");
		Regex regex3 = new Regex("^[a-zA-Z0-9_.,?(){}:; '|=\\/@#$%\\*&amp;&quot;-]*$");
		List<string> list = new List<string>();
		_ = string.Empty;
		if (currentAsDataRow != null)
		{
			if (!string.IsNullOrWhiteSpace(PayrollHelpers.RemovePunctuation(currentAsDataRow.Field<string>("stpABN").Trim())) && !Regex.IsMatch(PayrollHelpers.RemovePunctuation(currentAsDataRow.Field<string>("stpABN").Trim()), "^[0-9]{11}$"))
			{
				list.Add("- Payer ABN number must be numeric and should contain 11 digits.");
			}
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpPayerBranchCode").Trim()) && !Regex.IsMatch(currentAsDataRow.Field<string>("stpPayerBranchCode").Trim(), "^[0-9]*$"))
			{
				list.Add("- Payer branch code must be numeric.");
			}
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpPayerOrganisationName").Trim()) && !regex.IsMatch(currentAsDataRow.Field<string>("stpPayerOrganisationName").Trim()))
			{
				list.Add("- Invalid characters in Payer Organisation Name.");
			}
			if (!regex2.IsMatch(currentAsDataRow.Field<string>("stpContactName").Trim()))
			{
				list.Add("- Invalid characters in payer contact name.");
			}
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpEmailAddress").Trim()) && !Regex.IsMatch(currentAsDataRow.Field<string>("stpEmailAddress").Trim(), "^[a-zA-Z0-9_.@-]*$"))
			{
				list.Add("- Payer email address contains invalid characters.");
			}
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpPhoneNumber").Trim()) && !Regex.IsMatch(currentAsDataRow.Field<string>("stpPhoneNumber").Trim(), "^[0-9 ]*$"))
			{
				list.Add("- Payer business phone number contains invalid characters.");
			}
			if (!string.IsNullOrWhiteSpace(currentAsDataRow.Field<string>("stpPostCode").Trim()) && !Regex.IsMatch(currentAsDataRow.Field<string>("stpPostCode").Trim(), "^[0-9]{4}$"))
			{
				list.Add("- Payer post code must be numeric and should contain 4 digits.");
			}
			if (!regex2.IsMatch(currentAsDataRow.Field<string>("stpPayerDeclarerIdentifier").Trim()))
			{
				list.Add("- Invalid characters in payer declarer identifier.");
			}
			DataTable dataTable = database.GetDataTable("Select * From STPLines  Where stlSessionID = " + M1Util.ConvertToSql(currentAsDataRow.Field<int>("stpSessionID")));
			if (dataTable.Rows.Count != 0)
			{
				foreach (DataRow row3 in dataTable.Rows)
				{
					string text = PayrollHelpers.RemovePunctuation(row3.Field<string>("stlTaxFileNumber").Trim());
					short num = row3.Field<short>("stlLineID");
					string text2 = row3.Field<string>("stlEmployeeID").Trim();
					if (!string.IsNullOrWhiteSpace(text))
					{
						if (text.Length == 8 || text.Length == 9)
						{
							if (!Regex.IsMatch(text, "^[0-9]*$"))
							{
								list.Add($"- Line {num}, employee ID {text2}, payee tax file number must contain numeric characters only.");
							}
							else if ((text.Length == 8 || text.Length == 9) && !new List<string> { "000000000", "111111111", "333333333", "444444444" }.Contains(text))
							{
								bool flag = true;
								decimal num2 = int.Parse(text.Substring(0, 1)) + 4 * int.Parse(text.Substring(1, 1)) + 3 * int.Parse(text.Substring(2, 1)) + 7 * int.Parse(text.Substring(3, 1)) + 5 * int.Parse(text.Substring(4, 1)) + 8 * int.Parse(text.Substring(5, 1)) + 6 * int.Parse(text.Substring(6, 1));
								decimal num3 = 9 * int.Parse(text.Substring(7, 1));
								text = Regex.Replace(text, "^0+", "");
								if (!decimal.TryParse(text, out var _))
								{
									flag = false;
								}
								if (text.Length < 8)
								{
									flag = false;
								}
								if (text.Length == 8)
								{
									decimal num4 = 10 * int.Parse(text.Substring(7, 1));
									if ((num2 + num4) % 11m != 0m)
									{
										flag = false;
									}
								}
								if (text.Length == 9)
								{
									decimal num5 = 10 * int.Parse(text.Substring(8, 1));
									if ((num2 + num3 + num5) % 11m != 0m)
									{
										flag = false;
									}
								}
								if (!flag)
								{
									list.Add($"- Line {num}, employee ID {text2}, payee tax file number must be a valid value.");
								}
							}
						}
						else
						{
							list.Add($"- Line {num}, employee ID {text2}, payee tax file number must contain 8 or 9 digits only.");
						}
					}
					string text3 = PayrollHelpers.RemovePunctuation(row3.Field<string>("stlContractorABN").Trim());
					if (!string.IsNullOrWhiteSpace(text3) && !Regex.IsMatch(text3, "^[0-9]{11}$"))
					{
						list.Add($"- Line {num}, employee ID {text2}, contractor ABN must be numeric and should contain 11 digits.");
					}
					if (!string.IsNullOrWhiteSpace(text2) && !regex2.IsMatch(text2))
					{
						list.Add($"- Line {num}, employee ID {text2}, invalid characters in employee ID.");
					}
					string text4 = row3.Field<string>("stlPayeeFamilyName").Trim();
					if (!string.IsNullOrWhiteSpace(text4) && !regex2.IsMatch(text4))
					{
						list.Add($"- Line {num}, employee ID {text2}, invalid characters in employee's family name.");
					}
					string text5 = row3.Field<string>("stlPayeeFirstName").Trim();
					if (!string.IsNullOrWhiteSpace(text5) && !regex2.IsMatch(text5))
					{
						list.Add($"- Line {num}, employee ID {text2}, invalid characters in employee's first name.");
					}
					string text6 = row3.Field<string>("stlPayeeOtherName").Trim();
					if (!string.IsNullOrWhiteSpace(text6) && !regex2.IsMatch(text6))
					{
						list.Add($"- Line {num}, employee ID {text2}, invalid characters in employee's other name.");
					}
					string text7 = row3.Field<string>("stlAddressLine1").Trim();
					if (!string.IsNullOrWhiteSpace(text7) && !regex3.IsMatch(text7))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee address line 1 contains invalid characters.");
					}
					string text8 = row3.Field<string>("stlAddressLine2").Trim();
					if (!string.IsNullOrWhiteSpace(text8) && !regex3.IsMatch(text8))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee address line 2 contains invalid characters.");
					}
					string text9 = row3.Field<string>("stlSuburb").Trim();
					if (!string.IsNullOrWhiteSpace(text9) && !regex3.IsMatch(text9))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee suburb/town contains invalid characters.");
					}
					string text10 = row3.Field<string>("stlPostCode").Trim();
					if (!string.IsNullOrWhiteSpace(text10) && !Regex.IsMatch(text10, "^[0-9]{4}$"))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee post code must be numeric and should contain 4 digits.");
					}
					string text11 = row3.Field<string>("stlEmailAddress").Trim();
					if (!string.IsNullOrWhiteSpace(text11) && !Regex.IsMatch(text11, "^[a-zA-Z0-9_.@-]*$"))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee email address contains invalid characters.");
					}
					if (!string.IsNullOrWhiteSpace(row3.Field<string>("stlPhoneNumber").Trim()) && !Regex.IsMatch(row3.Field<string>("stlPhoneNumber").Trim(), "^[0-9 ]*$"))
					{
						list.Add($"- Line {num}, employee ID {text2}, payee phone number contains invalid characters.");
					}
					int num6 = row3.Field<int>("stlSessionID");
					string queryString = "Select staSessionID, staLineID, staAllowanceID, staOtherAllowanceType From STPAllowances  Where staSessionID = " + M1Util.ConvertToSql(num6) + " and staLineID = " + M1Util.ConvertToSql(num);
					using (DataTable dataTable2 = database.GetDataTable(queryString))
					{
						if (dataTable2.Rows.Count != 0)
						{
							if (dataTable2.Rows.Count > 30)
							{
								list.Add($"- Line {num}, employee ID {text2}, cannot have more than 30 allowances.");
							}
							foreach (DataRow row4 in dataTable2.Rows)
							{
								if (!string.IsNullOrWhiteSpace(row4.Field<string>("staOtherAllowanceType").Trim()) && !regex2.IsMatch(row4.Field<string>("staOtherAllowanceType").Trim()))
								{
									short num7 = row4.Field<short>("staAllowanceID");
									list.Add($"- Line {num}, employee ID  {text2}, allowance ID {num7}, invalid characters in payee's other allowance type.");
								}
							}
						}
					}
					string queryString2 = "Select Count(*) As Deduction_Count From STPDeductions Where stdSessionID = " + M1Util.ConvertToSql(num6) + " and stdLineID = " + M1Util.ConvertToSql(num);
					using (DataTable dataTable3 = database.GetDataTable(queryString2))
					{
						if (dataTable3.Rows.Count != 0 && dataTable3.Rows[0].Field<int>("Deduction_Count") > 4)
						{
							list.Add($"- Line {num}, employee ID {text2}, cannot have more than 2 deductions.");
						}
					}
					string queryString3 = "Select Count(*) As Termination_Count From STPTerminationPayment Where sttSessionID = " + M1Util.ConvertToSql(num6) + " and sttLineID = " + M1Util.ConvertToSql(num);
					using DataTable dataTable4 = database.GetDataTable(queryString3);
					if (dataTable4.Rows.Count != 0 && dataTable4.Rows[0].Field<int>("Termination_Count") > 25)
					{
						list.Add($"- Line {num}, employee ID {text2}, cannot have more than 25 termination payments.");
					}
				}
			}
			dataTable = null;
		}
		return string.Join("\n", list.ToArray()).ToString();
	}
}
