using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    #region Singletone
    [SerializeField] UnityEngine.UI.Image fadeImg;
    private static GameController _instance;
    public static GameController Instance {
        get { return _instance; }
    }
    #endregion
    public static event System.Action sceneLoaded;

    int currentSceneId = 1;
    bool loadingScene = true;
	public float fadeTimer = 1f;


    private void Awake()
    {
        if (_instance != null && _instance != this) {
            Destroy(this);
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (Application.isEditor)
            FindObjectOfType<OVRCameraRig>().enabled = false;

		fadeImg.color = new Color (fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 255f);

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            /*
             if (scene.name == "Room") {
                sceneLoaded.Invoke();
                Debug.Log("Scene loaded");
                StartCoroutine(FadeOut());
            }
            */

            sceneLoaded.Invoke();
            Debug.Log("Scene loaded");
            StartCoroutine(FadeOut());           

        };

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
	private void OnEnable()
    {
		
    }

    private void OnDisable()
    {
        
    }

    void Start() {

        //   SceneManager.LoadScene("Room", LoadSceneMode.Single);
        SceneManager.LoadScene("Room 1", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.Space))
            LoadNextScene();
    }

    public void LoadNextScene() {

        if (loadingScene == false)
        {
            currentSceneId++;
            if (currentSceneId == 4)
                currentSceneId = 1;

            StartCoroutine(FadeInAndLoad());
        }
        else
            Debug.Log("Wait for scene loading..");
    }
    

    private IEnumerator FadeOut() {

        fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1f);
        fadeImg.CrossFadeAlpha(0, fadeTimer, false);

        yield return new WaitForSeconds(fadeTimer);
        fadeImg.gameObject.SetActive(false);

        loadingScene = false;
    }

    private IEnumerator FadeInAndLoad()
    {
        loadingScene = true;

        fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0f);
        fadeImg.gameObject.SetActive(true);

        //fadeImg.CrossFadeAlpha(1, fadeTimer, false);
        while (fadeImg.color.a != 1f)
        {
            fadeImg.color = new Color(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, Mathf.MoveTowards(fadeImg.color.a, 1f, 2f * Time.deltaTime / fadeTimer ));
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(fadeTimer /2f);

        SceneManager.LoadScene("Room " + currentSceneId.ToString(), LoadSceneMode.Single);  
    }

}
