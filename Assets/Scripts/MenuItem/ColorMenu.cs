using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorMenu : MonoBehaviour, IClosebleObj {
	#region Params
	public CameraFollow cameraFollow;

	public bool over = false;

    public static bool inItem = false;

    public VRInteractiveItem item;

    #endregion

	private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();
        if (item == null)
        {
            Debug.Log("VRInteractiveItem not found at " + gameObject.name);
        }
        else
        {
            item.OnOver += HandleOver;
            item.OnOut += HandleOut;

            item.OnDown += HandleDown;
            item.OnUp += HandleUp;
        }

        //_dataAsset.Init(this);
    }

    private void OnDisable()
    {
        if (item != null)
        {
            item.OnDown -= HandleDown;
            item.OnUp -= HandleUp;

            item.OnOver -= HandleOver;
            item.OnOut -= HandleOut;
        }
    }
    
	public void Close(){
		
		this.gameObject.SetActive(false);
	}

	public void HandleOver()
    {
        over = true;
        cameraFollow.ry = false;
    }

    public void HandleOut()
    {
        over = false;
        if (!inItem) {
            cameraFollow.ry = true;
        }
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    }

}
