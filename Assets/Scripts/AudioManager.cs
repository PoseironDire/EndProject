using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool soundtrack;
    public bool ambience;
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.priority = s.priority;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            ;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        if (soundtrack)
            Play("Soundtrack");
        if (ambience)
            Play("Ambience");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
