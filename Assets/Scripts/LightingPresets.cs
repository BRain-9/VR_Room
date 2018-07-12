using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightingPresets : MonoBehaviour {

    public float timeToChangeLight = 4f;
    public float timeToChangeColor = 20f;
   
    float timerLight = 0;
    float timerColor = 0;

    public Gradient colorGradient;
    public Renderer wallRenderer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (timerLight > timeToChangeLight)
        {
            timerLight = 0;
            LightingScenarioSwitcher.instance.Switch();
        }
        else
            timerLight += Time.deltaTime;

        timerColor += Time.deltaTime;
        wallRenderer.material.color = colorGradient.Evaluate(timerColor / timeToChangeColor);
        if (timerColor > timeToChangeColor)
            timerColor = 0;
	}
}
