using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Start is called before the first frame update
    
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioSource WALKSFXSource;
    public AudioMixer mixer;


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

    public void SetSFX(float sliderVal)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderVal) * 20);
    }

    public void SetBGM(float sliderVal)
    {
        mixer.SetFloat("BGM", Mathf.Log10(sliderVal) * 20);
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
