using System;
using UnityEngine;
using UnityEngine.UI;

public class HealManager : MonoBehaviour
{
    public static HealManager Instance;

    [Header("References")]
    [SerializeField] private Slider _healSlider;
    [SerializeField] private ParticleSystem _damageParticle;
    [Header("Settings")]
    [SerializeField] private float _maxHeal;
   
    private float _currentHeal;

    private void Awake() 
    {
        Instance = this; 
        _currentHeal = _maxHeal; 
    }

    public void Healİncrease(float healAmount)
    {
        if(_currentHeal != _maxHeal)
        {
            _currentHeal += healAmount;
            if(_currentHeal >= _maxHeal)
            {
                _currentHeal = _maxHeal;
            }
        }
    }

    private void Update() 
    {
         var currentState = GameManager.Instance.GetGameState();

        if(currentState != GameManager.GameState.Pause && currentState != GameManager.GameState.GameOver) SetSlider();
    }

    private void SetSlider()
    {
        _healSlider.maxValue = _maxHeal;
        _healSlider.value = _currentHeal;     
    }
    public void Damage(float damageAmount)
    {
        if(_currentHeal >= 0f)
        {
            _damageParticle.Play();
            
            _currentHeal -= damageAmount;

            if(_currentHeal <= 0f)
            {
                GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
            }
        }
       
    }

    
    public float GetCurrentHeal()
    {
        return _currentHeal;
    }
}
