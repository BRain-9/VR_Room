using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    InterfaceController ui;
    SoundController sound;

    private void Awake()
    {
        ui.GetComponent<InterfaceController>();
        sound.GetComponent<SoundController>();
    }

    void Start () {
		
	}
	
	void Update () {
		
	}
}
