using System;

public interface IGamePlayView
{
    public event Action OnLobbyRoomPressed;
    public event Action OnLeaveRoomPressed;

    public void ShowGameEndView();
}