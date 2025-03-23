using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;

   [Header("References")]
   [SerializeField] private RectTransform _upgradeUI;
   [SerializeField] private GameObject _block;
   [SerializeField] private Button _staminaButton;
   [SerializeField] private Button _healButton;
   [SerializeField] private Button _damageButton;

   [Header("Stamina Settings")]
   [SerializeField] private float _staminaİncrease;
   [SerializeField] private float _staminaPlus;
   [SerializeField] private TMP_Text _staminaText;

   [Header("Heal Settings")]
   [SerializeField] private float _healİncrease;
   [SerializeField] private float _healPlus;
   [SerializeField] private TMP_Text _healText;


   [Header("Damages Settings")]
   [SerializeField] private float _damageİncrease;
   [SerializeField] private float _damagePlus;
   [SerializeField] private TMP_Text _damageText;


   [Header("Global Settings")]
   [SerializeField] private float _coolDown;

   [SerializeField] private int _currentUpgrade;
   private bool _isBlock;

   private void Awake() 
   {
        Instance = this;
        _staminaButton.onClick.AddListener(StaminaButtonClicked);
        _healButton.onClick.AddListener(HealButtonClicked);
        _damageButton.onClick.AddListener(DamageButtonClicked);

        UpdateTexts();
   }
    private void Update() 
    {
          if(_isBlock)
          {
               _coolDown -= Time.deltaTime;
               if(_coolDown <= 0f)
               {
                    _block.SetActive(false);
                    _isBlock = false;
               }
          }   
    }
    private void StaminaButtonClicked()
    {
       
       StaminaManager.Instance.UpdateMaxStamina(_staminaİncrease);
       _staminaİncrease += _staminaPlus;

       CloseUpgradeUI();
       UpdateTexts();
       _currentUpgrade++;
    }

    private void HealButtonClicked()
    {
       
       HealManager.Instance.UpdateMaxHeal(_healİncrease);
       _healİncrease += _healPlus;
       
       CloseUpgradeUI();
       UpdateTexts();
      _currentUpgrade++;
    }

    private void DamageButtonClicked()
    {
       
       Bullet.Instance.SetBulletDamage(_damageİncrease);
       _damageİncrease += _damagePlus;
       
       CloseUpgradeUI();
       UpdateTexts();
      _currentUpgrade++;
    }

   public void OpenUpgradeUI()
   {
        _upgradeUI.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        _block.SetActive(true);
        _isBlock = true;
        GameManager.Instance.ChangeState(GameManager.GameState.Pause);

   }

   public void CloseUpgradeUI()
   {
        _upgradeUI.DOScale(0f, 0.3f).SetEase(Ease.InBack);
        GameManager.Instance.ChangeState(GameManager.GameState.Play);
   }


     public int GetCurrentUpgrade()
     {
          return _currentUpgrade;
     }
   private void UpdateTexts()
   {
     _staminaText.text = "STAMİNA LİMİT +" + _staminaİncrease.ToString() + "!";
     _healText.text = "HEAL LİMİT +" + _healİncrease.ToString() + "!";
     _damageText.text = "DAMAGE LİMİT +" + _damageİncrease.ToString() + "!";
   }
}
