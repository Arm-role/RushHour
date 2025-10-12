using System;

public interface IGameLauncherView
{
    public event Action<int> OnKeyPressed;
    public event Action OnResetPressed;
    public event Action OnRollPressed;
    public event Action OnCreateRoomPressed;
    public event Action OnJoinRoomPressed;
    public event Action OnStartGamePressed;
    public event Action OnLeaveRoomPressed;


    public void ShowLobbyView();
    public void UpdatePlayerCount(int playerCount, int maxPlayers);
}
