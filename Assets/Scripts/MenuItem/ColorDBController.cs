using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorDBController : MonoBehaviour {
    [SerializeField]private GameObject color_item; //color item prefab
    [SerializeField]private GameObject parent, drawingItem; // instantiate color_item parent, and wall
    [SerializeField]private CameraFollow cameraFollow;
	[SerializeField] private Button _upBtn, _downBtn;

	private const string dbUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vQN1oHjuBaOgSOwbroD-pts0j_Xd8jDztHozfbDJGzK-nC0VlrtG7dfoLTbfn8shcqcFGhKHslOxiMY/pub?gid=0&single=true&output=csv";

    Color[] baseColors = new Color[4] { Color.red, Color.black, Color.green, Color.blue };

	int _maxColorCount = 21, _currentCounter = 0;
	public int CurrentCounter{
		get { return _currentCounter; }
	}
	public Dictionary<int,Dictionary<string,Vector4>> colorBlocksDict;

	public ColorMenu _colorMenu;

    #region ColorLoad
    public Dictionary<string,Vector4> ColorDictionary;
    #endregion

    public bool over = false;

    public static bool inItem = false;
         
    public VRInteractiveItem item;

    private static ColorDBController _instance;

    public static ColorDBController Instance {
        get { return _instance; }
    }

    Color lastColor = Color.white;

    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();
        ColorDictionary = new Dictionary<string, Vector4>();
		colorBlocksDict = new Dictionary<int, Dictionary <string,Vector4>>();
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

        ColorItem.changeFrame += ChangeFrame;
        GameController.sceneLoaded += () => {

            drawingItem = GameObject.Find("walls");
            Debug.Log("Walls found");

            Redraw(lastColor);
          };
		//_dataAsset.Init(this);
		VRInput.instance.OnSwipe += OVR_InputHandling;
        
    }

    private void OnDisable() {
        if (item != null)
        {
            item.OnDown -= HandleDown;
            item.OnUp -= HandleUp;

            item.OnOver -= HandleOver;
            item.OnOut -= HandleOut;
        }
		VRInput.instance.OnSwipe -= OVR_InputHandling;
    }
    
    void Start () {
        StartCoroutine (LoadDB());
	}

	private void Update()
	{
		OVR_InputHandling(VRInput.SwipeDirection.NONE);
	}
    
	private void OVR_InputHandling(VRInput.SwipeDirection direction){
		if (_colorMenu.isActiveAndEnabled)
		{
			if (direction == VRInput.SwipeDirection.UP)
			{
				UpArrow();
			}
			else if (direction == VRInput.SwipeDirection.DOWN)
			{
				DownArrow();
			}
			if (OVRInput.Get(OVRInput.Button.DpadUp))
			{
				UpArrow();
			}
			else if (OVRInput.Get(OVRInput.Button.DpadDown))
			{
				DownArrow();
			}
			else if (OVRInput.Get(OVRInput.Button.One))
			{

			}
		}
	}

	private IEnumerator LoadDB() {
		WWW www = new WWW(dbUrl);
        yield return new WaitForSeconds(3f);
        if (www.isDone)
        {
            try
            {
                var temp = CSVReader.SplitCsvLine(www.text);
                string[] lines = www.text.Split("\n"[0]);
                for (int i = 1; i < lines.Length; i++)
                {
                    string str = lines[i].Replace("\"", "");
                    string[] tempArr = str.Split(","[0]);
                    int r, g, b = 0;
                    System.Int32.TryParse(tempArr[1], out r);
                    System.Int32.TryParse(tempArr[2], out g);
                    System.Int32.TryParse(tempArr[3], out b);
                    if (ColorDictionary.ContainsKey(tempArr[0]) == false)
                        ColorDictionary.Add(tempArr[0], new Vector4(r, g, b, 255));
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                throw;
            }
        }
        else
        {
            ColorDictionary.Add("Red", Color.red);
            ColorDictionary.Add("Green", Color.green);
            ColorDictionary.Add("White", Color.white);
            ColorDictionary.Add("Blue", Color.blue);
        }

        CreateGamma(baseColors.Length);
    }



    public void Redraw(Color color) {
        Debug.Log(color);

        if (drawingItem != null)
        {
            drawingItem.GetComponent<MeshRenderer>().material.color = color;
            lastColor = color;
        }
        else
            Debug.Log("drawingItem == NUll");

    }

  /*  private void ChangeFrame(Image img) {

        Debug.Log("Frame must be changed");

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            parent.transform.GetChild(i).GetComponent<Image>().color = Color.clear;
        }

        img.color = Color.white;
    }*/

    private void ChangeFrame(GameObject targetItem)
    {

        Debug.Log("Frame must be changed");

        foreach (Transform t in parent.transform)
            t.GetComponent<ColorItem>().frame.SetActive(false);

        targetItem.SetActive(true);
    }

    public void CreateGamma(int items) {
        GameObject GO = null;
		int stepCount = 0, counter = 0;
		Dictionary<string,Vector4> tempList = new Dictionary<string, Vector4>();
        //StartCoroutine(FadeIn());
        
		foreach (Transform t in parent.transform)
            Destroy(t.gameObject);
        
		foreach (var item in ColorDictionary)
		{
			tempList.Add(item.Key, item.Value);
            stepCount++;
		    if (stepCount >_maxColorCount -1)
            {
				colorBlocksDict.Add(counter, tempList);
				tempList = new Dictionary<string, Vector4>();
				counter++;
				stepCount = 0;
            }
		}
		if (tempList.Count >0){
			colorBlocksDict.Add(counter, tempList);
		}
		FillColorMenu();
        /* This block for create colors line
         * foreach (var item in ColorDictionary)
        {
            GO = Instantiate(color_item, parent.transform);
            GO.GetComponent<ColorItem>().SetParams(cameraFollow);
            GO.GetComponent<ColorItem>().colorBody.color = new Color32 ((byte)item.Value.x, (byte)item.Value.y, (byte)item.Value.z, (byte)item.Value.w);
            GO.GetComponentInChildren<Text>().text = item.Key;
        }
        */
    }

	public void FillColorMenu(){
		GameObject GO = null;
		//List<Vector4> tempList;

		foreach (Transform t in parent.transform)
            Destroy(t.gameObject);
		
		foreach (var item in colorBlocksDict)
		{
			if (item.Key == _currentCounter)
			{
				foreach (var i in item.Value)
				{
                    GO = Instantiate(color_item, parent.transform);
                    GO.GetComponent<ColorItem>().SetParams(cameraFollow);
				    GO.GetComponent<ColorItem>().colorBody.color = 
						new Color32((byte)i.Value.x, (byte)i.Value.y, (byte)i.Value.z, (byte)i.Value.w);
					GO.GetComponentInChildren<Text>().text = i.Key;
				}

			}
		}
		BtnSwitcher();
	}

	public void UpArrow(){
		if (_currentCounter > 0)
		{
			_currentCounter--;
			FillColorMenu();
			this.PrintLog("Upp Arrow. Counter = " + _currentCounter);
		}
	}

	public void DownArrow(){
		if (_currentCounter < colorBlocksDict.Keys.Count-1){
    		_currentCounter++;
            FillColorMenu();
    		this.PrintLog("Down Arrow. Counter = " + _currentCounter);
		}
	}

	private void BtnSwitcher(){
		BoxCollider upBtnColl = _upBtn.gameObject.GetComponent<BoxCollider>();
		BoxCollider dowBtnColl = _downBtn.gameObject.GetComponent<BoxCollider>();
		if (_currentCounter == 0)
        {
			//_upBtn.interactable = false;
			_upBtn.gameObject.SetActive(false);
			upBtnColl.enabled = false;
        }
        else
        {
			//_upBtn.interactable = true;
			_upBtn.gameObject.SetActive(true);
			upBtnColl.enabled = true;
        }
		if (_currentCounter == colorBlocksDict.Keys.Count - 1)
        {
			//_downBtn.interactable = false;
			_downBtn.gameObject.SetActive(false);
			dowBtnColl.enabled = false;
        }
        else
        {
			//_downBtn.interactable = true;
			_downBtn.gameObject.SetActive(true);
			dowBtnColl.enabled = true;
        }
	}

	public void HandleOver()
    {
        //over = true;
        //cameraFollow.ry = false;
    }

    public void HandleOut()
    {
        //over = false;
        //if (!inItem) {
        //    cameraFollow.ry = true;
        //}
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {
        
    }

    public IEnumerator FadeIn()
    {
		_colorMenu.gameObject.SetActive(true);
		_colorMenu.transform.GetChild(0).gameObject.SetActive(true); // panel hidden by default
		CanvasGroup cg = _colorMenu.GetComponent<CanvasGroup>();
        cg.alpha = 0f;

        while (cg.alpha != 1f)
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, 1f, Time.deltaTime / 2f);
            yield return new WaitForEndOfFrame();
        }
    }

}
