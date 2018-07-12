using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonTimerAnimation : MonoBehaviour  
{
	[SerializeField] private CameraFollow cameraFollow;
	private BoxCollider bc;
	private RectTransform rectTransform;
    public Button button;
	Button.ButtonClickedEvent buttonClicked;
    public Image fill;
    public float value;
    public float timePast = 0f;
    float clickTimer = 1f;

    public bool over = false;
    
	public VRInteractiveItem item;

    private bool clicked = false;

    private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();
		bc = GetComponent<BoxCollider>();
		rectTransform = GetComponent<RectTransform>();
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

		over = false;
		timePast = 0f;
        fill.fillAmount = 0f;
    }

    void Start () {

        button = gameObject.GetComponent<Button>();
		buttonClicked = button.onClick;
        //fill = button.transform.GetChild(0).GetComponent<Image>();
      
	}

    void Update()
    {

        if (over && !clicked)
        {
            timePast += Time.deltaTime;

            if (timePast >= clickTimer)
            {
                timePast = 0f;
                fill.fillAmount = 0;
                over = false;
				buttonClicked.Invoke();
				clicked = true;
				var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerClickHandler);
				clicked = false;
            }

            fill.fillAmount = timePast / clickTimer;
        }

    }

	private void FixedUpdate()
	{
		bc.size = new Vector3 (rectTransform.sizeDelta.x,rectTransform.sizeDelta.y,5f);
	}

	public void HandleOver()
    {
        over = true;
		//cameraFollow.ry = false;
    }

    public void HandleOut()
    {
        over = false;      
        timePast = 0f;
        fill.fillAmount = 0f;
        clicked = false;
		//cameraFollow.ry = false;
    }

    public void HandleDown()
    {
        
    }

    public void HandleUp()
    {

    }
}
