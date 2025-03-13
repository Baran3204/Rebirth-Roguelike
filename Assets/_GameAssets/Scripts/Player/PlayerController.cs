using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [Header("Settings")]
    [SerializeField] private float _playerSpeed;

    [Header("Shift Settings")]
    [SerializeField] private float _shiftDrag;
    [SerializeField] private float _shiftSpeed;
    [SerializeField] private float _shiftCooldown;
    [SerializeField] private bool _canShift;
    [SerializeField] private KeyCode _shiftKey;
    
    private Vector2 _movementDirection;
    private Rigidbody2D _playerRigidbody;
    private BoxCollider2D _playerCollider;
    private float _horizontal, _vertical, coolDown;
    private bool _canFlip;
    private void Awake() 
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
         coolDown = _shiftCooldown;
    }
    private void Update() 
    {
        PlayerCanFlip();
        Setİnputs(); 
        SetPlayerState();
        SetShifting();
    }

    private void FixedUpdate() 
    {
        StateWorking(); 
        SetPlayerMovement();    
    }
    private void SetPlayerMovement()
    {
        
        _movementDirection = transform.up * _vertical + transform.right * _horizontal;

        _playerRigidbody.linearVelocity = _movementDirection * _playerSpeed * Time.deltaTime;
    }
    private void Setİnputs()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
    }

    private void SetPlayerState()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();
        var movementDirection = _movementDirection.normalized;
        var IsShifting = GetCanShift();
        var newState = currentState switch 
        {
            _ when movementDirection == Vector2.zero && !IsShifting => PlayerState.Idle,
            _ when movementDirection != Vector2.zero && !IsShifting => PlayerState.Walk,
            _ when movementDirection == Vector2.zero && IsShifting => PlayerState.Shift,
            _ => PlayerState.Idle
        };

        if(newState != currentState)
        {
            PlayerStateController.Instance.ChangePlayerState(newState);
        }
    }

    private void SetPlayerDrag()
    {
        PlayerState currentState = PlayerStateController.Instance.GetPlayerState();

        _playerRigidbody.linearDamping = currentState switch
        {
            _ when currentState == PlayerState.Idle => 5f,
            _ when currentState == PlayerState.Walk => 5f,
            _ when currentState == PlayerState.Shift => 0f,
            _ => 5f
        };
    }
    private void SetShifting()
    {  
        if(Input.GetKeyDown(_shiftKey) && !_canShift)
        {
            var currentStamina = StaminaManager.Instance.GetStamina();
           
            if(currentStamina >= 30f)
            {
                StaminaManager.Instance.StaminaDeincrease(30f);

                _canShift = true;

                _playerRigidbody.AddForce((transform.up * _vertical + transform.right * _horizontal) * _shiftSpeed, ForceMode2D.Force);

                coolDown -= Time.deltaTime;
                if(coolDown <= 0f)
                {
                    _canShift = false;
                    coolDown += _shiftCooldown;                   
                }
            }
        }
    } 

   #region Helper Funcionts

    private bool GetCanShift()
    {
        return _canShift;
    }
    public bool GetCanFlip()
    {
        return _canFlip;
    }

    private void PlayerCanFlip()
    {   
        if(_horizontal > 0f)
        {
            _canFlip = true;
        }
        else if(_horizontal < 0f)
        {
            _canFlip = false;
        }
    }

    private void StateWorking()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();
        if(currentState == PlayerState.Idle) { Debug.Log("IDLE"); }
        else if(currentState == PlayerState.Walk) { Debug.Log("WALK"); }
        else if(currentState == PlayerState.Shift) { Debug.Log("SHİFT"); }
    }
    #endregion
}
