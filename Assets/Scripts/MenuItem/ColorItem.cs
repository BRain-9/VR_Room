using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorItem : MonoBehaviour {
    public Image fill, colorBody;
    public VRInteractiveItem item;
    public CameraFollow _cameraFollow;
    public float clickTimer = 1f;
    private bool over;
    [SerializeField] private ColorDBController _colorDBController;
    Coroutine filedCoroutine = null;

    public GameObject frame;
    public static event System.Action<GameObject> changeFrame;

    private BoxCollider boxCollider;
   
	private RectTransform rectTransform;

    private void OnEnable()
    {
        item = GetComponent<VRInteractiveItem>();
        boxCollider = GetComponent<BoxCollider>();
        rectTransform = GetComponent<RectTransform>();

        if (frame != null)
            frame.SetActive(false);

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

    void Start()
    {
       
    }

    void FixedUpdate()
    {
        boxCollider.size = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 5f);
    }

    private IEnumerator StartField() {

        while (fill.fillAmount< 1f)
        {
            fill.fillAmount += Time.deltaTime / clickTimer;
            yield return new WaitForEndOfFrame();
        }
        AudioManager.Instance.PlayClip("ui_action");
        ColorDBController.Instance.Redraw(this.colorBody.color);
        if (changeFrame != null)
            changeFrame.Invoke(frame);
        fill.fillAmount = 0;
        Debug.Log ("EndCoroutine" + gameObject.name);
        yield return null;             
    }

    public void SetParams(CameraFollow cam) {
        _cameraFollow = cam;
    }

    public void HandleOver()
    {
        filedCoroutine = StartCoroutine(StartField());
        AudioManager.Instance.PlayClip("ui_over");
		ColorMenu.inItem = true;
        if (_cameraFollow!= null)
        _cameraFollow.ry = false;
        over = true;
    }

    public void HandleOut()
    {
        
		ColorMenu.inItem = false;
        over = false;
        StopCoroutine(filedCoroutine);
        filedCoroutine = null;
        fill.fillAmount = 0f;
    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    }
}
