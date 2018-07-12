using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    Text textfield;

    private void Awake()
    {
        textfield = GetComponent<Text>();

        if (Debug.isDebugBuild)
            gameObject.SetActive(false);
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
      
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = fps.ToString("00"); // string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        textfield.text = text;
    }
}