using UnityEngine;
using UnityEngine.UI;

public class HealManager : MonoBehaviour
{
    public static HealManager Instance;

    [Header("References")]
    [SerializeField] private Slider _healSlider;

    [Header("Settings")]
    [SerializeField] private float _maxHeal;

   
    private float _currentHeal;

    private void Awake() 
    {
        Instance = this; 
        _currentHeal = _maxHeal; 
    }

    private void Update() 
    {
        SetSlider();
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
            _currentHeal -= damageAmount;
            if(_currentHeal <= 0f)
            {
                _currentHeal = 0f;
            }
        }
       
    }

    public void Heal(float healAmaount)
    {
        _currentHeal += healAmaount;
    }
    public float GetCurrentHeal()
    {
        return _currentHeal;
    }
}
