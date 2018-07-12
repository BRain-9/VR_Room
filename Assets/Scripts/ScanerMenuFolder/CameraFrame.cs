using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraFrame : MonoBehaviour {

	Image myBody;

	private void Awake()
	{
		myBody = this.gameObject.GetComponentInChildren<Image>();
	}

	private void OnEnable()
	{
		myBody.color = Color.white;
	}
}
