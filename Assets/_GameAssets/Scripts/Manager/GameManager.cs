using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   public enum GameState
   {
      Play, Pause, GameOver
   }
    private GameState _currentGameState = GameState.Play;
    
    public void ChangeState(GameState gameState)
    {
        _currentGameState = gameState;
    }
    
    public GameState GetGameState()
    {
        return _currentGameState;
    }

   private void Awake() 
   {     
        Instance = this;
   }
   
}

