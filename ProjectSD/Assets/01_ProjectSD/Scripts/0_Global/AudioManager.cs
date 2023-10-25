using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public Sound[] loopSounds;

    public bool canPlay = true;
    public float delay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("TitleBGM");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }

        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("SFX Not Found");
        }

        else
        {
            PlayOneShot(sound);
        }

    }
    public void PlayOneShot(Sound sound)
    {
        if(canPlay)
        {
            canPlay = false;
            sfxSource.PlayOneShot(sound.clip);

            StartCoroutine(ResetSound());
        }
    }
    IEnumerator ResetSound()
    {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }

    public void PlayLoopSound(string name)
    {
        Sound sound = Array.Find(loopSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("SFX Not Found");
        }

        else
        {
            sfxSource.clip = sound.clip;
            sfxSource.loop = true; // 루프 설정
            sfxSource.Play(); // 루프로 재생 시작
        }

    }

    


    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }


}