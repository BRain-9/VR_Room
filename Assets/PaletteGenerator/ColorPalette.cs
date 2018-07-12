using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorPalette : MonoBehaviour {

    public GameObject cellPrefab;
    public GameObject groupPrefab;
    public Transform currentGroupContainer;
    public Transform paletteContainer;

    public bool draw = true;

    // Use this for initialization
    void Start () {

        ClearGroup(currentGroupContainer);
		
	}

    void ClearGroup(Transform gr) {

        foreach (Transform t in gr)
            DestroyImmediate(t.gameObject);

    }
	
	// Update is called once per frame
	void Update () {
        if (draw)
            DrawPalette();
	}
    
    public int hGroupsCount = 6;
    public int hCellsInGroupCount = 6;
    public int vCellsInGroupCount = 6;

    public float saturationDelta = 0.1f; // for customization first line

    public bool logPalette = false;

    float m_Hue;
    float m_Saturation;
    float m_Value;

    public void DrawPalette() {

        if (logPalette)
            ClearConsole();

        while (paletteContainer.childCount > 0)
            ClearGroup(paletteContainer);

        int totalCellsHorizontal = hGroupsCount * hCellsInGroupCount;
        float hstep = 1f / (float)totalCellsHorizontal;

        for (int groupsLineId = 0 ; groupsLineId < 3; groupsLineId++)   // 3 horizontal groups
        {            
            float m_SaturationMax = 1f - groupsLineId / 3f ;  // different max saturation value for each horizontal set of groups
            float m_ValueMax = groupsLineId / 3f;

            for (int hGroup = 0; hGroup < hGroupsCount; hGroup++)  // horizontal group sets
            {
                GameObject group = Instantiate(groupPrefab, paletteContainer, false);
                currentGroupContainer = group.transform;

                while (currentGroupContainer.childCount > 0)
                    ClearGroup(currentGroupContainer);

                int startCellId = hGroup * hCellsInGroupCount;
                int endCellId = startCellId + hCellsInGroupCount;

                for (int vertId = 0; vertId < vCellsInGroupCount; vertId++)  // column of lines
                {                  

                    for (int horizId = startCellId; horizId < endCellId; horizId++)   // horizontal line
                    {
                        
                        float vProgress = (((float)vertId) / (float)vCellsInGroupCount);

                        m_Hue = hstep * horizId;
                        m_Value = Mathf.Pow(1f - vProgress *  m_ValueMax , 2f) ;
                        m_Saturation = saturationDelta * groupsLineId + (((float)vertId) / (float)vCellsInGroupCount)  * m_SaturationMax;                        

                        Color drawColor = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);
                        GameObject cell = Instantiate(cellPrefab, currentGroupContainer, false);
                        cell.GetComponent<Image>().color = drawColor; // finally!

                        if (logPalette)
                            Debug.Log("m_Hue, m_Saturation, m_Value : " + m_Hue + " / " + m_Saturation + " / " + m_Value);
                    }

                    if (logPalette)
                        Debug.Log("---- line ----");

                }
                if (logPalette)
                    Debug.Log("---- group ----");
            }
            if (logPalette)
                Debug.Log("---- set ----");
        } 

        draw = false;
    }


    static void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }
}
