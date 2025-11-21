using Fusion;
using System.Collections.Generic;

public interface IQuestService
{
    void SetupLevel(LevelData levelData, List<PlayerRef> players);
    bool SubmitMenu(PlayerRef playerId, int menuId);
    void AssignNewQuestToPlayer(PlayerRef playerId);
}