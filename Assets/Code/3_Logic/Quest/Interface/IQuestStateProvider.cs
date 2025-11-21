using Fusion;
using System.Collections.Generic;

public interface IQuestStateProvider
{
    int CompletedCount { get; }
    int RequiredCount { get; }
    IReadOnlyDictionary<PlayerRef, int> PlayerAssignments { get; }
    bool IsLevelComplete { get; }
}
