using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera _camera;

    [Header("Settings")]
    [SerializeField] private float _playerSpeed;

    [Header("Shift Settings")]
    [SerializeField] private float _shiftSpeed;
    [SerializeField] private bool _canShift;
    [SerializeField] private KeyCode _shiftKey;
    
    private Vector2 _movementDirection;
    private Rigidbody2D _playerRigidbody;
    private BoxCollider2D _playerCollider;
    private float _horizontal, _vertical, _usedMedkit;
    private bool _canFlip;
    private void Awake() 
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _usedMedkit = 0f;
        GameManager.Instance.ChangeState(GameManager.GameState.Play);
    }
    private void Update() 
    {
        var currentState = GameManager.Instance.GetGameState();

          if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver)
          {
            PlayerCanFlip();
            Setİnputs();
            SetPlayerState();  
            SetShifting();
            SetPlayerSpeed();
            SetPlayerTrigger();
          }
             
    }

    private void FixedUpdate() 
    {
        var currentState = GameManager.Instance.GetGameState();

        if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver)
        {
           SetPlayerMovement(); 
        }
           
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
            _ when movementDirection != Vector2.zero && IsShifting => PlayerState.Shift,
            _ => PlayerState.Idle
        };

        if(newState != currentState)
        {
            PlayerStateController.Instance.ChangePlayerState(newState);
        }
    }

    private void SetPlayerTrigger()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();

        _playerCollider.isTrigger = currentState switch
        {
            _ when currentState == PlayerState.Idle => false,
            _ when currentState == PlayerState.Walk => false,
            _ when currentState == PlayerState.Shift => true,
            _ => false 
        };
    }
    private void SetPlayerSpeed()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();
        var currentStamina = PlayerStateController.Instance.GetPlayerState();

        if(currentStamina > 0f)
        {
            var newSpeed = currentState switch
            {
                _ when currentState == PlayerState.Shift => _shiftSpeed,
                _ => 200f
            };
      
            _playerSpeed = newSpeed;
        }
        else
        {
            _playerSpeed = 200f;
        }
       
    }

    private void SetShifting()
    {   
        if(Input.GetKey(_shiftKey))
        {      
            var currentStamina = StaminaManager.Instance.GetStamina();
        
            var currentCanShift = GetCanShift();

            if(!currentCanShift && currentStamina >= 3f)
            {
                _canShift = true;
                _camera.Lens.OrthographicSize = 9f;
            }
            else if(currentCanShift && currentStamina <= 0f)
            {
                _canShift = false;
                _camera.Lens.OrthographicSize = 8f;
            }    
        }
        if(Input.GetKeyUp(_shiftKey) && _canShift)
        {
            _canShift = false;
            _camera.Lens.OrthographicSize = 8f;
        }
        
    } 

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Medkit")) 
        {
            other.gameObject.TryGetComponent<IHealables>(out IHealables component);
            component.Heal();
            AudioManager.Instance.Play(SoundType.Collect);
            _usedMedkit++;
            Destroy(other.gameObject);
        }   
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

   #region Helper Funcionts

    public bool GetCanShift()
    {
        return _canShift;
    }
    public bool GetCanFlip()
    {
        return _canFlip;
    }


    private void StateWorking()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();
        if(currentState == PlayerState.Idle) { Debug.Log("IDLE"); }
        else if(currentState == PlayerState.Walk) { Debug.Log("WALK"); }
        else if(currentState == PlayerState.Shift) { Debug.Log("SHİFT"); }
    }
    public float GetUsedMedkit()
    {
        return _usedMedkit;
    }
    
    #endregion
}
