using System;
using System.Runtime.ExceptionServices;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    public static GameOverUI Instance;

   [Header("Text References")]
   [SerializeField] private TMP_Text _killedEnemies;
   [SerializeField] private TMP_Text _usedBullets;
   [SerializeField] private TMP_Text _usedMedkit;
   [SerializeField] private TMP_Text _maxHeal;
   [SerializeField] private TMP_Text _maxStamina;
   [SerializeField] private TMP_Text _maxDamage;

   [Header("References")]
   [SerializeField] private Button _tryAgainButton;
   [SerializeField] private Button _mainMenuButton;

    private PlayerController _playerController;
    private void Awake()
    {
        Instance = this;
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _tryAgainButton.onClick.AddListener(TryAgainButtonClicked);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
    }

    private void MainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void TryAgainButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Update() 
    {
        SetTexts();    
    }

    private void SetTexts()
    {
        _killedEnemies.text = "KILLED ENEMIES " + SpawnManager.Instance.GetKilledEnemies().ToString();
        _usedBullets.text = "USED BULLET " + GunManager.Instance.GetUsedBullet().ToString();
        _usedMedkit.text = "USED MEDKIT " + _playerController.GetUsedMedkit().ToString();
        _maxHeal.text = "MAX HEAL " +   HealManager.Instance.GetMaxHeal().ToString();
        _maxStamina.text = "MAX STAMINA " +  StaminaManager.Instance.GetMaxStamina().ToString();
        _maxDamage.text = "MAX DAMAGE " + GunManager.Instance.GetBulletDamage().ToString();
    }

    public void OpenGameOverUI()
    {
        this.transform.DOScale(1f, 0.5f).SetEase(Ease.InBack);
    }
}
