using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudioManager : MonoBehaviour
{
    public Sound[] sounds;

    [SerializeField] private static BGMAudioManager Instance;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Update()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        UpdateVol(gameManager.bgmVol);
    }

    public void UpdateVol(float vol)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = vol / 100;
        }
    }

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Play();
    }

    public void ResetSound()
    {
        AudioSource[] sources = GetComponentsInChildren<AudioSource>();

        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
    }
}
