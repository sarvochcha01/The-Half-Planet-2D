using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 100)]
    public int volume;
    [Range (-3, 3)]
    public int pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}