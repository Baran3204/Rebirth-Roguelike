using DG.Tweening;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{
    private Animator _playerAnimator;
    private Transform _playerTransform;
    private SpriteRenderer _playerSprite;
    private PlayerController _playerController;

    private void Awake() 
    {
        _playerAnimator = GetComponent<Animator>();  
        _playerTransform = GetComponent<Transform>();  
        _playerSprite = GetComponent<SpriteRenderer>();  
        _playerController = GetComponent<PlayerController>();  
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
        var canFlip = _playerController.GetCanFlip();

        if(canFlip) { _playerSprite.flipX = false; }
        else if(!canFlip) { _playerSprite.flipX = true; }
    }
}
