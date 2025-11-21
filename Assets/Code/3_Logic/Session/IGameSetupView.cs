using System;

public interface IGameSetupView
{
    public event Action<int> OnKeyPressed;
    public event Action OnResetPressed;
    public event Action OnRollPressed;
    public event Action OnStartGamePressed;
    public event Action OnLeaveRoomPressed;

    public void ShowLobbyView();
}
