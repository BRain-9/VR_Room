using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextureItemButton : MonoBehaviour {
    public Button button;
    public Image fill;
    public float value;
    public float timePast = 0f;
    public float clickTimer = 1f;
    public bool over = false;
    public VRInteractiveItem item;
    public GameObject btnFrame;
    private bool clicked = false;

  //  public float fillTime = 0.1f; // FieldButtonTime (recomed 0.1 - 0.3)

    Coroutine _fillCoroutine;

    public static event System.Action<int> textureBtnClk;

    private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();

        if (fill != null)
            fill.fillAmount = 0f;

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

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (over && !clicked)
        //{
        //    timePast += Time.deltaTime;

        //    if (timePast >= clickTimer)
        //    {
        //        timePast = 0f;
        //        fill.fillAmount = 0;
        //        over = false;

        //        var pointer = new PointerEventData(EventSystem.current);
        //        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerClickHandler);
        //        clicked = true;
        //    }

        //    fill.fillAmount = timePast / clickTimer;
        //}
    }

    private IEnumerator StartField()
    {
        while (fill.fillAmount < 1f) //1)
        {
            fill.fillAmount += Time.deltaTime / clickTimer;  //fillTime
            yield return new WaitForEndOfFrame();
        }
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerClickHandler);
        if (textureBtnClk!= null)
            textureBtnClk.Invoke(gameObject.transform.GetSiblingIndex());
        AudioManager.Instance.PlayClip("ui_action");

        fill.fillAmount = 0;
       // Debug.Log("EndCoroutine" + gameObject.name);
        yield return null;
    }

    public void HandleOver()
    {
        over = true;
        AudioManager.Instance.PlayClip("ui_over");
        //Debug.Log("Over" + gameObject.name);
        TextureItemController.inItem = true;
        _fillCoroutine = StartCoroutine(StartField());    }   

    public void HandleOut()
    {
        TextureItemController.inItem = false;
       // Debug.Log("Out" + gameObject.name);
        over = false;
        StopCoroutine(_fillCoroutine);
        _fillCoroutine = null;
        fill.fillAmount = 0f;
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    }
}
