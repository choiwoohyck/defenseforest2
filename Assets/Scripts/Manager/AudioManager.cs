using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Start is called before the first frame update
    
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioSource WALKSFXSource;


    [Header("BGM")]
    public AudioClip[] BGMClips;

    [Header("SPX")]
    public AudioClip[] SFXClips;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBGM(int BGMNum)
    {
        BGMSource.clip = BGMClips[BGMNum];
        BGMSource.loop = true;
        BGMSource.Play();
    }
    
    public void StopBGM()
    {
        BGMSource.Stop();
    }

    public void PlayOnShotSFX(int SFXNum)
    {
        SFXSource.clip = SFXClips[SFXNum];
        SFXSource.PlayOneShot(SFXSource.clip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public void PlayOnShotWALKSFX()
    {
        if (!WALKSFXSource.isPlaying)
        {
            WALKSFXSource.clip = SFXClips[4];
            WALKSFXSource.PlayOneShot(WALKSFXSource.clip);
        }
    }

}
