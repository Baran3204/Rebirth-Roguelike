using System;
using TMPro;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager Instance;
   [Header("References")]
   [SerializeField] private PlayerController _playerController;
   [SerializeField] private TMP_Text _staminaText;

   [Header("Settings")]
   [SerializeField] private float _maxStamina = 100f;
   [SerializeField] private float _staminaCooldown;
   [SerializeField] private float _staminaİncrease;
   private float currentStamina, currentCooldown;

   private void Awake() 
   {
        Instance = this;
        currentStamina = _maxStamina;  
        currentCooldown = _staminaCooldown;
   }

    private void Update() 
    {
        Staminaİncrease();
        if(currentStamina > 0f)
        {
             _staminaText.text = "Şu anki Stamina \n " + currentStamina.ToString();
        }
        else
        {
            _staminaText.text = "Stamina Kalmadı!";
        }
          
    }
    private void Staminaİncrease()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f)
        {
          currentStamina += _staminaİncrease;
            
          if(currentStamina >= _maxStamina)
          {
              currentStamina = _maxStamina;
          }
           
            currentCooldown += _staminaCooldown;
        }
    }
    public void StaminaDeincrease(float staminaAmount)
    {
        currentStamina -= staminaAmount;
    }
    public float GetStamina()
    {
        return currentStamina;
    }
    
}
