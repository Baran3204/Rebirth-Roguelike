using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public static PlayerStateController Instance;
    private PlayerState _playerState = PlayerState.Idle;

    private void Awake() 
    {
        Instance = this;
        ChangePlayerState(PlayerState.Idle);
    }
    public void ChangePlayerState(PlayerState newState)
    {
        if(_playerState != newState) { _playerState = newState; }
        else return;
    }

    public PlayerState GetPlayerState()
    {
        return _playerState;
    }
}
