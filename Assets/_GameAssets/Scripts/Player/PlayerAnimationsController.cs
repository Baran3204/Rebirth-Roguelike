using DG.Tweening;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Animator _playerAnimator;
    private Transform _playerTransform;
    private SpriteRenderer _playerSprite;
    private bool _isLeft;

    private void Awake() 
    {
        _playerAnimator = GetComponent<Animator>();  
        _playerTransform = GetComponent<Transform>();  
        _playerSprite = GetComponent<SpriteRenderer>();  
    }
    
    private void Update() 
    {
        SetPlayerFlip();
    }

    private void FixedUpdate() 
    {
       var currentState = PlayerStateController.Instance.GetPlayerState();

       switch(currentState)
       {
        case PlayerState.Idle:
            _playerAnimator.SetBool("isMove", false);
            break;
        case PlayerState.Walk:
            _playerAnimator.SetBool("isMove", true);
            break;
       }    
    }

    private void SetPlayerFlip()
    {
        var playerX = _playerTransform.position.x;
        if(playerX < playerX + _playerTransform.position.x)
        {
            _playerSprite.flipX = false;
        }
        else if(playerX > playerX +_playerTransform.position.x)
        {
            _playerSprite.flipX = true;
        }
        
    }
}
