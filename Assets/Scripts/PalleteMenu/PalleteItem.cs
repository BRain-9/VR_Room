using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PalleteItem : MonoBehaviour{

	#region Params
	private CameraFollow cameraFollow;
	ReticleImg _reticle;

	public GameObject colorItemPrefab;
	private int _chilCount;
	public Image[] palleteImgItems;

	public Image loader;
	public static event System.Action<GameObject> paletteItemChoosen;

    public float value;
    public float timePast = 0f;
    public float clickTimer = 2f;

    public bool over = false;
    public VRInteractiveItem item;
    private bool clicked = false;

	private Color _enabledColor, _disabledColor;

	Coroutine loaderCoroutine = null;
	#endregion

	private void Awake()
    {
        cameraFollow = GetComponentInParent<CameraFollow>();
        _reticle = FindObjectOfType<ReticleImg>();
        loader = _reticle.transform.GetChild(0).gameObject.GetComponent<Image>();
		item = GetComponent<VRInteractiveItem>();

		_enabledColor = this.gameObject.GetComponent<Image>().color;
		_disabledColor = Color.white;

		this.gameObject.GetComponent<Image>().color = _disabledColor;
    }

	private void OnEnable()
    {

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

		this.gameObject.GetComponent<Image>().color = _disabledColor;
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

    public void AddColorItems(List<Vector3> itemsList){
		GameObject go = null;
		Color tempColor;
		foreach (var item in itemsList)
		{
			tempColor = Color.HSVToRGB(item.x,item.y,item.z);
			go = Instantiate(colorItemPrefab,this.transform);
			go.GetComponent<Image>().color = tempColor;
		}
		_chilCount = this.transform.childCount;
        palleteImgItems = new Image[_chilCount];
		for (int i = 0; i < _chilCount; i++)
        {
            palleteImgItems[i] = this.transform.GetChild(i).gameObject.GetComponent<Image>();
        }
	}
    
	public IEnumerator Choose(){

		//yield return new WaitForSeconds(clickTimer);
		loader.fillAmount = 0;
		timePast = 0;
		while (loader.fillAmount<1)
		{
			timePast += Time.deltaTime;
			loader.fillAmount = timePast / clickTimer;
			yield return new WaitForEndOfFrame();
		}
		this.PrintLog("Choose try to invoke");
		    if (PalleteMenuController.inItem && PalleteMenuController.itemConteiner == this.gameObject)
		    {
    			this.PrintLog("Choose Invoking");
    			if (paletteItemChoosen!= null)
    			    paletteItemChoosen.Invoke(this.gameObject);
		    }
		loader.fillAmount = 0;
        timePast = 0;
	}

    public void HandleOver()
    {
		this.gameObject.GetComponent<Image>().color = _enabledColor;
		loader.fillAmount = 0;
		loaderCoroutine =  StartCoroutine (Choose());
		PalleteMenuController.inItem = true;
		PalleteMenuController.itemConteiner = this.gameObject;
        over = true;
    }

    public void HandleOut()
    {
		this.gameObject.GetComponent<Image>().color = _disabledColor;
		PalleteMenuController.inItem = false;
		PalleteMenuController.itemConteiner = null;
		over = false;
		StopCoroutine(loaderCoroutine);
		loaderCoroutine = null;
		loader.fillAmount = 0;
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    }
}
