using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PalleteColor : MonoBehaviour
{
	#region Params
    [SerializeField] private CameraFollow cameraFollow;
	[SerializeField] private ReticleImg _reticle;
	public Image colorBody, lockImg,loaderImg;
    public bool over = false;
    public static bool inItem = false;
    public VRInteractiveItem item;
	public GridLayoutGroup _gridLayout;
	public GameObject frame;

	public float timePast = 0f;
    
	public float clickTimer = 2f;
    
	public static event System.Action<GameObject> paletteColorChoosen;

	private Coroutine unlockCoroutine, chooseCoroutin;

    #endregion

	private void Awake()
	{
		_reticle = FindObjectOfType<ReticleImg>();
        cameraFollow = GetComponentInParent<CameraFollow>();
	}

	private void OnEnable()
    {
       // this.PrintLog("CameraFollow is " + cameraFollow.gameObject.name);
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

	public void ResizeCollider(GameObject parent)
    {
        _gridLayout = parent.GetComponent<GridLayoutGroup>();
        Vector2 cellSize = _gridLayout.cellSize;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(cellSize.x, cellSize.y, -6f);
    }
    
    public void HandleOver()
    {
		    over = true;
            CategoryMenu.inItem = true;
		this.PrintLog("LogIn");
		if (CategoryMenu.ItemContainer == null)
		{
			this.PrintLog("In Choosen");
			ColorDBController.Instance.Redraw(this.colorBody.color);
			frame.SetActive(true);
			this.PrintLog("Ray Hit");
			chooseCoroutin = StartCoroutine(Choose());
		}else if (CategoryMenu.ItemContainer == this.gameObject){
			this.PrintLog("In Unlock");
			unlockCoroutine = StartCoroutine(Unlock ());
		}
    }

    public void HandleOut()
    {
        over = false;
		CategoryMenu.inItem = false;
		frame.SetActive(false);
		if (chooseCoroutin!= null)
		StopCoroutine(chooseCoroutin);
		if (unlockCoroutine != null)
		StopCoroutine(unlockCoroutine);
		loaderImg.fillAmount = 0;
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    } 

	public IEnumerator Unlock(){
		loaderImg = _reticle.transform.GetChild(0).gameObject.GetComponent<Image>();
		loaderImg.fillAmount = 0;
        timePast = 0;
		while (loaderImg.fillAmount < 1)
        {
            timePast += Time.deltaTime;
			loaderImg.fillAmount = timePast / clickTimer;
            yield return new WaitForEndOfFrame();
        }
		if (this.lockImg.gameObject.activeInHierarchy)
		{
			this.PrintLog("Unlock try to invoke");
			this.lockImg.gameObject.SetActive(false);
			if (paletteColorChoosen != null)
				paletteColorChoosen.Invoke(null);
		}
		loaderImg.fillAmount = 0;
        timePast = 0;
	}

	public IEnumerator Choose()
    {
		loaderImg = _reticle.transform.GetChild(0).gameObject.GetComponent<Image>();
		loaderImg.fillAmount = 0;
        timePast = 0;
		while (loaderImg.fillAmount < 1)
        {
            timePast += Time.deltaTime;
			loaderImg.fillAmount = timePast / clickTimer;
            yield return new WaitForEndOfFrame();
        }
		this.PrintLog("Choose try to invoke");
		if (this.frame.activeInHierarchy)
        {
            this.PrintLog("Choose Invoking");
			this.lockImg.gameObject.SetActive(true);
            
			if (paletteColorChoosen != null)
			{
				paletteColorChoosen.Invoke(this.gameObject);
			}
			loaderImg.fillAmount = 0;
            timePast = 0;
        }
        
    }
}
