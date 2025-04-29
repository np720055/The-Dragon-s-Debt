using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);

        }
        
    }

    void Start()
    {
        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged();  });
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public  void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
    }
    public static void PlayAtPosition(string soundName, Vector3 position)
    {
        AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
    void PlayFootStep()
    {
        SoundEffectManager.PlayAtPosition("FootSteps", transform.position);
    }
    public static void PlayShieldBlock(Vector3 position)
    {
      
        AudioClip shieldBlockClip = soundEffectLibrary.GetClipByName("ShieldBlock");

        if (shieldBlockClip != null)
        {
            AudioSource.PlayClipAtPoint(shieldBlockClip, position);
        }
        else
        {
            Debug.LogWarning("Shield Block sound clip is missing from SoundEffectLibrary.");
        }
    }

}
