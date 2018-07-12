using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PalleteMenuController : MonoBehaviour, IClosebleObj {

#region Parrms
	[SerializeField] private CameraFollow cameraFollow;
	public bool over = false;
	public static bool inItem = false;
	public VRInteractiveItem item;
	public static GameObject itemConteiner;
	[SerializeField] private GameObject _palleteColorMenu;
	[SerializeField] private Animator _animator;

	private GameObject[] children;

	private static PalleteMenuController _instance;
	public static PalleteMenuController Instance {
		get { return _instance; }
	}

	private bool _canClose, _palleteHiden, _palletePrinted;
	#endregion

	private void Awake()
	{

		if (_instance != null && _instance != this)
			Destroy(this);
		_instance = this;
		DontDestroyOnLoad(this.gameObject);
		this.gameObject.SetActive(false);      
		children = new GameObject[this.transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = this.transform.GetChild(i).gameObject;
        }
	}

	private void OnEnable()
	{
		_canClose = false;
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
		PalleteItem.paletteItemChoosen += ChooseCategory;
		StartCoroutine(ShowPallete(true));
	}

	private void Start()
	{
	}

	private void OnDiasable()
    {
        if (item != null)
        {
            item.OnDown -= HandleDown;
            item.OnUp -= HandleUp;

            item.OnOver -= HandleOver;
            item.OnOut -= HandleOut;
        }
		PalleteItem.paletteItemChoosen -= ChooseCategory;
    }
    
    private void ChooseCategory(GameObject go)
    {
		StartCoroutine(ChooseCor(go));	
    }

	private IEnumerator ChooseCor(GameObject go){
		GameObject tempGO = null;
        this.PrintLog("In lambda");
		this.PrintLog(go.name);
        if (itemConteiner != null)
        {
			//UI_Manager.Instance.PlayAnimation(UI_Manager.HIDE_PALLETE_TAG);
			//this.gameObject.SetActive(false);

            if (_palleteColorMenu.transform.childCount > 0)
            {
                this.PrintLog("Pallete color menu !=  null");
                for (int i = 1; i < _palleteColorMenu.transform.childCount; i++)
                {
                    Destroy(_palleteColorMenu.transform.GetChild(i).gameObject);
                }
            }

            for (int i = 0; i < itemConteiner.transform.childCount; i++)
            {
                tempGO = Instantiate(go.transform.GetChild(i).gameObject, _palleteColorMenu.transform);
                tempGO.GetComponent<BoxCollider>().enabled = true;
                tempGO.GetComponent<PalleteColor>().ResizeCollider(_palleteColorMenu);
            }

            UI_Manager.Instance.PalleteCategoryManager();
            this.PrintLog("Color items must bee printed");
			Coroutine cor = StartCoroutine(CloseCor());
			this.PrintLog("Panel start closing");
			yield return cor;
			this.PrintLog("PanelMust bee closed");
		}

	}

	public void Close(){
		if (this.gameObject.activeInHierarchy && _canClose)
        {
		    StartCoroutine(CloseCor());
		}
	}

	private IEnumerator CloseCor(){
		
			Coroutine cor = StartCoroutine(ShowPallete(false));
		    
		yield return new WaitUntil(()=> _palleteHiden);
        this.gameObject.SetActive(false);
	}

	public IEnumerator ShowPallete(bool flag){

		for (int i = 0; i < children.Length; i++)
		{

			if (children[i].transform.childCount > 0){
				if (flag)
					StartCoroutine(SwitchOn(children[i]));
				else
					StartCoroutine(SwitchOff(children[i]));
			}
			yield return new WaitForFixedUpdate();
		}
		_canClose = true;

	}

	private IEnumerator SwitchOn (GameObject child){
		for (int j = 0; j < child.transform.childCount; j++)
        {
            child.transform.GetChild(j).gameObject.SetActive(true);

			for (int i = 0; i < child.transform.childCount; i++)
			{
				child.transform.GetChild(i).gameObject.GetComponent<BoxCollider>().enabled = false;
			}
			yield return new WaitForEndOfFrame();
        }

        while (child.GetComponent<CanvasGroup>().alpha < 1)
        {
            child.GetComponent<CanvasGroup>().alpha += Time.deltaTime;
			yield return new WaitForEndOfFrame();

        }
		_palleteHiden = false;
		_palletePrinted = true;
	}

	private IEnumerator SwitchOff(GameObject child){
		
		while (child.GetComponent<CanvasGroup>().alpha > 0)
        {
            child.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

		for (int j = 0; j < child.transform.childCount; j++)
        {
			child.transform.GetChild(j).gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
        }

		_palleteHiden = true;
		_palletePrinted = false;
	}

	public void HandleOver()
    {
        over = true;
		//this.PrintLog("Ray on inn");
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
}
