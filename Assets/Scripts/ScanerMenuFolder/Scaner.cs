using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;

public class Scaner : MonoBehaviour, IClosebleObj {
	#region Params
	private IScanner BarcodeScanner;
    public Text TextHeader;
    public RawImage Image;
    public AudioSource Audio;
    private float RestartTime, frameTime = 1f;
	public static event Action<string,Text> barcodeEvent;
#endregion
	// Disable Screen Rotation on that screen
	void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
		BarcodeScanner = new Scanner();
        
    }

	private void OnEnable()
	{
		BarcodeScanner.Camera.Play();
		BarcodeScanner.OnReady += (sender, arg) => {
            // Set Orientation & Texture
            Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            Image.transform.localScale = BarcodeScanner.Camera.GetScale();
            Image.texture = BarcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            var rect = Image.GetComponent<RectTransform>();
            var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
            RestartTime = Time.realtimeSinceStartup;
        };
		StartCoroutine(ScanCoroutine(frameTime));
	}

	private void Start(){
		StartCoroutine(ScanCoroutine(frameTime));
	}

	private void Update()
	{
		if (BarcodeScanner == null)
        {
            return;
        }
		if (this.Image.isActiveAndEnabled)
            BarcodeScanner.Update();
	}

	private IEnumerator ScanCoroutine(float waitTime){
		yield return new WaitForSeconds(waitTime);
		if (BarcodeScanner == null)
        {
			this.PrintLog("No valid camera - Click Start");
			yield break;
        }
        BarcodeScanner.Scan((barCodeType, barCodeValue) => {
            BarcodeScanner.Stop();
			if (barcodeEvent != null)
                barcodeEvent.Invoke(barCodeValue, TextHeader);
        });
		yield return StartCoroutine(ScanCoroutine(waitTime));
	}

	public void Close(){
		this.gameObject.SetActive(false);
	}

}
