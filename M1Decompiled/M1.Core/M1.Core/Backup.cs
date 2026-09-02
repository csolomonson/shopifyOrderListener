using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using M1.Extensions;

namespace M1.Core;

public class Backup
{
	public class BackupInfo
	{
		public bool Loaded;

		public string LastRunInfo = string.Empty;

		public decimal JobStartTime;

		public int NumberOfDays = 1;

		public string Location = "C:\\M1Backup\\";

		public int Copies = 1;

		public Dictionary<string, string> Databases = new Dictionary<string, string>();

		public string Description = string.Empty;
	}

	private ServerManager serverManager;

	private AppContext currentContext;

	private string m1JobName = "M1 Data Backup";

	public Backup(AppContext context, ServerManager attachedServerManager)
	{
		currentContext = context;
		serverManager = attachedServerManager;
	}

	public bool CheckIfAgentIsRunning()
	{
		if (GetSqlServerAgentStatus().Contains("Running"))
		{
			return true;
		}
		if (!serverManager.IsSQLExpress(null, null, string.Empty) && MessageBox.Show("The Sql Server Agent is not currently running. This needs to be running for the backups to work properly. Would you like to start the Sql Server Agent now?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
		{
			if (!StartSqlServerAgent())
			{
				MessageBox.Show("The SQL Server Agent could not be started automatically.\rPlease contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			return true;
		}
		return false;
	}

	public string GetSqlServerAgentStatus()
	{
		try
		{
			return (string)serverManager.ExecuteScalar(null, null, "master", "EXEC xp_servicecontrol 'QueryState', 'SQLSERVERAGENT'");
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public bool StartSqlServerAgent()
	{
		try
		{
			serverManager.ExecuteCommand(null, null, "master", "exec master.dbo.xp_servicecontrol 'START', 'SQLServerAgent'");
			return true;
		}
		catch
		{
			return false;
		}
	}

	public bool StopSqlServerAgent()
	{
		try
		{
			serverManager.ExecuteCommand(null, null, "master", "exec master.dbo.xp_servicecontrol 'STOP', 'SQLServerAgent'");
			return true;
		}
		catch
		{
			return false;
		}
	}

	private bool doesJobExist(string jobname)
	{
		jobname = jobname.Trim().ToUpper();
		using (DataTable dataTable = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_help_job"))
		{
			foreach (DataRow row in dataTable.Rows)
			{
				if (row.Field<string>("name").Trim().ToUpper() == jobname)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CanAccessBackupJob()
	{
		return serverManager.HasPermissionsToDatabase(null, null, "msdb");
	}

	public BackupInfo GetBackupInfo()
	{
		BackupInfo backupInfo = new BackupInfo();
		if (doesJobExist(m1JobName))
		{
			DataTable dataTable = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_help_job @job_name = " + m1JobName.ToSql() + ", @job_aspect = 'JOB'");
			if (dataTable.Rows.Count > 0)
			{
				string empty = string.Empty;
				empty = dataTable.Rows[0].Field<int>("last_run_outcome") switch
				{
					0 => "Failed", 
					1 => "Succeeded", 
					3 => "Cancelled", 
					_ => "Unknown", 
				};
				if (!empty.Equals("Unknown"))
				{
					empty = empty + " (" + convertSQLToDate(dataTable.Rows[0].Field<int>("last_run_date").ToString()).ToShortDateString() + " " + M1Math.Round(decimal.Parse(dataTable.Rows[0].Field<int>("last_run_time").ToString()) / 10000m, 2).ToString().Replace('.', ':') + ")";
				}
				backupInfo.LastRunInfo = empty;
			}
			dataTable = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_help_jobstep @job_name = " + m1JobName.ToSql());
			if (dataTable.Rows.Count > 0)
			{
				string empty2 = string.Empty;
				int num = 0;
				int result = 0;
				bool flag = false;
				bool flag2 = false;
				string empty3 = string.Empty;
				string empty4 = string.Empty;
				_ = string.Empty;
				string empty5 = string.Empty;
				foreach (DataRow row in dataTable.Rows)
				{
					empty5 = string.Empty;
					empty2 = row.Field<string>("command");
					num = empty2.IndexOf("BACKUP DATABASE ");
					if (num != -1)
					{
						empty2 = empty2.Substring(num + 16);
						num = empty2.IndexOf(" TO DISK");
						if (num != -1)
						{
							empty5 = empty2.Substring(0, num);
							if (empty5.StartsWith("["))
							{
								empty5 = empty5.Substring(1);
								if (empty5.EndsWith("]"))
								{
									empty5 = empty5.Substring(0, empty5.Length - 1);
								}
							}
							if (!backupInfo.Databases.ContainsKey(empty5.ToUpper()))
							{
								backupInfo.Databases.Add(empty5.ToUpper(), empty5);
							}
						}
						if (!flag2)
						{
							empty3 = empty2.Substring(num + 8);
							num = empty3.IndexOf("'");
							if (num != -1)
							{
								empty3 = empty3.Substring(num + 1);
								num = -1;
								empty4 = string.Empty;
								for (int i = 0; i <= empty3.Length; i++)
								{
									if (empty3[i] == '\'')
									{
										if (empty3.Length == i || empty3[i + 1] != '\'')
										{
											num = i;
											break;
										}
										i++;
										empty4 += "'";
									}
									else
									{
										empty4 += empty3[i];
									}
								}
								empty3 = empty4;
								if (empty3.Length != 0)
								{
									num = empty3.LastIndexOf("\\");
									if (num != -1)
									{
										empty3 = empty3.Substring(0, num + 1);
										backupInfo.Location = empty3;
										flag2 = true;
									}
								}
							}
						}
					}
					if (flag || empty5.Length == 0)
					{
						continue;
					}
					empty2 = row.Field<string>("command");
					num = empty2.IndexOf("EXEC xp_cmdshell 'DEL ");
					if (num != -1)
					{
						empty2 = empty2.Substring(num + 22);
						num = empty2.IndexOf(".bak");
						if (num != -1)
						{
							empty2 = empty2.Substring(0, num);
							if (int.TryParse(empty2.Substring(empty2.Length - 2), out result))
							{
								backupInfo.Copies = result;
								flag = true;
							}
						}
					}
					else
					{
						backupInfo.Copies = 0;
						flag = true;
					}
				}
			}
			DataTable dataTable2 = serverManager.GetDataTable(null, null, "msdb", 0, "EXEC sp_help_jobschedule @job_name = " + m1JobName.ToSql());
			if (dataTable2.Rows.Count > 0)
			{
				DataRow[] array = dataTable2.Select("schedule_name = " + m1JobName.ToLinq());
				if (array.Length != 0)
				{
					backupInfo.NumberOfDays = array[0].Field<int>("freq_interval");
					backupInfo.JobStartTime = M1Math.Round((decimal)array[0].Field<int>("active_start_time") / 10000m, 2);
				}
			}
		}
		backupInfo.Loaded = true;
		return backupInfo;
	}

	public DateTime? GetLastBackupTimeForDatabase(string databaseName)
	{
		return (DateTime?)serverManager.ExecuteScalar(null, null, "msdb", "SELECT TOP 1 backup_finish_date FROM msdb.dbo.BACKUPSET Where database_name = " + databaseName.ToSql() + " Order By backup_finish_date Desc");
	}

	private string getDatabaseDescription(string databaseName)
	{
		if (databaseName.ToUpper().StartsWith("M1_"))
		{
			return databaseName.Substring(3) + " - " + ((string)serverManager.ExecuteScalar(null, null, "msdb", "select xadDescription From " + databaseName + ".dbo.DatasetProperties")).Trim();
		}
		return databaseName;
	}

	public bool CreateBackupJob(BackupInfo backupInfo)
	{
		if (backupInfo.Location.Length == 0)
		{
			MessageBox.Show("Please enter a backup folder before continuing.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (doesJobExist(m1JobName))
		{
			if (MessageBox.Show("There is already a backup job created for M1. Do you want to continue?", "Backup Job Already Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
			{
				return false;
			}
			deleteJob(m1JobName);
		}
		new Dmo(currentContext, serverManager).SetConfigure();
		string text = backupInfo.Location.AddBackslash();
		new ServerFileSystem(serverManager).CreateFolder(text);
		if (backupInfo.Databases.Count > 0)
		{
			createJob(m1JobName, "M1 Database Backup");
			int num = 0;
			string empty = string.Empty;
			int copies = backupInfo.Copies;
			string empty2 = string.Empty;
			foreach (KeyValuePair<string, string> database in backupInfo.Databases)
			{
				empty = database.Value;
				num++;
				string text2 = string.Empty;
				if (copies > 0)
				{
					text2 = text2 + "EXEC xp_cmdshell 'DEL \"" + (text + empty + copies.ToString().PadLeft(2, '0')).Replace("'", "''") + ".bak\"'\r\n";
					for (int num2 = copies; num2 >= 2; num2--)
					{
						text2 = text2 + "EXEC xp_cmdshell 'REN \"" + (text + empty + (num2 - 1).ToString().PadLeft(2, '0')).Replace("'", "''") + ".bak\" \"" + empty + num2.ToString().PadLeft(2, '0') + ".bak\"'\r\n";
					}
					text2 = text2 + "EXEC xp_cmdshell 'REN \"" + (text + empty).Replace("'", "''") + ".bak\" \"" + empty.Replace("'", "''") + "01.bak\"'\r\n";
				}
				empty2 = getDatabaseDescription(empty);
				text2 = text2 + "BACKUP DATABASE [" + empty + "] TO DISK = " + (text + empty + ".bak").ToSql() + " WITH INIT, DESCRIPTION = " + ("Backup of " + empty2).ToSql();
				addStepToJob(m1JobName, num, "Backup of " + empty2, text2);
			}
			if (num > 0)
			{
				serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_update_jobstep @job_name = " + m1JobName.ToSql() + ", @step_id = " + num + ", @on_success_action = 1, @on_fail_action = 2");
			}
			if (backupInfo.NumberOfDays > 0)
			{
				decimal d = M1Math.Round(backupInfo.JobStartTime * 10000m, 0);
				serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_add_jobschedule @job_name = " + m1JobName.ToSql() + ", @name = " + m1JobName.ToSql() + ", @freq_type = 4, @freq_interval = " + backupInfo.NumberOfDays.ToSql() + ",@active_start_time = " + d.ToSql());
			}
			if (serverManager.IsSQLExpress(null, null, "msdb"))
			{
				createWindowsSchedule(backupInfo.NumberOfDays, backupInfo.JobStartTime);
			}
		}
		return true;
	}

	private bool deleteJob(string jobName)
	{
		try
		{
			serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_delete_job @job_name = " + jobName.ToSql());
			return true;
		}
		catch (Exception ex)
		{
			if (ex.Message.Contains("SQL Server blocked access to procedure 'dbo.sp_delete_job'"))
			{
				serverManager.ExecuteCommand(null, null, "msdb", "Delete msdb.dbo.sysjobschedules From msdb.dbo.sysjobschedules Inner Join msdb.dbo.sysjobs On sysjobs.job_id = sysjobschedules.job_id Where name = " + jobName.ToSql());
				serverManager.ExecuteCommand(null, null, "msdb", "Delete msdb.dbo.sysjobsteps From msdb.dbo.sysjobsteps Inner Join msdb.dbo.sysjobs On sysjobs.job_id = sysjobsteps.job_id Where name = " + jobName.ToSql());
				serverManager.ExecuteCommand(null, null, "msdb", "Delete msdb.dbo.sysjobservers From msdb.dbo.sysjobservers Inner Join msdb.dbo.sysjobs On sysjobs.job_id = sysjobservers.job_id Where name = " + jobName.ToSql());
				serverManager.ExecuteCommand(null, null, "msdb", "Delete msdb.dbo.sysjobhistory From msdb.dbo.sysjobhistory Inner Join msdb.dbo.sysjobs On sysjobs.job_id = sysjobhistory.job_id Where name = " + jobName.ToSql());
				serverManager.ExecuteCommand(null, null, "msdb", "Delete From msdb.dbo.sysjobs Where name = " + jobName.ToSql());
				return true;
			}
			throw;
		}
	}

	private void createJob(string jobName, string jobDescription)
	{
		serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_add_job @job_name = " + jobName.ToSql() + ", @enabled = 1, @description = " + jobDescription.ToSql() + ", @owner_login_name = " + serverManager.ConnectionInfo.SqlUserID.ToSql() + ", @category_name = 'Database Maintenance', @notify_level_eventlog = 2, @notify_level_email = 2, @notify_level_netsend = 2, @notify_level_page = 2");
		serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_add_jobserver @job_name = " + jobName.ToSql() + ", @server_name = '(local)'");
	}

	private void addStepToJob(string jobName, int stepID, string stepName, string command)
	{
		serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_add_jobstep @job_name = " + jobName.ToSql() + ", @step_id = " + stepID + ", @step_name = " + stepName.ToSql() + ", @subsystem = 'TSQL', @command = " + command.ToSql() + ", @on_success_action = 3, @on_fail_action = 3, @retry_attempts = 5, @retry_interval = 5");
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern int GetShortPathName(string LongPath, StringBuilder ShortPath, int BufferSize);

	private void createWindowsSchedule(int numberOfDays, decimal timeOfDay)
	{
		if (numberOfDays <= 0)
		{
			numberOfDays = 1;
		}
		if (timeOfDay <= 0m)
		{
			timeOfDay = default(decimal);
		}
		string text = "schtasks /delete /TN M1BackupTask /F \r \n";
		string longPath = currentContext.Server.Location.AddBackslash() + "M1Backup.exe";
		StringBuilder stringBuilder = new StringBuilder(255);
		GetShortPathName(longPath, stringBuilder, stringBuilder.Capacity);
		string text2 = stringBuilder.ToString();
		string text3 = "00" + (int)timeOfDay;
		text3 = text3.Substring(text3.Length - 2);
		string text4 = "00" + M1Math.Round((timeOfDay - (decimal)(int)timeOfDay) * 100m, 0);
		text4 = text4.Substring(text4.Length - 2);
		string text5 = (new Thread((ThreadStart)delegate
		{
		}).CurrentCulture.ToString().Equals("en-US") ? "MM/dd/yyyy" : "dd/MM/yyyy");
		text = text + "schtasks /create /RU SYSTEM /SC DAILY /MO " + numberOfDays + " /TN M1BackupTask /TR " + text2 + " /ST " + text3 + ":" + text4 + ":00 /SD " + DateTime.Now.ToString(text5);
		string text6 = Path.GetTempFileName() + ".bat";
		File.AppendAllText(text6, text);
		Process process = new Process();
		process.EnableRaisingEvents = false;
		process.StartInfo.FileName = text6;
		process.StartInfo.Verb = "runas";
		process.Start();
		MessageBox.Show("The current database is SQL Server Express Edition, which does not have the SQL Server Agent included. M1 has created a task in the Windows Scheduler to run the backup.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		process.Close();
		File.Delete(text6);
	}

	private DateTime convertSQLToDate(string value)
	{
		if (value == "0")
		{
			return default(DateTime);
		}
		return new DateTime(int.Parse(value.Substring(0, 4)), int.Parse(value.Substring(4, 2)), int.Parse(value.Substring(6, 2)));
	}

	public bool StartBackupJob()
	{
		if (doesJobExist(m1JobName))
		{
			if (serverManager.IsSQLExpress(null, null, string.Empty))
			{
				Process process = new Process();
				process.EnableRaisingEvents = false;
				process.StartInfo.FileName = currentContext.Server.Location.AddBackslash() + "M1Backup.exe";
				process.Start();
				return true;
			}
			if (CheckIfAgentIsRunning())
			{
				serverManager.ExecuteCommand(null, null, "msdb", "EXEC sp_start_job @job_name = " + m1JobName.ToSql());
				return true;
			}
		}
		return false;
	}

	public void CreateBackUp(BackupInfo backupInfo)
	{
		string empty = string.Empty;
		if (backupInfo.Databases.Count > 0)
		{
			foreach (KeyValuePair<string, string> database in backupInfo.Databases)
			{
				empty = "BACKUP DATABASE [" + database.Key + "] TO DISK = " + (backupInfo.Location + database.Value + ".bak").ToSql() + " WITH INIT, DESCRIPTION = " + ("Backup of " + backupInfo.Description).ToSql();
				serverManager.ExecuteCommand(null, null, "msdb", empty);
			}
			return;
		}
		MessageBox.Show("There is no backup database selected.");
	}

	public void RemoveBackup(string imageBackupName)
	{
		if (!imageBackupName.Contains(".bak"))
		{
			imageBackupName += ".bak";
		}
		new ServerFileSystem(serverManager).DeleteFile(imageBackupName);
	}

	public void RestoreBackup(M1User m1User, string backupFile, string oldDatabaseName, string newDatabaseName, int fileNumber)
	{
		newDatabaseName = newDatabaseName.Trim();
		if (newDatabaseName.Length == 0)
		{
			throw new M1Exception("The database name is required.");
		}
		string text = string.Empty;
		string text2 = string.Empty;
		backupFile = backupFile.Trim();
		string text3 = currentContext.Server.IniSettings.Get("DataLocation", "C:\\M1Data\\").AddBackslash();
		DataTable dataTable = serverManager.GetDataTable(null, m1User, "master", 0, "RESTORE FILELISTONLY FROM DISK = " + backupFile.ToSql() + " WITH FILE = " + fileNumber);
		if (dataTable.Rows.Count > 0)
		{
			DataRow[] array = dataTable.Select("type = 'D'");
			if (array.Length != 0)
			{
				text = array[0].Field<string>("LogicalName").Trim();
			}
			array = dataTable.Select("type = 'L'");
			if (array.Length != 0)
			{
				text2 = array[0].Field<string>("LogicalName").Trim();
			}
		}
		if (text.Length == 0 || text2.Length == 0)
		{
			throw new M1Exception("Unable to retrieve logical file name information from the backup file '" + backupFile + "'. The restore will not continue.");
		}
		serverManager.ClearAllPools();
		if (!serverManager.DatabaseCurrentlyInUse(m1User, newDatabaseName))
		{
			try
			{
				serverManager.ExecuteCommand(null, m1User, "master", "RESTORE DATABASE " + newDatabaseName + " FROM DISK = " + backupFile.ToSql() + " WITH FILE = " + fileNumber + ",  MOVE " + text.ToSql() + " TO " + (text3 + newDatabaseName + ".MDF").ToSql() + ",  MOVE " + text2.ToSql() + " TO " + (text3 + newDatabaseName + "_log.LDF").ToSql() + ", REPLACE");
				new Dmo(currentContext, serverManager).SetCompatibilityLevel(null, m1User, newDatabaseName);
			}
			catch (SqlException ex)
			{
				if (ex.Number == 3101)
				{
					throw new M1Exception("Unable to restore database " + newDatabaseName.ToLinq() + ", database cannot be restored to while in use.\n\nDatabase " + newDatabaseName.ToLinq() + " is currently being accessed by the following users.\n" + currentContext.DBServerManager.GetUsersAccessingDatabase(m1User, newDatabaseName));
				}
				throw;
			}
			if (newDatabaseName.StartsWith("M1_", StringComparison.CurrentCultureIgnoreCase))
			{
				currentContext.InstalledDatabases.Refresh();
			}
			else
			{
				currentContext.InstalledDataDictionaries.Refresh();
			}
			return;
		}
		throw new M1Exception("Unable to restore database " + newDatabaseName.ToLinq() + ", database cannot be restored to while in use.\n\nDatabase " + newDatabaseName.ToLinq() + " is currently being accessed by the following users.\n" + serverManager.GetUsersAccessingDatabase(m1User, newDatabaseName));
	}
}
