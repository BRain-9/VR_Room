using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundController instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.


    void Awake()
    {
        instance = this;
    }


    public void PlayFx(AudioClip clip)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
    
        efxSource.pitch = randomPitch;
        efxSource.clip = clip;
        
        efxSource.Play();
    }


    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;

        musicSource.Play();
    }


}
