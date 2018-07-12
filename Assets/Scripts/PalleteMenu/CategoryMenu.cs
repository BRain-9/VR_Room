using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryMenu : MonoBehaviour, IClosebleObj
{
	#region Params
	[SerializeField] private CameraFollow cameraFollow;
    public bool over = false;
    public static bool inItem = false;
    public VRInteractiveItem item;
	private static GameObject itemContainer;
	public static GameObject ItemContainer{
		get { return itemContainer; }
	}

	#endregion

	private void Awake()
	{
	}

	private void OnEnable()
    {
		//cameraFollow = GetComponentInParent<CameraFollow>();
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

		PalleteColor.paletteColorChoosen += ChooseItem;
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
		PalleteColor.paletteColorChoosen -= ChooseItem;

    }

	public void SelectFrame(GameObject go){
		
	}

	public void HandleOver()
    {
        over = true;
		cameraFollow.ry = false;
		this.PrintLog("Ray on inn");
    }

    public void HandleOut()
    {
		if (!inItem)
        {
            over = false;
        }
    }

    public void HandleDown()
    {
		
    }

    public void HandleUp()
    {

    }

	public void ChooseItem(GameObject obj){
		if (obj != null){
			itemContainer = obj;
		}else
        {
			this.PrintLog("Container null");
			itemContainer = null;
		}
	}

	public void ReopenPallete(){
		UI_Manager.Instance.ShowPallete();
		Close();
	}

	public void Close(){
		if (transform.childCount > 0)
		{
			for (int i = 1; i < transform.childCount; i++)
			{
				Destroy(transform.GetChild(i).gameObject);
			}
		}
        gameObject.SetActive(false);
		this.PrintLog("Closing");
	}

}
