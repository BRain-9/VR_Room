using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    #region Params
    [SerializeField] private AudioClip[] audioClips;
    private static AudioManager _instance;
    public static AudioManager Instance {
        get {return _instance; }
    }
    public AudioSource player;
    #endregion
    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        player = GetComponent<AudioSource>();
    }
    public void PlayClip(string name) {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == name) {
                player.clip = audioClips[i];
                player.Play();
            }
        }
        
    }
}
