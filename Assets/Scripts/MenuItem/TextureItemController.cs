using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureItemController : MonoBehaviour {
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private GameObject wall; // This object take taxture

    public bool over = false;
    public Transform btnParent;
    

    public static bool inItem = false;

    public VRInteractiveItem item; 

    private static TextureItemController _instance;

    public static TextureItemController Instance
    {
        get { return _instance; }
    }

    public static float fillButtonTime = 0.1f; //  


    public Texture[] wallTextures;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        StartCoroutine(FadeIn());
    }

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

            TextureItemButton.textureBtnClk += ChangeTexture;
        }
        GameController.sceneLoaded += () => {
            wall = GameObject.Find("walls");
        };
    }

    private void OnDiasable()
    {
        if (item != null)
        {
            item.OnDown -= HandleDown;
            item.OnUp -= HandleUp;

            item.OnOver -= HandleOver;
            item.OnOut -= HandleOut;

            TextureItemButton.textureBtnClk -= ChangeTexture;
        }
    }

    public void ChangeTexture(int i) {
        float val = 0;
        switch (i)
        {
            case 0:
                val = 0.25f;
                break;
            case 1:
                val = 0.5f;
                break;
            case 2:
                val = 0.75f;
                break;
            case 3:
                val = 0.9f;
                break;
            default:
                break;
        }

            for (int j = 0; j < btnParent.childCount; j++)
            {
                btnParent.GetChild(j).GetComponent<TextureItemButton>().btnFrame.SetActive(false);
            }
            if (btnParent.childCount>0)
                btnParent.GetChild(i).GetComponent<TextureItemButton>().btnFrame.SetActive(true);
            //Material tempM = wall.GetComponent<MeshRenderer>().materials[0];
        //tempM.SetFloat("_Glossiness", val);
        //wall.GetComponent<MeshRenderer>().material = tempM;

        wall.GetComponent<MeshRenderer>().materials[0].SetFloat("_GlossMapScale", val);
    }

    public void HandleOver()
    {
        over = true;
        cameraFollow.ry = false;
    }

    public void HandleOut()
    {
        over = false;
        if (!inItem)
        {
            cameraFollow.ry = true;
        }

    }

    public void HandleDown()
    {

    }

    public void HandleUp()
    {

    }

    IEnumerator FadeIn() {

        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.alpha = 0f;

        while (cg.alpha != 1f)
        {
            cg.alpha = Mathf.MoveTowards(cg.alpha, 1f, Time.deltaTime / 2f);
            yield return new WaitForEndOfFrame();
        }

    }

}
