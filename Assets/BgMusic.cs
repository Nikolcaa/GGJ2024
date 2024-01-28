using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgMusic : MonoBehaviour
{
    public static BgMusic instance;

    public AudioSource audioSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnGameLoad()
    {
        audioSource.volume = 0.1f;
    }
}
