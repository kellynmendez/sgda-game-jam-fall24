using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance = null;

    AudioSource _audioSource;

    bool _songIsPlaying;

    private void Awake()
    {
        _songIsPlaying = false;

        #region Singleton Pattern
        if (Instance == null)
        {
            // doesn't exist yet, this is now our singleton
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // fill references
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    public void PlaySong(AudioClip clip)
    {
        if (!_songIsPlaying)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
            _songIsPlaying = true;
        }
    }
}
