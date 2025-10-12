public interface IQuestStateProvider
{
    int GetCompletedCount();
    int GetRequiredCount();
    int GetPlayerAssignment(int playerId);
}
