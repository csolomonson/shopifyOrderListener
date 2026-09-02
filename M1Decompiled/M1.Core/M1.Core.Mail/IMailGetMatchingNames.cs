using System.Collections.Generic;

namespace M1.Core.Mail;

public interface IMailGetMatchingNames
{
	void GetMatchingNames(string name, Dictionary<string, string> searchResults);
}
