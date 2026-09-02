using System;

namespace M1.Core.Integrations;

public class M1CloudCredentials
{
	public Guid CompanyId { get; set; }

	public string Username { get; set; }

	public string EncryptedPassword { get; set; }
}
