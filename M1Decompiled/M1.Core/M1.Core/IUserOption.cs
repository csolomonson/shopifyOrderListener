namespace M1.Core;

public interface IUserOption
{
	void ApplyChanges();

	void InitData(string userID, M1UserSettings settings);
}
