using System;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance;
   [Header("References")]
   [SerializeField] private PlayerController _playerController;
   [SerializeField] private Slider _staminaSlider;

   [Header("Settings")]
   [SerializeField] private float _maxStamina = 100f;
   [SerializeField] private float _increaseCooldown;
   [SerializeField] private float _deincreaseCooldown;
   [SerializeField] private float _idleStaminaİncrease;
   [SerializeField] private float _movementStaminaİncrease;
   private float currentStamina, currentİncreaseCooldown, currentDeincreaseCooldown;

   private void Awake() 
   {
        Instance = this;
        currentStamina = _maxStamina;  
        currentİncreaseCooldown = _increaseCooldown;
        currentDeincreaseCooldown = _deincreaseCooldown;
   }

   private void Update() 
    {
        Staminaİncrease();
        SetStaminaDeinscrease(); 
        SetStaminaSlider();   
    }


    private void SetStaminaDeinscrease()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();

        float deinscrease = currentState switch
        {
            _ when currentState == PlayerState.Shift => 3f,
            _ => 0f
        };
        currentDeincreaseCooldown -= Time.deltaTime;

        if(currentDeincreaseCooldown <= 0f)
        {
            if(currentStamina > 0f)
            {
                currentStamina -= deinscrease;
            }
             if(currentStamina <= 0f)
             {
                currentStamina = 0f;
             }
        currentDeincreaseCooldown += _deincreaseCooldown;
        }
     
    }
    
    private void Staminaİncrease()
    {
        var currentState = PlayerStateController.Instance.GetPlayerState();
        var increase = currentState switch
        {
            _ when currentState == PlayerState.Idle => _idleStaminaİncrease,
            _ when currentState == PlayerState.Walk => _movementStaminaİncrease,
            _ when currentState == PlayerState.Shift => 0f,
            _ => 0f
        };
        currentİncreaseCooldown -= Time.deltaTime;
        if(currentİncreaseCooldown <= 0f)
        {
          currentStamina += increase;
            
          if(currentStamina >= _maxStamina)
          {
              currentStamina = _maxStamina;
          }
           
            currentİncreaseCooldown += _increaseCooldown;
        }
    }
   
   private void SetStaminaSlider()
   {
        _staminaSlider.maxValue = _maxStamina;
        _staminaSlider.value = currentStamina;
   }
   public float GetStamina()
   {
       return currentStamina;
   }
    
}
