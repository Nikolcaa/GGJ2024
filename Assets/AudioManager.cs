using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            SetNewSound(s);
        }
    }

    private void Start()
    {
        //Play("Theme"); -- do this in Gamemanager instead
    }

    private void SetNewSound(Sound s)
    {
        s.source = gameObject.AddComponent<AudioSource>();
        s.source.clip = s.clip;
        s.source.outputAudioMixerGroup = s.output;

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.volume = volume;
        s.source.Play();
    }

    public void Play(string name, float minPitch, float maxPitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        s.source.Play();
    }

    public void Play(string name, float max, float amount, bool infinity)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
        if (!infinity)
        {
            if (s.source.pitch + amount < max)
                s.source.pitch += amount;
        }
        else
            s.source.pitch += amount;
    }

    public void StopPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s.source.isPlaying)
            s.source.Stop();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public void ResetPitchToOne(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = 1;
    }
}