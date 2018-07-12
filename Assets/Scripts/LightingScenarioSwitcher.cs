using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingScenarioSwitcher : MonoBehaviour {

    public static LightingScenarioSwitcher instance;

    private LevelLightmapData LocalLevelLightmapData;
    private int LightingScenarioSelector;
    private int lightingScenariosCount;
    [SerializeField]
    public int DefaultLightingScenario;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        LocalLevelLightmapData = FindObjectOfType<LevelLightmapData>();
        LightingScenarioSelector = DefaultLightingScenario;
        lightingScenariosCount = LocalLevelLightmapData.lightingScenariosCount;
        LocalLevelLightmapData.LoadLightingScenario(DefaultLightingScenario);
        Debug.Log("Load default lighting scenario");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (OVRInput.Get (OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Space))        
            Switch();        
    }


    public void Switch() {

        LightingScenarioSelector = LightingScenarioSelector + 1;
        if (LightingScenarioSelector > (lightingScenariosCount - 1))
        {
            LightingScenarioSelector = 0;
        }
        LocalLevelLightmapData.LoadLightingScenario(LightingScenarioSelector);
        Debug.Log("Lighting Scenario " + (LightingScenarioSelector + 1));


    }
}
