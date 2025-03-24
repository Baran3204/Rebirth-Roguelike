using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    private AudioSource _audioSource;

    [SerializeField] private float _playVolume, _pauseVolume;

    private void Awake() 
    {
        _audioSource = GetComponent<AudioSource>();

        if(Instance != null)
        {
            Destroy(gameObject);
        } 
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        PlayBackgroundMusic(true);
    }
    
    
    public void SetMusicMute(bool isMuted)
    {
        _audioSource.mute = isMuted;
    }

    public void PlayBackgroundMusic(bool isMusicPlaying)
    {
        if (isMusicPlaying && !_audioSource.isPlaying) _audioSource.PlayDelayed(3f);
        else if (!isMusicPlaying) _audioSource.Stop();
    }
}
