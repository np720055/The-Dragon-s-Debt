using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSoure audioSource;
    public AudioClip backgroundMusic;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance == this;
            audioSource = GetComponent<AudioSource>();
            DontDestoryOnLoad(gameObject);
        }
        else
        {
            Destory(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayerBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource = audioClip;
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
