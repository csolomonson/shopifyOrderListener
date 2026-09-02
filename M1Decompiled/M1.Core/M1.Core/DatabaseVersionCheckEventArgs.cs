using System;

namespace M1.Core;

public class DatabaseVersionCheckEventArgs : EventArgs
{
	public string DatabaseName;

	public string DatabaseVersion;

	public string DatabaseDescription;

	public string DatabaseExtensionVersions;

	public string AppVersion;

	public string ExtensionVersions;

	public string ExtraMessage;

	public IServiceProvider ServiceProvider;

	public bool Cancel = true;

	public DatabaseVersionCheckEventArgs(IServiceProvider provider, string appVersion, string extensionVersions, string databaseName, string databaseVersion, string databaseExtensionVersions, string databaseDescription, string extraMessage)
	{
		ServiceProvider = provider;
		AppVersion = appVersion;
		ExtensionVersions = extensionVersions;
		DatabaseName = databaseName;
		DatabaseVersion = databaseVersion;
		DatabaseExtensionVersions = databaseExtensionVersions;
		DatabaseDescription = databaseDescription;
		ExtraMessage = extraMessage;
	}
}
