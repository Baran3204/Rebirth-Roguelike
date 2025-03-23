using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpManager : MonoBehaviour
{
   public static XpManager Instance;
   
   [Header("References")]
   [SerializeField] private Slider _xpSlider;

   [Header("Settings")]
   [SerializeField] private float _xpBorder;
   [SerializeField] private float _currentXp;
   [SerializeField] private float _xpDecreaseAmount;
   [SerializeField] private float _xpDecreasePlus;

   [SerializeField] private int _minXp;
   [SerializeField] private int _maxXp;
   
    private void Awake() 
    {
        Instance = this;
        _currentXp = 0f;    
    }
    private void Update() 
    {
        _xpSlider.maxValue = _xpBorder;
        _xpSlider.value = _currentXp;
    }

    public void Xpİncrease()
    {   
        float xp = Random.Range(_minXp, _maxXp + 1);
        _currentXp += xp;

        if(_currentXp >= _xpBorder)
        {
            UpgradeUI.Instance.OpenUpgradeUI();
            ResetXp(_xpDecreaseAmount);
            _xpDecreaseAmount += _xpDecreasePlus;
        }
    }

    public void ResetXp(float increaseAmount)
    {
        _currentXp = 0f;
        _xpBorder += increaseAmount;

    }
    public float GetCurrentXp()
    {
        return _currentXp;
    }

}
