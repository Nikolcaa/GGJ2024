using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup output;

    public float volume = 1;
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}