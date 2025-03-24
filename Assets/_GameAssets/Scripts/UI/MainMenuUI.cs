using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class MainMenuUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    private void Awake() 
    {
        _startButton.onClick.AddListener(StartButtonClicked);    
        _quitButton.onClick.AddListener(QuitButtonClicked);    
    }

    private void QuitButtonClicked()
    {
        AudioManager.Instance.Play(SoundType.ButtonClick);
        Application.Quit();
    }

    private void StartButtonClicked()
    {  
        AudioManager.Instance.Play(SoundType.ButtonClick);      
        SceneManager.LoadScene("GameScene");       
    }
}
