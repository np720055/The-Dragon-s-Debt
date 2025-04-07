using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(backgroundMusic!= null)
        {
            PlayerBackgroundMusic(false);
        }
    }

    public void PlayerBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioClip = null;
        }
        else if (audioSource.clip != null)
       
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
            
    }

   
    public void PauseBackgroundMusic()
        {
            audioSource.Pause();
        }
}
