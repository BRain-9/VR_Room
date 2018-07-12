using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraFollow : MonoBehaviour
{
	#region Params

	[SerializeField] private CameraFollow cameraFollow;
	public bool over = false;
	public static bool inItem = false;
	public VRInteractiveItem item;

    #endregion

	private void OnEnable()
    {
        this.PrintLog("CameraFollow is " + cameraFollow.gameObject.name);
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
	public void HandleOver()
    {
        over = true;
        this.PrintLog("Ray Hit");
		cameraFollow.ry = false;
        
    }

    public void HandleOut()
    {
        over = false;
		cameraFollow.ry = true;
        
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    } 

}
