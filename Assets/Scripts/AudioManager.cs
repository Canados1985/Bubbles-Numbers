using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    GameManager gameManagerRef;

    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }


            DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {
        gameManagerRef = GameObject.Find("GameManager").GetComponent<GameManager>();
    }



        public void Play(string name)
        {
            if(!gameManagerRef.mute)
            {
                Sound s = Array.Find(sounds, sound => sound.name == name);
                s.source.Play();
            }

        }
        public void Stop(string name)
        {
            if(!gameManagerRef.mute)
            {
                Sound s = Array.Find(sounds, sound => sound.name == name);
                s.source.Stop();
            }
        }
        public void Pause(string name)
        {
            if(!gameManagerRef.mute)
            {
                Sound s = Array.Find(sounds, sound => sound.name == name);
                s.source.Pause();
            }


        }
        public void Unpause(string name)
        {
            if(!gameManagerRef.mute)
            {
                Sound s = Array.Find(sounds, sound => sound.name == name);
                s.source.UnPause();
            }

        }
    

}
