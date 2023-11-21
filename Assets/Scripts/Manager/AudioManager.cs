using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Recorder.OutputPath;
#endif

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // Start is called before the first frame update
    
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioSource WALKSFXSource;
    public AudioMixer mixer;
    public GameObject SettingBackground;

    public Slider bgmSlide;
    public Slider sfxSlide;

    float bgmSlideValue = 1;
    float sfxSlideValue = 1;

    public double fadeOutSeconds = 1.0;
    public bool isFadeOut = false;
    double fadeDeltaTime = 0;

    [Header("BGM")]
    public AudioClip[] BGMClips;

    [Header("SFX")]
    public AudioClip[] SFXClips;
    private void Awake()
    {
       
  
        instance = this;
        sfxSlide.value = PlayerPrefs.GetFloat("SavedSFXVolume",1.0f);
        bgmSlide.value = PlayerPrefs.GetFloat("SavedBGMVolume", 1.0f);

        mixer.SetFloat("BGM", Mathf.Log10(bgmSlide.value) * 20);
        mixer.SetFloat("SFX", Mathf.Log10(sfxSlide.value) * 20);


    }

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeOut)
        {
            fadeDeltaTime += Time.deltaTime;
            if (fadeDeltaTime >= fadeOutSeconds)
            {
                fadeDeltaTime = fadeOutSeconds;
                isFadeOut = false;
            }
            BGMSource.volume = (float)(1.0 - (fadeDeltaTime / fadeOutSeconds));
        }
    }

    public void SetSFX(float sliderVal)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("SavedSFXVolume", sliderVal);
    }

    public void SetBGM(float sliderVal)
    {
        mixer.SetFloat("BGM", Mathf.Log10(sliderVal) * 20);
        PlayerPrefs.SetFloat("SavedBGMVolume", sliderVal);
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
